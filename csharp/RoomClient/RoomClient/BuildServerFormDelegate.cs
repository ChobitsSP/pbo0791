namespace PokemonBattle.RoomClient
{
    using PokemonBattle.BattleNetwork;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public delegate Form BuildServerFormDelegate(string userName, BattleMode mode, List<BattleRule> rules);
}

