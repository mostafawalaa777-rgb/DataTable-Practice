using System;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Security.AccessControl;

namespace Example
{
    internal class Program
    {
      
     
        static DataTable CreatEmployeesTable ()
        {
            DataTable Table = new DataTable ();
            Table.Columns.Add("ID", typeof (int));
            Table.Columns.Add("Name", typeof (string));
            Table.Columns.Add("Country", typeof (string));
            Table.Columns.Add("Salary", typeof (Double));
            Table.Columns.Add("Date", typeof (DateTime));

            //Make id column the Primary Key Column.
            DataColumn[] PrimaryKeyColumn = new DataColumn[1];
            PrimaryKeyColumn[0] = Table.Columns["ID"];
            Table.PrimaryKey = PrimaryKeyColumn;

            //Add Rows
            Table.Rows.Add(1, "Walaa Mostafaa", "Egypt", 10000, DateTime.Now);
            Table.Rows.Add(2, "Doaa Mostafa", "Egypt", 5000, new DateTime(2015, 12, 22));
            Table.Rows.Add(3, "Shimaa Mostafa", "Iraq", 8000, new DateTime(2013, 1, 30));
            Table.Rows.Add(4, "Mohamed Mostafa", "Dobai", 11000, new DateTime(2020, 7, 10));
            Table.Rows.Add(5, "Amr Mostafa", "Syria", 15000, new DateTime(2011, 8, 13));

            
            return Table;
        }

        static void CreatColumns(DataTable DT, string Name, Type type, bool IsAutoIncrement, bool IsReadOnly,
          bool IsUnique, int AutoIncrementSpeed = 1, int AutoIncremntStep = 1, string Caption = "Null")
        {
            DataColumn DC = new DataColumn();
            DC.ColumnName = Name;
            DC.DataType = type;
            DC.AutoIncrement = IsAutoIncrement;
            DC.ReadOnly = IsReadOnly;
            DC.Unique = IsUnique;

            if (IsAutoIncrement)
            {
                DC.AutoIncrementSeed = AutoIncrementSpeed;
                DC.AutoIncrementStep = AutoIncremntStep;
            }
            if (Caption != "Null")
            {
                DC.Caption = Caption;
            }
            DT.Columns.Add(DC);
        }

        static void SetPrimaryKey(DataTable DT, int ID)
        {
           DataColumn[] PrimaryKeyColumn = new DataColumn[1];
           PrimaryKeyColumn[ID] = DT.Columns["ID"];
           DT.PrimaryKey = PrimaryKeyColumn;
        }


        static void AddEmployees(DataTable DT)
        { 
            DT.Rows.Add(null, "Walaa Mostafaa", "Egypt", 10000, DateTime.Now);
            DT.Rows.Add(null, "Doaa Mostafa", "Egypt", 5000, new DateTime(2015, 12, 22));
            DT.Rows.Add(null, "Shimaa Mostafa", "Iraq", 8000, new DateTime(2013, 1, 30));
            DT.Rows.Add(null, "Mohamed Mostafa", "Dobai", 11000, new DateTime(2020, 7, 10));
            DT.Rows.Add(null, "Amr Mostafa", "Syria", 15000, new DateTime(2011, 8, 13));
        }

        static void PrintEmployees(DataTable table)
        {
            foreach (DataRow row in table.Rows)
            {
                Console.WriteLine($"ID : {row["id"]} \t  Name : {row["Name"]} \t Country :{row["Country"]} \t Salary : {row["Salary"]} \t Date  : {((DateTime)row["Date"]).ToShortDateString()}"); 
            }
            Console.WriteLine(new string('-', 100));

        }

        static void PrintStatistics (DataTable table)
        {
            int EmployeesCount = 0;
            double TotalSalaries = 0, AverageSalaries = 0, MinSalary = 0, MaxSalary = 0;

            if (table.Rows.Count == 0)
            {
                Console.WriteLine("No data to calculate statistics.");
                return;
            }
            EmployeesCount  = table.Rows.Count;
            TotalSalaries   = Convert.ToDouble(table.Compute("Sum(Salary)", string.Empty));
            AverageSalaries = Convert.ToDouble(table.Compute("AVG(Salary)", string.Empty));
            MaxSalary       = Convert.ToDouble(table.Compute("Max(Salary)", string.Empty));
            MinSalary       = Convert.ToDouble(table.Compute("Min(Salary)", string.Empty));

            Console.WriteLine("\n");

            Console.WriteLine($"Employees Count : {EmployeesCount}");
            Console.WriteLine("Average Salaries = " + AverageSalaries);
            Console.WriteLine("TotalSalaries    = " + TotalSalaries);
            Console.WriteLine("MaxSalary        = " + MaxSalary);
            Console.WriteLine("MinSalary        = " + MinSalary);
            Console.WriteLine("\n");

            Console.WriteLine(new string('-', 100));
        }

        static DataTable FilterByCountry(DataTable Table, string Country)
        {

            DataRow[] resultRows = Table.Select($"Country ='{Country}' ");
            DataTable result = Table.Clone(); //copies Columns only
            Console.WriteLine("\n\nEmployees List Filterd by  country : \n");

            foreach (DataRow row in resultRows)
            {
                result.Rows.Add(row.ItemArray);   // copies Data
            }
            return result;
        }
        static DataTable FilterBy2Countries(DataTable Table, string Country1, string Country2)
        {

            DataRow[] resultRows = Table.Select($"Country ='{Country1}' or Country = '{Country2}'");
            DataTable result = Table.Clone(); //copies Columns only
            Console.WriteLine("\n\nEmployees List Filterd by 2 countries : \n");

            foreach (DataRow row in resultRows)
            {
                result.Rows.Add(row.ItemArray);   // copies Data
            }
            return result;
        }

