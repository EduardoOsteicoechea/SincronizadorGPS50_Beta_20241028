using System;

namespace SincronizadorGPS50
{
   public class SynchronizableCompanyModel
   {
      public int? PAR_ID { get; set; } = -1;
      public string PAR_NOMBRE { get; set; } = "";
      public string PAR_CIF_NIF { get; set; } = "";
      public string SageCompanyNumber { get; set; } = "";

      // Syncronization fields
      public int? ID { get; set; } = -1;
      public string SYNC_STATUS { get; set; } = "";
      public string S50_CODE { get; set; } = "";
      public string S50_GUID_ID { get; set; } = ""; // c_factucom.GUID_ID
      public string S50_COMPANY_GROUP_NAME { get; set; } = "";
      public string S50_COMPANY_GROUP_CODE { get; set; } = "";
      public string S50_COMPANY_GROUP_MAIN_CODE { get; set; } = "";
      public string S50_COMPANY_GROUP_GUID_ID { get; set; } = "";
      public DateTime? LAST_UPDATE { get; set; } = DateTime.Now;
      public int? GP_USU_ID { get; set; } = -1;
      public string COMMENTS { get; set; } = "";
   }
}
