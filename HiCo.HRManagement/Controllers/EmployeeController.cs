using HiCo.HRManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HiCo.HRManagement.Controllers
{
    public class EmployeeController : ApiController
    {
        private DataProvider DataProvider = new DataProvider(AppDomain.CurrentDomain.GetData("DataDirectory").ToString());

        // api/Employee/GetEmployeeList
        [HttpPost]
        public GetEmployeeListResponse GetEmployeeList(GetEmployeeListRequest aRequest)
        {
            GetEmployeeListResponse ret = new GetEmployeeListResponse();
            ret.Employees = new List<EmployeeModel>();

            List<EmployeeModel> list = DataProvider.GetEmployees();

            // Ermitteln welche Elemente zurück gegeben werden
            int start_index = (aRequest.PageNumber - 1) * aRequest.EntriesPerPage;
            int end_index = start_index + aRequest.EntriesPerPage;
            if (end_index > list.Count) end_index = list.Count;

            for (int i = start_index; i < end_index; i++)
            {
                ret.Employees.Add(list[i]);
            } 
            ret.TotalItems = list.Count;

            return ret;
        }

        // api/Employee/GetEmployee
        [HttpPost]
        public EmployeeModel GetEmployee([FromBody] int aEmployeeId)
        {
            EmployeeModel ret = new EmployeeModel();
            if (aEmployeeId == 0) // Neu, Defaultwerte setzen
            {
                ret.Id = 0;
                ret.Age = 0;
                ret.FirstName = "";
                ret.LastName = "";
                ret.Addresses = new List<AddressModel>();
            }
            else
            {
                ret = DataProvider.GetSingleEmployee(aEmployeeId);
            }

            return ret;
        }

        // api/Employee/SaveEmployee
        [HttpPost]
        public int SaveEmployee([FromBody] EmployeeModel aEmployee)
        {
            if (aEmployee.Id == 0)
                return (int)DataProvider.InsertEmployee(aEmployee);
            else
                return (int)DataProvider.UpdateEmployee(aEmployee);
        }

        // api/Employee/DeleteEmployee
        [HttpPost]
        public int DeleteEmployee([FromBody] int aEmployeeId)
        {
            return (int)DataProvider.DeleteEmployee(aEmployeeId);
        }
    }

    public class GetEmployeeListRequest
    {
        public int PageNumber { get; set; }
        public int EntriesPerPage { get; set; }
    }

    public class GetEmployeeListResponse
    {
        public int TotalItems { get; set; }
        public List<EmployeeModel> Employees { get; set; }
    }
}
