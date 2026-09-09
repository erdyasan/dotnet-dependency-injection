public class FieldValidator
{
    private readonly List<string> _errors = new();

    public IReadOnlyList<string> Errors => _errors;

    public void AddName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            _errors.Add("error.name.empty");

            return;
        }

        if (name.Trim().Length < 2)
        {
            _errors.Add("error.name.short");
        }
    }

    public void AddEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            _errors.Add("error.email.empty");

            return;
        }

        if (!email.Contains('@'))
        {
            _errors.Add("error.email.invalid");
        }
    }
}
