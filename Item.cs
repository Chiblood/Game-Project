class Item
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int StrengthBonus { get; set; }
    public double HealthBonus { get; set; }
    public double HealingAmount { get; set; }
    public bool IsConsumable { get; set; }

    // Constructor with optional parameters for bonuses
    public Item(string name, string description, int strengthBonus = 0, double healthBonus = 0, double healingAmount = 0, bool isConsumable = false)
    {
        Name = name;
        Description = description;
        StrengthBonus = strengthBonus;
        HealthBonus = healthBonus;
        HealingAmount = healingAmount;
        IsConsumable = isConsumable;
    }
}