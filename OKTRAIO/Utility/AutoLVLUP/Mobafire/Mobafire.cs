using EloBuddy;

namespace OKTRAIO.Utility.AutoLVLUP.Mobafire
{
    class Mobafire
    {
        public static void Init()
        {
            if (Leveler.AbilityPower)
            {
                if (Leveler.Laner)
                {
                    switch (Player.Instance.ChampionName)
                    {
                        case "Ashe":
                            Leveler.AbilitySequence = new[] { 2, 1, 3, 2, 1, 4, 1, 1, 2, 1, 4, 2, 2, 3, 3, 4, 3, 3 };
                            break;
                        case "Caitlyn":
                            Leveler.AbilitySequence = new[] { 2, 3, 2, 1, 2, 4, 2, 3, 2, 3, 4, 3, 3, 1, 1, 4, 1, 1 };
                            break;
                    }
                }
                else if (Leveler.Jungler)
                {
                    switch (Player.Instance.ChampionName)
                    {
                        case "Ashe":
                            Leveler.AbilitySequence = new[] { 2, 1, 3, 2, 1, 4, 1, 1, 2, 1, 4, 2, 2, 3, 3, 4, 3, 3 };
                            break;
                        case "Caitlyn":
                            Leveler.AbilitySequence = new[] { 2, 3, 2, 1, 2, 4, 2, 3, 2, 3, 4, 3, 3, 1, 1, 4, 1, 1 };
                            break;
                    }
                }
                else if (Leveler.Support)
                {
                    switch (Player.Instance.ChampionName)
                    {
                        case "Ashe":
                            Leveler.AbilitySequence = new[] { 2, 1, 3, 2, 1, 4, 1, 1, 2, 1, 4, 2, 2, 3, 3, 4, 3, 3 };
                            break;
                        case "Caitlyn":
                            Leveler.AbilitySequence = new[] { 2, 3, 2, 1, 2, 4, 2, 3, 2, 3, 4, 3, 3, 1, 1, 4, 1, 1 };
                            break;
                    }
                }
            }
            else
            {
                if (Leveler.Laner)
                {
                    switch (Player.Instance.ChampionName)
                    {
                        case "Ashe":
                            Leveler.AbilitySequence = new[] {2, 1, 2, 3, 2, 4, 2, 1, 2, 1, 4, 1, 1, 2, 2, 4, 2, 2};
                            break;
                        case "Caitlyn":
                            Leveler.AbilitySequence = new[] {1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 1, 1};
                            break;
                    }
                }
                else if (Leveler.Jungler)
                {
                    switch (Player.Instance.ChampionName)
                    {
                        case "Ashe":
                            Leveler.AbilitySequence = new[] {1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3};
                            break;
                        case "Caitlyn":
                            Leveler.AbilitySequence = new[] {1, 2, 1, 3, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3};
                            break;
                    }
                }
                else if (Leveler.Support)
                {
                    switch (Player.Instance.ChampionName)
                    {
                        case "Ashe":
                            Leveler.AbilitySequence = new[] {3, 2, 1, 3, 3, 4, 2, 3, 2, 3, 4, 2, 2, 1, 1, 4, 1, 1};
                            break;
                        case "Caitlyn":
                            Leveler.AbilitySequence = new[] {2, 3, 2, 3, 3, 4, 3, 2, 3, 2, 2, 4, 1, 1, 1, 4, 1, 1};
                            break;
                    }
                }
            }
        }
    }
}
