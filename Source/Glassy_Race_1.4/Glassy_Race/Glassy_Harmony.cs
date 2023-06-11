using AlienRace;
using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;

namespace Glassy_Race
{

    [StaticConstructorOnStartup]
    static class Glassy_Harmony
    {
        static Glassy_Harmony()
        {
            var harmony = new Harmony("BEP.Glassy");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }

    // テイム出来るようにする
    [HarmonyPatch(typeof(TameUtility), "CanTame")]
    [HarmonyPatch(new Type[]
    {
        typeof(Pawn),
    })]
    internal class Glassy_FixCanTame
    {
        [HarmonyPrefix]
        private static bool Prefix(ref bool __result, Pawn pawn)
        {
            if (pawn.def.defName == "Glassy_Pawn")
            {
                Faction glassy_faction = FactionUtility.DefaultFactionFrom(Faction_Glassy.Glassy_WildGlassy);
                if (glassy_faction != null)
                {
                    if (pawn.Faction != null)
                    {
                        if (pawn.Faction == glassy_faction)
                        {
                            __result = true;
                            return false;
                        }
                    }
                }
            }
            return true;
        }
    }

}
