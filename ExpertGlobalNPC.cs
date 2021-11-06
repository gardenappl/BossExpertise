
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
		
		public override void ResetEffects(NPC npc)
		{
			/*if (BossExpertise.FakeExpert == false)
			{
				BossExpertise.HookExpertMode(false);
			}*/
		}

		bool ShouldModifyNPC(NPC npc)
		{
			if (ModContent.GetInstance<Config>().BossBlacklist.Contains(new NPCDefinition(npc.type)))
				return false;
			else if (ModContent.GetInstance<Config>().BossWhitelist.Contains(new NPCDefinition(npc.type)))
				return true;

			return npc.boss;
		}
		
		public override bool PreAI(NPC npc)
		{
			if (ModContent.GetInstance<Config>().ChangeBossAI && ShouldModifyNPC(npc))
				BossExpertise.HookExpertMode(true);
			return base.PreAI(npc);
		}
		
		public override void PostAI(NPC npc)
		{
			if (ModContent.GetInstance<Config>().ChangeBossAI && ShouldModifyNPC(npc) && !Main.expertMode)
				BossExpertise.HookExpertMode(false);
		}
		
		public override bool PreKill(NPC npc)
		{
			if(ModContent.GetInstance<Config>().DropTreasureBagsInNormal && ShouldModifyNPC(npc))
				BossExpertise.HookExpertMode(true);
			return base.PreKill(npc);
		}
		
		public override void ModifyNPCLoot(NPC npc, NPCLoot nPCLoot)
		{
			if (ModContent.GetInstance<Config>().DropTreasureBagsInNormal && ShouldModifyNPC(npc) && !Main.expertMode)
				BossExpertise.HookExpertMode(false);
		}
		
		public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if(ModContent.GetInstance<Config>().ChangeBossAI && ShouldModifyNPC(npc))
				BossExpertise.HookExpertMode(true);

			return base.PreDraw(npc, spriteBatch, screenPos, drawColor);
		}
		
		public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if (ModContent.GetInstance<Config>().ChangeBossAI && ShouldModifyNPC(npc) && !Main.expertMode)
			{
				BossExpertise.HookExpertMode(false);
			}
		}
	}
}
