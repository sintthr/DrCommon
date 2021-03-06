﻿/*
  DDValueSx.cs -- provides XML formating serialization and deserialization for DDValue of the 'DrData'  1.0, May 8, 2017
 
  Copyright (c) 2013-2017 Kudryashov Andrey aka Dr
 
  This software is provided 'as-is', without any express or implied
  warranty. In no event will the authors be held liable for any damages
  arising from the use of this software.

  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

      1. The origin of this software must not be misrepresented; you must not
      claim that you wrote the original software. If you use this software
      in a product, an acknowledgment in the product documentation would be
      appreciated but is not required.

      2. Altered source versions must be plainly marked as such, and must not be
      misrepresented as being the original software.

      3. This notice may not be removed or altered from any source distribution.

      Kudryashov Andrey <kudryashov.andrey at gmail.com>

*/

using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using DrOpen.DrCommon.DrData;
using DrOpen.DrCommon.DrData.Exceptions;

namespace DrOpen.DrCommon.DrDataSx
{
    /// <summary>
    /// provides XML formating serialization and deserialization for DDValue of the 'DrData'
    /// </summary>
    [XmlRoot(ElementName = "v")]
    public class DDValueSx : IXmlSerializable
    {

        private DDValueSx()
        { }
        private DDValueSx(DDValue v)
        {
            this.v = v;
        }
        /// <summary>
        /// returns/unboxes DDValue 
        /// </summary>
        /// <returns></returns>
        public DDValue GetDDValue()
        {
            return this.v;
        }

        private DDValue v;

        #region IXmlSerializable
        /// <summary>
        /// This method is reserved and should not be used. When implementing the IXmlSerializable interface, you should return null) from this method, and instead, 
        /// if specifying a custom schema is required, apply the XmlSchemaProviderAttribute to the class.
        /// </summary>
        /// <returns>null</returns>
        public XmlSchema GetSchema() { return null; }
        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer"></param>
        public virtual void WriteXml(XmlWriter writer)
        {
            DDValueSxe.XMLSerialize(v, writer);
        }
        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader"></param>
        public virtual void ReadXml(XmlReader reader)
        {
            this.v = DDValueSxe.Deserialize(reader);
        }
        #endregion IXmlSerializabl

        #region explicit operator
        /// <summary>
        /// boxes DDValue to for XML formating serialization and deserialization
        /// </summary>
        /// <param name="n">DDValue for box</param>
        /// <returns></returns>
        public static explicit operator DDValueSx(DDValue v)
        {
            return (v == null ? null : new DDValueSx(v));
        }
        /// <summary>
        /// unbox DDValue
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static implicit operator DDValue(DDValueSx v)
        {
            return (v == null ? null : v.v);
        }
        #endregion explicit operator
    }


    /// <summary>
    /// provides XML formating serialization and deserialization for DDValue of the 'DrData'
    /// </summary>
    public static class DDValueSxe
    {
        public static Type DEFAULT_VALUE_TYPE = typeof(string);