        static DataTable FilteredByID(DataTable Table, int ID)
        {
            DataRow[] ResaltRow = Table.Select($"ID = {ID} ");
            DataTable Result = Table.Clone();
            Console.WriteLine("\n\nEmployees List Filterd by ID : \n");

            foreach (DataRow row in ResaltRow) 
            {
                Result.Rows.Add(row.ItemArray);
            }
            return Result;
        }
        
        static DataTable SortingDataByID(DataTable Table )
        {
             Table.DefaultView.Sort = "ID desc";
            Table = Table.DefaultView.ToTable();
            DataTable Result = Table.Clone();

            Console.WriteLine("\n\nEmployees List sorted by ID Desc: \n");
            foreach (DataRow row in Table.Rows)
            {
                Result.Rows.Add (row.ItemArray);
            }
            return Result;
        }

        static DataTable SortingDataByName(DataTable Table)
        {
            Table.DefaultView.Sort = "Name Asc";
            Table = Table.DefaultView.ToTable();
            DataTable Result = Table.Clone();
            Console.WriteLine("\n\nEmployees List sorted by Name ASC: \n");

            foreach (DataRow Row in Table.Rows)
            {
                Result.Rows.Add(Row.ItemArray);
            }
            return Result;
        }

        static DataTable DeleteData (DataTable table , int ID)
        {
            DataRow[] ResultRows = table.Select($"ID = {ID}");
            DataTable Result = table.Clone();
            Console.WriteLine("\n\nEmployees List Deleted by ID : \n");

            foreach (var RecordRow  in ResultRows)
            {

                RecordRow.Delete();
            }
            table.AcceptChanges();
            return Result;
        }

        static DataTable UpdatedData (DataTable Table, int ID)
        {
            DataRow[] ResultRow = Table.Select($"ID = {ID}");
            DataTable Result = Table.Clone();
            Console.WriteLine("\n\nEmployees List Update by ID : \n");
            
            foreach(var RecordRow in ResultRow)
            {
                RecordRow["Name"] = "Tayiem Ayman";
                RecordRow["Salary"] = 10000;
            }
            
            Table.AcceptChanges();
           
            foreach (DataRow  Row in Table.Rows)
            {
                Result.Rows.Add(Row.ItemArray);
            }
            return Result;
            Console.ReadKey();
        }

        static void ClearData(DataTable Table)
        {
           Table.Clear();
            Console.WriteLine("No Data here");
        }

        static DataTable RestorData(DataTable BackUp)
        {
            return BackUp.Copy () ;
        }


        public static void Main(string[] args)
        {
            //// examble 1: Create Offline data table and ListData
            DataTable Employeesdt = new DataTable();

            CreatColumns(Employeesdt, "ID", typeof(int), true, true, true);
            CreatColumns(Employeesdt, "Name", typeof(string), false, false, false);
            CreatColumns(Employeesdt, "Country", typeof(string), false, false, false);
            CreatColumns(Employeesdt, "Salary", typeof(double), false, false, false);
            CreatColumns(Employeesdt, "Date", typeof(DateTime), false, false, false);

            SetPrimaryKey(Employeesdt, 0);

            AddEmployees(Employeesdt);

            PrintEmployees(Employeesdt);
           
            //// examble 2: Aggregate functions (CCount, sum, Avg, Min, Max)
            PrintStatistics(Employeesdt);

            //// examble 3: (Filter Data and List) 
            //DataTable FilterData = FilterByCountry(Employeesdt, "Iraq");
            //PrintEmployees(FilterData);
            //PrintStatistics(FilterData);

            //// examble 4 : (Filter Data and List 2 Varyabled) 
            //DataTable FilteredData = FilterBy2Countries(Employeesdt,  "Egypt"  , "Syria");
            //PrintEmployees(FilteredData);
            //PrintStatistics (FilteredData);

            //// examble 5 : (Filter BY ID) 
            //DataTable FilteredID = FilteredByID(Employeesdt, 1);
            //PrintEmployees(FilteredID);
            //PrintStatistics(FilteredID);

            //// examble 6 : (Sorting Data (ID) ) 
            //DataTable SortedID = SortingDataByID(Employeesdt);
            //PrintEmployees(SortedID);
            //PrintStatistics(SortedID);

            //// examble 7 : (Sorting Data (Name) ) 
            //DataTable SortedName = SortingDataByName(Employeesdt);
            //PrintEmployees(SortedName);
            //PrintStatistics(SortedName);

            //// examble 8 : (Delet Data (id) ) 
            //DataTable DeletedData = DeleteData(Employeesdt, 3);
            //PrintEmployees(DeletedData);
            //PrintStatistics (DeletedData);

            //// examble 8 : (Update Data (id) ) 
            //DataTable updateData = UpdatedData(Employeesdt, 4);
            //PrintEmployees(updateData);
            //PrintStatistics(updateData);

            // examble 9 : (Clear Data ) 
            //ClearData(Employeesdt);
            RestorData(Employeesdt);

            PrintEmployees(Employeesdt);
            PrintStatistics(Employeesdt);
        }
    } 
}


