﻿using HarmonyLib;
using System.IO;
using System.Reflection;
using System;
using DistantWorlds.Types;
using Stride.Core.Serialization.Contents;
using static System.Net.Mime.MediaTypeNames;
using Stride.Graphics;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Linq;
using Microsoft.VisualBasic;
using System.Runtime.CompilerServices;

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

            Core.ModRootLocation = Assembly.GetCallingAssembly().Location;
        }
    }

    [HarmonyDebug]
    [HarmonyPatch(typeof(DistantWorlds.Types.Galaxy))]
    [HarmonyPatch(nameof(DistantWorlds.Types.Galaxy.LoadStaticBaseData))]
    public class GalaxyTroopDefinitionPatcher1
    {
        public static void Postfix()
        {
            Core.ReplaceOriginalTroopDefinitionInstanses();
        }
    }

    [HarmonyDebug]
    [HarmonyPatch(typeof(DistantWorlds.Types.Galaxy))]
    [HarmonyPatch(nameof(DistantWorlds.Types.Galaxy.CopyGalaxyInstanceToStaticBaseData))]
    public class GalaxyTroopDefinitionPatcher2
    {
        public static void Postfix()
        {
            Core.ReplaceOriginalTroopDefinitionInstanses();
        }
    }

    [HarmonyDebug]
    [HarmonyPatch(typeof(DistantWorlds.Types.Galaxy))]
    [HarmonyPatch(nameof(DistantWorlds.Types.Galaxy.LoadImageForAllItemsStatic))]
    public class GalaxyLoadImagePatcher
    {
        public static void Postfix(ContentManager assets)
        {
            Core.LoadGroundImages(assets);
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
            Troop troopClass = new Troop(null);
            TroopDefinitionExt troopDefClass = new TroopDefinitionExt(new TroopDefinition());

            var codes = new List<CodeInstruction>(instructions);
            for (var i = 0; i < codes.Count; i++)
            {
                if (codes[i].Calls(GetMethodInfo(troopClass.GetImage)))
                {

                    codes[i] = CodeInstruction.Call(typeof(Troop), troopClass.GetType().GetProperty(nameof(troopClass.Definition)).GetGetMethod().Name, null, null);
                    //codes[i] = CodeInstruction.Call(typeof(Troop), nameof(DistantWorlds.Types.Troop.Definition), null, null);
                    //codes.Insert(i + 1, new CodeInstruction(OpCodes.Isinst, typeof(TroopDefinitionExt)));
                    codes.Insert(i + 1, new CodeInstruction(OpCodes.Callvirt, GetMethodInfo(troopDefClass.GetGroundImage)));

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
    }
}
