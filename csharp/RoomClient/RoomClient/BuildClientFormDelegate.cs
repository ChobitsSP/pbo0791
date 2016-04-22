namespace PokemonBattle.RoomClient
{
    using PokemonBattle.BattleNetwork;
    using System;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public delegate Form BuildClientFormDelegate(string serverAddress, byte position, string userName, BattleMode mode);
}

