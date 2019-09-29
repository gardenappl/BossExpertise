
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
			if (item.type == ItemID.DemonHeart && !player.extraAccessory &&
					!Main.expertMode && Config.Instance.DemonHeartWorksInNormal)
			{
				int stack = item.stack;
				item.SetDefaults(mod.ItemType<FakeDemonHeart>());
				item.stack = stack;
			}
			return true;
		}
	}
}
