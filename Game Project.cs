// Week1 Special Project.cs
// This is a special project for Week 1 of the course. Create a short story with user interactive input.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject
{
    class Game_Project
    {
        // private static void EncounterDragon(Character player)
        // {
        //     Console.WriteLine("\nYou chose to go left...");
        //     Console.WriteLine("You encounter a dragon. He breathes fire into the air and words rumble from deep inside his chest.");
        //     Console.WriteLine();
        //     Console.WriteLine("\"~~~Who are you?~~~~\" the dragon asks.");

        //     Console.WriteLine($"Your strength was: {player.TotalStrength}");
        //     // Create an item that gives a +3 strength bonus
        //     Item amulet = new Item("Dragon's Amulet", "A mystical amulet that hums with ancient power.", strengthBonus: 3);
        //     player.AddItemToInventory(amulet);
        //     Console.WriteLine($"Your strength is now: {player.TotalStrength}");
        // }

        private static void EncounterGoblin(Character player)
        {
            Console.WriteLine("\nYou chose to enter the dark cave...");
            // Create an enemy using the same Character class
            Character goblin = new Character("Goblin")
            {
                BaseStrength = 3,
                CurrentHealth = 30
            };
            BattleManager.StartCombat(player, goblin);

            if (player.IsAlive())
            {
                Console.WriteLine($"\nYou have defeated the {goblin.Name}!");
                Item goblinLoot = new Item("Crude Dagger", "A rusty but sharp dagger.", strengthBonus: 1);
                player.AddItemToInventory(goblinLoot);
            }
        }

        private static void MysteriousFigure(Character player)
        {
            Console.WriteLine("\nUnsure of where to go, you stand still...");
            Console.WriteLine("A mysterious traveler approaches you on the path. He looks you up and down...");

            // Use a switch with 'when' clauses for pattern matching based on health.
            switch (true)
            {

                case bool _ when player.CurrentHealth <= player.MaxHealth * 0.3: // Low health (30% or less)
                    Console.WriteLine("\"You look like you're in rough shape,\" he says, performing a quick ritual.");
                    double healthRestored = player.MaxHealth * 0.5; // Restore 50% of max health
                    player.CurrentHealth += healthRestored;
                    // Clamp health to the max health. Math.Min is a clean way to do this.
                    player.CurrentHealth = Math.Min(player.CurrentHealth, player.MaxHealth);
                    Console.WriteLine($"You feel a surge of vitality! Your health is now {player.CurrentHealth:F1}.");
                    break;

                case bool _ when player.CurrentHealth <= player.MaxHealth * 0.75: // Medium health
                    Console.WriteLine("\"You seem unsure of your path... perhaps fate will decide it.\"");
                    // Use the shared Random instance and an if/else if chain for clarity.
                    int roll = Character.rand.Next(1, 21); // d20 roll (1 to 20)

                    // The if/else-if chain ensures only one block is executed.
                    // The conditions are ordered from lowest roll to highest for clarity and correctness.
                    if (roll <= 5) // Rolls 1-5 (25% chance)
                    {
                        Console.WriteLine("\"You look a bit weak,\" he says with a hungry look in his eyes... He attacks!");
                        Character figure = new Character("Mysterious Figure") { BaseStrength = 8, CurrentHealth = 50 };
                        figure.Attack(player);
                    }
                    else if (roll <= 10) // Rolls 6-10 (25% chance)
                    {
                        Console.WriteLine("\"A gift, to aid your journey,\" he says, handing you a potion.");
                        Item healthPotion = new Item("Health Potion", "A swirling red liquid that restores 30 health.", healingAmount: 30, isConsumable: true);
                        player.EquipItem(healthPotion);
                    }
                    else if (roll <= 18) // Rolls 11-18 (40% chance)
                    {
                        Console.WriteLine("\"You look strong,\" he says, and continues on his way.");

                    }
                    else // Rolls 19-20 (10% chance)
                    {
                        Console.WriteLine("Without a word, he hands you a strange, glowing stone.");
                        player.EquipItem(new Item("Glowing Stone", "A stone that pulses with raw power.", strengthBonus: 2));
                    }
                    break;

                default: // High health
                    Console.WriteLine("He returns his gaze to the road and continues on his way.");
                    break;
            }
        }

        static void ShowInventoryMenu(Character player)
        {
            Console.WriteLine($"\n--- {player.Name}'s Inventory ---");
            bool hasItems = player.DisplayInventory(player);
            if (hasItems)
            {
                Console.Write("Enter the number of the item to use, or '0' to go back: ");
                if (int.TryParse(Console.ReadLine(), out int itemNumber) && itemNumber > 0)
                {
                    // Adjust for 0-based array index
                    player.UseItem(player, itemNumber - 1);
                }
            }
        }

        static bool ShowCrossroadsMenu(Character player)
        {
            Console.WriteLine($"\n--- Crossroads of Adventure ---");
            Console.WriteLine($"Health: {player.CurrentHealth:F1}/{player.MaxHealth:F1} | Strength: {player.TotalStrength}");
            Console.WriteLine("You find yourself at a crossroads. What do you do?");
            // Console.WriteLine("1. Go see a friendly dragon.");
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
                        // EncounterDragon(player);
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

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Jacks Week1 Special Project!");
            Console.WriteLine("This is an Adventure RPG!");
            Console.Write("Enter your character's name (press Enter for default): ");
            string playerNameInput = Console.ReadLine() ?? string.Empty;
            string playerName = string.IsNullOrEmpty(playerNameInput) ? "Player1" : playerNameInput;
            Character player = new Character(playerName);
            Console.WriteLine($"Hello, {player.Name}! Your adventure begins now.");

            bool gameIsRunning = true;
            while (gameIsRunning && player.IsAlive())
            {
                gameIsRunning = ShowCrossroadsMenu(player);
            }
            if (!player.IsAlive())
            {
                Console.WriteLine("\nYou have fallen. Your adventure ends here.");
            }
            Console.WriteLine("Thank you for playing the Game!");
        }
    }
}