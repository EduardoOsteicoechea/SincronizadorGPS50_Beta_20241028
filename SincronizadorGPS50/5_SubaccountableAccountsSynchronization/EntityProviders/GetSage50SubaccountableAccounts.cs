using sage.ew.db;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   public class GetSage50SubaccountableAccounts
   {
      public List<Sage50SubaccountableAccountModel> Entities { get; set; } = new List<Sage50SubaccountableAccountModel>();
      public GetSage50SubaccountableAccounts
      (      
         ISynchronizationTableSchemaProvider tableSchema
      )
      {
         try
         {
            string sqlString1DatabaseName = DB.SQLDatabase(tableSchema.SageTableData.dispatcherAndName.sageDispactcherMechanismRoute,tableSchema.SageTableData.dispatcherAndName.tableName).ToString();

            string sqlString1 = $@"
               SELECT 
                  CODIGO, 
                  NOMBRE, 
                  GUID_ID
               FROM 
                  {sqlString1DatabaseName}  
            ;";

            DataTable table1 = new DataTable();

            DB.SQLExec(sqlString1, ref table1);

            if(table1.Rows.Count > 0)
            {
               for(int i = 0; i < table1.Rows.Count; i++)
               {
                  if(
                     table1.Rows[i].ItemArray[0].ToString().Substring(0,1) == "6"
                     ||
                     table1.Rows[i].ItemArray[0].ToString().Substring(0,1) == "7"
                     ||
                     table1.Rows[i].ItemArray[0].ToString().Substring(0,3) == "553"
                  )
                  {
                     Sage50SubaccountableAccountModel sage50Entity = new Sage50SubaccountableAccountModel();

                     sage50Entity.CODIGO = table1.Rows[i].ItemArray[0].ToString().Trim();
                     sage50Entity.NOMBRE = table1.Rows[i].ItemArray[1].ToString().Trim();
                     sage50Entity.GUID_ID = table1.Rows[i].ItemArray[2].ToString().Trim();

                     Entities.Add(sage50Entity);
                  };
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
