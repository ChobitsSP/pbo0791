namespace PokemonBattle.RoomClient
{
    using PokemonBattle.BattleNetwork;
    using System;
    using System.Runtime.CompilerServices;

    public delegate void AgentBattleDelegate(int identity, byte playerPosition, BattleMode mode);
}

