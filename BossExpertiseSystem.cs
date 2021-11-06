using System;
using Terraria;
using Terraria.ModLoader;

namespace BossExpertise
{
    class BossExpertiseSystem : ModSystem
	{ 
		public override void PreSaveAndQuit()
		{
			if (BossExpertise.FakeExpert == true) //an extra check just in case
			{
				BossExpertise.HookExpertMode(false);
				BossExpertise.FakeExpert = null;
			}
		}

        public override void PreUpdateTime()
        {
			if (BossExpertise.FakeExpert == true)
				BossExpertise.HookExpertMode(true);
        }

		public override void PostUpdateTime()
		{
			if (BossExpertise.FakeExpert == true)
				BossExpertise.HookExpertMode(true);
		}
	}
}
