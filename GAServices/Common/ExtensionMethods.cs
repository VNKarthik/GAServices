using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
//using System.Windows.Forms;
//using Microsoft.Reporting.WinForms;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Xml;
//using Telerik.WinControls.UI;

namespace GAServices.Common
{
    public static class ExtensionMethods
    {
        public static bool HasRecords(this DataTable dt)
        {
            if (object.ReferenceEquals(dt, null)) //dt == null
                return false;
            else if (dt.Rows.Count == 0)
                return false;

            return true;
        }

        public static string ConcatenateWithComma(this string Value, object ValuetoConcatenate)
        {
            if (ValuetoConcatenate == null)
            {
                return "";
            }
            else if (ValuetoConcatenate.ToString() != "")
            {
                if (Value == null)
                {
                    return ValuetoConcatenate.ToString();
                }
                else if (Value == "")
                {
                    return ValuetoConcatenate.ToString();
                }
                else
                {
                    return Value + "," + ValuetoConcatenate;
                }
            }

            return "";
        }

        public static string Concatenate(this string Value, object ValuetoConcatenate, char SeparatingCharacter)
        {
            if (ValuetoConcatenate == null)
            {
                return "";
            }
            else if (ValuetoConcatenate.ToString() != "")
            {
                if (Value == "" || Value == null)
                {
                    return ValuetoConcatenate.ToString();
                }
                else
                {
                    return Value + SeparatingCharacter + ValuetoConcatenate;
                }
            }

            return "";
        }

        public static string GetDescription(this Enum GenericEnum)
        {
            Type genericEnumType = GenericEnum.GetType();
            MemberInfo[] memberInfo = genericEnumType.GetMember(GenericEnum.ToString());
            if ((memberInfo != null && memberInfo.Length > 0))
            {
                var _Attribs = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if ((_Attribs != null && _Attribs.Count() > 0))
                {
                    return ((System.ComponentModel.DescriptionAttribute)_Attribs.ElementAt(0)).Description;
                }
            }
            return GenericEnum.ToString();
        }

        public static string GetStringValue(this object Value)
        {
            if (Value == null)
                return "";
            else if (Convert.IsDBNull(Value))
                return "";
            else if (Value.ToString() == "")
                return "";
            else
                return Value.ToString();
        }

        public static string PascalCase(this string value)
        {
            string strReturnValue = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(value))
                {
                    return string.Empty;
                }
                else
                    value = value.ToLower();

                string[] strWords = value.Split(' ');
                char[] chrValue;

                foreach (string word in strWords)
                {
                    if (word.Trim() != string.Empty)
                    {
                        chrValue = word.ToCharArray();
                        chrValue[0] = char.ToUpper(chrValue[0]);

                        strReturnValue = strReturnValue + new string(chrValue) + " ";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return strReturnValue.Trim().ToString();
        }

        public static bool ToBool(this object Value)
        {
            if (Value == null)
                return false;
            else if (Convert.IsDBNull(Value))
                return false;
            else if (Value.ToString() == "")
                return false;
            else if (Value.ToString() == "0")
                return false;
            else if (Value.ToString() == "null")
                return false;
            else
                return true;
        }

        public static Double ToDouble(this object Value)
        {
            if (Value == null)
                return 0;
            else if (Convert.IsDBNull(Value))
                return 0;
            else if (Value.ToString() == "")
                return 0;
            else if (Value.ToString() == ".")
                return 0;
            else if (Value.ToString() == "null")
                return 0;
            else
                return Convert.ToDouble(Value.ToString());
        }

        public static long ToLong(this object Value)
        {
            if (Value == null)
                return 0;
            else if (Convert.IsDBNull(Value))
                return 0;
            else if (Value.ToString() == "")
                return 0;
            else if (Value.ToString() == ".")
                return 0;
            else if (Value.ToString() == "null")
                return 0;
            else
                return Convert.ToInt64(Value.ToString());
        }

        //Returns the index of the row that is duplicate of 
        public static int GetDuplicateRowIndex(this DataTable data)
        {
            int index = -1;

            if (!data.HasRecords())
                return -1;

            if (data.Rows.Count == 1)
                return -1;

            for (int intOuterIndex = 0; intOuterIndex < data.Rows.Count - 1; intOuterIndex++) //Loop till the last-but-one row
            {
                //if (intRowIndex != data.Rows.Count - 1)//If current Index is not the index of Last row
                //{
                var currentRow = data.Rows[intOuterIndex].ItemArray;

                for (int intInnerIndex = intOuterIndex + 1; intInnerIndex < data.Rows.Count; intInnerIndex++)
                {
                    var nextRow = data.Rows[intInnerIndex].ItemArray;

                    if (currentRow.SequenceEqual(nextRow.ToArray(), new DataRowComparer()))
                    {
                        return intInnerIndex;
                    }
                }
                //}
            }

            return index;
        }

        /// <summary>
        /// Returns the object of a Class as XML String
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetXmlString<T>(this T value)
        {
            if (value == null) return string.Empty;

            var xmlSerializer = new XmlSerializer(typeof(T));

            using (var stringWriter = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings { Indent = true }))
                {
                    xmlSerializer.Serialize(xmlWriter, value);
                    return stringWriter.ToString();
                }
            }
        }

    }
}
