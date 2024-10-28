using SincronizadorGPS50._Dependency.Interface;
using System.Collections.Generic;
using System.Reflection;

namespace SincronizadorGPS50
{
   internal class CreateProviderWorkflow : IEntityCreationWorkflow<GestprojectProviderModel>
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
            SincronizadorGPS50.CreateSage50Provider newSage50Entity = new SincronizadorGPS50.CreateSage50Provider(
               entity.PAR_PAIS_1,
               entity.PAR_NOMBRE ,
               entity.PAR_CIF_NIF,
               entity.PAR_CP_1,
               entity.PAR_DIRECCION_1,
               entity.PAR_PROVINCIA_1
            );

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
