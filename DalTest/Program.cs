using System.Diagnostics;

namespace DalTest;
using Do;
using DalApi;
using Dal;
using System;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Runtime.CompilerServices;

internal class Program
{

    private static readonly IWorker? s_dalWorker = new WorkerImplementation(); //stage 1
    private static readonly ITask? s_dalTask = new TaskImplementation(); //stage 1
    private static readonly IDependence? s_dalDependence = new DependenceImplementation(); //stage 1

    private static void Menu()
    {
        Console.WriteLine("Choose Entity:");
        Console.WriteLine("0 - Exit");
        Console.WriteLine("1 - Worker");
        Console.WriteLine("2 - Task");
        Console.WriteLine("3 - Dependence");
    }

    private static void SubMenu()
    {

        Console.WriteLine("Choose Actuon:");
        Console.WriteLine("0 - Exit");
        Console.WriteLine("1 - Add");
        Console.WriteLine("2 - Obgect View");
        Console.WriteLine("3 - List View");
        Console.WriteLine("4 - Update");
        Console.WriteLine("5 - Delete");
    }
    private static void AddWorker()
    {
        int _id = Console.Read();
        Rank _rank = (Rank)Console.Read();
        double _hourPrice = Console.Read();
        string? _name = Console.ReadLine();
        string? _email = Console.ReadLine();
        Worker _worker = new(_id, _rank, _hourPrice, _name, _email);
        Console.WriteLine(s_dalWorker?.Create(_worker));

    }
    private static void AddTask()
    {
        int Id = Console.Read();
        int IdWorker = Console.Read();
        string? Name = Console.ReadLine();
        string? Description = Console.ReadLine();
        bool MileStone = bool.Parse(Console.ReadLine()!);
        TimeSpan? Time = TimeSpan.Parse(Console.ReadLine()!);
        DateTime? CreateDate = DateTime.Parse(Console.ReadLine()!);
        DateTime? WantedStartDate = DateTime.Parse(Console.ReadLine()!);
        DateTime? StartDate = DateTime.Parse(Console.ReadLine()!);
        DateTime? EndingDate = DateTime.Parse(Console.ReadLine()!);
        DateTime? DeadLine = DateTime.Parse(Console.ReadLine()!);
        string? Product = Console.ReadLine();
        string? Notes = Console.ReadLine();
        int Rank = Console.Read();
        Task _Task = new(Id, IdWorker, Name, Description, MileStone, Time, CreateDate
            , WantedStartDate, StartDate, EndingDate, DeadLine, Product, Notes, Rank);
        Console.WriteLine(s_dalTask?.Create(_Task));


    }
    private static void AddDependence()
    {
        int _id = Console.Read();
        int _DependenceTask = Console.Read();
        int PrevTask = Console.Read();
        Dependencies _Dependence = new(_id, _DependenceTask, PrevTask);
        Console.WriteLine(s_dalDependence?.Create(_Dependence));

    }
    private static void WorkerObjectView()
    {
        int _id = Console.Read();
        Console.WriteLine(s_dalWorker?.Read(_id));
    }

    private static void TaskObjectView()
    {
        int _id = Console.Read();
        Console.WriteLine(s_dalTask?.Read(_id));
    }

    private static void DependenceObjectView()
    {
        int _id = Console.Read();
        Console.WriteLine(s_dalDependence?.Read(_id));
    }

    private static void UpdateWorker()
    {
        int _id = Console.Read();
        Console.WriteLine(s_dalDependence!.Read(_id));
        Console.WriteLine("Enter New Details:");
        Rank _rank = (Rank)Console.Read();
        double _hourPrice = Console.Read();
        string? _name = Console.ReadLine();
        string? _email = Console.ReadLine();
        Worker _worker = new(_id, _rank, _hourPrice, _name, _email);
        s_dalWorker?.Update(_worker);
    }

