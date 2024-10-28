using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   public class ValidateSubaccountableAccountSyncronizationStatus
   {
      public bool MustBeDeleted { get; set; } = false;
      public bool NeverWasSynchronized { get; set; } = false;
      public bool IsSynchronized { get; set; } = true;

      public string CreateErrorMesage(string nombreDeCampo, string valorEnSage50)
      {
         return $"\"{nombreDeCampo}\" no coincide. Su valor en Sage50 es: \"{valorEnSage50}\". ";
      }
      public ValidateSubaccountableAccountSyncronizationStatus
      (
         GestprojectSubaccountableAccountModel gestprojectEntity,
         List<Sage50SubaccountableAccountModel> sage50EntityList,
         string code,
         string name,
         string group,
         bool neverWasSynchronized
      )
      {
         try
         {
            bool existsInGestproject = gestprojectEntity.COS_ID != -1;
            bool hasSage50GuidId = gestprojectEntity.S50_CODE != "";
            bool hasSage50CompanyGroupGuidId = gestprojectEntity.S50_COMPANY_GROUP_GUID_ID != "";

            if( !existsInGestproject && hasSage50GuidId && hasSage50CompanyGroupGuidId )
            {
               NeverWasSynchronized = true;
               IsSynchronized = false;
               MustBeDeleted = true;
               gestprojectEntity.SYNC_STATUS = SynchronizationStatusOptions.Desincronizado;   
            }
            else
            {
               for(int i = 0; i < sage50EntityList.Count; i++)
               {
                  if(sage50EntityList[i].GUID_ID.Trim() == gestprojectEntity.S50_GUID_ID.Trim())
                  {
                     if(sage50EntityList[i].NOMBRE.Trim() != gestprojectEntity.COS_NOMBRE.Trim())
                     {
                        NeverWasSynchronized = false;
                        IsSynchronized = false;
                        MustBeDeleted = false;
                        gestprojectEntity.COMMENTS += this.CreateErrorMesage(name, sage50EntityList[i].NOMBRE);
                     };

                     if(sage50EntityList[i].CODIGO !=gestprojectEntity.COS_CODIGO.Trim())
                     {
                        NeverWasSynchronized = false;
                        IsSynchronized = false;
                        MustBeDeleted = false;

                        gestprojectEntity.COMMENTS += this.CreateErrorMesage(code, (sage50EntityList[i].CODIGO ?? "").ToString());
                     };

                     if
                     (
                        sage50EntityList[i].NOMBRE.Trim() == gestprojectEntity.COS_NOMBRE.Trim()
                        &&
                        sage50EntityList[i].CODIGO.Trim() == gestprojectEntity.COS_CODIGO.Trim()
                        //&&
                        //hasSage50GuidId
                     )
                     {
                        //MessageBox.Show("Sincronizado");
                        NeverWasSynchronized = false;
                        IsSynchronized = true;
                        MustBeDeleted = false;
                        gestprojectEntity.COMMENTS = "";
                        gestprojectEntity.SYNC_STATUS = SynchronizationStatusOptions.Sincronizado;
                        break;
                     };
                  }
                  else
                  {
                     //MessageBox.Show("Eliminado en Sage");
                     NeverWasSynchronized = true;
                     IsSynchronized = false;
                     MustBeDeleted = false;
                     gestprojectEntity.SYNC_STATUS = SynchronizationStatusOptions.Desincronizado;
                  };
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