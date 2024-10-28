using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SincronizadorGPS50
{
   internal interface ISynchronizationDatabaseTableManager
   {
      bool TableExists(
         System.Data.SqlClient.SqlConnection connection,
         string tableName
      );
      void CreateTable(
         System.Data.SqlClient.SqlConnection connection,
         ISynchronizationTableSchemaProvider synchronizationTableSchemaProvider
      );
   }
}
