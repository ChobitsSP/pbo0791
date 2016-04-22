namespace PokemonBattle.RoomClient
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public delegate Form BuildObserverFormDelegate(int identity, string serverAddress, byte playerPosition);
}

