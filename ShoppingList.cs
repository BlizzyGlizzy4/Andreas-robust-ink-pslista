// Holds the items and takes care of loading and saving them.
class ShoppingList
{
    private List<Item> items = new List<Item>();
   private string path;

    // The most the whole list may cost, in kronor.
    public int Budget { get; }

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
    public Item Find(string name)
    {
        foreach (Item item in items)
        {
            if (item.Name == name)
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

    // Writes one item per line, as "price;name".
    public void Save()
    {
        List<string> lines = new List<string>();

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

        Console.WriteLine("Listan är sparad.");
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
