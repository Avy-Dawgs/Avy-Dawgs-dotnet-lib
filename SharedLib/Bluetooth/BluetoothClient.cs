using Configuration;

namespace Bluetooth;

/// <summary>
/// Bluetooth client.
/// Meant for computer side application
/// </summary>
public class BluetoothClient : BluetoothCommunication
{
    /// <summary>
    /// Constructor. 
    /// </summary>
    /// <param name="serverConfiguration"></param>
    public BluetoothClient(
        BluetoothServerConfiguration serverConfiguration) : base(serverConfiguration)
    {
        
    }
}