namespace PokemonBattle.PokemonData
{
    using System;
    using System.IO;

    public class PokemonCustomInfo
    {
        private byte _attackEV;
        private byte _attackIV;
        private PokemonCharacter _character;
        private byte _defenceEV;
        private byte _defenceIV;
        private PokemonGender _gender;
        private byte _hpEV;
        private byte _hpIV;
        private int _identity;
        private PokemonBattle.PokemonData.Item _item;
        private byte _LV;
        private string _nickname;
        private string[] _selectedMoves = new string[] { "", "", "", "" };
        private byte _selectedTrait;
        private byte _spAttackEV;
        private byte _spAttackIV;
        private byte _spDefenceEV;
        private byte _spDefenceIV;
        private byte _speedEV;
        private byte _speedIV;

        public void CheckData()
        {
            if (this.AttackIV > 0x1f)
            {
                this.AttackIV = 0;
            }
            if (this.DefenceIV > 0x1f)
            {
                this.AttackIV = 0;
            }
            if (this.SpeedIV > 0x1f)
            {
                this.AttackIV = 0;
            }
            if (this.SpAttackIV > 0x1f)
            {
                this.AttackIV = 0;
            }
            if (this.SpDefenceIV > 0x1f)
            {
                this.AttackIV = 0;
            }
            if (this.HPIV > 0x1f)
            {
                this.AttackIV = 0;
            }
            if ((((((this.AttackEV + this.DefenceEV) + this.SpeedEV) + this.SpAttackEV) + this.SpDefenceEV) + this.HPEV) > 510)
            {
                this.AttackEV = 0;
                this.DefenceEV = 0;
                this.SpeedEV = 0;
                this.SpAttackEV = 0;
                this.SpDefenceEV = 0;
                this.HPEV = 0;
            }
        }

        public PokemonCustomInfo Clone()
        {
            PokemonCustomInfo info = base.MemberwiseClone() as PokemonCustomInfo;
            info._selectedMoves = new string[4];
            for (int i = 0; i < 4; i++)
            {
                info._selectedMoves[i] = this._selectedMoves[i];
            }
            return info;
        }

        public bool Equals(PokemonCustomInfo info)
        {
            if (info._identity != this._identity)
            {
                return false;
            }
            if (info._nickname != this._nickname)
            {
                return false;
            }
            if (info._LV != this._LV)
            {
                return false;
            }
            if (info._hpEV != this._hpEV)
            {
                return false;
            }
            if (info._attackEV != this._attackEV)
            {
                return false;
            }
            if (info._defenceEV != this._defenceEV)
            {
                return false;
            }
            if (info._speedEV != this._speedEV)
            {
                return false;
            }
            if (info._spAttackEV != this._spAttackEV)
            {
                return false;
            }
            if (info._spDefenceEV != this._spDefenceEV)
            {
                return false;
            }
            if (info._hpIV != this._hpIV)
            {
                return false;
            }
            if (info._attackIV != this._attackIV)
            {
                return false;
            }
            if (info._defenceIV != this._defenceIV)
            {
                return false;
            }
            if (info._speedIV != this._speedIV)
            {
                return false;
            }
            if (info._spAttackIV != this._spAttackIV)
            {
                return false;
            }
            if (info._spDefenceIV != this._spDefenceIV)
            {
                return false;
            }
            if (info._gender != this._gender)
            {
                return false;
            }
            if (info._character != this._character)
            {
                return false;
            }
            if (info._selectedTrait != this._selectedTrait)
            {
                return false;
            }
            if (info._item != this._item)
            {
                return false;
            }
            for (int i = 0; i < 4; i++)
            {
                if (this._selectedMoves[i] != info._selectedMoves[i])
                {
                    return false;
                }
            }
            return true;
        }

