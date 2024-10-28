using SincronizadorGPS50.GestprojectDataManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SincronizadorGPS50
{
   public interface IGestprojectConnectionManager
   {
      System.Data.SqlClient.SqlConnection GestprojectSqlConnection { get; set; }
      SynchronizerUserRememberableDataModel GestprojectUserRememberableData { get; set; }
   }
}
