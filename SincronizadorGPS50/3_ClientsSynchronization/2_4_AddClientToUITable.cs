using System;
using System.Data;

namespace SincronizadorGPS50
{
   public class AddClientToUITable
   {
      public AddClientToUITable
      (
          GestprojectDataManager.GestprojectCustomer gestprojectClient,
          DataTable sincronizationTable
      )
      {
         try
         {
            DataRow row = sincronizationTable.NewRow();

            //row[0] = gestprojectClient.synchronization_table_id.ToString() ?? "";
            row[0] = gestprojectClient.synchronization_table_id.ToString();
            row[1] = gestprojectClient.synchronization_status;
            row[2] = gestprojectClient.PAR_ID.ToString() ?? "";

            row[3] = gestprojectClient.PAR_SUBCTA_CONTABLE;
            row[4] = gestprojectClient.fullName;
            row[5] = gestprojectClient.PAR_NOMBRE_COMERCIAL;
            row[6] = gestprojectClient.PAR_CIF_NIF;
            row[7] = gestprojectClient.PAR_DIRECCION_1;
            row[8] = gestprojectClient.PAR_CP_1;
            row[9] = gestprojectClient.PAR_LOCALIDAD_1;
            row[10] = gestprojectClient.PAR_PROVINCIA_1;
            row[11] = gestprojectClient.PAR_PAIS_1;
            row[12] = gestprojectClient.sage50_client_code;
            row[13] = gestprojectClient.sage50_guid_id;
            row[14] = gestprojectClient.sage50_company_group_name;
            row[15] = gestprojectClient.sage50_company_group_code;
            row[16] = gestprojectClient.sage50_company_group_main_code;
            row[17] = gestprojectClient.sage50_company_group_guid_id;

            row[18] = gestprojectClient.last_record.ToString() ?? "";
            row[19] = gestprojectClient.parent_gesproject_user_id.ToString() ?? "";

            int commentsLenght = gestprojectClient.comments.Length;
            row[20] = (commentsLenght > 1000 ? gestprojectClient.comments.Substring(0,999): gestprojectClient.comments) ?? "";            

            // 0. SynchronizationTableClientIdColumn
            // 1. SynchronizationStatusColumn
            // 2. GestprojectClientIdColumn

            // 3. GestprojectClientAccountableSubaccountColumn
            // 4. GestprojectClientNameColumn
            // 5. GestprojectClientCommercialNameColumn
            // 6. GestprojectClientCIFNIFColumn
            // 7. GestprojectClientAddressColumn
            // 8. GestprojectClientPostalCodeColumn
            // 9. GestprojectClientLocalityColumn
            // 10. GestprojectClientProvinceColumn
            // 11. GestprojectClientCountryColumn

            // 12. Sage50ClientCodeColumn
            // 13. Sage50ClientGuidIdColumn

            // 14. Sage50ClientCompanyGroupNameColumn
            // 15. Sage50ClientCompanyGroupCodeColumn
            // 16. Sage50ClientCompanyGroupMainCodeColumn
            // 17. Sage50ClientCompanyGroupGuidIdColumn
            // 18. ClientLastUpdateTerminalColumn
            // 19. CommentsColumn

            sincronizationTable.Rows.Add(row);
         }
         catch (System.Exception exception)
         {
            throw new Exception($"At:\n\n{GetType().Namespace}\n.{GetType().Name}:\n\n{exception.Message}");
         };
      }
   }
}
