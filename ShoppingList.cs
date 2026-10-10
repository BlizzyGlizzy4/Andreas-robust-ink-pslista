// Holds the items and takes care of loading and saving them.
class ShoppingList
{
    private List<Item> items = new List<Item>();
   private string path;

    // The most the whole list may cost, in kronor.
    // Only Load() may change it, when the file has a saved budget.
    
    public int Budget { get; private set; }

    public ShoppingList(string path, int budget)
    {
        this.path = path;
        Budget = budget;
    }

    // Adds the item if the total stays within the budget.
    // Returns false if the item would make the list too expensive.
    // Compares with what is left of the budget, so a huge price cannot
    // overflow the sum and slip past the check.
    public bool Add(Item item)
    {
        if (item.Price > Budget - Total())
        {
            return false;
        }

        items.Add(item);
        return true;
    }

    // Removes the item the user sees as number 1, 2, 3 ...
    public bool RemoveAt(int number)
    {
        if (number < 1 || number > items.Count)
        {
            return false;
        }

        items.RemoveAt(number - 1);
        return true;
    
       
}

    // Adds up the price of every item on the list.
    public int Total()
    {
        int sum = 0;

        for (int i = 0; i < items.Count; i++)
        {
            sum += items[i].Price;
        }

        return sum;
    }

    // Looks up an item by its name. Returns null if there is no such item.
    // Upper/lower case and spaces around the name do not matter.
    public Item Find(string name)
    {
        string wanted = name.Trim();

        foreach (Item item in items)
        {
            if (string.Equals(item.Name, wanted, StringComparison.OrdinalIgnoreCase))
            {
                return item;
            }
        }

        return null;
    }

    public void Print()
    {
        for (int i = 0; i < items.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {items[i]}");
        }

        Console.WriteLine($"Totalt: {Total()} kr");
    }

    // Writes the budget on the first line, as "budget;amount",
    // and then one item per line, as "price;name".
    public void Save()
    {
        List<string> lines = new List<string>();
        lines.Add($"budget;{Budget}");

        foreach (Item item in items)

        {
            lines.Add($"{item.Price};{item.Name}");
        }

        try
        {
            File.WriteAllText(path, string.Join("\r\n", lines) + "\r\n");
            Console.WriteLine("Listan är sparad.");
        }
        catch(IOException ex)
        {
              Console.WriteLine("Kunde inte spara listan. " + ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine("Har inte behörighet att spara listan. " + ex.Message);
        }

    }

    // Reads the file back into the list.
    public void Load()
    {
       if (!File.Exists(path))
        {
            Console.WriteLine("Hittade ingen sparad lista. Börjar med en tom lista");
            return;
        }    
    
    string[] lines = File.ReadAllLines(path);

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            
                       string[] parts = line.Split(';');

            // The budget line replaces the budget from the constructor.
            if (parts[0] == "budget")
            {
                if (parts.Length == 2 && int.TryParse(parts[1], out int savedBudget) && savedBudget >= 0)
                {
                    Budget = savedBudget;
                }
                else
                {
                    Console.WriteLine("Budgeten i filen går inte att läsa. Använder " + Budget + " kr.");
                }

                continue;
            }

            if (parts.Length != 2 || !int.TryParse(parts[0], out int price))

            {
                Console.WriteLine("Hoppar över en rad som inte går att läsa: " + line);
                continue;
            }

            try
            {
                items.Add(new Item(parts[1], price));
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Hoppar över en ogiltig vara: " + line);
            }
        }
    }
}
