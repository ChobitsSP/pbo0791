namespace PokemonBattle.BattleNetwork
{
    using NetworkLib.Utilities;
    using System;

    public class BattleInfo
    {
        public string Caption;
        public string CustomDataHash;
        public BattleMode Mode;
        public BattleTerrain Terrain;

        public void ReadFromByteArray(ByteArray byteArray)
        {
            this.Terrain = BattleTerrainHelper.ReadFromByteArray(byteArray);
            this.Mode = BattleModeHelper.ReadFromByteArray(byteArray);
            this.Caption = byteArray.ReadUTF();
            this.CustomDataHash = byteArray.ReadUTF();
        }

        public void WriteToByteArray(ByteArray byteArray)
        {
            BattleTerrainHelper.WriteToByteArray(byteArray, this.Terrain);
            BattleModeHelper.WriteToByteArray(byteArray, this.Mode);
            byteArray.WriteUTF(this.Caption);
            byteArray.WriteUTF(this.CustomDataHash);
        }
    }
}

