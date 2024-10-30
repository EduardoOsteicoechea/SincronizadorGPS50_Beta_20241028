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
   public class GetSage50ReceivedBills
   {
      public List<Sage50ReceivedBillModel> Entities { get; set; } = new List<Sage50ReceivedBillModel>();
      public List<string> Codes { get; set; } = new List<string>();
      public List<string> Guids { get; set; } = new List<string>();
      public bool Exists { get; set; } = false;
      public GetSage50ReceivedBills()
      {
         try
         {
            string sqlString1DatabaseName = DB.SQLDatabase("gestion","c_factucom").ToString();
          
            string sqlString1 = $@"
               SELECT 
                  GUID_ID
                  ,EMPRESA
                  ,NUMERO
                  ,CREATED
                  ,PROVEEDOR
                  ,IMPORTE
                  ,TOTALDOC
               FROM
                  {DB.SQLDatabase("gestion","c_factucom")}
            ;";

            //MessageBox.Show(sqlString1);

            DataTable table1 = new DataTable();

            DB.SQLExec(sqlString1, ref table1);

            if(table1.Rows.Count > 0)
            {
               for(int i = 0; i < table1.Rows.Count; i++)
               {
                  Sage50ReceivedBillModel sage50Entity = new Sage50ReceivedBillModel();

                  sage50Entity.GUID_ID = table1.Rows[i].ItemArray[0].ToString().Trim();
                  sage50Entity.EMPRESA = table1.Rows[i].ItemArray[1].ToString().Trim();
                  sage50Entity.NUMERO = table1.Rows[i].ItemArray[2].ToString().Trim();
                  sage50Entity.CREATED = Convert.ToDateTime(table1.Rows[3].ItemArray[4]);
                  sage50Entity.PROVEEDOR = table1.Rows[i].ItemArray[4].ToString().Trim();
                  sage50Entity.IMPORTE = Convert.ToDecimal(table1.Rows[i].ItemArray[5]);
                  sage50Entity.TOTALDOC = Convert.ToDecimal(table1.Rows[i].ItemArray[6]);

                  Entities.Add(sage50Entity);
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
