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

    [HarmonyPatch(typeof(PlayerData), "AddListenerForVIPAmountChanged")]
    public static class PlayerData_AddListenerForVIPAmountChanged
    {
        public static void Prefix(PlayerData __instance)
        {
            PWPlus.playerData = __instance;
        }
    }

    [HarmonyPatch(typeof(Player), "Awake")]
    public static class Player_Awake
    {
        public static void Prefix(Player __instance)
        {
            if (__instance.myPlayerData == PWPlus.playerData)
            {
                PWPlus.player = __instance;
            }
        }
    }
}
