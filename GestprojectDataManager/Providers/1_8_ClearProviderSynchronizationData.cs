//using System;
//using System.Collections.Generic;
//using System.Data.SqlClient;
//using System.Windows.Forms;

//namespace SincronizadorGPS50.GestprojectDataManager
//{
//   public class ClearProviderSynchronizationData
//   {
//      public List<GestprojectProviderModel> GestprojectClientList { get; set; } = new List<GestprojectProviderModel>();
//      public ClearProviderSynchronizationData
//      (
//         GestprojectProviderModel Provider
//      )
//      {
//         try
//         {
//            Provider.synchronization_table_id = null;
//            Provider.synchronization_status = "Desincronizado";
//            Provider.PAR_SUBCTA_CONTABLE_2 = "";
//            Provider.sage50_code = "";
//            Provider.sage50_guid_id = "";
//            Provider.sage50_company_group_name = "";
//            Provider.sage50_company_group_code = "";
//            Provider.sage50_company_group_main_code = "";
//            Provider.sage50_company_group_guid_id = "";
//            Provider.comments = "";
//            Provider.parent_gesproject_user_id = null;
//            Provider.last_record = null;
//         }
//         catch(System.Exception exception)
//         {
//            throw new Exception($"At:\n\n{GetType().Namespace}\n.{GetType().Name}:\n\n{exception.Message}");
//         };
//      }
//   }
//}
