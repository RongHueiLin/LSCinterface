using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Peak.Can.Basic;
using TPCANHandle = System.UInt16;

namespace RDinterface
{
    public class BaseCommand
    {
        public string Tx_ID = "300";

        public string Tx_ID_Config = "30F";

        public string[] cmdFstFrame = new string[] { "1n", "nn", "36", "00", "00", "00", "00","00" };

        public string[] cmdConseqFrame = new string[] { "2n", "00", "00", "00", "00", "00", "00", "00" };

        public string[] cmdDiagnostic = new string[] { "02", "10", "03", "00", "00", "00", "00", "00" };

        public string[] cmdConfigPCU1 = new string[] { "02", "10", "83", "00", "00", "00", "00", "00" };

        public string[] cmdConfigPCU2 = new string[] { "02", "85", "82", "00", "00", "00", "00", "00" };

        public string[] cmdConfigPCU3 = new string[] { "03", "28", "82", "03", "00", "00", "00", "00" };

        public string[] cmdConfigPCU4 = new string[] { "03", "28", "80", "03", "00", "00", "00", "00" };

        public string[] cmdConfigPCU5 = new string[] { "02", "85", "81", "00", "00", "00", "00", "00" };

        public string[] cmdConfigPCU6 = new string[] { "02", "10", "81", "00", "00", "00", "00", "00" };

        public string[] cmdProgramming = new string[] { "02", "10", "02", "00", "00", "00", "00", "00" };

        public string[] cmdDownloadReq1 = new string[] { "10", "0B", "34", "00", "44", "00", "08", "00" };

        public string[] cmdDownloadReq2 = new string[] { "21", "00", "00", "20", "00", "00", "00", "00" };

        public string[] cmdTransferEnd = new string[] { "05", "37", "98", "F4", "CC", "33", "00", "00" };

        public string[] cmdPCUReset = new string[] { "02", "11", "01", "00", "00", "00", "00", "00" };
    
        public TPCANStatus Session_ExDiagnotic(TPCANHandle handle,ref List<string[]> DataInfo)
        {
            TPCANStatus stsResult;
            FrameRW frame = new FrameRW();

            stsResult = frame.WriteFrame(handle, Tx_ID, cmdDiagnostic, ref DataInfo);
            if (stsResult == TPCANStatus.PCAN_ERROR_OK)
            {
                stsResult = frame.ReadFrame(handle, ref DataInfo);
            }
            return stsResult;
        }

        public TPCANStatus Session_Program(TPCANHandle handle, ref List<string[]> DataInfo)
        {
            TPCANStatus stsResult;
            FrameRW frame = new FrameRW();

            stsResult = frame.WriteFrame(handle, Tx_ID, cmdProgramming, ref DataInfo);
            if (stsResult == TPCANStatus.PCAN_ERROR_OK)
            {
                stsResult = frame.ReadFrame(handle, ref DataInfo);
            }
            return stsResult;
        }

        public TPCANStatus Transfer_End(TPCANHandle handle, ref List<string[]> DataInfo)
        {
            TPCANStatus stsResult;
            FrameRW frame = new FrameRW();

            stsResult = frame.WriteFrame(handle, Tx_ID, cmdTransferEnd, ref DataInfo);
            if (stsResult == TPCANStatus.PCAN_ERROR_OK)
            {
                stsResult = frame.ReadFrame(handle, ref DataInfo);
            }
            return stsResult;
        }

        public TPCANStatus PCU_Reset(TPCANHandle handle, ref List<string[]> DataInfo)
        {
            TPCANStatus stsResult;
            FrameRW frame = new FrameRW();

            stsResult = frame.WriteFrame(handle, Tx_ID, cmdPCUReset, ref DataInfo);
            if (stsResult == TPCANStatus.PCAN_ERROR_OK)
            {
                stsResult = frame.ReadFrame(handle, ref DataInfo);
            }
            return stsResult;
        }

