
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace BossExpertise
{
	public class ExpertGlobalItem : GlobalItem
	{
		public override bool CanUseItem(Item item, Player player)
		{
			if (item.type == ItemID.DemonHeart && !player.extraAccessory && ModContent.GetInstance<Config>().SlotsWorksInNormal)
			{
				int stack = item.stack;
				item.SetDefaults(ModContent.ItemType<FakeDemonHeart>());
				item.stack = stack;
			}
			return true;
		}
	}
}
