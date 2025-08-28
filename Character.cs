class Character
{
    // Character attributes
    private string name = string.Empty; //backing field for Name property
    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public int BaseStrength
    { get; set; }
    public double CurrentHealth
    { get; set; }
    public int Gold
    { get; private set; }
    public Item?[] Inventory
    { get; set; }
    private const double BaseHealth = 100.0;

    // Calculated property for total strength including item bonuses
    public int TotalStrength
    {
        get
        {
            // Use LINQ to sum the StrengthBonus from all items in inventory
            int bonus = Inventory.Where(item => item != null).Sum(item => item?.StrengthBonus ?? 0);
            return BaseStrength + bonus;
        }
    }
    // Calculated property for max health including item bonuses
    public double MaxHealth
    {
        get
        {
            double bonus = Inventory.Where(item => item != null).Sum(item => item?.HealthBonus ?? 0);
            return BaseHealth + bonus;
        }
    }
    // New Character Constructor
    public Character(string name)
    {
        Name = name;
        BaseStrength = 5; // Initialize base strength
        CurrentHealth = BaseHealth;
        Gold = 100; // Starting gold
        Inventory = new Item[10]; // Initialize an array with 10 empty item slots
    }

    //  Character Methods
    public bool IsAlive()
    {
        return CurrentHealth > 0;
    }
    public void TakeDamage(double damageAmount)
    {
        CurrentHealth -= damageAmount;
        if (CurrentHealth < 0)
        {
            CurrentHealth = 0;
        }
    }
    public void Heal(double healAmount)
    {
        CurrentHealth += healAmount;
        if (CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
    }

    public void AddGold(int amount)
    {
        Gold += amount;
    }

    public bool SpendGold(int amount)
    {
        if (Gold >= amount)
        {
            Gold -= amount;
            return true; // Transaction successful
        }
        return false; // Not enough gold
    }
    // Inventory Methods
    public void AddItemToInventory(Item item)
    {
        for (int i = 0; i < Inventory.Length; i++)
        {
            // Find the first empty slot in the inventory
            if (Inventory[i] == null)
            {
                Inventory[i] = item;
                Console.WriteLine($"You added [{item.Name}] to your inventory.");
                return; // Exit after adding the item
            }
        }
        // This message will show if the loop finishes without finding an empty slot
        Console.WriteLine("Your inventory is full! You can't carry any more items.");
    }
    public void RemoveItemFromInventory(Item item)
    {
        for (int i = 0; i < Inventory.Length; i++)
        {
            if (Inventory[i] == item)
            {
                Inventory[i] = null; // Remove the item by setting the slot to null
                Console.WriteLine($"You removed [{item.Name}] from your inventory.");
                return; // Exit after removing the item
            }
        }
        Console.WriteLine($"Item [{item.Name}] not found in inventory.");
    }
    public bool DisplayInventory(Character character)
    {
        Console.WriteLine($"--- {character.Name}'s Inventory ---");
        bool empty = true;
        for (int i = 0; i < character.Inventory.Length; i++)
        {
            if (character.Inventory[i] != null)
            {
                if (character.Inventory[i] != null)
                {
                    Console.WriteLine($"{i + 1}. {character.Inventory[i]!.Name}: {character.Inventory[i]!.Description}");
                }
                empty = false;
            }
        }
        if (empty)
        {
            Console.WriteLine("(Your inventory is empty)");
        }
        Console.WriteLine("------------------------");
        return !empty; // Return true if inventory has items, false if empty.
    }
    public void UseItem(Character character, int inventoryIndex)
    {
        // Validate index
        if (inventoryIndex < 0 || inventoryIndex >= character.Inventory.Length)
        {
            Console.WriteLine("Invalid selection.");
            return;
        }

        Item itemToUse = character.Inventory[inventoryIndex]!;

        if (itemToUse == null)
        {
            Console.WriteLine("There is no item in that slot.");
            return;
        }

        // Check if it's a healing item
        if (itemToUse.HealingAmount > 0)
        {
            Console.WriteLine($"You use the {itemToUse.Name}.");
            character.Heal(itemToUse.HealingAmount);
            Console.WriteLine($"You restore {itemToUse.HealingAmount} health. Current health: {character.CurrentHealth:F1}/{character.MaxHealth:F1}");

            if (itemToUse.IsConsumable)
            {
                character.Inventory[inventoryIndex] = null; // Remove the item by setting the slot to null
            }
        }
        else
        {
            Console.WriteLine($"You can't use the {itemToUse.Name} that way.");
        }
    }
    // Combat Methods
    public void Attack(Character target)
    {
        int damage = BattleManager.RollDice(6) + TotalStrength;
        Console.WriteLine($"{Name} attacks {target.Name} for {damage} damage!");
        target.TakeDamage(damage);
    }
}