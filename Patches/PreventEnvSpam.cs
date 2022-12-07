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
            if (string.Equals(msg, "Setting forced environment MS_mine", System.StringComparison.Ordinal) ||
                string.Equals(msg, "Setting forced environment ", System.StringComparison.Ordinal)) 
                return false;
            return true;
        }
    }
}