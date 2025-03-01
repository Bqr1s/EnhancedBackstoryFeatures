using System.Collections.Generic;
using System.Reflection;
using Verse;
using RimWorld;
using RimWorld.Planet;
using HarmonyLib;
using UnityEngine;

namespace EnhancedBackstoryFeatures
{
	[StaticConstructorOnStartup]
	public class Main
	{
		static Main()
		{

		}
	}

	public class EnhancedBackstoryFeatures : Mod
	{
		public EnhancedBackstoryFeatures(ModContentPack content) : base(content)
		{
			Harmony pat = new Harmony("ecb_bb_EnhancedBackstoryFeatures");
			pat.PatchAll();
		}
	}

	[HarmonyPatch(typeof(PlayDataLoader), "ResetStaticDataPost")]
	public static class PostLoadedDefsFromXML
	{
		public static void UpdateThoughtsDictionary(ref Dictionary<string, int> indexesDictionary, string tag)
		{
			// We are initializing, this got to be static because no ThoughtWorker surroundings properly created
			ThoughtDef thoughtDef = DefDatabase<ThoughtDef>.GetNamed(tag);
			Dictionary<string, ThoughtStage> thoughtsDictionary = thoughtDef.GetModExtension<ThoghtDictionaryDef>()?.items ?? null;
			if (thoughtsDictionary == null)
			{
				return;
			}
			indexesDictionary = new Dictionary<string, int>();
			List<ThoughtStage> stages = DefDatabase<ThoughtDef>.GetNamed(tag).stages;
			if (stages == null)
			{
				stages = new List<ThoughtStage>();
				DefDatabase<ThoughtDef>.GetNamed(tag).stages = stages;
			}
			foreach (KeyValuePair<string, ThoughtStage> entry in thoughtsDictionary)
			{
				stages.Add(entry.Value);
				indexesDictionary.Add(entry.Key, stages.Count - 1);
			}
		}
		public static void Postfix()
		{
			// This is a bit wired due to vanilla design not too handy for these type of thoughts.
			// During initializing, thought worker object with surroundings/linked object does not exist, aslo got to use one dictionary for all objects of the given type, thus static
			// and because of static, just calling same initialize function here for different classes internals. 
			UpdateThoughtsDictionary(ref ThoughtWorker_OshiOtaku.stageIndexesDictionary, "ecb_bb_ThoughtOshiOtaku");
			UpdateThoughtsDictionary(ref ThoughtWorker_Matching.stageIndexesDictionary, "ecb_bb_ThoughtMatching");
		}
	}

	[HarmonyPatch(typeof(PawnBioAndNameGenerator), "GetBackstoryCategoryFiltersFor")]
	public static class GenderedStories
	{
		public static void Postfix(ref List<BackstoryCategoryFilter> __result, Pawn pawn)
		{
			string excludeTag = pawn.gender == Gender.Male ? "ecb_bb_OnlyWomen" : "ecb_bb_OnlyMen";
			List<BackstoryCategoryFilter> newResult = new List<BackstoryCategoryFilter>();
			foreach (BackstoryCategoryFilter filter in __result)
			{
				// Just deep copy constructor impl here, in case filters it the list, maybe from other mods, have non-local scope or so and 
				// are not intended to be changed individually for each generation case.
				BackstoryCategoryFilter newFilter = new BackstoryCategoryFilter();
				newFilter.categories = filter.categories?.ListFullCopy() ?? null;
				newFilter.exclude = filter.exclude?.ListFullCopy() ?? null;
				newFilter.categoriesChildhood = filter.categoriesChildhood?.ListFullCopy() ?? null;
				newFilter.excludeChildhood = filter.excludeChildhood?.ListFullCopy() ?? null;
				newFilter.categoriesAdulthood = filter.categoriesAdulthood?.ListFullCopy() ?? null;
				newFilter.excludeAdulthood = filter.excludeAdulthood?.ListFullCopy() ?? null;
				newFilter.commonality = filter.commonality;
				if (newFilter.exclude == null)
				{
					newFilter.exclude = new List<string>();
				}
				newFilter.exclude.Add(excludeTag);
				newResult.Add(newFilter);
			}
			__result = newResult;
		}
	}
}

