using System.Xml.Serialization;

namespace Configuration;

/// <summary>
/// Represents configuration shared between flight computer and base station.
/// </summary>
[XmlRoot("SharedConfiguration")]
public class SharedConfiguration
{
    
    [XmlElement("BluetoothServerConfiguration")]
    public required BluetoothServerConfiguration BluetoothServerConfiguration { get; set; } 
    
}