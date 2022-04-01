using HarborUWP.Models.Ships;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using System.Collections.ObjectModel;

namespace HarborUWP.Models
{
    internal class ShipStateTable
    {
        public void Initialize(Grid grid, ObservableCollection<Ship> ships)
        {
            DataGrid dataGrid = new DataGrid()
            {
                Margin = new Thickness(10),
                ItemsSource = ships,
                AutoGenerateColumns = false,
            };
            dataGrid.SetValue(Grid.RowProperty, 1);
            dataGrid.SetValue(Grid.RowSpanProperty, 2);

            DataGridTextColumn idColumnHeader = new DataGridTextColumn()
            {
                Header = "ID",
                MaxWidth = 100,
                MinWidth = 100,
                CanUserResize = false,
                Binding = new Binding() { Path = new PropertyPath("Id") }
            };

            DataGridTextColumn typeColumnHeader = new DataGridTextColumn()
            {
                Header = "Type",
                MaxWidth = 220,
                MinWidth = 220,
                CanUserResize = false,
                Binding = new Binding() { Path = new PropertyPath("ShipType") }
            };

            DataGridTextColumn stateColumnHeader = new DataGridTextColumn()
            {
                Header = "State",
                MaxWidth = 240,
                MinWidth = 240,
                CanUserResize = false,
                Binding = new Binding() { Path = new PropertyPath("State") }
            };

            DataGridTextColumn tudColumnHeader = new DataGridTextColumn()
            {
                Header = "Time until done",
                MaxWidth = 160,
                MinWidth = 160,
                CanUserResize = false,
                Binding = new Binding() { Path = new PropertyPath("TimeUntilDone.DurationInMins") }
            };

            dataGrid.Columns.Add(idColumnHeader);
            dataGrid.Columns.Add(typeColumnHeader);
            dataGrid.Columns.Add(stateColumnHeader);
            dataGrid.Columns.Add(tudColumnHeader);

            grid.Children.Add(dataGrid);
        }
    }
}
