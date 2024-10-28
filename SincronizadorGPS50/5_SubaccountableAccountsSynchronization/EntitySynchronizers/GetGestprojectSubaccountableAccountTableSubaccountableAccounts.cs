using Infragistics.Designers.SqlEditor;
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
   internal class GetGestprojectSubaccountableAccountTableSubaccountableAccounts
   {
      public List<GestprojectSubaccountableAccountModel> Entities {get;set;} = new List<GestprojectSubaccountableAccountModel>();
      public GetGestprojectSubaccountableAccountTableSubaccountableAccounts
      (
         System.Data.SqlClient.SqlConnection connection,
         string tableName
      )
      {
         try
         {
            connection.Open();

            string sqlString = $@"
            SELECT 
               COS_ID,
               COS_NOMBRE,
               COS_CODIGO,
               COS_GRUPO
            FROM
               {tableName}
            ;";

            //MessageBox.Show(sqlString);

            using(SqlCommand sqlCommand = new SqlCommand(sqlString, connection))
            {
               using(SqlDataReader reader = sqlCommand.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     GestprojectSubaccountableAccountModel entity = new GestprojectSubaccountableAccountModel();
                     entity.COS_ID = Convert.ToInt32(reader.GetValue(0));
                     entity.COS_NOMBRE = Convert.ToString(reader.GetValue(1));
                     entity.COS_CODIGO = Convert.ToString(reader.GetValue(2));
                     entity.COS_GRUPO = Convert.ToString(reader.GetValue(3));
                     Entities.Add(entity);
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
