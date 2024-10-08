﻿using BlApi;
using BO;
using DalTest;
using Do;

namespace BlTest;
internal class Programe
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

   
    private static void Menu()
    {
        Console.WriteLine("Choose logic Entity:");
        Console.WriteLine("0 - Exit");
        Console.WriteLine("1 - Worker");
        Console.WriteLine("2 - Task");
        Console.WriteLine("3 - to initialize the data");
        Console.WriteLine("4 - to Clear the data");
        if (s_bl.GetStartProject() == null)
            Console.WriteLine("5 - If you want to enter the start project date");
        

    }

    private static void SubMenuTask()
    {

        Console.WriteLine("Choose Actuon:");
        Console.WriteLine("0 - Exit");
        Console.WriteLine("1 - Add");
        Console.WriteLine("2 - Obgect View");
        Console.WriteLine("3 - List View");
        Console.WriteLine("4 - Update");
        Console.WriteLine("5 - Delete");
    }

    private static void SubMenuWorker()
    {

        Console.WriteLine("Choose Actuon:");
        Console.WriteLine("0 - Exit");
        Console.WriteLine("1 - Add");
        Console.WriteLine("2 - Obgect View");
        Console.WriteLine("3 - List View");
        Console.WriteLine("4 - Update");
        Console.WriteLine("5 - Delete");
        Console.WriteLine("6 - Group by Rank");
    }

    /// <summary>
    /// this function input details to a worker and add him to the list
    /// </summary>
    private static void AddWorker()
    {
        Console.WriteLine("Enter Id, level (number between 0 - 4), hoour price, name and email:");

        if (!int.TryParse(Console.ReadLine(), out int _id))//input the details and check if is right details
            throw new FormatException("Wrong Input, Try Again");//throw exception if is wrong input

        if (!BO.Rank.TryParse(Console.ReadLine(), out BO.Rank _rank))
            throw new FormatException("Wrong Input, Try Again");

        if (!double.TryParse(Console.ReadLine(), out double _hourPrice))
            throw new FormatException("Wrong Input, Try Again");

        string? _name = Console.ReadLine();

        string? _email = Console.ReadLine();

        BO.Worker _worker = new()
        {
            Id = _id,
            WorkerRank = _rank,
            HourPrice = _hourPrice,
            Name = _name,
            Email = _email,

        };//build the worker
        Console.WriteLine(s_bl.Worker?.Create(_worker));//add to the list and print the ID's worker
    }

    /// <summary>
    /// this function input id of worker and print the object with the right ID
    /// </summary>
    private static void WorkerObjectView()
    {
        Console.WriteLine("Enter Id for print");

        if (!int.TryParse(Console.ReadLine(), out int _id))
            throw new FormatException("Wrong Input, Try Again");

        if (s_bl.Worker?.Read(_id) == null)
            throw new DalDoesNotExistException($"this ID worker={_id} doesn't exist");
        Console.WriteLine(s_bl.Worker?.Read(_id));
    }

    /// <summary>
    /// this function input the wanted details to update and build new worker
    /// </summary>
    private static void UpdateWorker()
    {
        Console.WriteLine("Enter Id:");

        if (!int.TryParse(Console.ReadLine(), out int _id))//check if the user put right input and throw exception if he put wrong input
            throw new FormatException("Wrong Input, Try Again");

        Console.WriteLine(s_bl.Worker!.Read(_id));
        BO.Worker? worker1 = s_bl.Worker.Read(_id)!;

        Console.WriteLine("Enter New Details for level (number between 0 - 4), hour price, name, email:");

        if (!BO.Rank.TryParse(Console.ReadLine(), out BO.Rank _rank))//if the user doesnt put input we take the old input
            _rank = worker1!.WorkerRank;

        if (!double.TryParse(Console.ReadLine(), out double _hourPrice))
            _hourPrice = worker1!.HourPrice;

        string? _name = Console.ReadLine();
        if (_name == "")
            _name = worker1!.Name;

        string? _email = Console.ReadLine();
        if (_email == "")
            _email = worker1!.Email;

        BO.Worker _worker = new()
        {
            Id = _id,
            WorkerRank = _rank,
            HourPrice = _hourPrice,
            Name = _name,
            Email = _email,
        };//update the worker 

        s_bl.Worker?.Update(_worker);
    }

    /// <summary>
    /// this function ask from user id and delete the dependence with this id
    /// </summary>
    /// <exception cref="WrongInputException"></exception>
    private static void DeleteWorker()
    {
        Console.WriteLine("Enter Id to delete");

        if (!int.TryParse(Console.ReadLine(), out int _id))
            throw new FormatException("Wrong Input, Try Again");

        s_bl.Worker?.Delete(_id);
    }

    /// <summary>
    /// this function show the list of workers
    /// </summary>
    private static void WorkerListView()
    {
        Console.WriteLine("workers list:");

        IEnumerable<BO.Worker?> workerList = s_bl.Worker!.ReadAll();
        foreach (BO.Worker? worker in workerList)//The for goes through all the elements in the list and prints them to the user
        {
            Console.WriteLine(worker);
        }
    }
    /// <summary>
    ///The function returns a collection of employees with the same rank
    /// </summary>
    /// <returns></returns>
    /// <exception cref="FormatException"></exception>
    private static IEnumerable<BO.Worker> GroupByRank()
    {
        Console.WriteLine("Enter the rank you want the group to be sorted by:");

        if (!int.TryParse(Console.ReadLine(), out int _rank))
            throw new FormatException("Wrong Input, Try Again");

        IEnumerable<BO.Worker> group = s_bl.Worker.RankGroup(_rank);
        foreach (BO.Worker? worker in group)//The for goes through all the elements in the list and prints them to the user
        {
            Console.WriteLine(worker);
        }
        return group;
    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// the function ask details from user and add the task
    /// </summary>
    /// <exception cref="FormatException"></exception>
    /// <exception cref="BlDoesNotExistException"></exception>
    private static void AddTask()
    {
        //the user input the details that he want to add
        Console.WriteLine("Enter name, description, mile stone, time, " +
            "product, notes and level between 0 - 4");

        string? _Name = Console.ReadLine();

        string? _Description = Console.ReadLine();

        if (!bool.TryParse(Console.ReadLine(), out bool _MileStone))
            throw new FormatException("Wrong Input, Try Again");

        if (!TimeSpan.TryParse(Console.ReadLine(), out TimeSpan _Time))
            throw new FormatException("Wrong Input, Try Again");

        string? _Product = Console.ReadLine();

        string? _Notes = Console.ReadLine();

        if (!int.TryParse(Console.ReadLine(), out int _Rank))
            throw new FormatException("Wrong Input, Try Again");

        List<TaskList> dependencies = new List<TaskList>();
        // if this task have previous task the user enter the amount and the previous task and update 
        Console.WriteLine("Does this _task have previous tasks?");
        string? choice = Console.ReadLine();
        if (choice == "yes" || choice == "Yes")
        {
            Console.WriteLine("How many previous tasks this _task has?");
            int num = int.Parse(Console.ReadLine()!);

            Console.WriteLine("Enter the previous tasks: ");

            for (int i = 0; i < num; i++)
            {
                int depTask = int.Parse(Console.ReadLine()!);
                if (s_bl.Task.Read(depTask) == null)
                    throw new BlDoesNotExistException("this _task doesn't exist");
                TaskList task = new()
                {
                    Id = depTask,
                    Name = s_bl.Task.Read(depTask)!.Name,
                    Description = s_bl.Task.Read(depTask)!.Description,
                    Status = s_bl.Task.Read(depTask)!.Status
                };
                dependencies.Add(task);// update the dependence task if he enter

            }
        }

        BO.Task Task = new()
        {
            Name = _Name,
            Description = _Description,
            MileStone = _MileStone,
            Time = _Time,
            Product = _Product,
            Notes = _Notes,
            Rank = _Rank,
            DependenceTasks = dependencies
        };//build the _task object
        Console.WriteLine(s_bl.Task?.Create(Task));//add to the list and print the Id's _task
    }
    /// <summary>
    /// the function return the task with the id that the user enter
    /// </summary>
    /// <exception cref="FormatException"></exception>
    /// <exception cref="DalDoesNotExistException"></exception>
    private static void TaskObjectView()
    {
        Console.WriteLine("Enter Id for print");
        if (!int.TryParse(Console.ReadLine(), out int _id))
            throw new FormatException("Wrong Input, Try Again");

        if (s_bl.Task?.Read(_id) == null)
            throw new DalDoesNotExistException($"this ID _task={_id} doesn't exist");
        Console.WriteLine(s_bl.Task?.Read(_id));
    }

    /// <summary>
    /// this function input the wanted details to update and build new _task
    /// </summary>
    /// <exception cref="WrongInputException"></exception>
    private static void UpdateTask()
    {
        Console.WriteLine("Enter Id:");
        if (!int.TryParse(Console.ReadLine(), out int Id))
            throw new FormatException("Wrong Input, Try Again");

        Console.WriteLine(s_bl.Task!.Read(Id));
        BO.Task? task1 = s_bl.Task.Read(Id);

        Console.WriteLine("Enter New Details for: Id worker, name, description, mile stone, time, create date,");
        Console.WriteLine("wanted start date, start date, end date, product, notes and level between 0 - 4");

        string? idWorker = (Console.ReadLine());
        int? IdWorker = !string.IsNullOrEmpty(idWorker) ? int.Parse(idWorker) : task1!.IdWorker;
        string? NameWorker = null;
        if (IdWorker != null)           
            NameWorker = s_bl.Worker.Read(IdWorker.Value)!.Name;

        string? Name = Console.ReadLine();
        if (Name == "")
            Name = task1!.Name;

        string? Description = Console.ReadLine();
        if (Description == "")
            Description = task1!.Description;

        if (!bool.TryParse(Console.ReadLine(), out bool MileStone))
            MileStone = task1!.MileStone;

        string? time = Console.ReadLine();
        TimeSpan? Time = !string.IsNullOrEmpty(time) ? TimeSpan.Parse(time) : task1!.Time;

        string? createDate = Console.ReadLine();
        DateTime? CreateDate = !string.IsNullOrEmpty(createDate) ? DateTime.Parse(createDate) : task1!.CreateDate;

        string? wantedStartDate = Console.ReadLine();
        DateTime? WantedStartDate = !string.IsNullOrEmpty(wantedStartDate) ? DateTime.Parse(wantedStartDate) : task1!.WantedStartDate;

        string? startDate = Console.ReadLine();
        DateTime? StartDate = !string.IsNullOrEmpty(startDate) ? DateTime.Parse(startDate) : task1!.StartDate;

        string? endingDate = Console.ReadLine();
        DateTime? EndingDate = !string.IsNullOrEmpty(endingDate) ? DateTime.Parse(endingDate) : task1!.EndingDate;

        DateTime? DeadLine = StartDate > WantedStartDate ? StartDate + Time : WantedStartDate + Time;

        string? Product = Console.ReadLine();
        if (Product == "")
            Product = task1!.Product;

        string? Notes = Console.ReadLine();
        if (Notes == "")
            Notes = task1!.Notes;

        if (!int.TryParse(Console.ReadLine(), out int _Rank))
            _Rank = task1!.Rank;

        
        BO.Task Task = new()
        {
            Id = Id,
            IdWorker = IdWorker,
            NameWorker = NameWorker,
            Name = Name,
            Description = Description,
            MileStone = MileStone,
            Time = Time,
            CreateDate = CreateDate,
            WantedStartDate = WantedStartDate,
            StartDate = StartDate,
            EndingDate = EndingDate,
            DeadLine = DeadLine,
            Product = Product,
            Notes = Notes,
            Rank = _Rank,

        };//build the _task object
        s_bl.Task?.Update(Task);

    }
    /// <summary>
    /// the function delete the task 
    /// </summary>
    /// <exception cref="FormatException"></exception>
    private static void DeleteTask()
    {
        Console.WriteLine("Enter Id to delete");

        if (!int.TryParse(Console.ReadLine(), out int _id))
            throw new FormatException("Wrong Input, Try Again");

        s_bl.Task?.Delete(_id);
    }
    /// <summary>
    /// the function print the list of the task
    /// </summary>
    private static void TaskListView()
    {
        Console.WriteLine("tasks list:");
        IEnumerable<BO.TaskList?> taskList = s_bl.Task!.ReadAll();
        foreach (BO.TaskList? task in taskList)//The for goes through all the elements in the list and prints them to the user
        {
            Console.WriteLine(task);
        }
    }
    /// <summary>
    /// the action menu for the worker entity
    /// </summary>
    /// <param name="choose1"></param>
    private static void SubMenuWorkerActions(int choose1)
    {
        try
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
                case 6:
                    GroupByRank();
                    break;
            }
        }
        catch (Exception Ex)
        {
            Console.WriteLine(Ex);
        }
    }

    /// <summary>
    /// the action menu for the _task entity
    /// </summary>
    /// <param name="choose1"></param>
    private static void SubMenuTaskActions(int choose1)
    {
        try
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
        catch (Exception Ex)
        {
            Console.WriteLine(Ex);
        }
    }
    /// <summary>
    /// the function intilaize the xml file 
    /// </summary>
    /// <exception cref="FormatException"></exception>
    private static void InitializationData()
    {
        try
        {
            Console.WriteLine("Are you sure you want to create Initial data? (Y/N)"); //stage 3
            string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input"); //stage 3
            if (ans == "Y") //stage 3
            {
                Initialization.Do();
            }
        }
        catch (Exception Ex)
        {
            Console.WriteLine(Ex);
        }
    }
    static void Main(string[] args)
    {
        Menu();
        int choose = int.Parse(Console.ReadLine()!);// the user put his choise
        while (choose != 0)
        {

            switch (choose)
            {
                case 0://exit
                    break;
                case 1://if the user choose worker(1) the worker's sub menu opens for him 
                    SubMenuWorker();

                    if (!int.TryParse(Console.ReadLine(), out int choose1))
                        throw new FormatException("Wrong Input, Try Again");

                    SubMenuWorkerActions(choose1);
                    break;

                case 2://if the user choose _task(2) the _task's sub menu opens for him 
                    SubMenuTask();

                    if (!int.TryParse(Console.ReadLine(), out int choose2))
                        throw new FormatException("Wrong Input, Try Again");

                    SubMenuTaskActions(choose2);
                    break;

                case 3:
                    InitializationData();
                    break;
                case 4:
                    Initialization.Clear();
                    break;
                case 5:
                    Console.WriteLine("Enter the date:");
                    DateTime.TryParse(Console.ReadLine(), out DateTime date);
                    s_bl.SetStartProject(date);
                    break;
            }
            Menu();
            choose = int.Parse(Console.ReadLine()!);
        }
    }
}