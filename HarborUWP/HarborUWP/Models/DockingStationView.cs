using HarborUWP.Models.Enums;
using System;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace HarborUWP.Models
{
    public class DockingStationView
    {
        private List<DockingStation> currDockingStations;
        private bool firstCycle;
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
            currDockingStations = new List<DockingStation>();
            firstCycle = true;
            for (int i = 0; i < dockingStations.Count; i++)
            {
                currDockingStations.Add(dockingStations[i].Clone());
            }
            panelWidth = dockingStationStackPanel.RenderSize.Height;

            //Square root the amount of dockingstations to determine the amount of rows and colums
            rowAmount = (int)Math.Ceiling(Math.Sqrt(dockingStations.Count));

            //Determine width, height and margin per square based on width of parent StackPanel
            rectMargin = panelWidth / rowAmount / 10;
            rectSize = (panelWidth - (rowAmount * 2.1d * rectMargin)) / rowAmount;
        }

        internal void UpdateDockingStationsStackPanel(List<DockingStation> dockingStations, StackPanel dockingStationStackPanel)
        {
            bool isSame = IsSameCollection(dockingStations);

            if (isSame)
            {
                return;
            }

            currCount = 0;

            //Loop until all rows are filled
            for (int i = 1; i <= rowAmount; i++)
            {
                if(firstCycle)
                {
                    //Add stack panel to parent stack panel
                    StackPanel stackPanel = GenerateStackPanelRow(dockingStations);
                    dockingStationStackPanel.Children.Add(stackPanel);
                    continue;
                }
                
                UpdateStackPanelRow(dockingStations, (StackPanel)dockingStationStackPanel.Children[i - 1]);
            }

            firstCycle = false;

            //Update current list for next cycle
            currDockingStations = new List<DockingStation>();
            for (int i = 0; i < dockingStations.Count; i++)
            {
                currDockingStations.Add(dockingStations[i].Clone());
            }
        }

        private bool IsSameCollection(List<DockingStation> dockingStations)
        {
            bool isSame = true;
            for (int i = 0; i < dockingStations.Count; i++)
            {
                if(!IsSameShip(dockingStations[i], i))
                {
                    isSame = false;
                    break;
                }
            }

            if (firstCycle)
            {
                isSame = false;
            }

            return isSame;
        }

        private bool IsSameShip(DockingStation dockingStation, int i)
        {
            if (dockingStation.Ship == null && currDockingStations[i].Ship == null)
            {
                return true;
            }

            if (dockingStation.Ship != null && currDockingStations[i].Ship == null)
            {
                return false;
            }

            if (dockingStation.Ship == null && currDockingStations[i].Ship != null)
            {
                return false;
            }

            if (!dockingStation.Ship.Equals(currDockingStations[i].Ship))
            {
                return false;
            }

            return false;
        }

        private StackPanel GenerateStackPanelRow(List<DockingStation> dockingStations)
        {
            //Create each row for the parent StackPanel
            StackPanel stackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };

            //Loop until end of row or until end of dockingstations count, whichever one comes first
            for (int j = 1; j <= rowAmount && currCount <= dockingStations.Count - 1; j++)
            {
                UIElement uiElement = GenerateStackPanelCell(dockingStations[currCount]);
                stackPanel.Children.Add(uiElement);
            }
            
            return stackPanel;

        }

        private Grid GenerateStackPanelCell(DockingStation dockingStation)
        {
            Grid grid;
            //Red square for occupied dockingstation
            if (!dockingStation.IsOccupied())
            {
                grid = new Grid();

                Rectangle rectangle = new Rectangle
                {
                    Fill = greenBrush,
                    Width = rectSize,
                    Height = rectSize,
                    Margin = new Thickness(rectMargin),

                };

                TextBlock textBlock = new TextBlock()
                {
                    Text = "",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontSize = (rectMargin * 4f),
                };

                grid.Children.Add(rectangle);
                grid.Children.Add(textBlock);

                currCount++;

                return grid;
            }

            SolidColorBrush brush = SelectBrush(dockingStation.Ship.State);

            string shipId = dockingStation.Ship.Id.ToString();

            grid = new Grid();

            Rectangle rectangleFilled = new Rectangle
            {
                Fill = brush,
                Width = rectSize,
                Height = rectSize,
                Margin = new Thickness(rectMargin),
            };

            TextBlock textBlockFilled = new TextBlock()
            {
                Text = shipId,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = (rectMargin * 4f),
            };

            grid.Children.Add(rectangleFilled);
            grid.Children.Add(textBlockFilled);

            currCount++;

            return grid;
        }

        private void UpdateStackPanelRow(List<DockingStation> dockingStations, StackPanel stackPanel)
        {
            for (int i = 0; i < stackPanel.Children.Count; i++)
            {
                UpdateStackPanelCell(dockingStations, (Grid)stackPanel.Children[i]);
            }
        }

        private void UpdateStackPanelCell(List<DockingStation> dockingStations, Grid grid)
        {
            if (IsSameShip(dockingStations[currCount], currCount))
            {
                currCount++;
                return;
            }

            Rectangle rectangle = (Rectangle)grid.Children[0];
            TextBlock textBlock = (TextBlock)grid.Children[1];
            
            if(!dockingStations[currCount].IsOccupied())
            {
                rectangle.Fill = greenBrush;
                textBlock.Text = "";

                currCount++;
                return;
            }

            SolidColorBrush brush = SelectBrush(dockingStations[currCount].Ship.State);
            rectangle.Fill = brush;
            textBlock.Text = dockingStations[currCount].Ship.Id.ToString();

            currCount++;
        }

        private SolidColorBrush SelectBrush(State shipState)
        {
            switch (shipState)
            {
                case State.Docking:
                    return orangeBrush;
                case State.Leaving:
                    return yellowBrush;
                case State.Loading:
                    return redBrush;
                case State.Offloading:
                default:
                    return redBrush;
            }
        }
    }
}
