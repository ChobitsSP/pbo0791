namespace PokemonBattle.BattleNetwork
{
    using NetworkLib.Utilities;
    using System;

    public class PlayerMove
    {
        public BattleMove Move;
        public byte MoveIndex;
        public string Player;
        public PokemonIndex Pokemon;
        public TargetIndex Target;

        public void ReadFromByteArray(ByteArray byteArray)
        {
            this.Player = byteArray.ReadUTF();
            this.Move = BattleMoveHelper.ReadFromByteArray(byteArray);
            this.Pokemon = PokemonIndexHelper.ReadFromByteArray(byteArray);
            this.Target = TargetIndexHelper.ReadFromByteArray(byteArray);
            this.MoveIndex = byteArray.ReadByte();
        }

        public void WriteToByteArray(ByteArray byteArray)
        {
            byteArray.WriteUTF(this.Player);
            BattleMoveHelper.WriteToByteArray(byteArray, this.Move);
            PokemonIndexHelper.WriteToByteArray(byteArray, this.Pokemon);
            TargetIndexHelper.WriteToByteArray(byteArray, this.Target);
            byteArray.WriteByte(this.MoveIndex);
        }
    }
}

