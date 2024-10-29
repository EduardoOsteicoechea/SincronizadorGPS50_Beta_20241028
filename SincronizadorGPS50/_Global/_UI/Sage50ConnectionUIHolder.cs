using Infragistics.Win.Misc;
using Infragistics.Win.UltraWinEditors;
using SincronizadorGPS50.Workflows.Sage50Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
    internal class Sage50ConnectionUIHolder
    {
        internal static TableLayoutPanel Sage50ConnectionCenterRowCenterPanelTableLayoutPanel
        { get; set; } = null;


        internal static UltraPanel Sage50ConnectionTopRow { get; set; } = null;
        internal static UltraPanel Sage50ConnectionCenterRow { get; set; } = null;
        internal static UltraPanel Sage50ConnectionBottomRow { get; set; } = null;


        internal static UltraPanel Sage50ConnectionCenterRowLeftPanel { get; set; } = null;
        internal static UltraPanel Sage50ConnectionCenterRowCenterPanel { get; set; } = null;
        internal static TableLayoutPanel Sage50ConnectionCenterRowTableLayoutPanel { get; set; } = null;
        internal static UltraPanel Sage50ConnectionCenterRowRightPanel { get; set; } = null;


        internal static UltraLabel CenterRowCenterPanelStateLabel { get; set; } = null;
        internal static UltraLabel CenterRowCenterPanelStateStateMessageLabel { get; set; } = null;
        internal static UltraPictureBox CenterRowCenterPanelStateIcon1 { get; set; } = null;
        internal static UltraPictureBox CenterRowCenterPanelStateIcon2 { get; set; } = null;
        internal static UltraPictureBox CenterRowCenterPanelStateIcon3 { get; set; } = null;
        internal static UltraLabel CenterRowCenterPanelTitleLabel { get; set; } = null;
        internal static UltraLabel CenterRowCenterPanelLocalInstanceLabel { get; set; } = null;
        internal static UltraTextEditor CenterRowCenterPanelLocalInstanceTextBox { get; set; } = null;
        internal static UltraLabel CenterRowCenterPanelUsernameLabel { get; set; } = null;
        internal static UltraTextEditor CenterRowCenterPanelUsernameTextBox { get; set; } = null;
        internal static UltraLabel CenterRowCenterPanelPasswordLabel { get; set; } = null;
        internal static UltraTextEditor CenterRowCenterPanelPasswordTextBox { get; set; } = null;
        internal static UltraButton CenterRowCenterPanelValidateUserDataButton { get; set; } = null;
        internal static UltraLabel CenterRowCenterPanelSesionDataValidationLabel { get; set; } = null;

        internal static UltraPanel CenterRowCenterPanelRememberDataPanel { get; set; } = null;
        internal static UltraLabel CenterRowCenterPanelRememberDataLabel { get; set; } = null;
        internal static UltraCheckEditor CenterRowCenterPanelRememberDataCheckBox { get; set; } = null;

        internal static UltraButton CenterRowCenterPanelGetEnterpryseGroupButton { get; set; } = null;
        internal static UltraLabel CenterRowCenterPanelEnterpryseGroupLabel { get; set; } = null;
        internal static UltraComboEditor CenterRowCenterPanelEnterpryseGroupMenu { get; set; } = null;
        internal static UltraButton CenterRowCenterPanelConnectButton { get; set; } = null;
        internal static UltraButton CenterRowCenterPanelDisconnectButton { get; set; } = null;
        internal static UltraPictureBox CenterRowCenterPanelConnectingSpinner { get; set; } = null;

        internal static UltraButton CenterRowCenterPanelChangeEnterpryseGroupButton { get; set; } = null;


         public static Sage50ConnectionUIManager Sage50ConnectionUIManagerInstance { get; set; } = null;
   }
}
