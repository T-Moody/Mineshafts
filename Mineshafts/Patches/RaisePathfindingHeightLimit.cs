using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace Mineshafts.Patches
{
    [HarmonyPatch(typeof(Pathfinding), nameof(Pathfinding.BuildTile))]
    public static class RaisePathfindingHeightLimit
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var il = instructions.ToList();

            for (int i = 0; i < il.Count; ++i)
                if (il[i].opcode == OpCodes.Ldc_R4 && string.Equals(il[i].operand.ToString(), "6000", System.StringComparison.Ordinal))
                    il[i].operand = 10000f;

            return il.AsEnumerable();
        }
    }
}