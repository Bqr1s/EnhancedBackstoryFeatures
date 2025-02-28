using System.Collections.Generic;
using Verse;
using RimWorld;

namespace EnhancedBackstoryFeatures
{
	public class ThoughtWorker_LazyBumo : ThoughtWorker
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
			if (traits != null && (traits.HasTrait(TraitDefOf.Kind) || traits.HasTrait(TraitDefOf.Ascetic)))
			{
				return false;
			}
			TraitSet otherTraits = other?.story?.traits ?? null;
			// Should not clutter and overlap with Vanilla thought of hard worker vs lazy, so disabled for hard workers
			if (traits.DegreeOfTrait(TraitDefOf.Industriousness) == 0 && otherTraits.DegreeOfTrait(TraitDefOf.Industriousness) < 0)
			{
				return ThoughtState.ActiveAtStage(0);
			}
			if (traits.DegreeOfTrait(TraitDefOf.Industriousness) < 0 && otherTraits.DegreeOfTrait(TraitDefOf.Industriousness) < traits.DegreeOfTrait(TraitDefOf.Industriousness))
			{
				return ThoughtState.ActiveAtStage(1);
			}
			return false;
		}
	}
}

