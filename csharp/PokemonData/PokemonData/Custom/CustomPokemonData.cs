namespace PokemonBattle.PokemonData.Custom
{
    using PokemonBattle.PokemonData;
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class CustomPokemonData
    {
        private List<int> _afterEvolution = new List<int>();
        private byte _attackBase;
        private long _backImage = -1L;
        private long _backImageF = -1L;
        private int _beforeEvolution;
        private byte _defenceBase;
        private EggGroup _eggGroup1;
        private EggGroup _eggGroup2;
        private long _frame = -1L;
        private long _frameF = -1L;
        private long _frontImage = -1L;
        private long _frontImageF = -1L;
        private PokemonGenderRestriction _genderRestriction;
        private byte _hpBase;
        private long _icon = -1L;
        private int _identity;
        private List<string> _moves = new List<string>();
        private string _nameBase;
        private int _number;
        private byte _spAttackBase;
        private byte _spDefenceBase;
        private byte _speedBase;
        private Trait _trait1;
        private Trait _trait2;
        private string _type1;
        private string _type2;
        private double _weight;

        public CustomPokemonData Clone()
        {
            CustomPokemonData data = base.MemberwiseClone() as CustomPokemonData;
            data._afterEvolution = new List<int>(this._afterEvolution);
            data._moves = new List<string>(this._moves);
            return data;
        }

        public bool Equals(CustomPokemonData data)
        {
            if (data == null)
            {
                return false;
            }
            if (data._identity != this._identity)
            {
                return false;
            }
            if (data._nameBase != this._nameBase)
            {
                return false;
            }
            if (data._weight != this._weight)
            {
                return false;
            }
            if (data._number != this._number)
            {
                return false;
            }
            if (data._hpBase != this._hpBase)
            {
                return false;
            }
            if (data._attackBase != this._attackBase)
            {
                return false;
            }
            if (data._defenceBase != this._defenceBase)
            {
                return false;
            }
            if (data._speedBase != this._speedBase)
            {
                return false;
            }
            if (data._spAttackBase != this._spAttackBase)
            {
                return false;
            }
            if (data._spDefenceBase != this._spDefenceBase)
            {
                return false;
            }
            if (data._type1 != this._type1)
            {
                return false;
            }
            if (data._type2 != this._type2)
            {
                return false;
            }
            if (data._trait1 != this._trait1)
            {
                return false;
            }
            if (data._trait2 != this._trait2)
            {
                return false;
            }
            if (data._eggGroup1 != this._eggGroup1)
            {
                return false;
            }
            if (data._eggGroup2 != this._eggGroup2)
            {
                return false;
            }
            if (data._beforeEvolution != this._beforeEvolution)
            {
                return false;
            }
            if (data._afterEvolution.Count != this._afterEvolution.Count)
            {
                return false;
            }
            for (int i = 0; i < data._afterEvolution.Count; i++)
            {
                if (data._afterEvolution[i] != this._afterEvolution[i])
                {
                    return false;
                }
            }
            if (data._genderRestriction != this._genderRestriction)
            {
                return false;
            }
            if (data._moves.Count != this._moves.Count)
            {
                return false;
            }
            for (int j = 0; j < data._moves.Count; j++)
            {
                if (data._moves[j] != this._moves[j])
                {
                    return false;
                }
            }
            if (data._frontImage != this._frontImage)
            {
                return false;
            }
            if (data._frontImageF != this._frontImageF)
            {
                return false;
            }
            if (data._backImage != this._backImage)
            {
                return false;
            }
            if (data._backImageF != this._backImageF)
            {
                return false;
            }
            if (data._frame != this._frame)
            {
                return false;
            }
            if (data._frameF != this._frameF)
            {
                return false;
            }
            if (data._icon != this._icon)
            {
                return false;
            }
            return true;
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

        public List<string> Moves
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

