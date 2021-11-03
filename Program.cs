using System;
using System.Data.SqlClient;

namespace ConsoleApp1
   //carrots are 1 per 1
   //corn is 3 per 16
   //beets are 9 per 16
{
    class Program
    {
        int userID;
        public SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\snowd\Desktop\CSharpProjects\garden-boxes\ConsoleApp1\ConsoleApp1\Database1.mdf;Integrated Security=True");
        public Program()
        {
           
        }

        void CheckUser()
        {
            string name;
            Console.WriteLine("Welcome to the Make your Garden Adventure Game :D\nWhat is your name?");

            name = Console.ReadLine();
            SqlCommand readCommand = new SqlCommand($"SELECT * FROM Users Where Users = '{name}'", connection);
            SqlDataReader reader = readCommand.ExecuteReader();
            if (reader.Read())
            {
                Console.WriteLine("Welcome back to your Garden Box :D");
                userID = (int)reader["Id"];
                reader.Close();
            }
            else
            {
                reader.Close();
                SqlCommand insertCommand = new SqlCommand($"INSERT INTO Users (Users) Values ('{name}')", connection);
                insertCommand.ExecuteNonQuery();
                reader = readCommand.ExecuteReader();
                reader.Read();
                userID = (int)reader["Id"];
                reader.Close();

            }
        }

        void CreateGardenBox()
        {
            int area, width, length, plantablePlants = 0;
            string plantName = "";
            Console.Clear();
            Console.WriteLine("Please enter the length of your box.");
            length = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("\nPlease enter the width of your box.");
            width = Convert.ToInt32(Console.ReadLine());
            area = (length * width);
            Console.WriteLine();

            while (plantName != "carrots" && plantName != "corn" && plantName != "beets")
            {
                Console.WriteLine("We have 3 plants for you to plant today, you can choose from Corn, Carrots, Beets. ");
                plantName = Console.ReadLine().ToLower();

                if (plantName == "carrots")
                    plantablePlants = area;

                else if (plantName == "corn")
                    plantablePlants = (3 * area / 16);

                else if (plantName == "beets")
                    plantablePlants = (9 * area / 16);

                else
                    Console.WriteLine("Bad input, please try again?");

            }

            SqlCommand command = new SqlCommand($"INSERT INTO [GardenBox] (PlantName, Area, PlantablePlants, UsersID) VALUES ('{plantName}', {area}, {plantablePlants}, {userID}) SELECT @@IDENTITY AS ID", connection);

            command.ExecuteNonQuery();
            Console.WriteLine($"\nInserted new Garden Box with the values. Area: {area}, Plant Name: {plantName}, Plantable Plants: {plantablePlants}.\n");
        }
         
       void PrintTableData()
        {
            Console.Clear();
            SqlCommand readCommand = new SqlCommand($"SELECT * FROM GardenBox Where UsersID = {userID}", connection);
            SqlDataReader reader = readCommand.ExecuteReader();
            Console.WriteLine("Here are all your Garden Boxes\n------------------------------");
            if (!reader.HasRows)
                Console.WriteLine("You have no saved data");
            while (reader.Read())
            {
                Console.WriteLine($"You have a Garden Box with an area of {reader["Area"]} sq ft. that can plant {reader["PlantablePlants"]} {reader["PlantName"]}.\n");
            }
            reader.Close();

        }

        static void Main(string[] args)
        {

            Program program= new Program();
            program.connection.Open();

            program.CheckUser();

            string run = "y";
            while (run == "y"){
                Console.WriteLine("Do you want to (a)dd a new Garden Box, or (p)rint all Garden Boxes");
                string userChoice = Console.ReadLine().ToLower();
                if (userChoice == "a")
                    program.CreateGardenBox();
                else if (userChoice == "p")
                    program.PrintTableData();
                else
                    Console.WriteLine("Invalid Input D:");
                Console.WriteLine("Would you like to run again? Enter 'y' if so.");
                run = Console.ReadLine().ToLower();
            }
        
             program.connection.Close();

        }
    }
}
