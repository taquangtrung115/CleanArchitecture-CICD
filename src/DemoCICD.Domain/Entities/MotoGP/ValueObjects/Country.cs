using DemoCICD.Domain.Abstractions.Entities;

namespace DemoCICD.Domain.Entities.MotoGP.ValueObjects;

public class Country
{
    public string Code { get; private set; }
    public string Name { get; private set; }
    public string Flag { get; private set; }

    public Country(string code, string name, string flag)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Country code cannot be empty", nameof(code));
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Country name cannot be empty", nameof(name));

        Code = code.ToUpper();
        Name = name;
        Flag = flag;
    }

    public override bool Equals(object? obj)
    {
        return obj is Country country && Code == country.Code;
    }

    public override int GetHashCode()
    {
        return Code.GetHashCode();
    }

    public override string ToString()
    {
        return $"{Name} ({Code})";
    }
}