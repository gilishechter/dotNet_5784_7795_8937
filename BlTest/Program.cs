using BO;
using DalApi;
using DalTest;
using Do;
using System.Collections;

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
        Console.WriteLine("3 - to intilize the data");
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
        Console.WriteLine("Enter Id, level (number between 0 - 4), hoour price, name and email");

        if (!int.TryParse(Console.ReadLine(), out int _id))//input the details and check if is right details
            throw new FormatException("Wrong Input, Try Again");//throw exception if is wrong input

        if (!BO.Rank.TryParse(Console.ReadLine(), out BO.Rank _rank))
            throw new FormatException("Wrong Input, Try Again");

        if (!double.TryParse(Console.ReadLine(), out double _hourPrice))
            throw new FormatException("Wrong Input, Try Again");

        string? _name = Console.ReadLine();

        string? _email = Console.ReadLine();

        int? _idTask=int.Parse(Console.ReadLine()!);
        string? _nameTask = Console.ReadLine();
        BO.Worker _worker = new()
        { Id = _id,
            WorkerRank = _rank,
            HourPrice = _hourPrice,
            Name = _name,
            Email = _email,
            WorkerTask = new WorkerTask
            {
                Id = _idTask,
                Name=_nameTask,
            }
        };//build the worker
        Console.WriteLine(s_bl.Worker?.Create(_worker));//add to the list and print the ID's worker

    }

    /// <summary>
    /// this function input id of worker and print the object with the write ID
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

        Console.WriteLine("Enter New Details for level (number between 0 - 4), hour price, name, email and current task:");

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


        int? _idTask = int.Parse(Console.ReadLine()!);
        string? _nameTask = Console.ReadLine(); 
        if(_idTask!=null&&_nameTask!=null)
        {
            worker1.WorkerTask=new WorkerTask 
            { Name = _nameTask,
                Id=_idTask 
            };
       
        }

        BO.Worker _worker = new()
        {
            Id = _id,
            WorkerRank = _rank,
            HourPrice = _hourPrice,
            Name = _name,
            Email = _email,
            WorkerTask = new WorkerTask
            {
                Id = _idTask,
                Name = _nameTask,
            }
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

        if (!BO.Rank.TryParse(Console.ReadLine(), out BO.Rank _rank))
            throw new FormatException("Wrong Input, Try Again");

        IEnumerable<BO.Worker> group = s_bl.Worker.RankGroup(_rank);
        return group;
    }


    /// <summary>
    /// the action menu for the worker entity
    /// </summary>
    /// <param name="choose1"></param>
    private static void SubMenuWorkerAction(int choose1)
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
    static void Main(string[] args)
    {
        Console.Write("Would you like to create Initial data? (Y/N)");
        string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
        if (ans == "Y")
            DalTest.Initialization.Do();

    }
}