using System.Collections.Generic;
using System.Linq;
using System;
using ServerSync;
using System.IO;
using BepInEx;
using Mineshafts.Interfaces;
using Mineshafts.Services;

namespace Mineshafts.Configuration
{
    public static class ModConfig
    {
        public static GeneralConfig General { get; set; } = new GeneralConfig();

        public static AbandonedMineshaftConfig AbandonedMineshaft { get; set; } = new AbandonedMineshaftConfig();

        public static List<PieceRecipeConfig> PieceRecipes { get; set; } = new List<PieceRecipeConfig>();

        public static LocalizationConfig Localization { get; set; } = new LocalizationConfig();

        public static List<DropConfig> drops { get; set; } = new List<DropConfig>();

        public static ConfigSync sync = new ConfigSync(Main.GUID) { DisplayName = Main.MODNAME, CurrentVersion = Main.VERSION, MinimumRequiredVersion = Main.VERSION, IsLocked = true };
        public static CustomSyncedValue<string> configString = new CustomSyncedValue<string>(ModConfig.sync, Main.MODNAME);

        public static FileSystemWatcher fsw = new FileSystemWatcher()
        {
            Path = Paths.ConfigPath,
            IncludeSubdirectories = true,
            EnableRaisingEvents = true
        };

        public static void Setup()
        {
            var tileManagerService = ServiceLocator.Get<ITileManagerService>();

            ModConfig.configString.ValueChanged += () =>
            {
                LoadConfigs();
                ModConfig.Localization.InsertLocalization();
                tileManagerService.RequestUpdateAll();
                PieceRecipes.ForEach(r => r.Apply());
                AbandonedMineshaft.Apply();
            };

            fsw.Changed += (object sender, FileSystemEventArgs e) =>
            {
                if (!sync.IsSourceOfTruth) return;

                if (Path.GetFileName(e.FullPath) == Main.configName)
                {
                    configString.Value = ModConfig.configString.Value = File.ReadAllText(e.FullPath);
                }
            };
        }

        public static void LoadConfigs()
        {
            LoadGeneralConfig();
            LoadAbandonedMineshaft();
            LoadRecipes();
            LoadLocalization();
            LoadDropConfigs();
        }

        private static void LoadGeneralConfig()
        {
            var parsed = ConfigParser.Parse(configString.Value);
            General = ConfigParser.ToObject<GeneralConfig>(parsed["general"]);
        }

        private static void LoadLocalization()
        {
            var parsed = ConfigParser.Parse(configString.Value);
            Localization = ConfigParser.ToObject<LocalizationConfig>(parsed["localization"]);
        }

        private static void LoadDropConfigs()
        {
            var parsed = ConfigParser.Parse(configString.Value);
            var dropConfigs = parsed
                .Where(pair => pair.Key.StartsWith("drop", StringComparison.OrdinalIgnoreCase))
                .Select(drop => ConfigParser.ToObject<DropConfig>(drop.Value)).ToList();
            drops = dropConfigs; 
        }

        private static void LoadRecipes()
        {
            var parsed = ConfigParser.Parse(configString.Value);
            var pieceRecs =
                parsed.Where(pair => pair.Key.StartsWith("recipe_piece", StringComparison.OrdinalIgnoreCase))
                .Select(pieceRec => ConfigParser.ToObject<PieceRecipeConfig>(pieceRec.Value)).ToList();
            PieceRecipes = pieceRecs;

            //in future add item recipe config and load them here too
        }

        private static void LoadAbandonedMineshaft()
        {
            var parsed = ConfigParser.Parse(configString.Value);
            AbandonedMineshaft = ConfigParser.ToObject<AbandonedMineshaftConfig>(parsed["abandoned_mineshaft"]);
        }

        public static List<DropConfig> GetDropsForBiome(string biome, bool includeGlobal = true)
        {
            var biomeConfigs = drops.FindAll(drops => drops.biomes.Contains(biome));
            if (includeGlobal) biomeConfigs.AddRange(drops.FindAll(vein => vein.biomes.Contains("Global")));
            return biomeConfigs;
        }
    }
}
