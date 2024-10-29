using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SincronizadorGPS50.Workflows.Clients
{
    internal static class TopRowRefreshTableButtonEvents
    {
        internal static void Click(object sender, System.EventArgs e)
        {
            //Infragistics.Win.AppStyling.StyleManager.Load(System.Windows.Forms.Application.StartupPath + "\\Resources\\Styles\\Excel2013 - White.isl");

            new RemoveClientsSynchronizationTable();

            //new CenterRowUI(SynchronizationTable.Refresh);
            
            ClientsUIHolder.BottomRowSynchronizeFilteredButton.Enabled = false;
            ClientsUIHolder.BottomRowSynchronizeSelectedButton.Enabled = false;

            ClientsUIHolder.TopRowMainInstructionLabel.Text = "Visualize el estado actual de sus clientes respecto a la información de Sage50. Renderizado el " + DateTime.UtcNow.ToShortDateString().ToString() + " en el horario " + DateTime.Now.TimeOfDay.ToString();
        }
    }
}
