namespace PokemonBattle.PokemonData
{
    using PokemonBattle.PokemonData.Custom;
    using System;
    using System.Drawing;

    public interface IDataProvider
    {
        bool CheckPokemon(PokemonCustomInfo pokemon);
        MoveData[] GetAllMoves();
        PokemonBattle.PokemonData.PokemonData[] GetAllPokemons();
        PokemonBattle.PokemonData.Type[] GetAllTypes();
        Bitmap GetImage(int identity, long position);
        MoveData GetMoveData(string name);
        PokemonBattle.PokemonData.PokemonData GetPokemonData(int identity);
        TeamData GetRandomTeam(Random random);
        PokemonBattle.PokemonData.Type GetTypeData(string name);
        bool PokemonIsRemoved(int identity);

        CustomDataInfo CustomData { get; }
    }
}

