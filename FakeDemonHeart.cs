using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BossExpertise
{
	class FakeDemonHeart : ModItem
	{
		public override string Texture => "Terraria/Images/Item_" + ItemID.DemonHeart;

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.DemonHeart);
		}

		public override bool CanUseItem(Player player)
		{
			if (player.extraAccessory || !ModContent.GetInstance<Config>().SlotsWorksInNormal)
				return false;
			return base.CanUseItem(player);
		}

		public override bool? UseItem(Player player)
		{
			player.extraAccessory = true;
			if (Main.netMode == NetmodeID.MultiplayerClient || Main.netMode == NetmodeID.Server)
			{
				var msg = Mod.GetPacket();
				msg.Write((byte)ExpertMessageType.SyncDemonHeart);
				msg.Write(player.whoAmI);
				msg.Write(true);
				msg.Send();
			}

			ConvertToRegularHeart();
			return true;
		}

		public override bool OnPickup(Player player)
		{
			ConvertToRegularHeart();
			return base.OnPickup(player);
		}

		void ConvertToRegularHeart()
		{
			int stack = Item.stack;
			Item.SetDefaults(ItemID.DemonHeart);
			Item.stack = stack;
		}
	}
}
