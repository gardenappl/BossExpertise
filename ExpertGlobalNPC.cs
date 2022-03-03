
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
				BossExpertise.HookDifficultyMode(BossExpertise.CurrentDifficulty);
			return true;
		}
		
		public override void PostAI(NPC npc)
		{
			if (ModContent.GetInstance<Config>().ChangeBossAI && ShouldModifyNPC(npc))
				BossExpertise.HookDifficultyMode(BossExpertise.CurrentDifficulty);
		}
		
		public override bool PreKill(NPC npc)
		{
			if(ModContent.GetInstance<Config>().DropTreasureBagsInNormal && ShouldModifyNPC(npc))
				BossExpertise.HookDifficultyMode(BossExpertise.CurrentDifficulty);
			return true;
		}

        public override void OnKill(NPC npc)
        {
			if (ModContent.GetInstance<Config>().DropTreasureBagsInNormal && ShouldModifyNPC(npc))
				BossExpertise.HookDifficultyMode(BossExpertise.CurrentDifficulty);
		}
		
		public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if(ModContent.GetInstance<Config>().ChangeBossAI && ShouldModifyNPC(npc))
				BossExpertise.HookDifficultyMode(BossExpertise.CurrentDifficulty);

			return true;
		}
		
		public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if (ModContent.GetInstance<Config>().ChangeBossAI && ShouldModifyNPC(npc) && !Main.expertMode)
			{
				BossExpertise.HookDifficultyMode(BossExpertise.CurrentDifficulty);
			}
		}
	}
}
