using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using ServerSync;
using System.IO;
using BepInEx;

namespace Mineshafts.Configuration
{
    public static class ModConfig
    {
        public static ConfigSync sync = new ConfigSync(Main.GUID) { DisplayName = Main.MODNAME, CurrentVersion = Main.VERSION, MinimumRequiredVersion = Main.VERSION, IsLocked = true };
        public static CustomSyncedValue<string> configString = new CustomSyncedValue<string>(ModConfig.sync, Main.MODNAME);

        public static GeneralConfig general = new GeneralConfig();

        public static AbandonedMineshaftConfig abandonedMineshaft = new AbandonedMineshaftConfig();

        public static List<PieceRecipeConfig> pieceRecipes = new List<PieceRecipeConfig>();

        public static LocalizationConfig localization = new LocalizationConfig();

        public static List<VeinConfig> veins = new List<VeinConfig>();

        public static FileSystemWatcher fsw = new FileSystemWatcher()
        {
            Path = Paths.ConfigPath,
            IncludeSubdirectories = true,
            EnableRaisingEvents = true
        };

        public static void Setup()
        {
            ModConfig.configString.ValueChanged += () =>
            {
                LoadConfigs();
                ModConfig.localization.InsertLocalization();
                TileManager.RequestUpdateAll();
                pieceRecipes.ForEach(r => r.ApplyConfig());
                abandonedMineshaft.Apply();
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
            LoadVeinConfigs();
        }

        private static void LoadGeneralConfig()
        {
            var parsed = ConfigParser.Parse(configString.Value);
            general = ConfigParser.ToObject<GeneralConfig>(parsed["general"]);
        }

        private static void LoadLocalization()
        {
            var parsed = ConfigParser.Parse(configString.Value);
            localization = ConfigParser.ToObject<LocalizationConfig>(parsed["localization"]);
        }

        private static void LoadVeinConfigs()
        {
            var parsed = ConfigParser.Parse(configString.Value);
            var veinConfigs = parsed
                .Where(pair => pair.Key.StartsWith("vein", StringComparison.OrdinalIgnoreCase))
                .Select(vein => ConfigParser.ToObject<VeinConfig>(vein.Value)).ToList();
            veins = veinConfigs; 
        }

        private static void LoadRecipes()
        {
            var parsed = ConfigParser.Parse(configString.Value);
            var pieceRecs =
                parsed.Where(pair => pair.Key.StartsWith("recipe_piece", StringComparison.OrdinalIgnoreCase))
                .Select(pieceRec => ConfigParser.ToObject<PieceRecipeConfig>(pieceRec.Value)).ToList();
            pieceRecipes = pieceRecs;

            //in future add item recipe config and load them here too
        }

        private static void LoadAbandonedMineshaft()
        {
            var parsed = ConfigParser.Parse(configString.Value);
            abandonedMineshaft = ConfigParser.ToObject<AbandonedMineshaftConfig>(parsed["abandoned_mineshaft"]);
        }

        public static List<VeinConfig> GetVeinConfigsForBiome(string biome, bool includeGlobal = true)
        {
            var biomeConfigs = veins.FindAll(vein => vein.biomes.Contains(biome));
            if (includeGlobal) biomeConfigs.AddRange(veins.FindAll(vein => vein.biomes.Contains("Global")));
            return biomeConfigs;
        }

        public static VeinConfig GetVeinConfigForPosition(Vector3 pos)
        {
            var alignedPos = Util.ConvertVector3ToGridAligned(pos);

            var isVein = Util.GetRandomNumberForPosition(alignedPos, 0, 100) <= general.vein_chance;
            if (!isVein) return null;

            var possibleVeins = GetVeinConfigsForBiome(WorldGenerator.instance.GetBiome(alignedPos).ToString());
            if (possibleVeins.Count == 0) return null;

            var weightSum = 0;
            possibleVeins.ForEach(vein => weightSum += vein.weight);

            var rolledChance = Util.GetRandomNumberForPosition(alignedPos * 2, 0, weightSum);

            foreach (VeinConfig vein in possibleVeins)
            {
                if (rolledChance <= vein.weight) return vein;
                rolledChance -= vein.weight;
            }
            return null;
        }
    }
}
