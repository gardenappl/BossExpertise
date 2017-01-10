
using System;
using Terraria;
using Terraria.ModLoader;

namespace BossExpertise
{
	public class ExpertNPCInfo : NPCInfo
	{
		public override bool Autoload(ref string name)
		{
			return false;
		}
		
		internal bool FakeExpertMode;
	}
}
