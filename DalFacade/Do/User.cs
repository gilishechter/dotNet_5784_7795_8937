
namespace Do;

public record User(

    string? userName,
    string? Id,
    bool isAdmin
)
{
    User() : this(null, null, false) { }
}
