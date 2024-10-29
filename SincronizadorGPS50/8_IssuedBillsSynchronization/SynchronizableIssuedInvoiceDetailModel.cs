using System;

namespace SincronizadorGPS50
{
   public class SynchronizableIssuedInvoiceDetailModel
   {
      public int? DFE_ID { get; set; } = -1;
      public string DFE_CONCEPTO { get; set; } = "";
      public decimal? DFE_PRECIO_UNIDAD { get; set; } = 0;
      public decimal? DFE_UNIDADES { get; set; } = 0;
      public decimal? DFE_SUBTOTAL { get; set; } = 0;
      public int? PRY_ID { get; set; } = -1;
      public int? FCE_ID { get; set; } = -1;
      public decimal? DFE_SUBTOTAL_BASE { get; set; } = 0;      

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

   }
}
