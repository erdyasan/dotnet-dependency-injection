public class Translator
{
    private readonly Dictionary<string, string> _messages = new();

    public Translator()
    {
        Console.WriteLine("translator: reading messages.txt");

        var path = Path.Combine(AppContext.BaseDirectory, "messages.txt");

        foreach (var line in File.ReadAllLines(path))
        {
            var parts = line.Split('=');

            _messages[parts[0]] = parts[1];
        }
    }

    public string Get(string key)
    {
        return _messages.TryGetValue(key, out var value) ? value : key;
    }
}
