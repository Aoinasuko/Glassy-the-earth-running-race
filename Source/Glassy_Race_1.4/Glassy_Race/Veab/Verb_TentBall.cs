using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using static UnityEngine.GraphicsBuffer;
using Verse.Noise;

namespace Glassy_Race
{
    public class Verb_TentBall : Verb
	{
		protected override bool TryCastShot()
		{
			Pop(base.ReloadableCompSource);
			return true;
		}

		public override float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
		{
			needLOSToCenter = false;
			return this.verbProps.range;
		}

		public override void DrawHighlight(LocalTargetInfo target)
		{
			DrawHighlightFieldRadiusAroundTarget(caster);
		}

		public static void Pop(CompReloadable comp)
		{
			if (comp != null && comp.CanBeUsed)
			{
				Pawn pawn = comp.Wearer;
				if (pawn != null && pawn.Map != null)
                {
                    // ポジションチェック
                    CellRect rect = new CellRect(pawn.Position.x - 4, pawn.Position.z - 4, 9, 9);
                    Boolean flag = false;
                    foreach (IntVec3 item in rect)
                    {
                        // 範囲外の場合怒られが発生する
                        if (item.x < 0 || item.z < 0 || item.x >= pawn.Map.Size.x || item.z >= pawn.Map.Size.z)
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (flag)
                    {
                        Messages.Message("Glassy.UI.CantTent".Translate(), MessageTypeDefOf.RejectInput, historical: false);
                        return;
                    } else
                    {
                        GenExplosion.DoExplosion(pawn.Position, pawn.Map, 7.9f, DamageDefOf.Smoke, null, -1, -1f, null, null, null, null, null, 0f, 1, GasType.BlindSmoke);
                        // 自身の周囲7*7に部屋展開
                        // 床                    
                        foreach (IntVec3 item in rect)
                        {
                            Find.CurrentMap.terrainGrid.SetTerrain(item, TerrainDef.Named("CarpetWhite"));
                        }
                        // 壁(上)
                        CellRect rect2 = new CellRect(pawn.Position.x - 4, pawn.Position.z - 4, 9, 1);
                        foreach (IntVec3 item2 in rect2)
                        {
                            Thing thing = ThingMaker.MakeThing(ThingDefOf.Wall, ThingDefOf.WoodLog);
                            thing.SetFaction(Faction.OfPlayer);
                            GenSpawn.Spawn(thing, item2, pawn.Map);
                        }
                        // 壁(右)
                        CellRect rect3 = new CellRect(pawn.Position.x + 4, pawn.Position.z - 4, 1, 9);
                        foreach (IntVec3 item3 in rect3)
                        {
                            Thing thing = ThingMaker.MakeThing(ThingDefOf.Wall, ThingDefOf.WoodLog);
                            thing.SetFaction(Faction.OfPlayer);
                            GenSpawn.Spawn(thing, item3, pawn.Map);
                        }
                        // 壁(下)
                        CellRect rect4 = new CellRect(pawn.Position.x - 4, pawn.Position.z + 4, 9, 1);
                        foreach (IntVec3 item4 in rect4)
                        {
                            Thing thing = ThingMaker.MakeThing(ThingDefOf.Wall, ThingDefOf.WoodLog);
                            thing.SetFaction(Faction.OfPlayer);
                            GenSpawn.Spawn(thing, item4, pawn.Map);
                        }
                        // 壁(左)
                        CellRect rect5 = new CellRect(pawn.Position.x - 4, pawn.Position.z - 4, 1, 9);
                        foreach (IntVec3 item5 in rect5)
                        {
                            Thing thing = ThingMaker.MakeThing(ThingDefOf.Wall, ThingDefOf.WoodLog);
                            thing.SetFaction(Faction.OfPlayer);
                            GenSpawn.Spawn(thing, item5, pawn.Map);
                        }
                        // ドア(左)
                        Thing door_L = ThingMaker.MakeThing(ThingDefOf.Door, ThingDefOf.WoodLog);
                        door_L.SetFaction(Faction.OfPlayer);
                        GenSpawn.Spawn(door_L, new IntVec3(pawn.Position.x - 4, pawn.Position.y, pawn.Position.z), pawn.Map);
                        // ドア(右)
                        Thing door_R = ThingMaker.MakeThing(ThingDefOf.Door, ThingDefOf.WoodLog);
                        door_R.SetFaction(Faction.OfPlayer);
                        GenSpawn.Spawn(door_R, new IntVec3(pawn.Position.x + 4, pawn.Position.y, pawn.Position.z), pawn.Map);
                        // ベッド設置
                        Thing Bed_A = ThingMaker.MakeThing(ThingDefOf.Bed, ThingDefOf.WoodLog);
                        Bed_A.SetFaction(Faction.OfPlayer);
                        Bed_A.TryGetComp<CompQuality>().SetQuality(QualityCategory.Masterwork, ArtGenerationContext.Outsider);
                        GenSpawn.Spawn(Bed_A, new IntVec3(pawn.Position.x - 2, pawn.Position.y, pawn.Position.z - 3), pawn.Map);
                        Thing Bed_B = ThingMaker.MakeThing(ThingDefOf.Bed, ThingDefOf.WoodLog);
                        Bed_B.SetFaction(Faction.OfPlayer);
                        Bed_B.TryGetComp<CompQuality>().SetQuality(QualityCategory.Masterwork, ArtGenerationContext.Outsider);
                        GenSpawn.Spawn(Bed_B, new IntVec3(pawn.Position.x + 2, pawn.Position.y, pawn.Position.z - 3), pawn.Map);
                        comp.UsedOnce();
                        Thing Bed_C = ThingMaker.MakeThing(ThingDefOf.Bed, ThingDefOf.WoodLog);
                        Bed_C.SetFaction(Faction.OfPlayer);
                        Bed_C.TryGetComp<CompQuality>().SetQuality(QualityCategory.Masterwork, ArtGenerationContext.Outsider);
                        GenSpawn.Spawn(Bed_C, new IntVec3(pawn.Position.x - 2, pawn.Position.y, pawn.Position.z + 2), pawn.Map);
                        Thing Bed_D = ThingMaker.MakeThing(ThingDefOf.Bed, ThingDefOf.WoodLog);
                        Bed_D.SetFaction(Faction.OfPlayer);
                        Bed_D.TryGetComp<CompQuality>().SetQuality(QualityCategory.Masterwork, ArtGenerationContext.Outsider);
                        GenSpawn.Spawn(Bed_D, new IntVec3(pawn.Position.x + 2, pawn.Position.y, pawn.Position.z + 2), pawn.Map);
                        // グラシィーのエアコン
                        Thing AC = ThingMaker.MakeThing(ThingDef.Named("Glassy_AC"));
                        AC.SetFaction(Faction.OfPlayer);
                        GenSpawn.Spawn(AC, new IntVec3(pawn.Position.x, pawn.Position.y, pawn.Position.z + 1), pawn.Map);
                        // グラシィーの圧縮食料
                        Thing Pemi = ThingMaker.MakeThing(ThingDef.Named("Glassy_CompressionFood"));
                        Pemi.stackCount = 20;
                        GenSpawn.Spawn(Pemi, new IntVec3(pawn.Position.x, pawn.Position.y, pawn.Position.z - 1), pawn.Map);

                        // 最後に屋根設置
                        foreach (IntVec3 item in rect)
                        {
                            Find.CurrentMap.roofGrid.SetRoof(item, RoofDefOf.RoofConstructed);
                        }
                        comp.UsedOnce();
                    }
                }
			}
		}
	}
}
