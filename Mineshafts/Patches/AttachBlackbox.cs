using HarmonyLib;
using Mineshafts.Components;
using System.IO;
using UnityEngine;

namespace Mineshafts.Patches
{
    [HarmonyPatch(typeof(Player), nameof(Player.Start))]
    static class AttachBlackbox
    {
        private static void Postfix(Player __instance)
        {
            if (Player.m_localPlayer == __instance)
            {
                using var memStream = new MemoryStream(Properties.Resources.mineshafts);
                var bundle = AssetBundle.LoadFromStream(memStream);
                var box = bundle.LoadAsset<GameObject>("MS_blackbox");
                var boxComponent = __instance.gameObject.AddComponent<PlayerBlackbox>();
                boxComponent.box = GameObject.Instantiate(box, __instance.transform);
                bundle.Unload(false);
            }
        }
    }
}
