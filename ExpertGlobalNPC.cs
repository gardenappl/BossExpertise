
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

		public static bool ShouldModifyNPC(NPC npc)
		{
			Config config = ModContent.GetInstance<Config>();
			if (config.BossBlacklist.Contains(new NPCDefinition(npc.type)))
				return false;
			else if (config.BossWhitelist.Contains(new NPCDefinition(npc.type)))
				return true;

			return (config.ChangeBossAI && npc.boss) || (!npc.boss && config.ChangeNPCAI);
		}
		
		public override bool PreAI(NPC npc)
		{
			if (ShouldModifyNPC(npc))
				BossExpertise.HookDifficultyMode(BossExpertise.FakedDifficulty);
			return true;
		}
		
		public override void PostAI(NPC npc)
		{
			if (ShouldModifyNPC(npc))
				BossExpertise.HookDifficultyMode(BossExpertiseSystem.worldDifficulty);
		}
		
		public override bool PreKill(NPC npc)
		{
			if(ModContent.GetInstance<Config>().DropTreasureBagsInNormal && ShouldModifyNPC(npc))
				BossExpertise.HookDifficultyMode(BossExpertise.FakedBeneficialDifficulty);

			return true;
		}

        public override void OnKill(NPC npc)
        {
			if (ModContent.GetInstance<Config>().DropTreasureBagsInNormal && ShouldModifyNPC(npc))
				BossExpertise.HookDifficultyMode(BossExpertiseSystem.worldDifficulty);
		}
		
		public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if (ShouldModifyNPC(npc))
				BossExpertise.HookDifficultyMode(BossExpertise.FakedDifficulty);
			
			return true;
		}
		
		public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if (!Main.expertMode && ShouldModifyNPC(npc))
				BossExpertise.HookDifficultyMode(BossExpertiseSystem.worldDifficulty);
		}
	}
}
