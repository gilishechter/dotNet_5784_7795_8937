[Serializable]
public class BlDoesNotExistException : Exception
{
      public BlDoesNotExistException(string? message) : base(message) { }
      public BlDoesNotExistException(string message, Exception innerException)
                    : base(message, innerException) { }
}

[Serializable]
public class BlNullPropertyException : Exception
{
    public BlNullPropertyException(string? message) : base(message) { }
}

[Serializable]
public class BlAlreadyExistsException : Exception
{
    public BlAlreadyExistsException(string? message) : base(message) { }
    public BlAlreadyExistsException(string message, Exception innerException)
                   : base(message, innerException) { }
}

[Serializable]
public class BlCantBeDeleted : Exception
{
    public BlCantBeDeleted(string? message) : base(message) { }
}

[Serializable]
public class BlCantBeUpdated : Exception
{
    public BlCantBeUpdated(string? message) : base(message) { }
}

[Serializable]
public class BlWhilePlanning: Exception
{
        public BlWhilePlanning(string? message) : base(message) { }
}

[Serializable]
public class BlDuringExecution : Exception
{
    public BlDuringExecution(string? message) : base(message) { }
}