    private static void UpdateTask()
    {
        int Id = Console.Read();
        Console.WriteLine(s_dalDependence!.Read(Id));
        Console.WriteLine("Enter New Details:");
        int IdWorker = Console.Read();
        string? Name = Console.ReadLine();
        string? Description = Console.ReadLine();
        bool MileStone = bool.Parse(Console.ReadLine()!);
        TimeSpan? Time = TimeSpan.Parse(Console.ReadLine()!);
        DateTime? CreateDate = DateTime.Parse(Console.ReadLine()!);
        DateTime? WantedStartDate = DateTime.Parse(Console.ReadLine()!);
        DateTime? StartDate = DateTime.Parse(Console.ReadLine()!);
        DateTime? EndingDate = DateTime.Parse(Console.ReadLine()!);
        DateTime? DeadLine = DateTime.Parse(Console.ReadLine()!);
        string? Product = Console.ReadLine();
        string? Notes = Console.ReadLine();
        int Rank = Console.Read();
        Task _Task = new(Id, IdWorker, Name, Description, MileStone, Time, CreateDate
            , WantedStartDate, StartDate, EndingDate, DeadLine, Product, Notes, Rank);
        s_dalTask?.Update(_Task);
    }
    private static void UpdateDependence()
    {
        int _id = Console.Read();
        Console.WriteLine(s_dalDependence!.Read(_id));
        Console.WriteLine("Enter New Details:");
        int _DependenceTask = Console.Read();
        int PrevTask = Console.Read();
        Dependencies _Dependence = new(_id, _DependenceTask, PrevTask);
        s_dalDependence?.Update(_Dependence);
    }
    private static void DeleteDependence()
    {
        int _id = Console.Read();
        s_dalDependence?.Delete(_id);
    }

    private static void DeleteTask()
    {
        int _id = Console.Read();
        s_dalTask?.Delete(_id);
    }

    private static void DeleteWorker()
    {
        int _id = Console.Read();
        s_dalWorker?.Delete(_id);
    }
    private static void WorkerListView()
    {
        List<Worker> workerList = s_dalWorker!.ReadAll();
        for (int i = 0; i < workerList.Count; i++)
        {
            Console.WriteLine(workerList[i]);
        }
    }

    private static void TaskListView()
    {
        List<Task> taskList = s_dalTask!.ReadAll();
        for (int i = 0; i < taskList.Count; i++)
        {
            Console.WriteLine(taskList[i]);
        }
    }

    private static void DependenceListView()
    {
        List<Dependencies> dependenceList = s_dalDependence!.ReadAll();
        for (int i = 0; i < dependenceList.Count; i++)
        {
            Console.WriteLine(dependenceList[i]);
        }
    }

    private static void SubMenuWorker(int choose1)
    {
        switch (choose1)
        {
            case 0:
                break;
            case 1:
                AddWorker();
                break;
            case 2:
                WorkerObjectView();
                break;
            case 3:
                WorkerListView();
                break;
            case 4:
                UpdateWorker();
                break;
            case 5:
                DeleteWorker();
                break;
        }
    }

    private static void SubMenuTask(int choose1)
    {
        switch (choose1)
        {
            case 0:
                break;
            case 1:
                AddTask();
                break;
            case 2:
                TaskObjectView();
                break;
            case 3:
                TaskListView();
                break;
            case 4:
                UpdateTask();
                break;
            case 5:
                DeleteTask();
                break;
        }
    }

    private static void SubMenuDependence(int choose1)
    {
        switch (choose1)
        {
            case 0:
                break;
            case 1:
                AddDependence();
                break;
            case 2:
                DependenceObjectView();
                break;
            case 3:
                DependenceListView();
                break;
            case 4:
                UpdateDependence();
                break;
            case 5:
                DeleteDependence();
                break;
        }
    }
    static void Main(string[] args)
    {
        try
        {
            Initialization.Do(s_dalWorker, s_dalTask, s_dalDependence);

            Menu();
            int choose = int.Parse(Console.ReadLine());

            while (choose != 0)
            {
                int choose1;
                switch (choose)
                {
                    case 0:
                        break;
                    case 1:
                        SubMenu();
                        choose1 = int.Parse(Console.ReadLine());
                        SubMenuWorker(choose1);
                        break;
                    case 2:
                        SubMenu();
                        choose1 = int.Parse(Console.ReadLine());
                        SubMenuTask(choose1);
                        break;
                    case 3:
                        SubMenu();
                        choose1 = int.Parse(Console.ReadLine());
                        SubMenuDependence(choose1);
                        break;
                }
                choose = int.Parse(Console.ReadLine());
            }

        }
        catch (Exception Ex)
        {
            Console.WriteLine(Ex);
        }
    }
}