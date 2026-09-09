using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddScoped<Translator>();

var provider = services.BuildServiceProvider();

while (true)
{
    Console.Write("message key (0 = exit) > ");

    var key = Console.ReadLine();

    if (key == "0")
    {
        break;
    }

    using var scope = provider.CreateScope();

    var translator = scope.ServiceProvider.GetRequiredService<Translator>();

    Console.WriteLine(translator.Get(key ?? string.Empty));
}
