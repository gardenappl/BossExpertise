
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossExpertise
{
	public class ExpertGlobalItem : GlobalItem
	{
		public override void SetDefaults(Item item)
		{
			if(Config.DemonHeartHack && item.type == ItemID.DemonHeart)
				item.maxStack = 1;
		}
		
		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			if(Config.DemonHeartHack && item.type == ItemID.DemonHeart && !Main.expertMode)
			{
				var line = new TooltipLine(mod, "DemonHeart", "Right Click to use!");
				line.overrideColor = Colors.RarityRed;
				tooltips.Insert(2, line);
			}
		}
		
		public override bool CanRightClick(Item item)
		{
			return Config.DemonHeartHack && item.type == ItemID.DemonHeart && !Main.player[Main.myPlayer].extraAccessory && !Main.expertMode;
		}
		
		public override void RightClick(Item item, Player player)
		{
			if (Config.DemonHeartHack && item.type == ItemID.DemonHeart && !player.extraAccessory)
			{
				player.extraAccessory = true;
				if (Main.netMode == 1 || Main.netMode == 2)
				{
					var msg = mod.GetPacket();
					msg.Write((byte)ExpertMessageType.SyncDemonHeart);
					msg.Write(player.whoAmI);
					msg.Write(true);
					msg.Send();
				}
			}
		}
	}
}
