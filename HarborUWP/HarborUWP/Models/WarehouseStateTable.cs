using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace HarborUWP.Models
{
    internal class WarehouseStateTable
    {


        public void Initialize(Grid grid, Warehouse warehouse)
        {
            DataGrid dataGrid = new DataGrid()
            {
                Margin = new Thickness(10, 47, 10, 10),
                ItemsSource = warehouse.WarehouseView,
                AutoGenerateColumns = false,
            };

            dataGrid.SetValue(Grid.RowProperty, 3);

            DataGridTextColumn nameColumnHeader = new DataGridTextColumn()
            {
                Header = "Storage item name",
                MaxWidth = 300,
                MinWidth = 300,
                CanUserResize = false,
                Binding = new Binding() { Path = new PropertyPath("Name") }
            };

            DataGridTextColumn amountColumnHeader = new DataGridTextColumn()
            {
                Header = "Amount",
                MaxWidth = 420,
                MinWidth = 420,
                CanUserResize = false,
                Binding = new Binding() { Path = new PropertyPath("StorageObject.Amount") }
            };

            dataGrid.Columns.Add(nameColumnHeader);
            dataGrid.Columns.Add(amountColumnHeader);

            grid.Children.Add(dataGrid);
        }
    }
}
