namespace DalTest;
using DalApi;
using Do;
using System;
using System.Collections.Generic;

public static class Initialization
{
    //private static IWorker? s_dalWorker; //stage 1
    //private static ITask? s_dalTask; //stage 1
    //private static IDependency? s_dalDependence; //stage 1
    private static IDal? s_dal;
    //private static object _id;
    private static readonly Random s_rand = new();

    /// <summary>
    /// The function create new worker and add to the worker list
    /// </summary>
    private static void CreateWorker()
    {
        string[] workerNames =
        {
        "Avital Shenker", "Ayala Bikel", "Bibi Netanyahu",
        "Eliana Yakobs", "Dana Frider"
    };
        Random rand = new(DateTime.Now.Millisecond);
        foreach (string _name in workerNames)//the user input all the details to add the worker
        {

            int _id = rand.Next(100000000, 999999999);
            string _email = _name.Replace(" ", "") + "@gmail.com";
            int _hourPrice = rand.Next(30, 1000);
            Rank _workerRank = (Rank)rand.Next((int)Rank.Beginner, (int)Rank.Expert + 1);
            Worker _newWorker = new(_id, _workerRank, _hourPrice, _name, _email);
            s_dal?.Worker?.Create(_newWorker);

           // string pass = _id.ToString();
            //User _newUser = new(_name, pass, false);
           // s_dal?.User.Create(_newUser);
        }
        //User _newAdmin1 = new("Gili", "1212", true,325748937);
        //User _newAdmin2 = new("Ayelet", "1111", true,325697795);
        //s_dal?.User.Create(_newAdmin1);
        //s_dal?.User.Create(_newAdmin2);
    }

