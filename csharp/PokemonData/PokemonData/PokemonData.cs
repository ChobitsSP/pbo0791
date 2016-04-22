namespace PokemonBattle.PokemonData
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    [Serializable]
    public class PokemonData
    {
        private List<int> _afterEvolution = new List<int>();
        private byte _attackBase;
        private long _backImage;
        private long _backImageF;
        private int _beforeEvolution;
        private byte _defenceBase;
        private EggGroup _eggGroup1;
        private EggGroup _eggGroup2;
        private long _frame;
        private long _frameF;
        private long _frontImage;
        private long _frontImageF;
        private PokemonGenderRestriction _genderRestriction;
        private byte _hpBase;
        private long _icon;
        private int _identity;
        private Item _itemRestriction;
        private List<MoveLearnData> _moves = new List<MoveLearnData>();
        private string _nameBase;
        private int _number;
        private byte _spAttackBase;
        private byte _spDefenceBase;
        private byte _speedBase;
        private Trait _trait1;
        private Trait _trait2;
        private string _type1 = string.Empty;
        private string _type2 = string.Empty;
        private double _weight;

        public static PokemonBattle.PokemonData.PokemonData FromStream(Stream input)
        {
            PokemonBattle.PokemonData.PokemonData data = new PokemonBattle.PokemonData.PokemonData();
            BinaryReader reader = new BinaryReader(input);
            data._trait1 = (Trait) reader.ReadInt32();
            data._trait2 = (Trait) reader.ReadInt32();
            data._eggGroup1 = (EggGroup) reader.ReadInt32();
            data._eggGroup2 = (EggGroup) reader.ReadInt32();
            data._genderRestriction = (PokemonGenderRestriction) reader.ReadInt32();
            data._identity = reader.ReadInt32();
            data._nameBase = reader.ReadString();
            data._number = reader.ReadInt32();
            data._weight = reader.ReadDouble();
            data._hpBase = reader.ReadByte();
            data._attackBase = reader.ReadByte();
            data._defenceBase = reader.ReadByte();
            data._speedBase = reader.ReadByte();
            data._spAttackBase = reader.ReadByte();
            data._spDefenceBase = reader.ReadByte();
            data._type1 = reader.ReadString();
            data._type2 = reader.ReadString();
            data._beforeEvolution = reader.ReadInt32();
            data._frontImage = reader.ReadInt64();
            data._frontImageF = reader.ReadInt64();
            data._backImage = reader.ReadInt64();
            data._backImageF = reader.ReadInt64();
            data._icon = reader.ReadInt64();
            data._frame = reader.ReadInt64();
            data._frameF = reader.ReadInt64();
            int num = reader.ReadInt32();
            for (int i = 0; i < num; i++)
            {
                data._afterEvolution.Add(reader.ReadInt32());
            }
            num = reader.ReadInt32();
            for (int j = 0; j < num; j++)
            {
                data._moves.Add(MoveLearnData.FromStream(input));
            }
            data._itemRestriction = (Item) reader.ReadInt32();
            return data;
        }

        public MoveLearnData GetMove(string moveName)
        {
            return this._moves.Find(delegate (MoveLearnData move) {
                return move.MoveName == moveName;
            });
        }

        public void Save(Stream output)
        {
            BinaryWriter writer = new BinaryWriter(output);
            writer.Write((int) this._trait1);
            writer.Write((int) this._trait2);
            writer.Write((int) this._eggGroup1);
            writer.Write((int) this._eggGroup2);
            writer.Write((int) this._genderRestriction);
            writer.Write(this._identity);
            writer.Write(this._nameBase);
            writer.Write(this._number);
            writer.Write(this._weight);
            writer.Write(this._hpBase);
            writer.Write(this._attackBase);
            writer.Write(this._defenceBase);
            writer.Write(this._speedBase);
            writer.Write(this._spAttackBase);
            writer.Write(this._spDefenceBase);
            writer.Write(this._type1);
            writer.Write(this._type2);
            writer.Write(this._beforeEvolution);
            writer.Write(this._frontImage);
            writer.Write(this._frontImageF);
            writer.Write(this._backImage);
            writer.Write(this._backImageF);
            writer.Write(this._icon);
            writer.Write(this._frame);
            writer.Write(this._frameF);
            writer.Write(this._afterEvolution.Count);
            foreach (int num in this._afterEvolution)
            {
                writer.Write(num);
            }
            writer.Write(this._moves.Count);
            foreach (MoveLearnData data in this._moves)
            {
                data.Save(output);
            }
            writer.Write((int) this._itemRestriction);
        }

        public List<int> AfterEvolution
        {
            get
            {
                return this._afterEvolution;
            }
        }

        public byte AttackBase
        {
            get
            {
                return this._attackBase;
            }
            set
            {
                this._attackBase = value;
            }
        }

        public long BackImage
        {
            get
            {
                return this._backImage;
            }
            set
            {
                this._backImage = value;
            }
        }

        public long BackImageF
        {
            get
            {
                return this._backImageF;
            }
            set
            {
                this._backImageF = value;
            }
        }

        public int BeforeEvolution
        {
            get
            {
                return this._beforeEvolution;
            }
            set
            {
                this._beforeEvolution = value;
            }
        }

        public byte DefenceBase
        {
            get
            {
                return this._defenceBase;
            }
            set
            {
                this._defenceBase = value;
            }
        }

        public EggGroup EggGroup1
        {
            get
            {
                return this._eggGroup1;
            }
            set
            {
                this._eggGroup1 = value;
            }
        }

        public EggGroup EggGroup2
        {
            get
            {
                return this._eggGroup2;
            }
            set
            {
                this._eggGroup2 = value;
            }
        }

        public long Frame
        {
            get
            {
                return this._frame;
            }
            set
            {
                this._frame = value;
            }
        }

        public long FrameF
        {
            get
            {
                return this._frameF;
            }
            set
            {
                this._frameF = value;
            }
        }

        public long FrontImage
        {
            get
            {
                return this._frontImage;
            }
            set
            {
                this._frontImage = value;
            }
        }

        public long FrontImageF
        {
            get
            {
                return this._frontImageF;
            }
            set
            {
                this._frontImageF = value;
            }
        }

        public PokemonGenderRestriction GenderRestriction
        {
            get
            {
                return this._genderRestriction;
            }
            set
            {
                this._genderRestriction = value;
            }
        }

        public byte HPBase
        {
            get
            {
                return this._hpBase;
            }
            set
            {
                this._hpBase = value;
            }
        }

        public long Icon
        {
            get
            {
                return this._icon;
            }
            set
            {
                this._icon = value;
            }
        }

        public int Identity
        {
            get
            {
                return this._identity;
            }
            set
            {
                this._identity = value;
            }
        }

        public Item ItemRestriction
        {
            get
            {
                return this._itemRestriction;
            }
            set
            {
                this._itemRestriction = value;
            }
        }

        public List<MoveLearnData> Moves
        {
            get
            {
                return this._moves;
            }
        }

        public string NameBase
        {
            get
            {
                return this._nameBase;
            }
            set
            {
                this._nameBase = value;
            }
        }

        public int Number
        {
            get
            {
                return this._number;
            }
            set
            {
                this._number = value;
            }
        }

        public byte SpAttackBase
        {
            get
            {
                return this._spAttackBase;
            }
            set
            {
                this._spAttackBase = value;
            }
        }

        public byte SpDefenceBase
        {
            get
            {
                return this._spDefenceBase;
            }
            set
            {
                this._spDefenceBase = value;
            }
        }

        public byte SpeedBase
        {
            get
            {
                return this._speedBase;
            }
            set
            {
                this._speedBase = value;
            }
        }

        public Trait Trait1
        {
            get
            {
                return this._trait1;
            }
            set
            {
                this._trait1 = value;
            }
        }

        public Trait Trait2
        {
            get
            {
                return this._trait2;
            }
            set
            {
                this._trait2 = value;
            }
        }

        public string Type1
        {
            get
            {
                return this._type1;
            }
            set
            {
                this._type1 = value;
            }
        }

        public string Type2
        {
            get
            {
                return this._type2;
            }
            set
            {
                this._type2 = value;
            }
        }

        public double Weight
        {
            get
            {
                return this._weight;
            }
            set
            {
                this._weight = value;
            }
        }
    }
}

