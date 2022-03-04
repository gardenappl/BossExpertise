using System;
using Terraria;
using Terraria.ModLoader;

namespace BossExpertise
{
    class BossExpertiseSystem : ModSystem
	{
        public override void OnWorldLoad()
        {
			if (Main.masterMode)
				BossExpertise.ActualDifficulty = 2;
			else if (Main.expertMode)
				BossExpertise.ActualDifficulty = 1;
			else
				BossExpertise.ActualDifficulty = 0;
        }

		public override void PostUpdateTime()
		{
			if (Main.GameModeInfo.IsJourneyMode)
				if (Main.masterMode)
					BossExpertise.ActualDifficulty = 2;
				else if (Main.expertMode)
					BossExpertise.ActualDifficulty = 1;
				else
					BossExpertise.ActualDifficulty = 0;
			else
				return;
			
		}

		public override void PreSaveAndQuit()
		{
			if (BossExpertise.CurrentDifficulty > 0) //an extra check just in case
			{
				BossExpertise.ActualDifficulty = 0;
				BossExpertise.HookDifficultyMode(0);
			}
		}
	}
}
