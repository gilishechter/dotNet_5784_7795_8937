namespace Do;

/// <summary>
/// the Task entity
/// </summary>
/// <param name="Id"></param>
/// <param name="IdWorker"></param>
/// <param name="Name"></param>
/// <param name="Description"></param>
/// <param name="MileStone"></param>
/// <param name="Time"></param>
/// <param name="CreateDate"></param>
/// <param name="WantedStartDate"></param>
/// <param name="StartDate"></param>
/// <param name="EndingDate"></param>
/// <param name="DeadLine"></param>
/// <param name="Product"></param>
/// <param name="Notes"></param>
/// <param name="Rank"></param>
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
    Task() : this(0, 0) { }//ctor

}



