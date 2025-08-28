// Week1 Special Project.cs
// This is a special project for Week 1 of the course. Create a short story with user interactive input.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;

namespace GameProject
{
    class Game_Project
    {
        // Readability helper methods
        public static void Pause()
        {
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }

        public static void SlowWrite(string text, int delay = 30)
        {
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(delay); // Pause for a short duration
            }
            Console.WriteLine(); // Move to the next line after the text is written
        }

        // State-tracking variables
        private static int dragonVisits = 0;
        private static bool learnedDragonStory = false;
        private static bool silentDragonEncounter = false;

        // Encounter methods
        private static void EncounterDragon(Character player)
        {
            dragonVisits++; // Increment the visit counter

            SlowWrite("\nYou chose to go left...");
            SlowWrite("You encounter a dragon. He breathes fire into the air and words rumble from deep inside his chest.");
            Console.WriteLine();

            if (dragonVisits == 1)
            {
                SlowWrite("\"~~~Who are you?~~~~\" the dragon asks.");
                Console.WriteLine("1. Tell him your name: ");
                Console.WriteLine("2. Stay silent.");
                Console.WriteLine("3. Say 'I am the mighty hero!' and attack the dragon.");
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine() ?? "1";
                switch (choice) // First Meeting Choices
                {
                    case "1": // Tell him your name
                        SlowWrite("\"~~~Ah, " + player.Name + ",\" he says. \"~~~I have been expecting you.~~~\"");
                        SlowWrite("He seems pleased by your directness and drops a small, shimmering object at your feet.");
                        // Create an item that gives a +3 strength bonus
                        Item amulet = new Item("Dragon's Amulet", "A mystical amulet that hums with ancient power.", strengthBonus: 3);
                        player.AddItemToInventory(amulet);
                        SlowWrite("You feel a surge of strength as you pick it up.");
                        Pause();
                        SlowWrite("\"~~~I was told you may be able to release me from this nightmare.\" Come back when you are strong enough to defeat me.~~~\"");
                        Pause();
                        break;
                    case "2": // Stay silent
                        SlowWrite("\"Silence,\" he says. \"A wise choice. Many heroes are boastful and meet their end quickly.\"");
                        SlowWrite("Come back later when you have more courage.");
                        silentDragonEncounter = true;
                        break;
                    case "3": // Attack the dragon
                        SlowWrite("\"Foolish mortal!\" the dragon roars, and incinerates you with a blast of fire.");
                        player.CurrentHealth = 0; // Player dies
                        break;
                    default:
                        SlowWrite("\"You hesitate, and the moment is lost,\" the dragon says, clearly annoyed. He flies away, uninterested in you.");
                        return;
                }
            }
            else if (dragonVisits > 1)
            {
                SlowWrite("\"~~~You return.~~~\" the dragon rumbles, smoke curling from his nostrils.");
                Console.WriteLine("1. Ask for his story: ");
                Console.WriteLine("2. Stay silent.");
                Console.WriteLine("3. Say 'I am the mighty hero!' and attack the dragon.");
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine() ?? "1";
                switch (choice) // Second Meeting Choices
                {
                    case "1": // Ask for his story
                        SlowWrite("\"Ah, a curious one,\" the dragon rumbles, his voice like grinding stones. \"They call me Ignis. I was not always a prisoner of this damp cave.\"");
                        SlowWrite("\"Ages ago, I guarded the Sunstone of Valoria. A jealous archmage, unable to defeat me in open combat, laid a curse upon me.\"");
                        SlowWrite("\"I am bound here until a hero proves their worth not by the strength of their arm, but by the wisdom of their heart.\"");
                        SlowWrite("He gestures toward the amulet you carry. \"That you did not attack me on sight is a good sign. But the curse is strong. You must become stronger to break it.\"");
                        Pause();
                        break;
                    case "2": // Stay silent
                        if (silentDragonEncounter)
                        {
                            SlowWrite("\"~~~AGAIN you come in silence!?~~~\" he rumbles, his voice thick with menace. \"~~~THIS IS YOUR LAST CHANCE. SPEAK OR FACE MY WRATH!~~~\"");
                            Pause();
                        }
                        Console.WriteLine("1. \"My apologies, great one. I wish to hear your story.\"");
                        Console.WriteLine("2. (Stay silent)");
                        Console.Write("Enter your choice: ");
                        string silentChoice = Console.ReadLine() ?? "1";
                        if (silentChoice == "1")
                        {
                            SlowWrite("\"Hmph. Very well,\" he snorts, a puff of smoke escaping his nostrils. \"They call me Ignis...\"");
                            SlowWrite("He seems mollified and proceeds to tell you his tale, though his gaze remains burning on you.");
                            Pause();
                        }
                        else
                        {
                            SlowWrite("\"Your silence is your answer. You are found wanting!\" With a roar, he blasts you with a torrent of fire.");
                            player.CurrentHealth = 0; // Player dies
                        }
                        break;
                    case "3": // Attack the dragon
                        SlowWrite("\"Foolish mortal!\" the dragon roars, and incinerates you with a blast of fire.");
                        player.CurrentHealth = 0; // Player dies
                        break;
                    default:
                        SlowWrite("\"You hesitate, and the moment is lost,\" the dragon says, clearly annoyed. He flies away, uninterested in you.");
                        return;
                }
            }
        }
        private static void EncounterGoblin(Character player)
        {
            SlowWrite("\nYou chose to enter the dark cave...");
            // Create an enemy using the same Character class
            Character goblin = new Character("Goblin")
            {
                BaseStrength = 3,
                CurrentHealth = 30
            };
            BattleManager.StartCombat(player, goblin);

            if (player.IsAlive())
            {
                SlowWrite($"\nYou have defeated the {goblin.Name}!");
                Item goblinLoot = new Item("Crude Dagger", "A rusty but sharp dagger.", strengthBonus: 1);
                player.AddItemToInventory(goblinLoot);
            }
        }

