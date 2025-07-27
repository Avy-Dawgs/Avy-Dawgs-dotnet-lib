using Configuration;

namespace Bluetooth;

public class BluetoothServer : BluetoothCommunication
{
    public BluetoothServer(
        BluetoothConfiguration configuration, 
        string serverCommand) : base(configuration)
    {

    }
}