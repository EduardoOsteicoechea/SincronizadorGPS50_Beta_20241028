using sage.ew.docscompra;
using System;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   public class SincronizadorGPS50ReceivedInvoiceDetailModel
   {   
      public int? DFP_ID { get; set; } = -1;
      public string DFP_CONCEPTO { get; set; } = "";
      public decimal? DFP_PRECIO_UNIDAD { get; set; } = 0;
      public decimal? DFP_UNIDADES { get; set; } = 0;
      public decimal? DFP_SUBTOTAL { get; set; } = 0;
      public int? PRY_ID { get; set; } = -1;
      public int? FCP_ID { get; set; } = -1;
      public string DFP_ESTRUCTURAL { get; set; } = "0";
      public string INVOICE_GUID_ID { get; set; } = "";

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


      public SincronizadorGPS50ReceivedInvoiceDetailModel(){}
      public SincronizadorGPS50ReceivedInvoiceDetailModel
      (
         SageReceivedInvoiceEnrichedModel sageEnrichedModel, 
         ewDocCompraLinFACTURA receivedInvoiceLine,
         ISage50ConnectionManager SageConnectionManager, 
         IGestprojectConnectionManager GestprojectConnectionManager
      )
      {
         DFP_CONCEPTO = receivedInvoiceLine._Definicion;
         DFP_PRECIO_UNIDAD = receivedInvoiceLine._Precio;
         DFP_UNIDADES = receivedInvoiceLine._Unidades;
         DFP_SUBTOTAL = receivedInvoiceLine._Importe;
         PRY_ID = sageEnrichedModel.SageReceivedInvoiceProject.PRY_ID;
         DFP_ESTRUCTURAL = sageEnrichedModel.SageReceivedInvoice._Cabecera._Obra.Trim() == "" ? "1" : "0";
         INVOICE_GUID_ID = sageEnrichedModel.BaseReceivedInvoiceModel.GuidId;

         S50_CODE = receivedInvoiceLine._Numero;

         S50_COMPANY_GROUP_NAME = SageConnectionManager.CompanyGroupData.CompanyName;
         S50_COMPANY_GROUP_CODE = SageConnectionManager.CompanyGroupData.CompanyCode;
         S50_COMPANY_GROUP_MAIN_CODE = SageConnectionManager.CompanyGroupData.CompanyMainCode;
         S50_COMPANY_GROUP_GUID_ID = SageConnectionManager.CompanyGroupData.CompanyGuidId;
         GP_USU_ID = GestprojectConnectionManager.GestprojectUserRememberableData.GP_USU_ID;
      }
   }
}                     
