namespace Do;

/// <summary>
/// The id is not exist
/// </summary>
[Serializable]
public class DalDoesNotExistException : Exception
{
    public DalDoesNotExistException(string? message) : base(message) { }
}

/// <summary>
/// The id is already exist
/// </summary>
[Serializable]
public class DalAlreadyExistsException : Exception
{
    public DalAlreadyExistsException(string? message) : base(message) { }
}

/// <summary>
/// The input is wrong
/// </summary>
[Serializable]
public class WrongInputException : Exception
{
    public WrongInputException(string? message) : base(message) { }
}



