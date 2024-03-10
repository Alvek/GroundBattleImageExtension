﻿using DW2TroopsGroundBattleImage.GroundImages;
using Stride.Core.Serialization.Contents;
using Stride.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DW2TroopsGroundBattleImage
{
    internal class Core
    {
        private const string _ImageSettingsFileName = "GroundImages.xml";
        private const string _NoImage = "UserInterface/Placeholder";
        private static Dictionary<int, ImageSettings> _grImageSettings;
        public static string ModRootLocation { get; set; }
        

        public static void ReplaceOriginalTroopDefinitionInstanses()
        {
            var col = DistantWorlds.Types.Galaxy.TroopDefinitionsStatic;
            for (int i = 0; i < DistantWorlds.Types.Galaxy.TroopDefinitionsStatic.Count; i++)
            {
                var extItem = new TroopDefinitionExt(col[i]);
                col[i] = extItem;
            }
        }
        public static void LoadGroundImages(ContentManager assets)
        {
            var col = DistantWorlds.Types.Galaxy.TroopDefinitionsStatic;
            for (int i = 0; i < DistantWorlds.Types.Galaxy.TroopDefinitionsStatic.Count; i++)
            {
                string path = _NoImage;
                if (!assets.Exists(_NoImage))
                {
                    path = _NoImage;
                }
                ((TroopDefinitionExt)col[i]).SetGroundImage(assets.Load<Texture>(path, null));
            }
        }

        public static void LoadGroundImageSettings()
        {
            _grImageSettings = ImageSettinsManager.LoadSettings(Path.Combine(ModRootLocation, _ImageSettingsFileName));
        }
        public static void LoadGroundImageSettingsFromStream()
        {
        }
        public static void SaveGame(string path)
        { 
            сохранить мои настройки в стрим
        }
    }
}