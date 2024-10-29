using SincronizadorGPS50.Sage50Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SincronizadorGPS50
{
   internal class Sage50ConnectionManager : ISage50ConnectionManager
   {
      public CompanyGroup CompanyGroupData { get; set; }
      public Sage50ConnectionManager(string selectedCompanyGroup)
      {
         CompanyGroupData = SincronizadorGPS50.Sage50Connector
         .Sage50CompanyGroupActions
         .GetCompanyGroups()
         .FirstOrDefault(companyGroup => companyGroup.CompanyName == selectedCompanyGroup);
      }
   }
}
