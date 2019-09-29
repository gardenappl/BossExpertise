
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace BossExpertise
{
	public class ExpertGlobalNPC : GlobalNPC
	{
		public static bool FakeExpert;
		
		public override void ResetEffects(NPC npc)
		{
			if (FakeExpert)
			{
				Main.expertMode = false;
				FakeExpert = false;
			}
		}

		bool ShouldModifyNPC(NPC npc)
		{
			if (!Config.Instance.ChangeBossAI)
				return false;
			if (Config.Instance.BossBlacklist.Contains(new NPCDefinition(npc.type)))
				return false;
			else if (Config.Instance.BossWhitelist.Contains(new NPCDefinition(npc.type)))
				return true;

			return npc.boss;
		}
		
		public override bool PreAI(NPC npc)
		{
			if (Config.Instance.ChangeBossAI && ShouldModifyNPC(npc) && !Main.expertMode)
			{
				Main.expertMode = true;
				FakeExpert = true;
			}
			return base.PreAI(npc);
		}
		
		public override void PostAI(NPC npc)
		{
			if (FakeExpert)
			{
				Main.expertMode = false;
				FakeExpert = false;
			}
		}
		
		public override bool PreNPCLoot(NPC npc)
		{
			if(Config.Instance.DropTreasureBagsInNormal && ShouldModifyNPC(npc) && !Main.expertMode)
			{
				Main.expertMode = true;
				FakeExpert = true;
			}
			return base.PreNPCLoot(npc);
		}
		
		public override void NPCLoot(NPC npc)
		{
			if(FakeExpert)
			{
				Main.expertMode = false;
				FakeExpert = false;
			}
		}
		
		public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Color drawColor)
		{
			if(Config.Instance.ChangeBossAI && ShouldModifyNPC(npc) && !Main.expertMode)
			{
				Main.expertMode = true;
				FakeExpert = true;
			}
			return base.PreDraw(npc, spriteBatch, drawColor);
		}
		
		public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Color drawColor)
		{
			if(FakeExpert)
			{
				Main.expertMode = false;
				FakeExpert = false;
			}
		}
	}
}
