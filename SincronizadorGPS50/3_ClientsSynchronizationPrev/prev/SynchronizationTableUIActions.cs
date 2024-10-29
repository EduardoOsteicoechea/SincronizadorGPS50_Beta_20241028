using Infragistics.Win.UltraWinGrid;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SincronizadorGPS50
{
    internal static class SynchronizationTableUIActions
    {
        internal static List<UltraGridRow> UltraGridRowList { get; set; } = new List<UltraGridRow>();
        internal static List<int> GestprojectClientIdList { get; set; } = new List<int>();    
        internal static void Set(object sender, Infragistics.Win.UltraWinGrid.ClickCellEventArgs e) 
        {
            UltraGrid ultraGrid = sender as UltraGrid;
            UltraGridRow ultraGridRow = ultraGrid.ActiveRow;
            UltraGridRowList.Add(ultraGridRow);

            if((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
            {
                if(UltraGridRowList.Count > 1)
                {
                    UltraGridRow previousIndex = UltraGridRowList[UltraGridRowList.Count - 2];
                    int selectedIndex1 = previousIndex.Index;
                    int selectedIndex2 = ultraGridRow.Index;
                    int MaxIndex = Math.Max(selectedIndex1, selectedIndex2);
                    int MinIndex = Math.Min(selectedIndex1, selectedIndex2);

                    for(global::System.Int32 i = MinIndex; i < MaxIndex; i++)
                    {
                        ultraGrid.Rows[i].Selected = true;
                        UltraGridRowList.Add(ultraGrid.Rows[i]);
                    };
                };
            }
            else if((Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
                UltraGridRowList.Add(ultraGridRow);
                ultraGridRow.Selected = true;
            }
            else
            {
                UltraGridRowList.Clear();
                UltraGridRowList.Add(ultraGridRow);
                foreach(var item in ultraGrid.Rows)
                {
                    item.Selected = false;
                };
                ultraGridRow.Selected = true;
            };

            //DataHolder.ListOfSelectedClientIdInTable.Clear();

            foreach(var item in UltraGridRowList)
            {
                item.Selected = true;
                //GestprojectClientIdList.Add((int)item.Cells[2].Value);

                //if(!DataHolder.ListOfSelectedClientIdInTable.Contains((int)item.Cells[2].Value))
                //{
                //    DataHolder.ListOfSelectedClientIdInTable.Add((int)item.Cells[2].Value);
                //};
            };
        }

        internal static void CollectCurrentlySelected(UltraGrid ultraGrid)
        {
            UltraGridRow ultraGridRow = ultraGrid.ActiveRow;
            UltraGridRowList.Add(ultraGridRow);

            if((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
            {
                if(UltraGridRowList.Count > 1)
                {
                    UltraGridRow previousIndex = UltraGridRowList[UltraGridRowList.Count - 2];
                    int selectedIndex1 = previousIndex.Index;
                    int selectedIndex2 = ultraGridRow.Index;
                    int MaxIndex = Math.Max(selectedIndex1, selectedIndex2);
                    int MinIndex = Math.Min(selectedIndex1, selectedIndex2);

                    for(global::System.Int32 i = MinIndex; i < MaxIndex; i++)
                    {
                        ultraGrid.Rows[i].Selected = true;
                        //UltraGridRowList.Add(ultraGrid.Rows[i]);
                    };
                };
            };

            foreach(var item in UltraGridRowList)
            {
                item.Selected = true;
                //GestprojectClientIdList.Add((int)item.Cells[2].Value);

                //if(!DataHolder.ListOfSelectedClientIdInTable.Contains((int)item.Cells[2].Value))
                //{
                //    DataHolder.ListOfSelectedClientIdInTable.Add((int)item.Cells[2].Value);
                //};
            };
        } 
        
        internal static void CollectFilteredInTableUI(UltraGrid ultraGrid)
        {
            DataHolder.ListOfSelectedClientIdInTable.Clear();

            foreach(var item in ultraGrid.Rows)
            {
                if(!item.IsFilteredOut)
                {
                    if(!DataHolder.ListOfSelectedClientIdInTable.Contains((int)item.Cells[2].Value))
                    {
                        DataHolder.ListOfSelectedClientIdInTable.Add((int)item.Cells[2].Value);
                    };
                };
            };
        }
        internal static void DeselectRows(UltraGrid ultraGrid)
        {
            //DataHolder.ListOfSelectedClientIdInTable.Clear();

            foreach(var item in UltraGridRowList)
            {
                item.Selected = false;
            };
        }
    }
}
