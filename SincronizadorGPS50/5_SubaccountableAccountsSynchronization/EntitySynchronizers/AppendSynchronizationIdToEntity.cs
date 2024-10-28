using Infragistics.Designers.SqlEditor;
using SincronizadorGPS50.GestprojectDataManager;
using SincronizadorGPS50.Sage50Connector;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   internal class AppendSynchronizationIdToEntity
   {
      public AppendSynchronizationIdToEntity
      (
         System.Data.SqlClient.SqlConnection connection,
         ISynchronizationTableSchemaProvider tableSchema,
         GestprojectSubaccountableAccountModel entity
      )
      {
         try
         {
            connection.Open();

            string sqlString = $@"
            SELECT 
               ID
            FROM
               {tableSchema.TableName}
            WHERE
               COS_GRUPO=@COS_GRUPO
            ;";

            using(SqlCommand command = new SqlCommand(sqlString, connection))
            {
               command.Parameters.AddWithValue("@COS_GRUPO", entity.COS_GRUPO);

               using(SqlDataReader reader = command.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     entity.ID = Convert.ToInt32(reader.GetValue(0));
                  };
               };
            };
         }
         catch(System.Exception exception)
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         }
         finally
         {
            connection.Close();
         };
      }
   }
}
