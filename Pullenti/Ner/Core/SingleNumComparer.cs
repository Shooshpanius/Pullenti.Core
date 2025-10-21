/*
 * SDK Pullenti Lingvo, version 4.31, august 2025. Copyright (c) 2013-2025, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Text;

namespace Pullenti.Ner.Core
{
    class SingleNumComparer
    {
        public ComplexNumCompareType Typ;
        public double Rank;
        public int Delta;
        public SingleNumToken First;
        public SingleNumToken Second;
        public SingleNumValue Val1;
        public SingleNumValue Val2;
        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            if (Rank > 0) 
                res.AppendFormat("{0}: ", Rank);
            res.Append(First);
            if (Typ == ComplexNumCompareType.Uncomparable) 
                res.Append(" ?? ");
            else if (Typ == ComplexNumCompareType.Equals) 
                res.Append(" == ");
            else if (Typ == ComplexNumCompareType.Less) 
                res.Append(" < ");
            else if (Typ == ComplexNumCompareType.Great) 
                res.Append(" > ");
            res.Append(Second);
            if (Delta > 0) 
                res.AppendFormat(", Delt={0}", Delta);
            return res.ToString();
        }
        public void Process(SingleNumToken fir, SingleNumToken sec)
        {
            First = fir;
            Second = sec;
            Typ = ComplexNumCompareType.Uncomparable;
            Delta = 0;
            Rank = 0;
            Val1 = null;
            Val2 = null;
            foreach (SingleNumValue v1 in fir.Vals) 
            {
                foreach (SingleNumValue v2 in sec.Vals) 
                {
                    if (v1.Typ != v2.Typ) 
                        continue;
                    double rank = (double)1;
                    ComplexNumCompareType ty = ComplexNumCompareType.Uncomparable;
                    int delt = 0;
                    if (v1.Typ == SingleNumValueType.Letter) 
                    {
                        char ch1 = char.ToUpper(v1.Letter);
                        char ch2 = char.ToUpper(v2.Letter);
                        if (Pullenti.Morph.LanguageHelper.IsCyrillicChar(ch1) != Pullenti.Morph.LanguageHelper.IsCyrillicChar(ch2)) 
                        {
                            if (Pullenti.Morph.LanguageHelper.IsCyrillicChar(ch1)) 
                            {
                                char ch11 = Pullenti.Morph.LanguageHelper.GetLatForCyr(ch1);
                                if (ch11 != 0) 
                                    ch1 = ch11;
                                else 
                                {
                                    char ch22 = Pullenti.Morph.LanguageHelper.GetCyrForLat(ch2);
                                    if (ch22 != 0) 
                                        ch2 = ch22;
                                }
                            }
                            else 
                            {
                                char ch11 = Pullenti.Morph.LanguageHelper.GetCyrForLat(ch1);
                                if (ch11 != 0) 
                                    ch1 = ch11;
                                else 
                                {
                                    char ch22 = Pullenti.Morph.LanguageHelper.GetLatForCyr(ch2);
                                    if (ch22 != 0) 
                                        ch2 = ch22;
                                }
                            }
                        }
                        if (((int)ch1) < ((int)ch2)) 
                        {
                            ty = ComplexNumCompareType.Less;
                            delt = ((int)ch2) - ((int)ch1);
                            if (ch1 == 'И' && ch2 == 'К') 
                                delt = 1;
                            else if (ch1 == 'Е' && ch2 == 'Ж') 
                                delt = 1;
                        }
                        else if (((int)ch1) > ((int)ch2)) 
                        {
                            ty = ComplexNumCompareType.Great;
                            delt = ((int)ch1) - ((int)ch2);
                            if (ch2 == 'И' && ch1 == 'К') 
                                delt = 1;
                            else if (ch2 == 'Е' && ch1 == 'Ж') 
                                delt = 1;
                        }
                        else 
                            ty = ComplexNumCompareType.Equals;
                    }
                    else if (v1.Val < v2.Val) 
                    {
                        ty = ComplexNumCompareType.Less;
                        delt = v2.Val - v1.Val;
                    }
                    else if (v1.Val > v2.Val) 
                    {
                        ty = ComplexNumCompareType.Great;
                        delt = v1.Val - v2.Val;
                    }
                    else 
                        ty = ComplexNumCompareType.Equals;
                    if (ty == ComplexNumCompareType.Great) 
                        rank /= 2;
                    if (rank > Rank) 
                    {
                        Rank = rank;
                        Typ = ty;
                        Delta = delt;
                        Val1 = v1;
                        Val2 = v2;
                        if (Delta > 1) 
                            Rank *= 0.98;
                    }
                }
            }
            if (fir.Suffix != null && sec.Suffix != null) 
            {
                if (fir.Suffix != sec.Suffix) 
                    Rank *= 0.8;
            }
            else if (fir.Suffix != null || sec.Suffix != null) 
                Rank *= 0.9;
        }
    }
}