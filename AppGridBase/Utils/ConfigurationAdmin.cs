using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace AppGrid.Utils
{
    public class ConfigurationAdmin
    {
        public static AppGrid.Configuration ConfigurationLoader(string XmlPath)
        {
            AppGrid.Configuration conf;
            XmlSerializer serl = new XmlSerializer(typeof(AppGrid.Configuration));
            using (Stream str = File.OpenRead(XmlPath))
            {
                conf = (AppGrid.Configuration)serl.Deserialize(str);
                str.Close();
            }
            return conf;
        }
        public static bool ConfigurationSave(AppGrid.Configuration configuration, string XmlPath)
        {
            XmlSerializer serl = new XmlSerializer(typeof(AppGrid.Configuration));
            try
            {
                using (Stream str=File.OpenWrite(XmlPath))
                {
                    serl.Serialize(str, configuration);
                    str.Close();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
