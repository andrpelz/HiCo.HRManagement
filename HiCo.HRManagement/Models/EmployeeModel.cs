using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HiCo.HRManagement.Models
{
    public class EmployeeModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public List<AddressModel> Addresses { get; set; }

        public EmployeeModel() { }

        public EmployeeModel(string[] aLine)
        {
            Id = Convert.ToInt32(aLine[0]);
            FirstName = aLine[1];
            LastName = aLine[2];
            Age = Convert.ToInt32(aLine[3]);
        }

        public override string ToString()
        {
            return
                Id + ";" +
                (FirstName == null ? "" : FirstName) + ";" +
                (LastName == null ? "" : LastName) + ";" +
                Age;         
        }
    }
}