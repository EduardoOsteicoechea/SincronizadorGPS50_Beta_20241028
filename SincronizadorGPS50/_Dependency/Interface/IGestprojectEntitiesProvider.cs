using System.Collections.Generic;

namespace SincronizadorGPS50
{
   internal interface IGestprojectEntitiesProvider<T1>
   {
      List<T1> Get
      (
         System.Data.SqlClient.SqlConnection connection, 
         GestprojectEntityProviderDelegate<T1> entityProviderDelegate
      );
   }
}
