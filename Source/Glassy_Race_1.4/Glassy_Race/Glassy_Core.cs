using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Glassy_Race
{

    [DefOf]
    public static class Faction_Glassy
    {
        public static FactionDef Glassy_WildGlassy;
    }

    [DefOf]
    public static class FleshType_Glassy
    {
        public static FleshTypeDef Glassy;
    }

    [StaticConstructorOnStartup]
    public static class Graphic_Glassy
    {
        public static readonly Texture2D Acceleration_Icon = ContentFinder<Texture2D>.Get("Glassy/UI/Acceleration");
    }

}
