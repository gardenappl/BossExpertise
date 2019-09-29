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
		public override string Texture => "Terraria/Item_3335";

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.DemonHeart);
		}

		public override bool CanUseItem(Player player)
		{
			if (!Main.expertMode || player.extraAccessory || !Config.Instance.DemonHeartWorksInNormal)
				return false;
			return base.CanUseItem(player);
		}

		public override bool UseItem(Player player)
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
			int stack = item.stack;
			item.SetDefaults(ItemID.DemonHeart);
			item.stack = stack;
		}
	}
}
