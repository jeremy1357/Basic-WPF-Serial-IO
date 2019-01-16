using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
// For popups 
using System.Windows.Controls.Primitives;
// For serial
using System.IO.Ports;
using System.Threading;
using System.Collections.ObjectModel;

namespace Basic_Serial_IO_Example
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SerialPort port;
        ObservableCollection<string> portNames;
        public MainWindow()
        {
            InitializeComponent();
            Popup codePopup = new Popup();
            TextBlock popupText = new TextBlock();
            portDataText.IsEnabled = false;
            disconnectButton.IsEnabled = false;
            writeButton.IsEnabled = false;
            portList.ItemsSource = portNames;
            statusColor.Fill = Brushes.Red;

            popupText.Text = "Popup Text";
            popupText.Background = Brushes.LightBlue;
            popupText.Foreground = Brushes.Blue;
            codePopup.Child = popupText;
        }


        private void DetectSerialPorts(object sender, EventArgs e)
        {
            AttemptDisconnect();
            // Not very efficient but fine for demonstration
            portNames = new ObservableCollection<string>(SerialPort.GetPortNames());
            if (portNames.Count() == 0)
            {
                portNames.Insert(0, "No ports");
                portList.SelectedIndex = 0;
            }
            else
            {
                portNames.Insert(0, "Select port");
                portList.SelectedIndex = 0;
            }
            portList.ItemsSource = portNames;
        }

        private void LoadPortBaudRates(object sender, EventArgs e)
        {
            // Only a few to keep it simple
            int[] speeds = { 600,1200, 2400, 4800, 9600, 14400 };
            portBaudList.ItemsSource = speeds;
        }

        private void ConnectToPort(object sender, RoutedEventArgs e)
        {
            if (portBaudList.SelectedIndex != -1 && portList.SelectedIndex > 0)
            {
                port = new SerialPort(portList.SelectedItem.ToString());
                port.Open();
                if (readWriteTimeout.SelectedIndex == -1)
                {
                    int time = 500;
                    port.ReadTimeout = time;
                    port.WriteTimeout = time;
                }
                else
                {
                    port.ReadTimeout = Int32.Parse(readWriteTimeout.SelectedItem.ToString());
                    port.WriteTimeout = Int32.Parse(readWriteTimeout.SelectedItem.ToString());

                }
                port.StopBits = StopBits.One;
                port.Parity = Parity.None;
                port.DataBits = 8;

                if (port.IsOpen)
                {
                    connectButton.IsEnabled = false;
                    disconnectButton.IsEnabled = true;
                    portDataText.IsEnabled = true;
                    writeButton.IsEnabled = true;
                    port.DataReceived += new SerialDataReceivedEventHandler(SerialDataReceived);
                    statusColor.Fill = Brushes.LightGreen;

                }
                else
                {
                   // Add more safety
                }
            }

        }

        private void LoadReadWrites(object sender, EventArgs e)
        {
            // Milliseconds
            int[] times = { 10, 20, 50, 100, 200, 500 };
            readWriteTimeout.ItemsSource = times;
        }

        private void DisconnectPort(object sender, RoutedEventArgs e)
        {
            AttemptDisconnect();
        }
        private void AttemptDisconnect()
        {
            if (port != null)
            {
                if (port.IsOpen)
                {
                    port.Close();
                    statusColor.Fill = Brushes.Red;
                    disconnectButton.IsEnabled = false;
                    connectButton.IsEnabled = true;
                    portDataText.IsEnabled = false;
                }
            }
        }

        private void SerialDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string x = port.ReadExisting();
            this.Dispatcher.Invoke((Action)(() =>
            {
                portDataText.Text += (String)x;
            }));
        }

        private void OnCheckTab(object sender, RoutedEventArgs e)
        {
            lineEndingNewLineCheckBox.IsEnabled = false;

        }

        private void OnCheckNewLine(object sender, RoutedEventArgs e)
        {
            lineEndingTabCheckBox.IsEnabled = false;
        }

        private void UncheckLineEnding(object sender, RoutedEventArgs e)
        {
            lineEndingTabCheckBox.IsEnabled = true;
        }

        private void UncheckTabEnding(object sender, RoutedEventArgs e)
        {
            lineEndingNewLineCheckBox.IsEnabled = true;

        }

        private void WriteToPortButton(object sender, RoutedEventArgs e)
        {
            if (writeTextBox.Text.Count() != 0 && port != null)
            {
                if (port.IsOpen && writeTextBox.Text.Count() <= 80)
                {
                    string outText = writeTextBox.Text;
                    if (addStart_CheckBox.IsChecked == true)
                    {
                        outText = outText.Insert(0, addStart_TextBox.Text);
                    }
                    if (addEnd_CheckBox.IsChecked == true)
                    {
                        outText = outText.Insert(outText.Count(), addEnd_TextBox.Text);
                    }
                    if (lineEndingNewLineCheckBox.IsChecked == true)
                    {
                        outText = outText.Insert(outText.Count(), "\n");
                    }
                    if (lineEndingTabCheckBox.IsChecked == true)
                    {
                        outText = outText.Insert(outText.Count(), "\t");
                    }
                    port.Write(outText);

                }
            }
        }

        private void ChangeWriteLength(object sender, RoutedEventArgs e)
        {

        }

        private void ChangeWriteLength(object sender, DependencyPropertyChangedEventArgs e)
        {
            int value;

            if (int.TryParse((string)writeLength_TextBox.Text, out value))
            {
                writeTextBox.MaxLength = value;
                writeTextBox.Text = "";
            }
        }
    }
}
