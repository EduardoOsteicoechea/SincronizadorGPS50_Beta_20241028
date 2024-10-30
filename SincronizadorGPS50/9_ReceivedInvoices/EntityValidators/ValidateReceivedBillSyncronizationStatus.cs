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
   public class ValidateReceivedBillSyncronizationStatus
   {
      public bool MustBeDeleted { get; set; } = false;
      public bool NeverWasSynchronized { get; set; } = false;
      public bool IsSynchronized { get; set; } = true;

      public string CreateErrorMesage(string nombreDeCampo, string valorEnSage50)
      {
         return $"\"{nombreDeCampo}\" no coincide. Su valor en Sage50 es: \"{valorEnSage50}\". ";
      }
      public ValidateReceivedBillSyncronizationStatus
      (
         GestprojectReceivedBillModel gestprojectEntity,
         List<Sage50ReceivedBillModel> sage50EntityList,
         string GestprojectDaoId,
         string GestprojectBillNumber,
         string GestprojectDate,
         string GestprojectCliId,
         string GestprojectTaxableBase,
         string GestprojectBillTotal,
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

                        if(sage50EntityList[i].NUMERO.Trim() != gestprojectEntity.FCP_NUM_FACTURA.ToString())
                        {
                           NeverWasSynchronized = false;
                           IsSynchronized = false;
                           MustBeDeleted = false;
                           gestprojectEntity.COMMENTS += this.CreateErrorMesage(GestprojectBillNumber, sage50EntityList[i].NUMERO.Trim());
                        };

                        if(sage50EntityList[i].CREATED != gestprojectEntity.FCP_FECHA)
                        {
                           NeverWasSynchronized = false;
                           IsSynchronized = false;
                           MustBeDeleted = false;
                           gestprojectEntity.COMMENTS += this.CreateErrorMesage(GestprojectDate, sage50EntityList[i].CREATED.ToString());
                        };

                        if(sage50EntityList[i].PROVEEDOR.Trim() != gestprojectEntity.PAR_PRO_ID.ToString().Trim())
                        {
                           NeverWasSynchronized = false;
                           IsSynchronized = false;
                           MustBeDeleted = false;
                           gestprojectEntity.COMMENTS += this.CreateErrorMesage(GestprojectCliId, sage50EntityList[i].PROVEEDOR.Trim());
                        };

                        if(sage50EntityList[i].IMPORTE != gestprojectEntity.FCP_BASE_IMPONIBLE)
                        {
                           NeverWasSynchronized = false;
                           IsSynchronized = false;
                           MustBeDeleted = false;
                           gestprojectEntity.COMMENTS += this.CreateErrorMesage(GestprojectTaxableBase, sage50EntityList[i].IMPORTE.ToString());
                        };

                        if(sage50EntityList[i].TOTALDOC != gestprojectEntity.FCP_TOTAL_FACTURA)
                        {
                           NeverWasSynchronized = false;
                           IsSynchronized = false;
                           MustBeDeleted = false;
                           gestprojectEntity.COMMENTS += this.CreateErrorMesage(GestprojectBillTotal, sage50EntityList[i].TOTALDOC.ToString());
                        };

                        if
                        (
                           sage50EntityList[i].EMPRESA.Trim() == gestprojectEntity.PAR_DAO_ID.ToString()
                           &&
                           sage50EntityList[i].NUMERO.Trim() == gestprojectEntity.FCP_NUM_FACTURA.ToString()
                           &&
                           sage50EntityList[i].CREATED == gestprojectEntity.FCP_FECHA
                           &&
                           sage50EntityList[i].PROVEEDOR == gestprojectEntity.PAR_PRO_ID.ToString()
                           &&
                           sage50EntityList[i].IMPORTE == gestprojectEntity.FCP_BASE_IMPONIBLE
                           &&
                           sage50EntityList[i].TOTALDOC == gestprojectEntity.FCP_TOTAL_FACTURA
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
