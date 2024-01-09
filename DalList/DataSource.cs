namespace Dal;

internal static class DataSource
{
    internal static List<Do.Task> Tasks { get; } = new();
    internal static List<Do.Worker> Workers { get; } = new();
    internal static List<Do.Dependencies> Dependencies { get; } = new();
    internal static class Config
    {
        static DateTime? start = null;
        static DateTime? end = null;

        internal const int startTaskId = 1;
        private static int nextTaskId = startTaskId;
        internal static int NextTaskId { get => nextTaskId++; }

        internal const int startDependenceId = 1;
        private static int nextDependenceId = startDependenceId;
        internal static int NextDependenceId { get => nextDependenceId++; }

    }


}
