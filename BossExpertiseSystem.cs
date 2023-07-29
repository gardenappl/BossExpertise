using System;
using Terraria;
using Terraria.ModLoader;

namespace BossExpertise
{
    class BossExpertiseSystem : ModSystem
	{
		static public Difficulty worldDifficulty = new();
        public override void PostSetupContent()
        {
			if (Main.masterMode)
				worldDifficulty |= Difficulty.Master;
			else if (Main.expertMode)
				worldDifficulty |= Difficulty.Expert;
			else
				worldDifficulty |= Difficulty.Classic;
			/*
			if (Main.getGoodWorld)
				worldDifficulty |= Difficulty.ForTheWorthy;

			if (worldDifficulty.HasFlag(Difficulty.ForTheWorthy) || ModContent.GetInstance<Config>().ForTheWorthy)
			{
				BossExpertise.FakedDifficulty |= Difficulty.ForTheWorthy;
				BossExpertise.FakedBeneficialDifficulty |= Difficulty.ForTheWorthy;
			}*/

		}

		/*public override void PostUpdateTime()
		{
			if (Main.GameModeInfo.IsJourneyMode)
				if (Main.masterMode)
					BossExpertise.FakedDifficulty |= Difficulty.Master;
				else if (Main.expertMode)
					BossExpertise.FakedDifficulty |= Difficulty.Expert;
				else
					BossExpertise.FakedDifficulty |= Difficulty.Classic;
			else
				return;
			
		}*/

		public override void PreSaveAndQuit()
		{
			BossExpertise.HookDifficultyMode(worldDifficulty);
		}
	}
}
