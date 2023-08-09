
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;

namespace BossExpertise
{
    public class ExpertCommand : ModCommand
    {
        public override void Action(CommandCaller caller, string input, string[] args)
        {
            Difficulty newDifficulty = Difficulty.None;
            if (args.Length == 0)
            {
                Difficulty currentDifficulty = BossExpertise.FakedDifficulty;
                if (currentDifficulty.HasFlag(Difficulty.Classic))
                    newDifficulty = Difficulty.Expert;
                else if (currentDifficulty.HasFlag(Difficulty.Expert))
                    newDifficulty = Difficulty.Master;
                else
                    newDifficulty = Difficulty.Classic;
                ModContent.GetInstance<BossExpertise>().SyncDifficultyMode(newDifficulty, SettingTarget.World);
            }
            else if (args.Length == 1)
            {
                if (args[0].Equals("2", StringComparison.OrdinalIgnoreCase))
                {
                    newDifficulty = Difficulty.Master;
                    ModContent.GetInstance<BossExpertise>().SyncDifficultyMode(newDifficulty, SettingTarget.World);
                }
                else if (args[0].Equals("1", StringComparison.OrdinalIgnoreCase))
                {
                    newDifficulty = Difficulty.Expert;
                    ModContent.GetInstance<BossExpertise>().SyncDifficultyMode(newDifficulty, SettingTarget.World);
                }
                else if (args[0].Equals("0", StringComparison.OrdinalIgnoreCase))
                {
                    newDifficulty = Difficulty.Classic;
                    ModContent.GetInstance<BossExpertise>().SyncDifficultyMode(newDifficulty, SettingTarget.World);
                }
                else
                    Main.NewText(Language.GetTextValue("Mods.BossExpertise.DifficultyCommandUsage"));
            }
            else
            {
                Main.NewText(Language.GetTextValue("Mods.BossExpertise.DifficultyCommandUsage"));
            }

        }
        public override string Usage => Language.GetTextValue("Mods.BossExpertise.DifficultyCommandUsage");

        public override string Command
        {
            get { return "difficulty"; }
        }

        public override CommandType Type
        {
            get { return CommandType.World; }
        }

        public void AddCommand()
        {
            if(ModContent.GetInstance<Config>().AddExpertCommand)
                Register();
        }

    }
}
