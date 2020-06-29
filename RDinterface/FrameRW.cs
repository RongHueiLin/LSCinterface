using System;
using System.Collections.Generic;
using System.Threading;
using Peak.Can.Basic;
using TPCANHandle = System.UInt16;

namespace RDinterface
{
    public class FrameRW
    {
        public TPCANStatus WriteFrame(TPCANHandle CanHandle,string writeID, string[] writeData, ref List<string[]> DataInfo)
        {
            //CREATE MESSAGE STRUCTURE
            TPCANMsg CANMsg = new TPCANMsg();
            CANMsg.DATA = new byte[8];
            string[] dataInfo = new string[4];

            //CONFIGURATE THE MESSAGE
            CANMsg.ID = Convert.ToUInt32(writeID, 16);
            CANMsg.LEN = Convert.ToByte(8);
            CANMsg.MSGTYPE = TPCANMessageType.PCAN_MESSAGE_STANDARD;

            //GET TRANSMIT DATA MESSAGE
            for (int i = 0; i < GetLengthFromDLC(CANMsg.LEN, true); i++)
            {
                CANMsg.DATA[i] = Convert.ToByte(writeData[i], 16);
            }

            dataInfo[0] = DateTime.Now.TimeOfDay.ToString();
            dataInfo[1] = Convert.ToString(CANMsg.ID, 16);
            dataInfo[2] = CANMsg.DATA.Length.ToString();
            dataInfo[3] = BitConverter.ToString(CANMsg.DATA).Replace("-", " ");
            DataInfo.Add(dataInfo);

            return PCANBasic.Write(CanHandle, ref CANMsg);
        }

        public TPCANStatus ReadFrame(TPCANHandle CanHandle, ref List<string[]> DataInfo)
        {
            TPCANMsg CANMsg;
            TPCANTimestamp CANTimeStamp;
            TPCANStatus stsResult;
            string[] dataInfo = new string[4];

            Thread.Sleep(10);   //WAIT 10 MILLISECOND

            stsResult = PCANBasic.Read(CanHandle, out CANMsg, out CANTimeStamp);
            if (stsResult == TPCANStatus.PCAN_ERROR_OK)
            {
                dataInfo[0] = DateTime.Now.TimeOfDay.ToString();
                dataInfo[1] = Convert.ToString(CANMsg.ID, 16);
                dataInfo[2] = CANMsg.DATA.Length.ToString();
                dataInfo[3] = BitConverter.ToString(CANMsg.DATA).Replace("-", " ");
                DataInfo.Add(dataInfo);
            }
            return stsResult;
        }

        private static int GetLengthFromDLC(int dlc, bool isSTD)
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
    }
}
