using System;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
	public class GestprojectSubaccountableAccountModel : ISynchronizationModel
	{
		// Gestproject fields
		public int? COS_ID { get; set; }
		public string COS_CODIGO { get; set; }
		public string COS_NOMBRE { get; set; }
		public string COS_GRUPO  { get; set; }


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
