using CliWrap.Builders;

namespace RoberSanteNardo.Utils;

public static class CliExtensions
{
    public static ArgumentsBuilder AddOption(this ArgumentsBuilder args, string name, string value)
    {
        return args.Add(name).Add(value);
    }
}