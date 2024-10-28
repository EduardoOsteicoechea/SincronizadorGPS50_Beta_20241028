using System.Data.SqlClient;
using System.Reflection;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   internal class UpdateGestprojectTaxDataWithSageTaxData
   {
      public UpdateGestprojectTaxDataWithSageTaxData
      (
         SqlConnection connection,
         string tableName,
         string gestprojectTaxesTableColumnsAndValues, 
         GestprojectTaxModel entity 
      )
      {
         try
         {
            connection.Open();

            string sqlString = $@"
            UPDATE 
               {tableName} 
            SET
               {gestprojectTaxesTableColumnsAndValues}
            WHERE
               IMP_ID={entity.IMP_ID}
            ;";

            //MessageBox.Show("At: UpdateGestprojectTaxDataWithSageTaxData\n\n" + sqlString);

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