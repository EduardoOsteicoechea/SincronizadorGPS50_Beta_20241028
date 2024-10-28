using GestprojectDataManager;
using Infragistics.Designers.SqlEditor;
using Infragistics.Documents.Excel.ConditionalFormatting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   public class GestprojectProjectsManager
   {
      public List<GestprojectProjectModel> GestprojectEntityList { get; set; } = new List<GestprojectProjectModel>();
      public List<GestprojectProjectModel> GetEntities
      (
         System.Data.SqlClient.SqlConnection connection, 
         string tableName,
         List<(string columnName, Type columnType)> columnsAndTypesToQuery
      )
      {
         try
         {
            connection.Open();

            StringBuilder columnsAndValuesStringBuilder = new StringBuilder();
            for(global::System.Int32 i = 0; i < columnsAndTypesToQuery.Count; i++)
            {
               columnsAndValuesStringBuilder.Append($"{columnsAndTypesToQuery[i].columnName},");
            };

            string sqlString = $@"
            SELECT 
               {columnsAndValuesStringBuilder.ToString().TrimEnd(',')}
            FROM 
            {tableName} 
            ;";

            using(SqlCommand sqlCommand = new SqlCommand(sqlString, connection))
            {
               using(SqlDataReader reader = sqlCommand.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     GestprojectProjectModel entity = new GestprojectProjectModel();

                     PropertyInfo[] properties = entity.GetType().GetProperties();

                     for(global::System.Int32 i = 0; i < columnsAndTypesToQuery.Count; i++)
                     {
                        if(columnsAndTypesToQuery[i].columnType == typeof(int))
                        {
                           var scrutinizedValue = TypeProtector<int>.Scrutinize(reader, i, 0);
                           typeof(GestprojectProjectModel).GetProperty(columnsAndTypesToQuery[i].columnName).SetValue(entity, scrutinizedValue);
                        }
                        else if(columnsAndTypesToQuery[i].columnType == typeof(string))
                        {
                           var scrutinizedValue = TypeProtector<string>.Scrutinize(reader, i, string.Empty);
                           typeof(GestprojectProjectModel).GetProperty(columnsAndTypesToQuery[i].columnName).SetValue(entity, scrutinizedValue);
                        }
                        else if(columnsAndTypesToQuery[i].columnType == typeof(DateTime))
                        {
                           var scrutinizedValue = TypeProtector<DateTime>.Scrutinize(reader, i, DateTime.Now);
                           typeof(GestprojectProjectModel).GetProperty(columnsAndTypesToQuery[i].columnName).SetValue(entity, scrutinizedValue);
                        }
                        else
                        {
                           throw new Exception($"Unallowed type \"{reader.GetValue(i).GetType().Name}\" on \"{typeof(GestprojectProjectModel).Name}\", please check the data schema you're using.");
                        };
                     };

                     GestprojectEntityList.Add(entity);
                  };
               };
            };

            return GestprojectEntityList;
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
