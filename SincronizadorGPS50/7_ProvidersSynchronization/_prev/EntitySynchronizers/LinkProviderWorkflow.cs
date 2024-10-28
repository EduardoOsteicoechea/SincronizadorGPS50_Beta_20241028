using SincronizadorGPS50.GestprojectDataManager;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   internal class LinkProviderWorkflow : IEntityLinkWorkflow<GestprojectProviderModel>
   {
      public void Execute
      (
         IGestprojectConnectionManager GestprojectConnectionManager,
         ISage50ConnectionManager Sage50ConnectionManager,
         GestprojectProviderModel entity,
         ISynchronizationTableSchemaProvider tableSchemaProvider
      )
      {
         try
         {
            entity.S50_COMPANY_GROUP_NAME = Sage50ConnectionManager.CompanyGroupData.CompanyName;
            entity.S50_COMPANY_GROUP_CODE = Sage50ConnectionManager.CompanyGroupData.CompanyCode;
            entity.S50_COMPANY_GROUP_MAIN_CODE = Sage50ConnectionManager.CompanyGroupData.CompanyMainCode;
            entity.S50_COMPANY_GROUP_GUID_ID = Sage50ConnectionManager.CompanyGroupData.CompanyGuidId;
            entity.GP_USU_ID = GestprojectConnectionManager.GestprojectUserRememberableData.GP_USU_ID;
           
            new RegisterNewSage50EntityData(
               GestprojectConnectionManager.GestprojectSqlConnection,
               tableSchemaProvider.TableName,
               new List<(string, dynamic)>(){
                     (tableSchemaProvider.SynchronizationStatus.ColumnDatabaseName, SynchronizationStatusOptions.Desincronizado),
                     (tableSchemaProvider.Sage50Code.ColumnDatabaseName, entity.S50_CODE),
                     (tableSchemaProvider.Sage50GuidId.ColumnDatabaseName, entity.S50_GUID_ID),
                     (tableSchemaProvider.CompanyGroupName.ColumnDatabaseName, entity.S50_COMPANY_GROUP_NAME),
                     (tableSchemaProvider.CompanyGroupCode.ColumnDatabaseName, entity.S50_COMPANY_GROUP_CODE),
                     (tableSchemaProvider.CompanyGroupMainCode.ColumnDatabaseName, entity.S50_COMPANY_GROUP_MAIN_CODE),
                     (tableSchemaProvider.CompanyGroupGuidId.ColumnDatabaseName, entity.S50_COMPANY_GROUP_GUID_ID),
                     (tableSchemaProvider.ParentUserId.ColumnDatabaseName, entity.GP_USU_ID)
               },
               (tableSchemaProvider.GestprojectId.ColumnDatabaseName, entity.PAR_ID),
               "PARTICIPANTE",
               (tableSchemaProvider.AccountableSubaccount.ColumnDatabaseName, entity.S50_CODE),
               (tableSchemaProvider.GestprojectId.ColumnDatabaseName, entity.PAR_ID)
            );            
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
