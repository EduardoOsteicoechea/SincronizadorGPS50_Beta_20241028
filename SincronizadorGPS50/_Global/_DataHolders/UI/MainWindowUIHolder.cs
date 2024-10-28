using Infragistics.Win.UltraWinTabControl;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
    internal class MainWindowUIHolder
    {
        internal static System.Windows.Forms.Form MainWindow { get; set; } = null;
        internal static Infragistics.Win.UltraWinTabControl.UltraTabControl MainTabControl { get; set; } = null;
        internal static Infragistics.Win.Misc.UltraPanel MainTabControlMainPanel { get; set; } = null;


        internal static TableLayoutPanel MainWindowTableLayoutPanel { get; set; } = null;



        internal static UltraTab GlobalActionsTab { get; set; } = null;
        internal static UltraTab Sage50ConnectionTab { get; set; } = null;
        internal static UltraTab CompaniesTab { get; set; } = null;
        internal static UltraTab CustomersTab { get; set; } = null;
        internal static UltraTab ProvidersTab { get; set; } = null;
        internal static UltraTab ProjectsTab { get; set; } = null;
        internal static UltraTab TaxesTab { get; set; } = null;
        internal static UltraTab SubaccountableAccountsTab { get; set; } = null;
        internal static UltraTab IssuedBillsTab { get; set; } = null;
        internal static UltraTab ReceivedBillsTab { get; set; } = null;
    }
}
