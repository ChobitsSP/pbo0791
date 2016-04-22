namespace PokemonBattle.PokemonData.Custom
{
    using System;

    [Serializable]
    public class CustomDataInfo
    {
        private string _dataHash = string.Empty;
        private string _dataName = string.Empty;

        public override string ToString()
        {
            return this._dataName;
        }

        public string DataHash
        {
            get
            {
                return this._dataHash;
            }
            set
            {
                this._dataHash = value;
            }
        }

        public string DataName
        {
            get
            {
                return this._dataName;
            }
            set
            {
                this._dataName = value;
            }
        }
    }
}

