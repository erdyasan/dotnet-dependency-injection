using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddSingleton<Translator>();
services.AddScoped<OperationLogger>();
services.AddScoped<FieldValidator>();

var provider = services.BuildServiceProvider();

while (true)
{
    using var scope = provider.CreateScope();

    var translator = scope.ServiceProvider.GetRequiredService<Translator>();
    var log = scope.ServiceProvider.GetRequiredService<OperationLogger>();

    if (translator.Language.Length == 0)
    {
        ChooseLanguage(translator);

        log.Add($"language selected: {translator.Language}");
    }

    Console.WriteLine();
    Console.WriteLine($"1) {translator.Get("menu.register")}");
    Console.WriteLine($"2) {translator.Get("menu.language")}");
    Console.WriteLine($"0) {translator.Get("menu.exit")}");
    Console.Write($"{translator.Get("prompt.choice")} > ");

    var choice = Console.ReadLine();

    if (choice == "0")
    {
        log.Add("exit requested");

        break;
    }

    if (choice == "2")
    {
        ChooseLanguage(translator);

        log.Add($"language changed: {translator.Language}");

        continue;
    }

    if (choice == "1")
    {
        Console.WriteLine(translator.Get("hint.email.exit"));

        while (true)
        {
            Console.Write($"{translator.Get("prompt.name")} > ");

            var name = Console.ReadLine();

            if (name == "=exit")
            {
                break;
            }

            Console.Write($"{translator.Get("prompt.email")} > ");

            var email = Console.ReadLine();

            var validator = scope.ServiceProvider.GetRequiredService<FieldValidator>();

            validator.AddName(name ?? string.Empty);
            validator.AddEmail(email ?? string.Empty);

            if (validator.Errors.Count > 0)
            {
                foreach (var error in validator.Errors)
                {
                    Console.WriteLine($"  ! {translator.Get(error)}");
                }

                log.Add($"user not added: {name} / {email} -> {string.Join(", ", validator.Errors)}");

                continue;
            }

            Console.WriteLine(translator.Get("saved"));

            log.Add($"user added: {name} / {email}");
        }

        continue;
    }

    Console.WriteLine(translator.Get("error.choice"));

    log.Add($"unknown option: {choice}");
}

void ChooseLanguage(Translator translator)
{
    Console.WriteLine();
    Console.WriteLine("1) English");
    Console.WriteLine("2) Türkçe");
    Console.Write("> ");

    translator.Use(Console.ReadLine() == "2" ? "tr" : "en");
}
