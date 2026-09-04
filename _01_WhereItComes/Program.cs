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

        RegisterUser(email, displayName);
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

        Console.WriteLine(ReadUser(lineNumber) ?? "no user on that line");
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

void RegisterUser(string email, string displayName)
{
    Directory.CreateDirectory("data");

    File.AppendAllText("data/users.csv", $"{email},{displayName}{Environment.NewLine}");
}

string? ReadUser(int lineNumber)
{
    if (!File.Exists("data/users.csv"))
    {
        return null;
    }

    var lines = File.ReadAllLines("data/users.csv");

    if (lineNumber < 1 || lineNumber > lines.Length)
    {
        return null;
    }

    return lines[lineNumber - 1];
}
