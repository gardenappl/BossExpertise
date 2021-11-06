
using System;
using Terraria;
using Terraria.ModLoader;

namespace BossExpertise
{
	public class ExpertPlayer : ModPlayer
	{
		public override void ResetEffects()
		{
			/*if(BossExpertise.FakeExpert == true) //an extra check just in case
			{
				BossExpertise.HookExpertMode(false);
			}*/
		}
        public override void UpdateEquips()
        {
			if (!Main.expertMode || !ModContent.GetInstance<Config>().DemonHeartWorksInNormal)
				BossExpertise.HookExpertMode(true);
		}

		public override void PostUpdateEquips()
		{
			if (!Main.expertMode || !ModContent.GetInstance<Config>().DemonHeartWorksInNormal)
				BossExpertise.HookExpertMode(false);
		}

	}
}
