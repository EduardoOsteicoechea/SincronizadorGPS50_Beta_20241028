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
   internal class AppendGestprojectTaxTableIdToSynchronizationTable
   {
      public AppendGestprojectTaxTableIdToSynchronizationTable
      (
         System.Data.SqlClient.SqlConnection connection,
         string tableName,
         GestprojectTaxModel entity,
         ISynchronizationTableSchemaProvider tableSchema
      )
      {
         try
         {
            connection.Open();

            string sqlString = $@"
            SELECT 
               IMP_ID
            FROM
               {tableName}
            WHERE
               IMP_SUBCTA_CONTABLE='{entity.IMP_SUBCTA_CONTABLE}'
            ;";

            //MessageBox.Show("At: AppendGestprojectTaxTableIdToSynchronizationTable" + sqlString);

            using(SqlCommand sqlCommand = new SqlCommand(sqlString, connection))
            {
               using(SqlDataReader reader = sqlCommand.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     entity.IMP_ID = Convert.ToInt32(reader.GetValue(0).GetType().Name == "DBNull" ? -1 : reader.GetValue(0));
                  };
               };
            };

            string sqlString2 = $@"
            UPDATE 
               {tableSchema.TableName} 
            SET
               {tableSchema.GestprojectId.ColumnDatabaseName}={entity.IMP_ID}
            WHERE
               {tableSchema.Sage50GuidId.ColumnDatabaseName}='{entity.S50_GUID_ID}'
            ;";

            //MessageBox.Show("At: AppendGestprojectTaxTableIdToSynchronizationTable" + sqlString2);

            using(SqlCommand sqlCommand = new SqlCommand(sqlString2, connection))
            {
               sqlCommand.ExecuteNonQuery();
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
