using System;
using System.Collections.Generic;
using System.Linq;

namespace GroupHeist
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a list to store the crew members
            List<IRobber> crew = new List<IRobber>();

            // Create initial crew members with their properties
            Hacker Adam = new Hacker("Adam", 50, 15);
            Hacker Sergey = new Hacker("Sergey", 40, 15);
            Muscle Cullen = new Muscle("Cullen", 40, 15);
            Muscle Ace = new Muscle("Ace", 40, 15);
            LockSpecialist Courtney = new LockSpecialist("Courtney", 50, 15);

            // Add the initial crew members to the rolodex
            List<IRobber> rolodex = new List<IRobber>() { Adam, Sergey, Cullen, Ace, Courtney };

            // Print the total number of operatives in the rolodex
            System.Console.WriteLine($"There are {rolodex.Count} operatives.");

            // Call the NewTeamMember function to add a new crew member
            NewTeamMember();

            // Function to add a new crew member
            void NewTeamMember()
            {
                // Prompt the user for the new crew member's name
                System.Console.WriteLine($"Who is your new crew member?");
                string newCrewMember = Console.ReadLine();
                if (newCrewMember == "")
                {
                    return;
                }
                // Prompt the user to choose the new crew member's specialty
                System.Console.WriteLine($"Pick your specialty\n\t1) Hacker\n\t2) Muscle\n\t3) Lock Specialist");
                int specialty = int.Parse(Console.ReadLine());
                // Prompt the user for the new crew member's skill level
                System.Console.WriteLine("What is your skill level between 1 and 100?");
                int newCMSkillLevel = int.Parse(Console.ReadLine());
                // Prompt the user for the new crew member's cut
                System.Console.WriteLine("What is your cut?");
                int newCMCut = int.Parse(Console.ReadLine());

                // Add the new crew member to the rolodex based on their specialty
                switch (specialty)
                {
                    case 1:
                        rolodex.Add(new Hacker(newCrewMember, newCMSkillLevel, newCMCut));
                        break;
                    case 2:
                        rolodex.Add(new Muscle(newCrewMember, newCMSkillLevel, newCMCut));
                        break;
                    case 3:
                        rolodex.Add(new LockSpecialist(newCrewMember, newCMSkillLevel, newCMCut));
                        break;
                    default:
                        break;
                }

                // Print the updated count of operatives in the rolodex
                System.Console.WriteLine($"There are now {rolodex.Count} operatives.");
                addAnotherTeamMember();
            }

            // Function to add another crew member by calling NewTeamMember function
            void addAnotherTeamMember()
            {
                NewTeamMember();
            }

            // Generate random scores for the bank's security systems and cash on hand
            Random random = new Random();
            int theAlarmScore = random.Next(0, 101);
            int theVaultScore = random.Next(0, 101);
            int theSecurityGuardScore = random.Next(0, 101);
            int theCashOnHand = random.Next(50000, 1000000001);

            // Create a new bank object with the generated scores and cash on hand
            Bank theBank = new Bank(theCashOnHand, theAlarmScore, theVaultScore, theSecurityGuardScore);

            // Create a dictionary to store the bank's security scores
            Dictionary<string, int> Scores = new Dictionary<string, int>();
            Scores.Add("Alarm Score", theAlarmScore);
            Scores.Add("Vault Score", theVaultScore);
            Scores.Add("Security Guard Score", theSecurityGuardScore);

            // Find the security system with the highest and lowest scores
            var maxValueKey = Scores.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
            Console.WriteLine($"Most Secure: {maxValueKey}");

            var minValueKey = Scores.Aggregate((x, y) => x.Value < y.Value ? x : y).Key;
            Console.WriteLine($"Least Secure: {minValueKey}");

            // Call the CrewSelection function to select crew members
            CrewSelection();

            // Function to select crew members from the rolodex
            void CrewSelection()
            {
                System.Console.WriteLine($"Select your crew:");
                for (int i = 0; i < rolodex.Count; i++)
                {
                    IRobber robber = rolodex[i];
                    // If totalPercentageCut() + robber.PercentageCut <= 100, show robber info
                    int totalPercentageGiven = totalPercentageCut();
                    if ((totalPercentageGiven + rolodex[i].PercentageCut <= 100))
                    {
                        System.Console.WriteLine($"\t{i + 1}) {robber.Name}\n\tSpecialist: {robber.Specialist}\n\tSkill Level: {robber.SkillLevel}\n\tPercentage Cut: {robber.PercentageCut}\n");
                    }
                }

                System.Console.WriteLine($"The total percentage cut is {totalPercentageCut()}");
                System.Console.WriteLine("Select the number for the member you want to add to your crew.");
                string selectedCrewString = Console.ReadLine();

                if (selectedCrewString == "")
                {
                    return;
                }
                int selectedCrew = int.Parse(selectedCrewString);

                if (selectedCrew >= 0 && selectedCrew < rolodex.Count)
                {
                    System.Console.WriteLine(rolodex[selectedCrew - 1].Name);
                }

                // Add the selected crew member to the crew and remove them from the rolodex
                crew.Add(rolodex[selectedCrew - 1]);
                rolodex.RemoveAt(selectedCrew - 1);
                System.Console.WriteLine($"The total percentage cut is {totalPercentageCut()}");

                addAnotherCrewMember();
            }

            // Function to add another crew member by calling CrewSelection function
            void addAnotherCrewMember()
            {
                CrewSelection();
            }

            // Function to calculate the total percentage cut of the crew
            int totalPercentageCut()
            {
                int totalPercentageCut = 0;
                {
                    foreach (IRobber member in crew)
                    {
                        totalPercentageCut += member.PercentageCut;
                    }
                }

                // System.Console.WriteLine($"Total Percentage Cut: {totalPercentageCut}");
                return totalPercentageCut;
            }

            // Function to run the heist
            void runHeist()
            {
                // Perform the heist using each crew member's skill
                foreach (IRobber member in crew)
                {
                    member.PerformSkill(theBank);
                }

                // Check if the heist was successful
                if (theBank.AlarmScore + theBank.VaultScore + theBank.SecurityGuardScore <= 0)
                {
                    System.Console.WriteLine($"You robbed the bank and walked away with ${theBank.CashOnHand}");
                    // Calculate each crew member's cut and print it
                    foreach (IRobber member in crew)
                    {
                        int memberCut = member.PercentageCut * (theBank.CashOnHand / 100);
                        System.Console.WriteLine($"{member.Name} walked away with ${memberCut}.");
                    }
                    // Calculate the user's cut if the total percentage cut is less than 100
                    if (totalPercentageCut() < 100)
                    {
                        int yourCut = ((100 - totalPercentageCut()) * (theBank.CashOnHand / 100));
                        System.Console.WriteLine($"And you (the brains behind the operation) take ${yourCut}.");
                    }
                }
                else
                {
                    System.Console.WriteLine($"Go directly to jail - do not past go, do not collect $200");
                }
            }
            // Call the runHeist function to start the heist
            runHeist();

        }
    }
}
