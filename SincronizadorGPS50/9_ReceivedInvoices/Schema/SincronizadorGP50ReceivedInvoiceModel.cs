using System;
using System.Linq;

namespace SincronizadorGPS50
{
   public class SincronizadorGP50ReceivedInvoiceModel : ISynchronizationModel
   {
      // Gestproject fields
      public int? FCP_ID { get; set; } = -1;
      public int? PAR_DAO_ID { get; set; } = -1;
      public string FCP_NUM_FACTURA { get; set; } = "";
      public DateTime? FCP_FECHA { get; set; } = DateTime.Now;
      public int? PAR_PRO_ID { get; set; } = -1;
      public string FCP_SUBCTA_CONTABLE { get; set; } = "";
      public decimal? FCP_BASE_IMPONIBLE { get; set; } = 0;
      public decimal? FCP_VALOR_IVA { get; set; } = 0;
      public decimal? FCP_IVA { get; set; } = 0;
      public decimal? FCP_VALOR_IRPF { get; set; } = 0;
      public decimal? FCP_IRPF { get; set; } = 0;
      public decimal? FCP_TOTAL_FACTURA { get; set; } = 0;
      public string FCP_OBSERVACIONES { get; set; } = "";
      public int? PRY_ID { get; set; } = -1;
      public string FCP_EJERCICIO { get; set; } = DateTime.Now.Year.ToString();

      // Syncronization fields
      public int? ID { get; set; } = -1;
      public string SYNC_STATUS { get; set; } = "";
      public string S50_CODE { get; set; } = "";
      public string S50_GUID_ID { get; set; } = "";
      public string S50_COMPANY_GROUP_NAME { get; set; } = "";
      public string S50_COMPANY_GROUP_CODE { get; set; } = "";
      public string S50_COMPANY_GROUP_MAIN_CODE { get; set; } = "";
      public string S50_COMPANY_GROUP_GUID_ID { get; set; } = "";
      public DateTime? LAST_UPDATE { get; set; } = DateTime.Now;
      public int? GP_USU_ID { get; set; } = -1;
      public string COMMENTS { get; set; } = "";

      public SincronizadorGP50ReceivedInvoiceModel(){}
      public SincronizadorGP50ReceivedInvoiceModel
      (
         SageReceivedInvoiceEnrichedModel sageEnrichedModel,
         ISage50ConnectionManager SageConnectionManager, 
         IGestprojectConnectionManager GestprojectConnectionManager
      )
      {
         PAR_DAO_ID = sageEnrichedModel.SageReceivedInvoiceCompany.PAR_ID;

         FCP_NUM_FACTURA = sageEnrichedModel.SageReceivedInvoice._Cabecera._Factura;
         FCP_FECHA = sageEnrichedModel.SageReceivedInvoice._Cabecera._FechaFac;

         PAR_PRO_ID = sageEnrichedModel.SageReceivedInvoiceProvider.PAR_ID;
         
         FCP_BASE_IMPONIBLE = sageEnrichedModel.SageReceivedInvoice._Pie._TotalBase;
         FCP_VALOR_IRPF = sageEnrichedModel.SageReceivedInvoice._Pie._RetencionDocPorcen;
         FCP_IRPF = sageEnrichedModel.SageReceivedInvoice._Pie._RetencionDoc;
         FCP_TOTAL_FACTURA = sageEnrichedModel.SageReceivedInvoice._Pie._TotalDocumento;
         FCP_OBSERVACIONES = sageEnrichedModel.SageReceivedInvoice._Cabecera._Observacio;

         FCP_SUBCTA_CONTABLE = sageEnrichedModel.SageReceivedInvoice._Lineas.FirstOrDefault()._Cuenta;
         FCP_VALOR_IVA = sageEnrichedModel.SageReceivedInvoiceBaseModelTaxModelList.FirstOrDefault()._PrcIva; 
			FCP_IVA = sageEnrichedModel.SageReceivedInvoiceBaseModelTaxModelList.Select(taxObject => taxObject._ImpIva).ToList().Sum();

         PRY_ID = sageEnrichedModel.SageReceivedInvoiceProject.PRY_ID;

			S50_CODE = sageEnrichedModel.BaseReceivedInvoiceModel.Number;
			S50_GUID_ID = sageEnrichedModel.BaseReceivedInvoiceModel.GuidId;

         S50_COMPANY_GROUP_NAME = SageConnectionManager.CompanyGroupData.CompanyName;
         S50_COMPANY_GROUP_CODE = SageConnectionManager.CompanyGroupData.CompanyCode;
         S50_COMPANY_GROUP_MAIN_CODE = SageConnectionManager.CompanyGroupData.CompanyMainCode;
         S50_COMPANY_GROUP_GUID_ID = SageConnectionManager.CompanyGroupData.CompanyGuidId;
         GP_USU_ID = GestprojectConnectionManager.GestprojectUserRememberableData.GP_USU_ID;
      }
   }
}
