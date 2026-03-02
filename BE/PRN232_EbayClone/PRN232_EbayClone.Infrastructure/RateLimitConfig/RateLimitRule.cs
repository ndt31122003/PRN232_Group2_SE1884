using System.Text.RegularExpressions;

namespace PRN232_EbayClone.Api.Infrastructure.RateLimitConfig;

public class RateLimitRule
{
    private static readonly Regex TimePattern = new(@"([0-9]+(s|m|h|d))");

    private enum TimeUnit { s = 1, m = 60, h = 3600, d = 86400 }

    private static int ParseTime(string timeStr)
    {
        var match = TimePattern.Match(timeStr);
        if (!match.Success)
            throw new ArgumentException("Window không hợp lệ, phải dạng 30s, 1h,...");

        var unit = Enum.Parse<TimeUnit>(match.Value[^1].ToString());
        var num = int.Parse(match.Value.Substring(0, match.Value.Length - 1));
        return num * (int)unit;
    }

    public required string Path { get; set; }
    public required string PathRegex { get; set; }
    public required string Window { get; set; }
    public int MaxRequests { get; set; }

    internal int _windowSeconds = 0;
    internal string PathKey => !string.IsNullOrEmpty(Path) ? Path : PathRegex;
    internal int WindowSeconds => _windowSeconds < 1 ? _windowSeconds = ParseTime(Window) : _windowSeconds;

    public bool MatchPath(string path)
    {
        if (!string.IsNullOrEmpty(Path))
            return path.Equals(Path, StringComparison.InvariantCultureIgnoreCase);
        if (!string.IsNullOrEmpty(PathRegex))
            return Regex.IsMatch(path, PathRegex);
        return false;
    }

}
