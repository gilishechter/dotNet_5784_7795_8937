namespace Do;
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