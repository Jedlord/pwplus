using System;
using System.Collections.Generic;
using HarmonyLib;

namespace PWPlus
{
    [HarmonyPatch(typeof(World), "GenerateLayersFromBSON")]
    public static class World_GenerateLayersFromBSON
    {
        public static void Prefix(World __instance)
        {
            PWPlus.world = __instance;
        }
    }
}