    /// <summary>
    /// the function create dependence
    /// </summary>
    private static void CreateDependences()
    {
        Dependency[] _idDependences = new Dependency[41];


        _idDependences[0] = new(1, 1, 2);
        _idDependences[1] = new(2, 1, 6);
        _idDependences[2] = new(3, 3, 13);
        _idDependences[3] = new(4, 4, 1);
        _idDependences[4] = new(5, 5, 3);
        _idDependences[5] = new(6, 5, 6);
        _idDependences[6] = new(7, 6, 3);
        _idDependences[7] = new(8, 6, 14);
        _idDependences[8] = new(9, 7, 3);
        _idDependences[9] = new(10, 7, 5);

        _idDependences[10] = new(11, 7, 12);
        _idDependences[11] = new(12, 7, 13);
        _idDependences[12] = new(13, 8, 5);
        _idDependences[13] = new(14, 8, 7);
        _idDependences[14] = new(15, 8, 2);
        _idDependences[15] = new(16, 9, 3);
        _idDependences[16] = new(17, 9, 6);
        _idDependences[17] = new(18, 10, 3);
        _idDependences[18] = new(19, 10, 7);
        _idDependences[19] = new(20, 10, 11);

        _idDependences[20] = new(21, 11, 3);
        _idDependences[21] = new(22, 11, 7);
        _idDependences[22] = new(23, 12, 3);
        _idDependences[23] = new(24, 12, 6);
        _idDependences[24] = new(25, 13, 1);
        _idDependences[25] = new(26, 14, 15);
        _idDependences[26] = new(27, 14, 16);
        _idDependences[27] = new(28, 15, 3);
        _idDependences[28] = new(29, 15, 16);
        _idDependences[29] = new(30, 16, 3);

        _idDependences[30] = new(31, 17, 3);
        _idDependences[31] = new(32, 17, 15);
        _idDependences[32] = new(33, 18, 1);
        _idDependences[33] = new(34, 18, 6);
        _idDependences[34] = new(35, 19, 3);
        _idDependences[35] = new(36, 19, 7);
        _idDependences[36] = new(37, 19, 20);
        _idDependences[37] = new(38, 20, 3);
        _idDependences[38] = new(39, 20, 5);
        _idDependences[39] = new(40, 20, 6);
        for (int i = 0; i < 40; i++)
        {
            s_dal?.Dependency?.Create(_idDependences[i]);
        }

    }
    /// <summary>
    /// the function create task with random details
    /// </summary>
    private static void CreateTask()
    {
        int[] _idTasks = new int[20];
        for (int j = 1; j < 21; j++)//build array of id's  
        {
            _idTasks[j - 1] = 0;
        }
        string[] names =//array of names
            {
            "Market Research",//1 depence on 2, 6
            "Destination Analysis",//2
            "Package Development",//3 depence on 13
            "Price Structuring",//4 depence on 1
            "Marketing Campaign",//5 depence on 3, 6
            "Online Presence Enhancement",//6 depence on 3,14
            "Customer Service Training",//7 depence on 3,5,12,13
            "Collaboration with Local Businesses",//8 depence on 5,7,2
            "Digital Booking System Implementation",//9 depence on 3,6
            "Travel Insurance Options",//10 depence on 3, 7,11
            "Crisis Management Plan",//11 depence on 3, 7
            "Customer Feedback Collection",//12 depence on 3, 6
            "Sustainable Tourism Practices",//13 depence 1
            "Mobile App Development",//14 depence on 15,16
            "Customized Itinerary Planning",//15 depence on 3,16
            "Cross-promotions with Airlines",//16 depence on 3
            "Guided Tour Services",//17 depence on 3,15
            "Online Travel Resources",//18 depence on 1,6
            "Customer Loyalty Program",//19 depence on 3,7,20
            "Performance Analytics"//20 depence on 3,5,6
        };
        string[] descriptions =//array of descriptions
        {
            "Conduct market research to identify emerging travel trends.",
            "Evaluate potential tourist destinations for feasibility and attractiveness. ",
            "Create diverse travel packages catering to different customer preferences. ",
            "Develop competitive pricing strategies for each travel package.",
            "Design and implement a marketing campaign to promote new packages.",
            "Optimize the company's website and social media for better online visibility.",
            "Train staff in providing excellent customer service. ",
            "Establish partnerships with local businesses for mutual benefits. ",
            "Integrate a user-friendly digital booking system. ",
            "Provide information and options for travel insurance. ",
            "Develop a crisis management plan for unexpected events.",
            "Implement a system for collecting and analyzing customer feedback. ",
            "Integrate eco-friendly and sustainable practices. ",
            "Create a mobile app for easier access and bookings. ",
            "Offer personalized itinerary planning services. ",
            "Establish partnerships with airlines for joint promotions. ",
            "Provide experienced guides for specific destinations. ",
            "Create informative content and resources on the company website.",
            "Implement a customer loyalty program with rewards. ",
            "Implement analytics to track key performance indicators."
        };
        string[] proudcts =//array of proudcts
        {
            "Detailed report on current market demands and potential opportunities.",
            "List of recommended destinations with pros and cons.",
            "Portfolio of well-designed travel packages.",
            "Price list for various tour packages.",
            "Increased brand visibility and customer inquiries.",
            "Improved website traffic and engagement on social media.",
            "Improved customer satisfaction and positive reviews.",
            "Enhanced customer experiences and additional services.",
            "Streamlined booking process and improved efficiency.",
            "Increased customer confidence and risk mitigation.",
            "Improved company resilience and customer safety.",
            "Insights for continuous improvement and enhanced customer satisfaction.",
            "Position the company as environmentally responsible.",
            "Increased user engagement and bookings through the app.",
            " Increased customer satisfaction and loyalty.",
            "Discounts or added benefits for customers.",
            "Enhanced customer experience and cultural immersion.",
            "Increased website traffic and authority in the industry.",
            " Increased customer retention and repeat business.",
            " Informed decision-making and continuous improvement."
        };

        Random rand = new(DateTime.Now.Millisecond);
        int i = 0;
        foreach (int _id in _idTasks)//go through the id's array
        {
            string? _name = names[i];//put the right details from the array's
            string _desc = descriptions[i];
            string _product = proudcts[i];
            i++;
            bool _mileStone = false;//put random details
            int _rank = rand.Next(0, 5);
            int _idWorker = 0;
            DateTime tempDate = new(2023, 10, 10);
            int range = (DateTime.Today - tempDate).Days;
            DateTime _createDate = tempDate.AddDays(rand.Next(range));

            int randomHours = rand.Next(24); // 0 to 23
            int randomMinutes = rand.Next(60); // 0 to 59
            int randomSeconds = rand.Next(60); // 0 to 59

            TimeSpan _time = new(randomHours, randomMinutes, randomSeconds);
            Task newTask = new(_id, _idWorker, _name, _desc, _mileStone, _time, _createDate, null, null, null, null, _product, null, _rank);//create task
            s_dal?.Task?.Create(newTask);//add to the list
        }

    }

    /// <summary>
    /// create all the lists
    /// </summary>
    /// <param name="dal"></param>
    /// <exception cref="NullReferenceException"></exception>
    public static void Do()
    {
        // s_dal = dal ?? throw new NullReferenceException("DAL object can not be null!");
        s_dal = DalApi.Factory.Get;
        Clear();
        CreateDependences();
        CreateWorker();
        CreateTask();

    }

    public static void Clear()
    {
        s_dal = DalApi.Factory.Get;
        s_dal.Worker.ClearList();
        s_dal.Dependency.ClearList();
        s_dal.Task.ClearList();
        s_dal.User.ClearList();
        s_dal.SetStartDate(null);
    }
}

