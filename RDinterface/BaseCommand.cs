using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Peak.Can.Basic;
using TPCANHandle = System.UInt16;

namespace RDinterface
{
    public class BaseCommand
    {
        private string Tx_ID = "300";

        private string Tx_ID_Config = "30F";

        private byte[] cmdFstFrame = new byte[] { 16, 0, 54, 0, 0, 0, 0, 0 };

        private byte[] cmdConseqFrame = new byte[] { 32, 0, 0, 0, 0, 0, 0, 0 };

        private byte[] cmdDiagnostic = new byte[] { 2, 16, 3, 0, 0, 0, 0, 0 };

        private byte[] cmdConfigPCU1 = new byte[] { 2, 16, 131, 0, 0, 0, 0, 0 };

        private byte[] cmdConfigPCU2 = new byte[] { 2, 133, 130, 0, 0, 0, 0, 0 };

        private byte[] cmdConfigPCU3 = new byte[] { 3, 40, 130, 3, 0, 0, 0, 0 };

        private byte[] cmdConfigPCU4 = new byte[] { 3, 40,128, 3, 0, 0, 0, 0 };

        private byte[] cmdConfigPCU5 = new byte[] { 2, 133, 129, 0, 0, 0, 0, 0 };

        private byte[] cmdConfigPCU6 = new byte[] { 2, 16, 129, 0, 0, 0, 0, 0 };

        private byte[] cmdProgramming = new byte[] { 2, 16, 2, 0, 0, 0, 0, 0 };

        private byte[] cmdDownloadReq1 = new byte[] { 16, 11, 52, 0, 68, 0, 0, 0 };

        private byte[] cmdDownloadReq2 = new byte[] { 33, 0, 0, 0, 0, 0, 0, 0 };

        private byte[] cmdTransferEnd = new byte[] { 5, 55, 152, 244, 204, 51, 0, 0 };

        private byte[] cmdPCUReset = new byte[] { 2, 17, 1, 0, 0, 0, 0, 0 };

        private byte[] AddressBinA = new byte[] { 8, 4, 0, 0 };

        private byte[] AddressBinS = new byte[] { 8, 0, 136, 0 };

        private byte[] AddressBinP = new byte[] { 8, 0, 168, 0 };

        private byte[] AddressBinM1 = new byte[] { 8, 0, 200, 0 };

        private byte[] AddressBinM2 = new byte[] { 8, 1, 24, 0 };

        public ReturnData BaseCmd { get; set; }

        private int _delayTime = 10;

        public string DelayTime { set { _delayTime = Convert.ToInt32(value); } }

        public BaseCommand()
        {
            BaseCmd = new ReturnData();
        }

        public TPCANStatus Session_ExDiagnotic(TPCANHandle handle)
        {
            TPCANStatus stsResult;
            FrameRW frame = new FrameRW();

            stsResult = frame.WriteFrame(handle, Tx_ID, cmdDiagnostic);

            Thread.Sleep(_delayTime);

            if (stsResult == TPCANStatus.PCAN_ERROR_OK)
            {
                stsResult = frame.ReadFrame(handle);

                if (stsResult == TPCANStatus.PCAN_ERROR_OK)
                    this.BaseCmd = frame.ReturnData;
            }
            return stsResult;
        }

        public TPCANStatus Session_Program(TPCANHandle handle)
        {
            TPCANStatus stsResult;
            FrameRW frame = new FrameRW();

            stsResult = frame.WriteFrame(handle, Tx_ID, cmdProgramming);

            Thread.Sleep(_delayTime);

            if (stsResult == TPCANStatus.PCAN_ERROR_OK)
            {
                stsResult = frame.ReadFrame(handle);

                if (stsResult == TPCANStatus.PCAN_ERROR_OK)
                    this.BaseCmd = frame.ReturnData;
            }
            return stsResult;
        }

        public TPCANStatus Transfer_End(TPCANHandle handle)
        {
            TPCANStatus stsResult;
            FrameRW frame = new FrameRW();

            stsResult = frame.WriteFrame(handle, Tx_ID, cmdTransferEnd);

            Thread.Sleep(_delayTime);

            if (stsResult == TPCANStatus.PCAN_ERROR_OK)
            {
                stsResult = frame.ReadFrame(handle);

                if (stsResult == TPCANStatus.PCAN_ERROR_OK)
                    this.BaseCmd = frame.ReturnData;
            }
            return stsResult;
        }

