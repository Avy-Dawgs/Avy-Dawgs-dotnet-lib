using Configuration;

namespace Bluetooth;

/// <summary>
/// Bluetooth server.
/// </summary>
public class BluetoothServer : BluetoothCommunication
{
    /// <summary>
    /// Constructor. 
    /// </summary>
    /// <param name="serverConfiguration"></param>
    public BluetoothServer(
        BluetoothServerConfiguration serverConfiguration) : base(serverConfiguration)
    {

    }
}