namespace PokemonBattle.PokemonData
{
    using PokemonBattle.PokemonData.Custom;
    using System;
    using System.IO;

    public class TeamData
    {
        private CustomDataInfo _customInfo = new CustomDataInfo();
        private PokemonCustomInfo[] _pokemons = new PokemonCustomInfo[6];

        public TeamData()
        {
            for (int i = 0; i < 6; i++)
            {
                this._pokemons[i] = new PokemonCustomInfo();
            }
        }

        public void CheckData()
        {
            foreach (PokemonCustomInfo info in this._pokemons)
            {
                if (info.Identity != 0)
                {
                    if (!BattleData.CheckPokemon(info))
                    {
                        info.SelectedTrait = 1;
                        info.SelectedMoves[0] = BattleData.GetPokemon(info.Identity).Moves[0].MoveName;
                        info.SelectedMoves[1] = "";
                        info.SelectedMoves[2] = "";
                        info.SelectedMoves[3] = "";
                    }
                    info.CheckData();
                }
            }
        }

        public TeamData Clone()
        {
            TeamData data = new TeamData();
            data._customInfo.DataHash = this._customInfo.DataHash;
            data._customInfo.DataName = this._customInfo.DataName;
            for (int i = 0; i < 6; i++)
            {
                data._pokemons[i] = this._pokemons[i].Clone();
            }
            return data;
        }

        public bool Equals(TeamData data)
        {
            if (data == null)
            {
                return false;
            }
            for (int i = 0; i < 6; i++)
            {
                if (!this._pokemons[i].Equals(data._pokemons[i]))
                {
                    return false;
                }
            }
            if (this._customInfo.DataName != data._customInfo.DataName)
            {
                return false;
            }
            if (this._customInfo.DataHash != data._customInfo.DataHash)
            {
                return false;
            }
            return true;
        }

        public static TeamData FormStream(Stream input)
        {
            TeamData data = new TeamData();
            BinaryReader reader = new BinaryReader(input);
            data._customInfo.DataName = reader.ReadString();
            data._customInfo.DataHash = reader.ReadString();
            for (int i = 0; i < data._pokemons.Length; i++)
            {
                data._pokemons[i] = PokemonCustomInfo.FormStream(input);
            }
            return data;
        }

        public static TeamData FromBytes(byte[] bytes)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                stream.Write(bytes, 0, bytes.Length);
                stream.Seek(0L, SeekOrigin.Begin);
                return FormStream(stream);
            }
        }

        public void Save(Stream output)
        {
            BinaryWriter writer = new BinaryWriter(output);
            writer.Write(this._customInfo.DataName);
            writer.Write(this._customInfo.DataHash);
            foreach (PokemonCustomInfo info in this._pokemons)
            {
                info.Save(output);
            }
        }

        public byte[] ToBytes()
        {
            byte[] buffer;
            using (MemoryStream stream = new MemoryStream())
            {
                this.Save(stream);
                buffer = new byte[(int) stream.Position];
                Array.Copy(stream.GetBuffer(), buffer, buffer.Length);
            }
            return buffer;
        }

        public CustomDataInfo CustomInfo
        {
            get
            {
                return this._customInfo;
            }
            set
            {
                this._customInfo = value;
            }
        }

        public PokemonCustomInfo[] Pokemons
        {
            get
            {
                return this._pokemons;
            }
        }
    }
}

