namespace Do;

/// <summary>
/// the dependence entity
/// </summary>
/// <param name="Id"></param>
/// <param name="DependenceTask"></param>
/// <param name="PrevTask"></param>
public record Dependencies
(
    int Id,
    int DependenceTask,
    int PrevTask
)
{
    Dependencies() : this(0, 0, 0) { }//ctor

}
