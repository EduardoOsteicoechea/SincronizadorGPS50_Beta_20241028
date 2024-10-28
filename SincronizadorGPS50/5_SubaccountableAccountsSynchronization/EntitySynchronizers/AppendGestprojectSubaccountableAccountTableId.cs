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
   internal class AppendGestprojectSubaccountableAccountTableId
   {
      public AppendGestprojectSubaccountableAccountTableId
      (
         System.Data.SqlClient.SqlConnection connection,
         ISynchronizationTableSchemaProvider tableSchema,
         string tableName,
         GestprojectSubaccountableAccountModel entity
      )
      {
         try
         {
            connection.Open();

            string sqlString = $@"
            SELECT 
               COS_ID
            FROM
               {tableName}
            WHERE
               COS_GRUPO='{entity.COS_GRUPO}'
            ;";

            //MessageBox.Show("At: AppendGestprojectSubaccountableAccountTableIdToSynchronizationTable" + sqlString);

            using(SqlCommand sqlCommand = new SqlCommand(sqlString, connection))
            {
               using(SqlDataReader reader = sqlCommand.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     entity.COS_ID = Convert.ToInt32(reader.GetValue(0).GetType().Name == "DBNull" ? -1 : reader.GetValue(0));
                  };
               };
            };

            //string sqlString2 = $@"
            //UPDATE 
            //   {tableSchema.TableName} 
            //SET
            //   {tableSchema.GestprojectId.ColumnDatabaseName}={entity.COS_ID}
            //WHERE
            //   {tableSchema.Sage50GuidId.ColumnDatabaseName}='{entity.S50_GUID_ID}'
            //;";

            string sqlString2 = $@"
            UPDATE 
               {tableSchema.TableName} 
            SET
               {tableSchema.GestprojectId.ColumnDatabaseName}={entity.COS_ID}
            WHERE
               COS_ID='{entity.COS_ID}'
            ;";

            //MessageBox.Show("At: AppendGestprojectSubaccountableAccountTableIdToSynchronizationTable" + sqlString2);

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