        public TPCANStatus PCU_Reset(TPCANHandle handle)
        {
            TPCANStatus stsResult;
            FrameRW frame = new FrameRW();

            stsResult = frame.WriteFrame(handle, Tx_ID, cmdPCUReset);

            Thread.Sleep(_delayTime);

            if (stsResult == TPCANStatus.PCAN_ERROR_OK)
            {
                stsResult = frame.ReadFrame(handle);

                if (stsResult == TPCANStatus.PCAN_ERROR_OK)
                    this.BaseCmd = frame.ReturnData;
            }
            return stsResult;
        }

        public TPCANStatus Download_Request_1(TPCANHandle handle, string binTarget)
        {
            TPCANStatus stsResult;
            byte[] address = new byte[4];
            FrameRW frame = new FrameRW();

            address = GetAddress(binTarget);

            //REPLACE BIN TYPE ADDRESS INTO COMMAND
            Array.Copy(address, 0, cmdDownloadReq1, 5, 3);

            stsResult = frame.WriteFrame(handle, Tx_ID, cmdDownloadReq1);

            Thread.Sleep(_delayTime);

            if (stsResult == TPCANStatus.PCAN_ERROR_OK)
            {
                stsResult = frame.ReadFrame(handle);

                if (stsResult == TPCANStatus.PCAN_ERROR_OK)
                    this.BaseCmd = frame.ReturnData;
            }
            return stsResult;
        }

        public TPCANStatus Download_Request_2(TPCANHandle handle, string binTarget, int binSize)
        {
            TPCANStatus stsResult;
            byte[] address = new byte[4];
            FrameRW frame = new FrameRW();

            address = GetAddress(binTarget);

            byte[] bSize = BitConverter.GetBytes(binSize);
            Array.Reverse(bSize);
            Array.Copy(bSize, 0, cmdDownloadReq2, 2, bSize.Length);
            Array.Copy(address, 3, cmdDownloadReq2, 1, 1);

            stsResult = frame.WriteFrame(handle, Tx_ID, cmdDownloadReq2);

            Thread.Sleep(_delayTime);

            if (stsResult == TPCANStatus.PCAN_ERROR_OK)
            {
                stsResult = frame.ReadFrame(handle);

                if (stsResult == TPCANStatus.PCAN_ERROR_OK)
                    this.BaseCmd = frame.ReturnData;
            }
            return stsResult;
        }

        public TPCANStatus Config_PCU_Resp_1(TPCANHandle handle)
        {
            TPCANStatus stsResult;
            FrameRW frame = new FrameRW();

            stsResult = frame.WriteFrame(handle, Tx_ID_Config, cmdConfigPCU1);

            return stsResult;
        }

        public TPCANStatus Config_PCU_Resp_2(TPCANHandle handle)
        {
            TPCANStatus stsResult;
            FrameRW frame = new FrameRW();

            stsResult = frame.WriteFrame(handle, Tx_ID_Config, cmdConfigPCU2);

            return stsResult;
        }

        public TPCANStatus Config_PCU_Resp_3(TPCANHandle handle)
        {
            TPCANStatus stsResult;
            FrameRW frame = new FrameRW();

            stsResult = frame.WriteFrame(handle, Tx_ID_Config, cmdConfigPCU3);

            return stsResult;
        }

        public TPCANStatus Config_PCU_Resp_4(TPCANHandle handle)
        {
            TPCANStatus stsResult;
            FrameRW frame = new FrameRW();

            stsResult = frame.WriteFrame(handle, Tx_ID_Config, cmdConfigPCU4);

            return stsResult;
        }

        public TPCANStatus Config_PCU_Resp_5(TPCANHandle handle)
        {
            TPCANStatus stsResult;
            FrameRW frame = new FrameRW();

            stsResult = frame.WriteFrame(handle, Tx_ID_Config, cmdConfigPCU5);

            return stsResult;
        }

        public TPCANStatus Config_PCU_Resp_6(TPCANHandle handle)
        {
            TPCANStatus stsResult;
            FrameRW frame = new FrameRW();
            stsResult = frame.WriteFrame(handle, Tx_ID_Config, cmdConfigPCU6);

            return stsResult;
        }

        //public async Task<List<string[]>> BinTransmit(TPCANHandle handle, string BlockSize, byte[] BinData)
        //{
        //    TPCANStatus stsResult = 0;
        //    FrameRW frameRW = new FrameRW();
        //    int FFdataSize = 4;
        //    int CFdataSize = 7;
        //    List<byte[]> BFrame = new List<byte[]>();   //BLOCK DATA BUFFER
        //    List<byte[]> allFrame = new List<byte[]>(); //FRAME DATA BUFFER
        //    int blockSize = Convert.ToInt32(BlockSize, 16); //MAX SIZE OF A BLOCK
        //    int blockCount = BinData.Length / blockSize;    //NUMBER OF FULL BLOCK
        //    int LstBlockSize = BinData.Length % blockSize;  //LAST BLOCK DATA SIZE

