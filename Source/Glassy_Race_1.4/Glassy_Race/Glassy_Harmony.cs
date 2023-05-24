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

}
