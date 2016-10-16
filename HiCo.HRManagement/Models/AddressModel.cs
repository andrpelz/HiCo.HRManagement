using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HiCo.HRManagement.Models
{
    public class AddressModel
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string Street { get; set; }
        public int Zip { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        public AddressModel() { }

        public AddressModel(string[] aLine)
        {
            Id = Convert.ToInt32(aLine[0]);
            EmployeeId = Convert.ToInt32(aLine[1]);
            Street = aLine[2];
            Zip = Convert.ToInt32(aLine[3]);
            City = aLine[4];
            State = aLine[5];
        }

        public override string ToString()
        {
            return
                Id + ";" +
                EmployeeId + ";" +
                (Street == null ? "" : Street) + ";" +
                Zip + ";" +
                (City == null ? "" : City) + ";" +
                (State == null ? "" : State);
        }
    }
}