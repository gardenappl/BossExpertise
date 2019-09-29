
using System;
using Terraria;
using Terraria.ModLoader;

namespace BossExpertise
{
	public class ExpertPlayer : ModPlayer
	{
		public override void ResetEffects()
		{
			if(ExpertGlobalNPC.FakeExpert) //an extra check just in case
			{
				Main.expertMode = false;
				ExpertGlobalNPC.FakeExpert = false;
			}
			if(Config.Instance.DemonHeartWorksInNormal && player.extraAccessory && !(Main.expertMode || Main.gameMenu))
				player.extraAccessorySlots++;
		}
	}
}
