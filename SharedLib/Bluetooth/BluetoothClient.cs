using Configuration;

namespace Bluetooth;

public class BluetoothClient : BluetoothCommunication
{
    public BluetoothClient(
        BluetoothConfiguration configuration,
        string clientScriptCommand) : base(configuration)
    {
        
    }
}