        //    //SPLIT BIN DATA BY BLOCK SIZE
        //    for (int i = 0; i < blockCount; i++)
        //    {
        //        BFrame.Add(new byte[blockSize]);
        //        Array.Copy(BinData, 0 + i * blockSize, BFrame[i], 0, blockSize);
        //    }
        //    if (LstBlockSize != 0)
        //    {
        //        BFrame.Add(new byte[LstBlockSize]);
        //        Array.Copy(BinData, blockSize * blockCount, BFrame[blockCount], 0, LstBlockSize);
        //    }

        //    // SPLIT BLOCK DATA INTO FRAME
        //    for (int i = 0; i < BFrame.Count; i++)
        //    {
        //        string curBlockSize = Convert.ToString(BFrame[i].Length, 16);            //CURRENT BLOCK SIZE   
        //        int LstdataSize = (BFrame[i].Length - FFdataSize) % CFdataSize;         //LAST FRAME DATA SIZE
        //        string[] sftBlock = new string[BFrame[i].Length + 3];                   //BLOCK DATA BUFFER SHIFTING

        //        //CALCULATE AMOUNT OF FRAME
        //        int frameCount = ((BFrame[i].Length - FFdataSize) / CFdataSize) + 1;
        //        if (LstdataSize != 0)
        //            frameCount += 1;

        //        //FORMAT EACH FRAME
        //        for (int m = 0; m < frameCount; m++)
        //        {
        //            int frameIndex = m % 16;

        //            //FIRST FRAME
        //            if (m == 0)
        //            {
        //                allFrame.Add(new byte[8] { 16, 0, 54, 0, 0, 0, 0, 0 });

        //                //ADD FIRST BLOCK DATA
        //                Array.Copy(BFrame[i], 0, allFrame[allFrame.Count - 1], 8 - FFdataSize, FFdataSize);

        //                //ADD BLOCK SIZE
        //                allFrame[allFrame.Count - 1][0] += (byte)((BFrame[i].Length >> 8) & 0xFF);
        //                allFrame[allFrame.Count - 1][1] = (byte)(BFrame[i].Length & 0xFF);

        //                //ADD BLOCK SEQUNCE COUNTER
        //                allFrame[allFrame.Count - 1][3] = (byte)(i + 1);
        //            }
        //            //LAST FRAME
        //            else if (m == frameCount - 1)
        //            {
        //                allFrame.Add(new byte[8] { 32, 0, 0, 0, 0, 0, 0, 0 });
        //                Array.Copy(BFrame[i], FFdataSize + (CFdataSize * (m - 1)), allFrame[allFrame.Count - 1], 1, LstdataSize);
        //                allFrame[allFrame.Count - 1][0] += (byte)frameIndex;
        //            }
        //            //OTHER FRAME
        //            else
        //            {
        //                allFrame.Add(new byte[8] { 32, 0, 0, 0, 0, 0, 0, 0 });
        //                Array.Copy(BFrame[i], FFdataSize + (CFdataSize * (m - 1)), allFrame[allFrame.Count - 1], 1, 7);
        //                allFrame[allFrame.Count - 1][0] += (byte)frameIndex;
        //            }
        //        }
        //    }

        //    //TRANSMIT BIN FILE DATA
        //    foreach (byte[] frame in allFrame)
        //    {
        //        stsResult = frameRW.WriteFrame(handle, Tx_ID, frame);

        //        if (stsResult == TPCANStatus.PCAN_ERROR_OK)
        //        {
        //            await frameRW.ReadFrame(handle);
        //        }
        //        else return stsResult;
        //    }
        //    return stsResult;
        //}

        private byte[] GetAddress(string binTarget)
        {
            byte[] address = new byte[4];

            switch(binTarget)
            {
                case ("A"):
                    address = AddressBinA;
                    break;
                case ("S"):
                    address = AddressBinS;
                    break;
                case ("P"):
                    address = AddressBinP;
                    break;
                case ("M1"):
                    address = AddressBinM1;
                    break;
                case ("M2"):
                    address = AddressBinM2;
                    break;
            }
            return address;
        }
    }
}
