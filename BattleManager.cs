class BattleManager
{
    internal static Random rand = new Random();
    public static int RollDice(int sides)
    {
        return rand.Next(1, sides + 1);
    }
    public static void StartCombat(Character player, Character enemy)
    {
        Console.WriteLine($"A {enemy.Name} attacks!");

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
                        break;
                    case 2: // Flee
                        Console.WriteLine("You try to flee...");
                        if (BattleManager.RollDice(20) > 10) // 50% chance to flee
                        {
                            Console.WriteLine("You successfully escaped!");
                            return; // Exit the combat method
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
    }
    public static void Attack(Character attacker, Character target)
    {
        int damage = RollDice(6) + attacker.TotalStrength;
        // The BattleManager orchestrates, but the Character handles its own state change.
        target.TakeDamage(damage);
        Console.WriteLine($"{attacker.Name} attacks {target.Name} for {damage} damage!");
    }
}
