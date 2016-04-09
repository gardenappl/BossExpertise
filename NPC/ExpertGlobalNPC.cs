
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
				SetFakeExpert(npc, true);
			return true;
		}
		
		public override void PostAI(NPC npc)
		{
			if(IsFakeExpert(npc))
				SetFakeExpert(npc, false);
		}
		
		public override bool PreNPCLoot(NPC npc)
		{
			if(npc.boss && Config.DropBags && !Main.expertMode)
				SetFakeExpert(npc, true);
			return true;
		}
		
		public override void NPCLoot(NPC npc)
		{
			if(IsFakeExpert(npc))
				SetFakeExpert(npc, false);
		}
		
		
		void SetFakeExpert(NPC npc, bool expert)
		{
			var info = npc.GetModInfo<ExpertNPCInfo>(mod);
			Main.expertMode = expert;
			info.FakeExpertMode = expert;
		}
		
		bool IsFakeExpert(NPC npc)
		{
			var info = npc.GetModInfo<ExpertNPCInfo>(mod);
			return info.FakeExpertMode;
		}
	}
}
