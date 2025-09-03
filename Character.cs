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
    // New Character Constructor want to add default values for name
    public Character(string name)
    {
        Name = name;
        BaseStrength = 5; // Initialize base strength
        CurrentHealth = BaseHealth;
        Gold = 100; // Starting gold
        Inventory = new Item[10]; // Initialize an array with 10 empty item slots
    }
    public Character() : this("Player1") // Default name if none provided
    { }
    public void Save()
    {
        try
        {
            // Get the base directory where the application is running.
            // This makes the save data portable with the game's folder.
            string exePath = AppContext.BaseDirectory;
            string saveDataPath = Path.Combine(exePath, "SaveData");
            Directory.CreateDirectory(saveDataPath); // This ensures the directory exists.

            Console.WriteLine($"Your character's data will be saved in: {saveDataPath}");
            // Prevent repeated _CharacterData.txt in the filename
            string baseName = Name;
            string suffix = "_CharacterData.txt";
            if (baseName.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
            {
                baseName = baseName.Substring(0, baseName.Length - suffix.Length);
            }
            string fileName = $"{baseName}{suffix}";
            string fullPath = Path.Combine(saveDataPath, fileName);

            using (StreamWriter writer = new(fullPath))
            {
                writer.WriteLine($"Name: {Name}");
                writer.WriteLine($"Base Strength: {BaseStrength}");
                writer.WriteLine($"Current Health: {CurrentHealth}");
                writer.WriteLine($"Gold: {Gold}");
                // Save inventory items
                writer.WriteLine("[Inventory]"); // A header for the inventory section
                foreach (var item in Inventory.Where(i => i != null))
                {
                    // Save all item properties in a pipe-delimited format for robust loading.
                    writer.WriteLine($"{item!.Name}|{item.Description}|{item.StrengthBonus}|{item.HealingAmount}|{item.HealthBonus}|{item.IsConsumable}");
                }
            }
            Console.WriteLine($"Character data saved to: {fullPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while saving character data: {ex.Message}");
        }
    }
    public static Character Load(string name) // Factory method to load or create a character
    {
        try
        {
            string exePath = AppContext.BaseDirectory;
            string saveDataPath = Path.Combine(exePath, "SaveData");
            string fileName = $"{name}_CharacterData.txt";
            string fullPath = Path.Combine(saveDataPath, fileName);

            if (!File.Exists(fullPath))
            {
                Console.WriteLine("No saved data found for this character.");
                return new Character(name); // Return a new character if no save file exists
            }

            var lines = File.ReadAllLines(fullPath);
            var stats = new Dictionary<string, string>();
            var inventoryLines = new List<string>();
            bool readingInventory = false;

            // First pass: separate stats from inventory for robust parsing
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                if (line.Trim() == "[Inventory]")
                {
                    readingInventory = true;
                    continue;
                }

                if (readingInventory)
                {
                    inventoryLines.Add(line);
                }
                else
                {
                    var parts = line.Split(':', 2);
                    if (parts.Length == 2)
                    {
                        stats[parts[0].Trim()] = parts[1].Trim();
                    }
                }
            }

            // Second pass: build the character object from the parsed stats
            string characterName = stats.GetValueOrDefault("Name", name);
            Character character = new Character(characterName);

            character.BaseStrength = int.Parse(stats.GetValueOrDefault("Base Strength", "5"));
            character.CurrentHealth = double.Parse(stats.GetValueOrDefault("Current Health", "100.0"));
            character.Gold = int.Parse(stats.GetValueOrDefault("Gold", "0"));

            // Third pass: build the inventory from the parsed item lines
            foreach (var itemLine in inventoryLines)
            {
                var parts = itemLine.Split('|');
                if (parts.Length == 6) // Name|Desc|Str|Heal|HealthBonus|Consumable
                {
                    Item item = new Item(
                        name: parts[0],
                        description: parts[1],
                        strengthBonus: int.Parse(parts[2]),
                        healingAmount: double.Parse(parts[3]),
                        healthBonus: double.Parse(parts[4]),
                        isConsumable: bool.Parse(parts[5])
                    );
                    character.AddItemToInventory(item);
                }
            }
            Console.WriteLine($"Character data for '{character.Name}' loaded successfully.");
            return character;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while loading character data: {ex.Message}");
            // On failure, return a fresh character to prevent the game from crashing.
            return new Character(name);
        }
    }

    //  Character stat Methods
    public bool IsAlive() //returns true if CurrentHealth > 0
    {
        return CurrentHealth > 0;
    }
    public void TakeDamage(double damageAmount) // reduces CurrentHealth by damageAmount, not going below 0
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
        Console.WriteLine($"Gold: {character.Gold}");
        bool empty = true;
        for (int i = 0; i < character.Inventory.Length; i++)
        {
            Item? item = character.Inventory[i];
            if (item != null)
            {
                Console.WriteLine($"{i + 1}. {item.Name}: {item.Description}");
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