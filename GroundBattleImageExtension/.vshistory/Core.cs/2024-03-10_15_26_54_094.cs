﻿using DistantWorlds2;
using DW2TroopsGroundBattleImage.GroundImages;
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
        private const string _ModSavePrefixExtension = ".ImgMod";
        private static Dictionary<int, ImageSettings> _grImageSettings;
        private static ContentManager _contentManager;
        private static DWGame _game;
        private static string _saveGamePath;
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
        public static void LoadGroundImages()
        {
            var col = DistantWorlds.Types.Galaxy.TroopDefinitionsStatic;
            for (int i = 0; i < DistantWorlds.Types.Galaxy.TroopDefinitionsStatic.Count; i++)
            {
                string path = _NoImage;
                if (!_contentManager.Exists(_NoImage))
                {
                    path = _NoImage;
                }
                ((TroopDefinitionExt)col[i]).SetGroundImage(_contentManager.Load<Texture>(path, null));
            }
        }

        public static void LoadGroundImageSettings()
        {
            try
            { _grImageSettings = ImageSettinsManager.LoadSettings(Path.Combine(ModRootLocation, _ImageSettingsFileName)); }
            catch (Exception ex)
            {
                throw new ApplicationException($"Failed to load static ground image settings: {ex}");
            }
        }
        public static void LoadGroundImageSettingsFromStream()
        {
            try
            {
                if (File.Exists(_saveGamePath + _ModSavePrefixExtension))
                {
                    using FileStream fs = new FileStream(_saveGamePath + _ModSavePrefixExtension, FileMode.Open, FileAccess.Read, FileShare.Read);
                    _grImageSettings = ImageSettinsManager.LoadSettings(fs);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Failed to load stream ground image settings: {ex}");
            }
        }

        public static void SaveGame(string origSaveFile)
        {
            try
            {
                string modFile = origSaveFile + _ModSavePrefixExtension;
                using FileStream fs = new FileStream(modFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                fs.SetLength(0);
                var data = new ImageSettingsList();
                data.AddRange(_grImageSettings.Values.Select(x => x).ToList());
                ImageSettinsManager.SaveSettings(fs, data);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Failed to save ground image settings: {ex}");
            }
        }
        public static void SetContentManager(ContentManager manager)
        { _contentManager = manager; }

        internal static void SetDwGame(DWGame instance)
        {
            _game = instance;
        }

        internal static void SetSavepath(string filePath)
        {
            _saveGamePath = filePath;
        }
    }
}