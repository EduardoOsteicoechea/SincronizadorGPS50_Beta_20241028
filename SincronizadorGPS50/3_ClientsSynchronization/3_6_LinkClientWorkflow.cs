using SincronizadorGPS50.GestprojectDataManager;
using System.Linq;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   internal class LinkClientWorkflow
   {
      public LinkClientWorkflow(System.Data.SqlClient.SqlConnection connection, GestprojectCustomer gestprojectClient,
         CustomerSyncronizationTableSchema tableSchema)
      {
         var sage50CompanyGroup = SincronizadorGPS50.Sage50Connector
         .Sage50CompanyGroupActions
         .GetCompanyGroups()
         .FirstOrDefault(companyGroup => companyGroup.CompanyName == Sage50ConnectionUIHolder.Sage50ConnectionUIManagerInstance.SelectCompanyGroupUI.SelectEnterpryseGroupMenu.Text);

         SynchronizerUserRememberableDataModel userRememberableData = ManageRememberableUserData.GetSynchronizerUserRememberableDataForConnection(
            GestprojectDataHolder.GestprojectDatabaseConnection
         );

         gestprojectClient.sage50_company_group_name = sage50CompanyGroup.CompanyName;
         gestprojectClient.sage50_company_group_code = sage50CompanyGroup.CompanyCode;
         gestprojectClient.sage50_company_group_main_code = sage50CompanyGroup.CompanyMainCode;
         gestprojectClient.sage50_company_group_guid_id = sage50CompanyGroup.CompanyGuidId;

         gestprojectClient.parent_gesproject_user_id = userRememberableData.GP_USU_ID;

         new GestprojectDataManager.RegisterNewSage50ClientData(
            connection,
            gestprojectClient.PAR_ID,
            gestprojectClient.sage50_client_code,
            gestprojectClient.sage50_guid_id,
            gestprojectClient.sage50_company_group_name,
            gestprojectClient.sage50_company_group_code,
            gestprojectClient.sage50_company_group_main_code,
            gestprojectClient.sage50_company_group_guid_id,
            gestprojectClient.parent_gesproject_user_id,
            "Desincronizado",
            tableSchema
         );
      }
   }
}
