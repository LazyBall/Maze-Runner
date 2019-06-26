using System.IO;
using System.Xml.Serialization;
using System.Configuration;

namespace Maze_Runner
{
    public static class Serializer
    {
        public static void SerializeSettings(Settings settings)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(Settings));
            using (var fs = new FileStream(ConfigurationManager.AppSettings.Get("Settings"),
                FileMode.Create))
            {
                formatter.Serialize(fs, settings);
            }
        }

        public static Settings DeserializeSettings()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(Settings));
            Settings settings;
            try
            {
                using (FileStream fs = new FileStream(ConfigurationManager.AppSettings.Get("Settings"), 
                    FileMode.Open))
                {
                    settings = (Settings)formatter.Deserialize(fs);
                }
            }
            catch
            {
                settings = new Settings();
            }
            return settings;
        }

        public static void SerializeResults(Results results)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(Results));
            using (var fs = new FileStream(ConfigurationManager.AppSettings.Get("Results"), 
                FileMode.Create))
            {
                formatter.Serialize(fs, results);
            }
        }

        public static Results DeserializeResults()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(Results));
            Results results;
            try
            {
                using (FileStream fs = new FileStream(ConfigurationManager.AppSettings.Get("Results"), 
                    FileMode.Open))
                {
                    results = (Results)formatter.Deserialize(fs);
                }
            }
            catch
            {
                results = new Results();
            }
            return results;
        }
    }
}