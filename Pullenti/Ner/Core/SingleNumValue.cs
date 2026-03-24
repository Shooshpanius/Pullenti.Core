/*
 * SDK Pullenti Lingvo, version 4.33, fabruary 2026. Copyright (c) 2013-2026, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Core
{
    /// <summary>
    /// Возможное значение номера
    /// </summary>
    public class SingleNumValue
    {
        /// <summary>
        /// Тип значения
        /// </summary>
        public SingleNumValueType Typ = SingleNumValueType.Digit;
        /// <summary>
        /// Целочесленное значение
        /// </summary>
        public int Val;
        /// <summary>
        /// Символ при буквенном типе
        /// </summary>
        public char Letter;
        /// <summary>
        /// Признак, что буква в верхнем регистре
        /// </summary>
        public bool Upper;
        public override string ToString()
        {
            if (Typ == SingleNumValueType.Digit) 
                return Val.ToString();
            if (Typ == SingleNumValueType.Roman) 
                return string.Format("rom({0})", Val);
            return string.Format("{0}", Letter);
        }
        public bool IsOne
        {
            get
            {
                if (Val == 1) 
                    return true;
                if ((Letter == 'A' || Letter == 'А' || Letter == 'a') || Letter == 'а') 
                    return true;
                return false;
            }
        }
        public int ToInt()
        {
            if (Typ != SingleNumValueType.Letter) 
                return Val;
            if (!char.IsLetter(Letter)) 
                return -1;
            if (((int)Letter) < 0x80) 
            {
                if (char.IsUpper(Letter)) 
                    return (int)(((Letter - 'A') + 1));
                else 
                    return (int)(((Letter - 'a') + 1));
            }
            else 
            {
                char ch = char.ToUpper(Letter);
                int i = "АБВГДЕЖЗИКЛМНОПРСТУФХЦЧШЩЫЭЮЯ".IndexOf(ch);
                if (i >= 0) 
                    return i + 1;
                return -1;
            }
        }
    }
}