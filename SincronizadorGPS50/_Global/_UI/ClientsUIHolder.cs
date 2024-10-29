using Infragistics.Win.Misc;
using Infragistics.Win.UltraWinEditors;
using Infragistics.Win;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;

namespace SincronizadorGPS50
{
    internal static class ClientsUIHolder
    {
        internal static UltraPanel MainPanel { get; set; } = null;
        internal static UltraPanel TopRow { get; set; } = null;
        internal static UltraPanel CenterRow { get; set; } = null;
        internal static UltraPanel BottomRow { get; set; } = null;
        internal static TableLayoutPanel TableLayoutPanel { get; set; } = null;
        internal static TableLayoutPanel CenterRowTableLayoutPanel { get; set; } = null;
        internal static UltraGrid ClientDataTable { get; set; } = null;

        internal static TableLayoutPanel TopRowTableLayoutPanel { get; set; } = null;
        internal static UltraLabel TopRowMainInstructionLabel { get; set; } = null;
        internal static UltraButton TopRowRefreshTableButton { get; set; } = null;
        internal static UltraButton TopRowSelectAllButton { get; set; } = null;
        internal static UltraButton TopRowSynchronizeButton { get; set; } = null;


        internal static UltraButton TopRowCloseButton { get; set; } = null;

        internal static TableLayoutPanel BottomRowTableLayoutPanel { get; set; } = null;
        internal static UltraLabel BottomRowMainInstructionLabel { get; set; } = null;
        internal static UltraButton BottomRowSynchronizeSelectedButton { get; set; } = null;
        internal static UltraButton BottomRowSynchronizeFilteredButton { get; set; } = null;
        internal static UltraButton BottomRowSynchronizeAllButton { get; set; } = null;
        internal static UltraButton BottomRowCloseButton { get; set; } = null;

    }
}
