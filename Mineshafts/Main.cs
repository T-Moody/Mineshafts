using BepInEx;
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
        public const string VERSION = "1.0.0";

        public static ManualLogSource log;

        public static int gridSize = 3;
        public static int gridMaxHeight = Util.RoundToNearestGridPoint(8000);
        public static int gridMinHeight = Util.RoundToNearestGridPoint(7000);

        public static string assetBundleName = "mineshafts";
        public static string configName = "Mineshafts.cfg";

        public static Localization localizationInstance;
        public static Main instance;

        void Awake()
        {
            instance = this;

            new Harmony(GUID).PatchAll(Assembly.GetExecutingAssembly());
            log = Logger;

            ModConfig.Setup();

            var configSearch = Directory.GetFiles(Paths.ConfigPath, configName, SearchOption.AllDirectories);
            if(configSearch.Any())
            {
                ModConfig.configString.Value = File.ReadAllText(configSearch.First());
            }
            else
            {
                using var memStream = new MemoryStream(Properties.Resources.DefaultConfig);
                using var streamReader = new StreamReader(memStream);

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
