using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   internal class UnexsistingProjectListWorkflow
   {
      public UnexsistingProjectListWorkflow
      (
         IGestprojectConnectionManager gestprojectConnectionManager,
         ISage50ConnectionManager sage50ConnectionManager,
         ISynchronizationTableSchemaProvider tableSchema,
         List<GestprojectProjectModel> entityList
      )
      {
         try
         {
            List<GestprojectProjectModel> existingEntityList = new List<GestprojectProjectModel> ();
            List<GestprojectProjectModel> unexistingEntityList = new List<GestprojectProjectModel> ();

            for(global::System.Int32 i = 0; i < entityList.Count; i++)
            {
               GestprojectProjectModel entity = entityList[i];

               ProjectComparer comparer = new ProjectComparer(
                  tableSchema,
                  entity.PRY_NOMBRE,
                  entity.PRY_CODIGO
               );

               if(comparer.EntityExists)
               {
                  entity.S50_CODE = comparer.Code;
                  entity.S50_GUID_ID = comparer.GuidId;
                  existingEntityList.Add(entity);
               }
               else
               {
                  unexistingEntityList.Add(entity);
               };
            };

            ConfirmationDialog<GestprojectProjectModel> confirmationDialog = new ConfirmationDialog<GestprojectProjectModel>("proyecto(s)", existingEntityList, unexistingEntityList);

            if(confirmationDialog.Result == DialogResult.OK)
            {
               for(global::System.Int32 i = 0; i < existingEntityList.Count; i++)
               {
                  GestprojectProjectModel entity = existingEntityList[i];

                  //new LinkProjectWorkflow(gestprojectConnectionManager, sage50ConnectionManager, tableSchema, entity);
               };
               for(global::System.Int32 i = 0; i < unexistingEntityList.Count; i++)
               {
                  GestprojectProjectModel entity = unexistingEntityList[i];
                  new CreateProjectWorkflow(gestprojectConnectionManager, sage50ConnectionManager, tableSchema, entity);
               };
            };
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
