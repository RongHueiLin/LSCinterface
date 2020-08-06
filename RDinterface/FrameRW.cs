using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
using System.Threading.Tasks;
using Peak.Can.Basic;
using TPCANHandle = System.UInt16;

namespace RDinterface
{
    public class FrameRW
    {
        public TPCANStatus CanStatus { get; }

        public DataFormat ReturnData { get; }

        public FrameRW()
        {
            ReturnData = new DataFormat();
        }

        public TPCANStatus WriteFrame(TPCANHandle CanHandle,string writeID, byte[] writeData)
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
                CANMsg.DATA[i] = writeData[i];
            }

            return PCANBasic.Write(CanHandle, ref CANMsg);
        }

        public TPCANStatus ReadFrame(TPCANHandle CanHandle)
        {
            //CREATE MESSAGE STRUCTURE
            TPCANMsg CANMsg;
            TPCANTimestamp CANTimeStamp;
            TPCANStatus stsResult;

            //READ RETURN DATA
            stsResult = PCANBasic.Read(CanHandle, out CANMsg, out CANTimeStamp);
            
            //IF READ SUCCESS CHANGE PROPERTY, ELSE IF RETURN ERROR
            if (stsResult == TPCANStatus.PCAN_ERROR_OK)
            {
                this.ReturnData.Time = DateTime.Now.TimeOfDay.ToString();
                this.ReturnData.ID = Convert.ToString(CANMsg.ID, 16);
                this.ReturnData.Length = CANMsg.DATA.Length.ToString();
                this.ReturnData.Data = BitConverter.ToString(CANMsg.DATA).Replace("-", " ");

                if (CANMsg.DATA[0] == 7f || CANMsg.DATA[1] == 7f)
                    stsResult = TPCANStatus.PCAN_ERROR_RESPONSE;
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
