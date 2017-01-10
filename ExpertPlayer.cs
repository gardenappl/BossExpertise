
using System;
using Terraria;
using Terraria.ModLoader;

namespace BossExpertise
{
	public class ExpertPlayer : ModPlayer
	{
		public override void ResetEffects()
		{
			if(Config.DemonHeartHack && player.extraAccessory && !(Main.expertMode || Main.gameMenu))
				player.extraAccessorySlots++;
		}
	}
}
