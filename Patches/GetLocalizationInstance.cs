using HarmonyLib;

namespace Mineshafts.Patches
{
    [HarmonyPatch(typeof(FejdStartup), nameof(FejdStartup.Start))]
    public static class GetLocalizationInstance
    {
        static void Postfix()
        {
            Main.localizationInstance = Localization.instance;
        }
    }
}
