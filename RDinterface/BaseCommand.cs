using System;
using System.Collections.Generic;
using System.Threading;
using Peak.Can.Basic;
using TPCANHandle = System.UInt16;

namespace RDinterface
{
    public class BaseCommand
    {
        public string Tx_ID = "300";
        public string Tx_ID_Config = "30F";
        private byte[] cmdFstFrame = new byte[] { 16, 0, 54, 0, 0, 0, 0, 0 };
        private byte[] cmdConseqFrame = new byte[] { 32, 0, 0, 0, 0, 0, 0, 0 };
        public readonly byte[] cmdDiagnostic = new byte[] { 2, 16, 3, 0, 0, 0, 0, 0 };
        public readonly byte[] cmdConfigPCU1 = new byte[] { 2, 16, 131, 0, 0, 0, 0, 0 };
        public readonly byte[] cmdConfigPCU2 = new byte[] { 2, 133, 130, 0, 0, 0, 0, 0 };
        public readonly byte[] cmdConfigPCU3 = new byte[] { 3, 40, 130, 3, 0, 0, 0, 0 };
        public readonly byte[] cmdConfigPCU4 = new byte[] { 3, 40,128, 3, 0, 0, 0, 0 };
        public readonly byte[] cmdConfigPCU5 = new byte[] { 2, 133, 129, 0, 0, 0, 0, 0 };
        public readonly byte[] cmdConfigPCU6 = new byte[] { 2, 16, 129, 0, 0, 0, 0, 0 };
        public readonly byte[] cmdProgramming = new byte[] { 2, 16, 2, 0, 0, 0, 0, 0 };
        public readonly byte[] cmdDownloadReq1 = new byte[] { 16, 11, 52, 0, 68, 0, 0, 0 };
        public readonly byte[] cmdDownloadReq2 = new byte[] { 33, 0, 0, 0, 0, 0, 0, 0 };
        public readonly byte[] cmdTransferEnd = new byte[] { 5, 55, 152, 244, 204, 51, 0, 0 };
        public readonly byte[] cmdPCUReset = new byte[] { 2, 17, 1, 0, 0, 0, 0, 0 };
        public readonly byte[] AddressBinA = new byte[] { 8, 4, 0, 0 };
        public readonly byte[] AddressBinS = new byte[] { 8, 0, 136, 0 };
        public readonly byte[] AddressBinP = new byte[] { 8, 0, 168, 0 };
        public readonly byte[] AddressBinM1 = new byte[] { 8, 0, 200, 0 };
        public readonly byte[] AddressBinM2 = new byte[] { 8, 1, 24, 0 };
        public DataFormat Tx { get; set; }
        public DataFormat Rx { get; set; }
        public string Status { get{ return _status; } }
        public string DelayTime { set { _delayTime = Convert.ToInt32(value); } }
        private string _status;
        private int _delayTime;

        public BaseCommand()
        {
            this._delayTime = 1;
            this.Tx = new DataFormat();
            this.Rx = new DataFormat();
        }

        /// <summary>
        /// TRANSMIT DATA AND GETTING RESPONSE
        /// </summary>
        /// <param name="handle">PCAN HANDLE</param>
        /// <param name="id">PCAN TRANSMIT ID</param>
        /// <param name="command">PCAN TRANSMIT DATA</param>
        /// <param name="timeout">TIMEOUT FOR RESPONSE</param>
        /// <returns></returns>
        public TPCANStatus TransmitNormal(TPCANHandle handle, string id, byte[] command,int timeout)
        {
            TPCANStatus stsResult;
            bool getR = false;
            FrameRW frame = new FrameRW();

            Thread.Sleep(_delayTime);

            do { stsResult = frame.ReadFrame(handle); }
            while (stsResult == TPCANStatus.PCAN_ERROR_OK);

            this.Tx.Time = DateTime.Now.TimeOfDay.ToString();
            this.Tx.ID = id;
            this.Tx.Length = command.Length.ToString();
            this.Tx.Data = BitConverter.ToString(command).Replace("-", " ");

            this._status = "Write";
            stsResult = frame.WriteFrame(handle, id, command);

            if (stsResult == TPCANStatus.PCAN_ERROR_OK)
            {
                while (timeout > 0)
                {
                    Thread.Sleep(1);
                    
                    this._status = "Read";
                    stsResult = frame.ReadFrame(handle);

                    timeout--;

                    if (stsResult == TPCANStatus.PCAN_ERROR_OK || stsResult == TPCANStatus.PCAN_ERROR_RESPONSE)
                    {
                        this.Rx = frame.ReturnData;
                        getR = true;

                        if(stsResult == TPCANStatus.PCAN_ERROR_RESPONSE)
                            break;
                    }
                    else if(stsResult == TPCANStatus.PCAN_ERROR_QRCVEMPTY)
                    {
                        if(getR)
                        {
                            stsResult = TPCANStatus.PCAN_ERROR_OK;
                            break;
                        }
                    }
                }
            }
            return stsResult;
        }

