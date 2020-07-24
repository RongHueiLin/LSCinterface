using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
        byte[] DatabyteA, DatabyteS, DatabyteP, DatabyteM1, DatabyteM2;
        BaseCommand baseCommand = new BaseCommand();
        private TPCANHandle m_PcanHandle;   //DEVICE HANDLE
        private TPCANBaudrate m_Baudrate = TPCANBaudrate.PCAN_BAUD_500K;   //TRANSMIT BAUD RATE
        private TPCANType m_HwType = TPCANType.PCAN_TYPE_ISA;   //HARDWARE TYPE
        private List<string[]> rawDatas = new List<string[]>();

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
            //TPCANStatus stsResult;
            //string[] rawDatas = new string[4];

            //stsResult = baseCommand.Session_ExDiagnotic(m_PcanHandle, ref rawDatas);

            //if (stsResult == TPCANStatus.PCAN_ERROR_OK)
            //{
            //    //UpdateGUI(rawDatas);
            //}
            //else
            //    tbAlarmLog.Text += stsResult.ToString() + "\r\n"; //SHOW ERROR
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            cbDevice.Text = ""; //CLEAR COMBOBOX TEXT
            cbDevice.Items.Clear(); //CLEAR COMBOBOX ITEMS

            CheckDevice();  //CHECK AVAILABLE DEVICES
        }

        private void btnBinA_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialogA = new OpenFileDialog();
            dialogA.Title = "Select file";
            dialogA.Filter = "bin files (*.*)|*.bin";
            if (dialogA.ShowDialog() == true)
            {
                lblBinA.Content = dialogA.FileName;
                try
                {
                    FileStream fs = File.Open(dialogA.FileName, FileMode.Open);
                    BinaryReader br = new BinaryReader(fs);

                    DatabyteA = br.ReadBytes(Convert.ToInt32(fs.Length));
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
                try
                {
                    FileStream fs = File.Open(dialogS.FileName, FileMode.Open);
                    BinaryReader br = new BinaryReader(fs);

                    DatabyteS = br.ReadBytes(Convert.ToInt32(fs.Length));
                }
                catch (FileNotFoundException) { tbAlarmLog.Text += "BinS File not Found"; }
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
                try
                {
                    FileStream fs = File.Open(dialogP.FileName, FileMode.Open);
                    BinaryReader br = new BinaryReader(fs);

                    DatabyteP = br.ReadBytes(Convert.ToInt32(fs.Length));
                }
                catch (FileNotFoundException) { tbAlarmLog.Text += "BinP File not Found"; }
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
                try
                {
                    FileStream fs = File.Open(dialogM1.FileName, FileMode.Open);
                    BinaryReader br = new BinaryReader(fs);

                    DatabyteM1 = br.ReadBytes(Convert.ToInt32(fs.Length));
                }
                catch (FileNotFoundException) { tbAlarmLog.Text += "BinM1 File not Found"; }
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
                try
                {
                    FileStream fs = File.Open(dialogM2.FileName, FileMode.Open);
                    BinaryReader br = new BinaryReader(fs);

                    DatabyteM2 = br.ReadBytes(Convert.ToInt32(fs.Length));
                }
                catch (FileNotFoundException) { tbAlarmLog.Text += "BinM2 File not Found"; }
            }
        }

        private void btnBootLoad_Click(object sender, RoutedEventArgs e)
        {
            TPCANStatus stsResult;
            Dictionary<string,byte[]> BinSelect = new Dictionary<string, byte[]>();
            //List<string[]> rawDatas = new List<string[]>();

            GridFW.IsEnabled = false;

            if (cbBinA.IsChecked == true && DatabyteA.Length > 0)
                BinSelect.Add("A",DatabyteA);
            if (cbBinS.IsChecked == true && DatabyteS.Length > 0)
                BinSelect.Add("S",DatabyteS);
            if (cbBinP.IsChecked == true && DatabyteP.Length > 0)
                BinSelect.Add("P",DatabyteP);
            if (cbBinM1.IsChecked == true && DatabyteM1.Length > 0)
                BinSelect.Add("M1",DatabyteM1);
            if (cbBinM2.IsChecked == true && DatabyteM2.Length > 0)
                BinSelect.Add("M2",DatabyteM2);

            foreach (var bin in BinSelect)
            {

                stsResult = BinProcess(m_PcanHandle, bin.Key, bin.Value);
            }

            //stsResult = baseCommand.Session_ExDiagnotic(m_PcanHandle, ref rawDatas);
            //await UpdateGUI(dgRawData, rawDatas);
            //rawDatas.Clear();

            //stsResult = baseCommand.Config_PCU_Resp_1(m_PcanHandle, ref rawDatas);
            //Thread.Sleep(1000);
            //stsResult = baseCommand.Config_PCU_Resp_2(m_PcanHandle, ref rawDatas);
            //Thread.Sleep(1000);
            //stsResult = baseCommand.Config_PCU_Resp_3(m_PcanHandle, ref rawDatas);
            //Thread.Sleep(1000);
            //await UpdateGUI(dgRawData, rawDatas);
            //rawDatas.Clear();

            //stsResult = baseCommand.Session_Program(m_PcanHandle, ref rawDatas);
            //await UpdateGUI(dgRawData, rawDatas);
            //rawDatas.Clear();

            //stsResult = baseCommand.Download_Request_1(m_PcanHandle, ref rawDatas);
            //await UpdateGUI(dgRawData, rawDatas);
            //rawDatas.Clear();

            //stsResult = baseCommand.Download_Request_2(m_PcanHandle, ref rawDatas, DatabyteA.Length);
            //string[] temp = rawDatas[1][3].Split(" ");
            //await UpdateGUI(dgRawData, rawDatas);
            //rawDatas.Clear();

            //if (cbBinA.IsChecked == true && DatabyteA.Length > 0)
            //{
            //    stsResult = baseCommand.BinTransmit(m_PcanHandle, temp[3] + temp[4], DatabyteA, ref rawDatas);
            //    await UpdateGUI(dgRawData, rawDatas);
            //    rawDatas.Clear();
            //}

            //stsResult = baseCommand.Transfer_End(m_PcanHandle, ref rawDatas);
            //await UpdateGUI(dgRawData, rawDatas);
            //rawDatas.Clear();

            //stsResult = baseCommand.PCU_Reset(m_PcanHandle, ref rawDatas);
            //await UpdateGUI(dgRawData, rawDatas);
            //rawDatas.Clear();

            //stsResult = baseCommand.Session_ExDiagnotic(m_PcanHandle, ref rawDatas);
            //await UpdateGUI(dgRawData, rawDatas);
            //rawDatas.Clear();

            //stsResult = baseCommand.Config_PCU_Resp_4(m_PcanHandle, ref rawDatas);
            //Thread.Sleep(1000);
            //stsResult = baseCommand.Config_PCU_Resp_5(m_PcanHandle, ref rawDatas);
            //Thread.Sleep(1000);
            //stsResult = baseCommand.Config_PCU_Resp_6(m_PcanHandle, ref rawDatas);
            //Thread.Sleep(1000);
            //await UpdateGUI(dgRawData, rawDatas);
            //rawDatas.Clear();

            GridFW.IsEnabled = true;
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
            try
            {
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
            catch (DllNotFoundException)
            {
                tbAlarmLog.Text += "PCANBasic.dll file Not Found.";
            }
        }

        private TPCANStatus BinProcess(TPCANHandle handle, string binTarget, byte[] binData)
        {
            TPCANStatus stsResult;

            stsResult = baseCommand.Session_ExDiagnotic(m_PcanHandle, ref rawDatas);
            UpdateGUI(dgRawData, rawDatas);
            rawDatas.Clear();

            stsResult = baseCommand.Config_PCU_Resp_1(m_PcanHandle, ref rawDatas);
            Thread.Sleep(1000);
            stsResult = baseCommand.Config_PCU_Resp_2(m_PcanHandle, ref rawDatas);
            Thread.Sleep(1000);
            stsResult = baseCommand.Config_PCU_Resp_3(m_PcanHandle, ref rawDatas);
            Thread.Sleep(1000);
            UpdateGUI(dgRawData, rawDatas);
            rawDatas.Clear();

            stsResult = baseCommand.Download_Request_1(m_PcanHandle, binTarget, ref rawDatas);
            UpdateGUI(dgRawData, rawDatas);
            rawDatas.Clear();

            stsResult = baseCommand.Download_Request_2(m_PcanHandle, binTarget, ref rawDatas, binData.Length);
            string[] temp = rawDatas[1][3].Split(" ");
            UpdateGUI(dgRawData, rawDatas);
            rawDatas.Clear();

            stsResult = baseCommand.BinTransmit(m_PcanHandle, temp[3] + temp[4], binData, ref rawDatas);
            UpdateGUI(dgRawData, rawDatas);
            rawDatas.Clear();

            stsResult = baseCommand.Transfer_End(m_PcanHandle, ref rawDatas);
            UpdateGUI(dgRawData, rawDatas);
            rawDatas.Clear();

            stsResult = baseCommand.PCU_Reset(m_PcanHandle, ref rawDatas);
            UpdateGUI(dgRawData, rawDatas);
            rawDatas.Clear();

            stsResult = baseCommand.Session_ExDiagnotic(m_PcanHandle, ref rawDatas);
            UpdateGUI(dgRawData, rawDatas);
            rawDatas.Clear();

            stsResult = baseCommand.Config_PCU_Resp_4(m_PcanHandle, ref rawDatas);
            Thread.Sleep(1000);
            stsResult = baseCommand.Config_PCU_Resp_5(m_PcanHandle, ref rawDatas);
            Thread.Sleep(1000);
            stsResult = baseCommand.Config_PCU_Resp_6(m_PcanHandle, ref rawDatas);
            Thread.Sleep(1000);
            UpdateGUI(dgRawData, rawDatas);
            rawDatas.Clear();

            return stsResult;
        }

        private void UpdateGUI(DataGrid dataGrid, List<string[]> rawData)
        //private async Task UpdateGUI(DataGrid dataGrid, List<string[]> rawData)
        {
            if (rawData.Count > 0)
            {
                foreach (string[] data in rawData)
                {
                    var tempData = new RawDataFormat { RawTime = data[0], RawID = data[1], RawLength = data[2], RawData = data[3] };

                    dataGrid.Dispatcher.Invoke(() =>
                    {
                       dataGrid.Items.Add(tempData);
                    }
                    );
                    //await Task.Delay(1);
                }
            }
        }
    }
}