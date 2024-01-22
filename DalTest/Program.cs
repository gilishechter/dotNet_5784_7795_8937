namespace DalTest;
using Dal;
using DalApi;
using Do;
using System;
//using System.ComponentModel;
//using System.Security.Cryptography;
//using System.Runtime.CompilerServices;

internal class Program
{

    //static readonly IDal s_dal = new DalList();
    static readonly IDal s_dal = new DalXml();
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
        Console.WriteLine("4 - to intilize the data");
        //string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input"); //stage 3
        //if (ans == "Y")
        //{  //stage 3
        //    Initialization.Do(s_dal); //stage 2
        //    Menu();
        //}
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
        if (!int.TryParse(Console.ReadLine(), out int _id))//input the details and check if is right details
            throw new FormatException("Wrong Input, Try Again");//throw exception if is wrong input
        if(!Rank.TryParse(Console.ReadLine(), out Rank _rank))
            throw new FormatException("Wrong Input, Try Again");
        if(!double.TryParse(Console.ReadLine(), out double _hourPrice))
            throw new FormatException("Wrong Input, Try Again");
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
        //the user input the details that he want to add
        Console.WriteLine("Enter Id worker, name, description, mile stone, time, create date, " +
            "wanted start date, start date, end date, dead line, product, notes and level between 0 - 4");
        //if(!int.TryParse(Console.ReadLine(), out int Id))
           // throw new WrongInputException("Wrong Input, Try Again");
        if (!int.TryParse(Console.ReadLine(), out int IdWorker))//input the details and check if is right details
            throw new FormatException("Wrong Input, Try Again");//throw exception if is wrong input
        string? Name = Console.ReadLine();
        string? Description = Console.ReadLine();
        if(!bool.TryParse(Console.ReadLine(), out bool MileStone))
            throw new FormatException("Wrong Input, Try Again");
        if(!TimeSpan.TryParse(Console.ReadLine(), out TimeSpan Time))
            throw new FormatException("Wrong Input, Try Again");
        if(!DateTime.TryParse(Console.ReadLine(), out DateTime CreateDate))
            throw new FormatException("Wrong Input, Try Again");
        if(!DateTime.TryParse(Console.ReadLine(), out DateTime WantedStartDate))
            throw new FormatException("Wrong Input, Try Again");
        if(!DateTime.TryParse(Console.ReadLine(), out DateTime StartDate))
            throw new FormatException("Wrong Input, Try Again");
        if(!DateTime.TryParse(Console.ReadLine(), out DateTime EndingDate))
            throw new FormatException("Wrong Input, Try Again");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime DeadLine))
            throw new FormatException("Wrong Input, Try Again");
        string? Product = Console.ReadLine();
        string? Notes = Console.ReadLine();
        if(!int.TryParse(Console.ReadLine(), out int Rank))
            throw new FormatException("Wrong Input, Try Again");
        Task _Task = new(0, IdWorker, Name, Description, MileStone, Time, CreateDate
            , WantedStartDate, StartDate, EndingDate, DeadLine, Product, Notes, Rank);//build the task object
        Console.WriteLine(s_dal.Task?.Create(_Task));//add to the list and print the Id's task


    }
    /// <summary>
    /// this function input details to a dependence and add it to the list
    /// </summary>
    private static void AddDependence()
    {
        Console.WriteLine("Enter dependence task and previous task");
        //if(!int.TryParse(Console.ReadLine(), out int _id))
            //throw new WrongInputException("Wrong Input, Try Again");
        if(!int.TryParse(Console.ReadLine(), out int _DependenceTask))//input the details
            throw new FormatException("Wrong Input, Try Again");
        if(!int.TryParse(Console.ReadLine(), out int _PrevTask))
            throw new FormatException("Wrong Input, Try Again");
        Dependency _Dependence = new(0, _DependenceTask, _PrevTask);//build the dependence
        Console.WriteLine(s_dal.Dependency?.Create(_Dependence));//add to the list and print the ID

    }
    /// <summary>
    /// this function input id of worker and print the object with the write ID
    /// </summary>
    private static void WorkerObjectView()
    {
        Console.WriteLine("Enter Id for print");
        if(!int.TryParse(Console.ReadLine(), out int _id))
            throw new FormatException("Wrong Input, Try Again");
        if (s_dal.Worker?.Read(_id) == null)
            throw new DalDoesNotExistException($"this ID worker={_id} doesn't exist");
        Console.WriteLine(s_dal.Worker?.Read(_id));
    }
    /// <summary>
    /// this function input id of task and print the object with the write ID
    /// </summary>
    private static void TaskObjectView()
    {
        Console.WriteLine("Enter Id for print");
        if(!int.TryParse(Console.ReadLine(), out int _id))
            throw new FormatException("Wrong Input, Try Again");
        if(s_dal.Task?.Read(_id)==null)
            throw new DalDoesNotExistException($"this ID task={_id} doesn't exist");
        Console.WriteLine(s_dal.Task?.Read(_id));
    }
    /// <summary>
    /// this function input id of dependence and print the object with the write ID
    /// </summary>
    private static void DependenceObjectView()
    {
        Console.WriteLine("Enter Id for print");
        if(!int.TryParse(Console.ReadLine(), out int _id))
            throw new FormatException("Wrong Input, Try Again");
        if(s_dal.Dependency?.Read(_id)==null)
            throw new DalDoesNotExistException($"this ID dependence={_id} doesn't exist");
        Console.WriteLine(s_dal.Dependency?.Read(_id));
    }
    /// <summary>
    /// this function input the wanted details to update and build new worker
    /// </summary>
    private static void UpdateWorker()
    {
        Console.WriteLine("Enter Id:");
        if(!int.TryParse(Console.ReadLine(), out int _id))//check if the user put right input and throw exception if he put wrong input
            throw new FormatException("Wrong Input, Try Again");
        Console.WriteLine(s_dal.Worker!.Read(_id));
        Worker worker1 = s_dal.Worker.Read(_id)!;
        Console.WriteLine("Enter New Details for level (number between 0 - 4), hour price, name and email:");

        if (!Rank.TryParse(Console.ReadLine(), out Rank _rank))//if the user doesnt put input we take the old input
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
    /// <summary>
    /// this function input the wanted details to update and build new task
    /// </summary>
    /// <exception cref="WrongInputException"></exception>
    private static void UpdateTask()
    {
        Console.WriteLine("Enter Id:");
        if(!int.TryParse(Console.ReadLine(), out int Id))
            throw new FormatException("Wrong Input, Try Again");
        Console.WriteLine(s_dal.Task!.Read(Id));
        Task? task1 = s_dal.Task.Read(Id);
        Console.WriteLine("Enter New Details for Enter Id, Id worker, name, description, mile stone, time, create date,");
        Console.WriteLine("wanted start date, start date, end date, dead line, product, notes and level between 0 - 4");
        if (!int.TryParse(Console.ReadLine(), out int IdWorker))
            IdWorker = task1!.IdWorker;
        string? Name = Console.ReadLine();
        if (Name == "")
            Name = task1!.Name;

        string? Description = Console.ReadLine();
        if (Description == "")
            Description = task1!.Description;

       if(!bool.TryParse(Console.ReadLine(), out bool MileStone))
            throw new FormatException("Wrong Input, Try Again");
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
    /// <summary>
    /// this function input the wanted details to update and build new dependence
    /// </summary>
    /// <exception cref="WrongInputException"></exception>
    private static void UpdateDependence()
    {
        Console.WriteLine("Enter Id:");
        if(!int.TryParse(Console.ReadLine(), out int _id))
            throw new FormatException("Wrong Input, Try Again");
        Console.WriteLine(s_dal.Dependency!.Read(_id));
        Dependency? dependence1 = s_dal.Dependency.Read(_id);
        if (dependence1 != null)
        {
            Console.WriteLine("Enter New Details for dependence task and previous task:");
            if (!int.TryParse(Console.ReadLine(), out int _DependenceTask))
                _DependenceTask = dependence1.DependenceTask;
            if (!int.TryParse(Console.ReadLine(), out int PrevTask))
                PrevTask = dependence1.PrevTask;
            Dependency _Dependence = new(_id, _DependenceTask, PrevTask);
            s_dal.Dependency?.Update(_Dependence);
        }
    }
    /// <summary>
    /// this function ask from user id and delete the dependence with this id
    /// </summary>
    /// <exception cref="WrongInputException"></exception>
    private static void DeleteDependence()
    {
        Console.WriteLine("Enter Id to delete");
        if(!int.TryParse(Console.ReadLine(), out int _id))
            throw new FormatException("Wrong Input, Try Again");
        s_dal.Dependency?.Delete(_id);
    }
    /// <summary>
    /// this function ask from user id and delete the task with this id
    /// </summary>
    /// <exception cref="WrongInputException"></exception>
    private static void DeleteTask()
    {
        Console.WriteLine("Enter Id to delete");
        if(!int.TryParse(Console.ReadLine(), out int _id))
            throw new FormatException("Wrong Input, Try Again");
        s_dal.Task?.Delete(_id);
    }
    /// <summary>
    /// this function ask from user id and delete the dependence with this id
    /// </summary>
    /// <exception cref="WrongInputException"></exception>
    private static void DeleteWorker()
    {
        Console.WriteLine("Enter Id to delete");
        if(!int.TryParse(Console.ReadLine(), out int _id))
            throw new FormatException("Wrong Input, Try Again");
        s_dal.Worker?.Delete(_id);
    }
    /// <summary>
    /// this function show the list of workers
    /// </summary>
    private static void WorkerListView()
    {
        Console.WriteLine("workers list:");
        IEnumerable<Worker?> workerList = s_dal.Worker!.ReadAll();
        foreach (Worker? worker in workerList)//The for goes through all the elements in the list and prints them to the user
        {
            Console.WriteLine(worker);
        }
    }
    /// <summary>
    /// this function show the list of tasks
    /// </summary>
    private static void TaskListView()
    {
        Console.WriteLine("tasks list:");
        IEnumerable<Task?> taskList = s_dal.Task!.ReadAll();
        foreach (Task? task in taskList)//The for goes through all the elements in the list and prints them to the user
        {
            Console.WriteLine(task);
        }
    }
    /// <summary>
    /// this function show the list of dependencies
    /// </summary>
    private static void DependenceListView()
    {
        Console.WriteLine("dependences list:");
        IEnumerable<Dependency?> dependenceList = s_dal.Dependency!.ReadAll();
        foreach (Dependency? dep in dependenceList)//The for goes through all the elements in the list and prints them to the user
            Console.WriteLine(dep);
        

    }

    private static void ClearAll()
    {
        try
        {
            Console.WriteLine("Are you sure you want to create Initial data? (Y/N)"); //stage 3
            string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input"); //stage 3
            if (ans == "Y") //stage 3
            {
                s_dal.Worker.ClearList();
                s_dal.Dependency.ClearList();
                s_dal.Task.ClearList();
                Initialization.Do(s_dal);
            }
        }
        catch (Exception Ex)
        {
            Console.WriteLine(Ex);
        }
    }

    /// <summary>
    /// the action menu for the worker entity
    /// </summary>
    /// <param name="choose1"></param>
    private static void SubMenuWorker(int choose1)
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
            }
        }
        catch (Exception Ex)
        {
            Console.WriteLine(Ex);
        }
    }
    /// <summary>
    /// the action menu for the task entity
    /// </summary>
    /// <param name="choose1"></param>
    private static void SubMenuTask(int choose1)
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
    /// the action menu for the dependence entity
    /// </summary>
    /// <param name="choose1"></param>
    private static void SubMenuDependence(int choose1)
    {
        try
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
                        SubMenu();
                        if (!int.TryParse(Console.ReadLine(), out int choose1))
                            throw new FormatException("Wrong Input, Try Again");
                        SubMenuWorker(choose1);
                        break;
                    case 2://if the user choose task(2) the task's sub menu opens for him 
                        SubMenu();
                        if (!int.TryParse(Console.ReadLine(), out int choose2))
                            throw new FormatException("Wrong Input, Try Again");
                        SubMenuTask(choose2);
                        break;
                    case 3://if the user choose dependence(3) the dependence's sub menu opens for him 
                        SubMenu();
                        if (!int.TryParse(Console.ReadLine(), out int choose3))
                            throw new FormatException("Wrong Input, Try Again");
                        SubMenuDependence(choose3);
                        break;

                    case 4:
                       ClearAll();
                       break;


                }



            Menu();
                choose = int.Parse(Console.ReadLine()!);                                                                       
        }
       

    }
}