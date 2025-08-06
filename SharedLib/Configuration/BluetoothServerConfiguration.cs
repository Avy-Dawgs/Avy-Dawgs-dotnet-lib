using System.Xml.Serialization;

namespace Configuration;

/// <summary>
/// Represents configuration for a bluetooth server. Can be used on
/// client or server side.
/// </summary>
[XmlRoot("BluetoothServerConfiguration")]
public class BluetoothServerConfiguration
{
    [XmlElement("Name")]
    public required string Name { get; set; }

    [XmlElement("Uuid")]
    public required string Uuid { get; set; }

    [XmlElement("Address")]
    public required string Address { get; set; }
    
    [XmlElement("ScriptPath")]
    public required string ScriptPath { get; set; }
    
    [XmlElement("InterpreterPath")]
    public required string InterpreterPath { get; set; }
}