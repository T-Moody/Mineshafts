using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;
using Mineshafts.Configuration;
using System.IO;
using System.Linq;
using Mineshafts.Services;
using Mineshafts.Interfaces;

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

        public static int roomTheme = 1024;

        public static string configName = GUID + "_1.2" + ".cfg";

        public static Localization localizationInstance;

        public Main()
        {
            var assetService = new AssetService();
            var damageService = new DamageService();
            var gridService = new GridService();
            var randomService = new RandomService(gridService);
            var tileService = new TileService(gridService);
            var tileManagerService = new TileManagerService(tileService);

            ServiceLocator.Register<IAssetService>(assetService);
            ServiceLocator.Register<IRandomService>(randomService);
            ServiceLocator.Register<IDamageService>(damageService);
            ServiceLocator.Register<IGridService>(gridService);
            ServiceLocator.Register<ITileService>(tileService);
            ServiceLocator.Register<ITileManagerService>(tileManagerService);
        }

        void Awake()
        {
            log = Logger;

            var assembly = Assembly.GetExecutingAssembly();
            new Harmony(GUID).PatchAll(assembly);

            ModConfig.Setup();

            var configSearch = Directory.GetFiles(Paths.ConfigPath, configName, SearchOption.AllDirectories);
            if (configSearch.Any())
            {
                ModConfig.configString.Value = File.ReadAllText(configSearch.First());
            }
            else
            {
                var configFileName = assembly.GetManifestResourceNames().Single(str => str.EndsWith("DefaultConfig.cfg"));
                using var streamReader = new StreamReader(assembly.GetManifestResourceStream(configFileName));

                var configString = streamReader.ReadToEnd();
                ModConfig.configString.Value = configString;
                File.WriteAllText(Path.Combine(Paths.ConfigPath, configName), configString);
            }
        }

        void FixedUpdate()
        {
            var tileManagerService = ServiceLocator.Get<ITileManagerService>();
            tileManagerService.UpdateRequests();
        }

        void OnDestroy()
        {
            this.StopAllCoroutines();
        }
    }
}
