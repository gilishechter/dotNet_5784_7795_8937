namespace Do;

public record Dependencies
(
    int Id,
    int DependenceTask,
    int PrevTask
)
{
    Dependencies() : this(0, 0, 0) { }

}
