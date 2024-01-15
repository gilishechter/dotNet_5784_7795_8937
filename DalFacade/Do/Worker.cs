namespace Do;

/// <summary>
/// the Worker entity
/// </summary>
/// <param name="Id"></param>
/// <param name="WorkerRank"></param>
/// <param name="HourPrice"></param>
/// <param name="Name"></param>
/// <param name="Email"></param>
public record Worker
(
    int Id,
    Rank WorkerRank,
    double HourPrice,
    string? Name = null,
    string? Email = null
)

{
    Worker() : this(0, Rank.Beginner, 0) { }


}