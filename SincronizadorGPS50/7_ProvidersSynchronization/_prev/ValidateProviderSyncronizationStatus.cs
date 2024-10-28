using SincronizadorGPS50.Sage50Connector;
using System.Collections.Generic;
using System.Reflection;

namespace SincronizadorGPS50
{
   public class ValidateProviderSyncronizationStatus
   {
      public bool MustBeDeleted { get; set; } = false;
      public bool NeverWasSynchronized { get; set; } = false;
      public bool IsSynchronized { get; set; } = true;

      public string CreateErrorMesage(string nombreDeCampo, string valorEnSage50)
      {
         return $"\"{nombreDeCampo}\" no coincide. Su valor en Sage50 es: \"{valorEnSage50}\". ";
      }
      public ValidateProviderSyncronizationStatus
      (
         GestprojectProviderModel gestprojectEntity,
         List<Sage50ProviderModel> sage50ParticipantList,
         string entityNameColumnName,
         string entityCIFNIFColumnName,
         string entityPostalCodeColumnName,
         string entityAddressColumnName,
         string entityProvinceColumnName
      )
      {
         try
         {
            if(gestprojectEntity.S50_CODE != null && gestprojectEntity.S50_CODE != "")
            {
               for(int i = 0; i < sage50ParticipantList.Count; i++)
               {
                  if( sage50ParticipantList[i].GUID_ID.Trim() == gestprojectEntity.S50_GUID_ID.Trim() )
                  {
                     if(sage50ParticipantList[i].NOMBRE.Trim() != gestprojectEntity.NOMBRE_COMPLETO.Trim())
                     {
                        NeverWasSynchronized = false;
                        IsSynchronized = false;
                        MustBeDeleted = false;
                        gestprojectEntity.COMMENTS += this.CreateErrorMesage(entityNameColumnName, sage50ParticipantList[i].NOMBRE);
                     };

                     if(sage50ParticipantList[i].CIF.Trim() != gestprojectEntity.PAR_CIF_NIF.Trim())
                     {
                        NeverWasSynchronized = false;
                        IsSynchronized = false;
                        MustBeDeleted = false;
                        gestprojectEntity.COMMENTS += this.CreateErrorMesage(entityCIFNIFColumnName, sage50ParticipantList[i].CIF);
                     };

                     if(sage50ParticipantList[i].CODPOST.Trim() != gestprojectEntity.PAR_CP_1.Trim())
                     {
                        NeverWasSynchronized = false;
                        IsSynchronized = false;
                        MustBeDeleted = false;
                        gestprojectEntity.COMMENTS += this.CreateErrorMesage(entityPostalCodeColumnName, sage50ParticipantList[i].CODPOST);
                     };

                     if(sage50ParticipantList[i].DIRECCION.Trim() != gestprojectEntity.PAR_DIRECCION_1.Trim())
                     {
                        NeverWasSynchronized = false;
                        IsSynchronized = false;
                        MustBeDeleted = false;
                        gestprojectEntity.COMMENTS += this.CreateErrorMesage(entityAddressColumnName, sage50ParticipantList[i].DIRECCION);
                     };

                     if(sage50ParticipantList[i].PROVINCIA.Trim() != gestprojectEntity.PAR_PROVINCIA_1.Trim())
                     {
                        NeverWasSynchronized = false;
                        IsSynchronized = false;
                        MustBeDeleted = false;
                        gestprojectEntity.COMMENTS += this.CreateErrorMesage(entityProvinceColumnName, sage50ParticipantList[i].PROVINCIA);
                     };

                     // Don't check "country" because sage registers it as a number
                     // because the Provider was already compared with it's match in Sage, stop the comparison and record state

                     if
                     (
                        sage50ParticipantList[i].NOMBRE.Trim() == gestprojectEntity.NOMBRE_COMPLETO.Trim()
                        &&
                        sage50ParticipantList[i].CIF.Trim() == gestprojectEntity.PAR_CIF_NIF.Trim()
                        &&
                        sage50ParticipantList[i].CODPOST.Trim() == gestprojectEntity.PAR_CP_1.Trim()
                        &&
                        sage50ParticipantList[i].DIRECCION.Trim() == gestprojectEntity.PAR_DIRECCION_1.Trim()
                        &&
                        sage50ParticipantList[i].PROVINCIA.Trim() == gestprojectEntity.PAR_PROVINCIA_1.Trim()
                     )
                     {
                        //MessageBox.Show("Sincronizado");
                        NeverWasSynchronized = false;
                        IsSynchronized = true;
                        MustBeDeleted = false;
                        gestprojectEntity.COMMENTS = "";
                        gestprojectEntity.SYNC_STATUS = SynchronizationStatusOptions.Sincronizado;
                     };

                     break;
                  }
                  else
                  {
                     //MessageBox.Show("Eliminado en Sage");
                     NeverWasSynchronized = true;
                     MustBeDeleted = true;
                     gestprojectEntity.SYNC_STATUS = SynchronizationStatusOptions.Desincronizado;
                  };
               };
            }
            else
            {
               //MessageBox.Show("Nunca sincronizado");
               NeverWasSynchronized = true;
               IsSynchronized = false;
               MustBeDeleted = false;
               gestprojectEntity.SYNC_STATUS = SynchronizationStatusOptions.Desincronizado;
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
