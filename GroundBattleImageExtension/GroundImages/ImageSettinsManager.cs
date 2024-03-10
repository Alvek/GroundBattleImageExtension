using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace GroundBattleImageExtension.GroundImages
{
    internal class ImageSettinsManager
    {

        public static Dictionary<int, ImageSettings> LoadSettings(string path)
        {
            using FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            return ImageSettingsList.XmlSerialization.LoadFromStream(fs).ToDictionary(x=>x.TroopDefinitonId);
        }
        public static Dictionary<int, ImageSettings> LoadSettings(Stream stream)
        {
            return ImageSettingsList.XmlSerialization.LoadFromStream(stream).ToDictionary(x => x.TroopDefinitonId);
        }
        public static void SaveSettings(Stream stream, ImageSettingsList data)
        {
            ImageSettingsList.XmlSerialization.WriteXml(stream, data);
        }
    }
}
