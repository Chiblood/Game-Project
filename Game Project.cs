/* Week1 Special Project.cs
    This is a special project for Week 1 of the course. Create a short story with user interactive input.
    Author note: Yeah I went overboard on this one. But hey! It was fun! */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading;

namespace GameProject
{
    class Game_Project
    {
        // Readability helper methods
        public static void Pause() // Utility method to pause the console
        {
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }
        public static void SlowWrite(string text, int delay = 10) // Default delay of 10ms
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
        private static bool beatGoblin = false;
        private static bool learnedDragonStory = false;
        private static bool silentDragonEncounter = false;
        private static int figureEncounters = 0;

        // Encounter methods
        private static void EncounterDragon(Character player) // Main Boss of the Game
        {
            dragonVisits++; // Increment the visit counter
            Console.WriteLine("\nYou chose to go towards the massive dragon cave...");
            Console.WriteLine();

            if (dragonVisits == 1)
            {
                SlowWrite("You encounter a dragon. He breathes fire into the air and words rumble from deep inside his chest.");
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
                        Console.WriteLine("He seems pleased by your directness and drops a small, shimmering object at your feet.");
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
                if (learnedDragonStory)
                {
                    Console.WriteLine("3. 'I have come to release you from this nightmare.' and attack the dragon.");
                }
                else
                {
                    Console.WriteLine("3. (Attack the dragon)");
                }
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine() ?? "1";
                switch (choice) // After First Meeting Choices
                {
                    case "1": // Ask for his story
                        SlowWrite("~~~\"Ah, a curious one,\" the dragon rumbles, his voice like grinding stones. \"They call me Ignis. I was not always a prisoner of this damp cave.\"");
                        SlowWrite("\"Ages ago, I guarded a sacred treasure of a deity. A jealous mage, who was unable to defeat me in open combat, laid a curse upon me and trapped me in this cave for eternity.\"");
                        SlowWrite("\"I am bound here in torment until I am slain!\"~~~");
                        learnedDragonStory = true;
                        Pause();
                        break;
                    case "2": // Stay silent
                        if (silentDragonEncounter)
                        {
                            SlowWrite("\"~~~AGAIN you come in silence!?~~~\" he rumbles, his voice thick with menace. \"~~~THIS IS YOUR LAST CHANCE. SPEAK OR FACE MY WRATH!~~~\"");
                            Pause();
                        }
                        silentDragonEncounter = true; // Mark that the player has chosen silence again
                        Console.WriteLine("1. \"My apologies, great one. I wish to hear your story.\"");
                        Console.WriteLine("2. (Stay silent)");
                        Console.Write("Enter your choice: ");
                        string silentChoice = Console.ReadLine() ?? "1";
                        if (silentChoice == "1")
                        {
                            SlowWrite("~~~\"Hmph. Very well,\" he snorts, a puff of smoke escaping his nostrils. \"They call me Ignis...\"");
                            SlowWrite("He seems mollified and proceeds to tell you his tale, though his gaze remains burning on you.");
                            SlowWrite("His voice like grinding stones. \"They call me Ignis. I was not always a prisoner of this damp cave.\"");
                            SlowWrite("\"Ages ago, I guarded a sacred treasure of a deity. A jealous mage, who was unable to defeat me in open combat, laid a curse upon me and trapped me in this cave for eternity.\"");
                            SlowWrite("\"I am bound here in torment until I am slain.\"~~~");
                            Pause();
                        }
                        else
                        {
                            SlowWrite("\"Your silence is your answer. You are found wanting!\" With a roar, he blasts you with a torrent of fire.");
                            player.CurrentHealth = 0; // Player dies
                        }
                        break;
                    case "3": // Attack the dragon
                        if (learnedDragonStory)
                        {
                            SlowWrite($"~~~\"Thank you {player.Name}!~~~\" the Ignis roars, and meets you in battle.");
                        }
                        else
                        {
                            SlowWrite("The Dragon Roars and meets you in Battle.");
                        }
                        BattleManager.StartCombat(player, new Character("Dragon") { BaseStrength = 10, CurrentHealth = 200 });
                        break;
                    default:
                        SlowWrite("\"You hesitate, and the moment is lost,\" the dragon says, clearly annoyed. He turns away, uninterested in you.");
                        return;
                }
            }
        }
        private static void EncounterGoblin(Character player) // Easy Mob Battle
        {
            SlowWrite("\nYou chose to enter the dark cave...");
            // Create an enemy using the same Character class
            Character goblin = new Character("Goblin")
            {
                BaseStrength = 3,
                CurrentHealth = 30
            };
            bool playerWon = BattleManager.StartCombat(player, goblin);

            if (playerWon)
            {
                SlowWrite($"\nYou have defeated the {goblin.Name} and picked up his weapon and coin pouch!");
                Item goblinLoot = new Item("Crude Dagger", "A rusty but sharp dagger.", strengthBonus: 1);
                player.AddItemToInventory(goblinLoot);
                beatGoblin = true;
                player.AddGold(20);
            }
        }
        private static void MysteriousFigure(Character player) // Random Encounter
        {
            SlowWrite("A mysterious traveler approaches you on the path. He looks you up and down...");
            figureEncounters++; // Increment the encounter counter at the start for simpler logic.

            // After 2 encounters, the traveler is different and the interaction changes.
            if (figureEncounters > 2)
            {
                HandleNewFigureEncounter(player);
            }
            else // First two encounters use the original logic
            {
                HandleFirstFigureEncounters(player);
            }
            Pause();
        }

