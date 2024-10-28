//using SincronizadorGPS50.GestprojectDataManager;
//using SincronizadorGPS50.Sage50Connector;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;

//namespace SincronizadorGPS50
//{
//   internal class GestprojectProvidersProvider : IGestprojectEntitiesProvider<GestprojectProviderModel>
//   {
//      public List<GestprojectProviderModel> GetProviders(System.Data.SqlClient.SqlConnection connection) 
//      {
//         try
//         {
//            return new GestprojectProvidersManager().GetProviders(connection);
//         }
//         catch(System.Exception exception)
//         {
//            throw ApplicationLogger.ReportError(
//               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
//               MethodBase.GetCurrentMethod().DeclaringType.Name,
//               MethodBase.GetCurrentMethod().Name,
//               exception
//            );
//         };
//      }
//	}
//}
