namespace Do;

/// <summary>
/// the dependence entity
/// </summary>
/// <param name="Id"></param>
/// <param name="DependenceTask"></param>
/// <param name="PrevTask"></param>
public record Dependency
(
    int Id,
    int DependenceTask,
    int PrevTask
)
{
    Dependency() : this(0, 0, 0) { }//ctor

}
