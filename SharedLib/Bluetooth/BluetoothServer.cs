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
    /// <param name="configuration"></param>
    public BluetoothServer(
        BluetoothConfiguration configuration) : base(configuration)
    {

    }
}