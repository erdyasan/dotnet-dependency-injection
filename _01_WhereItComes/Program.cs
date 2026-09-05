using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddScoped<TableResolver>();
services.AddScoped<UserRepository>();

var provider = services.BuildServiceProvider();

while (true)
{
    ShowMenu();

    var choice = Console.ReadLine();

    if (choice == "0")
    {
        break;
    }

    using var scope = provider.CreateScope();

    if (choice == "1")
    {
        Console.Write("email: ");
        var email = Console.ReadLine() ?? string.Empty;

        Console.Write("display name: ");
        var displayName = Console.ReadLine() ?? string.Empty;

        var users = scope.ServiceProvider.GetRequiredService<UserRepository>();

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

        var users = scope.ServiceProvider.GetRequiredService<UserRepository>();

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
