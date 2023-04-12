using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MYRIDE
{
    internal class Admin
    {
        List<Driver> drivers;
        public Admin()
        {
            drivers = new List<Driver>();
        }

        public List<Driver> Drivers { get { return drivers; } }

        public void addDriver(string name, int age, string gender, string address, string vType, string vModel, string vLP)
        {
            Driver driver = new Driver();
            driver.Name = name;
            driver.Age = age;
            driver.Gender = gender;
            driver.Address = address;
            driver.Vehicle.Type = vType;
            driver.Vehicle.Model = vModel;
            driver.Vehicle.LicensePlate = vLP;
            //store data in file
            FileStream fout = new FileStream("drivers.txt", FileMode.OpenOrCreate);
            if(fout.Length == 0)
            {
                driver.ID = 100;
                drivers.Add(driver);
                string jsonDriver = JsonSerializer.Serialize(driver);
                StreamWriter sw = new StreamWriter(fout);
                sw.WriteLine(jsonDriver);
                sw.Close();
                fout.Close();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("The Driver has added to the system with ID: ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(driver.ID);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Reminder: As this is a new Driver, they should update:\n1) Current Location\n2) Availability");
                Console.ResetColor();
            }
            else
            {
                fout.Close ();
                if(drivers.Count == 0)
                    loadDriversList("drivers.txt");
                int index = drivers.Count - 1;
                driver.ID = drivers[index].ID + 1;
                drivers.Add(driver);
                StreamWriter sw = new StreamWriter("drivers.txt",append:true);
                string jsonDriver = JsonSerializer.Serialize(driver);
                sw.WriteLine(jsonDriver);
                sw.Close();
                fout.Close();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("The Driver has added to the system with ID: ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(driver.ID);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Reminder: As this is a new Driver, they should update:\n1) Current Location\n2) Availability");
                Console.ResetColor();
            }
        }

        public void loadDriversList(string fileName)
        {
            using (StreamReader sr = new StreamReader(fileName))
            {
                string jsonData;
                Driver driver = new Driver();
                while((jsonData = sr.ReadLine())!= null)
                {
                    driver = JsonSerializer.Deserialize<Driver>(jsonData);
                    drivers.Add(driver);
                }
            }
        }

        public void setRating(Driver d,int stars)
        {
            d.AvgStars = d.AvgStars + stars;
            d.RatingCount = d.RatingCount + 1;
            d.AvgStars = (d.AvgStars / d.RatingCount);
            int c = drivers.Count;
            if(c == 0)
            {
                loadDriversList("drivers.txt");
                c = drivers.Count;
            }
            string jsonData;
            StreamWriter sw = new StreamWriter("drivers.txt");
            for(int i = 0;i<c;i++)
            {
                if (drivers[i].ID == d.ID)
                {
                    drivers[i].AvgStars = d.AvgStars;
                    jsonData= JsonSerializer.Serialize(drivers[i]);
                    sw.WriteLine(jsonData);
                }
                else
                {
                    jsonData = JsonSerializer.Serialize(drivers[i]);
                    sw.WriteLine(jsonData);
                }
            }
            sw.Close();

        }


        public void updateDriverCurrLocation(int id, string currLoc)
        {
            int c = drivers.Count;
            if (c == 0)
            {
                loadDriversList("drivers.txt");
                c = drivers.Count;
            }
            string jsonData;
            StreamWriter sw = new StreamWriter("drivers.txt");
            for (int i = 0; i < c; i++)
            {
                if (drivers[i].ID == id)
                {
                    drivers[i].updateLocation(currLoc);
                    jsonData = JsonSerializer.Serialize(drivers[i]);
                    sw.WriteLine(jsonData);
                }
                else
                {
                    jsonData = JsonSerializer.Serialize(drivers[i]);
                    sw.WriteLine(jsonData);
                }
            }
            sw.Close();
        }

        public void removeDriver(int id)
        {
            int c = drivers.Count;
            if (c == 0)
            {
                loadDriversList("drivers.txt");
                c = drivers.Count;
            }
            for (int i = 0; i < c; i++)
            {
                if (drivers[i].ID == id)
                {
                    deleteDriverFromFile("drivers.txt", id);
                    drivers.RemoveAt(i);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Driver is Removed Successfully");
                    Console.ResetColor();
                    return;
                }
            }
        }
        public void deleteDriverFromFile(string fileName, int id)
        {
            List<Driver> remainingDrivers = new List<Driver>();
            string jsonData;
            using (StreamReader sr = new StreamReader(fileName))
            {
                Driver driver = new Driver();
                while ((jsonData = sr.ReadLine()) != null)
                {
                    driver = JsonSerializer.Deserialize<Driver>(jsonData);
                    if(driver.ID == id)
                    {
                        Console.WriteLine($"Driver {jsonData}");
                    }
                    else
                        remainingDrivers.Add(driver);   
                }
                sr.Close();
                StreamWriter sw = new StreamWriter(fileName);
                int rDriverCount = remainingDrivers.Count;
                Console.WriteLine(rDriverCount);
                for (int i = 0; i<rDriverCount;i++)
                {
                    jsonData = JsonSerializer.Serialize(remainingDrivers[i]);
                    sw.WriteLine(jsonData);
                }
                sw.Close();
            }
        }

        public void updateDriver(int id)
        {
            int c = drivers.Count;
            if(c==0)
            {
                loadDriversList("drivers.txt");
                c = drivers.Count;
            }
            string jsonData;
            StreamWriter sw = new StreamWriter("drivers.txt");
            for (int i = 0; i < c; i++)
            {
                if (drivers[i].ID == id)
                {
                    Console.WriteLine("------------Driver with ID " + id + " exists------------");
                    Console.Write("Enter Name: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    string name = Console.ReadLine();
                    Console.ResetColor();
                    if (name != string.Empty)
                    {
                        drivers[i].Name = name;
                    }
                    Console.Write("Enter Age: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    string age = Console.ReadLine();
                    Console.ResetColor();
                    if (age != string.Empty)
                    {
                        int driverAge = Convert.ToInt32(age);
                        drivers[i].Age = driverAge;
                    }
                    Console.Write("Enter Gender: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    string gender = Console.ReadLine();
                    Console.ResetColor();
                    if (gender != string.Empty)
                    {
                        drivers[i].Gender = gender;
                    }
                    Console.Write("Enter Address: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    string address = Console.ReadLine();
                    Console.ResetColor();
                    if (address != string.Empty)
                    {
                        drivers[i].Address = address;
                    }
                    Console.Write("Enter Vehicle Type (Car/Bike/Rickshaw): ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    string vehicleType = Console.ReadLine();
                    Console.ResetColor();
                    if (vehicleType != string.Empty)
                    {
                        drivers[i].Vehicle.Type = vehicleType;
                    }
                    Console.Write("Enter Vehicle Model: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    string vehicleModel = Console.ReadLine();
                    Console.ResetColor();
                    if (vehicleModel != string.Empty)
                    {
                        drivers[i].Vehicle.Model = vehicleModel;
                    }
                    Console.Write("Enter Vehicle License Plate: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    string vehicleLP = Console.ReadLine();
                    Console.ResetColor();
                    if (vehicleLP != string.Empty)
                    {
                        drivers[i].Vehicle.LicensePlate = vehicleLP;
                    }
                    jsonData = JsonSerializer.Serialize(drivers[i]);
                    sw.WriteLine(jsonData);
                }
                else
                {
                    jsonData = JsonSerializer.Serialize(drivers[i]);
                    sw.WriteLine(jsonData);
                }
            }
            sw.Close();

        }

       
        public void updateAvailability(int id, bool val)
        {
            int c = drivers.Count;
            if (c == 0)
            {
                loadDriversList("drivers.txt");
                c = drivers.Count;
            }
            StreamWriter sw = new StreamWriter("drivers.txt");
            string jsonData;
            for (int i = 0; i < c; i++)
            {
                if (drivers[i].ID == id)
                {
                    drivers[i].updateAvailability(val);
                    jsonData = JsonSerializer.Serialize(drivers[i]);
                    sw.WriteLine(jsonData);
                }
                else
                {
                    jsonData = JsonSerializer.Serialize(drivers[i]);
                    sw.WriteLine(jsonData);
                }
            }
            sw.Close();
        }

        public void updateStarsInFile(int id)
        {
            int c = drivers.Count;
            if (c == 0)
            {
                loadDriversList("drivers.txt");
                c = drivers.Count;
            }
            StreamWriter sw = new StreamWriter("drivers.txt");
            string jsonData;
            for (int i = 0; i < c; i++)
            {
                if (drivers[i].ID == id)
                {
                    drivers[i].calculateRating();
                    jsonData = JsonSerializer.Serialize(drivers[i]);
                    sw.WriteLine(jsonData);
                }
                else
                {
                    jsonData = JsonSerializer.Serialize(drivers[i]);
                    sw.WriteLine(jsonData);
                }
            }
            sw.Close();
        }

        public void displayDriver(Driver driver)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("----------------------------------------------------------------------------");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Name           Age       Gender       V.Type        V.Model        V.License");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("----------------------------------------------------------------------------");
            Console.ResetColor();
            Console.WriteLine(driver.Name + "   " + driver.Age + "   " + driver.Gender + "   " + driver.Vehicle.Type + "   " + driver.Vehicle.Model + "     " + driver.Vehicle.LicensePlate);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("----------------------------------------------------------------------------");
            Console.ResetColor();
        }
        
        public void searchDriver()
        {
            int c = drivers.Count;
            if(c == 0)
            {
                loadDriversList("drivers.txt");
                c = drivers.Count;
            }
            int byID = -1;
            int[] byName = new int[c];
            int[] byAge = new int[c];
            int[] byGender = new int[c];
            int[] byAddress = new int[c];
            int[] byVehicleType = new int[c];
            int[] byVehicleModel = new int[c];
            int[] byVehicleLP = new int[c];
            Console.Write("Enter Driver ID: ");
            Console.ForegroundColor = ConsoleColor.Green;
            string id = Console.ReadLine();
            Console.ResetColor();
            if (id != string.Empty)
            {
                int ID = Convert.ToInt32(id);
                for (int i = 0; i < c; i++)
                {
                    if (drivers[i].ID == ID)
                    {
                        byID = i;
                    }
                }
            }
            Console.Write("Enter Name: ");
            Console.ForegroundColor = ConsoleColor.Green;
            string name = Console.ReadLine();
            Console.ResetColor();
            if (name != string.Empty)
            {
                for (int i = 0; i < c; i++)
                {
                    if (drivers[i].Name == name)
                    {
                        byName[i] = 1;
                    }
                }
            }
            Console.Write("Enter Age: ");
            Console.ForegroundColor = ConsoleColor.Green;
            string age = Console.ReadLine();
            Console.ResetColor();
            if (age != string.Empty)
            {
                int ageID = Convert.ToInt32(age);
                for (int i = 0; i < c; i++)
                {
                    if (drivers[i].Age == ageID)
                    {
                        byAge[i] = 1;
                    }
                }
            }
            Console.Write("Enter Gender: ");
            Console.ForegroundColor = ConsoleColor.Green;
            string gender = Console.ReadLine();
            Console.ResetColor();
            if (gender != string.Empty)
            {
                for (int i = 0; i < c; i++)
                {
                    if (drivers[i].Gender == gender)
                    {
                        byGender[i] = 1;
                    }
                }
            }
            Console.Write("Enter Address: ");
            Console.ForegroundColor = ConsoleColor.Green;
            string address = Console.ReadLine();
            Console.ResetColor();
            if (address != string.Empty)
            {
                for (int i = 0; i < c; i++)
                {
                    if (drivers[i].Address == address)
                    {
                        byAddress[i] = 1;
                    }
                }
            }
            Console.Write("Enter Vehicle Type (Car/Bike/Rickshaw): ");
            Console.ForegroundColor = ConsoleColor.Green;
            string vehicleType = Console.ReadLine();
            Console.ResetColor();
            if (vehicleType != string.Empty)
            {
                for (int i = 0; i < c; i++)
                {
                    if (drivers[i].Vehicle.Type == vehicleType)
                    {
                        byVehicleType[i] = 1;
                    }
                }
            }
            Console.Write("Enter Vehicle Model: ");
            Console.ForegroundColor = ConsoleColor.Green;
            string vehicleModel = Console.ReadLine();
            Console.ResetColor();
            if (vehicleModel != string.Empty)
            {
                for (int i = 0; i < c; i++)
                {
                    if (drivers[i].Vehicle.Model == vehicleModel)
                    {
                        byVehicleModel[i] = 1;
                    }
                }
            }
            Console.Write("Enter Vehicle License Plate: ");
            Console.ForegroundColor = ConsoleColor.Green;
            string vehicleLP = Console.ReadLine();
            Console.ResetColor();
            if (vehicleLP != string.Empty)
            {
                for (int i = 0; i < c; i++)
                {
                    if (drivers[i].Vehicle.LicensePlate == vehicleLP)
                    {
                        byVehicleLP[i] = 1;
                    }
                }
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Results by ID");
            Console.ResetColor();
            //byID
            if (byID != -1)
                displayDriver(drivers[byID]);
            //byName
            if (byName.Length > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Results by Name");
                Console.ResetColor();
                for (int i = 0; i < c; i++)
                {
                    if (byName[i] == 1)
                        displayDriver(drivers[i]);
                }
            }
            //byAge
            if (byAge.Length > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Results by Age");
                Console.ResetColor();
                for (int i = 0; i < c; i++)
                {
                    if (byAge[i] == 1)
                        displayDriver(drivers[i]);
                }
            }
            //byGender
            if (byGender.Length > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Results by Gender");
                Console.ResetColor();
                for (int i = 0; i < c; i++)
                {
                    if (byGender[i] == 1)
                        displayDriver(drivers[i]);
                }
            }
            //byAddress
            if (byAddress.Length > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Results by Address");
                Console.ResetColor();
                for (int i = 0; i < c; i++)
                {
                    if (byAddress[i] == 1)
                        displayDriver(drivers[i]);
                }
            }
            //byVehicleType
            if (byVehicleType.Length > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Results by Vehicle Type");
                Console.ResetColor();
                for (int i = 0; i < c; i++)
                {
                    if (byVehicleType[i] == 1)
                        displayDriver(drivers[i]);
                }
            }
            //byVehicelModel
            if (byVehicleModel.Length > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Results by Vehicle Model");
                Console.ResetColor();
                for (int i = 0; i < c; i++)
                {
                    if (byVehicleModel[i] == 1)
                        displayDriver(drivers[i]);
                }
            }
            //byVehicelLP
            if (byVehicleLP.Length > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Results by Vehicle License Plate");
                Console.ResetColor();
                for (int i = 0; i < c; i++)
                {
                    if (byVehicleLP[i] == 1)
                        displayDriver(drivers[i]);
                }
            }

        }


        public bool searchDriverByID(int id)
        {
            int c = drivers.Count;
            if(c == 0)
            {
                loadDriversList("drivers.txt");
                c = drivers.Count;
            }
            for (int i = 0; i < c; i++)
            {
                if (drivers[i].ID == id)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
