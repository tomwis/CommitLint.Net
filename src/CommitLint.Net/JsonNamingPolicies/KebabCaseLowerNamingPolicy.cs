using System.Text.Json;

namespace CommitLint.Net.JsonNamingPolicies;

public class KebabCaseLowerNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return name;
        }

        char[] chars = name.ToCharArray();
        List<char> kebabCasedName = new List<char>();

        for (int i = 0; i < chars.Length; i++)
        {
            if (char.IsUpper(chars[i]))
            {
                if (i > 0)
                {
                    kebabCasedName.Add('-');
                }
                kebabCasedName.Add(char.ToLower(chars[i]));
            }
            else
            {
                kebabCasedName.Add(chars[i]);
            }
        }
        return new string(kebabCasedName.ToArray());
    }
}
