using sage.ew.db;
using System;
using System.Collections.Generic;
using System.Data;

namespace SincronizadorGPS50
{
   public class ProjectComparer
   {
      public List<Sage50ProjectModel> Sage50EntityList { get; set; } = new List<Sage50ProjectModel>();
      public bool EntityExists { get; set; } = false;
      public string GuidId { get; set; } = "";
      public string Code { get; set; } = "";
      public ProjectComparer
      (
         ISynchronizationTableSchemaProvider tableSchema,
         string name,
         string code
      )
      {
         try
         {
            Sage50EntityList = GetSage50Projects(tableSchema);

            foreach(Sage50ProjectModel entity in Sage50EntityList)
            {
               if( name == entity.NOMBRE && code == entity.CODIGO )
               {
                  GuidId = entity.GUID_ID;
                  Code = entity.CODIGO;
                  EntityExists = true;
               };
            };
         }
         catch(System.Exception exception)
         {
            throw ApplicationLogger.ReportError(
               System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
               System.Reflection.MethodBase.GetCurrentMethod().Name,
               exception
            );
         };
      }

      public List<Sage50ProjectModel> GetSage50Projects(ISynchronizationTableSchemaProvider tableSchema)
      {
         try
         {
            List<Sage50ProjectModel> entityList = Sage50EntityList;

            string sqlString = $@"
                SELECT 
                    CODIGO, 
                    NOMBRE, 
                    DIRECCION, 
                    CODPOST, 
                    POBLACION, 
                    PROVINCIA, 
                    GUID_ID
                FROM 
                  {DB.SQLDatabase(tableSchema.SageTableData.dispatcherAndName.sageDispactcherMechanismRoute,tableSchema.SageTableData.dispatcherAndName.tableName)}
               ;";

            DataTable sage50EntitiesDataTable = new DataTable();

            DB.SQLExec(sqlString, ref sage50EntitiesDataTable);

            if(sage50EntitiesDataTable.Rows.Count > 0)
            {
               for(global::System.Int32 i = 0; i < sage50EntitiesDataTable.Rows.Count; i++)
               {
                  Sage50ProjectModel entity = new Sage50ProjectModel();
                  
                  (int, string) codeIndex = (0,"");
                  (int, string) nameIndex = (1,"");
                  (int, string) addressIndex = (2,"");
                  (int, string) postalCodeIndex = (3,"");
                  (int, string) localityIndex = (4,"");
                  (int, string) provinceIndex = (5,"");
                  (int, string) guidIndex = (6,"");

                  entity.CODIGO = Convert.ToString(sage50EntitiesDataTable.Rows[i]
                  .ItemArray[codeIndex.Item1].ToString().Trim() == "DBNull" ? 
                             codeIndex.Item2 : sage50EntitiesDataTable.Rows[i]
                  .ItemArray[codeIndex.Item1].ToString().Trim());
               
                  entity.NOMBRE = Convert.ToString(sage50EntitiesDataTable.Rows[i]
                  .ItemArray[nameIndex.Item1].ToString().Trim() == "DBNull" ? 
                             nameIndex.Item2 : sage50EntitiesDataTable.Rows[i]
                  .ItemArray[nameIndex.Item1].ToString().Trim());
                  
                  entity.DIRECCION = Convert.ToString(sage50EntitiesDataTable.Rows[i]
                  .ItemArray[addressIndex.Item1].ToString().Trim() == "DBNull" ? 
                             addressIndex.Item2 : sage50EntitiesDataTable.Rows[i]
                  .ItemArray[addressIndex.Item1].ToString().Trim());
                  
                  entity.CODPOST = Convert.ToString(sage50EntitiesDataTable.Rows[i]
                  .ItemArray[postalCodeIndex.Item1].ToString().Trim() == "DBNull" ? 
                             postalCodeIndex.Item2 : sage50EntitiesDataTable.Rows[i]
                  .ItemArray[postalCodeIndex.Item1].ToString().Trim());
                  
                  entity.POBLACION = Convert.ToString(sage50EntitiesDataTable.Rows[i]
                  .ItemArray[localityIndex.Item1].ToString().Trim() == "DBNull" ? 
                             localityIndex.Item2 : sage50EntitiesDataTable.Rows[i]
                  .ItemArray[localityIndex.Item1].ToString().Trim());
                  
                  entity.PROVINCIA = Convert.ToString(sage50EntitiesDataTable.Rows[i]
                  .ItemArray[provinceIndex.Item1].ToString().Trim() == "DBNull" ? 
                             provinceIndex.Item2 : sage50EntitiesDataTable.Rows[i]
                  .ItemArray[provinceIndex.Item1].ToString().Trim());

                  entity.GUID_ID = Convert.ToString(sage50EntitiesDataTable.Rows[i]
                  .ItemArray[guidIndex.Item1].ToString().Trim() == "DBNull" ? 
                             guidIndex.Item2 : sage50EntitiesDataTable.Rows[i]
                  .ItemArray[guidIndex.Item1].ToString().Trim());

                  entityList.Add(entity);
               };
            };

            return entityList;
         }
         catch(System.Exception exception)
         {
            throw ApplicationLogger.ReportError(
               System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
               System.Reflection.MethodBase.GetCurrentMethod().Name,
               exception
            );
         };
      }
   }
}
