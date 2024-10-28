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
   internal class AppendSynchronizationDataToEntityRegistry

   {
      public AppendSynchronizationDataToEntityRegistry
      (
         System.Data.SqlClient.SqlConnection connection,
         ISynchronizationTableSchemaProvider tableSchema,
         GestprojectTaxModel entity,
         string gestprojectTaxesTableColumnsAndValues
      )
      {
         try
         {
            connection.Open();

            string sqlString = $@"
            UPDATE 
               {tableSchema.TableName} 
            SET
               {gestprojectTaxesTableColumnsAndValues}
            WHERE
               S50_GUID_ID='{entity.S50_GUID_ID}'
            ;";

            //MessageBox.Show("At: AppendSynchronizationDataToEntityRegistry\n\n" + sqlString);

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