        private static void MysteriousFigure(Character player)
        {
            SlowWrite("\nUnsure of where to go, you stand still...");
            SlowWrite("A mysterious traveler approaches you on the path. He looks you up and down...");

            // Use a switch with 'when' clauses for pattern matching based on health.
            switch (true)
            {

                case bool _ when player.CurrentHealth <= player.MaxHealth * 0.3: // Low health (30% or less)
                    SlowWrite("\"You look like you're in rough shape,\" he says, performing a quick ritual.");
                    double healthRestored = player.MaxHealth * 0.5; // Restore 50% of max health
                    player.CurrentHealth += healthRestored;
                    // Clamp health to the max health. Math.Min is a clean way to do this.
                    player.CurrentHealth = Math.Min(player.CurrentHealth, player.MaxHealth);
                    Console.WriteLine($"You feel a surge of vitality! Your health is now {player.CurrentHealth:F1}.");
                    break;

                case bool _ when player.CurrentHealth <= player.MaxHealth * 0.75: // Medium health
                    SlowWrite("You seem unsure of your path... perhaps fate will decide it.");
                    // Use the shared Random instance and an if/else if chain for clarity.
                    int roll = BattleManager.RollDice(20); // d20 roll (1 to 20)
                    if (roll <= 5)
                    // Rolls 1-5 (25% chance) Attacks the player
                    {
                        SlowWrite("\"You look a bit weak,\" he says with a hungry look in his eyes... He attacks!");
                        Character figure = new Character("Mysterious Figure") { BaseStrength = 8, CurrentHealth = 50 };
                        BattleManager.StartCombat(player, figure);
                    }
                    else if (roll <= 10) // Rolls 6-10 (25% chance) Offers to sell a potion
                    {
                        SlowWrite("\"I'll sell you a health potion if you need it friend,\" he says, profering you a potion.");
                        Item healthPotion = new Item("Health Potion", "A swirling red liquid that restores 30 health.", healingAmount: 30, isConsumable: true);
                        Pause();
                        Console.Write("Do you want to buy the health potion for 10 gold? (yes/no): ");
                        string response = Console.ReadLine()?.Trim().ToLower() ?? "no";
                        if (response == "yes")
                        {
                            if (player.SpendGold(10))
                            {
                                player.AddItemToInventory(healthPotion);
                                Console.WriteLine("You purchased the health potion.");
                            }
                            else
                            {
                                Console.WriteLine("You don't have enough gold to buy the potion.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("You declined the offer.");
                        }
                    }
                    else if (roll <= 18) // Rolls 11-18 (40% chance)
                    {
                        SlowWrite("\"You look strong,\" he says, and continues on his way.");

                    }
                    else // Rolls 19-20 (10% chance)
                    {
                        SlowWrite("Without a word, he hands you a strange, glowing stone.");
                        player.AddItemToInventory(new Item("Glowing Stone", "A stone that pulses with raw power.", strengthBonus: 2));
                    }
                    break;
                default: // High health
                    SlowWrite("He returns his gaze to the road and continues on his way.");
                    break;
            }
        }
        // Menu methods
        static void ShowInventoryMenu(Character player)
        {
            Console.WriteLine($"\n--- {player.Name}'s Inventory ---");
            bool hasItems = player.DisplayInventory();
            if (hasItems)
            {
                Console.Write("Enter the number of the item to use, or '0' to go back: ");
                if (int.TryParse(Console.ReadLine(), out int itemNumber) && itemNumber > 0)
                {
                    // Adjust for 0-based array index
                    player.UseItem(itemNumber - 1);
                }
            }
        }

        static bool ShowCrossroadsMenu(Character player)
        {
            Console.WriteLine($"\n--- Crossroads of Adventure ---");
            Console.WriteLine($"Health: {player.CurrentHealth:F1}/{player.MaxHealth:F1} | Strength: {player.TotalStrength}");
            Console.WriteLine("You find yourself at a crossroads. What do you do?");
            Console.WriteLine("1. Go see a dragon.");
            Console.WriteLine("2. Enter the goblin cave.");
            Console.WriteLine("3. Stand still and watch the road.");
            Console.WriteLine("4. Check Inventory.");
            Console.WriteLine("5. Quit Game");
            Console.Write("Enter your choice: ");

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                switch (choice)
                {
                    case 1:
                        EncounterDragon(player);
                        break;
                    case 2:
                        EncounterGoblin(player);
                        break;
                    case 3:
                        MysteriousFigure(player);
                        break;
                    case 4:
                        ShowInventoryMenu(player);
                        break;
                    case 5:
                        return false; // Exit the game loop
                    default:
                        Console.WriteLine("Invalid choice. Please select a valid option.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a number.");
            }
            return true; // Signal to continue the game
        }

        static void Main(string[] args) // Entry point of the game
        {
            SlowWrite("Welcome to Jacks Week1 Special Project!");
            SlowWrite("This is an Adventure RPG!");
            Console.Write("Enter your character's name (press Enter for default): ");
            string playerNameInput = Console.ReadLine() ?? string.Empty;
            string playerName = string.IsNullOrEmpty(playerNameInput) ? "Player1" : playerNameInput;
            Character player = new Character(playerName);
            SlowWrite($"Hello, {player.Name}! Your adventure begins now.");

            bool gameIsRunning = true;
            while (gameIsRunning && player.IsAlive())
            {
                gameIsRunning = ShowCrossroadsMenu(player);
            }
            if (!player.IsAlive())
            {
                SlowWrite("\nYou have fallen. Your adventure ends here.");
            }
            SlowWrite("Thank you for playing the Game!");
        }
    }
}