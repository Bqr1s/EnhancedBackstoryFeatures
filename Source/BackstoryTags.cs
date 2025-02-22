using System.Collections.Generic;
using Verse;
using RimWorld;

namespace EnhancedBackstoryFeatures
{
	public class BackstoryTags : DefModExtension
	{
		public List<string> RelationalThoughtTags;
		// These tags should match on 2 pawns to work. So they are placed in separate list for optimization.
		public List<int> MatchingTags;
		// Asymmetric relation with otaku having thought about oshi
		public List<int> OshiTags;
		public List<int> OtakuTags;
	}
}

