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
                if (il[i].opcode == OpCodes.Ldc_R4 && il[i].operand.ToString() == "6000")
                    il[i].operand = 9000f;

            return il.AsEnumerable();

            /*foreach (CodeInstruction i in instructions)
            {
                if (i.opcode == OpCodes.Ldc_R4 && i.operand.ToString() == "6000") i.operand = 8000f;

                Main.log.LogWarning(i.opcode.Name + " - " + i.opcode.Value);
                Main.log.LogWarning(i.operand?.ToString());
            }

            return instructions;*/
        }
    }
}
