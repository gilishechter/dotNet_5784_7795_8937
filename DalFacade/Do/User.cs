
namespace Do;

public record User(

    string? userName,
    string? password,
    bool isAdmin,
    int id
)
{
    User() : this(null, null, false,0) { }
}
