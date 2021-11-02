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
        bool[] dataLedArrayY1;
        bool[] dataLedArrayX1;
        bool[] dataLedArrayY2;
        bool[] dataLedArrayX2;
        bool inLoop = false;

        public MainWindow()
        {
            InitializeComponent();
            dataLedArrayY1 = new bool[ledArray1.ColumnDefinitions.Count];
            dataLedArrayX1 = new bool[ledArray1.RowDefinitions.Count];
            dataLedArrayY2 = new bool[ledArray2.ColumnDefinitions.Count];
            dataLedArrayX2 = new bool[ledArray2.RowDefinitions.Count];
            InitialLedUIRender();
        }

        void InitialLedUIRender()
        {
            for (int y = 0; y < ledArray1.RowDefinitions.Count; y++)
            {
                for (int x = 0; x < ledArray1.ColumnDefinitions.Count; x++)
                {
                    Ellipse newEllipse1 = new Ellipse { Fill = Brushes.Gray };
                    newEllipse1.SetValue(Grid.RowProperty, y);
                    newEllipse1.SetValue(Grid.ColumnProperty, x);
                    ledArray1.Children.Add(newEllipse1);

                    Ellipse newEllipse2 = new Ellipse { Fill = Brushes.Gray };
                    newEllipse2.SetValue(Grid.RowProperty, y);
                    newEllipse2.SetValue(Grid.ColumnProperty, x);
                    ledArray2.Children.Add(newEllipse2);
                }
            }
        }

        void LedInputEmulation(int data, int registerID)
        {
            if (((registerID & (1 << 0)) >> 0) == 1) //C1 register
            {
                for (int i = 0; i < ledArray1.RowDefinitions.Count; i++)
                {
                    dataLedArrayY1[ledArray1.RowDefinitions.Count - 1 - i] = !Convert.ToBoolean((data >> i) & 1);
                }

            }

            if (((registerID & (1 << 1)) >> 1) == 1) //C2 register
            {
                for (int i = 0; i < ledArray2.RowDefinitions.Count; i++)
                {
                    dataLedArrayY2[ledArray2.RowDefinitions.Count - 1 - i] = !Convert.ToBoolean((data >> i) & 1);
                }

            }

            if (((registerID & (1 << 6)) >> 6) == 1) //C7 register
            {
                for (int i = 0; i < ledArray1.ColumnDefinitions.Count; i++)
                {
                    dataLedArrayX1[ledArray1.ColumnDefinitions.Count - 1 - i] = !Convert.ToBoolean((data >> i) & 1);
                    dataLedArrayX2[ledArray2.ColumnDefinitions.Count - 1 - i] = !Convert.ToBoolean((data >> i) & 1);
                }
            }
            UpdateLedIU();
        }
        
        void UpdateLedIU()
        {
            for (int y = 0; y < ledArray1.RowDefinitions.Count; y++)
            {
                for (int x = 0; x < ledArray1.ColumnDefinitions.Count; x++)
                {
                    if (dataLedArrayY1[y] && dataLedArrayX1[x])
                    {
                        Ellipse targetLED = ledArray1.Children.Cast<Ellipse>().First(value => Grid.GetRow(value) == y && Grid.GetColumn(value) == x);
                        targetLED.Fill = Brushes.Green;
                    }
                    else
                    {
                        Ellipse targetLED = ledArray1.Children.Cast<Ellipse>().First(value => Grid.GetRow(value) == y && Grid.GetColumn(value) == x);
                        targetLED.Fill = Brushes.Gray;
                    }

                    if (dataLedArrayY2[y] && dataLedArrayX2[x])
                    {
                        Ellipse targetLED = ledArray2.Children.Cast<Ellipse>().First(value => Grid.GetRow(value) == y && Grid.GetColumn(value) == x);
                        targetLED.Fill = Brushes.Green;
                    }
                    else
                    {
                        Ellipse targetLED = ledArray2.Children.Cast<Ellipse>().First(value => Grid.GetRow(value) == y && Grid.GetColumn(value) == x);
                        targetLED.Fill = Brushes.Gray;
                    }
                }
            }
        }


        void DebugPrintArray()
        {
            string output = "";
            for (int y = 0; y < ledArray1.RowDefinitions.Count; y++)
            {
                for (int x = 0; x < ledArray1.ColumnDefinitions.Count; x++)
                {
                    output += $"{y}_{x}: " + dataLedArrayY1[y] + " " + dataLedArrayX1[x] + "\n";
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
            MessageBox.Show(inLoop ? "Loop is running" : "Loop was stopped");
            //while (inLoop)
            //{
            //    LedInputEmulation(0b11100111, 0b01000000);
            //    LedInputEmulation(0b01111111, 0b00000001);
            //    await Task.Delay(delay);

            //    LedInputEmulation(0b11000111, 0b01000000);
            //    LedInputEmulation(0b10111111, 0b00000001);
            //    await Task.Delay(delay);

            //    LedInputEmulation(0b10000111, 0b01000000);
            //    LedInputEmulation(0b11011111, 0b00000001);
            //    await Task.Delay(delay);

            //    LedInputEmulation(0b11100111, 0b01000000);
            //    LedInputEmulation(0b11101111, 0b00000001);
            //    await Task.Delay(delay);

            //    LedInputEmulation(0b11100111, 0b01000000);
            //    LedInputEmulation(0b11110111, 0b00000001);
            //    await Task.Delay(delay);

            //    LedInputEmulation(0b11100111, 0b01000000);
            //    LedInputEmulation(0b11111011, 0b00000001);
            //    await Task.Delay(delay);

            //    LedInputEmulation(0b11100111, 0b01000000);
            //    LedInputEmulation(0b11111101, 0b00000001);
            //    await Task.Delay(delay);

            //    LedInputEmulation(0b11100111, 0b01000000);
            //    LedInputEmulation(0b11111110, 0b00000001);
            //    await Task.Delay(delay);
            //}

            while (inLoop)
            {
                for (int i = 0; i < FontNumbers.fontZero.GetLength(0); i++)
                {
                    LedInputEmulation(FontNumbers.fontZero[i], 0b01000001);
                    LedInputEmulation((0b10000000 >> i) ^ 0b11111111 , 0b01000000);
                    await Task.Delay(delay);
                }
            }

        }
    }


    static class FontNumbers
    {
        public static int[] fontZero = new int[]
        {
            0b11000011,
            0b10011001,
            0b10011001,
            0b10011001,
            0b10011001,
            0b10011001,
            0b10011001,
            0b11000011
        };

        public static int[] fontOne = new int[]
        {
            0b11100111,
            0b11000111,
            0b10000111,
            0b11100111,
            0b11100111,
            0b11100111,
            0b11100111,
            0b11100111,
        };

        public static int[] fontTwo = new int[]
        {
            0b11100111,
            0b11000011,
            0b10011001,
            0b11111001,
            0b11110011,
            0b11100111,
            0b11001111,
            0b10000001,
        };

        public static int[] fontThree = new int[]
        {
            0b11000011,
            0b10011001,
            0b11111001,
            0b11100011,
            0b11100011,
            0b11111001,
            0b10011001,
            0b11000011,
        };

        public static int[] fontFour = new int[]
        {
            0b11110001,
            0b11100001,
            0b11001001,
            0b10011001,
            0b00111001,
            0b00000000,
            0b11111001,
            0b11111001,
        };

        public static int[] fontFive = new int[]
        {
            0b11000001,
            0b10000001,
            0b10011111,
            0b10000011,
            0b10000001,
            0b11111001,
            0b10000001,
            0b11000011,
        };

        public static int[] fontSix = new int[]
        {
            0b11100011,
            0b11000001,
            0b10011111,
            0b10000011,
            0b10011001,
            0b10011001,
            0b10011001,
            0b11000011,
        };

        public static int[] fontSeven = new int[]
        {
            0b11000011,
            0b10000001,
            0b11111001,
            0b11111001,
            0b11110011,
            0b11100111,
            0b11001111,
            0b11001111,
        };

        public static int[] fontEight = new int[]
        {
            0b11000011,
            0b10011001,
            0b10011001,
            0b11000011,
            0b11000011,
            0b10011001,
            0b10011001,
            0b11000011,
        };

        public static int[] fontNine = new int[]
        {
            0b11000011,
            0b10011001,
            0b10011001,
            0b10011001,
            0b11000001,
            0b11111001,
            0b11110011,
            0b11000111,
        };
    }
}
