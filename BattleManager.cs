class BattleManager
{
    public static void Pause() // Utility method to pause the console
    {
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }
    internal static Random rand = new Random(); // Random number generator
    /// <summary>
    /// Rolls a dice with the specified number of sides.
    /// </summary>
    /// <param name="sides">The number of sides on the dice.</param>
    /// <returns>A random number between 1 and the specified number of sides.</returns>
    public static int RollDice(int sides)
    {
        return rand.Next(1, sides + 1);
    }
    /// <summary>
    /// Manages a turn-based combat loop between two characters.
    /// </summary>
    /// <returns>True if the player wins, false otherwise (loss or flee).</returns>
    /// <param name="player">The player character.</param>
    /// <param name="enemy">The enemy character.</param>
    public static bool StartCombat(Character player, Character enemy)
    {
        Console.WriteLine($"A {enemy.Name} attacks!");

        // Main combat loop
        while (player.IsAlive() && enemy.IsAlive())
        {
            Console.WriteLine("\n--- Combat ---");
            Console.WriteLine($"{player.Name}: {player.CurrentHealth:F1} HP | {enemy.Name}: {enemy.CurrentHealth:F1} HP");
            Console.WriteLine("1. Attack");
            Console.WriteLine("2. Flee");
            Console.WriteLine("3. Use Item");
            Console.Write("Action: ");

            if (int.TryParse(Console.ReadLine(), out int action))
            {
                switch (action)
                {
                    case 1: // Attack
                        player.Attack(enemy);
                        if (enemy.IsAlive())
                        {
                            enemy.Attack(player);
                        }
                        Pause();
                        break;
                    case 2: // Flee
                        Console.WriteLine("You try to flee...");
                        Pause();
                        if (BattleManager.RollDice(20) > 10) // 50% chance to flee
                        {
                            Console.WriteLine("You successfully escaped!");
                            return false; // Exit the combat method
                        }
                        Console.WriteLine("You failed to escape! The enemy attacks.");
                        enemy.Attack(player);
                        break;
                    case 3: // Use Item
                        player.DisplayInventory(player);
                        Console.Write("Enter the number of the item to use: ");
                        player.UseItem(player, int.Parse(Console.ReadLine() ?? "0") - 1);
                        // Using an item consumes a turn, so the enemy gets to attack.
                        if (enemy.IsAlive())
                        {
                            enemy.Attack(player);
                        }
                        break;
                    default:
                        Console.WriteLine("Invalid action. You hesitate and the enemy attacks!");
                        enemy.Attack(player);
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid input. You fumble and the enemy attacks!");
                enemy.Attack(player);
            }
        }
        if (player.IsAlive())
        {
            Console.WriteLine($"{player.Name} wins the battle!");
            return true; // Player wins
        }
        else
        {
            return false; // Player has been defeated
        }
    }
}
