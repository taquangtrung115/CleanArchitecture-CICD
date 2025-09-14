namespace DemoCICD.Domain.Entities.MotoGP.ValueObjects;

public class RaceSchedule
{
    public DateTime Practice1 { get; private set; }
    public DateTime Practice2 { get; private set; }
    public DateTime? Practice3 { get; private set; }
    public DateTime Qualifying { get; private set; }
    public DateTime? SprintQualifying { get; private set; }
    public DateTime? SprintRace { get; private set; }
    public DateTime Race { get; private set; }

    public RaceSchedule(DateTime practice1, DateTime practice2, DateTime qualifying, DateTime race,
                       DateTime? practice3 = null, DateTime? sprintQualifying = null, DateTime? sprintRace = null)
    {
        if (practice1 >= practice2)
            throw new ArgumentException("Practice 1 must be before Practice 2");
        if (practice2 >= qualifying)
            throw new ArgumentException("Practice 2 must be before Qualifying");
        if (qualifying >= race)
            throw new ArgumentException("Qualifying must be before Race");

        Practice1 = practice1;
        Practice2 = practice2;
        Practice3 = practice3;
        Qualifying = qualifying;
        SprintQualifying = sprintQualifying;
        SprintRace = sprintRace;
        Race = race;
    }

    public bool HasSprintWeekend => SprintQualifying.HasValue && SprintRace.HasValue;

    public DateTime StartDate => Practice1;
    public DateTime EndDate => Race;

    public IEnumerable<(string Session, DateTime DateTime)> GetAllSessions()
    {
        yield return ("Practice 1", Practice1);
        yield return ("Practice 2", Practice2);
        if (Practice3.HasValue)
            yield return ("Practice 3", Practice3.Value);
        if (SprintQualifying.HasValue)
            yield return ("Sprint Qualifying", SprintQualifying.Value);
        yield return ("Qualifying", Qualifying);
        if (SprintRace.HasValue)
            yield return ("Sprint Race", SprintRace.Value);
        yield return ("Race", Race);
    }

    public override bool Equals(object? obj)
    {
        return obj is RaceSchedule schedule && 
               Practice1 == schedule.Practice1 && 
               Race == schedule.Race;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Practice1, Race);
    }
}