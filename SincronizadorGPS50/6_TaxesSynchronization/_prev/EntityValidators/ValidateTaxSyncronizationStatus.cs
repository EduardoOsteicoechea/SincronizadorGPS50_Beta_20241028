using Infragistics.Designers.SqlEditor;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   public class ValidateTaxSyncronizationStatus
   {
      public bool MustBeDeleted { get; set; } = false;
      public bool NeverWasSynchronized { get; set; } = false;
      public bool IsSynchronized { get; set; } = true;

      public string CreateErrorMesage(string nombreDeCampo, string valorEnSage50)
      {
         return $"\"{nombreDeCampo}\" no coincide. Su valor en Sage50 es: \"{valorEnSage50}\". ";
      }
      public ValidateTaxSyncronizationStatus
      (
         GestprojectTaxModel gestprojectEntity,
         List<Sage50TaxModel> sage50EntityList,
         string entityNameColumnName,
         string entityValueColumnName,
         string entitySubaccountableAccountColumnName,
         string entitySubaccountableAccount2ColumnName,
         bool neverWasSynchronized
      )
      {
         try
         {
            if(gestprojectEntity.S50_CODE != null && gestprojectEntity.S50_CODE != "")
            {
               if(!neverWasSynchronized)
               {
                  for(int i = 0; i < sage50EntityList.Count; i++)
                  {
                     //MessageBox.Show(
                     //"sage50EntityList[i].GUID_ID.Trim(): " + sage50EntityList[i].GUID_ID.Trim() + "\n\n" +
                     //"gestprojectEntity.S50_GUID_ID.Trim(): " + gestprojectEntity.S50_GUID_ID.Trim() + ""
                     //+ "\n\n\n" +
                     //"sage50EntityList[i].NOMBRE.Trim(): " + sage50EntityList[i].NOMBRE.Trim() + "\n\n" +
                     //"gestprojectEntity.IMP_DESCRIPCION.Trim(): " + gestprojectEntity.IMP_DESCRIPCION.Trim() + ""
                     //+ "\n\n\n" +
                     //"sage50EntityList[i].IVA: " + sage50EntityList[i].IVA + "\n\n" +
                     //"Math.Round(gestprojectEntity.IMP_VALOR, 2).ToString()): " + Math.Round(gestprojectEntity.IMP_VALOR, 2).ToString() + ""
                     //+ "\n\n\n" +
                     //"(sage50EntityList[i].CTA_IV_REP ?? \"\").ToString(): " + (sage50EntityList[i].CTA_IV_REP ?? "").ToString() + "\n\n" +
                     //"(gestprojectEntity.IMP_SUBCTA_CONTABLE ?? \"\").ToString(): " + (gestprojectEntity.IMP_SUBCTA_CONTABLE ?? "").ToString() + ""
                     //+ "\n\n\n" +
                     //"(sage50EntityList[i].CTA_IV_SOP ?? \"\").ToString(): " + (sage50EntityList[i].CTA_IV_SOP ?? "").ToString() + "\n\n" +
                     //"gestprojectEntity.IMP_SUBCTA_CONTABLE_2.Trim(): " + gestprojectEntity.IMP_SUBCTA_CONTABLE_2.Trim() + 
                     //"\n\n\n" +                      
                     //(sage50EntityList[i].NOMBRE.Trim() == gestprojectEntity.IMP_DESCRIPCION.Trim()).ToString() + 
                     //"\n\n" +                              
                     //(Math.Round(Convert.ToDecimal(sage50EntityList[i].IVA), 2).ToString().Trim() == (Math.Round(gestprojectEntity.IMP_VALOR, 2)).ToString().Trim()).ToString() + 
                     //"\n\n" +                              
                     //(sage50EntityList[i].CTA_IV_REP.Trim() == gestprojectEntity.IMP_SUBCTA_CONTABLE.Trim()).ToString() + 
                     //"\n\n" +                              
                     //(sage50EntityList[i].CTA_IV_SOP.Trim() == gestprojectEntity.IMP_SUBCTA_CONTABLE_2.Trim()).ToString()
                     //);

                     if(sage50EntityList[i].GUID_ID.Trim() == gestprojectEntity.S50_GUID_ID.Trim())
                     {
                        if(sage50EntityList[i].NOMBRE.Trim() != gestprojectEntity.IMP_DESCRIPCION.Trim())
                        {
                           NeverWasSynchronized = false;
                           IsSynchronized = false;
                           MustBeDeleted = false;
                           gestprojectEntity.COMMENTS += this.CreateErrorMesage(entityNameColumnName, sage50EntityList[i].NOMBRE);
                        };

                        if(gestprojectEntity.IMP_TIPO == "IVA")
                        {
                           if(sage50EntityList[i].IVA != Math.Round(gestprojectEntity.IMP_VALOR, 2).ToString())
                           {
                              NeverWasSynchronized = false;
                              IsSynchronized = false;
                              MustBeDeleted = false;

                              gestprojectEntity.COMMENTS += this.CreateErrorMesage(entityValueColumnName, (sage50EntityList[i].IVA ?? "").ToString());
                           };

                           if((sage50EntityList[i].CTA_IV_REP ?? "").ToString() != (gestprojectEntity.IMP_SUBCTA_CONTABLE ?? "").ToString())
                           {
                              NeverWasSynchronized = false;
                              IsSynchronized = false;
                              MustBeDeleted = false;
                              gestprojectEntity.COMMENTS += this.CreateErrorMesage(entitySubaccountableAccountColumnName, (sage50EntityList[i].CTA_IV_REP ?? "").ToString());
                           };

                           if((sage50EntityList[i].CTA_IV_SOP ?? "").ToString() != gestprojectEntity.IMP_SUBCTA_CONTABLE_2.Trim())
                           {
                              NeverWasSynchronized = false;
                              IsSynchronized = false;
                              MustBeDeleted = false;
                              gestprojectEntity.COMMENTS += this.CreateErrorMesage(entitySubaccountableAccount2ColumnName, (sage50EntityList[i].CTA_IV_SOP ?? "").ToString());
                           };

                           if
                           (
                              sage50EntityList[i].NOMBRE.Trim() == gestprojectEntity.IMP_DESCRIPCION.Trim()
                              &&
                              Math.Round(Convert.ToDecimal(sage50EntityList[i].IVA), 2).ToString().Trim() == (Math.Round(gestprojectEntity.IMP_VALOR, 2)).ToString().Trim()
                              &&
                              sage50EntityList[i].CTA_IV_REP.Trim() == gestprojectEntity.IMP_SUBCTA_CONTABLE.Trim()
                              &&
                              sage50EntityList[i].CTA_IV_SOP.Trim() == gestprojectEntity.IMP_SUBCTA_CONTABLE_2.Trim()
                           )
                           {
                              //MessageBox.Show("Sincronizado");
                              NeverWasSynchronized = false;
                              IsSynchronized = true;
                              MustBeDeleted = false;
                              gestprojectEntity.COMMENTS = "";
                              gestprojectEntity.SYNC_STATUS = SynchronizationStatusOptions.Sincronizado;
                           };
                           break;
                        }
                        else
                        {
                           if(sage50EntityList[i].RETENCION != Math.Round(gestprojectEntity.IMP_VALOR, 2))
                           {
                              NeverWasSynchronized = false;
                              IsSynchronized = false;
                              MustBeDeleted = false;
                              gestprojectEntity.COMMENTS += this.CreateErrorMesage(entitySubaccountableAccount2ColumnName, sage50EntityList[i].RETENCION.ToString());
                           };

                           if((sage50EntityList[i].CTA_RE_REP ?? "").ToString() != gestprojectEntity.IMP_SUBCTA_CONTABLE.Trim())
                           {
                              NeverWasSynchronized = false;
                              IsSynchronized = false;
                              MustBeDeleted = false;
                              gestprojectEntity.COMMENTS += this.CreateErrorMesage(entitySubaccountableAccount2ColumnName, (sage50EntityList[i].CTA_RE_REP ?? "").ToString());
                           };

                           if((sage50EntityList[i].CTA_RE_SOP ?? "").ToString() != gestprojectEntity.IMP_SUBCTA_CONTABLE_2.Trim())
                           {
                              NeverWasSynchronized = false;
                              IsSynchronized = false;
                              MustBeDeleted = false;
                              gestprojectEntity.COMMENTS += this.CreateErrorMesage(entitySubaccountableAccount2ColumnName, (sage50EntityList[i].CTA_RE_SOP ?? "").ToString());
                           };

                           if
                           (
                              sage50EntityList[i].NOMBRE.Trim() == gestprojectEntity.IMP_DESCRIPCION.Trim()
                              &&
                              Math.Round(sage50EntityList[i].RETENCION, 2) == Math.Round(gestprojectEntity.IMP_VALOR, 2)
                              //sage50EntityList[i].IVA == gestprojectEntity.IMP_VALOR
                              &&
                              sage50EntityList[i].CTA_RE_REP.Trim() == gestprojectEntity.IMP_SUBCTA_CONTABLE.Trim()
                              &&
                              sage50EntityList[i].CTA_RE_SOP.Trim() == gestprojectEntity.IMP_SUBCTA_CONTABLE_2.Trim()
                           )
                           {
                              //MessageBox.Show("Sincronizado");
                              NeverWasSynchronized = false;
                              IsSynchronized = true;
                              MustBeDeleted = false;
                              gestprojectEntity.COMMENTS = "";
                              gestprojectEntity.SYNC_STATUS = SynchronizationStatusOptions.Sincronizado;
                           };
                           break;
                        };
                     }
                     else
                     {
                        //MessageBox.Show("Eliminado en Sage");
                        NeverWasSynchronized = true;
                        MustBeDeleted = false;
                        gestprojectEntity.SYNC_STATUS = SynchronizationStatusOptions.Desincronizado;
                     }; 
                     //break;                 
                  };                
               }
               else
               {
                  //MessageBox.Show("Nunca sincronizado");
                  NeverWasSynchronized = true;
                  IsSynchronized = false;
                  MustBeDeleted = false;
                  gestprojectEntity.SYNC_STATUS = SynchronizationStatusOptions.Desincronizado;                  
               };  
            }
            else
            {
               //MessageBox.Show("Nunca sincronizado");
               NeverWasSynchronized = true;
               IsSynchronized = false;
               MustBeDeleted = false;
               gestprojectEntity.SYNC_STATUS = SynchronizationStatusOptions.Desincronizado;
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