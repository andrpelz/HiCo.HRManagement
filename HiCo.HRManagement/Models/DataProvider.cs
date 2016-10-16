using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace HiCo.HRManagement.Models
{
    public enum EditStatus
    {
        OK = 1,
        Error = -1,
        PathNotFound = -2,
        EntityNotFound = -3
    }

    public class DataProvider
    {
        #region -------------------- Variablen/Konstruktor --------------------
        public const string EmployeeFilename = "Employee.dat";
        public const string AddressFilename = "Address.dat";

        private string DataPath = "";

        public DataProvider(string aDataPath)
        {
            DataPath = aDataPath;
        }
        #endregion

        #region -------------------- Public Datenzugriff Employees --------------------
        public List<EmployeeModel> GetEmployees()
        {
            List<EmployeeModel> emps = ReadEmployeeData();
            List<AddressModel> adrs = ReadAddressData();

            // Wenn keine Daten im File stehen, welche generrieren
            if (emps.Count == 0)
            {
                emps = GenerateDefaultEmployees();
                adrs = GenerateDefaultAddresses();
                WriteEmployeeData(emps);
                WriteAddressData(adrs);
            }

            // Addressen zu den Mitarbeitern suchen
            foreach (EmployeeModel emp in emps)
            {
                emp.Addresses = adrs.Where(a => a.EmployeeId == emp.Id).ToList();
            }

            // Liste sortieren
            emps.Sort((x, y) =>
            {
                int result = string.Compare(x.FirstName, y.FirstName);
                if (result == 0)
                    result = string.Compare(x.LastName, y.LastName);
                return result;
            });

            return emps;
        }

        public EmployeeModel GetSingleEmployee(int aEmployeeID)
        {
            List<EmployeeModel> emps = ReadEmployeeData();
            List<AddressModel> adrs = ReadAddressData();

            EmployeeModel ret = emps.FirstOrDefault(e => e.Id == aEmployeeID);
            if (ret != null) ret.Addresses = adrs.Where(a => a.EmployeeId == ret.Id).ToList();
            return ret;
        }

        public EditStatus InsertEmployee(EmployeeModel aEmployee)
        {
            List<EmployeeModel> emps = ReadEmployeeData();
            int next_id = emps.Max(e => e.Id) + 1;

            aEmployee.Id = next_id;
            emps.Add(aEmployee);

            return WriteEmployeeData(emps);
        }

        public EditStatus UpdateEmployee(EmployeeModel aEmployee)
        {
            List<EmployeeModel> emps = ReadEmployeeData();
            EmployeeModel emp = emps.FirstOrDefault(e => e.Id == aEmployee.Id);
            if (emp != null)
            {
                emp.Age = aEmployee.Age;
                emp.FirstName = aEmployee.FirstName;
                emp.LastName = aEmployee.LastName;

                return WriteEmployeeData(emps);
            }
            else
                return EditStatus.EntityNotFound;      
        }

        public EditStatus DeleteEmployee(int aEmployeeId)
        {
            List<EmployeeModel> emps = ReadEmployeeData();
            List<AddressModel> adrs = ReadAddressData();
            EmployeeModel emp = emps.FirstOrDefault(e => e.Id == aEmployeeId);
            if (emp != null)
            {
                // Mitarbeiter löschen
                emps.Remove(emp);
                // Alle Addressen dazu löschen
                adrs.RemoveAll(a => a.EmployeeId == aEmployeeId);
                WriteAddressData(adrs);
                return WriteEmployeeData(emps);
            }
            else
                return EditStatus.EntityNotFound;
        }
        #endregion

        #region -------------------- Public Datenzugriff Addresses --------------------
        public List<AddressModel> GetEmployeeAddresses(int aEmployeId)
        {
            List<AddressModel> adrs = ReadAddressData();
            return adrs.Where(a => a.EmployeeId == aEmployeId).ToList();
        }

        public AddressModel GetSingleAddress(int aAddressId)
        {
            List<AddressModel> adrs = ReadAddressData();
            return adrs.FirstOrDefault(a => a.Id == aAddressId);
        }

        public EditStatus InsertAddress(AddressModel aAddress)
        {
            List<AddressModel> adrs = ReadAddressData();
            int next_id = adrs.Max(a => a.Id) + 1;

            aAddress.Id = next_id;
            adrs.Add(aAddress);

            return WriteAddressData(adrs);
        }

        public EditStatus UpdateAddress(AddressModel aAddress)
        {
            List<AddressModel> adrs = ReadAddressData();
            AddressModel adr = adrs.FirstOrDefault(a => a.Id == aAddress.Id);
            if (adr != null)
            {
                adr.Street = aAddress.Street;
                adr.Zip = aAddress.Zip;
                adr.City = aAddress.City;
                adr.State = aAddress.State;

                return WriteAddressData(adrs);
            }
            else
                return EditStatus.EntityNotFound;
        }

        public EditStatus DeleteAddress(int aAddressId)
        {
            List<AddressModel> adrs = ReadAddressData();
            AddressModel adr = adrs.FirstOrDefault(a => a.Id == aAddressId);
            if (adr != null)
            {
                adrs.Remove(adr);
                return WriteAddressData(adrs);
            }
            else
                return EditStatus.EntityNotFound;
        }
        #endregion

        #region -------------------- Filezugriff --------------------
        private List<EmployeeModel> GenerateDefaultEmployees()
        {
            return new List<EmployeeModel>() {
                new EmployeeModel() { Id = 1, FirstName = "Thomas", LastName = "Moser", Age = 28, },
                new EmployeeModel() { Id = 2, FirstName = "Max", LastName = "Musterman", Age = 35, },
                new EmployeeModel() { Id = 3, FirstName = "Peter", LastName = "Thaler", Age = 17, }};
        }

        private List<AddressModel> GenerateDefaultAddresses()
        {
            return new List<AddressModel>() {
                new AddressModel() { Id = 1, Street = "Schillerstrasse", City = "Weiner Neudorf", Zip = 2751, State = "Niederösterreich", EmployeeId = 1 },
                new AddressModel() { Id = 2, Street = "Teststrasse 1", City = "Eggendorf", Zip = 2492, State = "Niederösterreich", EmployeeId = 2 },
                new AddressModel() { Id = 3, Street = "Hauptplatz 5", City = "Wiener Neustadt", Zip = 2700, State = "Niederösterreich", EmployeeId = 2 },
                new AddressModel() { Id = 4, Street = "Kurgasse 89", City = "Baden", Zip = 2500, State = "Niederösterreich", EmployeeId = 3 } };
        }

        private List<EmployeeModel> ReadEmployeeData()
        {
            List<EmployeeModel> ret = new List<EmployeeModel>();

            try
            {
                if (Directory.Exists(DataPath))
                {
                    using (FileStream fs = new FileStream(Path.Combine(DataPath, EmployeeFilename), FileMode.OpenOrCreate, FileAccess.Read))
                    {
                        using (StreamReader sr = new StreamReader(fs))
                        {
                            string line;
                            while ((line = sr.ReadLine()) != null)
                            {
                                string[] split_line = line.Split(';');
                                if (split_line.Length == 4)
                                    ret.Add(new EmployeeModel(split_line));
                            }
                        } 
                    }
                }
            }
            catch (Exception E)
            {
                // Fehlermeldung Ausgabe in Logfile
            }

            return ret;
        }

        private List<AddressModel> ReadAddressData()
        {
            List<AddressModel> ret = new List<AddressModel>();

            try
            {
                if (Directory.Exists(DataPath))
                {
                    using (FileStream fs = new FileStream(Path.Combine(DataPath, AddressFilename), FileMode.OpenOrCreate, FileAccess.Read))
                    {
                        using (StreamReader sr = new StreamReader(fs))
                        {
                            string line;
                            while ((line = sr.ReadLine()) != null)
                            {
                                string[] split_line = line.Split(';');
                                if (split_line.Length == 6)
                                    ret.Add(new AddressModel(split_line));
                            }
                        }
                    }
                }
            }
            catch (Exception E)
            {
                // Fehlermeldung Ausgabe in Logfile
            }

            return ret;
        }

        private EditStatus WriteEmployeeData(List<EmployeeModel> aData)
        {
            EditStatus ret = EditStatus.Error;
            try
            {
                if (Directory.Exists(DataPath))
                {
                    using (FileStream fs = new FileStream(Path.Combine(DataPath, EmployeeFilename), FileMode.Create, FileAccess.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(fs))
                        {
                            foreach (EmployeeModel emp in aData)
                            {
                                sw.WriteLine(emp.ToString());
                            }
                        }
                    }
                    ret = EditStatus.OK;
                }
                else
                    ret = EditStatus.PathNotFound;
            }
            catch (Exception E)
            {
                ret = EditStatus.Error;
                // Fehlermeldung Ausgabe in Logfile
            }
            return ret;
        }

        private EditStatus WriteAddressData(List<AddressModel> aData)
        {
            EditStatus ret = EditStatus.Error;
            try
            {
                if (Directory.Exists(DataPath))
                {
                    using (FileStream fs = new FileStream(Path.Combine(DataPath, AddressFilename), FileMode.Create, FileAccess.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(fs))
                        {
                            foreach (AddressModel adr in aData)
                            {
                                sw.WriteLine(adr.ToString());
                            }
                        }
                    }
                    ret = EditStatus.OK;
                }
                else
                    ret = EditStatus.PathNotFound;
            }
            catch (Exception E)
            {
                ret = EditStatus.Error;
                // Fehlermeldung Ausgabe in Logfile
            }
            return ret;
        }
        #endregion
    }
}