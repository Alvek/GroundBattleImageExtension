using DistantWorlds2;
using DW2TroopsGroundBattleImage.GroundImages;
using HarmonyLib;
using Stride.Core.Assets;
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
        //private static ContentManager _contentManager;
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
            var troopDef = DistantWorlds.Types.Galaxy.TroopDefinitionsStatic;
            foreach (var item in troopDef)
            {
                string path;
                if (_grImageSettings.ContainsKey(item.TroopDefinitionId) && troopDef.ContainsId(item.TroopDefinitionId))
                {
                    path = _grImageSettings[item.TroopDefinitionId]?.GroundImageFileName;
                    if (path != null && assets.Exists(path))
                    {
                        ((TroopDefinitionExt)troopDef[item.TroopDefinitionId]).SetGroundImage(assets.Load<Texture>(path, null));
                    }
                }
            }
        }
        public static void LoadGroundImages()
        {
            var troopDef = DistantWorlds.Types.Galaxy.TroopDefinitionsStatic;
            foreach (var item in troopDef)
            {
                string path;
                if (_grImageSettings.ContainsKey(item.TroopDefinitionId) && troopDef.ContainsId(item.TroopDefinitionId))
                {
                    path = _grImageSettings[item.TroopDefinitionId]?.GroundImageFileName;
                    if (path != null && _game.Content.Exists(path))
                    {
                        ((TroopDefinitionExt)troopDef[item.TroopDefinitionId]).SetGroundImage(_game.Content.Load<Texture>(path, null));
                    }
                }
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
                string modFile = "data" + _saveGamePath + _ModSavePrefixExtension;
                if (File.Exists(modFile))
                {
                    using FileStream fs = new FileStream(modFile, FileMode.Open, FileAccess.Read, FileShare.Read);
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

                string modFile = Path.GetFullPath(Path.Combine("data", origSaveFile + _ModSavePrefixExtension));
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
