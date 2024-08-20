
using Xunit.Sdk;

/// <summary>
/// This class models health events (healing and damage) and allows you to apply them to entities.
/// </summary>
public class HealthEvent
{
    public int _physicalDelta;
    public int _electricDelta;

    /// <summary>
    /// Initializes a HealthEvent
    /// </summary>
    /// <param name="electricDelta">Physical delta.</param>
    /// <param name="physicalDelta">Electric delta.</param>
    public HealthEvent(int physicalDelta, int electricDelta)
    {
        _physicalDelta = physicalDelta;
        _electricDelta = electricDelta;
    }
}
