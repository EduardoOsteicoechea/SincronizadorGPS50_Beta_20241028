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
   internal class UpdateTax
   {
      public UpdateTax
      (
         System.Data.SqlClient.SqlConnection connection,
         string gestprojectTaxesTableColumnsAndValues, 
         GestprojectTaxModel entity,
         ISynchronizationTableSchemaProvider tableSchema,
         CompanyGroup Sage50ConnectionManager
      )
      {
         try
         {
            connection.Open();

            // Update Tax data in Gestproject database
            string sqlString = $@"
            UPDATE {"IMPUESTO_CONFIG"} SET
               {gestprojectTaxesTableColumnsAndValues}
            WHERE
               IMP_ID={entity.IMP_ID}
            ;";

            MessageBox.Show(sqlString);

            using(SqlCommand sqlCommand = new SqlCommand(sqlString, connection))
            {
               sqlCommand.ExecuteNonQuery();
            };
         
            // Update Tax data in Synchronization table
            string updateEntitySyncrhonizationStatusSql = $@"
               UPDATE 
                  {tableSchema.TableName} 
               SET 
                  {tableSchema.SynchronizationStatus.ColumnDatabaseName}='{SynchronizationStatusOptions.Sincronizado}',
                  {tableSchema.SynchronizationStatus.ColumnDatabaseName}='{SynchronizationStatusOptions.Sincronizado}',
                  {tableSchema.SynchronizationStatus.ColumnDatabaseName}='{SynchronizationStatusOptions.Sincronizado}',
                  {tableSchema.SynchronizationStatus.ColumnDatabaseName}='{SynchronizationStatusOptions.Sincronizado}',
                  {tableSchema.SynchronizationStatus.ColumnDatabaseName}='{SynchronizationStatusOptions.Sincronizado}',
                  {tableSchema.SynchronizationStatus.ColumnDatabaseName}='{SynchronizationStatusOptions.Sincronizado}',
               WHERE
                  S50_GUID_ID='{entity.S50_GUID_ID}'                                    
            ";

            MessageBox.Show(updateEntitySyncrhonizationStatusSql);

            using(SqlCommand sqlCommand = new SqlCommand(updateEntitySyncrhonizationStatusSql, connection))
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