        /// <summary>
        /// TRANSMIT DATA WITHOUT GETTING RESPONSE
        /// </summary>
        /// <param name="handle">PCAN HANDLE</param>
        /// <param name="id">PCAN TRANSMIT ID</param>
        /// <param name="command">PCAN TRANSMIT DATA</param>
        /// <returns></returns>
        public TPCANStatus TransmitOnly(TPCANHandle handle, string id, byte[] command)
        {
            TPCANStatus stsResult;
            FrameRW frame = new FrameRW();

            Thread.Sleep(_delayTime);

            this.Tx.Time = DateTime.Now.TimeOfDay.ToString();
            this.Tx.ID = id;
            this.Tx.Length = command.Length.ToString();
            this.Tx.Data = BitConverter.ToString(command).Replace("-", " ");

            this._status = "Write";
            stsResult = frame.WriteFrame(handle, id, command);

            return stsResult;
        }

        /// <summary>
        /// FORMAT RAW BIN FILE DATA BEFORE TRANSMIT
        /// </summary>
        /// <param name="BlockSize">TRANSMIT BLOCK SIZE</param>
        /// <param name="BinData">RAW BIN DATA</param>
        /// <returns></returns>
        public List<byte[]> BinFormat(int BlockSize, byte[] BinData)
        {
            int FFdataSize = 4;
            int CFdataSize = 7;
            List<byte[]> BFrame = new List<byte[]>();   //BLOCK DATA BUFFER
            List<byte[]> allFrame = new List<byte[]>(); //FRAME DATA BUFFER
            int blockCount = BinData.Length / BlockSize;    //NUMBER OF FULL BLOCK
            int LstBlockSize = BinData.Length % BlockSize;  //LAST BLOCK DATA SIZE

            //SPLIT BIN DATA BY BLOCK SIZE
            for (int i = 0; i < blockCount; i++)
            {
                BFrame.Add(new byte[BlockSize]);
                Array.Copy(BinData, 0 + i * BlockSize, BFrame[i], 0, BlockSize);
            }

            if (LstBlockSize != 0)
            {
                BFrame.Add(new byte[LstBlockSize]);
                Array.Copy(BinData, BlockSize * blockCount, BFrame[blockCount], 0, LstBlockSize);
            }

            //SPLIT BLOCK DATA INTO FRAME
            for (int i = 0; i < BFrame.Count; i++)
            {
                string curBlockSize = Convert.ToString(BFrame[i].Length, 16);            //CURRENT BLOCK SIZE   
                int LstdataSize = (BFrame[i].Length - FFdataSize) % CFdataSize;         //LAST FRAME DATA SIZE
                string[] sftBlock = new string[BFrame[i].Length + 3];                   //BLOCK DATA BUFFER SHIFTING

                //CALCULATE AMOUNT OF FRAME
                int frameCount = ((BFrame[i].Length - FFdataSize) / CFdataSize) + 1;
                if (LstdataSize != 0)
                    frameCount += 1;

                //FORMAT EACH FRAME
                for (int m = 0; m < frameCount; m++)
                {
                    int frameIndex = m % 16;

                    //FIRST FRAME
                    if (m == 0)
                    {
                        allFrame.Add(new byte[8] { 16, 0, 54, 0, 0, 0, 0, 0 });

                        //ADD FIRST BLOCK DATA
                        Array.Copy(BFrame[i], 0, allFrame[allFrame.Count - 1], 8 - FFdataSize, FFdataSize);

                        //ADD BLOCK SIZE
                        allFrame[allFrame.Count - 1][0] += (byte)((BFrame[i].Length >> 8) & 0xFF);
                        allFrame[allFrame.Count - 1][1] = (byte)(BFrame[i].Length & 0xFF);

                        //ADD BLOCK SEQUNCE COUNTER
                        allFrame[allFrame.Count - 1][3] = (byte)(i + 1);
                    }
                    //LAST FRAME
                    else if (m == frameCount - 1)
                    {
                        allFrame.Add(new byte[8] { 32, 0, 0, 0, 0, 0, 0, 0 });
                        Array.Copy(BFrame[i], FFdataSize + (CFdataSize * (m - 1)), allFrame[allFrame.Count - 1], 1, LstdataSize);
                        allFrame[allFrame.Count - 1][0] += (byte)frameIndex;
                    }
                    //OTHER FRAME
                    else
                    {
                        allFrame.Add(new byte[8] { 32, 0, 0, 0, 0, 0, 0, 0 });
                        Array.Copy(BFrame[i], FFdataSize + (CFdataSize * (m - 1)), allFrame[allFrame.Count - 1], 1, 7);
                        allFrame[allFrame.Count - 1][0] += (byte)frameIndex;
                    }
                }
            }
            return allFrame;
        }

        /// <summary>
        /// GET ADDRESS OF DIFFERENT BIN FILE TYPE
        /// </summary>
        /// <param name="binTarget">BIN FILE TYPE</param>
        /// <returns></returns>
        public byte[] GetAddress(string binTarget)
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

        public void FCdelay(string FC)
        {
            if(FC != null && FC != "")
            {
                string[] fc = FC.Split(" ");
                
                if(this._delayTime < Convert.ToInt32(fc[2]))
                    this._delayTime = Convert.ToInt32(fc[2]);
            }
        }
    }
}
