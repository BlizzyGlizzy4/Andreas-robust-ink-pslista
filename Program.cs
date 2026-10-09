ShoppingList list = new ShoppingList("items.txt", 500);
list.Load();

while (true)
{
    Console.WriteLine();
    list.Print();
    Console.WriteLine();
    Console.WriteLine("1. Lägg till vara");
    Console.WriteLine("2. Ta bort vara");
    Console.WriteLine("3. Spara");
    Console.WriteLine("4. Sök vara");
    Console.WriteLine("5. Avsluta");
    Console.Write("Välj: ");

   if (!int.TryParse(Console.ReadLine(), out int choice))
    {
        Console.WriteLine("Skriv en siffra mellan 1 och 5.");
        continue;
    }

    if (choice == 1)
    {
        Console.Write("Namn: ");
        string name = Console.ReadLine();
        Console.Write("Pris: ");
        if (!int.TryParse(Console.ReadLine(), out int price))
        {
            Console.WriteLine("Priset måste vara ett heltal.");
            continue;
        }

        Item item;

        try
        {
            item = new Item(name, price);
        }
        catch (ArgumentOutOfRangeException)
        {
            Console.WriteLine("Priset får inte vara negativt. Varan lades inte till.");
            continue;
        }
        catch (ArgumentException)
        {
            Console.WriteLine("Namnet får inte vara tomt. Varan lades inte till.");
            continue;
        }

        if (list.Add(item))
        {
            Console.WriteLine("Varan är tillagd.");
        }
        else
        {
            Console.WriteLine($"{item.Name} för {item.Price} kr ryms inte i budgeten på {list.Budget} kr. Varan lades inte till.");
        }
    }
    else if (choice == 2)
    {
        Console.Write("Nummer: ");
        if (!int.TryParse(Console.ReadLine(), out int number))
        {
            Console.WriteLine("Numret måste vara ett heltal.");
            continue;
        }

         if (list.RemoveAt(number))
        {
            Console.WriteLine("Varan är borttagen.");
        }
        else
        {
            Console.WriteLine("Det finns ingen vara med nummer " + number + ".");
        }
    }
    else if (choice == 3)
    {
        list.Save();
    }
    else if (choice == 4)
    {
        Console.Write("Namn att söka efter: ");
        string wanted = Console.ReadLine();
        Item found = list.Find(wanted);

        if (found == null)
        {
            Console.WriteLine("Varan finns inte i listan.");
        }
        else
        {
            Console.WriteLine($"Hittade: {found}");
        }
    }
    else if (choice == 5)
    {
        break;
    }
}
