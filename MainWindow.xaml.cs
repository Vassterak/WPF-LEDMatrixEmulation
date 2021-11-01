using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_LEDMatrixMultiplexing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        bool[] dataLedArrayY;
        bool[] dataLedArrayX;
        bool inLoop = false;

        public MainWindow()
        {
            InitializeComponent();
            dataLedArrayY = new bool[ledArray.ColumnDefinitions.Count];
            dataLedArrayX = new bool[ledArray.RowDefinitions.Count];
            InitialLedUIRender();
        }

        void InitialLedUIRender()
        {
            for (int y = 0; y < ledArray.RowDefinitions.Count; y++)
            {
                for (int x = 0; x < ledArray.ColumnDefinitions.Count; x++)
                {
                    Ellipse newEllipse = new Ellipse { Fill = Brushes.Gray };
                    newEllipse.SetValue(Grid.RowProperty, y);
                    newEllipse.SetValue(Grid.ColumnProperty, x);
                    ledArray.Children.Add(newEllipse);
                }
            }
        }

        void LedInputEmulation(int data, int registerID)
        {
            if (registerID == 0b00000001) //C1 register
            {
                for (int i = 0; i < ledArray.RowDefinitions.Count; i++)
                {
                    dataLedArrayY[ledArray.RowDefinitions.Count - 1 - i] = !Convert.ToBoolean((data >> i) & 1);
                }

            }

            else if (registerID == 0b01000000) //C7 register
            {
                for (int i = 0; i < ledArray.ColumnDefinitions.Count; i++)
                {
                    dataLedArrayX[ledArray.ColumnDefinitions.Count - 1 - i] = !Convert.ToBoolean((data >> i) & 1);
                }
            }
            UpdateLedIU();
        }
        
        void UpdateLedIU()
        {
            for (int y = 0; y < ledArray.RowDefinitions.Count; y++)
            {
                for (int x = 0; x < ledArray.ColumnDefinitions.Count; x++)
                {
                    if (dataLedArrayY[y] && dataLedArrayX[x])
                    {
                        Ellipse targetLED = ledArray.Children.Cast<Ellipse>().First(value => Grid.GetRow(value) == y && Grid.GetColumn(value) == x);
                        targetLED.Fill = Brushes.Green;
                    }
                    else
                    {
                        Ellipse targetLED = ledArray.Children.Cast<Ellipse>().First(value => Grid.GetRow(value) == y && Grid.GetColumn(value) == x);
                        targetLED.Fill = Brushes.Gray;
                    }
                }
            }
        }


        void DebugPrintArray()
        {
            string output = "";
            for (int y = 0; y < ledArray.RowDefinitions.Count; y++)
            {
                for (int x = 0; x < ledArray.ColumnDefinitions.Count; x++)
                {
                    output += $"{y}_{x}: " + dataLedArrayY[y] + " " + dataLedArrayX[x] + "\n";
                }
            }
            MessageBox.Show(output);
        }

        private void debugButton_Click(object sender, RoutedEventArgs e)
        {
            DebugPrintArray();
        }

        private void testButton_Click(object sender, RoutedEventArgs e) //Input your data code here!
        {
            inLoop = !inLoop;
            EmulatedAVRLoop(int.Parse(delayTextBox.Text));
        }

        private async void EmulatedAVRLoop(int delay)
        {
            while (inLoop)
            {
                LedInputEmulation(0b11100111, 0b01000000);
                LedInputEmulation(0b01111111, 0b00000001);
                await Task.Delay(delay);

                LedInputEmulation(0b11000111, 0b01000000);
                LedInputEmulation(0b10111111, 0b00000001);
                await Task.Delay(delay);

                LedInputEmulation(0b10000111, 0b01000000);
                LedInputEmulation(0b11011111, 0b00000001);
                await Task.Delay(delay);

                LedInputEmulation(0b11100111, 0b01000000);
                LedInputEmulation(0b11101111, 0b00000001);
                await Task.Delay(delay);

                LedInputEmulation(0b11100111, 0b01000000);
                LedInputEmulation(0b11110111, 0b00000001);
                await Task.Delay(delay);

                LedInputEmulation(0b11100111, 0b01000000);
                LedInputEmulation(0b11111011, 0b00000001);
                await Task.Delay(delay);

                LedInputEmulation(0b11100111, 0b01000000);
                LedInputEmulation(0b11111101, 0b00000001);
                await Task.Delay(delay);

                LedInputEmulation(0b11100111, 0b01000000);
                LedInputEmulation(0b11111110, 0b00000001);
                await Task.Delay(delay);
            }
        }
    }
}
