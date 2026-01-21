public static class NumberFormatter
{
    static readonly string[] _suffixes = { "", "Thousand", "Million", "Billion", "Trillion", "Quadrillion", "Quintillion", "Sextillion", "Septillion", "Octillion", "Nonillion", "Decillion" };
    static readonly string[] _smallSuffixes = { "", "K", "M", "B", "T", "Qa", "Qi", "Sx", "Sp", "Oc", "No", "De" };

    public static string FormatNumberBig(double pNumber)
    {
        if (pNumber < 1000000)
            return pNumber.ToString("0.#");
        int suffixIndex = 0;
        while (pNumber >= 1000 && suffixIndex < _suffixes.Length - 1)
        {
            pNumber /= 1000;
            suffixIndex++;
        }
        return $"{pNumber:F2} {_suffixes[suffixIndex]}".Trim();
    }

    public static string FormatNumberSmall(double pNumber)
    {
        if (pNumber < 1000)
            return pNumber.ToString("0.#");
        int suffixIndex = 0;
        while (pNumber >= 1000 && suffixIndex < _smallSuffixes.Length - 1)
        {
            pNumber /= 1000;
            suffixIndex++;
        }
        return $"{pNumber:F2}{_smallSuffixes[suffixIndex]}".Trim();
    }
}
