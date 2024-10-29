using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SincronizadorGPS50.Workflows.Clients
{
    internal static class SynchronizeFilteredButtonEvents
    {
        internal static void Click(object sender, System.EventArgs e) 
        {
            SynchronizationTableUIActions.CollectFilteredInTableUI(ClientsUIHolder.ClientDataTable);

            GetSelectedClientsInUITable selectedClientsInUITable = new GetSelectedClientsInUITable(DataHolder.ListOfSelectedClientIdInTable);

            new RemoveClientsSynchronizationTable();

            new SynchronizeClients(selectedClientsInUITable.Clients);

            //new CenterRowUI(SynchronizationTable.Refresh);

            DataHolder.ListOfSelectedClientIdInTable.Clear();

            ClientsUIHolder.BottomRowSynchronizeSelectedButton.Enabled = false;
        }
    }
}
