using Infragistics.Designers.SqlEditor;
using System;
using System.Data.SqlClient;
using System.Linq;
using static sage._50.CambioAnchuraCampos;

namespace SincronizadorGPS50
{
   public class SincronizadorGP50ReceivedInvoiceModel : ISynchronizationModel
   {
      // Gestproject fields
      public int? FCP_ID { get; set; } = -1;
      public int? PAR_DAO_ID { get; set; } = -1; // Obtener de la primera consulta al sql (1. Obtener numero de empresa. 2. Obtener nif de la empresa en "comunes.empresa", 3. Obtener id de esta empresa en la tabla de sincronización de empresas. 4. Colocar el id aquí.)
      public string FCP_NUM_FACTURA { get; set; } = ""; // doc._Cabecera._Factura
      public DateTime? FCP_FECHA { get; set; } = null; // doc._Cabecera._FechaFac
      public int? PAR_PRO_ID { get; set; } = -1; // doc._Cabecera._Proveedor -> Obtener PAR_ID de la tabla de sincronización de proveedores usando éste valor (subctacontable)
      public string FCP_SUBCTA_CONTABLE { get; set; } = ""; // doc._Lineas (de la primera línea) ewDocCompraLinFACTURA._Cuenta
      public decimal? FCP_BASE_IMPONIBLE { get; set; } = null; // doc._Pie._TotalBase
      public decimal? FCP_VALOR_IVA { get; set; } = null;  // De la consulta a la tabla c_factucom, en el campo IVA, sumar los valores del campo "_ImpIva" de todos los elementos
      public decimal? FCP_IVA { get; set; } = null;   // De la consulta a la tabla c_factucom, en el campo IVA, seleccionar el campo "_PrcIva" del primer objeto del array
      public decimal? FCP_VALOR_IRPF { get; set; } = null; // doc._Pie._RetencionDoc
      public decimal? FCP_IRPF { get; set; } = null; // doc._Pie._RetencionDocPorcen
      public decimal? FCP_TOTAL_FACTURA { get; set; } = null; // doc._Pie._TotalDocumento
      public string FCP_OBSERVACIONES { get; set; } = ""; // doc._Cabecera._Observacio
      public int? PRY_ID { get; set; } = -1; // doc._Cabecera._Obra
      public string FCP_EJERCICIO { get; set; } = DateTime.Now.Year.ToString();


      // Syncronization fields
      public int? ID { get; set; } = null;
      public string SYNC_STATUS { get; set; } = "";
      public string S50_CODE { get; set; } = "";
      public string S50_GUID_ID { get; set; } = ""; // c_factucom.GUID_ID
      public string S50_COMPANY_GROUP_NAME { get; set; } = "";
      public string S50_COMPANY_GROUP_CODE { get; set; } = "";
      public string S50_COMPANY_GROUP_MAIN_CODE { get; set; } = "";
      public string S50_COMPANY_GROUP_GUID_ID { get; set; } = "";
      public DateTime? LAST_UPDATE { get; set; } = null;
      public int? GP_USU_ID { get; set; } = null;
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
         //FCP_VALOR_IRPF = sageEnrichedModel.SageReceivedInvoice._Pie._RetencionDoc;
         FCP_IRPF = sageEnrichedModel.SageReceivedInvoice._Pie._RetencionDoc;
         //FCP_IRPF = sageEnrichedModel.SageReceivedInvoice._Pie._RetencionDocPorcen;
         FCP_TOTAL_FACTURA = sageEnrichedModel.SageReceivedInvoice._Pie._TotalDocumento;
         FCP_OBSERVACIONES = sageEnrichedModel.SageReceivedInvoice._Cabecera._Observacio;

         FCP_SUBCTA_CONTABLE = sageEnrichedModel.SageReceivedInvoice._Lineas.FirstOrDefault()._Cuenta;
         FCP_VALOR_IVA = sageEnrichedModel.SageReceivedInvoiceBaseModelTaxModelList.FirstOrDefault()._PrcIva; 
         //FCP_VALOR_IVA = sageEnrichedModel.SageReceivedInvoiceBaseModelTaxModelList.Select(taxObject => taxObject._ImpIva).ToList().Sum(); 
			FCP_IVA = sageEnrichedModel.SageReceivedInvoiceBaseModelTaxModelList.Select(taxObject => taxObject._ImpIva).ToList().Sum();
			//FCP_IVA = sageEnrichedModel.SageReceivedInvoiceBaseModelTaxModelList.FirstOrDefault()._PrcIva;

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