        public TPCANStatus Download_Request_1(TPCANHandle handle, ref List<string[]> DataInfo)
        {
            TPCANStatus stsResult;
            FrameRW frame = new FrameRW();

            stsResult = frame.WriteFrame(handle, Tx_ID, cmdDownloadReq1, ref DataInfo);
            if (stsResult == TPCANStatus.PCAN_ERROR_OK)
            {
                stsResult = frame.ReadFrame(handle, ref DataInfo);
            }
            return stsResult;
        }

        public TPCANStatus Download_Request_2(TPCANHandle handle, ref List<string[]> DataInfo)
        {
            TPCANStatus stsResult;
            FrameRW frame = new FrameRW();

            stsResult = frame.WriteFrame(handle, Tx_ID, cmdDownloadReq2, ref DataInfo);
            if (stsResult == TPCANStatus.PCAN_ERROR_OK)
            {
                stsResult = frame.ReadFrame(handle, ref DataInfo);
            }
            return stsResult;
        }

        public TPCANStatus Config_PCU_Resp_1(TPCANHandle handle, ref List<string[]> DataInfo)
        {
            TPCANStatus stsResult;
            FrameRW frame = new FrameRW();

            stsResult = frame.WriteFrame(handle, Tx_ID_Config, cmdConfigPCU1, ref DataInfo);

            return stsResult;
        }

        public TPCANStatus Config_PCU_Resp_2(TPCANHandle handle, ref List<string[]> DataInfo)
        {
            TPCANStatus stsResult;
            FrameRW frame = new FrameRW();

            stsResult = frame.WriteFrame(handle, Tx_ID_Config, cmdConfigPCU2, ref DataInfo);

            return stsResult;
        }

        public TPCANStatus Config_PCU_Resp_3(TPCANHandle handle, ref List<string[]> DataInfo)
        {
            TPCANStatus stsResult;
            FrameRW frame = new FrameRW();

            stsResult = frame.WriteFrame(handle, Tx_ID_Config, cmdConfigPCU3, ref DataInfo);

            return stsResult;
        }

        public TPCANStatus Config_PCU_Resp_4(TPCANHandle handle, ref List<string[]> DataInfo)
        {
            TPCANStatus stsResult;
            FrameRW frame = new FrameRW();

            stsResult = frame.WriteFrame(handle, Tx_ID_Config, cmdConfigPCU4, ref DataInfo);

            return stsResult;
        }

        public TPCANStatus Config_PCU_Resp_5(TPCANHandle handle, ref List<string[]> DataInfo)
        {
            TPCANStatus stsResult;
            FrameRW frame = new FrameRW();

            stsResult = frame.WriteFrame(handle, Tx_ID_Config, cmdConfigPCU5, ref DataInfo);

            return stsResult;
        }

        public TPCANStatus Config_PCU_Resp_6(TPCANHandle handle, ref List<string[]> DataInfo)
        {
            TPCANStatus stsResult;
            FrameRW frame = new FrameRW();
            stsResult = frame.WriteFrame(handle, Tx_ID_Config, cmdConfigPCU6, ref DataInfo);

            return stsResult;
        }
    
