using Infragistics.Designers.SqlEditor;
using sage.ew.cliente;
using sage.ew.docsven;
using sage.ew.empresa;
using sage.ew.interficies;
using sage.ew.objetos;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using static sage.ew.contabilidad.Cuenta.DatosRelacionados;

namespace SincronizadorGPS50
{
   public class ValidateIssuedBillSyncronizationStatus
   {
      public bool MustBeDeleted { get; set; } = false;
      public bool NeverWasSynchronized { get; set; } = false;
      public bool IsSynchronized { get; set; } = true;

      public string CreateErrorMesage(string nombreDeCampo, string valorEnSage50)
      {
         return $"\"{nombreDeCampo}\" no coincide. Su valor en Sage50 es: \"{valorEnSage50}\". ";
      }
      public ValidateIssuedBillSyncronizationStatus
      (
         SynchronizableIssuedInvoiceModel gestprojectEntity,
         List<Sage50IssuedBillModel> sage50EntityList,
         string GestprojectDaoId,
         string GestprojectReference,
         string GestprojectDate,
         string GestprojectCliId,
         string GestprojectTaxableBase,
         string GestprojectBillTotal,
         string GestprojectBillObservations,
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
                     if(sage50EntityList[i].GUID_ID.Trim() == gestprojectEntity.S50_GUID_ID.Trim())
                     {
                        if(sage50EntityList[i].EMPRESA.Trim() != gestprojectEntity.PAR_DAO_ID.ToString().Trim())
                        {
                           NeverWasSynchronized = false;
                           IsSynchronized = false;
                           MustBeDeleted = false;
                           gestprojectEntity.COMMENTS += this.CreateErrorMesage(GestprojectDaoId, sage50EntityList[i].EMPRESA.Trim());
                        };

                        if(sage50EntityList[i].NUMERO != gestprojectEntity.FCE_REFERENCIA)
                        {
                           NeverWasSynchronized = false;
                           IsSynchronized = false;
                           MustBeDeleted = false;
                           gestprojectEntity.COMMENTS += this.CreateErrorMesage(GestprojectReference, sage50EntityList[i].NUMERO.ToString());
                        };

                        if(sage50EntityList[i].FECHA != gestprojectEntity.FCE_FECHA)
                        {
                           NeverWasSynchronized = false;
                           IsSynchronized = false;
                           MustBeDeleted = false;
                           gestprojectEntity.COMMENTS += this.CreateErrorMesage(GestprojectDate, sage50EntityList[i].FECHA.ToString());
                        };

                        if(sage50EntityList[i].CLIENTE.Trim() != gestprojectEntity.PAR_CLI_ID.ToString().Trim())
                        {
                           NeverWasSynchronized = false;
                           IsSynchronized = false;
                           MustBeDeleted = false;
                           gestprojectEntity.COMMENTS += this.CreateErrorMesage(GestprojectCliId, sage50EntityList[i].CLIENTE.Trim());
                        };

                        if(sage50EntityList[i].IMPORTE != gestprojectEntity.FCE_BASE_IMPONIBLE)
                        {
                           NeverWasSynchronized = false;
                           IsSynchronized = false;
                           MustBeDeleted = false;
                           gestprojectEntity.COMMENTS += this.CreateErrorMesage(GestprojectTaxableBase, sage50EntityList[i].IMPORTE.ToString());
                        };

                        if(sage50EntityList[i].TOTALDOC != gestprojectEntity.FCE_TOTAL_FACTURA)
                        {
                           NeverWasSynchronized = false;
                           IsSynchronized = false;
                           MustBeDeleted = false;
                           gestprojectEntity.COMMENTS += this.CreateErrorMesage(GestprojectBillTotal, sage50EntityList[i].TOTALDOC.ToString());
                        };

                        if(sage50EntityList[i].OBSERVACIO.Trim() != gestprojectEntity.FCE_OBSERVACIONES.Trim())
                        {
                           NeverWasSynchronized = false;
                           IsSynchronized = false;
                           MustBeDeleted = false;
                           gestprojectEntity.COMMENTS += this.CreateErrorMesage(GestprojectBillObservations, sage50EntityList[i].OBSERVACIO);
                        };

                        // sage50EntityList[i].LETRA == gestprojectEntity.FCE_REFERENCIA. No se usa en v.1.0.0.0

                        if
                        (
                           sage50EntityList[i].EMPRESA.Trim() == gestprojectEntity.PAR_DAO_ID.ToString()
                           &&
                           sage50EntityList[i].NUMERO.Trim() == gestprojectEntity.FCE_REFERENCIA.ToString()
                           &&
                           sage50EntityList[i].FECHA == gestprojectEntity.FCE_FECHA
                           &&
                           sage50EntityList[i].CLIENTE == gestprojectEntity.PAR_CLI_ID.ToString()
                           &&
                           sage50EntityList[i].IMPORTE == gestprojectEntity.FCE_BASE_IMPONIBLE
                           &&
                           sage50EntityList[i].TOTALDOC == gestprojectEntity.FCE_TOTAL_FACTURA
                           &&
                           sage50EntityList[i].OBSERVACIO.Trim() == gestprojectEntity.FCE_OBSERVACIONES.Trim()
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
                        //MessageBox.Show("Eliminado en Sage");
                        NeverWasSynchronized = true;
                        MustBeDeleted = true;
                        gestprojectEntity.SYNC_STATUS = SynchronizationStatusOptions.Desincronizado;
                     };                  
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
