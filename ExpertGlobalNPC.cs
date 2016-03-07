
using System;
using Terraria;
using Terraria.ModLoader;

namespace BossExpertise
{
	public class ExpertGlobalNPC : GlobalNPC
	{
		public override bool PreAI(NPC npc)
		{
			if(npc.boss && !Main.expertMode)
			{
				var info = (ExpertNPCInfo)npc.GetModInfo(mod, "ExpertNPCInfo");
				Main.expertMode = true;
				info.FakeExpertMode = true;
			}
			return true;
		}
		
		public override void PostAI(NPC npc)
		{
			var info = (ExpertNPCInfo)npc.GetModInfo(mod, "ExpertNPCInfo");
			if(info.FakeExpertMode)
				Main.expertMode = false;
		}
	}
}
