using System.Collections.Generic;
using Verse;
using RimWorld;

namespace EnhancedBackstoryFeatures
{
	public class ThoughtWorker_Tabemono : ThoughtWorker
	{
		protected override ThoughtState CurrentSocialStateInternal(Pawn pawn, Pawn other)
		{
			if (other.RaceProps == null || pawn.RaceProps == null)
			{
				return false;
			}
			if (!other.RaceProps.Humanlike || other.Dead || !pawn.RaceProps.Humanlike)
			{
				return false;
			}
			if (!RelationsUtility.PawnsKnowEachOther(pawn, other))
			{
				return false;
			}
			TraitSet traits = pawn?.story?.traits ?? null;
			if (traits == null)
			{
				return false;
			}
			if (traits.HasTrait(DefDatabase<TraitDef>.GetNamed("Cannibal")))
			{
				if (ModsConfig.BiotechActive)
				{
					if ((other.genes?.Xenotype?.defName ?? "") == "Waster")
					{
						return ThoughtState.ActiveAtStage(1);
					}
					if ((other.genes?.Xenotype?.defName ?? "") == "Pigskin")
					{
						return ThoughtState.ActiveAtStage(2);
					}
				}
				return ThoughtState.ActiveAtStage(0);
			}
			return false;
		}
	}
}

