namespace PokemonBattle.FourPlayer.Client
{
    using NetworkLib.Utilities;
    using System;

    public class FourPlayerClientHelper
    {
        public static ByteArray Close()
        {
            ByteArray array = new ByteArray();
            array.WriteInt(-811482585);
            return array;
        }

        public static bool InterpretMessage(int sessionID, ByteArray byteArray, IFourPlayerClientService clientService)
        {
            byteArray.BypassHeader();
            switch (byteArray.ReadInt())
            {
                case -811482585:
                    return OnClose(byteArray, clientService);

                case -357168322:
                    return OnStartBattle(byteArray, clientService);

                case 0x7df0feed:
                    return OnSetPositionSuccess(byteArray, clientService);

                case 0x7ec8b97b:
                    return OnSetPosition(byteArray, clientService);
            }
            byteArray.Rewind();
            return false;
        }

        public static ByteArray Logon(int identity)
        {
            ByteArray array = new ByteArray();
            array.WriteInt(0x5bbe67ef);
            array.WriteInt(identity);
            return array;
        }

        private static bool OnClose(ByteArray byteArray, IFourPlayerClientService clientService)
        {
            clientService.OnClose();
            return true;
        }

        private static bool OnSetPosition(ByteArray byteArray, IFourPlayerClientService clientService)
        {
            byte position = byteArray.ReadByte();
            string player = byteArray.ReadUTF();
            clientService.OnSetPosition(position, player);
            return true;
        }

        private static bool OnSetPositionSuccess(ByteArray byteArray, IFourPlayerClientService clientService)
        {
            byte position = byteArray.ReadByte();
            clientService.OnSetPositionSuccess(position);
            return true;
        }

        private static bool OnStartBattle(ByteArray byteArray, IFourPlayerClientService clientService)
        {
            int identity = byteArray.ReadInt();
            clientService.OnStartBattle(identity);
            return true;
        }

        public static ByteArray SetPosition(byte position, string player)
        {
            ByteArray array = new ByteArray();
            array.WriteInt(0x7ec8b97b);
            array.WriteByte(position);
            array.WriteUTF(player);
            return array;
        }

        public static ByteArray StartBattle()
        {
            ByteArray array = new ByteArray();
            array.WriteInt(-357168322);
            return array;
        }
    }
}

