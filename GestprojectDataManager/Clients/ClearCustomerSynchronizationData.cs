using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SincronizadorGPS50.GestprojectDataManager
{
   public class ClearCustomerSynchronizationData
   {
      public List<GestprojectCustomer> GestprojectClientList { get; set; } = new List<GestprojectCustomer>();
      public ClearCustomerSynchronizationData
      (
         GestprojectCustomer customer
      )
      {
         try
         {
            customer.synchronization_table_id = null;
            customer.synchronization_status = "Nunca ha sido sincronizado";
            customer.PAR_SUBCTA_CONTABLE = "";
            customer.sage50_client_code = "";
            customer.sage50_guid_id = "";
            customer.sage50_company_group_name = "";
            customer.sage50_company_group_code = "";
            customer.sage50_company_group_main_code = "";
            customer.sage50_company_group_guid_id = "";
            customer.comments = "";
            customer.parent_gesproject_user_id = null;
            customer.last_record = null;
         }
         catch(System.Exception exception)
         {
            throw new Exception($"At:\n\n{GetType().Namespace}\n.{GetType().Name}:\n\n{exception.Message}");
         };
      }
   }
}
