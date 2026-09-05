public class UserRepository
{
    public void Save(string email, string displayName)
    {
        Directory.CreateDirectory("data");

        File.AppendAllText("data/users.csv", $"{email},{displayName}{Environment.NewLine}");
    }

    public string? Read(int lineNumber)
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
}
