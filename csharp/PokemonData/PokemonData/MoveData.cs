namespace PokemonBattle.PokemonData
{
    using System;
    using System.IO;

    [Serializable]
    public class MoveData
    {
        private double _accuracy;
        private MoveAdditionalEffect _addEffect1;
        private MoveAdditionalEffect _addEffect2;
        private double _addEffectOdds;
        private bool[] _details = new bool[8];
        private MoveEffect _effect;
        private string _info;
        private PokemonBattle.PokemonData.MoveType _moveType;
        private string _name;
        private int _power;
        private int _PP;
        private int _priority;
        private MoveTarget _target;
        private string _type;

        public static MoveData FromStream(Stream input)
        {
            MoveData data = new MoveData();
            BinaryReader reader = new BinaryReader(input);
            data._moveType = (PokemonBattle.PokemonData.MoveType) reader.ReadInt32();
            data._target = (MoveTarget) reader.ReadInt32();
            data._effect = (MoveEffect) reader.ReadInt32();
            data._addEffect1 = (MoveAdditionalEffect) reader.ReadInt32();
            data._addEffect2 = (MoveAdditionalEffect) reader.ReadInt32();
            data._name = reader.ReadString();
            data._type = reader.ReadString();
            data._info = reader.ReadString();
            data._PP = reader.ReadInt32();
            data._power = reader.ReadInt32();
            data._priority = reader.ReadInt32();
            data._accuracy = reader.ReadDouble();
            data._addEffectOdds = reader.ReadDouble();
            data._details[0] = reader.ReadBoolean();
            data._details[1] = reader.ReadBoolean();
            data._details[2] = reader.ReadBoolean();
            data._details[3] = reader.ReadBoolean();
            data._details[4] = reader.ReadBoolean();
            data._details[5] = reader.ReadBoolean();
            data._details[6] = reader.ReadBoolean();
            data._details[7] = reader.ReadBoolean();
            return data;
        }

        public void Save(Stream output)
        {
            BinaryWriter writer = new BinaryWriter(output);
            writer.Write((int) this._moveType);
            writer.Write((int) this._target);
            writer.Write((int) this._effect);
            writer.Write((int) this._addEffect1);
            writer.Write((int) this._addEffect2);
            writer.Write(this._name);
            writer.Write(this._type);
            writer.Write(this._info);
            writer.Write(this._PP);
            writer.Write(this._power);
            writer.Write(this._priority);
            writer.Write(this._accuracy);
            writer.Write(this._addEffectOdds);
            writer.Write(this._details[0]);
            writer.Write(this._details[1]);
            writer.Write(this._details[2]);
            writer.Write(this._details[3]);
            writer.Write(this._details[4]);
            writer.Write(this._details[5]);
            writer.Write(this._details[6]);
            writer.Write(this._details[7]);
        }

        public double Accuracy
        {
            get
            {
                return this._accuracy;
            }
            set
            {
                this._accuracy = value;
            }
        }

        public MoveAdditionalEffect AddEffect1
        {
            get
            {
                return this._addEffect1;
            }
            set
            {
                this._addEffect1 = value;
            }
        }

        public MoveAdditionalEffect AddEffect2
        {
            get
            {
                return this._addEffect2;
            }
            set
            {
                this._addEffect2 = value;
            }
        }

        public double AddEffectOdds
        {
            get
            {
                return this._addEffectOdds;
            }
            set
            {
                this._addEffectOdds = value;
            }
        }

        public bool[] Details
        {
            get
            {
                return this._details;
            }
            set
            {
                this._details = value;
            }
        }

        public MoveEffect Effect
        {
            get
            {
                return this._effect;
            }
            set
            {
                this._effect = value;
            }
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

        public PokemonBattle.PokemonData.MoveType MoveType
        {
            get
            {
                return this._moveType;
            }
            set
            {
                this._moveType = value;
            }
        }

        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

        public int Power
        {
            get
            {
                return this._power;
            }
            set
            {
                this._power = value;
            }
        }

        public int PP
        {
            get
            {
                return this._PP;
            }
            set
            {
                this._PP = value;
            }
        }

        public int Priority
        {
            get
            {
                return this._priority;
            }
            set
            {
                this._priority = value;
            }
        }

        public MoveTarget Target
        {
            get
            {
                return this._target;
            }
            set
            {
                this._target = value;
            }
        }

        public string Type
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = value;
            }
        }
    }
}

