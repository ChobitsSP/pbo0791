namespace PokemonBattle.PokemonData
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    [Serializable]
    public class Type
    {
        private System.Drawing.Image _image;
        private string _name;
        private Dictionary<string, double> _typeEffects = new Dictionary<string, double>();
        private Dictionary<Weather, double> _weatherEffects = new Dictionary<Weather, double>();

        public static PokemonBattle.PokemonData.Type FromStream(Stream input)
        {
            PokemonBattle.PokemonData.Type type = new PokemonBattle.PokemonData.Type();
            BinaryReader reader = new BinaryReader(input);
            type._name = reader.ReadString();
            int num = reader.ReadInt32();
            for (int i = 0; i < num; i++)
            {
                type._typeEffects.Add(reader.ReadString(), reader.ReadDouble());
            }
            num = reader.ReadInt32();
            for (int j = 0; j < num; j++)
            {
                type._weatherEffects.Add((Weather) reader.ReadInt32(), reader.ReadDouble());
            }
            BinaryFormatter formatter = new BinaryFormatter();
            type._image = formatter.Deserialize(input) as System.Drawing.Image;
            return type;
        }

        public void Save(Stream output)
        {
            BinaryWriter writer = new BinaryWriter(output);
            writer.Write(this._name);
            writer.Write(this._typeEffects.Count);
            foreach (string str in this._typeEffects.Keys)
            {
                writer.Write(str);
                writer.Write(this._typeEffects[str]);
            }
            writer.Write(this._weatherEffects.Count);
            foreach (Weather weather in this._weatherEffects.Keys)
            {
                writer.Write((int) weather);
                writer.Write(this._weatherEffects[weather]);
            }
            new BinaryFormatter().Serialize(output, this._image);
        }

        public System.Drawing.Image Image
        {
            get
            {
                return this._image;
            }
            set
            {
                this._image = value;
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

        public Dictionary<string, double> TypeEffects
        {
            get
            {
                return this._typeEffects;
            }
        }

        public Dictionary<Weather, double> WeatherEffects
        {
            get
            {
                return this._weatherEffects;
            }
        }
    }
}

