using HarmonyLib;

namespace Mineshafts.Patches
{
    [HarmonyPatch(typeof(FejdStartup), nameof(FejdStartup.Start))]
    public static class GetLocalizationInstancePatch
    {
        static void Postfix()
        {
            Main.localizationInstance = Localization.instance;
        }
    }
}
