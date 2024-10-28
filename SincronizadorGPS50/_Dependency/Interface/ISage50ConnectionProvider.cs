using SincronizadorGPS50.Sage50Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SincronizadorGPS50
{
   public interface ISage50ConnectionManager
   {
      CompanyGroup CompanyGroupData { get; set; }
   }
}
