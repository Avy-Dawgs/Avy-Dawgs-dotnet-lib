using System.Xml.Serialization;

namespace Configuration;

/// <summary>
/// Represents configuration for a bluetooth server. Can be used on
/// client or server side.
/// </summary>
[XmlRoot("BluetoothServerConfiguration")]
public class BluetoothServerConfiguration
{
    [XmlAttribute("name")]
    public required string Name { get; set; }

    [XmlAttribute("uuid")]
    public required string Uuid { get; set; }

    [XmlAttribute("address")]
    public required string Address { get; set; }
    
    [XmlAttribute("script")]
    public required string ScriptCommand { get; set; }
}