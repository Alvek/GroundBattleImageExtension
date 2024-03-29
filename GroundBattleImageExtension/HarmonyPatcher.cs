﻿using HarmonyLib;
using System.IO;
using System.Reflection;
using System;
using DistantWorlds.Types;
using Stride.Core.Serialization.Contents;
using Stride.Graphics;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Linq;
using System.Runtime.CompilerServices;
using DistantWorlds2;
using Stride.Core.IO;
using Stride.Core;
using Stride.Core.Assets;
using Stride.Core.Storage;



public static class Mod
{
    public static void Init()
    {
        GroundBattleImageExtension.Preloader.Init();
    }
}

namespace GroundBattleImageExtension
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
            Core.ModRootLocation = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);

            //Logger.ChannelFilter = Logger.LogChannel.All;
            //Harmony.DEBUG = true;
            //FileLog.Reset();
            var harmony = new Harmony("DW2.TroopsGroundBattleImage");
            harmony.PatchAll();

            //FileLog.Log($"{DateTime.Now} patch done");
            //FileLog.FlushBuffer();

        }
    }

    //[HarmonyDebug]
    //[HarmonyPatch(typeof(DistantWorlds.Types.Galaxy))]
    //[HarmonyPatch(nameof(DistantWorlds.Types.Galaxy.LoadStaticBaseData))]
    //public class GalaxyTroopDefinitionPatcher1
    //{
    //    public static void Postfix()
    //    {
    //        Core.ReplaceOriginalTroopDefinitionInstanses();
    //        Core.LoadGroundImages();
    //    }
    //}

    [HarmonyDebug]
    [HarmonyPatch(typeof(DistantWorlds.Types.Galaxy))]
    [HarmonyPatch(nameof(DistantWorlds.Types.Galaxy.CopyGalaxyInstanceToStaticBaseData))]
    public class GalaxyTroopDefinitionPatcher2
    {
        public static void Postfix()
        {
            Core.ReplaceOriginalTroopDefinitionInstanses();
            Core.LoadGroundImageSettingsFromStream();
            Core.LoadGroundImages();
        }
    }

    [HarmonyDebug]
    [HarmonyPatch(typeof(DistantWorlds.Types.Galaxy))]
    [HarmonyPatch(nameof(DistantWorlds.Types.Galaxy.LoadImageForAllItemsStatic))]
    public class GalaxyLoadImagePatcher
    {
        public static void Postfix(ContentManager assets)
        {
            Core.LoadGroundImageSettings();
            Core.ReplaceOriginalTroopDefinitionInstanses();
            Core.LoadGroundImages(assets);
        }
    }

    [HarmonyDebug]
    [HarmonyPatch(typeof(DistantWorlds2.DWGame))]
    [HarmonyPatch(nameof(DistantWorlds2.DWGame.SaveGame))]
    public class DwGameSave
    {
        public static void Postfix(string filePath)
        {
            Core.SaveGame(filePath);
        }
    }

    [HarmonyDebug]
    [HarmonyPatch(typeof(DistantWorlds2.DWGame))]
    [HarmonyPatch(nameof(DistantWorlds2.DWGame.LoadGame))]
    public class DwGameLoad
    {
        public static void Prefix(string filePath)
        {
            Core.SetSavepath(filePath);
            Core.ReplaceOriginalTroopDefinitionInstanses();
            Core.LoadGroundImageSettingsFromStream();
            Core.LoadGroundImages();
        }
    }

    [HarmonyDebug]
    [HarmonyPatch(typeof(DistantWorlds.Types.UserInterfaceHelper))]
    //[HarmonyPatch(nameof(DistantWorlds.Types.UserInterfaceHelper.DrawTroopColumn))]
    public class GroundTroopImagePatcher
    {
        /// <summary>
        /// Looks for methods we need to patch. Could be removed if right HarmonyPatch attribute values are added.
        /// </summary>
        /// <returns></returns>
        [HarmonyTargetMethods]
        static IEnumerable<MethodBase> TargetMethods()
        {
            var res = AccessTools.GetTypesFromAssembly(typeof(DistantWorlds.Types.UserInterfaceHelper).Assembly)
                .Select(type => type.GetMethod("DrawTroopColumn", BindingFlags.NonPublic | BindingFlags.Static))
                .Where(method => method != null)
                .Cast<MethodBase>();
            return res;
        }
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            //Troop troopClass = new Troop(null);
            //TroopDefinitionExt troopDefClass = new TroopDefinitionExt(new TroopDefinition());

            var codes = new List<CodeInstruction>(instructions);
            for (var i = 0; i < codes.Count; i++)
            {
                if (codes[i].Calls(GetMethodInfo(typeof(Troop), nameof(Troop.GetImage))))
                {
                    codes[i] = CodeInstruction.Call(typeof(Troop), typeof(Troop).GetProperty(nameof(Troop.Definition)).GetGetMethod().Name, null, null);
                    //codes[i] = CodeInstruction.Call(typeof(Troop), nameof(DistantWorlds.Types.Troop.Definition), null, null);
                    //codes.Insert(i + 1, new CodeInstruction(OpCodes.Isinst, typeof(TroopDefinitionExt)));
                    codes.Insert(i + 1, new CodeInstruction(OpCodes.Callvirt, GetMethodInfo(typeof(TroopDefinitionExt), nameof(TroopDefinitionExt.GetGroundImage))));

                    //codes[i] = CodeInstruction.Call(typeof(Troop), troopClass.GetType().GetProperty(nameof(troopClass.Definition)).GetGetMethod().Name, null, null);
                    ////codes[i] = CodeInstruction.Call(typeof(Troop), nameof(DistantWorlds.Types.Troop.Definition), null, null);
                    //codes.Insert(i + 1, new CodeInstruction(OpCodes.Isinst, typeof(TroopDefinitionExt)));
                    //codes.Insert(i + 2, new CodeInstruction(OpCodes.Callvirt, GetMethodInfo(troopDefClass.GetGroundImage)));
                    break;
                }
            }

            return codes.AsEnumerable();
        }
        static MethodInfo GetMethodInfo(Func<Texture> method)
        {
            return method.Method;
        }
        static MethodInfo GetMethodInfo(Type type, string name)
        {
            return type.GetMethod(name);
        }
    }

    [HarmonyDebug]
    [HarmonyPatch(typeof(DistantWorlds2.DWGame))]
    [HarmonyPatch("Initialize")]
    public class DWGamePatcher
    {
        public static void Prefix(DWGame __instance)
        {
            Core.SetDwGame(__instance);
        }
    }
}
