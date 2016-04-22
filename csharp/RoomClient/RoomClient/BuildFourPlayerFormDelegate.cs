namespace PokemonBattle.RoomClient
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public delegate Form BuildFourPlayerFormDelegate(int identity, string serverAddress, string userName, bool asHost, FourPlayerFormCallback callback);
}

