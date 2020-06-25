using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Peak.Can.Basic;
using TPCANHandle = System.UInt16;

namespace RDinterface
{
    public class BaseCommand
    {
        public string Tx_ID = "300";

        public string Tx_ID_Config = "30F";

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
    
        public TPCANStatus Session_ExDiagnotic(TPCANHandle handle,ref string[]DataInfo)
        {
            TPCANStatus stsResult;
            FrameRW frame = new FrameRW();

            stsResult = frame.WriteFrame(handle, Tx_ID, cmdDiagnostic);
            if (stsResult == TPCANStatus.PCAN_ERROR_OK)
            {
                Thread.Sleep(10);
                stsResult = frame.ReadFrame(handle, ref DataInfo);
            }
            return stsResult;
        }

        public TPCANStatus Session_Program(TPCANHandle handle, ref string[] DataInfo)
        {
            TPCANStatus stsResult;
            FrameRW frame = new FrameRW();

            stsResult = frame.WriteFrame(handle, Tx_ID, cmdProgramming);
            if (stsResult == TPCANStatus.PCAN_ERROR_OK)
            {
                stsResult = frame.ReadFrame(handle, ref DataInfo);
            }
            return stsResult;
        }

        public TPCANStatus Transfer_End(TPCANHandle handle, ref string[] DataInfo)
        {
            TPCANStatus stsResult;
            FrameRW frame = new FrameRW();

            stsResult = frame.WriteFrame(handle, Tx_ID, cmdTransferEnd);
            if (stsResult == TPCANStatus.PCAN_ERROR_OK)
            {
                stsResult = frame.ReadFrame(handle, ref DataInfo);
            }
            return stsResult;
        }

        public TPCANStatus PCU_Reset(TPCANHandle handle, ref string[] DataInfo)
        {
            TPCANStatus stsResult;
            FrameRW frame = new FrameRW();

            stsResult = frame.WriteFrame(handle, Tx_ID, cmdPCUReset);
            if (stsResult == TPCANStatus.PCAN_ERROR_OK)
                stsResult = frame.ReadFrame(handle, ref DataInfo);

            return stsResult;
        }

        public TPCANStatus Download_Request_1(TPCANHandle handle)
        {
            TPCANStatus stsResult;
            FrameRW frame = new FrameRW();

            stsResult = frame.WriteFrame(handle, Tx_ID, cmdDownloadReq1);

            return stsResult;
        }

        public TPCANStatus Download_Request_2(TPCANHandle handle)
        {
            TPCANStatus stsResult;
            FrameRW frame = new FrameRW();

            stsResult = frame.WriteFrame(handle, Tx_ID, cmdDownloadReq2);

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
    }
}
