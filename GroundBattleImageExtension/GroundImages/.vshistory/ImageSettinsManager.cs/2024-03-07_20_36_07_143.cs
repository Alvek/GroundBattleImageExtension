﻿using System;
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
        private const string _FileName = "GroundImages.txt";
        private string _folderPath;
        public ImageSettinsManager(string folderPath)
        {
            _folderPath = folderPath;
        }

        public List<ImageSettings> LoadSettings()
        {
            List<ImageSettings> res = new List<ImageSettings>();

            using FileStream fs = new FileStream(Path.Combine(_folderPath, _FileName), FileMode.Open, FileAccess.Write);
            //XmlSerializer serializer = new XmlSerializer(typeof(ImageSettings));
            //serializer.Deserialize(fs);

            //var obj = ImageSettingsList.XmlSerialization.LoadFromStream(fs);

            ImageSettingsList t = new ImageSettingsList();
            t.Add(new ImageSettings() {  GroundImageFileName = "t1", TroopDefinitonId = 6});
            t.Add(new ImageSettings());

            ImageSettingsList.XmlSerialization.WriteXml(fs, t);

            return res;
        }
    }
}