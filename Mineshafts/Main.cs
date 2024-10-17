﻿using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;
using Mineshafts.Configuration;
using System.IO;
using System.Linq;

namespace Mineshafts
{
    [BepInPlugin(GUID, MODNAME, VERSION)]
    public class Main : BaseUnityPlugin
    {
        public const string MODNAME = "Mineshafts";
        public const string AUTHOR = "GoldenJude";
        public const string GUID = AUTHOR + "_" + MODNAME;
        public const string VERSION = "1.0.7";

        public static ManualLogSource log;

        public static int gridSize = 3;
        public static int gridMaxHeight = Util.RoundToNearestGridPoint(7500); //7500
        public static int gridMinHeight = Util.RoundToNearestGridPoint(7000); //6999, 7251 center

        public static int roomTheme = 1024;

        public static string assetBundleName = "mineshafts";
        public static string configName = GUID + "_1.2" + ".cfg";

        public static Localization localizationInstance;

        void Awake()
        {
            log = Logger;

            var assembly = Assembly.GetExecutingAssembly();
            new Harmony(GUID).PatchAll(assembly);

            ModConfig.Setup();

            var configSearch = Directory.GetFiles(Paths.ConfigPath, configName, SearchOption.AllDirectories);
            if(configSearch.Any())
            {
                ModConfig.configString.Value = File.ReadAllText(configSearch.First());
            }
            else
            {
                //using var memStream = new MemoryStream(Properties.Resources.DefaultConfig);
                var configFileName = assembly.GetManifestResourceNames().Single(str => str.EndsWith("DefaultConfig.cfg"));
                using var streamReader = new StreamReader(assembly.GetManifestResourceStream(configFileName));

                var configString = streamReader.ReadToEnd();
                ModConfig.configString.Value = configString;
                File.WriteAllText(Path.Combine(Paths.ConfigPath, configName), configString);
            }
        }

        void FixedUpdate()
        {
            TileManager.UpdateRequests();
        }

        void OnDestroy()
        {
            this.StopAllCoroutines();
        }
    }
}
