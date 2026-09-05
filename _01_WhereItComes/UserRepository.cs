public class UserRepository
{
    private readonly TableResolver _tableResolver;

    public UserRepository(TableResolver tableResolver)
    {
        _tableResolver = tableResolver;
    }

    public void Save(string email, string displayName)
    {
        Directory.CreateDirectory("data");

        File.AppendAllText(
            _tableResolver.GetUserTableSource(),
            $"{email},{displayName}{Environment.NewLine}");
    }

    public string? Read(int lineNumber)
    {
        var source = _tableResolver.GetUserTableSource();

        if (!File.Exists(source))
        {
            return null;
        }

        var lines = File.ReadAllLines(source);

        if (lineNumber < 1 || lineNumber > lines.Length)
        {
            return null;
        }

        return lines[lineNumber - 1];
    }
}
