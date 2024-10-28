using Sage.ES.S50.Addons;
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
   internal class WasSubaccountableAccountRegistered
   {
      public bool ItWas { get; set; } = false;
      public WasSubaccountableAccountRegistered
      (
         SqlConnection connection, 
         ISynchronizationTableSchemaProvider tableSchema, 
         GestprojectSubaccountableAccountModel entity
      )
      {
         try
         {
            connection.Open();
                 
            string sqlString = $@"
               SELECT 
                  ID
               FROM 
                  {tableSchema.TableName}
               WHERE 
                  COS_NOMBRE=@COS_NOMBRE
               AND 
                  COS_CODIGO=@COS_CODIGO
               AND 
                  COS_GRUPO=@COS_GRUPO
            ";

            using(SqlCommand command = new SqlCommand(sqlString, connection))
            {
               command.Parameters.AddWithValue("@COS_NOMBRE", entity.COS_NOMBRE);
               command.Parameters.AddWithValue("@COS_CODIGO", entity.COS_CODIGO);
               command.Parameters.AddWithValue("@COS_GRUPO", entity.COS_GRUPO);
               
               using(SqlDataReader reader = command.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     ItWas = true;
                     break;
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
         }
      }
   }
}