        public static PokemonCustomInfo FormStream(Stream input)
        {
            PokemonCustomInfo info = new PokemonCustomInfo();
            BinaryReader reader = new BinaryReader(input);
            info.Identity = reader.ReadInt32();
            if (info.Identity != 0)
            {
                info.Nickname = reader.ReadString();
                info.LV = reader.ReadByte();
                info.AttackEV = reader.ReadByte();
                info.DefenceEV = reader.ReadByte();
                info.SpeedEV = reader.ReadByte();
                info.SpAttackEV = reader.ReadByte();
                info.SpDefenceEV = reader.ReadByte();
                info.HPEV = reader.ReadByte();
                info.AttackIV = reader.ReadByte();
                info.DefenceIV = reader.ReadByte();
                info.SpeedIV = reader.ReadByte();
                info.SpAttackIV = reader.ReadByte();
                info.SpDefenceIV = reader.ReadByte();
                info.HPIV = reader.ReadByte();
                info.Gender = (PokemonGender) reader.ReadInt32();
                info.SelectedTrait = reader.ReadByte();
                info.Character = (PokemonCharacter) reader.ReadInt32();
                info.Item = (PokemonBattle.PokemonData.Item) reader.ReadInt32();
                for (int i = 0; i < 4; i++)
                {
                    info.SelectedMoves[i] = reader.ReadString();
                }
            }
            return info;
        }

        public void Save(Stream output)
        {
            BinaryWriter writer = new BinaryWriter(output);
            writer.Write(this._identity);
            if (this._identity != 0)
            {
                writer.Write(this._nickname);
                writer.Write(this._LV);
                writer.Write(this._attackEV);
                writer.Write(this._defenceEV);
                writer.Write(this._speedEV);
                writer.Write(this._spAttackEV);
                writer.Write(this._spDefenceEV);
                writer.Write(this._hpEV);
                writer.Write(this._attackIV);
                writer.Write(this._defenceIV);
                writer.Write(this._speedIV);
                writer.Write(this._spAttackIV);
                writer.Write(this._spDefenceIV);
                writer.Write(this._hpIV);
                writer.Write((int) this._gender);
                writer.Write(this._selectedTrait);
                writer.Write((int) this._character);
                writer.Write((int) this._item);
                foreach (string str in this._selectedMoves)
                {
                    writer.Write(str);
                }
            }
        }

        public byte AttackEV
        {
            get
            {
                return this._attackEV;
            }
            set
            {
                this._attackEV = value;
            }
        }

        public byte AttackIV
        {
            get
            {
                return this._attackIV;
            }
            set
            {
                this._attackIV = value;
            }
        }

        public PokemonCharacter Character
        {
            get
            {
                return this._character;
            }
            set
            {
                this._character = value;
            }
        }

        public byte DefenceEV
        {
            get
            {
                return this._defenceEV;
            }
            set
            {
                this._defenceEV = value;
            }
        }

        public byte DefenceIV
        {
            get
            {
                return this._defenceIV;
            }
            set
            {
                this._defenceIV = value;
            }
        }

        public PokemonGender Gender
        {
            get
            {
                return this._gender;
            }
            set
            {
                this._gender = value;
            }
        }

        public byte HPEV
        {
            get
            {
                return this._hpEV;
            }
            set
            {
                this._hpEV = value;
            }
        }

        public byte HPIV
        {
            get
            {
                return this._hpIV;
            }
            set
            {
                this._hpIV = value;
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

        public PokemonBattle.PokemonData.Item Item
        {
            get
            {
                return this._item;
            }
            set
            {
                this._item = value;
            }
        }

        public byte LV
        {
            get
            {
                return this._LV;
            }
            set
            {
                this._LV = value;
            }
        }

        public string Nickname
        {
            get
            {
                return this._nickname;
            }
            set
            {
                this._nickname = value;
            }
        }

        public string[] SelectedMoves
        {
            get
            {
                return this._selectedMoves;
            }
        }

        public byte SelectedTrait
        {
            get
            {
                return this._selectedTrait;
            }
            set
            {
                this._selectedTrait = value;
            }
        }

        public byte SpAttackEV
        {
            get
            {
                return this._spAttackEV;
            }
            set
            {
                this._spAttackEV = value;
            }
        }

        public byte SpAttackIV
        {
            get
            {
                return this._spAttackIV;
            }
            set
            {
                this._spAttackIV = value;
            }
        }

        public byte SpDefenceEV
        {
            get
            {
                return this._spDefenceEV;
            }
            set
            {
                this._spDefenceEV = value;
            }
        }

        public byte SpDefenceIV
        {
            get
            {
                return this._spDefenceIV;
            }
            set
            {
                this._spDefenceIV = value;
            }
        }

        public byte SpeedEV
        {
            get
            {
                return this._speedEV;
            }
            set
            {
                this._speedEV = value;
            }
        }

        public byte SpeedIV
        {
            get
            {
                return this._speedIV;
            }
            set
            {
                this._speedIV = value;
            }
        }
    }
}