        #region Serialize
        /// <summary>
        /// Serializes the specified DDValue into its XML representation and writes to a text writer.
        /// The parent XML element &lt;v&gt; will be writed&lt;/v&gt;
        /// </summary>
        /// <param name="v">the value to serialize</param>
        /// <param name="tw">text writer used to write the XML document.</param>
        public static void Serialize(this DDValue v, TextWriter tw)
        {
            using (XmlWriter writer = new XmlTextWriter(tw))
            {
                v.Serialize(writer);
            }
        }
        /// <summary>
        /// Serializes the specified DDValue into its XML representation and writes to a string builder.
        /// The parent XML element &lt;v&gt; will be writed&lt;/v&gt;
        /// </summary>
        /// <param name="v">the value to serialize</param>
        /// <param name="sb">string builder used to write the XML document.</param>
        public static void Serialize(this DDValue v, StringBuilder sb)
        {
            using (StringWriter sw = new StringWriter(sb))
            {
                using (XmlWriter writer = new XmlTextWriter(sw))
                {
                    v.Serialize(writer);
                }
            }
        }
        /// <summary>
        /// Serializes the specified DDValue into its XML representation and writes to a stream.
        /// The parent XML element &lt;v&gt; will be writed&lt;/v&gt;
        /// </summary>
        /// <param name="v">the value to serialize</param>
        /// <param name="s">stream used to write the XML document.</param>
        public static void Serialize(this DDValue v, Stream s)
        {
            using (StreamWriter sw = new StreamWriter(s))
            {
                using (XmlWriter writer = new XmlTextWriter(sw))
                {
                    v.Serialize(writer);
                }
            }
        }
        /// <summary>
        /// Serializes the specified DDValue into its XML representation and writes to a XML writer.
        /// The parent XML element &lt;v&gt; will be writed&lt;/v&gt;
        /// </summary>
        /// <param name="v">the value to serialize</param>
        /// <param name="writer">XML writer used to write the XML document.</param>
        public static void Serialize(this DDValue v, XmlWriter writer)
        {
            writer.WriteStartElement(DDSchema.XML_SERIALIZE_NODE_VALUE);
            XMLSerialize(v, writer);
            writer.WriteEndElement();
        }
        /// <summary>
        /// Serializes the specified DDValue into its XML representation and writes to a XML writer.
        /// The parent XML element &lt;v&gt; will be writed&lt;/v&gt;
        /// </summary>
        /// <param name="v">the value to serialize</param>
        /// <param name="writer">XML writer used to write the XML document.</param>
        internal static void XMLSerialize(DDValue v, XmlWriter writer)
        {
            if (v == null)
            {
                writer.WriteAttributeString(DDSchema.XML_SERIALIZE_ATTRIBUTE_TYPE, DDSchema.XML_SERIALIZE_VALUE_NULL);
            }
            else if (v.Type == null)
            {
                writer.WriteAttributeString(DDSchema.XML_SERIALIZE_ATTRIBUTE_TYPE, DDSchema.XML_SERIALIZE_VALUE_TYPE_NULL);
            }
            else
            {
                // doesn't write System.String value type, it's default type
                if (v.Type != typeof(System.String)) writer.WriteAttributeString(DDSchema.XML_SERIALIZE_ATTRIBUTE_TYPE, v.Type.ToString());
                if (IsThisTypeXMLSerializeAsArray(v.Type))
                {
                    foreach (var element in v.ToStringArray())
                    {
                        writer.WriteStartElement(DDSchema.XML_SERIALIZE_NODE_ARRAY_VALUE_ITEM);
                        writer.WriteString(element);
                        writer.WriteEndElement();
                    }
                }
                else
                {
                    writer.WriteString(v.ToString());
                }

            }
        }
        /// <summary>
        /// Return true if this type should be serialization per each array element
        /// </summary>
        /// <param name="type">Type to serialize</param>
        /// <returns>Return true if this type should be serialization per each array element, otherwise: false</returns>
        /// <example>For example: byte[] should be serialize as HEX single string therefore return n is false for this type, all other arrays should be serialized per elements</example>
        private static bool IsThisTypeXMLSerializeAsArray(Type type)
        {
            return ((type.IsArray) && (type != typeof(byte[])));
        }
        #endregion Serialize
        #region Deserialize
        /// <summary>
        /// Generates an new DDValue from its XML representation.
        /// </summary>
        /// <param name="s">stream</param>
        /// <returns>an new DDValue </returns>
        public static DDValue Deserialize(Stream s)
        {
            using (StreamReader sr = new StreamReader(s))
            {
                using (XmlReader r = new XmlTextReader(sr))
                {
                    return Deserialize(r);
                }
            }
        }
        /// <summary>
        /// Generates an new DDValue from its XML representation.
        /// </summary>
        /// <param name="v">The deserialized value.</param>
        /// <param name="s">stream</param>
        public static void Deserialize(this DDValue v, Stream s)
        {
            v.Deserialize(s);
        }
        /// <summary>
        /// Generates an new DDValue from its XML representation.
        /// </summary>
        /// <param name="tr">text reader</param>
        /// <returns>an new DDValue </returns>
        public static DDValue Deserialize(TextReader tr)
        {
            using (XmlReader r = XmlReader.Create(tr))
            {
                return Deserialize(r);
            }
        }
        /// <summary>
        /// Generates an new DDValue from its XML representation.
        /// </summary>
        /// <param name="v">The deserialized value.</param>
        /// <param name="tr">text reader</param>
        public static void Deserialize(this DDValue v, TextReader tr)
        {
            v.Deserialize(tr);
        }
        /// <summary>
        /// Generates an new DDValue from its XML representation.
        /// </summary>
        /// <param name="s">string</param>
        /// <returns>an new DDValue </returns>
        public static DDValue Deserialize(String s)
        {
            using (XmlReader r = XmlReader.Create(new StringReader(s)))
            {
                return Deserialize(r);
            }
        }
        /// <summary>
        /// Generates an new DDValue from its XML representation.
        /// </summary>
        /// <param name="v">The deserialized value.</param>
        /// <param name="s">string</param>
        public static void Deserialize(this DDValue v, String s)
        {
            v.Deserialize(s);
        }
        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">XML reader</param>
        /// <returns>an new DDValue </returns>
        public static DDValue Deserialize(XmlReader reader)
        {
            DDValue v = null;
            reader.MoveToContent();

            var t = reader.GetAttribute(DDSchema.XML_SERIALIZE_ATTRIBUTE_TYPE);
            if (t == null) // if attribute type doesn't set default type - string
                t = DEFAULT_VALUE_TYPE.ToString();

            if (t == String.Empty) // here is type attribute equals string.empty like t="", the value is null here
            {
                if (reader.NodeType == XmlNodeType.Element) reader.ReadStartElement();
            }
            else  if (t == DDSchema.XML_SERIALIZE_VALUE_TYPE_NULL) // here is type is null like t="null", the value is new DDValue here
            {
                v = new DDValue(); // data and type are null here, but class has been initialezed yet
                if (reader.NodeType == XmlNodeType.Element) reader.ReadStartElement();
            }
            else
            {
                var type = Type.GetType(t);
                if (type == null) throw new DDTypeIncorrectException(t);
                if (IsThisTypeXMLSerializeAsArray(type))
                {
                    var value = ReadXmlValueArray(reader);
                    if (value == null) value = new string[] { }; // support empty array
                    v = new DDValue(DDValue.ConvertFromStringArrayTo(type, value));
                }
                else
                {
                    v = new DDValue(DDValue.ConvertFromStringTo(type, GetXmlElementValue(reader)));
                }
            }
            if ((reader.NodeType == XmlNodeType.EndElement) && (reader.Name == DDSchema.XML_SERIALIZE_NODE_VALUE)) reader.ReadEndElement(); // Need to close the opened element </n>, only self
            return v;
        }