        // Helper method to break up the logic of the MysteriousFigure encounter
        private static void HandleFirstFigureEncounters(Character player)
        {
            if (player.CurrentHealth <= player.MaxHealth * 0.3) // Low health (30% or less)
            {
                SlowWrite("\"You look like you're in rough shape,\" he says, performing a quick ritual.");
                double healthRestored = player.MaxHealth * 0.5; // Restore 50% of max health
                player.CurrentHealth += healthRestored;
                player.CurrentHealth = Math.Min(player.CurrentHealth, player.MaxHealth); // Clamp health
                Console.WriteLine($"You feel a surge of vitality! Your health is now {player.CurrentHealth:F1}.");
            }
            else if (player.CurrentHealth <= player.MaxHealth * 0.80) // Medium health
            {
                SlowWrite("Fate will decide your path.");
                int roll = BattleManager.RollDice(20); // d20 roll
                if (roll <= 5) // 25% chance: Attacks the player
                {
                    SlowWrite("\"You look a bit weak,\" he says with a hungry look in his eyes... He attacks!");
                    Character figure = new Character("Mysterious Figure") { BaseStrength = 8, CurrentHealth = 50 };
                    if (BattleManager.StartCombat(player, figure))
                    {
                        SlowWrite("\"You have defeated the mysterious figure and taken his belongings!\"");
                        Item figureLoot = new("Health Potion", "A swirling red liquid that restores 30 health.", healingAmount: 30, isConsumable: true);
                        player.AddItemToInventory(figureLoot);
                        player.AddGold(15);
                    }
                }
                else if (roll <= 10) // 25% chance: Offers to sell a potion
                {
                    SlowWrite("\"I'll sell you a health potion if you need it friend,\" he says, profering a potion.");
                    Item healthPotion = new Item("Health Potion", "A swirling red liquid that restores 30 health.", healingAmount: 30, isConsumable: true);
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
                else if (roll <= 18) // 40% chance: Compliments the player
                {
                    SlowWrite("\"You look strong,\" he says, and continues on his way.");
                }
                else // 10% chance: Gives the player a strength-boosting item
                {
                    SlowWrite("Without a word, he hands you a strange, glowing stone.");
                    player.AddItemToInventory(new Item("Glowing Stone", "A stone that pulses with raw power.", strengthBonus: 2));
                }
            }
            else // High health
            {
                SlowWrite("He returns his gaze to the road and continues on his way.");
            }
        }

        // Helper method for the third+ encounter with the Mysteriousfigure
        private static void HandleNewFigureEncounter(Character player)
        {
            SlowWrite("A new figure is at the crossroads. What will you do?");
            Console.WriteLine("1. Greet the new figure.");
            Console.WriteLine("2. Attack him.");
            Console.WriteLine("3. Ignore him.");
            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine() ?? "3";

            if (choice == "1")
            {
                // This logic is similar to the first encounters, but could be changed later.
                HandleFirstFigureEncounters(player);
            }
            else if (choice == "2")
            {
                SlowWrite("You attack the figure without warning!");
                Character figure = new Character("Mysterious Figure") { BaseStrength = 8, CurrentHealth = 50 };
                if (BattleManager.StartCombat(player, figure))
                {
                    SlowWrite("You have defeated the mysterious figure!");
                    player.AddGold(15);
                }
            }
            else
            {
                SlowWrite("You ignore the figure, and they watch you pass in silence.");
            }
        }
        private static void TownShop(Character player) // Shop Menu
        {
            SlowWrite("\nYou enter the town shop. The shopkeeper greets you warmly.");
            bool shopping = true;
            while (shopping)
            {
                Console.WriteLine($"\nYour Gold: {player.Gold}");
                Console.WriteLine("Shop Inventory:");
                Console.WriteLine("1. Health Potion (Restores 30 health) - 30 Gold");
                Console.WriteLine("2. Full Heal - 100 Gold");
                Console.WriteLine("3. Exit Shop");
                // Console.WriteLine("4. Talk to the shopkeeper.");
                Console.Write("Enter the number of the item you want to buy, or '3' to exit: ");
                string choice = Console.ReadLine() ?? "3";
                switch (choice)
                {
                    case "1":
                        if (player.SpendGold(30))
                        {
                            Item healthPotion = new Item("Health Potion", "A swirling red liquid that restores 30 health.", healingAmount: 30, isConsumable: true);
                            player.AddItemToInventory(healthPotion);
                            Console.WriteLine("You purchased a Health Potion.");
                        }
                        else
                        {
                            Console.WriteLine("You don't have enough gold to buy that.");
                        }
                        break;
                    case "2":
                        if (player.SpendGold(100))
                        {
                            player.Heal(player.MaxHealth); // Fully heal the player
                            Console.WriteLine("You were fully healed.");
                        }
                        else
                        {
                            Console.WriteLine("You don't have enough gold to buy that.");
                        }
                        break;
                    case "3":
                        shopping = false; // Exit the shop
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please select a valid option.");
                        break;
                }
            }
            SlowWrite("You leave the shop and return to the crossroads.");
        }
        // Menu methods
        static void ShowInventoryMenu(Character player)
        {
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
            Pause();
        }
        static bool ShowCrossroadsMenu(Character player)
        {
            Console.WriteLine($"\n--- Crossroads of Adventure ---");
            Console.WriteLine($"Health: {player.CurrentHealth:F1}/{player.MaxHealth:F1} | Strength: {player.TotalStrength}");
            Console.WriteLine("You find yourself at a crossroads. What do you do?");
            if (learnedDragonStory)
            {
                Console.WriteLine("1. Go see Igis the dragon.");
            }
            else Console.WriteLine("1. Go towards a dragon cave.");

            if (beatGoblin)
            {
                Console.WriteLine("2. Return to the goblin cave."); ;
            }
            else
            {
                Console.WriteLine("2. Go into a strange monster cave.");
            }
            Console.WriteLine("3. Guard the crossroads.");
            Console.WriteLine("4. Check Inventory.");
            Console.WriteLine("5. Go to the town shop.");
            Console.WriteLine("6. Quit Game");
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
                        TownShop(player);
                        break;
                    case 6:
                        player.Save(); // Save the game before quitting
                        SlowWrite("Game saved. Goodbye!");
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

        private static Character CreateNewCharacter()
        {
            Console.WriteLine("\n--- Character Creation ---");
            Console.Write("Please enter your character's name (or press Enter for 'Player1'): ");
            string playerNameInput = Console.ReadLine() ?? string.Empty;
            string playerName = string.IsNullOrEmpty(playerNameInput) ? "Player1" : playerNameInput;

            Character player = new Character(playerName);
            player.AddItemToInventory(new Item("Health Potion", "A swirling red liquid that restores 30 health.", healingAmount: 30, isConsumable: true)); // Starting item
            player.Save(); // Save the newly created character

            SlowWrite($"\nHello, {player.Name}! Your adventure begins now.");
            return player;
        }

        static void Main(string[] args) // Entry point of the game
        {
            Console.Clear();
            SlowWrite("Welcome to Jacks Adventure RPG Game Project! This is a work in progress!");
            Pause();

            Character? player = null;
            string saveDataPath = Path.Combine(AppContext.BaseDirectory, "SaveData");
            string[] saveFiles = Directory.Exists(saveDataPath) ? Directory.GetFiles(saveDataPath, "*_CharacterData.txt") : Array.Empty<string>();

            if (saveFiles.Length > 0)
            {
                Console.WriteLine("\nSaved games found:");
                for (int i = 0; i < saveFiles.Length; i++)
                {
                    string characterName = Path.GetFileNameWithoutExtension(saveFiles[i]).Replace("_CharacterData", "");
                    Console.WriteLine($"{i + 1}. {characterName}");
                }
                Console.WriteLine("N. Start a New Game");
                Console.Write("Choose a save to load, or start a new game: ");

                string choice = Console.ReadLine()?.Trim().ToUpper() ?? "N";
                if (int.TryParse(choice, out int fileIndex) && fileIndex > 0 && fileIndex <= saveFiles.Length)
                {
                    player = Character.Load(saveFiles[fileIndex - 1]);
                    if (player != null)
                    {
                        SlowWrite($"\nWelcome back, {player.Name}!");
                    }
                }
            }

            // If player is still null (no saves, chose new game, or load failed), create a new character.
            if (player == null)
            {
                player = CreateNewCharacter();
            }

            bool gameIsRunning = true;
            // The player object could theoretically still be null if creation fails, so we check.
            while (gameIsRunning && player != null && player.IsAlive())
            {
                gameIsRunning = ShowCrossroadsMenu(player);
            }
            if (player != null && !player.IsAlive())
            {
                SlowWrite("\nYou have fallen. Your adventure ends here.");
            }
            SlowWrite("Thank you for playing the Game!");
        }
    }
}