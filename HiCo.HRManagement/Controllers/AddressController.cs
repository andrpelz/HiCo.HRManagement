using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using HiCo.HRManagement.Models;

namespace HiCo.HRManagement.Controllers
{
    public class AddressController : ApiController
    {
        private DataProvider DataProvider = new DataProvider(AppDomain.CurrentDomain.GetData("DataDirectory").ToString());

        // api/Address/GetEmployeeAddressList
        [HttpPost]
        public List<AddressModel> GetEmployeeAddressList([FromBody] int aEmployeeId)
        {
            return DataProvider.GetEmployeeAddresses(aEmployeeId);
        }

        // api/Address/GetAddress
        [HttpPost]
        public AddressModel GetAddress([FromBody] int aAddressId)
        {
            AddressModel ret = new AddressModel();
            if (aAddressId == 0) // Neu, Defaultwerte setzen
            {
                ret.Id = 0;
                ret.EmployeeId = 0;
                ret.Street = "";
                ret.Zip = 0;
                ret.City = "";
                ret.State = "";
            }
            else
            {
                ret = DataProvider.GetSingleAddress(aAddressId);
            }

            return ret;
        }

        // api/Address/SaveAddress
        [HttpPost]
        public int SaveAddress([FromBody] AddressModel aAddress)
        {
            if (aAddress.Id == 0)
                return (int)DataProvider.InsertAddress(aAddress);
            else
                return (int)DataProvider.UpdateAddress(aAddress);
        }

        // api/Address/DeleteAddress
        [HttpPost]
        public int DeleteAddress([FromBody] int aAddressId)
        {
            return (int)DataProvider.DeleteAddress(aAddressId);
        }
    }
}