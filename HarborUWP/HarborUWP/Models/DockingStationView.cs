using HarborUWP.Models.Enums;
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
        private SolidColorBrush orangeBrush;
        private SolidColorBrush yellowBrush;

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
            orangeBrush = new SolidColorBrush(new Color
            {
                R = 252,
                G = 115,
                B = 7,
                A = 255
            });
            yellowBrush = new SolidColorBrush(new Color
            {
                R = 252,
                G = 203,
                B = 26,
                A = 255
            });
        }

        internal void Initialize(List<DockingStation> dockingStations, StackPanel dockingStationStackPanel)
        {
            panelWidth = dockingStationStackPanel.RenderSize.Height;

            //Square root the amount of dockingstations to determine the amount of rows and colums
            rowAmount = (int)Math.Ceiling(Math.Sqrt(dockingStations.Count));

            //Determine width, height and margin per square based on width of parent StackPanel
            rectMargin = panelWidth / rowAmount / 10;
            rectSize = (panelWidth - (rowAmount * 2.1d * rectMargin)) / rowAmount;
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
                GenerateStackPanelCell(dockingStations[currCount], stackPanel);
            }

            return stackPanel;
        }

        private void GenerateStackPanelCell(DockingStation dockingStation, StackPanel stackPanel)
        {
            UIElement uiElement;
            //Red square for occupied dockingstation
            if(!dockingStation.IsOccupied())
            {
                Rectangle rectangle = new Rectangle
                {
                    Fill = greenBrush,
                    Width = rectSize,
                    Height = rectSize,
                    Margin = new Thickness(rectMargin),

                };
                uiElement = rectangle;
                stackPanel.Children.Add(uiElement);
                return;
            }

            SolidColorBrush brush;
            switch (dockingStation.Ship.State)
            {
                case State.Docking:
                    brush = orangeBrush;
                    break;
                case State.Leaving:
                    brush = yellowBrush;
                    break;
                case State.Loading:
                    brush = redBrush;
                    break;
                case State.Offloading:
                default:
                    brush = redBrush;
                    break;
            }

            string shipId = dockingStation.Ship.Id.ToString();

            Grid grid = new Grid();

            Rectangle rectangleInner = new Rectangle
            {
                Fill = brush,
                Width = rectSize,
                Height = rectSize,
                Margin = new Thickness(rectMargin)
            };

            TextBlock textBlock = new TextBlock()
            {
                Text = shipId,
                Margin = new Thickness(rectSize / 2.5f - shipId.Length, rectSize / 2.5f, 0, 0),
                FontSize = (rectMargin * 2f)
            };

            grid.Children.Add(rectangleInner);
            grid.Children.Add(textBlock);

            uiElement = grid;

            //Add square to row StackPanel
            stackPanel.Children.Add(uiElement);
        }
    }
}
