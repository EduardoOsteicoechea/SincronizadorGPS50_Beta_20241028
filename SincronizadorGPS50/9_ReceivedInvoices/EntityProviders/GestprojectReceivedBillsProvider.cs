using SincronizadorGPS50.GestprojectDataManager;
using SincronizadorGPS50.Sage50Connector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SincronizadorGPS50
{
   internal class GestprojectReceivedBillsProvider : IGestprojectEntitiesProvider<GestprojectReceivedBillModel>
   {
      public List<GestprojectReceivedBillModel> Get(System.Data.SqlClient.SqlConnection connection, GestprojectEntityProviderDelegate<GestprojectReceivedBillModel> entityProviderDelegate) 
      {
         try
         {            
            return entityProviderDelegate();
         }
         catch(System.Exception exception)
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         };
      }
   }
}
