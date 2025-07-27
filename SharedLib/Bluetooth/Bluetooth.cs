using System.Diagnostics;
using Configuration;

namespace Bluetooth;

public enum BluetoothConnectionState
{
    Connected, 
    Disconnected,
}

public abstract class BluetoothCommunication : IDisposable
{
    private const string ConnectedTag = "[CONNECTED]";
    private const string DisconnectedTag = "[DISCONNECTED]";
    
    public event EventHandler<string>? DataReceived;
    public event EventHandler<BluetoothConnectionState>? ConnectionStateChanged;
    
    private readonly Process _bluetoothScript;
    private readonly CancellationTokenSource _cts;
    
    private Task? _receiveTask;
    
    private BluetoothConnectionState _connectionState;
    private BluetoothConnectionState ConnectionState
    {
        get => _connectionState;
        set
        {
            if (value != _connectionState)
            {
                _connectionState = value;
                ConnectionStateChanged?.Invoke(this, value);
            }
        }
    }

    private protected BluetoothCommunication(BluetoothConfiguration configuration)
    {
        _connectionState = BluetoothConnectionState.Disconnected;
        
        _bluetoothScript = new();
        _bluetoothScript.StartInfo.Arguments = configuration.BluetoothScript + " " +
                                              configuration.ServerName + " " + 
                                              configuration.ServerUuid + " " +
                                              configuration.ServerAddress; 
        
        _bluetoothScript.StartInfo.RedirectStandardInput = true;
        _bluetoothScript.StartInfo.RedirectStandardOutput = true;
        
        _bluetoothScript.StandardInput.AutoFlush = true;

        _cts = new();
    }

    public void Start()
    {
        _bluetoothScript.Start();
        _receiveTask = Task.Run(() => ReceiveLoop(_cts.Token));
    }

    private void ReceiveLoop(CancellationToken token)
    {
        while (true)
        {
            if (token.IsCancellationRequested)
            {
                break;
            }
            
            string newData = _bluetoothScript.StandardOutput.ReadToEnd();

            if (!newData.Contains("\n"))
            {
                continue;
            }
            
            // find tags
            int disConTagIdx = newData.IndexOf(DisconnectedTag);
            if (disConTagIdx != -1)
            {
                ConnectionState = BluetoothConnectionState.Disconnected;
                newData = newData.Remove(disConTagIdx, DisconnectedTag.Length);
            }

            int conTagIdx = newData.IndexOf(ConnectedTag);
            if (conTagIdx != -1)
            {
                ConnectionState = BluetoothConnectionState.Connected;
                newData = newData.Remove(conTagIdx, ConnectedTag.Length);
            }

            DataReceived?.Invoke(this, newData);
        }
    }

    
    public void Send(byte[] data)
    {
        if (ConnectionState == BluetoothConnectionState.Disconnected)
        {
            throw new Exception("Disconnected, cannot send.");
        }

        _bluetoothScript.StandardInput.Write(data);
    }

    public void Dispose()
    {
        _cts.Cancel();
        _receiveTask?.Wait();
        
        _bluetoothScript.Kill();
        _bluetoothScript.Dispose();
    }
}