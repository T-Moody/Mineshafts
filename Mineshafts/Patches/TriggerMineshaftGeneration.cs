using HarmonyLib;
using Mineshafts.Components;
using UnityEngine;

namespace Mineshafts.Patches
{
    [HarmonyPatch(typeof(ZoneSystem), nameof(ZoneSystem.SpawnLocation))]
    public static class TriggerMineshaftGeneration
    {
        private static void Postfix(ZoneSystem.ZoneLocation location, Vector3 pos, ZoneSystem.SpawnMode mode)
        {
            if(!ZoneSystem.instance.IsZoneGenerated(ZoneSystem.instance.GetZone(pos)) &&
                ZNet.instance.IsServer() &&
                location.m_prefabName.StartsWith("MS_AbandonedMine", System.StringComparison.Ordinal))
            {
                //MineshaftGenerator.Generate(pos);
            }

            /*Main.log.LogWarning(location.m_prefabName + " " + mode.ToString());

            if(mode == ZoneSystem.SpawnMode.Full && location.m_prefabName.StartsWith("MS_AbandonedMine", System.StringComparison.Ordinal))
            {
                
            }*/
        }

        /*private static readonly MethodInfo GenerateDungeons = AccessTools.Method(typeof(TriggerMineshaftGeneration), nameof(TriggerMineshaftGeneration.Bruh));

        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator ilGen)
        {
            var originalIl = instructions.ToList();

            //return instructions;

            var ifLabel = ilGen.DefineLabel();

            //originalIl.First().labels.Add(firstOriginalInstructionLabel);

            var loadSpawnMode = new CodeInstruction(OpCodes.Ldarg, 4);
            var loadOne = new CodeInstruction(OpCodes.Ldc_I4_0);
            var evaluateSpawnMode = new CodeInstruction(OpCodes.Bne_Un, ifLabel);
            //if spawnmode == 0 (full spawn)
            var loadPosition = new CodeInstruction(OpCodes.Ldarg, 3);
            var callGeneration = new CodeInstruction(OpCodes.Call, GenerateDungeons);
            var nop = new CodeInstruction(OpCodes.Nop);
            nop.labels.Add(ifLabel);

            var newIl = new List<CodeInstruction>()
            {
                loadSpawnMode,
                loadOne,
                evaluateSpawnMode,
                //loadPosition,
                callGeneration,
                nop
            };
            newIl.AddRange(originalIl);

            //newIl[5].labels.Add(firstOriginalInstructionLabel);

            return newIl.AsEnumerable();
        }*/
    }
}