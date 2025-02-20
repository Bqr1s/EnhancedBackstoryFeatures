using System.Collections.Generic;
using System.Linq;
using Verse;
using RimWorld;

namespace EnhancedBackstoryFeatures
{
	/*public class MatchingDictionaryDef : Def
	{
		public Dictionary<string, string> thoughts;
	}*/
	
	public class ThoughtWorker_Matching : ThoughtWorker
	{
		private List<int> GetAllMatchingTaggs(Pawn pawn)
		{
			List<int> result = new List<int>();
			foreach (BackstoryDef bs in pawn.story.AllBackstories)
			{
				BackstoryTags tags = bs.GetModExtension<BackstoryTags>();
				if (tags != null && tags.MatchingTags != null)
				{
					result.AddRange(tags.MatchingTags);
				}
			}
			return result;
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
			if (pawn.story == null || other.story == null)
			{
				return false;
			}
			if (!RelationsUtility.PawnsKnowEachOther(pawn, other))
			{
				return false;
			}
			List<int> ourTags = GetAllMatchingTaggs(pawn);
			List<int> theirTags = GetAllMatchingTaggs(other);
			IEnumerable<int> commonTags = ourTags.Intersect(theirTags);
			if (commonTags.Any())
			{
				// A hack. Thought stage is actually is a number in stages list. Each stage represent a thought for diffetent group of people with same backstory tags
				return ThoughtState.ActiveAtStage(commonTags.First());
			}
			return false;
		}
	}
}

