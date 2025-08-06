using System.Diagnostics;
using Configuration;

namespace Bluetooth;

public enum BluetoothConnectionState
{
    Connected, 
    Disconnected,
}

/// <summary>
/// Abstract class representing a bluetooth connection.
/// Relies on a script which uses standard input and output as the means of communicating.
/// </summary>
public abstract class BluetoothCommunication : IDisposable
{
    private const string ConnectedTag = "[CONNECTED]";
    private const string DisconnectedTag = "[DISCONNECTED]";
    
    /// <summary>
    /// New data has been received.
    /// </summary>
    public event EventHandler<string>? DataReceived;
    
    /// <summary>
    /// The connection state has changed.
    /// </summary>
    public event EventHandler<BluetoothConnectionState>? ConnectionStateChanged;
    
    private readonly Process _bluetoothScript;
    private readonly CancellationTokenSource _cts;
    private Task? _receiveTask;
    
    private BluetoothConnectionState _connectionState;
    /// <summary>
    /// Current connection state. Setting sets off event if state
    /// has changed.
    /// </summary>
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

    /// <summary>
    /// Constructor taking server configuration.
    /// </summary>
    /// <param name="configuration">server configuration</param>
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

    /// <summary>
    /// Start the connection.
    /// </summary>
    public void Start()
    {
        _bluetoothScript.Start();
        _receiveTask = Task.Run(() => ReceiveLoop(_cts.Token));
    }

    /// <summary>
    /// Main loop for receiving data.
    /// </summary>
    /// <param name="token"></param>
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
            
            // find connection state tags, remove from stream.
            int tagIdx = newData.IndexOf(DisconnectedTag);
            if (tagIdx != -1)
            {
                ConnectionState = BluetoothConnectionState.Disconnected;
                newData = newData.Remove(tagIdx, DisconnectedTag.Length);
            }
            tagIdx = newData.IndexOf(ConnectedTag);
            if (tagIdx != -1)
            {
                ConnectionState = BluetoothConnectionState.Connected;
                newData = newData.Remove(tagIdx, ConnectedTag.Length);
            }

            DataReceived?.Invoke(this, newData);
        }
    }

    /// <summary>
    /// Send data.
    /// </summary>
    /// <param name="data"></param>
    /// <exception cref="Exception">disconnected, cannot send</exception>
    public void Send(byte[] data)
    {
        if (ConnectionState == BluetoothConnectionState.Disconnected)
        {
            throw new Exception("Disconnected, cannot send.");
        }

        _bluetoothScript.StandardInput.Write(data);
    }

    /// <summary>
    /// Dispose of this object by killing the underlying script.
    /// </summary>
    public void Dispose()
    {
        _cts.Cancel();
        _receiveTask?.Wait();
        
        _bluetoothScript.Kill();
        _bluetoothScript.Dispose();
    }
}