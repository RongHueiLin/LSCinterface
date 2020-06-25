using System;
using Peak.Can.Basic;
using TPCANHandle = System.UInt16;

namespace RDinterface
{
    public class FrameRW
    {
        public TPCANStatus WriteFrame(TPCANHandle CanHandle,string writeID, string[] writeData)
        {
            //CREATE MESSAGE STRUCTURE
            TPCANMsg CANMsg = new TPCANMsg();
            CANMsg.DATA = new byte[8];

            //CONFIGURATE THE MESSAGE
            CANMsg.ID = Convert.ToUInt32(writeID, 16);
            CANMsg.LEN = Convert.ToByte(8);
            CANMsg.MSGTYPE = TPCANMessageType.PCAN_MESSAGE_STANDARD;

            //GET TRANSMIT DATA MESSAGE
            for (int i = 0; i < GetLengthFromDLC(CANMsg.LEN, true); i++)
            {
                CANMsg.DATA[i] = Convert.ToByte(writeData[i], 16);
            }

            //WRITE DATA TO PCAN
            return PCANBasic.Write(CanHandle, ref CANMsg);
        }

        public TPCANStatus ReadFrame(TPCANHandle CanHandle, ref string[] DataInfo)
        {
            TPCANMsg CANMsg;
            TPCANTimestamp CANTimeStamp;
            TPCANStatus stsResult;

            stsResult = PCANBasic.Read(CanHandle, out CANMsg, out CANTimeStamp);
            if (stsResult == TPCANStatus.PCAN_ERROR_OK)
            {
                DataInfo[0] = DateTime.Now.TimeOfDay.ToString();
                DataInfo[1] = Convert.ToString(CANMsg.ID, 16);
                DataInfo[2] = CANMsg.DATA.Length.ToString();
                DataInfo[3] = BitConverter.ToString(CANMsg.DATA).Replace("-", " ");
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
