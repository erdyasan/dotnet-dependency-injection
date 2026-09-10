public class Translator
{
    private static readonly string[] Languages = { "en", "tr" };

    private readonly Dictionary<string, Dictionary<string, string>> _catalogs = new();

    private readonly OperationLogger _log;

    public string Language { get; private set; } = string.Empty;

    public Translator(OperationLogger log)
    {
        _log = log;

        foreach (var language in Languages)
        {
            Console.WriteLine($"translator: reading messages.{language}.txt");

            _catalogs[language] = Read(language);
        }
    }

    public void Use(string language)
    {
        Language = language;

        _log.Add($"language selected: {language}");
    }

    public string Get(string key)
    {
        if (_catalogs.TryGetValue(Language, out var messages) && messages.TryGetValue(key, out var value))
        {
            return value;
        }

        return key;
    }

    private static Dictionary<string, string> Read(string language)
    {
        var path = Path.Combine(AppContext.BaseDirectory, $"messages.{language}.txt");

        var messages = new Dictionary<string, string>();

        foreach (var line in File.ReadAllLines(path))
        {
            var parts = line.Split('=', 2);

            messages[parts[0]] = parts[1];
        }

        return messages;
    }
}
