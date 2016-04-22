namespace PokemonBattle.BattleNetwork
{
    using System;

    public enum BattleMode
    {
        Double = 0x49a4342f,
        Double_4P = 0x56844ca5,
        Single = 0x49944f53,
        WrongInput = -1
    }
}

