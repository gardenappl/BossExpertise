using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader.Config;

namespace BossExpertise
{
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
			if (Main.netMode == NetmodeID.MultiplayerClient)
            {
				message = Language.GetTextValue("Mods.BossExpertise.Configs.Config.ServerBlocked");
				return false;
			}
			return true;
		}

        public override void OnChanged()
        {
			Difficulty newDifficulty = Difficulty.None;
			Difficulty newBeneficialDifficulty = Difficulty.None;
			/*if (BossExpertiseSystem.worldDifficulty.HasFlag(Difficulty.ForTheWorthy) || ForTheWorthy)
			{
				newDifficulty |= Difficulty.ForTheWorthy;
				newBeneficialDifficulty |= Difficulty.ForTheWorthy;
			}*/	

			if (CurrentFakedDifficulty == "Expert")
			{
				newDifficulty |= Difficulty.Expert;
			}
			else if (CurrentFakedDifficulty == "Master")
			{
				newDifficulty |= Difficulty.Master;
			}
			else
			{
				newDifficulty |= Difficulty.Classic;
			}

			if (FakedBeneficialDifficulty == "Expert")
			{
				newBeneficialDifficulty |= Difficulty.Expert;
			}
			else if (FakedBeneficialDifficulty == "Master")
			{
				newBeneficialDifficulty |= Difficulty.Master;
			}
			else
			{
				newBeneficialDifficulty |= Difficulty.Classic;
			}

			if (newDifficulty != BossExpertise.FakedDifficulty)
				BossExpertise.FakedDifficulty = newDifficulty;

			if (newBeneficialDifficulty != BossExpertise.FakedBeneficialDifficulty)
				BossExpertise.FakedBeneficialDifficulty = newBeneficialDifficulty;
		}

        [DrawTicks]
		[OptionStrings(new string[] { "Expert", "Master"})]
		[DefaultValue("Expert")]
		public string CurrentFakedDifficulty;

		[DrawTicks]
		[OptionStrings(new string[] { "Expert", "Master" })]
		[DefaultValue("Expert")]
		public string FakedBeneficialDifficulty;

		//[DefaultValue(false)]
		//public bool ForTheWorthy;

		[DefaultValue(false)]
		public bool DropTreasureBagsInNormal;

		[DefaultValue(true)]
		public bool ChangeBossAI;

		[DefaultValue(false)]
		public bool ChangeNPCAI;

		[DefaultValue(false)]
		public bool SlotsWorksInNormal;

		public List<NPCDefinition> BossBlacklist = new List<NPCDefinition>();

		public List<NPCDefinition> BossWhitelist = new List<NPCDefinition>
				{
					new NPCDefinition(NPCID.DD2Betsy),
					new NPCDefinition(NPCID.PirateShip),
					new NPCDefinition(NPCID.PirateShipCannon)
				};

		[ReloadRequired]
		[DefaultValue(true)]
		public bool AddExpertCommand;

		[ReloadRequired]
		[DefaultValue(true)]
		public bool AddCheatSheetButton;

		[OnDeserialized]
		internal void OnDeserializedMethod(StreamingContext context)
		{
			// RangeAttribute is just a suggestion to the UI. If we want to enforce constraints, we need to validate the data here. Users can edit config files manually with values outside the RangeAttribute, so we fix here if necessary.
			// Both enforcing ranges and not enforcing ranges have uses in mods. Make sure you fix config values if values outside the range will mess up your mod.
			if (CurrentFakedDifficulty != "Expert" && CurrentFakedDifficulty != "Master")
				CurrentFakedDifficulty = "Expert";

			if (FakedBeneficialDifficulty != "Expert" && FakedBeneficialDifficulty != "Master")
				FakedBeneficialDifficulty = "Expert";
		}
	}
}
