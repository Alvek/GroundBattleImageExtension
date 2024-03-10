using HarmonyLib;
using System.IO;
using System.Reflection;
using System;
using DistantWorlds.Types;

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

            DistantWorlds.Types.TroopDefinitionList.XmlSerializer = new XmlSerializationHelper<TroopDefinitionList>();
        }
    }

    [HarmonyDebug]
    [HarmonyPatch(typeof())]
    [HarmonyPatch(new Type[] {  })]
    public class NotificationFilterPatcher
    {
        public static void Postfix(ref bool __result, EmpireMessage message)
        {
        }
    }

}
