/*
 * SDK Pullenti Lingvo, version 4.33, fabruary 2026. Copyright (c) 2013-2026, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Ner.Person.Internal
{
    class PersonNormalNode
    {
        public PersonNormalNode(bool fioLast)
        {
            if (!fioLast) 
                Items.Add(new PersonNormalItem() { Typ = PersonNormalItemType.Last });
            Items.Add(new PersonNormalItem() { Typ = PersonNormalItemType.First });
            Items.Add(new PersonNormalItem() { Typ = PersonNormalItemType.Middle });
            if (fioLast) 
                Items.Add(new PersonNormalItem() { Typ = PersonNormalItemType.Last });
        }
        public List<PersonNormalItem> Items = new List<PersonNormalItem>();
        static bool IsAsian(string str)
        {
            int gl = 0;
            foreach (char ch in str) 
            {
                if (Pullenti.Morph.LanguageHelper.IsCyrillicVowel(ch)) 
                    gl++;
            }
            if (gl == 1) 
                return true;
            return false;
        }
        public double Process(List<PersonItemToken> src)
        {
            int i;
            for (i = 0; i < Items.Count; i++) 
            {
                Items[i].Init(src);
            }
            if ((src.Count == 3 && IsAsian(src[0].Value) && IsAsian(src[1].Value)) && IsAsian(src[2].Value)) 
            {
                if (Items[0].Typ == PersonNormalItemType.First) 
                    return 0;
                for (i = 0; i < 3; i++) 
                {
                    Items[i].Rnd0 = (Items[i].Rnd1 = i);
                }
                return 1;
            }
            double bestCoef = (double)0;
            for (int fem = 0; fem < 2; fem++) 
            {
                for (int i0 = 0; i0 < (src.Count - 2); i0++) 
                {
                    Items[0].Ind0 = 0;
                    Items[0].Ind1 = i0;
                    Items[0].Gender = fem + 1;
                    double co = Items[0].CalcCoef();
                    if (co <= bestCoef) 
                        continue;
                    for (int i1 = i0 + 1; i1 < (src.Count - 1); i1++) 
                    {
                        Items[1].Ind0 = i0 + 1;
                        Items[1].Ind1 = i1;
                        Items[1].Gender = fem + 1;
                        double co1 = Items[1].CalcCoef();
                        if ((co * co1) <= bestCoef) 
                            continue;
                        for (int i2 = i1 + 1; i2 < src.Count; i2++) 
                        {
                            Items[2].Ind0 = i1 + 1;
                            Items[2].Ind1 = i2;
                            Items[2].Gender = fem + 1;
                            double co2 = Items[2].CalcCoef();
                            co2 *= (co1 * co);
                            for (i = i2 + 1; i < src.Count; i++) 
                            {
                                if (src[i].Chars.IsAllLower) 
                                    co2 *= 0.9;
                                else 
                                    co2 *= 0.3;
                            }
                            if (co2 > bestCoef) 
                            {
                                bestCoef = co2;
                                for (i = 0; i < Items.Count; i++) 
                                {
                                    Items[i].Fix();
                                }
                            }
                        }
                    }
                }
            }
            if (bestCoef == 0 && src.Count == 2) 
            {
                for (i = 0; i < Items.Count; i++) 
                {
                    Items[i].Init(src);
                }
                for (int fem = 0; fem < 2; fem++) 
                {
                    for (int i0 = 0; i0 < (src.Count - 1); i0++) 
                    {
                        Items[0].Ind0 = 0;
                        Items[0].Ind1 = i0;
                        Items[0].Gender = fem + 1;
                        double co = Items[0].CalcCoef();
                        if (co <= bestCoef) 
                            continue;
                        int k = 1;
                        if (Items[k].Typ == PersonNormalItemType.Middle) 
                            k++;
                        for (int i1 = i0 + 1; i1 < src.Count; i1++) 
                        {
                            Items[k].Ind0 = i0 + 1;
                            Items[k].Ind1 = i1;
                            Items[k].Gender = fem + 1;
                            double co1 = Items[k].CalcCoef();
                            if ((co * co1) <= bestCoef) 
                                continue;
                            bestCoef = co * co1;
                            for (i = 0; i < Items.Count; i++) 
                            {
                                Items[i].Fix();
                            }
                        }
                    }
                }
            }
            return bestCoef;
        }
        public void CreateResult(Pullenti.Ner.Person.PersonNormalData res)
        {
            res.Gender = Items[0].Render;
            foreach (PersonNormalItem it in Items) 
            {
                if (it.Typ == PersonNormalItemType.First) 
                {
                    res.Firstname = it.Result;
                    res.FirstnameAlt = it.ResultAlt;
                }
                else if (it.Typ == PersonNormalItemType.Middle) 
                    res.Middlename = it.Result;
                else if (it.Typ == PersonNormalItemType.Last) 
                    res.Lastname = it.Result;
            }
        }
    }
}