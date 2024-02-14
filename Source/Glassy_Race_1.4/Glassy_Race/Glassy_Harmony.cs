using AlienRace;
using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
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

    // 見つかりにくさ軽減
    [HarmonyPatch(typeof(CaravanVisibilityCalculator), "Visibility")]
    [HarmonyPatch(new Type[]
    {
            typeof(List<Pawn>),
            typeof(bool),
            typeof(StringBuilder),
    })]
    static class Patch_CaravanVisibilityCalculator
    {
        [HarmonyPostfix]
        public static void Postfix(ref float __result, List<Pawn> pawns, bool caravanMovingNow, StringBuilder explanation = null)
        {
            bool finished_A = ResearchProjectDef.Named("Glassy_CaravanSkill1").IsFinished;
            float bonus = 1.0f;
            if (finished_A)
            {
                bonus = 0.7f;
                bool finished_B = ResearchProjectDef.Named("Glassy_CaravanSkill2").IsFinished;
                if (finished_B)
                {
                    bonus = 0.5f;
                    bool finished_C = ResearchProjectDef.Named("Glassy_CaravanSkill3").IsFinished;
                    if (finished_C)
                    {
                        bonus = 0.3f;
                    }
                }
            }
            if (bonus < 1.0f)
            {
                __result *= bonus;
                if (explanation != null)
                {
                    explanation.AppendLine();
                    explanation.Append("Glassy.UI.CaravanBonus".Translate() + ": " + bonus.ToStringPercent());
                }
            }            
            return;
        }
    }

    /// <summary>
    /// キャラバン重量アップ
    /// </summary>
    [HarmonyPatch(typeof(MassUtility), "Capacity")]
    [HarmonyPatch(new Type[]
    {
        typeof(Pawn),
        typeof(StringBuilder)
    })]
    [HarmonyPatch(typeof(MassUtility), "Capacity")]
    internal class Patch_Capacity
    {
        private static void Postfix(ref float __result, Pawn p, StringBuilder explanation)
        {
            bool finished_A = ResearchProjectDef.Named("Glassy_CaravanSkill1").IsFinished;
            float bonus = 1.0f;
            if (finished_A)
            {
                bonus = 1.5f;
                bool finished_B = ResearchProjectDef.Named("Glassy_CaravanSkill2").IsFinished;
                if (finished_B)
                {
                    bonus = 2.0f;
                    bool finished_C = ResearchProjectDef.Named("Glassy_CaravanSkill3").IsFinished;
                    if (finished_C)
                    {
                        bonus = 3.0f;
                    }
                }
                __result *= bonus;
                if (explanation != null)
                {
                    if (explanation.Length > 0)
                    {
                        explanation.AppendLine();
                    }
                    explanation.Append("  - " + "Glassy.UI.CaravanBonus".Translate() + ": " + bonus.ToStringPercent());
                }
            }
        }
    }

}
