using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   internal class GestprojectEntities<T> where T : new()
   {
      public List<T> EntityList { get; set; } = new List<T>();
      public List<T> GetAll
      (
         System.Data.SqlClient.SqlConnection connection,
         List<int> selectedIdList,
         string tableName,
         List<(string columnName, System.Type columnType)> columnsAndTypesToQuery,
         (string columnName, string value) condition1Data,
         (string columnName, string value) condition2Data = default
      ) 
      {
         try
         {
            connection.Open();

            string fieldNamesForSqlStatement = string.Empty;
            for (global::System.Int32 i = 0; i < columnsAndTypesToQuery.Count; i++)
            {
               fieldNamesForSqlStatement += $"{columnsAndTypesToQuery[i].columnName},";
            };
            fieldNamesForSqlStatement = fieldNamesForSqlStatement.TrimEnd(',');

            string sqlString = $@"
            SELECT 
               {fieldNamesForSqlStatement}
            FROM 
               {tableName} 
            WHERE 
               {condition1Data.columnName} IN ({condition1Data.value})
            ;";

			   //if(tableName == "INT_SAGE_SYNCHRONIZATION_ENTITY_DATA_PROVIDERS")
				  // MessageBox.Show(sqlString);

            using(SqlCommand sqlCommand = new SqlCommand(sqlString, connection))
            {
               using(SqlDataReader reader = sqlCommand.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     T entity = new T();
                     PropertyInfo[] properties = entity.GetType().GetProperties();

                     for (global::System.Int32 i = 0; i < columnsAndTypesToQuery.Count; i++)
                     {					 
						      try
						      {
							      //if(columnsAndTypesToQuery[i].columnName == "NOMBRE_COMPLETO")
							      //	MessageBox.Show(reader.GetValue(i)+ "");

							      TypeRevisor<T>.Check(
								      columnsAndTypesToQuery[i].columnType,
								      columnsAndTypesToQuery[i].columnName,
								      entity,
								      reader,
								      i,
								      properties
							      );                                                
						      }
						      catch(System.Exception exception)
						      {
							      MessageBox.Show($@"Error when retrieving
								      {columnsAndTypesToQuery[i].columnName}
							      ");

							      throw ApplicationLogger.ReportError(
								      MethodBase.GetCurrentMethod().DeclaringType.Namespace,
								      MethodBase.GetCurrentMethod().DeclaringType.Name,
								      MethodBase.GetCurrentMethod().Name,
								      exception
							      );
						      };

                        //if(columnsAndTypesToQuery[i].columnType == typeof(int))
                        //{
                        //   var scrutinizedValue = TypeProtector<int>.Scrutinize(reader, i, 0);
                        //   typeof(T).GetProperty(columnsAndTypesToQuery[i].columName).SetValue(entity, scrutinizedValue);
                        //}
                        //else if(columnsAndTypesToQuery[i].columnType == typeof(string))
                        //{
                        //   var scrutinizedValue = TypeProtector<string>.Scrutinize(reader, i, string.Empty);
                        //   typeof(T).GetProperty(columnsAndTypesToQuery[i].columName).SetValue(entity, scrutinizedValue);
                        //}
                        //else if(columnsAndTypesToQuery[i].columnType == typeof(DateTime))
                        //{
                        //   var scrutinizedValue = TypeProtector<DateTime>.Scrutinize(reader, i, DateTime.Now);
                        //   typeof(T).GetProperty(columnsAndTypesToQuery[i].columName).SetValue(entity,scrutinizedValue);
                        //}
                        //else 
                        //{
                        //   throw new Exception($"Unallowed type \"{reader.GetValue(i).GetType().Name}\" on \"{typeof(T).Name}\", please check the data schema you're using.");
                        //};
                     };

						   //StringBuilder stringBuilder = new StringBuilder();
         //            foreach (var propertyInfo in entity.GetType().GetProperties())
         //            {
							  // stringBuilder.AppendLine($"{propertyInfo.Name}: {propertyInfo.GetValue(entity)}");
         //            };
						   //MessageBox.Show(stringBuilder.ToString());

                     EntityList.Add(entity);
                  };
               };
            };

            return EntityList;
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
      public List<T> GetAll
      (
         System.Data.SqlClient.SqlConnection connection,
         string tableName,
         List<(string columnName, System.Type columnType)> columnsAndTypesToQuery
      ) 
      {
         try
         {
            connection.Open();

            string fieldNamesForSqlStatement = string.Empty;
            for (global::System.Int32 i = 0; i < columnsAndTypesToQuery.Count; i++)
            {
               fieldNamesForSqlStatement += $"{columnsAndTypesToQuery[i].columnName},";
            };
            fieldNamesForSqlStatement = fieldNamesForSqlStatement.TrimEnd(',');

            string sqlString = $@"
            SELECT 
               {fieldNamesForSqlStatement}
            FROM 
               {tableName}
            ;";

			   //if(tableName == "INT_SAGE_SYNCHRONIZATION_ENTITY_DATA_PROVIDERS")
				  // MessageBox.Show(sqlString);

            using(SqlCommand sqlCommand = new SqlCommand(sqlString, connection))
            {
               using(SqlDataReader reader = sqlCommand.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     T entity = new T();
                     PropertyInfo[] properties = entity.GetType().GetProperties();

                     for (global::System.Int32 i = 0; i < columnsAndTypesToQuery.Count; i++)
                     {					 
						      try
						      {
							      //if(columnsAndTypesToQuery[i].columnName == "NOMBRE_COMPLETO")
							      //	MessageBox.Show(reader.GetValue(i)+ "");

							      TypeRevisor<T>.Check(
								      columnsAndTypesToQuery[i].columnType,
								      columnsAndTypesToQuery[i].columnName,
								      entity,
								      reader,
								      i,
								      properties
							      );                                                
						      }
						      catch(System.Exception exception)
						      {
							      MessageBox.Show($@"Error when retrieving
								      {columnsAndTypesToQuery[i].columnName}
							      ");

							      throw ApplicationLogger.ReportError(
								      MethodBase.GetCurrentMethod().DeclaringType.Namespace,
								      MethodBase.GetCurrentMethod().DeclaringType.Name,
								      MethodBase.GetCurrentMethod().Name,
								      exception
							      );
						      };

                        //if(columnsAndTypesToQuery[i].columnType == typeof(int))
                        //{
                        //   var scrutinizedValue = TypeProtector<int>.Scrutinize(reader, i, 0);
                        //   typeof(T).GetProperty(columnsAndTypesToQuery[i].columName).SetValue(entity, scrutinizedValue);
                        //}
                        //else if(columnsAndTypesToQuery[i].columnType == typeof(string))
                        //{
                        //   var scrutinizedValue = TypeProtector<string>.Scrutinize(reader, i, string.Empty);
                        //   typeof(T).GetProperty(columnsAndTypesToQuery[i].columName).SetValue(entity, scrutinizedValue);
                        //}
                        //else if(columnsAndTypesToQuery[i].columnType == typeof(DateTime))
                        //{
                        //   var scrutinizedValue = TypeProtector<DateTime>.Scrutinize(reader, i, DateTime.Now);
                        //   typeof(T).GetProperty(columnsAndTypesToQuery[i].columName).SetValue(entity,scrutinizedValue);
                        //}
                        //else 
                        //{
                        //   throw new Exception($"Unallowed type \"{reader.GetValue(i).GetType().Name}\" on \"{typeof(T).Name}\", please check the data schema you're using.");
                        //};
                     };

						   //StringBuilder stringBuilder = new StringBuilder();
         //            foreach (var propertyInfo in entity.GetType().GetProperties())
         //            {
							  // stringBuilder.AppendLine($"{propertyInfo.Name}: {propertyInfo.GetValue(entity)}");
         //            };
						   //MessageBox.Show(stringBuilder.ToString());

                     EntityList.Add(entity);
                  };
               };
            };

            return EntityList;
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
