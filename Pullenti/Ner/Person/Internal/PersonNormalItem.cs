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

namespace Pullenti.Ner.Person.Internal
{
    class PersonNormalItem
    {
        public PersonNormalItemType Typ;
        public int Ind0;
        public int Ind1;
        public List<PersonItemToken> Src;
        public int Gender;
        public int Rnd0;
        public int Rnd1;
        public int Render;
        public override string ToString()
        {
            if ((Rnd0 < 0) || (Rnd1 < Rnd0) || Src == null) 
                return "?";
            if (Rnd0 == Rnd1) 
                return string.Format("{0}: {1}", Typ, Src[Rnd0].Value);
            StringBuilder res = new StringBuilder();
            res.AppendFormat("{0}: ", Typ);
            for (int i = Rnd0; i <= Rnd1; i++) 
            {
                res.AppendFormat("{0}{1}", (i > Rnd0 ? "-" : ""), Src[i].Value);
            }
            return res.ToString();
        }
        public string ResultAlt;
        public string Result
        {
            get
            {
                ResultAlt = null;
                if ((Rnd0 < 0) || (Rnd1 < Rnd0) || Src == null) 
                    return null;
                if ((Rnd0 + 1) == Rnd1 && Typ != PersonNormalItemType.Last) 
                {
                    if (Src[Rnd0].Value.Length == 1 && Src[Rnd1].Value.Length > 1) 
                    {
                        if (!Src[Rnd0].EndToken.Next.IsHiphen) 
                            return Src[Rnd1].Value;
                    }
                    if (Src[Rnd0].Value.Length > 1 && Src[Rnd1].Value.Length == 1) 
                        return Src[Rnd0].Value;
                }
                string res = Src[Rnd0].Value;
                for (int i = Rnd0 + 1; i <= Rnd1; i++) 
                {
                    if (Typ == PersonNormalItemType.Last && (Rnd0 + 1) == Rnd1 && res.Length == 1) 
                    {
                        if (res == "Д" || res == "О") 
                        {
                            res = string.Format("{0}'{1}", res, Src[i].Value);
                            continue;
                        }
                        if (!Pullenti.Morph.LanguageHelper.IsCyrillicVowel(res[0])) 
                        {
                            res = string.Format("{0}{1}", res, Src[i].Value);
                            continue;
                        }
                    }
                    if (i == Rnd1 && !Src[i].EndToken.Previous.IsHiphen) 
                    {
                        int ii;
                        string v = Src[i].Value;
                        for (ii = 1; ii < v.Length; ii++) 
                        {
                            if (v[ii] != v[0]) 
                                break;
                        }
                        if (ii >= v.Length) 
                            continue;
                    }
                    res = string.Format("{0}-{1}", res, Src[i].Value);
                }
                if (Typ == PersonNormalItemType.First && Rnd0 == Rnd1) 
                {
                    PersonItemToken pit = Src[Rnd0];
                    if (pit.Firstname != null && pit.Firstname.IsInDictionary && pit.Firstname.Vars.Count > 0) 
                    {
                        foreach (PersonItemToken.MorphPersonItemVariant v in pit.Firstname.Vars) 
                        {
                            if (v.Gender == Pullenti.Morph.MorphGender.Masculine && Render == 1) 
                            {
                                bool exi = false;
                                foreach (PersonItemToken.MorphPersonItemVariant vv in pit.Firstname.Vars) 
                                {
                                    if (vv.Gender == Pullenti.Morph.MorphGender.Masculine && Render == 1 && vv.Value == pit.Value) 
                                        exi = true;
                                }
                                if ((!exi && v.Value != pit.Value && pit.Value != "ВЛАД") && pit.Value != "АЛЕКС") 
                                {
                                    ResultAlt = pit.Value;
                                    return v.Value;
                                }
                            }
                            else if (v.Gender == Pullenti.Morph.MorphGender.Feminie && Render == 2) 
                            {
                                bool exi = false;
                                foreach (PersonItemToken.MorphPersonItemVariant vv in pit.Firstname.Vars) 
                                {
                                    if (vv.Gender == Pullenti.Morph.MorphGender.Feminie && Render == 2 && vv.Value == pit.Value) 
                                        exi = true;
                                }
                                if ((!exi && v.Value != pit.Value && pit.Value != "КАТЕРИНА") && pit.Value != "МАРЬЯ" && pit.Value != "МАРИЯ") 
                                {
                                    ResultAlt = pit.Value;
                                    if (pit.Value == "ЛЕНА") 
                                        return "ЕЛЕНА";
                                    return v.Value;
                                }
                            }
                        }
                    }
                }
                if ((Typ == PersonNormalItemType.Last && Rnd0 == Rnd1 && ((res[0] == 'О' || res[0] == 'Д'))) && res.Length > 3) 
                {
                    PersonItemToken pit = Src[Rnd0];
                    string txt = pit.GetSourceText();
                    for (int i = 0; (i < txt.Length) && (i < 3); i++) 
                    {
                        if (Pullenti.Ner.Core.BracketHelper.IsBracketChar(txt[i], false)) 
                        {
                            res = string.Format("{0}'{1}", res[0], res.Substring(1));
                            break;
                        }
                    }
                }
                return res;
            }
        }
        public void Init(List<PersonItemToken> src)
        {
            Ind0 = 0;
            Ind1 = -1;
            Rnd0 = (Rnd1 = -1);
            Src = src;
            Render = 0;
            Gender = 0;
        }
        public void Fix()
        {
            Rnd0 = Ind0;
            Rnd1 = Ind1;
            Render = Gender;
        }
        public double CalcCoef()
        {
            if (Ind1 < Ind0) 
            {
                if (Typ == PersonNormalItemType.Middle) 
                    return 1;
                return 0;
            }
            if (Ind1 > Ind0) 
            {
                for (int i = Ind0; i < Ind1; i++) 
                {
                    if (Src[i].WhitespacesAfterCount >= 4) 
                        return 0;
                }
            }
            if (Ind0 > 0 && Src[Ind0].BeginToken.Previous.IsHiphen) 
            {
                if (Src[Ind0].WhitespacesBeforeCount < 4) 
                    return 0;
            }
            if (((Ind1 + 1) < Src.Count) && Src[Ind1].EndToken.Next.IsHiphen) 
            {
                if (Src[Ind1].EndToken.Next.WhitespacesAfterCount < 4) 
                    return 0;
            }
            double co = (double)1;
            for (int i = Ind0; i <= Ind1; i++) 
            {
                PersonItemToken pit = Src[i];
                if (Typ == PersonNormalItemType.First) 
                {
                    if (Ind0 == Ind1 && pit.Value.Length == 1) 
                    {
                    }
                    else if (pit.Firstname != null) 
                    {
                        if (Gender == 2 && pit.Firstname.Morph.Gender == Pullenti.Morph.MorphGender.Feminie) 
                        {
                        }
                        else if (Gender == 1 && pit.Firstname.Morph.Gender == Pullenti.Morph.MorphGender.Masculine) 
                        {
                        }
                        else 
                            co *= 0.98;
                    }
                    else if (pit.Middlename != null) 
                        co *= 0.8;
                    else if (pit.Lastname != null && ((pit.Lastname.IsInDictionary || pit.Lastname.IsLastnameHasStdTail))) 
                        co *= 0.8;
                    else if (pit.Lastname != null && pit.Lastname.IsLastnameHasStdTail) 
                        co *= 0.9;
                    else if (Gender == 2 && pit.Morph.Gender == Pullenti.Morph.MorphGender.Feminie) 
                        co *= 0.98;
                    else if (Gender == 1 && pit.Morph.Gender == Pullenti.Morph.MorphGender.Masculine) 
                        co *= 0.98;
                    else 
                        co *= 0.7;
                }
                else if (Typ == PersonNormalItemType.Middle) 
                {
                    if (Ind0 == Ind1 && pit.Value.Length == 1) 
                    {
                    }
                    else if (pit.Middlename != null) 
                    {
                        if (Gender == 2 && pit.Middlename.Morph.Gender == Pullenti.Morph.MorphGender.Feminie) 
                        {
                        }
                        else if (Gender == 1 && pit.Middlename.Morph.Gender == Pullenti.Morph.MorphGender.Masculine) 
                        {
                        }
                        else 
                            co *= 0.7;
                    }
                    else if (((pit.Value.EndsWith("ВНА") || pit.Value.EndsWith("ЧНА") || pit.Value.EndsWith("КЫЗЫ"))) && Gender == 2) 
                    {
                    }
                    else if (((pit.Value.EndsWith("ИЧ") || pit.Value.EndsWith("ОГЛЫ"))) && Gender == 1) 
                        co *= 0.99;
                    else if (pit.Firstname != null) 
                        co *= 0.98;
                    else if (((Gender == 2 && pit.Value.EndsWith("ВА") && Ind0 == Ind1) && Ind0 == (Src.Count - 1) && Src[0].Lastname != null) && Src[0].Value.Length > 2) 
                        co *= 0.99;
                    else if (pit.Lastname != null && pit.Lastname.IsInDictionary) 
                        co *= 0.8;
                    else 
                        co *= 0.7;
                }
                else if (Typ == PersonNormalItemType.Last) 
                {
                    if (pit.Value.EndsWith("КЫЗЫ")) 
                        return 0;
                    if (Ind0 == Ind1 && pit.Value.Length == 1) 
                    {
                        if (Pullenti.Morph.LanguageHelper.IsCyrillicVowel(pit.Value[0])) 
                            co *= 0.5;
                        else 
                            return 0;
                    }
                    if (pit.Value.EndsWith("ВНА") || pit.Value.EndsWith("ЧНА")) 
                    {
                        if (Gender == 2) 
                        {
                            if (Ind0 == 0) 
                                co *= 0.7;
                            else 
                                co *= 0.2;
                        }
                        else 
                            co *= 0.3;
                    }
                    else if ((Gender == 2 && pit.Value.EndsWith("ВА") && Ind0 == Ind1) && Ind0 == 2 && Src[0].Lastname != null) 
                        co *= 0.3;
                    else if (pit.Value.EndsWith("ИЧ")) 
                    {
                        if ((i + 1) >= Src.Count) 
                            co *= 0.3;
                        else if (Ind0 == 0 || Gender == 2) 
                        {
                        }
                        else 
                            co *= 0.85;
                    }
                    else if (pit.Value.EndsWith("ОГЛЫ")) 
                        co *= 0.3;
                    else if (pit.Lastname != null && ((pit.Lastname.IsInDictionary || pit.Lastname.IsLastnameHasStdTail))) 
                    {
                        if (Gender == 2 && pit.Lastname.Morph.Gender == Pullenti.Morph.MorphGender.Feminie) 
                        {
                        }
                        else if (Gender == 1 && pit.Lastname.Morph.Gender == Pullenti.Morph.MorphGender.Masculine) 
                        {
                        }
                        else 
                            co *= 0.98;
                    }
                    else if (pit.Firstname != null || pit.Middlename != null) 
                        co *= 0.8;
                    else 
                        co *= 0.98;
                }
            }
            if ((Typ == PersonNormalItemType.Last && Ind0 == 0 && Ind1 > (Ind0 + 1)) && (co < 0.6)) 
                co *= 1.5;
            return co;
        }
    }
}