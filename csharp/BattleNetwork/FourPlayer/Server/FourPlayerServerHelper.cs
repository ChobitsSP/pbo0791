namespace PokemonBattle.FourPlayer.Server
{
    using NetworkLib.Utilities;
    using System;

    public class FourPlayerServerHelper
    {
        public static ByteArray Close()
        {
            ByteArray array = new ByteArray();
            array.WriteInt(-811482585);
            return array;
        }

        public static bool InterpretMessage(int sessionID, ByteArray byteArray, IFourPlayerServerService serverService)
        {
            byteArray.BypassHeader();
            switch (byteArray.ReadInt())
            {
                case -811482585:
                    return OnClose(sessionID, byteArray, serverService);

                case -357168322:
                    return OnStartBattle(sessionID, byteArray, serverService);

                case 0x5bbe67ef:
                    return OnLogon(sessionID, byteArray, serverService);

                case 0x7ec8b97b:
                    return OnSetPosition(sessionID, byteArray, serverService);
            }
            byteArray.Rewind();
            return false;
        }

        private static bool OnClose(int sessionID, ByteArray byteArray, IFourPlayerServerService serverService)
        {
            serverService.OnClose(sessionID);
            return true;
        }

        private static bool OnLogon(int sessionID, ByteArray byteArray, IFourPlayerServerService serverService)
        {
            int identity = byteArray.ReadInt();
            serverService.OnLogon(sessionID, identity);
            return true;
        }

        private static bool OnSetPosition(int sessionID, ByteArray byteArray, IFourPlayerServerService serverService)
        {
            byte position = byteArray.ReadByte();
            string player = byteArray.ReadUTF();
            serverService.OnSetPosition(sessionID, position, player);
            return true;
        }

        private static bool OnStartBattle(int sessionID, ByteArray byteArray, IFourPlayerServerService serverService)
        {
            serverService.OnStartBattle(sessionID);
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

        public static ByteArray SetPositionSuccess(byte position)
        {
            ByteArray array = new ByteArray();
            array.WriteInt(0x7df0feed);
            array.WriteByte(position);
            return array;
        }

        public static ByteArray StartBattle(int identity)
        {
            ByteArray array = new ByteArray();
            array.WriteInt(-357168322);
            array.WriteInt(identity);
            return array;
        }
    }
}

