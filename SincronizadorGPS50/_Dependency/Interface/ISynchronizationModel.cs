using System;

namespace SincronizadorGPS50
{
	public interface ISynchronizationModel
	{
		int? ID { get; set; }
		string SYNC_STATUS { get; set; }

		string S50_CODE { get; set; }
		string S50_GUID_ID { get; set; }
		string S50_COMPANY_GROUP_NAME { get; set; }
		string S50_COMPANY_GROUP_CODE { get; set; }
		string S50_COMPANY_GROUP_MAIN_CODE { get; set; }
		string S50_COMPANY_GROUP_GUID_ID { get; set; }

		DateTime? LAST_UPDATE { get; set; }

		int? GP_USU_ID { get; set; }
		string COMMENTS { get; set; }
	}
}