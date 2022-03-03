using System;
using Terraria;
using Terraria.ModLoader;

namespace BossExpertise
{
    class BossExpertiseSystem : ModSystem
	{ 
		public override void PreSaveAndQuit()
		{
			if (BossExpertise.CurrentDifficulty > 0) //an extra check just in case
			{
				BossExpertise.HookDifficultyMode(0);
			}
		}
	}
}
