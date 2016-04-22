namespace PokemonBattle.BattleNetwork
{
    using System;
    using System.Runtime.CompilerServices;

    public delegate void ServerTieDelegate(int identity, string player, TieMessage message);
}

