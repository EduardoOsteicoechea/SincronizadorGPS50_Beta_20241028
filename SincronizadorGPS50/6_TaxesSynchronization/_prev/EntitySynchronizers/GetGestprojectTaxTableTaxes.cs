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
   internal class GetGestprojectTaxTableTaxes
   {
      public List<GestprojectTaxModel> Entities {get;set;} = new List<GestprojectTaxModel>();
      public GetGestprojectTaxTableTaxes
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
               IMP_ID,
               IMP_SUBCTA_CONTABLE
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
                     GestprojectTaxModel entity = new GestprojectTaxModel();
                     entity.IMP_ID = Convert.ToInt32(reader.GetValue(0));
                     entity.IMP_SUBCTA_CONTABLE = Convert.ToString(reader.GetValue(1));
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
