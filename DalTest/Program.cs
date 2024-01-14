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

    static readonly IDal s_dal = new DalList();
    /// <summary>
    /// the main menu
    /// </summary>
    private static void Menu()
    {
        Console.WriteLine("Choose Entity:");
        Console.WriteLine("0 - Exit");
        Console.WriteLine("1 - Worker");
        Console.WriteLine("2 - Task");
        Console.WriteLine("3 - Dependence");
    }

    /// <summary>
    /// the sub menu
    /// </summary>
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

    /// <summary>
    /// this function input details to a worker and add him to the list
    /// </summary>
    private static void AddWorker()
    {
        Console.WriteLine("Enter Id, level (number between 0 - 4), hoour price, name and email");
        int.TryParse(Console.ReadLine(),out int _id);//input the details
        Rank.TryParse(Console.ReadLine(),out Rank _rank);
        double.TryParse(Console.ReadLine(),out double _hourPrice);
        string? _name = Console.ReadLine();
        string? _email = Console.ReadLine();
        Worker _worker = new(_id, _rank, _hourPrice, _name, _email);//build the worker
        Console.WriteLine(s_dal.Worker?.Create(_worker));//add to the list and print the ID's worker

    }
    /// <summary>
    /// this function input details to a task and add it to the list
    /// </summary>
    private static void AddTask()
    {
        Console.WriteLine("Enter Id, Id worker, name, description, mile stone, time, create date, " +
            "wanted start date, start date, end date, dead line, product, notes and level between 0 - 4");
        int.TryParse(Console.ReadLine(),out int Id);//input the details
        int.TryParse(Console.ReadLine(), out int IdWorker);
        string? Name = Console.ReadLine();
        string? Description = Console.ReadLine();
        bool.TryParse(Console.ReadLine(), out bool MileStone);
        TimeSpan.TryParse(Console.ReadLine(), out TimeSpan Time);
        DateTime.TryParse(Console.ReadLine(), out DateTime CreateDate);
        DateTime.TryParse(Console.ReadLine(), out DateTime WantedStartDate);
        DateTime.TryParse(Console.ReadLine(), out DateTime StartDate);
        DateTime.TryParse(Console.ReadLine(), out DateTime EndingDate);
        DateTime.TryParse(Console.ReadLine(), out DateTime DeadLine);
        string? Product = Console.ReadLine();
        string? Notes = Console.ReadLine();
        int.TryParse(Console.ReadLine(), out int Rank);
        Task _Task = new(Id, IdWorker, Name, Description, MileStone, Time, CreateDate
            , WantedStartDate, StartDate, EndingDate, DeadLine, Product, Notes, Rank);//build the task object
        Console.WriteLine(s_dal.Task?.Create(_Task));//add to the list and print the Id's task


    }
    /// <summary>
    /// this function input details to a dependence and add it to the list
    /// </summary>
    private static void AddDependence()
    {
        Console.WriteLine("Enter Id, dependence task and previous task");
        int.TryParse(Console.ReadLine(), out int _id);//input the details
        int.TryParse(Console.ReadLine(), out int _DependenceTask);
        int.TryParse(Console.ReadLine(), out int _PrevTask);
        Dependencies _Dependence = new(_id, _DependenceTask, _PrevTask);//build the dependence
        Console.WriteLine(s_dal.Dependencies?.Create(_Dependence));//add to the list and print the ID

    }
    /// <summary>
    /// this function input id of worker and print the object with the write ID
    /// </summary>
    private static void WorkerObjectView()
    {
        Console.WriteLine("Enter Id for print");
        int.TryParse(Console.ReadLine(), out int _id);
        Console.WriteLine(s_dal.Worker?.Read(_id));
    }
    /// <summary>
    /// this function input id of task and print the object with the write ID
    /// </summary>
    private static void TaskObjectView()
    {
        Console.WriteLine("Enter Id for print");
        int.TryParse(Console.ReadLine(),out int _id);
        Console.WriteLine(s_dal.Task?.Read(_id));
    }
    /// <summary>
    /// this function input id of dependence and print the object with the write ID
    /// </summary>
    private static void DependenceObjectView()
    {
        Console.WriteLine("Enter Id for print");
        int.TryParse(Console.ReadLine(), out int _id);
        Console.WriteLine(s_dal.Dependencies?.Read(_id));
    }
    /// <summary>
    /// this function input the wanted details to update and build new worker
    /// </summary>
    private static void UpdateWorker()
    {
        Console.WriteLine("Enter Id:");
        int.TryParse(Console.ReadLine(), out int _id);
        Console.WriteLine(s_dal.Worker!.Read(_id));
        Worker worker1 = s_dal.Worker.Read(_id)!;
        Console.WriteLine("Enter New Details for level (number between 0 - 4), hour price, name and email:");
        
        if (!Rank.TryParse(Console.ReadLine(), out Rank _rank))
            _rank = worker1!.WorkerRank; 
        
        if (!double.TryParse(Console.ReadLine(), out double _hourPrice))
            _hourPrice = worker1!.HourPrice; 
        
        string? _name = Console.ReadLine();
        if (_name == "")
           _name = worker1!.Name; 
        
        string? _email = Console.ReadLine();
        if (_email == "")
          _email = worker1!.Email; 
        
        Worker _worker = new(_id, _rank, _hourPrice, _name, _email);
        s_dal.Worker?.Update(_worker);
    }

    private static void UpdateTask()
    {
        Console.WriteLine("Enter Id:");
        int.TryParse( Console.ReadLine(),out int Id );
        Console.WriteLine(s_dal.Task!.Read(Id));
        Task? task1 = s_dal.Task.Read(Id);
        Console.WriteLine("Enter New Details for Enter Id, Id worker, name, description, mile stone, time, create date,");
        Console.WriteLine("wanted start date, start date, end date, dead line, product, notes and level between 0 - 4");
        if(!int.TryParse(Console.ReadLine(),out int IdWorker))
            IdWorker = task1!.IdWorker;
        string? Name = Console.ReadLine();
        if(Name =="")
            Name = task1!.Name;
        
        string? Description = Console.ReadLine();
        if (Description == "")
            Description = task1!.Description;
        
       bool.TryParse(Console.ReadLine(),out bool MileStone);
        if (!MileStone)
            MileStone = task1!.MileStone;
        TimeSpan? Time = TimeSpan.Parse(Console.ReadLine()!);
        if (Time == null)
            Time = task1!.Time;
        
        DateTime? CreateDate = DateTime.Parse(Console.ReadLine()!);
        if (CreateDate == null)
            CreateDate = task1!.CreateDate;
        
        DateTime? WantedStartDate = DateTime.Parse(Console.ReadLine()!);
        if (WantedStartDate == null)
            WantedStartDate = task1!.WantedStartDate;
        
        DateTime? StartDate = DateTime.Parse(Console.ReadLine()!);
        if (StartDate == null)
            StartDate = task1!.StartDate;
        
        DateTime? EndingDate = DateTime.Parse(Console.ReadLine()!);
        if (EndingDate == null)
            EndingDate = task1!.EndingDate;
        
        DateTime? DeadLine = DateTime.Parse(Console.ReadLine()!);
        if (DeadLine == null)
            DeadLine = task1!.DeadLine;
        
        string? Product = Console.ReadLine();
        if (Product == "")
            Product = task1!.Product;
    
        string? Notes = Console.ReadLine();
        if (Notes == "")
            Notes = task1!.Notes;
        
       
        if (!int.TryParse(Console.ReadLine(), out int _Rank))
            _Rank = task1!.Rank;
        Task _Task = new(Id, IdWorker, Name, Description, MileStone, Time, CreateDate
        , WantedStartDate, StartDate, EndingDate, DeadLine, Product, Notes, _Rank);
        s_dal.Task?.Update(_Task);
    }
    private static void UpdateDependence()
    {
        Console.WriteLine("Enter Id:");
        int.TryParse(Console.ReadLine(), out int _id);
        Console.WriteLine(s_dal.Dependencies!.Read(_id));
        Dependencies? dependence1 = s_dal.Dependencies.Read(_id);
        if (dependence1 != null)
        {
            Console.WriteLine("Enter New Details for dependence task and previous task:");
            if (!int.TryParse(Console.ReadLine(), out int _DependenceTask))
                _DependenceTask=dependence1.DependenceTask;
              if( !int.TryParse(Console.ReadLine(), out int PrevTask))
                PrevTask=dependence1.PrevTask;  
            Dependencies _Dependence = new(_id, _DependenceTask, PrevTask);
            s_dal.Dependencies?.Update(_Dependence);
        }       
    }
    private static void DeleteDependence()
    {
        Console.WriteLine("Enter Id to delete");
        int.TryParse(Console.ReadLine(), out int _id);
        s_dal.Dependencies?.Delete(_id);
    }

    private static void DeleteTask()
    {
        Console.WriteLine("Enter Id to delete");
        int.TryParse(Console.ReadLine(), out int _id);
        s_dal.Task?.Delete(_id);
    }

    private static void DeleteWorker()
    {
        Console.WriteLine("Enter Id to delete");
        int.TryParse(Console.ReadLine(), out int _id);
        s_dal.Worker?.Delete(_id);
    }
    private static void WorkerListView()
    {
        Console.WriteLine("workers list:");
        List<Worker> workerList = s_dal.Worker!.ReadAll();
        for (int i = 0; i < workerList.Count; i++)
        {
            Console.WriteLine(workerList[i]);
        }
    }

    private static void TaskListView()
    {
        Console.WriteLine("tasks list:");
        List<Task> taskList = s_dal.Task!.ReadAll();
        for (int i = 0; i < taskList.Count; i++)
        {
            Console.WriteLine(taskList[i]);
        }
    }

    private static void DependenceListView()
    {
        Console.WriteLine("dependences list:");
        List<Dependencies> dependenceList = s_dal.Dependencies!.ReadAll();
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
            Initialization.Do(s_dal);

            Menu();
            int.TryParse(Console.ReadLine(), out int choose);

            while (choose != 0)
            {
                //int choose1;
                switch (choose)
                {
                    case 0:
                        break;
                    case 1:
                        SubMenu();
                        int.TryParse(Console.ReadLine(), out int choose1);
                        SubMenuWorker(choose1);
                        break;
                    case 2:
                        SubMenu();
                        int.TryParse(Console.ReadLine(), out int choose2);
                        SubMenuTask(choose2);
                        break;
                    case 3:
                        SubMenu();
                        int.TryParse(Console.ReadLine(), out int choose3);
                        SubMenuDependence(choose3 );
                        break;
                }
                Menu();
                choose = int.Parse(Console.ReadLine()!);
            }

        }
        catch (Exception Ex)
        {
            Console.WriteLine(Ex);
        }
    }
}