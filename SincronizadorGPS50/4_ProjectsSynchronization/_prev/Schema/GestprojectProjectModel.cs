using System;

namespace SincronizadorGPS50
{
	public class GestprojectProjectModel : ISynchronizationModel
	{
		// Gestproject fields
		public int? PRY_ID { get; set; } = -1;
		public string PRY_CODIGO { get; set; } = "";
		public string PRY_NOMBRE { get; set; } = "";
		public string PRY_DIRECCION { get; set; } = "";
		public string PRY_LOCALIDAD { get; set; } = "";
		public string PRY_PROVINCIA { get; set; } = "";
		public string PRY_CP { get; set; } = "";

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
