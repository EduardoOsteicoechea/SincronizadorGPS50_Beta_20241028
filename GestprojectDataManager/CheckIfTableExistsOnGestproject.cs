using System.Data.SqlClient;
using System.Windows.Forms;

namespace SincronizadorGPS50.GestprojectDataManager
{
   public class CheckIfTableExistsOnGestproject
   {
      public bool Exists { get; set; } = false;
      public CheckIfTableExistsOnGestproject(System.Data.SqlClient.SqlConnection connection, string tableName)
      {
         try
         {
            connection.Open();

            string sqlString = $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE \"TABLE_NAME\" = '{tableName}'";

            using(SqlCommand sqlCommand = new SqlCommand(sqlString, connection))
            {
               int? Sage50SincronizationTableCount = (int)sqlCommand.ExecuteScalar();
               if(Sage50SincronizationTableCount != null)
               {
                  Exists = Sage50SincronizationTableCount > 0;
               }
               else
               {
                  MessageBox.Show($"La búsqueda de la tabla \"{tableName}\" retornó un valor nulo.");
               };
            };
         }
         catch(SqlException exception)
         {
            throw new System.Exception(
               $"At:\n\nSincronizadorGPS50.GestprojectDataManager\n.CheckIfTableExistsOnGestproject:\n\n{exception.Message}"
            );
         }
         finally
         {
            connection.Close();
         };
      }
   }
}
