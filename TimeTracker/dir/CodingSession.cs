namespace ConsoleTracker.dir;

public class CodingSession
{
    public CodingSession() { }

    public CodingSession(TimeSpan startTime, TimeSpan finishTime, TimeSpan duration)
    {
        this.StartTime = startTime;
        this.FinishTime = finishTime;
        Duration = duration;
    }

    public int Id { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan FinishTime { get; set; }
    public TimeSpan Duration { get; set; }
}