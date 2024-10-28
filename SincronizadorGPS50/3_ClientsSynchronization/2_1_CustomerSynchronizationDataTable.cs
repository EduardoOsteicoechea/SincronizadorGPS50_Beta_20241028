using SincronizadorGPS50.GestprojectDataManager;
using SincronizadorGPS50.Sage50Connector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
   internal static class CustomerSynchronizationDataTable
   {
      public static DataTable Create
      (
         System.Data.SqlClient.SqlConnection connection,
         CompanyGroup sage50CompanyGroupData,
         CustomerSyncronizationTableSchema tableSchema
      )
      {
         bool CustomerSincronizationTableExists = new GestprojectDataManager.CheckIfTableExistsOnGestproject(
            connection,
            tableSchema.TableName
         ).Exists;

         if(!CustomerSincronizationTableExists)
            new GestprojectDataManager.CreateClientSynchronizationTable(connection, tableSchema);

         List<GestprojectDataManager.GestprojectCustomer> gestprojectCustomerList = new GestprojectClientsManager().GetClients(connection);

         List<Sage50Customer> sage50CustomerList = new GetSage50Customer().CustomerList;

         DataTable table = new CreateTableControl(tableSchema).Table;

         for(int i = 0; i < gestprojectCustomerList.Count; i++)
         {
            GestprojectDataManager.GestprojectCustomer gestprojectCustomer = gestprojectCustomerList[i];

            new GestprojectDataManager.AddSynchronizationTableCustomerData(connection, gestprojectCustomer, tableSchema);

            bool mustRegister = !CustomerSincronizationTableExists || !new GestprojectDataManager.WasGestprojectClientRegistered(
                  connection,
                  gestprojectCustomer,
                  sage50CompanyGroupData.CompanyGuidId,
                  tableSchema
               ).ItIs;

            bool registeredInDifferentCompanyGroup = gestprojectCustomer.sage50_company_group_guid_id != "" && sage50CompanyGroupData.CompanyGuidId != gestprojectCustomer.sage50_company_group_guid_id;

            bool neverSynchronized = gestprojectCustomer.sage50_company_group_guid_id == "";

            bool synchronizedInThePast = gestprojectCustomer.sage50_company_group_guid_id != "" && sage50CompanyGroupData.CompanyGuidId == gestprojectCustomer.sage50_company_group_guid_id;

            // add existing customer data to avoid dsincronization status in just linked and equal items

            //new GestprojectDataManager.RegisterClient(connection, gestprojectCustomer, tableSchema);

            if(registeredInDifferentCompanyGroup)
            {
               continue;
            }
            else if(mustRegister)
            {
               new GestprojectDataManager.RegisterClient(connection, gestprojectCustomer, tableSchema);

               new AddSynchronizationTableCustomerData(connection, gestprojectCustomer, tableSchema);
            }
            else if(neverSynchronized || synchronizedInThePast)
            {
               new GestprojectDataManager.UpdateClientState(connection, gestprojectCustomer, tableSchema);
            };

            ValidateClientSyncronizationStatus clientSyncronizationStatusValidator = new ValidateClientSyncronizationStatus(
               gestprojectCustomer,
               sage50CustomerList,
               tableSchema
            );

            if(clientSyncronizationStatusValidator.MustBeDeleted)
            {
               new DeleteFromSynchronizationTable(connection, gestprojectCustomer, tableSchema);

               new DeleteClientCodeInGestproject(connection, gestprojectCustomer, tableSchema);

               new ClearCustomerSynchronizationData(gestprojectCustomer);

               new RegisterClient(connection, gestprojectCustomer, tableSchema);

               new AddSynchronizationTableCustomerData(connection, gestprojectCustomer, tableSchema);
            }
            else if(clientSyncronizationStatusValidator.NeverWasSynchronized)
            {
               gestprojectCustomer.synchronization_status = "Nunca ha sido sincronizado";
            }
            else if(!clientSyncronizationStatusValidator.IsSynchronized)
            {
               gestprojectCustomer.synchronization_status = "Desincronizado";
            };

            new GestprojectDataManager.UpdateClientState(
               connection,
               gestprojectCustomer,
               tableSchema
            );

            new AddClientToUITable(gestprojectCustomer, table);
         };

         return table;
      }
   }
}










