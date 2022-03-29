using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace HarborUWP.Models
{
    public class DockingStationView
    {
        private double panelWidth;
        private int rowAmount;
        private int currCount;
        private double rectMargin;
        private double rectSize;
        private SolidColorBrush redBrush;
        private SolidColorBrush greenBrush;

        public DockingStationView()
        {
            redBrush = new SolidColorBrush(new Color
            {
                R = 254,
                G = 39,
                B = 18,
                A = 255
            }); 
            greenBrush = new SolidColorBrush(new Color
            {
                R = 102,
                G = 176,
                B = 50,
                A = 255
            });
        }

        internal void Initialize(List<DockingStation> dockingStations, StackPanel dockingStationStackPanel)
        {
            panelWidth = dockingStationStackPanel.RenderSize.Width;

            //Square root the amount of dockingstations to determine the amount of rows and colums
            rowAmount = (int)Math.Ceiling(Math.Sqrt(dockingStations.Count));

            //Determine width, height and margin per square based on width of parent StackPanel
            rectMargin = panelWidth / rowAmount / 10;
            rectSize = (panelWidth - (rowAmount * 2 * rectMargin)) / rowAmount;
        }

        internal void UpdateDockingStationsStackPanel(List<DockingStation> dockingStations, StackPanel dockingStationStackPanel)
        {
            dockingStationStackPanel.Children.Clear();
            currCount = 0;

            //Loop until all rows are filled
            for (int i = 1; i <= rowAmount; i++)
            {
                StackPanel stackPanel = GenerateStackPanelRow(dockingStations);

                //Add row to parent StackPanel
                dockingStationStackPanel.Children.Add(stackPanel);
            }
        }

        private StackPanel GenerateStackPanelRow(List<DockingStation> dockingStations)
        {
            //Create each row for the parent StackPanel
            StackPanel stackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };

            //Loop until end of row or until end of dockingstations count, whichever one comes first
            for (int j = 1; j <= rowAmount && currCount <= dockingStations.Count - 1; currCount++, j++)
            {
                GenerateStackPanelCell(dockingStations, stackPanel);
            }

            return stackPanel;
        }

        private void GenerateStackPanelCell(List<DockingStation> dockingStations, StackPanel stackPanel)
        {
            Rectangle rectangle;
            //Red square for occupied dockingstation
            if (dockingStations[currCount].IsOccupied())
            {
                rectangle = new Rectangle
                {
                    Fill = redBrush,
                    Width = rectSize,
                    Height = rectSize,
                    Margin = new Thickness(rectMargin)
                };
            }
            //Green square for vacant dockingstation
            else
            {
                rectangle = new Rectangle
                {
                    Fill = greenBrush,
                    Width = rectSize,
                    Height = rectSize,
                    Margin = new Thickness(rectMargin)
                };
            }
            //Add square to row StackPanel
            stackPanel.Children.Add(rectangle);
        }
    }
}
