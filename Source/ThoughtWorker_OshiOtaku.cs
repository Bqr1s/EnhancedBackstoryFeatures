using System.Collections.Generic;
using System.Linq;
using Verse;
using RimWorld;

namespace EnhancedBackstoryFeatures
{
	public class ThoughtWorker_OshiOtaku : ThoughtWorker
	{
		public static Dictionary<string, int> thoughtIndexesDictionary;
		/*public static void UpdateThoughts(string tag)
		{
			// We are initializing, this got to be static because no ThoughtWorker surroundings properly created
			ThoughtDef thoughtDef = DefDatabase<ThoughtDef>.GetNamed(tag);
			Dictionary<string, ThoughtStage> thoughtsDictionary = thoughtDef.GetModExtension<ThoghtDictionaryDef>()?.items ?? null;
			if (thoughtsDictionary == null)
			{
				return;
			}
			thoughtIndexesDictionary = new Dictionary<string, int>();
			List<ThoughtStage> stages = DefDatabase<ThoughtDef>.GetNamed(tag).stages;
			if(stages == null)
			{
				stages = new List<ThoughtStage>();
				DefDatabase<ThoughtDef>.GetNamed(tag).stages = stages;
			}
			foreach (KeyValuePair<string, ThoughtStage> entry in thoughtsDictionary)
			{
				stages.Add(entry.Value);
				thoughtIndexesDictionary.Add(entry.Key, stages.Count - 1);
			}
		}*/
		private List<string> GetAllOshiTags(Pawn pawn)
		{
			List<string> result = new List<string>();
			foreach (BackstoryDef bs in pawn.story.AllBackstories)
			{
				BackstoryTags tags = bs.GetModExtension<BackstoryTags>();
				if (tags != null && tags.OshiTags != null)
				{
					result.AddRange(tags.OshiTags);
				}
			}
			return result;
		}

		private List<string> GetAllOtakuTags(Pawn pawn)
		{
			List<string> result = new List<string>();
			foreach (BackstoryDef bs in pawn.story.AllBackstories)
			{
				BackstoryTags tags = bs.GetModExtension<BackstoryTags>();
				if (tags != null && tags.OtakuTags != null)
				{
					result.AddRange(tags.OtakuTags);
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

			List<string> ourOtakuTags = GetAllOtakuTags(pawn);
			List<string> theirOshiTags = GetAllOshiTags(other);
			IEnumerable<string> oshiOtakuMatches = theirOshiTags.Intersect(ourOtakuTags);
			if (oshiOtakuMatches.Any())
			{
				// A hack. Thought stage is actually is a number in stages list. Each stage represent a thought for diffetent group of people with same backstory tags
				string key = oshiOtakuMatches.First();
				Log.Warning("++Thought with key:" + key);
				return ThoughtState.ActiveAtStage(thoughtIndexesDictionary[key]);
			}
			return false;
		}
	}
}

