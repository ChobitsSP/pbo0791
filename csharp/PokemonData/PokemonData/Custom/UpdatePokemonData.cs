namespace PokemonBattle.PokemonData.Custom
{
    using PokemonBattle.PokemonData;
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class UpdatePokemonData
    {
        private List<string> _addMoves = new List<string>();
        private byte _attackBase;
        private byte _defenceBase;
        private byte _hpBase;
        private int _identity;
        private string _nameBase;
        private int _number;
        private List<string> _removeMoves = new List<string>();
        private byte _spAttackBase;
        private byte _spDefenceBase;
        private byte _speedBase;
        private Trait _trait1;
        private Trait _trait2;
        private string _type1;
        private string _type2;
        private double _weight;

        public UpdatePokemonData Clone()
        {
            UpdatePokemonData data = base.MemberwiseClone() as UpdatePokemonData;
            data._addMoves = new List<string>(this._addMoves);
            data._removeMoves = new List<string>(this._removeMoves);
            return data;
        }

        public bool Equals(UpdatePokemonData data)
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
            if (data._addMoves.Count != this._addMoves.Count)
            {
                return false;
            }
            for (int i = 0; i < data._addMoves.Count; i++)
            {
                if (data._addMoves[i] != this._addMoves[i])
                {
                    return false;
                }
            }
            if (data._removeMoves.Count != this._removeMoves.Count)
            {
                return false;
            }
            for (int j = 0; j < data._removeMoves.Count; j++)
            {
                if (data._removeMoves[j] != this._removeMoves[j])
                {
                    return false;
                }
            }
            return true;
        }

        public List<string> AddMoves
        {
            get
            {
                return this._addMoves;
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

        public List<string> RemoveMoves
        {
            get
            {
                return this._removeMoves;
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

