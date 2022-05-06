using UnityEngine;

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

            for (int i = 0; i < 100; i++)
            {
                var targetPos = new Vector3(pos.x + i * Main.gridSize, Util.GetInitialHeight(), pos.z);
                TileManager.RequestPlacement(targetPos);
            }

            TileManager.RequestUpdateAll();
        }
    }
}
