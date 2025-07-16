namespace Challenge.Skopia.TaskManagement.Domain.Enumerators;

public enum TraceType : byte
{
    /// <summary>
    /// Entity was not changed
    /// </summary>
    None = 0,

    /// <summary>
    /// Entity was created
    /// </summary>
    Create = 1,

    /// <summary>
    /// Entity was updated
    /// </summary>
    Update = 2,

    /// <summary>
    /// Entity was deleted
    /// </summary>
    Delete = 3
}