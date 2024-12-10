namespace TimesheetSystem.Domain.ValueObjects;

public class TimePeriod
{
    public decimal HoursWorked { get; private set; } 

    public TimePeriod()
    {
        HoursWorked = 1;
    } 

    public TimePeriod(decimal hoursWorked)
    {
        if (hoursWorked <= 0 || hoursWorked > 24)
            throw new ArgumentException("Hours worked must be between 1 and 24.");

        HoursWorked = hoursWorked;
    }

    public override bool Equals(object obj)
    {
        if (obj is TimePeriod other)
            return HoursWorked == other.HoursWorked;

        return false;
    }

    public override int GetHashCode()
    {
        return HoursWorked.GetHashCode();
    }

    public override string ToString()
    {
        return $"{HoursWorked} hours";
    }
}