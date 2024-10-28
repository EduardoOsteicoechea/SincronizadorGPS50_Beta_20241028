using System.Collections.Generic;
using System.Reflection;

namespace SincronizadorGPS50
{
   public class ValidateProjectSyncronizationStatus
   {
      public bool MustBeDeleted { get; set; } = false;
      public bool NeverWasSynchronized { get; set; } = false;
      public bool IsSynchronized { get; set; } = true;

      public string CreateErrorMesage(string nombreDeCampo, string valorEnSage50)
      {
         return $"\"{nombreDeCampo}\" no coincide. Su valor en Sage50 es: \"{valorEnSage50}\". ";
      }
      public ValidateProjectSyncronizationStatus
      (
         GestprojectProjectModel gestprojectEntity,
         List<Sage50ProjectModel> sage50EntityList,
         string entityNameColumnName,
         string entityPostalCodeColumnName,
         string entityAddressColumnName,
         string entityLocalityColumnName,
         string entityProvinceColumnName
      )
      {
         try
         {
            if(gestprojectEntity.S50_CODE != null && gestprojectEntity.S50_CODE != "")
            {
               for(int i = 0; i < sage50EntityList.Count; i++)
               {
                  if(sage50EntityList[i].GUID_ID.Trim() == gestprojectEntity.S50_GUID_ID.Trim())
                  {
                     if(sage50EntityList[i].NOMBRE.Trim() != gestprojectEntity.PRY_NOMBRE.Trim())
                     {
                        NeverWasSynchronized = false;
                        IsSynchronized = false;
                        MustBeDeleted = false;
                        gestprojectEntity.COMMENTS += this.CreateErrorMesage(entityNameColumnName, sage50EntityList[i].NOMBRE);
                     };

                     if(sage50EntityList[i].CODPOST.Trim() != gestprojectEntity.PRY_CP.Trim())
                     {
                        NeverWasSynchronized = false;
                        IsSynchronized = false;
                        MustBeDeleted = false;
                        gestprojectEntity.COMMENTS += this.CreateErrorMesage(entityPostalCodeColumnName, sage50EntityList[i].CODPOST.Trim());
                     };

                     if(sage50EntityList[i].DIRECCION.Trim() != gestprojectEntity.PRY_DIRECCION.Trim())
                     {
                        NeverWasSynchronized = false;
                        IsSynchronized = false;
                        MustBeDeleted = false;
                        gestprojectEntity.COMMENTS += this.CreateErrorMesage(entityAddressColumnName, sage50EntityList[i].DIRECCION);
                     };

                     if(sage50EntityList[i].POBLACION.Trim() != gestprojectEntity.PRY_LOCALIDAD.Trim())
                     {
                        NeverWasSynchronized = false;
                        IsSynchronized = false;
                        MustBeDeleted = false;
                        gestprojectEntity.COMMENTS += this.CreateErrorMesage(entityLocalityColumnName, sage50EntityList[i].POBLACION);
                     };

                     if(sage50EntityList[i].PROVINCIA.Trim() != gestprojectEntity.PRY_PROVINCIA.Trim())
                     {
                        NeverWasSynchronized = false;
                        IsSynchronized = false;
                        MustBeDeleted = false;
                        gestprojectEntity.COMMENTS += this.CreateErrorMesage(entityProvinceColumnName, sage50EntityList[i].PROVINCIA);
                     };

                     if
                     (
                        sage50EntityList[i].NOMBRE.Trim() == gestprojectEntity.PRY_NOMBRE.Trim()
                        &&
                        sage50EntityList[i].DIRECCION.Trim() == gestprojectEntity.PRY_DIRECCION.Trim()
                        &&
                        sage50EntityList[i].CODPOST.Trim() == gestprojectEntity.PRY_CP.Trim()
                        &&
                        sage50EntityList[i].POBLACION.Trim() == gestprojectEntity.PRY_LOCALIDAD.Trim()
                        &&
                        sage50EntityList[i].PROVINCIA.Trim() == gestprojectEntity.PRY_PROVINCIA.Trim()
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
