using HarmonyLib;
using Mineshafts.Components;
using System.Collections.Generic;
using System.Linq;

//removes floor hits if sidewalls are hit to reduce pickaxe jank

namespace Mineshafts.Patches
{
    [HarmonyPatch(typeof(Attack), nameof(Attack.AddHitPoint))]
    public static class MineWallHitAdjustmentPatch
    {
        public static void Postfix(List<Attack.HitPoint> list)
        {
            var sideWallNames = new List<string>() { "northern", "eastern", "southern", "western" };
            var floorName = "floor";
            var ceilingName = "ceiling";
            var allWallsNames = sideWallNames.Concat(new string[] { floorName, ceilingName });

            var hitWalls = list.FindAll(hit => allWallsNames.Contains(hit.go?.name));
            /*
            var ceilingHit = hitWalls.Find(hit => hit.go?.name == ceilingName);
            if(ceilingName != null)
            {
                hitWalls.Remove(ceilingHit);
                list.RemoveAll(hit => hitWalls.Contains(hit));
                return;
            }
            */
            var sideWallHits = list.FindAll(hit => sideWallNames.Contains(hit.go?.name));


            var floorHit = hitWalls.Find(hit => hit.go?.name == floorName);


            if(sideWallHits.Count > 0 && floorHit != null)
            {
                hitWalls.RemoveAll(hit => sideWallNames.Contains(hit.go?.name));
                list.RemoveAll(hit => hitWalls.Contains(hit));
            }
        }
    }
}
