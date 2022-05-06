using HarmonyLib;

namespace Mineshafts.Patches
{
    [HarmonyPatch(typeof(ZLog), nameof(ZLog.Log))]
    public static class PreventEnvSpam
    {
        //removes the EnvMan log spam cause by moving between mine tiles
        public static bool Prefix(object o)
        {
            var msg = o.ToString();
            if (msg == "Setting forced environment MS_mine" || msg == "Setting forced environment ") return false;
            return true;
        }
    }
}