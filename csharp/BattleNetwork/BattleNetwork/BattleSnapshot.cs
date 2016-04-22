namespace PokemonBattle.BattleNetwork
{
    using NetworkLib.Utilities;
    using System;

    public class BattleSnapshot
    {
        public string NewText;
        public PokemonSnapshotSequence Pokemons;
        public int TextColor;

        public void ReadFromByteArray(ByteArray byteArray)
        {
            this.NewText = byteArray.ReadUTF();
            this.TextColor = byteArray.ReadInt();
            this.Pokemons = new PokemonSnapshotSequence();
            this.Pokemons.ReadFromByteArray(byteArray);
        }

        public void WriteToByteArray(ByteArray byteArray)
        {
            byteArray.WriteUTF(this.NewText);
            byteArray.WriteInt(this.TextColor);
            this.Pokemons.WriteToByteArray(byteArray);
        }
    }
}

