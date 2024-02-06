namespace Dal;
/// <summary>
/// bulid the data
/// </summary>
internal static class DataSource
{
    internal static List<Do.Task> Tasks { get; } = new();//build lists of the 3 interfaces
    internal static List<Do.Worker> Workers { get; } = new();
    internal static List<Do.Dependency> Dependencies { get; } = new();
    internal static class Config
    {
        static DateTime? startDate = null;
        static DateTime? endDate = null;

        internal const int startTaskId = 1;//build the automatic running numbers
        private static int nextTaskId = startTaskId;
        internal static int NextTaskId { get => nextTaskId++; }

        internal const int startDependenceId = 1;
        private static int nextDependenceId = startDependenceId;
        internal static int NextDependenceId { get => nextDependenceId++; }

    }


}
