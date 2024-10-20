using HarmonyLib;
using UnityEngine;

namespace Mineshafts.Patches
{
    [HarmonyPatch(typeof(EnvMan), nameof(EnvMan.Awake))]
    public static class AddEnv
    {
        public static void Postfix()
        {
            var env = EnvMan.instance.m_environments.Find(x => x.m_name == "Crypt").Clone();
            env.m_name = "MS_mine";

            ColorUtility.TryParseHtmlString("#333333", out var envColor);

            env.m_isColdAtNight = true;
            env.m_isCold = true;

            ////bruh.....
            //env.m_ambColorDay = envColor;
            //env.m_ambColorNight = envColor;
            //env.m_fogColorDay = envColor;
            //env.m_fogColorEvening = envColor;
            //env.m_fogColorMorning = envColor;
            //env.m_fogColorNight = envColor;
            //env.m_fogColorSunDay = envColor;
            //env.m_fogColorSunEvening = envColor;
            //env.m_fogColorSunNight = envColor;

            //env.m_sunColorDay = envColor;
            //env.m_sunColorEvening = envColor;
            //env.m_sunColorMorning = envColor;
            //env.m_sunColorNight = envColor;

            EnvMan.instance.m_environments.Add(env);
        }
    }
}
