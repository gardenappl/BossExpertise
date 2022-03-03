using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader.Config;

namespace BossExpertise
{
	[Label("$Mods.BossExpertise.Config")]
	public class Config : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ServerSide;

		//Load the config manually after handling LegacyConfigs
		public override bool Autoload(ref string name)
		{
			return false;
		}

		public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref string message)
		{
			message = Language.GetTextValue("Mods.BossExpertise.Config.ServerBlocked");
			return false;
		}

		[Label("$Mods.BossExpertise.Config.CurrentFakedDifficulty")]
		[Tooltip("$Mods.BossExpertise.Config.CurrentFakedDifficulty.Desc")]
		[ReloadRequired]
		[DrawTicks]
		[OptionStrings(new string[] { "Expert", "Master"})]
		[DefaultValue("Expert")]
		public string CurrentFakedDifficulty;

		[Label("$Mods.BossExpertise.Config.DropBags")]
		[DefaultValue(false)]
		public bool DropTreasureBagsInNormal;

		/*[Label("$Mods.BossExpertise.Config.AddCheatSheetButton")]
		[Tooltip("$Mods.BossExpertise.Config.AddCheatSheetButton.Desc")]
		[ReloadRequired]
		[DefaultValue(true)]
		public bool AddCheatSheetButton;*/

		[Label("$Mods.BossExpertise.Config.ChangeAI")]
		[Tooltip("$Mods.BossExpertise.Config.ChangeAI.Desc")]
		[DefaultValue(true)]
		public bool ChangeBossAI;

		[Label("$Mods.BossExpertise.Config.BossBlacklist")]
		[Tooltip("$Mods.BossExpertise.Config.BossBlacklist.Desc")]
		public List<NPCDefinition> BossBlacklist = new List<NPCDefinition>();

		[Label("$Mods.BossExpertise.Config.BossWhitelist")]
		[Tooltip("$Mods.BossExpertise.Config.BossWhitelist.Desc")]
		public List<NPCDefinition> BossWhitelist = new List<NPCDefinition>
				{
					new NPCDefinition(NPCID.DD2Betsy)
				};

		/*[Label("$Mods.BossExpertise.Config.ExpertCommand")]
		[Tooltip("$Mods.BossExpertise.Config.ExpertCommand.Desc")]
		[ReloadRequired]
		[DefaultValue(true)]
		public bool AddExpertCommand;*/

		[Label("$Mods.BossExpertise.Config.SlotsHack")]
		[Tooltip("$Mods.BossExpertise.Config.SlotsHack.Desc")]
		public bool SlotsWorksInNormal;

		[OnDeserialized]
		internal void OnDeserializedMethod(StreamingContext context)
		{
			// RangeAttribute is just a suggestion to the UI. If we want to enforce constraints, we need to validate the data here. Users can edit config files manually with values outside the RangeAttribute, so we fix here if necessary.
			// Both enforcing ranges and not enforcing ranges have uses in mods. Make sure you fix config values if values outside the range will mess up your mod.
			if (CurrentFakedDifficulty != "Expert" && CurrentFakedDifficulty != "Master")
				CurrentFakedDifficulty = "Expert";
		}
	}
}