        public TPCANStatus BinTransmit(TPCANHandle handle,string BlockSize,string[] BinData, ref List<string[]> DataInfo)
        {
            TPCANStatus stsResult = 0;
            FrameRW frameRW = new FrameRW();
            int FFdataSize = 5;
            int CFdataSize = 7;
            List<string[]> BFrame = new List<string[]>();   //BLOCK DATA BUFFER
            List<string[]> allFrame = new List<string[]>(); //FRAME DATA BUFFER
            int blockSize = Convert.ToInt32(BlockSize, 16); //MAX SIZE OF A BLOCK
            int blockCount = BinData.Length / blockSize;    //NUMBER OF FULL BLOCK
            int LstBlockSize = BinData.Length % blockSize;  //LAST BLOCK DATA SIZE

            //SPLIT BIN DATA BY BLOCK SIZE
            for (int i = 0; i < blockCount; i++)
            {
                BFrame.Add(new string[blockSize]);
                Array.Copy(BinData, 0 + i * blockSize, BFrame[i], 0, blockSize);
            }
            if (LstBlockSize != 0)
            {
                BFrame.Add(new string[LstBlockSize]);
                Array.Copy(BinData, blockSize * blockCount, BFrame[blockCount], 0, LstBlockSize);
            }

            // SPLIT BLOCK DATA INTO FRAME
            for (int i = 0; i < BFrame.Count; i++)
            {
                string curBlockSize = Convert.ToString(BFrame[i].Length,16);            //CURRENT BLOCK SIZE   
                int LstdataSize = (BFrame[i].Length - FFdataSize) % CFdataSize;         //LAST FRAME DATA SIZE
                int frameCount = ((BFrame[i].Length - FFdataSize) / CFdataSize) + 2;    //TOTAL FRAME COUNT
                string[] sftBlock = new string[BFrame[i].Length + 3];                   //BLOCK DATA BUFFER SHIFTING

                //SHIFT BLOCK DATA OF FIRST THREE BYTES
                for (int j = 0; j < sftBlock.Length; j++)
                {
                    if (j < 3)
                        sftBlock[j] = "";
                    else
                        sftBlock[j] = BFrame[i][j - 3];
                }

                for(int m = 0; m < frameCount; m++)
                {
                    string frameIndex = Convert.ToString(m % 16, 16);

                    //FIRST FRAME
                    if (m == 0)
                    {
                        allFrame.Add(new string[8] { "1n", "nn", "36", "00", "00", "00", "00", "00" });
                        Array.Copy(sftBlock, 3, allFrame[allFrame.Count - 1], 3, 5);

                        if (curBlockSize.Length > 2)
                        {
                            allFrame[allFrame.Count - 1][0] = allFrame[allFrame.Count - 1][0].Replace("n", curBlockSize.Substring(0, 1));
                            allFrame[allFrame.Count - 1][1] = allFrame[allFrame.Count - 1][1].Replace("nn", curBlockSize.Substring(1, 2));
                        }
                        else
                        {
                            allFrame[allFrame.Count - 1][0] = allFrame[allFrame.Count - 1][0].Replace("n", "0");
                            allFrame[allFrame.Count - 1][1] = allFrame[allFrame.Count - 1][1].Replace("nn", curBlockSize);
                        }
                    }
                    //LAST FRAME
                    else if (m == frameCount - 1)
                    {
                        allFrame.Add(new string[8] { "2n", "00", "00", "00", "00", "00", "00", "00" });
                        Array.Copy(sftBlock, 8 + (7 * (m - 1)), allFrame[allFrame.Count - 1], 1, LstdataSize);
                        allFrame[allFrame.Count - 1][0] = allFrame[allFrame.Count - 1][0].Replace("n", frameIndex);
                    }
                    //OTHER FRAME
                    else
                    {
                        allFrame.Add(new string[8] { "2n", "00", "00", "00", "00", "00", "00", "00" });
                        Array.Copy(sftBlock, 8 + (7 * (m - 1)), allFrame[allFrame.Count - 1], 1, 7);
                        allFrame[allFrame.Count - 1][0] = allFrame[allFrame.Count - 1][0].Replace("n", frameIndex);
                    }
                }
            }

            //TRANSMIT BIN FILE DATA
            foreach (string[] frame in allFrame)
            {
                stsResult = frameRW.WriteFrame(handle, Tx_ID, frame, ref DataInfo);
                if (stsResult == TPCANStatus.PCAN_ERROR_OK)
                {
                    stsResult = frameRW.ReadFrame(handle, ref DataInfo);
                }
                else return stsResult;
            }
            return stsResult;
        }
    }
}
