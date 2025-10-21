/*
 * SDK Pullenti Lingvo, version 4.31, august 2025. Copyright (c) 2013-2025, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Ner.Core
{
    /// <summary>
    /// Поддержка сложной нумерации разделов, пунктов, формул и т.п. 
    /// (составная, возможны римские цифры, буквы)
    /// </summary>
    public class ComplexNumToken : Pullenti.Ner.MetaToken
    {
        public ComplexNumToken(Pullenti.Ner.Token b, Pullenti.Ner.Token e) : base(b, e, null)
        {
        }
        /// <summary>
        /// Элементы составного номера
        /// </summary>
        public List<SingleNumToken> Nums = new List<SingleNumToken>();
        public string Normal
        {
            get
            {
                if (Nums.Count == 0) 
                    return null;
                string res = Nums[0].Normal;
                for (int i = 1; i < Nums.Count; i++) 
                {
                    res = string.Format("{0}.{1}", res, Nums[i].Normal);
                }
                return res;
            }
        }
        public string Prefix
        {
            get
            {
                return (Nums.Count > 0 ? Nums[0].Prefix : null);
            }
        }
        public string Suffix
        {
            get
            {
                return (Nums.Count > 0 ? Nums[Nums.Count - 1].Suffix : null);
            }
        }
        public bool IsOne
        {
            get
            {
                if (Nums.Count == 0) 
                    return false;
                return Nums[Nums.Count - 1].IsOne;
            }
        }
        public override string ToString()
        {
            return this.ToStringEx(false);
        }
        public string ToStringEx(bool ignoreSuffix)
        {
            if (Nums.Count == 0) 
                return "";
            string res = Nums[0].Value;
            for (int i = 1; i < Nums.Count; i++) 
            {
                res += Nums[i].Value;
            }
            if (ignoreSuffix && Suffix != null && res.EndsWith(Suffix)) 
                res = res.Substring(0, res.Length - Suffix.Length);
            return res;
        }
        /// <summary>
        /// Выделить сложный номер с указанного токена
        /// </summary>
        /// <param name="prev">предыдущий номер (если есть)</param>
        /// <param name="force">обязательно ли выделять</param>
        /// <param name="ignoreLastSuffix">суффикс у последнего составного числа игнорировать</param>
        /// <return>номер или null</return>
        public static ComplexNumToken TryParse(Pullenti.Ner.Token t, ComplexNumToken prev = null, bool force = false, bool ignoreLastSuffix = false)
        {
            if (t == null) 
                return null;
            if (t is Pullenti.Ner.ReferentToken) 
            {
                Pullenti.Ner.Referent r = t.GetReferent();
                if (r != null && r.TypeName == "BOOKLINKREF") 
                    t = (t as Pullenti.Ner.ReferentToken).BeginToken;
            }
            SingleNumToken nt = SingleNumToken.TryParse(t, true, force);
            if (nt == null) 
                return null;
            ComplexNumToken res = new ComplexNumToken(t, nt.EndToken);
            res.Nums.Add(nt);
            for (t = nt.EndToken.Next; t != null; t = t.Next) 
            {
                if (t.IsWhitespaceBefore) 
                {
                    if (t.WhitespacesBeforeCount > 2) 
                        break;
                    if (force && t.IsCharOf("<(")) 
                    {
                    }
                    else 
                        break;
                }
                nt = SingleNumToken.TryParse(t, false, force);
                if (nt == null) 
                {
                    if (((force && t.IsHiphen && !t.IsNewlineBefore) && !t.IsNewlineAfter && (t.Next is Pullenti.Ner.NumberToken)) && res.Nums[res.Nums.Count - 1].Suffix == null) 
                    {
                        nt = SingleNumToken.TryParse(t.Next, false, false);
                        if (nt != null) 
                        {
                            res.Nums[res.Nums.Count - 1].EndToken = t;
                            res.Nums[res.Nums.Count - 1].Suffix = "-";
                        }
                    }
                    if (nt == null) 
                        break;
                }
                if (nt.Vals[0].Typ == SingleNumValueType.Letter) 
                {
                    if (res.Nums[0].Vals[0].Typ == SingleNumValueType.Letter) 
                        return null;
                    if (res.Nums[res.Nums.Count - 1].Vals[0].Typ == SingleNumValueType.Letter) 
                        return null;
                }
                if (nt.Suffix == null && res.Suffix == ")") 
                    break;
                res.Nums.Add(nt);
                res.EndToken = (t = nt.EndToken);
            }
            if (res.Nums.Count == 1) 
            {
                if (res.Prefix == "<" && res.Suffix == ">") 
                    return null;
                if (!force && res.IsNewlineBefore && res.IsNewlineAfter) 
                    return null;
            }
            if ((ignoreLastSuffix && res.Suffix != null && res.Suffix.Length == 1) && res.EndToken.IsChar(res.Suffix[0])) 
            {
                res.EndToken = res.EndToken.Previous;
                SingleNumToken n1 = res.Nums[res.Nums.Count - 1];
                if (n1.Value != null && n1.Value.EndsWith(n1.Suffix)) 
                    n1.Value = n1.Value.Substring(0, n1.Value.Length - 1);
                n1.Suffix = null;
                n1.EndToken = res.EndToken;
            }
            return res;
        }
        public static void CorrectSeq(List<ComplexNumToken> seq)
        {
            if (seq.Count == 0) 
                return;
            seq[0].Correct(null);
            for (int i = 1; i < seq.Count; i++) 
            {
                seq[i].Correct(seq[i - 1]);
            }
            int lat = 0;
            int cyr = 0;
            foreach (ComplexNumToken s in seq) 
            {
                foreach (SingleNumToken n in s.Nums) 
                {
                    foreach (SingleNumValue v in n.Vals) 
                    {
                        if (v.Typ == SingleNumValueType.Letter) 
                        {
                            if (((char)v.Letter) < 0x80) 
                                lat++;
                            else 
                                cyr++;
                        }
                    }
                }
            }
            if (lat > cyr || cyr > lat) 
            {
                foreach (ComplexNumToken s in seq) 
                {
                    foreach (SingleNumToken n in s.Nums) 
                    {
                        for (int i = n.Vals.Count - 1; i >= 0; i--) 
                        {
                            if (n.Vals[i].Typ == SingleNumValueType.Letter && n.Vals.Count > 1) 
                            {
                                if (((char)n.Vals[i].Letter) < 0x80) 
                                {
                                    if (cyr > lat) 
                                        n.Vals.RemoveAt(i);
                                }
                                else if (lat > cyr) 
                                    n.Vals.RemoveAt(i);
                            }
                        }
                    }
                }
            }
        }
        void Correct(ComplexNumToken prev)
        {
            if (prev != null && prev.Nums.Count == Nums.Count) 
                Nums[Nums.Count - 1].Correct(prev.Nums[prev.Nums.Count - 1]);
            else if (prev == null) 
                Nums[Nums.Count - 1].Correct(null);
        }
        public bool CanBePsevdoSubseq(ComplexNumToken sub)
        {
            if (sub == null) 
                return false;
            if (Prefix != sub.Prefix || Suffix != sub.Suffix) 
                return false;
            if ((Nums.Count + 1) != sub.Nums.Count) 
                return false;
            if (!sub.Nums[sub.Nums.Count - 1].IsOne) 
                return false;
            SingleNumComparer comp = new SingleNumComparer();
            for (int i = 0; i < Nums.Count; i++) 
            {
                comp.Process(Nums[i], sub.Nums[i]);
                if (comp.Typ != ComplexNumCompareType.Equals) 
                    return false;
            }
            return true;
        }
    }
}