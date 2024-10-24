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
        public const string AUTHOR = "GoldenJude: UpdatedBy TM";
        public const string GUID = AUTHOR + "_" + MODNAME;
        public const string VERSION = "1.0.8";

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

            log.LogInfo($"Checking for config files in: {Paths.ConfigPath}");

            if (TryLoadConfigFromFile(out var configString))
            {
                ModConfig.configString.Value = configString;
            }
            else if (TryLoadDefaultConfigFromEmbeddedResource(assembly, out configString))
            {
                ModConfig.configString.Value = configString;
                SaveConfigToFile(configString);
            }
            else
            {
                log.LogError("DefaultConfig.cfg not found in embedded resources.");
            }
        }

        private bool TryLoadConfigFromFile(out string configString)
        {
            var configSearch = Directory.GetFiles(Paths.ConfigPath, configName, SearchOption.AllDirectories);
            if (configSearch.Any())
            {
                configString = File.ReadAllText(configSearch.First());
                return true;
            }

            configString = null;
            return false;
        }

        private bool TryLoadDefaultConfigFromEmbeddedResource(Assembly assembly, out string configString)
        {
            var resourceNames = assembly.GetManifestResourceNames();
            log.LogInfo("Embedded resource names:");
            foreach (var resourceName in resourceNames)
            {
                log.LogInfo(resourceName);
            }

            var configFileName = resourceNames.FirstOrDefault(str => str.EndsWith("DefaultConfig.cfg"));
            if (configFileName != null)
            {
                log.LogInfo($"Found embedded resource: {configFileName}");
                using var streamReader = new StreamReader(assembly.GetManifestResourceStream(configFileName));
                configString = streamReader.ReadToEnd();
                log.LogInfo("Read default config from embedded resource.");
                return true;
            }

            configString = null;
            return false;
        }

        private void SaveConfigToFile(string configString)
        {
            var configFilePath = Path.Combine(Paths.ConfigPath, configName);
            File.WriteAllText(configFilePath, configString);
            log.LogInfo($"Default config written to: {configFilePath}");
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
