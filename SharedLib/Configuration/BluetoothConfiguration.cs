using System.Xml.Serialization;

namespace Configuration;

[XmlRoot("BluetoothServerConfiguration")]
public class BluetoothConfiguration
{
    [XmlAttribute("name")]
    public required string ServerName { get; set; }

    [XmlAttribute("uuid")]
    public required string ServerUuid { get; set; }

    [XmlAttribute("address")]
    public required string ServerAddress { get; set; }
    
    [XmlAttribute("script")]
    public required string BluetoothScript { get; set; }
}