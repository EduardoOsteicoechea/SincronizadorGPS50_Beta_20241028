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
   public class GetSage50Taxes
   {
      public List<Sage50TaxModel> Entities { get; set; } = new List<Sage50TaxModel>();
      public List<string> Codes { get; set; } = new List<string>();
      public List<string> Guids { get; set; } = new List<string>();
      public int LastCodeValue { get; set; }
      public int NextCodeAvailable { get; set; }
      public string Code { get; set; }
      public string Guid { get; set; }
      public bool Exists { get; set; } = false;
      public GetSage50Taxes()
      {
         try
         {
            string sqlString1DatabaseName = DB.SQLDatabase("gestion","tipo_iva").ToString();
          
            string sqlString1 = $@"SELECT 
               [GUID_ID], 
               [IVA], 
               [NOMBRE],
               [CTA_IV_REP], 
               [CTA_IV_SOP],
               [CODIGO]
            FROM {sqlString1DatabaseName};";

            //MessageBox.Show(sqlString1);

            DataTable table1 = new DataTable();

            DB.SQLExec(sqlString1, ref table1);

            if(table1.Rows.Count > 0)
            {
               for(int i = 0; i < table1.Rows.Count; i++)
               {
                  Sage50TaxModel sage50Entity = new Sage50TaxModel();

                  sage50Entity.GUID_ID = table1.Rows[i].ItemArray[0].ToString().Trim();
                  //sage50Entity.IVA = Convert.ToDecimal(table1.Rows[i].ItemArray[1]);
                  sage50Entity.IVA = Convert.ToString(table1.Rows[i].ItemArray[1]);
                  sage50Entity.NOMBRE = table1.Rows[i].ItemArray[2].ToString().Trim();
                  sage50Entity.CTA_IV_REP = table1.Rows[i].ItemArray[3].ToString().Trim();
                  sage50Entity.CTA_IV_SOP = table1.Rows[i].ItemArray[4].ToString().Trim();
                  sage50Entity.CODIGO = table1.Rows[i].ItemArray[5].ToString().Trim();
                  sage50Entity.IMP_TIPO = "IVA";

                  Entities.Add(sage50Entity);
               };
            };

            string sqlString2 = $@"
                SELECT 
                    guid_id,
                    nombre,
                    retencion,
                    cta_re_rep,
                    cta_re_sop,
                    codigo
                FROM {DB.SQLDatabase("gestion","tipo_ret")};";

            DataTable table2 = new DataTable();

            DB.SQLExec(sqlString2, ref table2);

            if(table2.Rows.Count > 0)
            {
               for(int i = 0; i < table2.Rows.Count; i++)
               {
                  Sage50TaxModel sage50Entity = new Sage50TaxModel();

                  sage50Entity.GUID_ID = table2.Rows[i].ItemArray[0].ToString().Trim();
                  sage50Entity.NOMBRE = table2.Rows[i].ItemArray[1].ToString().Trim();
                  sage50Entity.RETENCION = Convert.ToDecimal(table2.Rows[i].ItemArray[2]);
                  sage50Entity.CTA_RE_REP = table2.Rows[i].ItemArray[3].ToString().Trim();
                  sage50Entity.CTA_RE_SOP = table2.Rows[i].ItemArray[4].ToString().Trim();
                  sage50Entity.CODIGO = table2.Rows[i].ItemArray[5].ToString().Trim();
                  sage50Entity.IMP_TIPO = "IRPF";

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
