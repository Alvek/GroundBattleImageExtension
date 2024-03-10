using HarmonyLib;
using System.IO;
using System.Reflection;
using System;
using DistantWorlds.Types;
using Stride.Core.Serialization.Contents;
using static System.Net.Mime.MediaTypeNames;
using Stride.Graphics;

namespace DW2TroopsGroundBattleImage
{
    public class Preloader
    {
        public static void Init()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            HarmonyPatcher.Init();
        }
        private static System.Reflection.Assembly? CurrentDomain_AssemblyResolve(object? sender, ResolveEventArgs args)
        {
            AssemblyName name = new AssemblyName(args.Name);
            if (name.Name == "0Harmony")
            {
                var asmLoc = typeof(HarmonyPatcher).Assembly.Location;
                var asmDir = Path.GetDirectoryName(asmLoc);
                var harmonyDll = Path.Join(asmDir, "0Harmony.dll");
                return Assembly.LoadFrom(harmonyDll);
            }
            return null;
        }
    }
    public class HarmonyPatcher
    {
        public static void Init()
        {
            //Logger.ChannelFilter = Logger.LogChannel.All;
            //Harmony.DEBUG = true;
            //FileLog.Reset();
            var harmony = new Harmony("DW2.TroopsGroundBattleImage");
            harmony.PatchAll();

            //FileLog.Log($"{DateTime.Now} patch done");
            //FileLog.FlushBuffer();
        }
    }

    [HarmonyDebug]
    [HarmonyPatch(typeof(DistantWorlds.Types.Galaxy))]
    [HarmonyPatch(nameof(DistantWorlds.Types.Galaxy.LoadStaticBaseData))]
    public class GalaxyTroopDefinitionPatcher
    {
        public static void Postfix()
        {
            var col = DistantWorlds.Types.Galaxy.TroopDefinitionsStatic;
            for (int i = 0; i < DistantWorlds.Types.Galaxy.TroopDefinitionsStatic.Count; i++) 
            {
                var extItem = new TroopDefinitionExt(col[i]);
                col[i] = extItem;
            }
        }
    }
    [HarmonyDebug]
    [HarmonyPatch(typeof(DistantWorlds.Types.Galaxy))]
    [HarmonyPatch(nameof(DistantWorlds.Types.Galaxy.LoadImagesForItems))]
    public class GalaxyLoadImagePatcher
    {
        private const string _NoImage = "UserInterface/Placeholder";
        public static void Postfix(ContentManager assets)
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
    }

    class test
    {
        public test()
        {
            Troop test = new Troop(null);
            var img = (test.Definition as TroopDefinitionExt).GetGroundImage();
        }
    }
}
