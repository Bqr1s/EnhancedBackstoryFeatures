using System.Collections.Generic;
using System.Reflection;
using Verse;
using RimWorld;
using RimWorld.Planet;
using HarmonyLib;
using UnityEngine;

namespace BiggerTooltips
{
	[StaticConstructorOnStartup]
	public class Main
	{
		static Main()
		{
			// Use unique harmony ID based on mod name (or location), so that assembly can be shipped in 2 separate mods being active together

			Harmony pat = new Harmony("ecb_bb_EnhancedBackstoryFeatures");
			pat.PatchAll();
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

