using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI.Group;

namespace Glassy_Race
{
    class IncidentWorker_GlassyPass : IncidentWorker
	{
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			if (map.gameConditionManager.ConditionIsActive(GameConditionDefOf.ToxicFallout))
			{
				return false;
			}
			if (!map.mapTemperature.SeasonAndOutdoorTemperatureAcceptableFor(ThingDef.Named("Glassy_Pawn")))
			{
				return false;
			}
			IntVec3 cell;
			return TryFindEntryCell(map, out cell);
		}

		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			if (!TryFindEntryCell(map, out IntVec3 cell))
			{
				return false;
			}
			if (!TryFindFormerFaction(out Faction formerFaction))
			{
				return false;
			}
			List<Pawn> pawns = new List<Pawn>();
			PawnKindDef Resurreviewy = PawnKindDef.Named("Glassy_Visitor");
			Pawn pawn = PawnGenerator.GeneratePawn(Resurreviewy, Find.FactionManager.FirstFactionOfDef(Faction_Glassy.Glassy_WildGlassy));
			pawn.kindDef = PawnKindDefOf.WildMan;
			GenSpawn.Spawn(pawn, cell, map);
			pawns.Add(pawn);
			RCellFinder.TryFindRandomSpotJustOutsideColony(pawns[0], out IntVec3 result);
			LordMaker.MakeNewLord(Find.FactionManager.FirstFactionOfDef(Faction_Glassy.Glassy_WildGlassy), new LordJob_VisitColony(Find.FactionManager.FirstFactionOfDef(Faction_Glassy.Glassy_WildGlassy), result, 60000), map, pawns);
			SendStandardLetter("Glassy.Incident.LetterLabelGlassyPass".Translate().CapitalizeFirst(), "Glassy.Incident.LetterGlassyPass".Translate(), LetterDefOf.NeutralEvent, parms, pawn);
			return true;
		}

		private bool TryFindEntryCell(Map map, out IntVec3 cell)
		{
			return CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => map.reachability.CanReachColony(c), map, CellFinder.EdgeRoadChance_Ignore, out cell);
		}

		private bool TryFindFormerFaction(out Faction formerFaction)
		{
			return Find.FactionManager.TryGetRandomNonColonyHumanlikeFaction(out formerFaction, tryMedievalOrBetter: false, allowDefeated: true);
		}
	}
}
