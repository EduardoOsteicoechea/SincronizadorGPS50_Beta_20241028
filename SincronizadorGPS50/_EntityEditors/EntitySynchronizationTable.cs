using Infragistics.Documents.Excel.ConditionalFormatting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   public class EntitySynchronizationTable<T> where T : new()
   {
      T Entity { get; set; }
      public T AppendTableDataToEntity
      (
         SqlConnection connection,
         string tableName,
         List<(string columnName, System.Type columnType)> fieldsToBeRetrieved,
         (string conditionColumnName, dynamic conditionValue) condition1Data,
         T entity,
         (string conditionColumnName, dynamic conditionValue) condition2Data = default
      )
      {
         try
         {
            connection.Open();

            //if(tableName == "INT_SAGE_SYNCHRONIZATION_ENTITY_DATA_SUBACCOUNTABLE_ACCOUNTS")
            //   new VisualizePropertiesAndValues<T>(
            //       "At: " + MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name,
            //       "SUBACCOUNTABLE_ACCOUNTS entity:",
            //       entity
            //   );

            string fieldNamesForSqlStatement = string.Empty;
            for(global::System.Int32 i = 0; i < fieldsToBeRetrieved.Count; i++)
            {
               fieldNamesForSqlStatement += $"{fieldsToBeRetrieved[i].columnName},";
            };

            fieldNamesForSqlStatement = fieldNamesForSqlStatement.TrimEnd(',');

            // generate a conditional condition for the sql. This might be necessary if some items come from sage and other from Gestproject at this point of the process. This would imply that some have a Gestproject Id but no Guid_id, and others the reverse. Therefore, it's necessary to generate the sql condition according to which id item has the entity.

            StringBuilder sqlCondition = new StringBuilder();

            if(condition1Data.conditionValue.GetType() == typeof(string))
            {
               if(condition1Data.conditionValue == "")
               {
                  sqlCondition.Append($"{condition2Data.conditionColumnName}={DynamicValuesFormatters.Formatters[condition2Data.conditionValue.GetType()](condition2Data.conditionValue)}");
               }
               else
               {
                  sqlCondition.Append($"{condition1Data.conditionColumnName}={DynamicValuesFormatters.Formatters[condition1Data.conditionValue.GetType()](condition1Data.conditionValue)}");
               };
            }
            else
            {
               if(condition1Data.conditionValue == null)
               {
                  sqlCondition.Append($"{condition2Data.conditionColumnName}={DynamicValuesFormatters.Formatters[condition2Data.conditionValue.GetType()](condition2Data.conditionValue)}");
               }
               else
               {
                  sqlCondition.Append($"{condition1Data.conditionColumnName}={DynamicValuesFormatters.Formatters[condition1Data.conditionValue.GetType()](condition1Data.conditionValue)}");
               };
            };

            string sqlString = $@"
            SELECT
               {fieldNamesForSqlStatement}
            FROM
               {tableName}
            WHERE
               {sqlCondition.ToString()}
            ;";

            //if(tableName == "INT_SAGE_SYNCHRONIZATION_ENTITY_DATA_SUBACCOUNTABLE_ACCOUNTS")
            //  new VisualizeData<string>(
            //      "At: " + MethodBase.GetCurrentMethod().DeclaringType.Name + "." + MethodBase.GetCurrentMethod().Name, 
            //      "SQL STRING:", 
            //      sqlString
            //  );

            using(SqlCommand sqlCommand = new SqlCommand(sqlString, connection))
            {
               using(SqlDataReader reader = sqlCommand.ExecuteReader())
               {
                  while(reader.Read())
                  {
                     Entity = entity;

                     PropertyInfo[] properties = Entity.GetType().GetProperties();

                     for(global::System.Int32 i = 0; i < fieldsToBeRetrieved.Count; i++)
                     {
                        TypeRevisor<T>.Check(
                           fieldsToBeRetrieved[i].columnType,
                           fieldsToBeRetrieved[i].columnName,
                           Entity,
                           reader,
                           i,
                           properties
                        );
                     };
                  };
               };
            };

            //if(Entity.GetType() == typeof(GestprojectTaxModel))
            //   new VisualizePropertiesAndValues<GestprojectTaxModel>("Populated GestprojectTaxModel", Entity as GestprojectTaxModel);

            return Entity;
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
