using Infragistics.Designers.SqlEditor;
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
   internal class AppendSubaccountableAccountSynchronizationData

   {
      public AppendSubaccountableAccountSynchronizationData
      (
         System.Data.SqlClient.SqlConnection connection,
         ISynchronizationTableSchemaProvider tableSchema,
         GestprojectSubaccountableAccountModel entity,
         string ColumnsAndValues
      )
      {
         try
         {
            connection.Open();

            //string sqlString = $@"
            //UPDATE 
            //   {tableSchema.TableName} 
            //SET
            //   {ColumnsAndValues}
            //WHERE
            //   S50_GUID_ID='{entity.S50_GUID_ID}'
            //;";

            string sqlString = $@"
            UPDATE 
               {tableSchema.TableName} 
            SET
               {ColumnsAndValues}
            WHERE
               COS_GRUPO='{entity.COS_GRUPO}'
            ;";

            //MessageBox.Show("At: AppendSubaccountableAccountSynchronizationData\n\n" + sqlString);

            using(SqlCommand sqlCommand = new SqlCommand(sqlString, connection))
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
