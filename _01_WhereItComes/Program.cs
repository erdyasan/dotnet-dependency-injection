while (true)
{
    ShowMenu();

    var choice = Console.ReadLine();

    if (choice == "0")
    {
        break;
    }

    if (choice == "1")
    {
        Console.Write("email: ");
        var email = Console.ReadLine() ?? string.Empty;

        Console.Write("display name: ");
        var displayName = Console.ReadLine() ?? string.Empty;

        var users = new UserRepository(new TableResolver());

        users.Save(email, displayName);
        Console.WriteLine("saved");
    }
    else if (choice == "2")
    {
        Console.Write("line number: ");

        if (!int.TryParse(Console.ReadLine(), out var lineNumber))
        {
            Console.WriteLine("line number must be a number");
            continue;
        }

        var users = new UserRepository(new TableResolver());

        Console.WriteLine(users.Read(lineNumber) ?? "no user on that line");
    }
    else
    {
        Console.WriteLine("unknown option");
    }
}

void ShowMenu()
{
    Console.WriteLine();
    Console.WriteLine("1) register user");
    Console.WriteLine("2) read user");
    Console.WriteLine("0) exit");
    Console.Write("> ");
}
