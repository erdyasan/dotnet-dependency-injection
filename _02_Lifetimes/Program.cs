using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

var provider = services.BuildServiceProvider();

Console.WriteLine("root provider ready");
