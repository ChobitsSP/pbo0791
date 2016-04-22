namespace PokemonBattle.BattleNetwork
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public delegate bool ClientConnectedDelegate(string identity, BattleMode modeInfo, string versionInfo, out string message);
}

