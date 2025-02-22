using System.Collections.Generic;
using Verse;
using RimWorld;

namespace EnhancedBackstoryFeatures
{
	public class ThoughtWorker_BackstoryRelational : ThoughtWorker
	{
		private bool HasTag(Pawn pawn, string tag)
		{
			if (pawn.story == null)
			{
				return false;
			}
			foreach (BackstoryDef bs in pawn.story.AllBackstories)
			{
				BackstoryTags tags = bs.GetModExtension<BackstoryTags>();
				if (tags != null && tags.RelationalThoughtTags != null && tags.RelationalThoughtTags.Contains(tag))
				{
					return true;
				}
			}
			return false;
		}
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
			if (!HasTag(pawn, def.defName) && HasTag(other, def.defName))
			{
				return true;
			}
			return false;
		}
	}
}

