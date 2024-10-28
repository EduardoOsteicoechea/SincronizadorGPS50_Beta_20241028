using sage.ew.db;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   public class GetSage50Projects
   {
      public List<Sage50ProjectModel> Entities { get; set; } = new List<Sage50ProjectModel>();
      public List<string> Codes { get; set; } = new List<string>();
      public List<string> Guids { get; set; } = new List<string>();
      public int LastCodeValue { get; set; }
      public int NextCodeAvailable { get; set; }
      public string Code { get; set; }
      public string Guid { get; set; }
      public bool Exists { get; set; } = false;
      public GetSage50Projects
      (
         (
            (string sageDispactcherMechanismRoute, string tableName) dispatcherAndName,
            List<(string name, Type type)> tableFieldsAlongTypes
         ) sageTableDataEntity = default
      )
      {
         try
         {
            //StringBuilder fieldsToQueryStringBuilder = new StringBuilder();
            //foreach(var item in sageTableDataEntity.tableFieldsAlongTypes)
            //{
            //   fieldsToQueryStringBuilder.Append($"[{item.name}], ");
            //};

            string getSage50EntitySQLQuery = $@"
            SELECT 
               CODIGO
               ,NOMBRE
               ,DIRECCION
               ,CODPOST
               ,POBLACION
               ,PROVINCIA
               ,GUID_ID
            FROM 
               {DB.SQLDatabase("comunes","obra")}
            ;";

            //MessageBox.Show(getSage50EntitySQLQuery);

            DataTable sage50EntityDataTable = new DataTable();

            DB.SQLExec(getSage50EntitySQLQuery, ref sage50EntityDataTable);

            if(sage50EntityDataTable.Rows.Count > 0)
            {
               for(int i = 0; i < sage50EntityDataTable.Rows.Count; i++)
               {
                  Sage50ProjectModel entity = new Sage50ProjectModel();
                  DataRow dataRow = sage50EntityDataTable.Rows[i];

                  entity.CODIGO = dataRow.ItemArray[0].ToString().Trim();
                  entity.NOMBRE = dataRow.ItemArray[1].ToString().Trim();
                  entity.DIRECCION = dataRow.ItemArray[2].ToString().Trim();
                  entity.CODPOST = dataRow.ItemArray[3].ToString().Trim();
                  entity.POBLACION = dataRow.ItemArray[4].ToString().Trim();
                  entity.PROVINCIA = dataRow.ItemArray[5].ToString().Trim();
                  entity.GUID_ID = dataRow.ItemArray[6].ToString().Trim();


                  //PropertyInfo[] entityPropertyInfoArray = entity.GetType().GetProperties();

                  //for(global::System.Int32 j = 0; j < entityPropertyInfoArray.Length; j++)
                  //{
                  //   PropertyInfo property = entityPropertyInfoArray[j];
                  //   Type entityType = sageTableDataEntity.tableFieldsAlongTypes[j].type;
                  //   property.SetValue(
                  //      entity,
                  //      sage50EntityDataTable.Rows[i].ItemArray[j].ToString().Trim() ?? ""
                  //   );
                  //};

                  Entities.Add(entity);
                  Codes.Add(entity.CODIGO);
                  Guids.Add(entity.GUID_ID);
               };

               int Sage50HigestCodeNumber = int.Parse(Entities.Last().CODIGO);
               Sage50HigestCodeNumber++;

               if(Entities.Count > 0)
               {
                  LastCodeValue = Sage50HigestCodeNumber;
                  NextCodeAvailable = Sage50HigestCodeNumber + 1;
               }
               else
               {
                  LastCodeValue = 1;
                  NextCodeAvailable = 2;
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
         };
      }
   }
}
