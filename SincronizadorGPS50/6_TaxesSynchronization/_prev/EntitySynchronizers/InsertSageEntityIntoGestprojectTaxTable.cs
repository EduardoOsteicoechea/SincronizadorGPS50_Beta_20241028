using System;
using System.Data.SqlClient;
using System.Reflection;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   internal class InsertSageEntityIntoGestprojectTaxTable
   {
      public InsertSageEntityIntoGestprojectTaxTable
      (
         SqlConnection connection,
         string tableName,
         string gestprojectTaxesTableColumns, 
         string gestprojectTaxesTableValues, 
         GestprojectTaxModel entity 
      )
      {
         try
         {
            connection.Open();
            
            ////////////////////////////////////////
            /// The IMPUESTO_CONFIG table doesn't have
            /// an autoincremental index, therefore, we need
            /// to get the highest and add one to asign the
            /// inserted entity Id
            ////////////////////////////////////////

            string sqlString = $@"
            SELECT 
               MAX(IMP_ID)
            FROM
               {tableName}
            ;";

            //MessageBox.Show("At: InsertSageEntityIntoGestprojectTaxTable\n\n" + sqlString);

            using(SqlCommand sqlCommand = new SqlCommand(sqlString, connection))
            {
               using(SqlDataReader reader = sqlCommand.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     int maxIdValue = Convert.ToInt32(reader.GetValue(0).GetType().Name == "DBNull" ? 0 : reader.GetValue(0));
                     entity.IMP_ID = ++maxIdValue;
                  };
               };
            };

            string sqlString2 = $@"
            INSERT INTO 
               {tableName} 
               (IMP_ID,{gestprojectTaxesTableColumns})
            VALUES
               ({entity.IMP_ID},{gestprojectTaxesTableValues})
            ;";

            //MessageBox.Show("At: InsertSageEntityIntoGestprojectTaxTable\n\n" + sqlString2);

            using(SqlCommand sqlCommand = new SqlCommand(sqlString2, connection))
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
