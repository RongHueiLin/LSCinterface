using System;
using System.Collections.Generic;
using System.IO;
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
            dgRawData.Items.Refresh();  //CLEAR HMI RAW DATA GRID
            tbAlarmLog.Text = "";   //CLEAR HMI ALARM LOG
        }

        private void btnWriteTest_Click(object sender, RoutedEventArgs e)
        {
            TPCANStatus stsResult;
            var dataGrid = new RawDataFormat();
            TPCANMsg CANMsg = new TPCANMsg();
            CANMsg.DATA = new byte[8];

            string[] test = new string[] { "02", "10", "03", "00", "00", "00", "00", "00" };   //TEST DATA

            //CONFIGURATE THE MESSAGE
            CANMsg.ID = Convert.ToUInt32("300", 16);
            CANMsg.LEN = Convert.ToByte(8);
            CANMsg.MSGTYPE = TPCANMessageType.PCAN_MESSAGE_STANDARD;

            //GET TRANSMIT DATA MESSAGE
            for (int i = 0; i < GetLengthFromDLC(CANMsg.LEN, true); i++)
            {
                CANMsg.DATA[i] = Convert.ToByte(test[i], 16);
            }

            stsResult = PCANBasic.Write(m_PcanHandle, ref CANMsg);

            if (stsResult == TPCANStatus.PCAN_ERROR_OK)
            {
                dataGrid = new RawDataFormat
                {
                    RawTime = DateTime.Now.TimeOfDay.ToString(),
                    RawID = CANMsg.ID.ToString(),
                    RawLength = CANMsg.DATA.Length.ToString(),
                    RawData = BitConverter.ToString(CANMsg.DATA).Replace("-", " ")
                };

                dgRawData.Items.Add(dataGrid);

                tbAlarmLog.Text += "Message was successfully SENT \r\n";
            }
            else
                tbAlarmLog.Text += stsResult.ToString() + "\r\n"; //SHOW ERROR
        }

        private void btnReadTest_Click(object sender, RoutedEventArgs e)
        {
            TPCANStatus stsResult;
            var dataGrid = new RawDataFormat();
            string[] rawDatas = new string[4];

            stsResult = ReadMessage(ref rawDatas);  //READ DATA FRAME
            if (stsResult == TPCANStatus.PCAN_ERROR_OK)
            {
                dataGrid = new RawDataFormat { RawTime = rawDatas[0], RawID = rawDatas[1], RawLength = rawDatas[2], RawData = rawDatas[3] };
                dgRawData.Items.Add(dataGrid);
                tbAlarmLog.Text += stsResult.ToString() + "\r\n";
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
                        tbAlarmLog.Text += line;
                    }
                }
                catch (FileNotFoundException) { tbAlarmLog.Text += "BinA File not Found"; }
            }
        }

        private void btnBinS_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialogS = new OpenFileDialog();
            dialogS.Title = "Select file";
            dialogS.Filter = "bin files (*.*)|*.bin";
            if (dialogS.ShowDialog() == true)
            {
                lblBinS.Content = dialogS.FileName;
            }
        }

        private void btnBinP_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialogP = new OpenFileDialog();
            dialogP.Title = "Select file";
            dialogP.Filter = "bin files (*.*)|*.bin";
            if (dialogP.ShowDialog() == true)
            {
                lblBinP.Content = dialogP.FileName;
            }
        }

        private void btnBinM1_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialogM1 = new OpenFileDialog();
            dialogM1.Title = "Select file";
            dialogM1.Filter = "bin files (*.*)|*.bin";
            if (dialogM1.ShowDialog() == true)
            {
                lblBinM1.Content = dialogM1.FileName;
            }
        }

        private void btnBinM2_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialogM2 = new OpenFileDialog();
            dialogM2.Title = "Select file";
            dialogM2.Filter = "bin files (*.*)|*.bin";
            if (dialogM2.ShowDialog() == true)
            {
                lblBinM2.Content = dialogM2.FileName;
            }
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

        public static int GetLengthFromDLC(int dlc, bool isSTD)
        {
            if (dlc <= 8)
                return dlc;

            if (isSTD)
                return 8;

            switch (dlc)
            {
                case 9: return 12;
                case 10: return 16;
                case 11: return 20;
                case 12: return 24;
                case 13: return 32;
                case 14: return 48;
                case 15: return 64;
                default: return dlc;
            }
        }

        //private TPCANStatus WriteFrame(TPCANMsg writeData)
        //{
        //    ////CREATE MESSAGE STRUCTURE
        //    //TPCANMsg CANMsg = new TPCANMsg();
        //    //CANMsg.DATA = new byte[8];

        //    ////CONFIGURATE THE MESSAGE
        //    //CANMsg.ID = Convert.ToUInt32("300", 16);
        //    //CANMsg.LEN = Convert.ToByte(8);
        //    //CANMsg.MSGTYPE = TPCANMessageType.PCAN_MESSAGE_STANDARD;

        //    ////GET TRANSMIT DATA MESSAGE
        //    //for (int i = 0; i < GetLengthFromDLC(CANMsg.LEN, true); i++)
        //    //{
        //    //    CANMsg.DATA[i] = Convert.ToByte(writeData[i], 16);
        //    //}

        //    //return PCANBasic.Write(m_PcanHandle, ref writeData);
        //}

        private TPCANStatus ReadMessage(ref string[] DataInfo)
        {
            TPCANMsg CANMsg;
            TPCANTimestamp CANTimeStamp;
            TPCANStatus stsResult;
            string[] rawDatas = new string[4];

            stsResult = PCANBasic.Read(m_PcanHandle, out CANMsg, out CANTimeStamp);
            if (stsResult == TPCANStatus.PCAN_ERROR_OK)
            {
                DataInfo[0] = DateTime.Now.TimeOfDay.ToString();
                DataInfo[1] = "test";
                DataInfo[2] = CANMsg.DATA.Length.ToString();
                DataInfo[3] = BitConverter.ToString(CANMsg.DATA).Replace("-", " ");
            }
            return stsResult;
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
                                cbDevice.Items.Add($"PCAN-PCI({iDeviceID.ToString()}h)");
                                break;

                            case 0x51:
                            case 0x52:
                            case 0x53:
                            case 0x54:
                            case 0x55:
                                cbDevice.Items.Add($"PCAN-USB{iDeviceID.ToString()}h)");
                                break;

                            case 0x801:
                            case 0x802:
                            case 0x803:
                            case 0x804:
                            case 0x805:
                                cbDevice.Items.Add($"PCAN-LAN{iDeviceID.ToString()}h)");
                                break;
                        }
                    }
                }
            }
        }
    }
}