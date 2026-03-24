/*
 * SDK Pullenti Lingvo, version 4.33, fabruary 2026. Copyright (c) 2013-2026, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Pullenti.Ner.Core
{
    /// <summary>
    /// Элемент составного номера
    /// </summary>
    public class SingleNumToken : Pullenti.Ner.MetaToken
    {
        public SingleNumToken(Pullenti.Ner.Token b, Pullenti.Ner.Token e) : base(b, e, null)
        {
        }
        /// <summary>
        /// Значение номера (несколько вариантов - в случае неоднозначности)
        /// </summary>
        public List<SingleNumValue> Vals = new List<SingleNumValue>();
        /// <summary>
        /// Исходное строковое значение
        /// </summary>
        public string Value;
        /// <summary>
        /// Возмодный префикс
        /// </summary>
        public string Prefix;
        /// <summary>
        /// Возможный суффикс
        /// </summary>
        public string Suffix;
        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            res.AppendFormat("{0}: ", Value);
            for (int i = 0; i < Vals.Count; i++) 
            {
                if (i > 0) 
                    res.Append("/");
                res.Append(Vals[i].ToString());
            }
            return res.ToString();
        }
        /// <summary>
        /// Нормализация значения
        /// </summary>
        public string Normal
        {
            get
            {
                if (Vals.Count == 0) 
                    return "?";
                if (Vals[0].Typ == SingleNumValueType.Letter) 
                    return string.Format("{0}", Vals[0].Letter);
                return Vals[0].Val.ToString();
            }
        }
        void _addNum(int val, bool rom, bool upper = false)
        {
            SingleNumValue res = new SingleNumValue();
            res.Val = val;
            if (rom) 
                res.Typ = SingleNumValueType.Roman;
            if (rom && upper) 
                Vals.Insert(0, res);
            else 
                Vals.Add(res);
        }
        void _addChar(char ch, bool up)
        {
            foreach (SingleNumValue v in Vals) 
            {
                if (ch == v.Letter) 
                    return;
            }
            SingleNumValue res = new SingleNumValue();
            res.Typ = SingleNumValueType.Letter;
            res.Letter = char.ToLower(ch);
            res.Upper = up;
            Vals.Add(res);
        }
        public static SingleNumToken TryParse(Pullenti.Ner.Token t, bool first, bool force = false)
        {
            if (t == null) 
                return null;
            if (t.IsValue("I", null)) 
            {
            }
            if (!first && t.IsWhitespaceBefore) 
            {
                if (!force && !t.IsCharOf(".(<")) 
                    return null;
            }
            Pullenti.Ner.Token t0 = t;
            SingleNumToken res = null;
            if (t.IsChar('(') && !t.IsWhitespaceAfter) 
            {
                res = TryParse(t.Next, first, false);
                if (res != null && res.EndToken.IsChar(')')) 
                {
                    res.BeginToken = t;
                    res.Prefix = "(";
                    return res;
                }
                else 
                    return null;
            }
            if (t.IsChar('<') && !t.IsWhitespaceAfter) 
            {
                res = TryParse(t.Next, first, false);
                if (res != null && res.EndToken.IsChar('>')) 
                {
                    res.BeginToken = t;
                    res.Prefix = "<";
                    if (!first) 
                    {
                        res.Prefix = (res.Suffix = null);
                        res.Value = "<" + res.Value;
                    }
                    if (!res.EndToken.IsWhitespaceAfter) 
                    {
                        t = res.EndToken.Next;
                        if (t != null) 
                        {
                            if (t.IsCharOf(".])\\")) 
                            {
                                res.EndToken = t;
                                res.Suffix = t.GetSourceText();
                                res.Value += res.Suffix;
                            }
                        }
                    }
                }
                return res;
            }
            bool hasPref = false;
            if (first) 
            {
                Pullenti.Ner.Token tt = MiscHelper.CheckNumberPrefix(t);
                if (tt != null && !tt.IsCharOf(".)")) 
                {
                    if (t.IsNewlineBefore) 
                    {
                        hasPref = true;
                        t = tt;
                    }
                    else if (t.Previous != null && ((_checkKeyword(t.Previous) || t.Previous.IsNewlineBefore))) 
                    {
                        hasPref = true;
                        t = tt;
                    }
                }
            }
            Pullenti.Ner.NumberToken nt = t as Pullenti.Ner.NumberToken;
            if (nt != null && nt.IntValue != null && (nt.IntValue.Value < 3000)) 
            {
                if (nt.Typ == Pullenti.Ner.NumberSpellingType.Words && t == t0) 
                {
                    if (!first) 
                        return null;
                    if (force) 
                    {
                    }
                    else if (!t.IsNewlineBefore && _checkKeyword(t.Previous)) 
                    {
                    }
                    else if (t.Next != null && !t.IsNewlineAfter && _checkKeyword(t.Next)) 
                    {
                    }
                    else 
                        return null;
                }
                if (t.Next != null) 
                {
                    if (t.Next.IsValue("ГОД", null) || t.Next.IsValue("ЛЕТ", null)) 
                        return null;
                }
                res = new SingleNumToken(t0, t);
                res._addNum((t as Pullenti.Ner.NumberToken).IntValue.Value, false, false);
            }
            else if ((t is Pullenti.Ner.TextToken) && t.Chars.IsLetter && t.LengthChar == 1) 
            {
                if (string.IsNullOrEmpty((t as Pullenti.Ner.TextToken).Term)) 
                    return null;
                if (t.Next != null && t.Next.IsCharOf(")")) 
                {
                }
                else if (!first || force) 
                {
                }
                else 
                {
                    if (t.Previous != null && t.Previous.IsCharOf("(<") && ((t.Previous.IsNewlineBefore || _checkKeyword(t.Previous.Previous)))) 
                    {
                    }
                    else if (_checkKeyword(t.Previous)) 
                    {
                    }
                    else if (t.Chars.IsAllUpper && t.IsCharOf("IXХ") && !t.IsWhitespaceAfter) 
                    {
                    }
                    else if (t.IsNewlineBefore && !t.IsWhitespaceAfter) 
                    {
                    }
                    else 
                        return null;
                    if (t.Next != null && t.Next.IsCharOf(".)>")) 
                    {
                    }
                    else if (!t.IsWhitespaceAfter && t.Next.IsChar('(')) 
                    {
                    }
                    else if (t.IsNewlineAfter) 
                    {
                    }
                    else 
                        return null;
                }
                nt = null;
                if (t.Chars.IsAllUpper || t.IsValue("I", null)) 
                    nt = NumberHelper.TryParseRoman(t);
                bool up = t.Chars.IsAllUpper;
                res = new SingleNumToken(t0, t);
                char ch = (t as Pullenti.Ner.TextToken).Term[0];
                res._addChar(ch, up);
                if (((int)ch) < 0x80) 
                {
                    char cyr = Pullenti.Morph.LanguageHelper.GetCyrForLat(ch);
                    if (cyr != ((char)0)) 
                        res._addChar(cyr, up);
                }
                else 
                {
                    char lat = Pullenti.Morph.LanguageHelper.GetLatForCyr(ch);
                    if (lat != ((char)0)) 
                        res._addChar(lat, up);
                }
                if (ch == 'I') 
                    res._addNum(1, true, up);
                if (ch == 'V') 
                    res._addNum(5, true, up);
                if (ch == 'X' || ch == 'Х') 
                    res._addNum(10, true, up);
            }
            else if (t.IsCharOf("ˡ¹²³")) 
            {
                res = new SingleNumToken(t, t);
                res._addNum((t.IsCharOf("ˡ¹") ? 1 : (t.IsChar('²') ? 2 : 3)), false, false);
            }
            else 
            {
                nt = NumberHelper.TryParseRoman(t);
                if (nt == null || nt.IntValue == null) 
                    return null;
                bool ok = false;
                if (!first || force) 
                    ok = true;
                else if (t.Next != null && t.Next.IsCharOf(".)")) 
                    ok = true;
                else if (!t.IsNewlineBefore && _checkKeyword(t.Previous)) 
                    ok = true;
                if (!ok) 
                    return null;
                res = new SingleNumToken(t0, t);
                res._addNum(nt.IntValue.Value, true, nt.Chars.IsAllUpper);
            }
            if (res == null) 
                return null;
            if (!res.EndToken.IsWhitespaceAfter) 
            {
                t = res.EndToken.Next;
                if (t != null) 
                {
                    if (t.IsCharOf(".])>\\")) 
                    {
                        res.EndToken = t;
                        res.Suffix = t.GetSourceText();
                    }
                }
            }
            if (res.Value == null) 
                res.Value = res.GetSourceText();
            if ((!hasPref && first && !force) && res.Suffix == null && !res.IsNewlineAfter) 
            {
                Pullenti.Ner.Token tt = res.EndToken.Next;
                if ((tt is Pullenti.Ner.TextToken) && tt.Chars.IsAllLower) 
                    return null;
            }
            if (res.Value.Length == 1 && res.Suffix == null && res.Prefix == null) 
            {
                if (char.IsLetter(res.Value[0]) && res.BeginToken.Chars.IsAllUpper) 
                {
                    if (res.BeginToken.Next != null && res.BeginToken.Next.Chars.IsAllLower) 
                        return null;
                }
            }
            return res;
        }
        static bool _checkKeyword(Pullenti.Ner.Token t)
        {
            if (!(t is Pullenti.Ner.TextToken)) 
                return false;
            string term = (t as Pullenti.Ner.TextToken).Term;
            if (((term == "СТАТЬЯ" || term == "ГЛАВА" || term == "РАЗДЕЛ") || term == "ЧАСТЬ" || term == "ПОДРАЗДЕЛ") || term == "ПАРАГРАФ" || term == "ПОДПАРАГРАФ") 
                return true;
            return false;
        }
        /// <summary>
        /// Признак, что с номер может начинаться нумерация
        /// </summary>
        public bool IsOne
        {
            get
            {
                foreach (SingleNumValue v in Vals) 
                {
                    if (v.IsOne) 
                        return true;
                }
                return false;
            }
        }
        public void Correct(SingleNumToken prev)
        {
            if (prev == null) 
            {
                if (IsOne) 
                {
                    for (int i = Vals.Count - 1; i >= 0; i--) 
                    {
                        if (!Vals[i].IsOne) 
                            Vals.RemoveAt(i);
                    }
                }
                return;
            }
            SingleNumComparer comp = new SingleNumComparer();
            comp.Process(prev, this);
            if (comp.Typ != ComplexNumCompareType.Less) 
                return;
            for (int i = prev.Vals.Count - 1; i >= 0; i--) 
            {
                if (prev.Vals[i] != comp.Val1) 
                    prev.Vals.RemoveAt(i);
            }
            for (int i = Vals.Count - 1; i >= 0; i--) 
            {
                if (Vals[i] != comp.Val2) 
                    Vals.RemoveAt(i);
            }
        }
    }
}