//using SincronizadorGPS50.GestprojectDataManager;
//using SincronizadorGPS50.Sage50Connector;
//using System;
//using System.Collections.Generic;
//using System.Windows.Forms;

//namespace SincronizadorGPS50
//{
//   public class ValidateProviderSyncronizationStatus
//   {
//      public bool MustBeDeleted { get; set; } = false;
//      public bool NeverWasSynchronized { get; set; } = false;
//      public bool IsSynchronized { get; set; } = true;

//      public string CreateErrorMesage(string nombreDeCampo, string valorEnSage50)
//      {
//         return $"\"{nombreDeCampo}\" no coincide. Su valor en Sage50 es: \"{valorEnSage50}\". ";
//      }
//      public ValidateProviderSyncronizationStatus
//      (
//         GestprojectProviderModel gestprojectParticipant,
//         List<Sage50ProviderModel> sage50ParticipantList,
//         string entityNameColumnName,
//         string entityCIFNIFColumnName,
//         string entityPostalCodeColumnName,
//         string entityAddressColumnName,
//         string entityProvinceColumnName
//      )
//      {
//         try
//         {
//            //if(gestprojectParticipant.sage50_guid_id != null && gestprojectParticipant.sage50_guid_id != "")
//            //{
//            //   for(int i = 0; i < sage50ParticipantList.Count; i++)
//            //   {
//            //      if
//            //      (
//            //         sage50ParticipantList[i].GUID_ID.Trim() == gestprojectParticipant.sage50_guid_id.Trim()
//            //      )
//            //      {
//            //         if(sage50ParticipantList[i].NOMBRE.Trim() != gestprojectParticipant.fullName.Trim())
//            //         {
//            //            NeverWasSynchronized = false;
//            //            IsSynchronized = false;
//            //            MustBeDeleted = false;
//            //            gestprojectParticipant.comments += this.CreateErrorMesage(entityNameColumnName, sage50ParticipantList[i].NOMBRE);
//            //         };

//            //         if(sage50ParticipantList[i].CIF.Trim() != gestprojectParticipant.PAR_CIF_NIF.Trim())
//            //         {
//            //            NeverWasSynchronized = false;
//            //            IsSynchronized = false;
//            //            MustBeDeleted = false;
//            //            gestprojectParticipant.comments += this.CreateErrorMesage(entityCIFNIFColumnName, sage50ParticipantList[i].CIF);
//            //         };

//            //         if(sage50ParticipantList[i].CODPOST.Trim() != gestprojectParticipant.PAR_CP_1.Trim())
//            //         {
//            //            NeverWasSynchronized = false;
//            //            IsSynchronized = false;
//            //            MustBeDeleted = false;
//            //            gestprojectParticipant.comments += this.CreateErrorMesage(entityPostalCodeColumnName, sage50ParticipantList[i].CODPOST);
//            //         };

//            //         if(sage50ParticipantList[i].DIRECCION.Trim() != gestprojectParticipant.PAR_DIRECCION_1.Trim())
//            //         {
//            //            NeverWasSynchronized = false;
//            //            IsSynchronized = false;
//            //            MustBeDeleted = false;
//            //            gestprojectParticipant.comments += this.CreateErrorMesage(entityAddressColumnName, sage50ParticipantList[i].DIRECCION);
//            //         };

//            //         if(sage50ParticipantList[i].PROVINCIA.Trim() != gestprojectParticipant.PAR_PROVINCIA_1.Trim())
//            //         {
//            //            NeverWasSynchronized = false;
//            //            IsSynchronized = false;
//            //            MustBeDeleted = false;
//            //            gestprojectParticipant.comments += this.CreateErrorMesage(entityProvinceColumnName, sage50ParticipantList[i].PROVINCIA);
//            //         };

//            //         // Don't check "country" because sage registers it as a number
//            //         // because the Provider was already compared with it's match in Sage, stop the comparison and record state

//            //         if
//            //         (
//            //            sage50ParticipantList[i].NOMBRE.Trim() == gestprojectParticipant.fullName.Trim()
//            //            &&
//            //            sage50ParticipantList[i].CIF.Trim() == gestprojectParticipant.PAR_CIF_NIF.Trim()
//            //            &&
//            //            sage50ParticipantList[i].CODPOST.Trim() == gestprojectParticipant.PAR_CP_1.Trim()
//            //            &&
//            //            sage50ParticipantList[i].DIRECCION.Trim() == gestprojectParticipant.PAR_DIRECCION_1.Trim()
//            //            &&
//            //            sage50ParticipantList[i].PROVINCIA.Trim() == gestprojectParticipant.PAR_PROVINCIA_1.Trim()
//            //         )
//            //         {
//            //            //MessageBox.Show("Sincronizado");
//            //            NeverWasSynchronized = false;
//            //            IsSynchronized = true;
//            //            MustBeDeleted = false;
//            //            gestprojectParticipant.comments = "";
//            //            gestprojectParticipant.synchronization_status = "Sincronizado";
//            //         };

//            //         break;
//            //      }
//            //      else
//            //      {
//            //         //MessageBox.Show("Eliminado en Sage");
//            //         NeverWasSynchronized = true;
//            //         MustBeDeleted = true;
//            //         gestprojectParticipant.synchronization_status = "Desincronizado";
//            //      };
//            //   };
//            //}
//            //else
//            //{
//            //   //MessageBox.Show("Nunca sincronizado");
//            //   NeverWasSynchronized = true;
//            //   IsSynchronized = false;
//            //   MustBeDeleted = false;
//            //   gestprojectParticipant.synchronization_status = "Desincronizado";
//            //};
//         }
//         catch(System.Exception exception)
//         {
//            throw new Exception($"At:\n\n{GetType().Namespace}\n.{GetType().Name}:\n\n{exception.Message}");
//         };
//      }
//   }
//}
