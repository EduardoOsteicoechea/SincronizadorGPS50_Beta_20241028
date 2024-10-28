using SincronizadorGPS50.Sage50Connector;
using System.Collections.Generic;
using System.Reflection;

namespace SincronizadorGPS50
{
   internal class CreateProjectWorkflow
   {
      public CreateProjectWorkflow
      (
         IGestprojectConnectionManager GestprojectConnectionManager,
         ISage50ConnectionManager Sage50ConnectionManager,
         ISynchronizationTableSchemaProvider tableSchema,
         GestprojectProjectModel entity
      )
      {
         try
         {
            //new VisualizePropertiesAndValues<GestprojectProjectModel>("SincronizadorGPS50.CreateProjectWorkflow", entity);

            CreateSage50Project newSage50Entity = new CreateSage50Project(
               entity.PRY_NOMBRE ,
               entity.PRY_DIRECCION,
               entity.PRY_CP,
               entity.PRY_LOCALIDAD,
               entity.PRY_PROVINCIA,
               tableSchema
            );

            //new RegisterNewSage50EntityData(
            //   GestprojectConnectionManager.GestprojectSqlConnection,
            //   tableSchema.TableName,
            //   new List<(string, dynamic)>()
            //   {
            //      (tableSchema.SynchronizationStatus.ColumnDatabaseName, SynchronizationStatusOptions.Desincronizado),
            //      (tableSchema.Sage50Code.ColumnDatabaseName, entity.S50_CODE),
            //      (tableSchema.Sage50GuidId.ColumnDatabaseName, entity.S50_GUID_ID),
            //      (tableSchema.CompanyGroupName.ColumnDatabaseName, entity.S50_COMPANY_GROUP_NAME),
            //      (tableSchema.CompanyGroupCode.ColumnDatabaseName, entity.S50_COMPANY_GROUP_CODE),
            //      (tableSchema.CompanyGroupMainCode.ColumnDatabaseName, entity.S50_COMPANY_GROUP_MAIN_CODE),
            //      (tableSchema.CompanyGroupGuidId.ColumnDatabaseName, entity.S50_COMPANY_GROUP_GUID_ID),
            //      (tableSchema.ParentUserId.ColumnDatabaseName, entity.GP_USU_ID)
            //   },
            //   (tableSchema.GestprojectId.ColumnDatabaseName, entity.PRY_ID),
            //   "PROYECTO",
            //   (tableSchema.AccountableSubaccount.ColumnDatabaseName, entity.S50_CODE),
            //   (tableSchema.GestprojectId.ColumnDatabaseName, entity.PRY_ID)
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
