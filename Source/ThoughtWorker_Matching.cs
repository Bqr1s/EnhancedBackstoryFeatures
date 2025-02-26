using System.Collections.Generic;
using System.Linq;
using Verse;
using RimWorld;

namespace EnhancedBackstoryFeatures
{
	public class ThoughtWorker_Matching : ThoughtWorker
	{
		public static Dictionary<string, int> stageIndexesDictionary;
		private List<string> GetAllMatchingTags(Pawn pawn)
		{
			List<string> result = new List<string>();
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
			List<string> ourTags = GetAllMatchingTags(pawn);
			List<string> theirTags = GetAllMatchingTags(other);
			IEnumerable<string> commonTags = ourTags.Intersect(theirTags);
			if (commonTags.Any())
			{
				// A hack. Thought stage is actually is a number in stages list. Each stage represent a thought for diffetent group of people with same backstory tags
				string key = commonTags.First();
				return ThoughtState.ActiveAtStage(stageIndexesDictionary[key]);
			}
			return false;
		}
	}
}

