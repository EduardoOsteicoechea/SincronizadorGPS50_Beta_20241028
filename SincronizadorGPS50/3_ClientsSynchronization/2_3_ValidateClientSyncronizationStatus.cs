using SincronizadorGPS50.GestprojectDataManager;
using SincronizadorGPS50.Sage50Connector;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   public class ValidateClientSyncronizationStatus
   {
      public bool MustBeDeleted { get; set; } = false;
      public bool NeverWasSynchronized { get; set; } = false;
      public bool IsSynchronized { get; set; } = true;

      public string CreateErrorMesage(string nombreDeCampo, string valorEnSage50)
      {
         return $"\"{nombreDeCampo}\" no coincide. Su valor en Sage50 es: \"{valorEnSage50}\". ";
      }
      public ValidateClientSyncronizationStatus
      (
         GestprojectCustomer gestprojectCustomer,
         List<Sage50Customer> sage50ClientList,
         GestprojectDataManager.CustomerSyncronizationTableSchema tableSchema
      )
      {
         try
         {
            if(gestprojectCustomer.sage50_guid_id != null && gestprojectCustomer.sage50_guid_id != "")
            {
               for(int i = 0; i < sage50ClientList.Count; i++)
               {
                  if
                  (
                     sage50ClientList[i].GUID_ID.Trim() == gestprojectCustomer.sage50_guid_id.Trim()
                  )
                  {
                     if(sage50ClientList[i].NOMBRE.Trim() != gestprojectCustomer.fullName.Trim())
                     {
                        NeverWasSynchronized = false;
                        IsSynchronized = false;
                        MustBeDeleted = false;
                        gestprojectCustomer.comments += this.CreateErrorMesage(tableSchema.GestprojectClientNameColumn.ColumnUserFriendlyNane, sage50ClientList[i].NOMBRE);
                     };

                     if(sage50ClientList[i].CIF.Trim() != gestprojectCustomer.PAR_CIF_NIF.Trim())
                     {
                        NeverWasSynchronized = false;
                        IsSynchronized = false;
                        MustBeDeleted = false;
                        gestprojectCustomer.comments += this.CreateErrorMesage(tableSchema.GestprojectClientCIFNIFColumn.ColumnUserFriendlyNane, sage50ClientList[i].CIF);
                     };

                     if(sage50ClientList[i].CODPOST.Trim() != gestprojectCustomer.PAR_CP_1.Trim())
                     {
                        NeverWasSynchronized = false;
                        IsSynchronized = false;
                        MustBeDeleted = false;
                        gestprojectCustomer.comments += this.CreateErrorMesage(tableSchema.GestprojectClientPostalCodeColumn.ColumnUserFriendlyNane, sage50ClientList[i].CODPOST);
                     };

                     if(sage50ClientList[i].DIRECCION.Trim() != gestprojectCustomer.PAR_DIRECCION_1.Trim())
                     {
                        NeverWasSynchronized = false;
                        IsSynchronized = false;
                        MustBeDeleted = false;
                        gestprojectCustomer.comments += this.CreateErrorMesage(tableSchema.GestprojectClientAddressColumn.ColumnUserFriendlyNane, sage50ClientList[i].DIRECCION);
                     };

                     if(sage50ClientList[i].PROVINCIA.Trim() != gestprojectCustomer.PAR_PROVINCIA_1.Trim())
                     {
                        NeverWasSynchronized = false;
                        IsSynchronized = false;
                        MustBeDeleted = false;
                        gestprojectCustomer.comments += this.CreateErrorMesage(tableSchema.GestprojectClientProvinceColumn.ColumnUserFriendlyNane, sage50ClientList[i].PROVINCIA);
                     };

                     // Don't check "country" because sage registers it as a number
                     // because the client was already compared with it's match in Sage, stop the comparison and record state

                     if
                     (
                        sage50ClientList[i].NOMBRE.Trim() == gestprojectCustomer.fullName.Trim()
                        &&
                        sage50ClientList[i].CIF.Trim() == gestprojectCustomer.PAR_CIF_NIF.Trim()
                        &&
                        sage50ClientList[i].CODPOST.Trim() == gestprojectCustomer.PAR_CP_1.Trim()
                        &&
                        sage50ClientList[i].DIRECCION.Trim() == gestprojectCustomer.PAR_DIRECCION_1.Trim()
                        &&
                        sage50ClientList[i].PROVINCIA.Trim() == gestprojectCustomer.PAR_PROVINCIA_1.Trim()
                     )
                     {
                        //MessageBox.Show("Sincronizado");
                        NeverWasSynchronized = false;
                        IsSynchronized = true;
                        MustBeDeleted = false;
                        gestprojectCustomer.comments = "";
                        gestprojectCustomer.synchronization_status = "Sincronizado";

                     };

                     break;
                  }
                  else
                  {
                     //MessageBox.Show("Eliminado en Sage");
                     NeverWasSynchronized = true;
                     MustBeDeleted = true;
                  };
               };
            }
            else
            {
               //MessageBox.Show("Nunca sincronizado");
               NeverWasSynchronized = true;
               IsSynchronized = false;
               MustBeDeleted = false;
            };
         }
         catch(System.Exception exception)
         {
            throw new Exception($"At:\n\n{GetType().Namespace}\n.{GetType().Name}:\n\n{exception.Message}");
         };
      }
   }
}
