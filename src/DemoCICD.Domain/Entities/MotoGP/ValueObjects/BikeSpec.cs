namespace DemoCICD.Domain.Entities.MotoGP.ValueObjects;

public class BikeSpec
{
    public string Engine { get; private set; }
    public int Displacement { get; private set; } // in cc
    public int MaxPower { get; private set; } // in HP
    public decimal MaxSpeed { get; private set; } // in km/h
    public decimal Weight { get; private set; } // in kg
    public string Transmission { get; private set; }
    public string Frame { get; private set; }
    public string FrontSuspension { get; private set; }
    public string RearSuspension { get; private set; }
    public string FrontBrakes { get; private set; }
    public string RearBrakes { get; private set; }

    public BikeSpec(string engine, int displacement, int maxPower, decimal maxSpeed, decimal weight,
                   string transmission, string frame, string frontSuspension, string rearSuspension,
                   string frontBrakes, string rearBrakes)
    {
        if (string.IsNullOrWhiteSpace(engine))
            throw new ArgumentException("Engine specification cannot be empty", nameof(engine));
        if (displacement <= 0)
            throw new ArgumentException("Displacement must be positive", nameof(displacement));
        if (maxPower <= 0)
            throw new ArgumentException("Max power must be positive", nameof(maxPower));
        if (maxSpeed <= 0)
            throw new ArgumentException("Max speed must be positive", nameof(maxSpeed));
        if (weight <= 0)
            throw new ArgumentException("Weight must be positive", nameof(weight));

        Engine = engine;
        Displacement = displacement;
        MaxPower = maxPower;
        MaxSpeed = maxSpeed;
        Weight = weight;
        Transmission = transmission ?? string.Empty;
        Frame = frame ?? string.Empty;
        FrontSuspension = frontSuspension ?? string.Empty;
        RearSuspension = rearSuspension ?? string.Empty;
        FrontBrakes = frontBrakes ?? string.Empty;
        RearBrakes = rearBrakes ?? string.Empty;
    }

    public override bool Equals(object? obj)
    {
        return obj is BikeSpec spec &&
               Engine == spec.Engine &&
               Displacement == spec.Displacement &&
               MaxPower == spec.MaxPower &&
               Weight == spec.Weight;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Engine, Displacement, MaxPower, Weight);
    }

    public override string ToString()
    {
        return $"{Engine} {Displacement}cc, {MaxPower}HP, {Weight}kg";
    }
}