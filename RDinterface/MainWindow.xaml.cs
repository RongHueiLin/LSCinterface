using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.Win32;
using Peak.Can.Basic;   //INCLUDE PCAN API
using TPCANHandle = System.UInt16;

namespace RDinterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BaseCommand baseCommand = new BaseCommand();
        private TPCANHandle m_PcanHandle;   //DEVICE HANDLE
        private TPCANBaudrate m_Baudrate = TPCANBaudrate.PCAN_BAUD_500K;   //TRANSMIT BAUD RATE
        private TPCANType m_HwType = TPCANType.PCAN_TYPE_ISA;   //HARDWARE TYPE

        //SCAN DEVICE LIST
        TPCANHandle[] m_NonPnPHandles = new TPCANHandle[]
        {
            PCANBasic.PCAN_PCIBUS1,
            PCANBasic.PCAN_PCIBUS2,
            PCANBasic.PCAN_PCIBUS3,
            PCANBasic.PCAN_USBBUS1,
            PCANBasic.PCAN_USBBUS2,
            PCANBasic.PCAN_USBBUS3,
            PCANBasic.PCAN_USBBUS4,
            PCANBasic.PCAN_USBBUS5,
            PCANBasic.PCAN_LANBUS1,
            PCANBasic.PCAN_LANBUS2,
            PCANBasic.PCAN_LANBUS3,
            PCANBasic.PCAN_LANBUS4,
            PCANBasic.PCAN_LANBUS5,
        };

        public MainWindow()
        {
            InitializeComponent();

            CheckDevice();  //CHECK AVAILABLE DEVICES
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            TPCANStatus stsResult;

            m_PcanHandle = Convert.ToUInt16(PCANBasic.PCAN_USBBUS1);

            stsResult = PCANBasic.Initialize(m_PcanHandle, m_Baudrate, m_HwType, Convert.ToUInt32("0100", 16), 3);
            if (stsResult == TPCANStatus.PCAN_ERROR_OK)
            {
                btnConnect.IsEnabled = false;
                tbAlarmLog.Text += "The desired Device-ID was successfully configured \r\n";
            }
        }

        private void btnRelease_Click(object sender, RoutedEventArgs e)
        {
            PCANBasic.Uninitialize(m_PcanHandle);   //DISCONNECT PCAN DEVICE
            m_PcanHandle = 0;   //RESET PCAN HANDLE
            btnConnect.IsEnabled = true;
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            //CLEAR HMI RAW DATA GRID
            dgRawData.Items.Refresh();
            dgRawData.Items.Clear();
            tbAlarmLog.Text = "";   //CLEAR HMI ALARM LOG
        }

        private void btnWriteTest_Click(object sender, RoutedEventArgs e)
        {
            TPCANStatus stsResult;
            string[] rawDatas = new string[4];

            stsResult = baseCommand.Session_ExDiagnotic(m_PcanHandle, ref rawDatas);

            if (stsResult == TPCANStatus.PCAN_ERROR_OK)
            {
                UpdateRawData(rawDatas);
            }
            else
                tbAlarmLog.Text += stsResult.ToString() + "\r\n"; //SHOW ERROR
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            cbDevice.Text = ""; //CLEAR COMBOBOX TEXT
            cbDevice.Items.Clear(); //CLEAR COMBOBOX ITEMS

            CheckDevice();  //CHECK AVAILABLE DEVICES
        }

        private async void btnBinA_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialogA = new OpenFileDialog();
            dialogA.Title = "Select file";
            dialogA.Filter = "bin files (*.*)|*.bin";
            if (dialogA.ShowDialog() == true)
            {
                lblBinA.Content = dialogA.FileName;
                try
                {
                    using (StreamReader srA = new StreamReader(dialogA.FileName))
                    {
                        string line = await srA.ReadToEndAsync();
                        line = line.Replace("\r\n", " ").Replace("  ", " ");
                        string[] Databyte = line.Split(" ");
                        //tbAlarmLog.Text += Databyte.Length;
                    }
                }
                catch (FileNotFoundException) { tbAlarmLog.Text += "BinA File not Found"; }
            }
        }

        private async void btnBinS_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialogS = new OpenFileDialog();
            dialogS.Title = "Select file";
            dialogS.Filter = "bin files (*.*)|*.bin";
            if (dialogS.ShowDialog() == true)
            {
                lblBinS.Content = dialogS.FileName;
                try
                {
                    using (StreamReader srS = new StreamReader(dialogS.FileName))
                    {
                        string line = await srS.ReadToEndAsync();
                    }
                }
                catch (FileNotFoundException) { tbAlarmLog.Text += "BinS File not Found"; }
            }
        }

        private async void btnBinP_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialogP = new OpenFileDialog();
            dialogP.Title = "Select file";
            dialogP.Filter = "bin files (*.*)|*.bin";
            if (dialogP.ShowDialog() == true)
            {
                lblBinP.Content = dialogP.FileName;
                try
                {
                    using (StreamReader srP = new StreamReader(dialogP.FileName))
                    {
                        string line = await srP.ReadToEndAsync();
                    }
                }
                catch (FileNotFoundException) { tbAlarmLog.Text += "BinP File not Found"; }
            }
        }

        private async void btnBinM1_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialogM1 = new OpenFileDialog();
            dialogM1.Title = "Select file";
            dialogM1.Filter = "bin files (*.*)|*.bin";
            if (dialogM1.ShowDialog() == true)
            {
                lblBinM1.Content = dialogM1.FileName;
                try
                {
                    using (StreamReader srM1 = new StreamReader(dialogM1.FileName))
                    {
                        string line = await srM1.ReadToEndAsync();
                    }
                }
                catch (FileNotFoundException) { tbAlarmLog.Text += "BinM1 File not Found"; }
            }
        }

        private async void btnBinM2_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialogM2 = new OpenFileDialog();
            dialogM2.Title = "Select file";
            dialogM2.Filter = "bin files (*.*)|*.bin";
            if (dialogM2.ShowDialog() == true)
            {
                lblBinM2.Content = dialogM2.FileName;
                try
                {
                    using (StreamReader srM2 = new StreamReader(dialogM2.FileName))
                    {
                        string line = await srM2.ReadToEndAsync();
                    }
                }
                catch (FileNotFoundException) { tbAlarmLog.Text += "BinM2 File not Found"; }
            }
        }

        private void btnBootLoad_Click(object sender, RoutedEventArgs e)
        {
            TPCANStatus stsResult;
            string[] rawDatas = new string[4];
            var dataGrid = new RawDataFormat();

            GridFW.IsEnabled = false;

            stsResult = baseCommand.Session_ExDiagnotic(m_PcanHandle, ref rawDatas);

            stsResult = baseCommand.Config_PCU_Resp_1(m_PcanHandle);
            Thread.Sleep(1000);
            stsResult = baseCommand.Config_PCU_Resp_2(m_PcanHandle);
            Thread.Sleep(1000);
            stsResult = baseCommand.Config_PCU_Resp_3(m_PcanHandle);
            Thread.Sleep(1000);
        }

        //SET BAUD RATE WHEN USER CHANGE THE SELECTION
        private void cbBaudRate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (cbBaudRate.SelectedIndex)
            {
                case 0:
                    m_Baudrate = TPCANBaudrate.PCAN_BAUD_1M;
                    break;
                case 1:
                    m_Baudrate = TPCANBaudrate.PCAN_BAUD_800K;
                    break;
                case 2:
                    m_Baudrate = TPCANBaudrate.PCAN_BAUD_500K;
                    break;
                case 3:
                    m_Baudrate = TPCANBaudrate.PCAN_BAUD_250K;
                    break;
                case 4:
                    m_Baudrate = TPCANBaudrate.PCAN_BAUD_125K;
                    break;
                case 5:
                    m_Baudrate = TPCANBaudrate.PCAN_BAUD_100K;
                    break;
                case 6:
                    m_Baudrate = TPCANBaudrate.PCAN_BAUD_95K;
                    break;
                case 7:
                    m_Baudrate = TPCANBaudrate.PCAN_BAUD_83K;
                    break;
                case 8:
                    m_Baudrate = TPCANBaudrate.PCAN_BAUD_50K;
                    break;
                case 9:
                    m_Baudrate = TPCANBaudrate.PCAN_BAUD_47K;
                    break;
                case 10:
                    m_Baudrate = TPCANBaudrate.PCAN_BAUD_33K;
                    break;
                case 11:
                    m_Baudrate = TPCANBaudrate.PCAN_BAUD_20K;
                    break;
                case 12:
                    m_Baudrate = TPCANBaudrate.PCAN_BAUD_10K;
                    break;
                case 13:
                    m_Baudrate = TPCANBaudrate.PCAN_BAUD_5K;
                    break;
            }
        }

        // CHECK AVAILABLE DEVICES
        private void CheckDevice()
        {
            TPCANStatus stsResult;
            uint isChannelValid;
            UInt32 iDeviceID = 0;

            foreach (var Device in m_NonPnPHandles)
            {
                stsResult = PCANBasic.GetValue(Device, TPCANParameter.PCAN_CHANNEL_CONDITION, out isChannelValid, sizeof(uint));
                if (stsResult == TPCANStatus.PCAN_ERROR_OK)
                {
                    if (isChannelValid == PCANBasic.PCAN_CHANNEL_AVAILABLE)
                    {
                        stsResult = PCANBasic.GetValue(Device, TPCANParameter.PCAN_DEVICE_ID, out iDeviceID, sizeof(UInt32));

                        switch (Device)
                        {
                            case 0x41:
                            case 0x42:
                            case 0x43:
                                cbDevice.Items.Add($"PCAN-PCI ({iDeviceID.ToString()}h)");
                                break;

                            case 0x51:
                            case 0x52:
                            case 0x53:
                            case 0x54:
                            case 0x55:
                                cbDevice.Items.Add($"PCAN-USB ({iDeviceID.ToString()}h)");
                                break;

                            case 0x801:
                            case 0x802:
                            case 0x803:
                            case 0x804:
                            case 0x805:
                                cbDevice.Items.Add($"PCAN-LAN ({iDeviceID.ToString()}h)");
                                break;
                        }
                    }
                }
            }
        }

        public void UpdateRawData(string[] rawData)
        {
            var dataGrid = new RawDataFormat { RawTime = rawData[0], RawID = rawData[1], RawLength = rawData[2], RawData = rawData[3] };
            dgRawData.Items.Add(dataGrid);
        }
    }
}