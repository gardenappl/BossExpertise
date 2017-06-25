
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace BossExpertise
{
	public class ExpertGlobalNPC : GlobalNPC
	{
		public static bool FakeExpert;
		
		public override void ResetEffects(NPC npc)
		{
			if(FakeExpert)
			{
				(mod as BossExpertise).SyncExpertMode(false);
				FakeExpert = false;
			}
		}
		
		public override bool PreAI(NPC npc)
		{
			if(npc.boss && Config.ChangeBossAI && !Main.expertMode)
			{
				FakeExpert = true;
				(mod as BossExpertise).SyncExpertMode(true);
			}
			return base.PreAI(npc);
		}
		
		public override void PostAI(NPC npc)
		{
			if(FakeExpert)
			{
				(mod as BossExpertise).SyncExpertMode(false);
			}
		}
		
		public override bool PreNPCLoot(NPC npc)
		{
			if(npc.boss && Config.DropBags && !Main.expertMode)
			{
				FakeExpert = true;
				(mod as BossExpertise).SyncExpertMode(true);
			}
			return base.PreNPCLoot(npc);
		}
		
		public override void NPCLoot(NPC npc)
		{
			if(FakeExpert)
			{
				FakeExpert = false;
				(mod as BossExpertise).SyncExpertMode(false);
			}
		}
		
		public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Color drawColor)
		{
			if(!Main.expertMode)
			{
				FakeExpert = true;
				Main.expertMode = true; //drawing is client-side only so there's no need to sync
			}
			return base.PreDraw(npc, spriteBatch, drawColor);
		}
		
		public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Color drawColor)
		{
			if(FakeExpert)
			{
				FakeExpert = false;
				Main.expertMode = false;
			}
		}
	}
}
