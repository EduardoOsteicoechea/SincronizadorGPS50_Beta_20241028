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
   internal class WasReceivedBillRegistered
   {
      public bool ItWas { get; set; } = false;
      public WasReceivedBillRegistered
      (
         SqlConnection connection,
         string tableName,
         string columnName,
         (string columnName, dynamic value) condition1,
         (string columnName, dynamic value) condition2 = default
      )
      {
         try
         {
            connection.Open();
                 
            StringBuilder sqlCondition = new StringBuilder();
            
            if(condition1.value.GetType() == typeof(string))
            {
               if(condition1.value == "")
               {
                  sqlCondition.Append($"{condition2.columnName}={DynamicValuesFormatters.Formatters[condition2.value.GetType()](condition2.value)}");
               }
               else
               {
                  sqlCondition.Append($"{condition1.columnName}={DynamicValuesFormatters.Formatters[condition1.value.GetType()](condition1.value)}");            
               };
            }
            else
            {
               if(condition1.value == null || condition1.value == 0)
               {
                  sqlCondition.Append($"{condition2.columnName}={DynamicValuesFormatters.Formatters[condition2.value.GetType()](condition2.value)}");
               }
               else
               {
                  sqlCondition.Append($"{condition1.columnName}={DynamicValuesFormatters.Formatters[condition1.value.GetType()](condition1.value)}");            
               };
            };

            string sqlString = $@"
               SELECT 
                  {columnName}
               FROM 
                  {tableName}
               WHERE 
                  {sqlCondition.ToString()}
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
                        ItWas = true;
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
