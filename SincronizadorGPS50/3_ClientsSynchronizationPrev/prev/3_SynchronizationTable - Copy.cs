//using SincronizadorGPS50.GestprojectAPI;
//using System.Collections.Generic;
//using System.Data;

//namespace SincronizadorGPS50.Workflows.Clients
//{
//    internal static class SynchronizationTable
//    {
//        public static DataTable Create()
//        {
//            List<GestprojectClient> gestprojectClientList = GestprojectClients.Get();

//            DataTable table = new CreateTableControl().Table;

//            bool Sage50SincronizationTableExists = new CheckIfTableExistsOnGestproject("INT_SAGE_SINC_CLIENTE").Exists;

//            if(!Sage50SincronizationTableExists)
//            {
//                new CreateGestprojectSage50SynchronizationTable();
//            };

//            bool Sage50SynchronizationTableWasJustCreated = !Sage50SincronizationTableExists;

//            for(int i = 0; i < gestprojectClientList.Count; i++)
//            {
//                GestprojectClient gestprojectClient = gestprojectClientList[i];

//                new PopulateGestprojectClientSynchronizationData(gestprojectClient);

//                if(Sage50SynchronizationTableWasJustCreated)
//                {
//                    new RegisterClient(
//                        gestprojectClient,
//                        SynchronizationStatusOptions.Nunca_ha_sido_sincronizado
//                    );

//                    new PopulateGestprojectClientSynchronizationData(gestprojectClient);

//                    new AddClientToSynchronizationUITable(
//                        gestprojectClient,
//                        table,
//                        SynchronizationStatusOptions.Nunca_ha_sido_sincronizado
//                    );
//                }
//                else
//                {
//                    bool GestprojectClientWasRegistered = new IsGestprojectClientRegistered(gestprojectClient).ItIs;

//                    if(GestprojectClientWasRegistered)
//                    {
//                        new PopulateGestprojectClientSynchronizationData(gestprojectClient);

//                        bool GestprojectClientWasSynchronized = new CheckIfGestprojectClientWasSynchronized(gestprojectClient).ItIs;

//                        if(!GestprojectClientWasSynchronized)
//                        {
//                            new UpdateClientSynchronizationStatus(
//                                gestprojectClient,
//                                SynchronizationStatusOptions.Nunca_ha_sido_sincronizado
//                            );

//                            new PopulateGestprojectClientSynchronizationData(gestprojectClient);

//                            new AddClientToSynchronizationUITable(
//                                gestprojectClient,
//                                table,
//                                SynchronizationStatusOptions.Nunca_ha_sido_sincronizado
//                            );
//                        }
//                        else
//                        {
//                            IsGestprojectClientSynchronized isGestprojectClientSynchronized =  new IsGestprojectClientSynchronized(gestprojectClient);

//                            if(isGestprojectClientSynchronized.ItIs)
//                            {
//                                new UpdateClientSynchronizationStatus(
//                                    gestprojectClient,
//                                    SynchronizationStatusOptions.Sincronizado
//                                );

//                                new PopulateGestprojectClientSynchronizationData(gestprojectClient);

//                                new CheckEndPointsDataConformity(gestprojectClient);

//                                new AddClientToSynchronizationUITable(
//                                    gestprojectClient,
//                                    table,
//                                    SynchronizationStatusOptions.Sincronizado
//                                );
//                            }
//                            else
//                            {
//                                new UpdateClientSynchronizationStatus(
//                                    gestprojectClient,
//                                    SynchronizationStatusOptions.Desincronizado
//                                );

//                                new PopulateGestprojectClientSynchronizationData(gestprojectClient);

//                                new CheckEndPointsDataConformity(gestprojectClient);

//                                new AddClientToSynchronizationUITable(
//                                    gestprojectClient,
//                                    table,
//                                    "",
//                                    isGestprojectClientSynchronized.Comment
//                                );
//                            };
//                        };
//                    }
//                    else
//                    {
//                        new RegisterClient(
//                            gestprojectClient,
//                            SynchronizationStatusOptions.Nunca_ha_sido_sincronizado
//                        );

//                        new PopulateGestprojectClientSynchronizationData(gestprojectClient);

//                        new CheckEndPointsDataConformity(gestprojectClient);

//                        new AddClientToSynchronizationUITable(
//                            gestprojectClient,
//                            table,
//                            SynchronizationStatusOptions.Nunca_ha_sido_sincronizado
//                        );
//                    };
//                };
//            };

//            return table;
//        }        
        
//        public static DataTable Refresh()
//        {
//            List<GestprojectClient> gestprojectClientList = GestprojectClients.Get();

//            DataTable table = new CreateTableControl().Table;

//            for(int i = 0; i < gestprojectClientList.Count; i++)
//            {
//                GestprojectClient gestprojectClient = gestprojectClientList[i];

//                new PopulateGestprojectClientSynchronizationData(gestprojectClient);

//                new CheckEndPointsDataConformity(gestprojectClient);

//                new AddClientToSynchronizationUITable(
//                    gestprojectClient,
//                    table,
//                    gestprojectClient.synchronization_status
//                );
//            };

//            return table;
//        }
//    }
//}
