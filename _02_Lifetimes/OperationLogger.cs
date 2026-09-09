public class OperationLogger : IDisposable
{
    private readonly List<string> _lines = new();

    public Guid Id { get; } = Guid.NewGuid();

    public void Add(string line)
    {
        _lines.Add(line);
    }

    public void Dispose()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "log.txt");

        var body = new List<string> { $"+++ BEGIN SCOPE = {Id} +++" };

        body.AddRange(_lines);
        body.Add($"+++ END SCOPE = {Id} +++");

        File.AppendAllLines(path, body);
    }
}
