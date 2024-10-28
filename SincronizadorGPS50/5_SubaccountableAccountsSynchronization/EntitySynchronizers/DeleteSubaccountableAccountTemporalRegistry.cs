using Sage.ES.S50.Addons;
using System;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Reflection;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   internal class DeleteSubaccountableAccountTemporalRegistry
   {
      public DeleteSubaccountableAccountTemporalRegistry
      (
         SqlConnection connection,
         string tableName,
         GestprojectSubaccountableAccountModel entity
      )
      {

         try
         {
            connection.Open();

            string sqlString2 = $@"
            DELETE FROM 
               {tableName} 
            WHERE
               COS_GRUPO=@COS_GRUPO
            AND
               COS_ID=@COS_ID
            ;";

            //MessageBox.Show("At: DeleteSubaccountableAccountTemporalRegistry\n\n" + sqlString2);

            using(SqlCommand command = new SqlCommand(sqlString2, connection))
            {
               command.Parameters.AddWithValue("@COS_GRUPO", entity.COS_GRUPO);
               command.Parameters.AddWithValue("@COS_ID", -1);

               command.ExecuteNonQuery();
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
