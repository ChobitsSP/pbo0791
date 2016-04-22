namespace PokemonBattle.PokemonData
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;

    public class XmlFormatter
    {
        public static void FormatMoveData(List<MoveData> moves, Stream stream)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            using (XmlWriter writer = XmlWriter.Create(stream, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("MoveList");
                foreach (MoveData data in moves)
                {
                    writer.WriteStartElement("Move");
                    writer.WriteStartElement("Name");
                    writer.WriteAttributeString("value", data.Name);
                    writer.WriteEndElement();
                    writer.WriteStartElement("Type");
                    writer.WriteAttributeString("value", data.Type);
                    writer.WriteEndElement();
                    writer.WriteStartElement("Category");
                    writer.WriteAttributeString("value", data.MoveType.ToString());
                    writer.WriteEndElement();
                    writer.WriteStartElement("Power");
                    writer.WriteAttributeString("value", data.Power.ToString());
                    writer.WriteEndElement();
                    writer.WriteStartElement("PP");
                    writer.WriteAttributeString("value", data.PP.ToString());
                    writer.WriteEndElement();
                    writer.WriteStartElement("Priority");
                    writer.WriteAttributeString("value", data.Priority.ToString());
                    writer.WriteEndElement();
                    writer.WriteStartElement("Accuracy");
                    writer.WriteAttributeString("value", data.Accuracy.ToString());
                    writer.WriteEndElement();
                    writer.WriteStartElement("Target");
                    writer.WriteAttributeString("value", data.Target.ToString());
                    writer.WriteEndElement();
                    writer.WriteStartElement("接触");
                    writer.WriteAttributeString("value", data.Details[0].ToString());
                    writer.WriteEndElement();
                    writer.WriteStartElement("声音");
                    writer.WriteAttributeString("value", data.Details[1].ToString());
                    writer.WriteEndElement();
                    writer.WriteStartElement("王证有效");
                    writer.WriteAttributeString("value", data.Details[2].ToString());
                    writer.WriteEndElement();
                    writer.WriteStartElement("可抢夺");
                    writer.WriteAttributeString("value", data.Details[3].ToString());
                    writer.WriteEndElement();
                    writer.WriteStartElement("作用在对象上");
                    writer.WriteAttributeString("value", data.Details[4].ToString());
                    writer.WriteEndElement();
                    writer.WriteStartElement("对替身无效");
                    writer.WriteAttributeString("value", data.Details[5].ToString());
                    writer.WriteEndElement();
                    writer.WriteStartElement("对保护无效");
                    writer.WriteAttributeString("value", data.Details[6].ToString());
                    writer.WriteEndElement();
                    writer.WriteStartElement("拳击");
                    writer.WriteAttributeString("value", data.Details[7].ToString());
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        public static void FormatPokemonData(List<PokemonBattle.PokemonData.PokemonData> pokemons, Stream stream)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            using (XmlWriter writer = XmlWriter.Create(stream, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("PokemonList");
                foreach (PokemonBattle.PokemonData.PokemonData data in pokemons)
                {
                    writer.WriteStartElement("Pokemon");
                    writer.WriteStartElement("Number");
                    writer.WriteAttributeString("value", data.Number.ToString());
                    writer.WriteEndElement();
                    writer.WriteStartElement("Name");
                    writer.WriteAttributeString("value", data.NameBase);
                    writer.WriteEndElement();
                    writer.WriteStartElement("Type1");
                    writer.WriteAttributeString("value", data.Type1);
                    writer.WriteEndElement();
                    if (!string.IsNullOrEmpty(data.Type2))
                    {
                        writer.WriteStartElement("Type2");
                        writer.WriteAttributeString("value", data.Type2);
                        writer.WriteEndElement();
                    }
                    writer.WriteStartElement("Trait1");
                    writer.WriteAttributeString("value", data.Trait1.ToString());
                    writer.WriteEndElement();
                    if (data.Trait2 != Trait.无)
                    {
                        writer.WriteStartElement("Trait2");
                        writer.WriteAttributeString("value", data.Trait2.ToString());
                        writer.WriteEndElement();
                    }
                    writer.WriteStartElement("EggGroup1");
                    writer.WriteAttributeString("value", data.EggGroup1.ToString());
                    writer.WriteEndElement();
                    if (data.EggGroup2 != EggGroup.无)
                    {
                        writer.WriteStartElement("EggGroup2");
                        writer.WriteAttributeString("value", data.EggGroup2.ToString());
                        writer.WriteEndElement();
                    }
                    writer.WriteStartElement("HPBase");
                    writer.WriteAttributeString("value", data.HPBase.ToString());
                    writer.WriteEndElement();
                    writer.WriteStartElement("AttackBase");
                    writer.WriteAttributeString("value", data.AttackBase.ToString());
                    writer.WriteEndElement();
                    writer.WriteStartElement("DefenceBase");
                    writer.WriteAttributeString("value", data.DefenceBase.ToString());
                    writer.WriteEndElement();
                    writer.WriteStartElement("SpAttackBase");
                    writer.WriteAttributeString("value", data.SpAttackBase.ToString());
                    writer.WriteEndElement();
                    writer.WriteStartElement("SpDefenceBase");
                    writer.WriteAttributeString("value", data.SpDefenceBase.ToString());
                    writer.WriteEndElement();
                    writer.WriteStartElement("SpeedBase");
                    writer.WriteAttributeString("value", data.SpeedBase.ToString());
                    writer.WriteEndElement();
                    writer.WriteStartElement("Weight");
                    writer.WriteAttributeString("value", data.Weight.ToString());
                    writer.WriteEndElement();
                    writer.WriteStartElement("性别限制");
                    writer.WriteAttributeString("value", data.GenderRestriction.ToString());
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }
    }
}

