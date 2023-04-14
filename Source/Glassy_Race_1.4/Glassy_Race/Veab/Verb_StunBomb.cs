using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Glassy_Race
{
    public class Verb_StunBomb : Verb
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
				if (pawn != null)
                {
					List<Pawn> press = comp.parent.MapHeld.mapPawns.AllPawnsSpawned.Where(x => x.Position.DistanceTo(pawn.Position) <= 10.9f && x.HostileTo(pawn)).ToList();
					for (int j = press.Count() - 1; j >= 0; j--)
					{
						press.ElementAt(j).TakeDamage(new DamageInfo(DamageDefOf.Stun, 30, 3.0f, -1, pawn));
						press.ElementAt(j).TakeDamage(new DamageInfo(DamageDefOf.EMP, 30, 3.0f, -1, pawn));
					}
					GenExplosion.DoExplosion(pawn.Position, pawn.Map, 10.9f, DamageDefOf.Smoke, null, -1, -1f, null, null, null, null, null, 0f, 1, GasType.BlindSmoke);
					comp.UsedOnce();
				}
			}
		}
	}
}
