using SincronizadorGPS50.GestprojectDataManager;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   internal class UpdateProviderWorkflow
   {
      public UpdateProviderWorkflow(System.Data.SqlClient.SqlConnection connection, GestprojectCustomer gestprojectClient,
         CustomerSyncronizationTableSchema tableSchema, string sage50ClientCode = "", string sage50guid = "", int? parentUserId = null)
      {
         try
         {
            //new SincronizadorGPS50.Sage50Connector.UpdateSage50Customer(
            //   gestprojectClient.sage50_guid_id,
            //   gestprojectClient.PAR_PAIS_1,
            //   gestprojectClient.PAR_NOMBRE,
            //   gestprojectClient.PAR_CIF_NIF,
            //   gestprojectClient.PAR_CP_1,
            //   gestprojectClient.PAR_DIRECCION_1,
            //   gestprojectClient.PAR_PROVINCIA_1
            //);

            //new GestprojectDataManager.RegisterNewSage50ClientData(
            //   connection,
            //   gestprojectClient.PAR_ID,
            //   gestprojectClient.sage50_client_code,
            //   gestprojectClient.sage50_guid_id,
            //   sage50CompanyGroup.CompanyName,
            //   sage50CompanyGroup.CompanyMainCode,
            //   sage50CompanyGroup.CompanyCode,
            //   sage50CompanyGroup.CompanyGuidId,
            //   userRememberableData.GP_USU_ID,
            //   "Sincronizado",
            //   tableSchema
            //);
         }
         catch(System.Exception exception)
         {
            throw ApplicationLogger.ReportError(
               MethodBase.GetCurrentMethod().DeclaringType.Namespace,
               MethodBase.GetCurrentMethod().DeclaringType.Name,
               MethodBase.GetCurrentMethod().Name,
               exception
            );
         };
      }
   }
}
