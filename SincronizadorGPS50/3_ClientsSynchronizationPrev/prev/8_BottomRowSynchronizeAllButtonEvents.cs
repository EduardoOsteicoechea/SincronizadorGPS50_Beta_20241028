namespace SincronizadorGPS50.Workflows.Clients
{
    internal static class SynchronizeAllButtonEvents
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
            ClientsUIHolder.BottomRowSynchronizeFilteredButton.Enabled = false;
        }
    }
}