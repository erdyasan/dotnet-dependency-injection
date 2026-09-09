using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddSingleton<Translator>();

var provider = services.BuildServiceProvider();

while (true)
{
    using var scope = provider.CreateScope();

    var translator = scope.ServiceProvider.GetRequiredService<Translator>();

    if (translator.Language.Length == 0)
    {
        ChooseLanguage(translator);
    }

    Console.WriteLine();
    Console.WriteLine($"1) {translator.Get("menu.register")}");
    Console.WriteLine($"2) {translator.Get("menu.language")}");
    Console.WriteLine($"0) {translator.Get("menu.exit")}");
    Console.Write($"{translator.Get("prompt.choice")} > ");

    var choice = Console.ReadLine();

    if (choice == "0")
    {
        break;
    }

    if (choice == "2")
    {
        ChooseLanguage(translator);

        continue;
    }

    if (choice == "1")
    {
        Console.Write($"{translator.Get("prompt.email")} > ");

        var email = Console.ReadLine();

        Console.WriteLine($"{email} -> {translator.Get("saved")}");

        continue;
    }

    Console.WriteLine(translator.Get("error.choice"));
}

void ChooseLanguage(Translator translator)
{
    Console.WriteLine();
    Console.WriteLine("1) English");
    Console.WriteLine("2) Türkçe");
    Console.Write("> ");

    translator.Use(Console.ReadLine() == "2" ? "tr" : "en");
}
