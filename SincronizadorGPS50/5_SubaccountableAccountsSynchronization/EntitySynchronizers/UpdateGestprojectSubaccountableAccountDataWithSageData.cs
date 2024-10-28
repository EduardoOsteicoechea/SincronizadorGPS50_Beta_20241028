using System.Data.SqlClient;
using System.Reflection;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   internal class UpdateGestprojectSubaccountableAccountDataWithSageSubaccountableAccountData
   {
      public UpdateGestprojectSubaccountableAccountDataWithSageSubaccountableAccountData
      (
         SqlConnection connection,
         string tableName,
         string gestprojectSubaccountableAccountesTableColumnsAndValues, 
         GestprojectSubaccountableAccountModel entity 
      )
      {
         try
         {
            connection.Open();

            string sqlString = $@"
            UPDATE 
               {tableName} 
            SET
               {gestprojectSubaccountableAccountesTableColumnsAndValues}
            WHERE
               COS_ID={entity.COS_ID}
            ;";

            //MessageBox.Show("At: UpdateGestprojectSubaccountableAccountDataWithSageSubaccountableAccountData\n\n" + sqlString);

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