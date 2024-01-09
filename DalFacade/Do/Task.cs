namespace Do;
public record Task(
    int Id,
    int IdWorker,
    string? Name = null,
    string? Description = null,
    bool MileStone = false,
    TimeSpan? Time = null,
    DateTime? CreateDate = null,
    DateTime? WantedStartDate = null,
    DateTime? StartDate = null,
    DateTime? EndingDate = null,
    DateTime? DeadLine = null,
    string? Product = null,
    string? Notes = null,
    int Rank = 0
)
{
    Task() : this(0, 0) { }

}



