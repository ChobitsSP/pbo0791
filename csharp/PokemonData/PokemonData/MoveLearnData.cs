namespace PokemonBattle.PokemonData
{
    using System;
    using System.IO;

    [Serializable]
    public class MoveLearnData
    {
        private string _info = string.Empty;
        private string _learnBy = string.Empty;
        private string _moveName = string.Empty;
        private Trait _withoutTrait;

        public static MoveLearnData FromStream(Stream input)
        {
            MoveLearnData data = new MoveLearnData();
            BinaryReader reader = new BinaryReader(input);
            data._withoutTrait = (Trait) reader.ReadInt32();
            data._moveName = reader.ReadString();
            data._learnBy = reader.ReadString();
            data._info = reader.ReadString();
            return data;
        }

        public void Save(Stream output)
        {
            BinaryWriter writer = new BinaryWriter(output);
            writer.Write((int) this._withoutTrait);
            writer.Write(this._moveName);
            writer.Write(this._learnBy);
            writer.Write(this._info);
        }

        public string Info
        {
            get
            {
                return this._info;
            }
            set
            {
                this._info = value;
            }
        }

        public string LearnBy
        {
            get
            {
                return this._learnBy;
            }
            set
            {
                this._learnBy = value;
            }
        }

        public string MoveName
        {
            get
            {
                return this._moveName;
            }
            set
            {
                this._moveName = value;
            }
        }

        public Trait WithoutTrait
        {
            get
            {
                return this._withoutTrait;
            }
            set
            {
                this._withoutTrait = value;
            }
        }
    }
}

