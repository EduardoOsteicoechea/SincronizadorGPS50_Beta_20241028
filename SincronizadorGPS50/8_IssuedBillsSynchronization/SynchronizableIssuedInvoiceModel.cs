using System;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;

namespace SincronizadorGPS50
{
   public class SynchronizableIssuedInvoiceModel : ISynchronizationModel
   {
      // Gestproject fields
      public int? FCE_ID { get; set; } = -1;
      public int? PAR_DAO_ID { get; set; } = -1;
      public string FCE_REFERENCIA { get; set; } = "";
      public int? FCE_NUM_FACTURA { get; set; } = -1;
      public DateTime? FCE_FECHA { get; set; } = null;
      public int? PAR_CLI_ID { get; set; } = -1;
      public decimal? FCE_BASE_IMPONIBLE { get; set; } = 0;
      public decimal? FCE_VALOR_IVA { get; set; } = 0;
      public decimal? FCE_IVA { get; set; } = 0;
      public decimal? FCE_VALOR_IRPF { get; set; } = 0;
      public decimal? FCE_IRPF { get; set; } = 0;
      public decimal? FCE_TOTAL_SUPLIDO { get; set; } = 0;
      public decimal? FCE_TOTAL_FACTURA { get; set; } = 0;
      public string FCE_OBSERVACIONES { get; set; } = "";
      public string PROYECTO { get; set; } = "";
      public string TIPO { get; set; } = "";
      public string FCE_IVA_IGIC { get; set; } = "";
      public string PAR_SUBCTA_CONTABLE { get; set; } = "";
      public string SageCompanyNumber { get; set; } = "";
      public string TaxCode { get; set; } = "";
      public string FCE_SUBCTA_CONTABLE { get; set; } = "";
      public string SageProjectCode { get; set; } = "";

      

      // Syncronization fields
      public int? ID { get; set; } = null;
      public string SYNC_STATUS { get; set; } = "";
      public string S50_CODE { get; set; } = "";
      public string S50_GUID_ID { get; set; } = "";
      public string S50_COMPANY_GROUP_NAME { get; set; } = "";
      public string S50_COMPANY_GROUP_CODE { get; set; } = "";
      public string S50_COMPANY_GROUP_MAIN_CODE { get; set; } = "";
      public string S50_COMPANY_GROUP_GUID_ID { get; set; } = "";
      public DateTime? LAST_UPDATE { get; set; } = null;
      public int? GP_USU_ID { get; set; } = null;
      public string COMMENTS { get; set; } = "";
   }
}
