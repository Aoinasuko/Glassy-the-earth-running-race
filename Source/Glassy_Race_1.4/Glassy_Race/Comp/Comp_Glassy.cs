using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Glassy_Race
{
	public class CompProperties_Glassy : CompProperties
	{
		public CompProperties_Glassy()
		{
			compClass = typeof(Comp_Glassy);
		}
	}

	public class Comp_Glassy : ThingComp
	{
        public override void CompTick()
        {
			Pawn comppawn = (Pawn)this.parent;
			if (comppawn.Downed)
            {
				if (comppawn.IsHashIntervalTick(10))
				{
					if (comppawn.health.hediffSet.HasHediff(HediffDef.Named("Glassy_Acceleration")))
					{
						comppawn.health.RemoveHediff(comppawn.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("Glassy_Acceleration")));
					}
				}
			}
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			Pawn comppawn = (Pawn)this.parent;
			if ((comppawn.Faction?.IsPlayer ?? false) == false)
			{
				yield break;
			} else
            {
				if (Find.Selector.SelectedPawns.Contains(comppawn))
				{
					Gizmo Gizmo = new Command_Toggle
					{
						defaultLabel = "Glassy.UI.label_Acceleration".Translate(),
						icon = Graphic_Glassy.Acceleration_Icon,
						defaultDesc = "Glassy.UI.desc_Acceleration".Translate(),
						disabled = comppawn.Downed,
						disabledReason = "Glassy.UI.disabled_Acceleration".Translate(),
						isActive = delegate
						{
							return comppawn.health.hediffSet.HasHediff(HediffDef.Named("Glassy_Acceleration"));
						},
						toggleAction = delegate
						{
							if (!comppawn.health.hediffSet.HasHediff(HediffDef.Named("Glassy_Acceleration")))
                            {
								comppawn.health.AddHediff(HediffDef.Named("Glassy_Acceleration"));
							} else
                            {
								comppawn.health.RemoveHediff(comppawn.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("Glassy_Acceleration")));
							}
						}
					};
					yield return Gizmo;
				}
			}
		}
	}

}
