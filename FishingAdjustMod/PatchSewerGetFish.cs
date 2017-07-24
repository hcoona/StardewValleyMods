﻿using Harmony;
using StardewValley;
using StardewValley.Locations;

namespace FishingAdjustMod
{
    [HarmonyPatch(typeof(Sewer))]
    [HarmonyPatch("getFish")]
    [HarmonyPatch(new[] { typeof(float), typeof(int), typeof(int), typeof(global::StardewValley.Farmer), typeof(double) })]
    internal static class PatchSewerGetFish
    {
        internal static bool Prefix(ref global::StardewValley.Object __result, int waterDepth, global::StardewValley.Farmer who)
        {
            if (Global.Config.OverrideGetSewerFishKing
                && (Game1.random.NextDouble() < Global.Config.SewerFishKingThreshold + ((who.getTileX() > 14 && who.getTileY() > 42) ? 0.08 : 0.0)))
            {
                __result = new global::StardewValley.Object(682, 1, false, -1, 0);
                return false;
            }

            return true;
        }
    }
}
