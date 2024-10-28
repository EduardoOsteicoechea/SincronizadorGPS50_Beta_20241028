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
   public class GetSage50IssuedBills
   {
      public List<Sage50IssuedBillModel> Entities { get; set; } = new List<Sage50IssuedBillModel>();
      public List<string> Codes { get; set; } = new List<string>();
      public List<string> Guids { get; set; } = new List<string>();
      public bool Exists { get; set; } = false;
      public GetSage50IssuedBills()
      {
         try
         {
            string sqlString1DatabaseName = DB.SQLDatabase("gestion","c_albven").ToString();
          
            string sqlString1 = $@"SELECT 
               GUID_ID,
               EMPRESA,
               LETRA,
               NUMERO,
               FECHA,
               OBRA,
               CLIENTE,
               IMPORTE,
               TOTALDOC,
               OBSERVACIO
            FROM {sqlString1DatabaseName};";

            //MessageBox.Show(sqlString1);

            DataTable table1 = new DataTable();

            DB.SQLExec(sqlString1, ref table1);

            if(table1.Rows.Count > 0)
            {
               for(int i = 0; i < table1.Rows.Count; i++)
               {
                  Sage50IssuedBillModel sage50Entity = new Sage50IssuedBillModel();

                  sage50Entity.GUID_ID = table1.Rows[i].ItemArray[0].ToString().Trim();
                  sage50Entity.EMPRESA = table1.Rows[i].ItemArray[1].ToString().Trim();
                  sage50Entity.LETRA = table1.Rows[i].ItemArray[2].ToString().Trim();
                  sage50Entity.NUMERO = table1.Rows[i].ItemArray[3].ToString().Trim();
                  sage50Entity.FECHA = Convert.ToDateTime(table1.Rows[i].ItemArray[4]);
                  sage50Entity.OBRA = table1.Rows[i].ItemArray[5].ToString().Trim();
                  sage50Entity.CLIENTE = table1.Rows[i].ItemArray[6].ToString().Trim();
                  sage50Entity.IMPORTE = Convert.ToDecimal(table1.Rows[i].ItemArray[7]);
                  sage50Entity.TOTALDOC = Convert.ToDecimal(table1.Rows[i].ItemArray[8]);
                  sage50Entity.OBSERVACIO = table1.Rows[i].ItemArray[9].ToString().Trim();

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
