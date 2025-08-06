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
    /// <param name="configuration"></param>
    public BluetoothClient(
        BluetoothConfiguration configuration) : base(configuration)
    {
        
    }
}