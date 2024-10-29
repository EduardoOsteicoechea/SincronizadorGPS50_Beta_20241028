using Infragistics.Win.UltraWinGrid;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SincronizadorGPS50.Workflows.Clients
{
    internal class RemoveClientsSynchronizationTable
    {
        public RemoveClientsSynchronizationTable()
        {
            ClientsUIHolder.CenterRow.ClientArea.Controls.Remove(ClientsUIHolder.ClientDataTable);
            DataHolder.GestprojectClientClassList.Clear();
        }
    }
}
