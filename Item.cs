// One item on the shopping list.
class Item
{
    // No setters, so an item cannot be changed into something invalid later.
    public string Name { get; }
    public int Price { get; }

    // Refuses to create an item with an empty name or a negative price.
    public Item(string name, int price)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Namnet får inte vara tomt.", nameof(name));
        }

        if (price < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(price), "Priset får inte vara negativt.");
        }

        Name = name;
        Price = price;
    }

    public override string ToString()
    {
        return $"{Name} - {Price} kr";
    }
}