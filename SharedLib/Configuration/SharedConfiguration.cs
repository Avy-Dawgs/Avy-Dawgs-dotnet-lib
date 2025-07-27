using System.Xml.Serialization;

namespace Configuration;

public class SharedConfiguration
{
    
    [XmlElement("BluetoothServerConfiguration")]
    public required BluetoothConfiguration BluetoothConfiguration { get; set; } 
    
}