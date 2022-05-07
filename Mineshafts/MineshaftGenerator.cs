using Mineshafts.Configuration;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/*
namespace Mineshafts
{
    public static class MineshaftGenerator
    {
        public static int floors = 1;
        public static int halls = 10;

        public static void Generate(Vector3 pos)
        {
            

            Main.log.LogWarning("generating bruhgeon has been called");

            if(ZNetScene.instance == null)
            {
                Main.log.LogWarning("bruh....");
                return;
            }

            var biome = WorldGenerator.instance.GetBiome(pos);
            var possibleCfgs = ModConfig.GetAbandonedMineshaftConfigForBiome(biome.ToString());
            var cfg = possibleCfgs[(int)Util.GetRandomNumberForPosition(pos, 0, possibleCfgs.Count)];

            //loop levels
            //per level - loop tunnel count
            //per tunnel - loop tunnel length

            var previousPositions = new List<Vector3>();
            previousPositions.Add(pos);
            for (int level = 0; level < cfg.levels; level++)
            {
                for (int tunnelI = 0; tunnelI < cfg.tunnels; tunnelI++)
                {
                    GenerateTunnel(previousPositions[Util.get])
                }

                //add shaft down and add it to previous positions
                currentDirection = RandomizeDirection(previousPositions.Last());
                currentPos = previousPositions[(int)Util.GetRandomNumberForPosition(previousPositions.Last(), 0, previousPositions.Count - 1)];
                previousPositions.Clear();
            }

            for (int i = 0; i < 100; i++)
            {
                var targetPos = new Vector3(pos.x + i * Main.gridSize, Util.GetInitialHeight(), pos.z);
                previousPositions.Add(targetPos);
                TileManager.RequestPlacement(targetPos);
            }

            TileManager.RequestUpdateAll();
        }

        private static void GenerateTunnel(int length, List<Vector3> previousPositions)
        {
            var currentPos = startingPos;

            for (int i = 0; i < length; i++)
            {
                var nextPos = new Vector3(
                    currentPos.x + direction.x * Main.gridMaxHeight,
                    currentPos.y,
                    currentPos.z + direction.z * Main.gridMaxHeight);
                if (previousPositions.Find(p => p == nextPos) == null)
                {
                    TileManager.RequestPlacement(nextPos);
                }
                currentPos = nextPos;
                previousPositions.Add(nextPos);
            }
        }

        private static Vector3 RandomizeDirection(Vector3 pos)
        {
            var roll = Util.GetRandomNumberForPosition(pos, 0, 3);
            return roll switch
            {
                0 => Vector3.forward,
                1 => Vector3.right,
                2 => Vector3.back,
                3 => Vector3.left,
                _ => Vector3.forward
            };
        }
    }
}
*/