        /// <summary>
        /// Return XML Element n.
        /// Open XML Element if needed, read n, close element and return n 
        /// </summary>
        /// <param name="reader">Xml stream reder</param>
        /// <returns>XML Element n</returns>
        private static string GetXmlElementValue(XmlReader reader)
        {
            if (reader.NodeType == XmlNodeType.Element) reader.ReadStartElement();
            var value = reader.Value; // read node n for none array types
            if (reader.HasValue) // read n of element if there is
            {
                reader.Read(); // read n of element
                if (reader.NodeType == XmlNodeType.EndElement) reader.ReadEndElement(); // need to close the opened element
            }
            return value;
        }

        /// <summary>
        /// Read XML Subling Nodes for array
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static string[] ReadXmlValueArray(XmlReader reader)
        {
            int i = 0;
            string[] v = null;

            // Exit if element is empty value, like <v t="System.String[]"/>
            //           attribue empty array, like <a n="a3" t="System.String[]" />
            if (((reader.IsStartElement(DDSchema.XML_SERIALIZE_NODE_VALUE)) || 
                 (reader.IsStartElement(DDSchema.XML_SERIALIZE_NODE_ATTRIBUTE))) && (reader.IsEmptyElement))
            {
                reader.Read(); // go to next element
                return v;
            }
            reader.Read();
            // Exit if element is empty value, like <v t="System.String[]"></v>
            //           attribue empty array, like <a n="a3" t="System.String[]" /></a>
            if (((reader.Name == DDSchema.XML_SERIALIZE_NODE_VALUE) || 
                 (reader.Name == DDSchema.XML_SERIALIZE_NODE_ATTRIBUTE)) && (reader.NodeType == XmlNodeType.EndElement))
            {
                reader.ReadEndElement() ; // close openned element
                return v;
            }
            var initialDepth = reader.Depth;
            if (reader.NodeType == XmlNodeType.None)  return v; // 

            while ((reader.Depth >= initialDepth)) // do all childs
            {
                if ((reader.IsStartElement(DDSchema.XML_SERIALIZE_NODE_ARRAY_VALUE_ITEM) == false) || (reader.Depth > initialDepth))
                {
                    reader.Skip(); // Skip none <n> elements with childs and subchilds <n> elements 'Deep protection'
                    if (reader.NodeType == XmlNodeType.EndElement) reader.ReadEndElement(); // need to close the opened element after deep protection
                }
                else
                {
                    Array.Resize(ref v, i + 1);
                    v[i] = GetXmlElementValue(reader);
                    i++;
                }
                reader.MoveToContent();
            }
            return v;
        }
        #endregion Deserialize
    }
}

