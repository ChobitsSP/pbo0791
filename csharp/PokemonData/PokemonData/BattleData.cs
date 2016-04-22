namespace PokemonBattle.PokemonData
{
    using PokemonBattle.PokemonData.Custom;
    using System;
    using System.Drawing;

    public class BattleData
    {
        private static IDataProvider _dataProvider;

        private BattleData()
        {
        }

        public static bool CheckPokemon(PokemonCustomInfo pokemon)
        {
            if (_dataProvider == null)
            {
                return false;
            }
            return _dataProvider.CheckPokemon(pokemon);
        }

        public static MoveData[] GetAllMoves()
        {
            if (_dataProvider == null)
            {
                return null;
            }
            return _dataProvider.GetAllMoves();
        }

        public static PokemonBattle.PokemonData.PokemonData[] GetAllPokemons()
        {
            if (_dataProvider == null)
            {
                return null;
            }
            return _dataProvider.GetAllPokemons();
        }

        public static PokemonBattle.PokemonData.Type[] GetAllTypes()
        {
            if (_dataProvider == null)
            {
                return null;
            }
            return _dataProvider.GetAllTypes();
        }

        public static Bitmap GetImage(int identity, long position)
        {
            if (_dataProvider == null)
            {
                return null;
            }
            return _dataProvider.GetImage(identity, position);
        }

        public static MoveData GetMove(string name)
        {
            if (_dataProvider == null)
            {
                return null;
            }
            return _dataProvider.GetMoveData(name);
        }

        public static PokemonBattle.PokemonData.PokemonData GetPokemon(int identity)
        {
            if (_dataProvider == null)
            {
                return null;
            }
            return _dataProvider.GetPokemonData(identity);
        }

        public static TeamData GetRandomTeam(Random random)
        {
            if (_dataProvider == null)
            {
                return null;
            }
            return _dataProvider.GetRandomTeam(random);
        }

        public static PokemonBattle.PokemonData.Type GetTypeData(string name)
        {
            if (_dataProvider == null)
            {
                return null;
            }
            return _dataProvider.GetTypeData(name);
        }

        public static bool PokemonIsRemoved(int identity)
        {
            if (_dataProvider == null)
            {
                return false;
            }
            return _dataProvider.PokemonIsRemoved(identity);
        }

        public static CustomDataInfo CustomData
        {
            get
            {
                if (_dataProvider == null)
                {
                    return null;
                }
                return _dataProvider.CustomData;
            }
        }

        public static IDataProvider DataProvider
        {
            get
            {
                return _dataProvider;
            }
            set
            {
                _dataProvider = value;
            }
        }
    }
}

