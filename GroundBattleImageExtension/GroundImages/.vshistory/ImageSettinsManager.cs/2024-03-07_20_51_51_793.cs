using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace DW2TroopsGroundBattleImage.GroundImages
{
    internal class ImageSettinsManager
    {

        public static Dictionary<int, ImageSettings> LoadSettings(string path)
        {
            using FileStream fs = new FileStream(Path.Combine(_folderPath, _FileName), FileMode.Open, FileAccess.Read);
            //XmlSerializer serializer = new XmlSerializer(typeof(ImageSettings));
            //serializer.Deserialize(fs);

            var obj = ImageSettingsList.XmlSerialization.LoadFromStream(fs);
            return obj.ToDictionary(x=>x.TroopDefinitonId);
        }
    }
}
