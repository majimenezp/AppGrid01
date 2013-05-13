using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppGrid
{
    [Serializable]
    public class Configuration
    {
        [System.Xml.Serialization.XmlElement("HttpPort", IsNullable = false)]
        public int HttpPort { get; set; }
        [System.Xml.Serialization.XmlElement("Name", IsNullable = false)]
        public string Name { get; set; }
        [System.Xml.Serialization.XmlElement("Host", IsNullable = false)]
        public string Host { get; set; }

        [System.Xml.Serialization.XmlElement("HttpPortConfig", IsNullable = true)]
        public Nullable<HttpPortConfigurationOptions> HttpPortConfig { get; set; }

        [System.Xml.Serialization.XmlArrayItem("Variables", IsNullable = true)]
        public ConfigurationVariable[] Variables { get; set; }

        public Configuration()
        {
            HttpPortConfig = HttpPortConfigurationOptions.IncrementallyFromAssigned;
        }
    }
    [Serializable]
    public class ConfigurationVariable
    {
        [System.Xml.Serialization.XmlAttribute("Name")]
        public string Name{get;set;}
        [System.Xml.Serialization.XmlAttribute("Value")]
        public string Value{get;set;}
    }
    [Serializable]
    public enum HttpPortConfigurationOptions
    {
        IncrementallyFromAssigned=1,
        NextGeneralOpenPort=2
    }
}
