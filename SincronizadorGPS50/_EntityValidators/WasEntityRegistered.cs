using System;
using System.Data.SqlClient;
using System.Reflection;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   public class WasEntityRegistered
   {
      public bool ItIs { get; set; } = false;
      public WasEntityRegistered
      (
         SqlConnection connection,
         string tableName,
         string columnName,
         (string columnName, dynamic value) condition
      )
      {
         try
         {
            connection.Open();

            string sqlString = $@"
               SELECT 
                  {columnName}
               FROM 
                  {tableName}
               WHERE 
                  {condition.columnName}={DynamicValuesFormatters.Formatters[condition.value.GetType()](condition.value)}
            ";

            //MessageBox.Show(sqlString);

            using(SqlCommand sqlCommand = new SqlCommand(sqlString, connection))
            {
               using(SqlDataReader reader = sqlCommand.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     if(reader.GetValue(0).GetType().Name != "DBNull" || System.Convert.ToString(reader.GetValue(0)) != "")
                     {
                        ItIs = true;
                        break;
                     }
                  };
               };
            };
         }
         catch(SqlException exception)
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
