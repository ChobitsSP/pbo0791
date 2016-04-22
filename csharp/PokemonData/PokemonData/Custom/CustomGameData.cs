namespace PokemonBattle.PokemonData.Custom
{
    using PokemonBattle.PokemonData;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    [Serializable]
    public class CustomGameData
    {
        private List<CustomPokemonData> _addPokemons = new List<CustomPokemonData>();
        private int _idBase = 0x3e8;
        private long _imageOffset;
        private List<Bitmap> _images = new List<Bitmap>();
        private string _name;
        private List<int> _removePokemons = new List<int>();
        private List<UpdatePokemonData> _updatePokemons = new List<UpdatePokemonData>();

        public CustomGameData(string name)
        {
            this._name = name;
        }

        public CustomPokemonData AddNewPokemon(string name)
        {
            this._idBase++;
            CustomPokemonData item = new CustomPokemonData();
            item.Identity = this._idBase;
            item.NameBase = name;
            item.Type1 = "普通";
            item.Trait1 = Trait.自我中心;
            item.EggGroup1 = EggGroup.人型;
            item.Number = 0x1ee + this._addPokemons.Count;
            this._addPokemons.Add(item);
            return item;
        }

        public UpdatePokemonData AddUpdatePokemon(int identity, string name)
        {
            UpdatePokemonData item = new UpdatePokemonData();
            item.Identity = identity;
            item.NameBase = name;
            this._updatePokemons.Add(item);
            return item;
        }

        public CustomGameData Clone()
        {
            CustomGameData data = base.MemberwiseClone() as CustomGameData;
            data._images = new List<Bitmap>(this._images);
            data._removePokemons = new List<int>(this._removePokemons);
            data._addPokemons = new List<CustomPokemonData>();
            foreach (CustomPokemonData data2 in this._addPokemons)
            {
                data._addPokemons.Add(data2.Clone());
            }
            data._updatePokemons = new List<UpdatePokemonData>();
            foreach (UpdatePokemonData data3 in this._updatePokemons)
            {
                data._updatePokemons.Add(data3.Clone());
            }
            return data;
        }

        public bool Equals(CustomGameData data)
        {
            if (data == null)
            {
                return false;
            }
            if (this._idBase != data._idBase)
            {
                return false;
            }
            if (data._addPokemons.Count != this._addPokemons.Count)
            {
                return false;
            }
            for (int i = 0; i < data._addPokemons.Count; i++)
            {
                if (!data._addPokemons[i].Equals(this._addPokemons[i]))
                {
                    return false;
                }
            }
            if (data._removePokemons.Count != this._removePokemons.Count)
            {
                return false;
            }
            for (int j = 0; j < data._removePokemons.Count; j++)
            {
                int num5 = data._removePokemons[j];
                if (!num5.Equals(this._removePokemons[j]))
                {
                    return false;
                }
            }
            if (data._images.Count != this._images.Count)
            {
                return false;
            }
            for (int k = 0; k < data._images.Count; k++)
            {
                if (!data._images[k].Equals(this._images[k]))
                {
                    return false;
                }
            }
            if (data._removePokemons.Count != this._removePokemons.Count)
            {
                return false;
            }
            for (int m = 0; m < data._removePokemons.Count; m++)
            {
                if (data._removePokemons[m] != this._removePokemons[m])
                {
                    return false;
                }
            }
            return true;
        }

        public static CustomGameData FromFile(string path)
        {
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    CustomGameData data = new CustomGameData(reader.ReadString());
                    data._idBase = reader.ReadInt32();
                    data._addPokemons = formatter.Deserialize(stream) as List<CustomPokemonData>;
                    data._updatePokemons = formatter.Deserialize(stream) as List<UpdatePokemonData>;
                    data._removePokemons = formatter.Deserialize(stream) as List<int>;
                    long[] numArray = formatter.Deserialize(stream) as long[];
                    data._imageOffset = stream.Position;
                    foreach (CustomPokemonData data2 in data._addPokemons)
                    {
                        if (data2.FrontImage != -1L)
                        {
                            data2.FrontImage = numArray[(int) data2.FrontImage];
                        }
                        if (data2.FrontImageF != -1L)
                        {
                            data2.FrontImageF = numArray[(int) data2.FrontImageF];
                        }
                        if (data2.BackImage != -1L)
                        {
                            data2.BackImage = numArray[(int) data2.BackImage];
                        }
                        if (data2.BackImageF != -1L)
                        {
                            data2.BackImageF = numArray[(int) data2.BackImageF];
                        }
                        if (data2.Frame != -1L)
                        {
                            data2.Frame = numArray[(int) data2.Frame];
                        }
                        if (data2.FrameF != -1L)
                        {
                            data2.FrameF = numArray[(int) data2.FrameF];
                        }
                        if (data2.Icon != -1L)
                        {
                            data2.Icon = numArray[(int) data2.Icon];
                        }
                    }
                    return data;
                }
            }
        }

        public Bitmap GetImage(long index)
        {
            if (index == -1L)
            {
                return null;
            }
            return this._images[(int) index];
        }

        public void RemoveCustomPokemon(CustomPokemonData pm)
        {
            this._addPokemons.Remove(pm);
            foreach (CustomPokemonData data in this._addPokemons)
            {
                if (data.Number > pm.Number)
                {
                    data.Number--;
                }
            }
        }

        public void Save(string path)
        {
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    byte[] buffer;
                    writer.Write(this._name);
                    writer.Write(this._idBase);
                    formatter.Serialize(stream, this._addPokemons);
                    formatter.Serialize(stream, this._updatePokemons);
                    formatter.Serialize(stream, this._removePokemons);
                    long[] graph = new long[this._images.Count];
                    using (MemoryStream stream2 = new MemoryStream())
                    {
                        for (int i = 0; i < this._images.Count; i++)
                        {
                            graph[i] = stream2.Position;
                            formatter.Serialize(stream2, this._images[i]);
                        }
                        buffer = stream2.GetBuffer();
                    }
                    formatter.Serialize(stream, graph);
                    writer.Write(buffer);
                }
            }
        }

        public List<CustomPokemonData> CustomPokemons
        {
            get
            {
                return this._addPokemons;
            }
        }

        public long ImageOffset
        {
            get
            {
                return this._imageOffset;
            }
            set
            {
                this._imageOffset = value;
            }
        }

        public List<Bitmap> Images
        {
            get
            {
                return this._images;
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

        public List<int> RemovePokemons
        {
            get
            {
                return this._removePokemons;
            }
        }

        public List<UpdatePokemonData> UpdatePokemons
        {
            get
            {
                return this._updatePokemons;
            }
        }
    }
}

