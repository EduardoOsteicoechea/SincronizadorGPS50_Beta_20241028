using sage.ew.db;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   internal class Sage50Entities<T> where T : new()
   {
      public List<T> EntityList { get; set; } = new List<T>();
      public List<T> GetAll
      (
         string sageDispactcherMechanismRoute,
         string tableName,
         List<(string columName, System.Type columnType)> fieldsToBeRetrieved
      ) 
      {
         try
         {
            string fieldNamesForSqlStatement = string.Empty;
            for(global::System.Int32 i = 0; i < fieldsToBeRetrieved.Count; i++)
            {
               fieldNamesForSqlStatement += $"{fieldsToBeRetrieved[i].columName},";
            };
            fieldNamesForSqlStatement = fieldNamesForSqlStatement.TrimEnd(',');

            string getSage50ProviderSQLQuery = $@"
            SELECT 
               {fieldNamesForSqlStatement}
            FROM 
               {DB.SQLDatabase(sageDispactcherMechanismRoute,tableName)}";

            DataTable entityDataTable = new DataTable();

            DB.SQLExec(getSage50ProviderSQLQuery, ref entityDataTable);

            if(entityDataTable.Rows.Count > 0)
            {
               for(int i = 0; i < entityDataTable.Rows.Count; i++)
               {
                  T entity = new T();

                  for(global::System.Int32 j = 0; j < fieldsToBeRetrieved.Count; j++)
                  {
                     try
                     {
                        var entityColumnValue = entityDataTable.Rows[i].ItemArray[j].ToString().Trim();
                        typeof(T).GetProperty(fieldsToBeRetrieved[j].columName).SetValue(entity, entityColumnValue);
                     }
                     catch(System.Exception exception)
                     {
                        MessageBox.Show(
                           "entityColumnValue: " + entityDataTable.Rows[i].ItemArray[j].ToString().Trim() + "\n" +
                           "fieldsToBeRetrieved[j].columName: " + fieldsToBeRetrieved[j].columName + "\n" +
                           "entity: " + entity + "\n"
                        );
                        throw ApplicationLogger.ReportError(
                           MethodBase.GetCurrentMethod().DeclaringType.Namespace,
                           MethodBase.GetCurrentMethod().DeclaringType.Name,
                           MethodBase.GetCurrentMethod().Name,
                           exception
                        );
                     };
                  };

                  EntityList.Add(entity);
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
         };
      }
   }
}
