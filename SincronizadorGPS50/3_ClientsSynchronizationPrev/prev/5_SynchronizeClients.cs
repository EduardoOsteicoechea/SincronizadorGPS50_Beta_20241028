using SincronizadorGPS50.GestprojectAPI;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SincronizadorGPS50.Workflows.Clients
{
    internal class SynchronizeClients
    {
        public SynchronizeClients(List<GestprojectClient> selectedGestprojectClients)
        {
            DialogResult userAuthorizationToUpdateInSage50 = MessageBox.Show("¿Autoriza a Gestproject para actualizar datos en Sage50?", "Solicitud de Credenciales", MessageBoxButtons.YesNo);

            if(userAuthorizationToUpdateInSage50 == DialogResult.Yes)
            {
                GetSage50Clients sage50Clients = new GetSage50Clients();

                for(int i = 0; i < selectedGestprojectClients.Count; i++)
                {
                    GestprojectClient registeredClient = selectedGestprojectClients[i];

                    //new PopulateGestprojectClientSynchronizationData(registeredClient);

                    bool clientAlreadyExists = new CheckIfGestprojectClientWasSynchronized(registeredClient).ItIs;

                    if(clientAlreadyExists)
                    {
                        if(!new EndPointsData(registeredClient).Matches)
                        {
                        
                            //new UpdateSage50Customer(registeredClient);

                            //new PopulateGestprojectClientSynchronizationData(registeredClient);

                            new RecordSage50ClientCodeInGestproject(
                                registeredClient.PAR_ID,
                                registeredClient.sage50_client_code
                            );

                            if(!new EndPointsData(registeredClient).Matches)
                            {
                                new UpdateClientSynchronizationStatus(
                                    registeredClient,
                                    SynchronizationStatusOptions.Desincronizado
                                );
                            };

                            new UpdateClient(
                                registeredClient,
                                registeredClient.synchronization_status
                            );
                        };
                    } 
                    else 
                    {
                        //CreateSage50Customer newSage50Customer = new CreateSage50Customer(
                        //    registeredClient,
                        //    Convert.ToInt32(sage50Clients.NextClientCodeAvailable) + i
                        //);

                        //if(newSage50Customer.WasSuccessful)
                        //{
                        //    new UpdateRegisteredClientModelData(
                        //        registeredClient,
                        //        newSage50Customer.ClientCode,
                        //        newSage50Customer.GUID_ID
                        //    );

                        //    new RecordSage50ClientCodeInGestproject(
                        //        registeredClient.PAR_ID,
                        //        newSage50Customer.ClientCode
                        //    );

                        //    if(!new EndPointsData(registeredClient).Matches)
                        //    {
                        //        new UpdateClientSynchronizationStatus(
                        //            registeredClient,
                        //            SynchronizationStatusOptions.Desincronizado
                        //        );
                        //    };

                        //    new UpdateClient(
                        //        registeredClient,
                        //        registeredClient.synchronization_status
                        //    );
                        //}
                        //else
                        //{
                        //    MessageBox.Show(
                        //        "El programa se detuvo en el cliente registrado número: " + (i + 1) + "\n" +
                        //        "registeredClient.synchronization_table_id: " + registeredClient.synchronization_table_id + "\n" +
                        //        "registeredClient.PAR_PAIS_1: " + registeredClient.PAR_PAIS_1 + "\n" +
                        //        "registeredClient.PAR_LOCALIDAD_1: " + registeredClient.PAR_LOCALIDAD_1 + "\n" +
                        //        "registeredClient.PAR_CP_1: " + registeredClient.PAR_CP_1 + "\n" +
                        //        "registeredClient.PAR_CIF_NIF: " + registeredClient.PAR_CIF_NIF + "\n" +
                        //        "registeredClient.PAR_DIRECCION_1: " + registeredClient.PAR_DIRECCION_1 + "\n" +
                        //        "registeredClient.PAR_PROVINCIA_1: " + registeredClient.PAR_PROVINCIA_1
                        //    );
                        //};
                    };                
                };
            };
        }
    }
}
