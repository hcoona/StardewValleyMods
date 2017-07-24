﻿using Harmony;
using StardewValley;
using StardewValley.Locations;

namespace FishingAdjustMod
{
    [HarmonyPatch(typeof(Town))]
    [HarmonyPatch("getFish")]
    [HarmonyPatch(new[] { typeof(float), typeof(int), typeof(int), typeof(global::StardewValley.Farmer), typeof(double) })]
    internal static class PatchTownGetFish
    {
        internal static bool Prefix(ref global::StardewValley.Object __result, int waterDepth, global::StardewValley.Farmer who)
        {
            if (Global.Config.OverrideGetFallFishKing
                && Game1.currentSeason.Equals("fall")
                && who.getTileLocation().Y < 15f
                && who.FishingLevel >= 3
                && Game1.random.NextDouble() < Global.Config.FallFishKingThreshold)
            {
                __result = new global::StardewValley.Object(160, 1, false, -1, 0);
                return false;
            }

            return true;
        }
    }
}
