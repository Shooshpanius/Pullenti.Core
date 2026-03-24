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
    class PersonNormalHelper
    {
        public static void Analyze(Pullenti.Ner.Person.PersonNormalData res, string txt, Dictionary<string, Dictionary<string, string>> corr)
        {
            Dictionary<string, string> co = null;
            if (corr != null && corr.ContainsKey("")) 
                co = corr[""];
            StringBuilder ttt = new StringBuilder(txt);
            for (int i = 1; i < (ttt.Length - 2); i++) 
            {
                if (ttt[i] == '-' && char.IsLower(ttt[i + 1])) 
                    ttt[i + 1] = char.ToUpper(ttt[i + 1]);
                else if (ttt[i] == '-' && ttt[i + 1] == ' ' && char.IsLetter(ttt[i + 2])) 
                {
                    ttt[i + 2] = char.ToUpper(ttt[i + 2]);
                    ttt.Remove(i + 1, 1);
                }
            }
            for (int i = ttt.Length - 2; i >= 0; i--) 
            {
                if (ttt[i] == ' ' && ttt[i + 1] == ' ') 
                    ttt.Remove(i + 1, 1);
                else if (Pullenti.Morph.LanguageHelper.IsHiphen(ttt[i]) && Pullenti.Morph.LanguageHelper.IsHiphen(ttt[i + 1])) 
                    ttt.Remove(i + 1, 1);
                else if (ttt[i] == '\t') 
                    ttt[i] = '\n';
            }
            txt = ttt.ToString();
            res.ResTyp = Pullenti.Ner.Person.PersonNormalResult.NotPerson;
            res.ErrorMessage = "Похоже на просто текст";
            if (txt.Length > 200) 
                return;
            Pullenti.Ner.AnalysisResult ar = null;
            string altSurname = null;
            Dictionary<string, string> addcorr = null;
            for (int kk = 0; kk < 20; kk++) 
            {
                ar = Pullenti.Ner.ProcessorService.EmptyProcessor.Process(new Pullenti.Ner.SourceOfAnalysis(txt) { CorrectionDict = co, DoWordsMergingByMorph = false, DoWordCorrectionByMorph = false }, null, null);
                if (ar == null || ar.FirstToken == null) 
                    return;
                bool ch = false;
                for (Pullenti.Ner.Token t = ar.FirstToken; t != null; t = t.Next) 
                {
                    if (((((t is Pullenti.Ner.TextToken) && !t.Chars.IsLetter && !t.IsWhitespaceBefore) && (t.Previous is Pullenti.Ner.TextToken) && t.Previous.Chars.IsCyrillicLetter) && !t.IsWhitespaceAfter && (t.Next is Pullenti.Ner.TextToken)) && t.Next.Chars.IsCyrillicLetter) 
                    {
                        string str = (t.Previous as Pullenti.Ner.TextToken).Term + (t.Next as Pullenti.Ner.TextToken).Term;
                        Pullenti.Morph.MorphWordForm mb = Pullenti.Morph.MorphologyService.GetWordBaseInfo(str, Pullenti.Morph.MorphLang.RU, true, true);
                        if (mb.Class.IsProperSurname || mb.Class.IsProperName || mb.Class.IsProperSecname) 
                        {
                            bool ok = true;
                            if (t.IsHiphen && !t.Next.Chars.IsAllLower) 
                            {
                                ok = false;
                                if (m_MaleNames.Contains(str) || m_MaleSecnames.Contains(str) || m_FemaleSecnames.Contains(str)) 
                                    ok = true;
                            }
                            if (ok) 
                            {
                                txt = string.Format("{0}{1}", txt.Substring(0, t.BeginChar), txt.Substring(t.EndChar + 1));
                                ch = true;
                                break;
                            }
                        }
                    }
                    if ((((t is Pullenti.Ner.TextToken) && ((t.Chars.IsCapitalUpper || t.Chars.IsAllUpper)) && t.WhitespacesAfterCount == 1) && (t.Next is Pullenti.Ner.TextToken) && ((t.Next.Chars.IsAllLower || t.LengthChar == 1))) && t.Chars.IsLetter && t.Next.Chars.IsLetter) 
                    {
                        if (t.LengthChar == 1 && (t.Next.LengthChar < 4)) 
                            continue;
                        if ((t.LengthChar < 4) && t.Next.LengthChar == 1) 
                            continue;
                        string str = (t as Pullenti.Ner.TextToken).Term + (t.Next as Pullenti.Ner.TextToken).Term;
                        Pullenti.Morph.MorphWordForm mb = Pullenti.Morph.MorphologyService.GetWordBaseInfo(str, Pullenti.Morph.MorphLang.RU, true, true);
                        if (mb.Class.IsProperSurname || mb.Class.IsProperName || mb.Class.IsProperSecname) 
                        {
                            txt = txt.Substring(0, t.EndChar + 1) + txt.Substring(t.Next.BeginChar);
                            ch = true;
                            break;
                        }
                    }
                    else if (((t is Pullenti.Ner.NumberToken) && !t.IsWhitespaceAfter && (t.Next is Pullenti.Ner.TextToken)) && t.Next.Chars.IsLetter) 
                    {
                        string n = (t as Pullenti.Ner.NumberToken).Value;
                        string str = null;
                        if (n == "0") 
                            str = "О" + (t.Next as Pullenti.Ner.TextToken).Term;
                        else if (n == "3") 
                            str = "З" + (t.Next as Pullenti.Ner.TextToken).Term;
                        if (str != null) 
                        {
                            Pullenti.Morph.MorphWordForm mb = Pullenti.Morph.MorphologyService.GetWordBaseInfo(str, Pullenti.Morph.MorphLang.RU, true, true);
                            if ((mb.Class.IsProperSurname || mb.Class.IsProperName || mb.Class.IsProperSecname) || t.LengthChar == 1) 
                            {
                                txt = string.Format("{0}{1}{2}", txt.Substring(0, t.EndChar), str[0], txt.Substring(t.Next.BeginChar));
                                ch = true;
                                break;
                            }
                        }
                        if (!t.IsWhitespaceBefore && (t.Previous is Pullenti.Ner.TextToken) && t.Previous.Chars.IsCyrillicLetter) 
                        {
                            Pullenti.Morph.MorphWordForm mb = Pullenti.Morph.MorphologyService.GetWordBaseInfo((t.Previous as Pullenti.Ner.TextToken).Term + (t.Next as Pullenti.Ner.TextToken).Term, Pullenti.Morph.MorphLang.RU, true, true);
                            if (mb.Class.IsProperSurname || mb.Class.IsProperName || mb.Class.IsProperSecname) 
                            {
                                txt = string.Format("{0}{1}", txt.Substring(0, t.BeginChar), txt.Substring(t.EndChar + 1));
                                ch = true;
                                break;
                            }
                        }
                    }
                    else if (t.IsValue("РАНЕЕ", null) && (t.Next is Pullenti.Ner.TextToken) && t.Previous != null) 
                    {
                        if (t.Next.EndChar < (txt.Length - 1)) 
                            txt = txt.Substring(0, t.Previous.EndChar + 1) + " " + txt.Substring(t.Next.EndChar + 1);
                        else 
                            txt = txt.Substring(0, t.Previous.EndChar + 1);
                        ch = true;
                        break;
                    }
                    else if ((t.IsValue("ДО", null) && (t.Next is Pullenti.Ner.TextToken) && t.Previous != null) && t.Next != null) 
                    {
                        Pullenti.Ner.Token tt = t.Next;
                        if (tt.IsHiphen || tt.IsCharOf("\\/")) 
                        {
                            if (tt.Next != null) 
                                tt = tt.Next;
                        }
                        if (((((tt.IsValue("БРАК", null) || tt.IsValue("ЗАМУЖЕСТВО", null) || tt.IsValue("ПЕРЕМЕНА", null)) || tt.IsValue("ПЕРЕМ", null) || tt.IsValue("ПЕР", null)) || tt.IsValue("СМЕНА", null) || tt.IsValue("БР", null)) || tt.IsValue("РАСТ", null) || tt.IsValue("РАСТОРЖЕНИЕ", null)) || tt.IsValue("РАСТОРЖ", null)) 
                        {
                            for (tt = tt.Next; tt != null; tt = tt.Next) 
                            {
                                if (((tt.IsChar('.') || tt.IsValue("ФАМИЛИЯ", null) || tt.IsValue("ФАМ", null)) || tt.IsValue("ФАМИЛ", null) || tt.IsValue("БР", null)) || tt.IsValue("БРАК", null)) 
                                {
                                }
                                else 
                                    break;
                            }
                            if (tt is Pullenti.Ner.TextToken) 
                            {
                                PersonItemToken ppp = PersonItemToken.TryAttach(tt, PersonItemToken.ParseAttr.CanBeLower, null);
                                if (ppp != null) 
                                {
                                    if (ppp.Lastname != null) 
                                    {
                                        altSurname = ppp.Value;
                                        tt = ppp.EndToken.Next;
                                    }
                                }
                            }
                            if (t.Previous != null && t.Previous.Previous != null && ((t.Previous.IsHiphen || t.Previous.IsCharOf("\\/")))) 
                                t = t.Previous;
                            if (tt != null) 
                                txt = txt.Substring(0, t.Previous.EndChar + 1) + " " + txt.Substring(tt.BeginChar);
                            else 
                                txt = txt.Substring(0, t.Previous.EndChar + 1);
                            ch = true;
                            break;
                        }
                    }
                    else if (t.Next == null && (t is Pullenti.Ner.TextToken) && t.BeginChar > 10) 
                    {
                        Pullenti.Morph.MorphClass mc = t.GetMorphClassInDictionary();
                        if ((mc.IsProperSecname || mc.IsProperName || t.Previous == null) || t.Previous.Previous == null) 
                        {
                        }
                        else 
                        {
                            Pullenti.Ner.Person.PersonNormalData rr = new Pullenti.Ner.Person.PersonNormalData(null);
                            rr.Middlename = (t as Pullenti.Ner.TextToken).Term;
                            _corrCoef(t, rr, corr);
                            if (rr.CorrWords.ContainsKey((t as Pullenti.Ner.TextToken).Term)) 
                            {
                                addcorr = rr.CorrWords;
                                txt = txt.Substring(0, t.BeginChar) + rr.CorrWords[(t as Pullenti.Ner.TextToken).Term];
                                ch = true;
                                break;
                            }
                        }
                    }
                    if ((t is Pullenti.Ner.TextToken) && t.LengthChar > 10 && t.Chars.IsCyrillicLetter) 
                    {
                        if ((!t.Chars.IsAllLower && !t.Chars.IsCapitalUpper && !t.Chars.IsAllUpper) && !t.Chars.IsLastLower) 
                        {
                            string str = t.GetSourceText();
                            if (char.IsUpper(str[0]) && char.IsLower(str[1])) 
                            {
                                for (int ii = 2; ii < (str.Length - 4); ii++) 
                                {
                                    if (char.IsUpper(str[ii]) && char.IsLower(str[ii + 1]) && char.IsLetter(str[ii + 1])) 
                                    {
                                        string s1 = str.Substring(0, ii).ToUpper();
                                        string s2 = str.Substring(ii).ToUpper();
                                        Pullenti.Morph.MorphWordForm b1 = Pullenti.Morph.MorphologyService.GetWordBaseInfo(s1, Pullenti.Morph.MorphLang.RU, true, true);
                                        Pullenti.Morph.MorphWordForm b2 = Pullenti.Morph.MorphologyService.GetWordBaseInfo(s2, Pullenti.Morph.MorphLang.RU, true, true);
                                        if (b1.Class.IsProper && b2.Class.IsProper) 
                                        {
                                            txt = txt.Substring(0, t.BeginChar + ii) + " " + txt.Substring(t.BeginChar + ii);
                                            ch = true;
                                        }
                                        break;
                                    }
                                }
                            }
                            if (ch) 
                                break;
                        }
                    }
                    if (t.IsHiphen) 
                    {
                        bool okk = false;
                        if (t.IsWhitespaceBefore || t.IsWhitespaceAfter) 
                            okk = true;
                        else if ((t.Previous != null && t.Previous.Morph.Class.IsProperSurname && t.Next != null) && t.Next.GetMorphClassInDictionary().IsProperName) 
                            okk = true;
                        if (okk) 
                        {
                            txt = txt.Substring(0, t.BeginChar) + " " + txt.Substring(t.EndChar + 1);
                            ch = true;
                            break;
                        }
                    }
                }
                if (!ch) 
                    break;
            }
            int maxChar = 0;
            for (Pullenti.Ner.Token t = ar.FirstToken; t != null; t = t.Next) 
            {
                if (t.IsValue("ДО", null) && t.Next != null && t.Next.IsValue("БРАК", null)) 
                {
                    maxChar = t.Previous.EndChar;
                    break;
                }
                else if (t.IsValue("БЕЗ", null) && t.Next != null && t.Next.IsValue("ОТЧЕСТВА", null)) 
                {
                    maxChar = t.Previous.EndChar;
                    break;
                }
                else if (t.IsValue("БЕЗ", null) && t.Next != null && t.Next.IsValue("СОГЛАСИЕ", null)) 
                    return;
                else if (t.IsValue("ПО", null) && t.Next != null && t.Next.IsValue("ДАННЫЙ", null)) 
                {
                    maxChar = t.Previous.EndChar;
                    break;
                }
                else if ((t is Pullenti.Ner.TextToken) && t.Next == null) 
                {
                    string term = (t as Pullenti.Ner.TextToken).Term;
                    if (((term == "НЕТ" || term == "РФ" || term == "РОССИЯ") || term == "МУЖ" || term == "ЖЕН") || term == "ЖЕНА") 
                    {
                        maxChar = t.Previous.EndChar;
                        break;
                    }
                }
            }
            int cou = 0;
            int dict = 0;
            int prdict = 0;
            int props = 0;
            int org = 0;
            for (Pullenti.Ner.Token tt = ar.FirstToken; tt != null; tt = tt.Next) 
            {
                if (maxChar > 0 && tt.BeginChar > maxChar) 
                    break;
                cou++;
                Pullenti.Ner.Address.Internal.AddressItemToken ait = Pullenti.Ner.Address.Internal.AddressItemToken.TryParsePureItem(tt, null, null);
                if (ait != null) 
                {
                    if (ait.Typ == Pullenti.Ner.Address.Internal.AddressItemType.Number || ait.Value == null) 
                        continue;
                    res.ResTyp = Pullenti.Ner.Person.PersonNormalResult.NotPerson;
                    res.ErrorMessage = "Похоже на просто текст";
                    return;
                }
                Pullenti.Morph.MorphClass mc = tt.GetMorphClassInDictionary();
                if (mc.IsProperName || mc.IsProperSecname || mc.IsProperSurname) 
                {
                    prdict = 0;
                    dict--;
                    props++;
                    continue;
                }
                Pullenti.Ner.ReferentToken oty = tt.Kit.ProcessReferent("ORGANIZATION", tt, "TYPE");
                if (oty != null && oty.EndToken.IsValue("ОЛИМПИАДА", null)) 
                    oty = null;
                if (oty != null) 
                {
                    org++;
                    if (oty.BeginToken != oty.EndToken || tt == ar.FirstToken) 
                    {
                        bool ok = false;
                        if (oty.EndToken.Next != null) 
                        {
                            Pullenti.Morph.MorphClass mc1 = oty.EndToken.Next.GetMorphClassInDictionary();
                            if (mc1.IsProperName || mc1.IsProperSecname) 
                                ok = true;
                        }
                        if (!ok) 
                        {
                            res.ErrorMessage = "Похоже на организацию";
                            return;
                        }
                    }
                }
                if (Pullenti.Ner.Core.BracketHelper.IsBracket(tt, true)) 
                {
                    if (org > 0) 
                    {
                        res.ErrorMessage = "Похоже на организацию";
                        return;
                    }
                }
                if (tt.LengthChar >= 2 && ((tt.Chars.IsAllLower || !mc.IsUndefined))) 
                {
                    if (!tt.Chars.IsAllLower && (tt.LengthChar < 5)) 
                    {
                    }
                    else if (mc.IsAdjective) 
                    {
                        prdict++;
                        dict += prdict;
                    }
                }
                else 
                    prdict = 0;
            }
            if (dict > (cou - 2) || ((dict == 2 && (cou < 5))) || dict >= 3) 
            {
                if (props < 2) 
                    return;
            }
            if (org > 1 || ((org > 0 && (props < 1)))) 
            {
                res.ErrorMessage = "Похоже на организацию";
                return;
            }
            Pullenti.Ner.Token t0 = ar.FirstToken;
            for (Pullenti.Ner.Token tt = t0; tt != null; tt = tt.Next) 
            {
                if (maxChar > 0 && tt.BeginChar > maxChar) 
                    break;
                if (tt.LengthChar == 1 && !tt.Chars.IsLetter) 
                {
                    t0 = tt.Next;
                    continue;
                }
                if (!tt.Chars.IsAllLower) 
                    break;
                Pullenti.Morph.MorphClass mc = tt.GetMorphClassInDictionary();
                if (mc.IsProper) 
                    break;
                if (mc.IsUndefined) 
                {
                    if (tt.Next != null && tt.Next.IsCharOf(",.")) 
                    {
                    }
                    else 
                        break;
                }
                break;
            }
            PersonItemToken.ParseAttr attrs = (PersonItemToken.ParseAttr.CanBeLower | PersonItemToken.ParseAttr.MustBeItemAlways | PersonItemToken.ParseAttr.NominativeCase) | PersonItemToken.ParseAttr.SurnamePrefixNotMerge;
            List<PersonItemToken> pits = PersonItemToken.TryAttachList(t0, attrs, 10);
            Pullenti.Ner.Token tt0 = ar.FirstToken;
            if (pits != null && pits.Count > 0) 
            {
                PersonItemToken pit = pits[pits.Count - 1];
                tt0 = pit.EndToken.Next;
                if (tt0 != null && pit.Chars.IsAllLower && pits.Count > 1) 
                {
                    tt0 = pit.BeginToken;
                    pits.RemoveAt(pits.Count - 1);
                }
            }
            else 
                pits = new List<PersonItemToken>();
            double coefMult = (double)1;
            for (; tt0 != null; tt0 = tt0.Next) 
            {
                if (maxChar > 0 && tt0.BeginChar > maxChar) 
                    break;
                coefMult *= 0.9;
                List<PersonItemToken> pits1 = PersonItemToken.TryAttachList(tt0, PersonItemToken.ParseAttr.NominativeCase | PersonItemToken.ParseAttr.SurnamePrefixNotMerge, 10);
                if (pits1 != null && pits.Count > 0) 
                {
                    pits.AddRange(pits1);
                    tt0 = pits1[pits1.Count - 1].EndToken;
                }
            }
            if (pits.Count > 15) 
                return;
            if (pits != null && pits.Count > 3) 
            {
                for (int i = 0; i < (pits.Count - 3); i++) 
                {
                    if (pits[i].Value == pits[i + 2].Value && pits[i + 1].Value == pits[i + 3].Value) 
                    {
                        pits[i + 1].EndToken = pits[i + 3].EndToken;
                        pits.RemoveAt(i + 3);
                        pits.RemoveAt(i + 2);
                        i--;
                    }
                }
                for (int i = 0; i < (pits.Count - 2); i++) 
                {
                    if (pits[i].Value == string.Format("{0}-{1}", pits[i + 1].Value, pits[i + 2].Value)) 
                    {
                        pits[i + 1].BeginToken = pits[i].BeginToken;
                        pits.RemoveAt(i);
                        i--;
                    }
                }
                for (int i = 0; i < (pits.Count - 2); i++) 
                {
                    if (pits[i + 2].Value == string.Format("{0}-{1}", pits[i].Value, pits[i + 1].Value)) 
                    {
                        pits[i + 1].EndToken = pits[i + 2].EndToken;
                        pits.RemoveAt(i + 2);
                        i--;
                    }
                }
                for (int i = 0; i < (pits.Count - 1); i++) 
                {
                    if (pits[i].Value == pits[i + 1].Value) 
                    {
                        pits[i].EndToken = pits[i + 1].EndToken;
                        pits.RemoveAt(i + 1);
                        i--;
                    }
                }
            }
            if (pits != null) 
            {
                double coef = m_Fio.Process(pits);
                double ciof = m_Iof.Process(pits);
                ciof *= 0.9;
                PersonNormalNode pn = m_Fio;
                if (coef < ciof) 
                {
                    pn = m_Iof;
                    coef = ciof;
                }
                if (coef > 0.3) 
                {
                    pn.CreateResult(res);
                    res.Coef = (int)((coef * 100 * coefMult));
                    res.LastnameAlt = altSurname;
                    _corrCoef(ar.FirstToken, res, corr);
                    res.ResTyp = Pullenti.Ner.Person.PersonNormalResult.OK;
                    if (res.Coef < 60) 
                        res.ResTyp = Pullenti.Ner.Person.PersonNormalResult.Manual;
                    if (addcorr != null) 
                    {
                        foreach (KeyValuePair<string, string> kp in addcorr) 
                        {
                            if (!res.CorrWords.ContainsKey(kp.Key)) 
                                res.CorrWords.Add(kp.Key, kp.Value);
                        }
                    }
                    return;
                }
            }
            ar = Pullenti.Ner.ProcessorService.StandardProcessor.Process(new Pullenti.Ner.SourceOfAnalysis(txt) { CorrectionDict = co, DoWordCorrectionByMorph = false }, null, null);
            if (ar != null) 
            {
                for (Pullenti.Ner.Token tt = ar.FirstToken; tt != null; tt = tt.Next) 
                {
                    Pullenti.Ner.Referent r = tt.GetReferent();
                    if (r != null && ((r.TypeName == "GEO" || r.TypeName == "STREET" || r.TypeName == "ADDRESS"))) 
                    {
                        res.ErrorMessage = "Похоже на адрес";
                        return;
                    }
                    if (r != null && r.TypeName == "ORGANIZATION") 
                    {
                        res.ErrorMessage = "Похоже на организацию";
                        return;
                    }
                    if (r is Pullenti.Ner.Person.PersonReferent) 
                    {
                        Pullenti.Ner.Person.PersonReferent pr = r as Pullenti.Ner.Person.PersonReferent;
                        res.Coef = 100;
                        res.ResTyp = Pullenti.Ner.Person.PersonNormalResult.OK;
                        if ((((res.Firstname = pr.GetStringValue(Pullenti.Ner.Person.PersonReferent.ATTR_FIRSTNAME)))) == null) 
                            res.Coef = (int)((0.8 * res.Coef));
                        res.Middlename = pr.GetStringValue(Pullenti.Ner.Person.PersonReferent.ATTR_MIDDLENAME);
                        if ((((res.Lastname = pr.GetStringValue(Pullenti.Ner.Person.PersonReferent.ATTR_LASTNAME)))) == null) 
                            res.Coef = (int)((0.5 * res.Coef));
                        for (Pullenti.Ner.Token tt2 = tt.Next; tt2 != null; tt2 = tt2.Next) 
                        {
                            if ((tt2 is Pullenti.Ner.TextToken) && !tt2.Chars.IsLetter) 
                            {
                            }
                            else 
                                res.Coef = (int)((0.5 * res.Coef));
                        }
                        for (Pullenti.Ner.Token tt2 = tt.Previous; tt2 != null; tt2 = tt2.Previous) 
                        {
                            if ((tt2 is Pullenti.Ner.TextToken) && !tt2.Chars.IsLetter) 
                            {
                            }
                            else 
                                res.Coef = (int)((0.5 * res.Coef));
                        }
                        _corrCoef(ar.FirstToken, res, corr);
                        if (res.Coef < 90) 
                            res.ResTyp = Pullenti.Ner.Person.PersonNormalResult.Manual;
                        return;
                    }
                }
            }
        }
        static Dictionary<string, string> CorrMiddnameTails;
        static void _loadCorrMiddlaTails(string txt)
        {
            CorrMiddnameTails = new Dictionary<string, string>();
            foreach (string s in txt.Split('\n')) 
            {
                string line = s.Trim().ToUpper();
                int i = line.IndexOf(":");
                if (i < 0) 
                    i = line.IndexOf(';');
                if (i < 0) 
                    continue;
                string err = line.Substring(0, i);
                if (err.EndsWith("$")) 
                    err = err.Substring(0, err.Length - 1);
                string ok = line.Substring(i + 1);
                if (!CorrMiddnameTails.ContainsKey(err)) 
                    CorrMiddnameTails.Add(err, ok);
            }
        }
        static void _corrCoef(Pullenti.Ner.Token t, Pullenti.Ner.Person.PersonNormalData r, Dictionary<string, Dictionary<string, string>> corr)
        {
            if (t.Kit.CorrectedTokens != null) 
            {
                foreach (KeyValuePair<Pullenti.Ner.Token, string> kp in t.Kit.CorrectedTokens) 
                {
                    if (kp.Key is Pullenti.Ner.TextToken) 
                    {
                        if (!r.CorrWords.ContainsKey(kp.Value)) 
                            r.CorrWords.Add(kp.Value, (kp.Key as Pullenti.Ner.TextToken).Term);
                    }
                }
            }
            if (corr != null) 
            {
                Dictionary<string, string> dic;
                if (r.Firstname != null && corr.TryGetValue("name", out dic)) 
                {
                    if (dic.ContainsKey(r.Firstname)) 
                    {
                        if (!r.CorrWords.ContainsKey(r.Firstname)) 
                            r.CorrWords.Add(r.Firstname, dic[r.Firstname]);
                        r.Firstname = dic[r.Firstname];
                        r.Coef = (int)((r.Coef * 0.95));
                    }
                }
                if (r.Middlename != null && corr.TryGetValue("midname", out dic)) 
                {
                    if (dic.ContainsKey(r.Middlename)) 
                    {
                        if (!r.CorrWords.ContainsKey(r.Middlename)) 
                            r.CorrWords.Add(r.Middlename, dic[r.Middlename]);
                        r.Middlename = dic[r.Middlename];
                        r.Coef = (int)((r.Coef * 0.95));
                    }
                }
                if (r.Lastname != null && corr.TryGetValue("lastname", out dic)) 
                {
                    if (dic.ContainsKey(r.Lastname)) 
                    {
                        if (!r.CorrWords.ContainsKey(r.Lastname)) 
                            r.CorrWords.Add(r.Lastname, dic[r.Lastname]);
                        r.Lastname = dic[r.Lastname];
                        r.Coef = (int)((r.Coef * 0.95));
                    }
                }
            }
            if (r.Middlename != null) 
            {
                foreach (KeyValuePair<string, string> kp in CorrMiddnameTails) 
                {
                    if (r.Middlename.EndsWith(kp.Key)) 
                    {
                        string mmm = r.Middlename;
                        r.Middlename = r.Middlename.Substring(0, r.Middlename.Length - kp.Key.Length);
                        r.Middlename += kp.Value;
                        r.Coef = (int)((r.Coef * 0.95));
                        if (!r.CorrWords.ContainsKey(mmm)) 
                            r.CorrWords.Add(mmm, r.Middlename);
                        break;
                    }
                }
            }
            if (r.FirstnameAlt != null) 
            {
                if (ShortNameHelper.GetNamesForShortname(r.FirstnameAlt) == null) 
                {
                    if (r.FirstnameAlt != "СВЕТА") 
                    {
                        if (m_MaleNames.Contains(r.Firstname)) 
                        {
                            if (!r.CorrWords.ContainsKey(r.FirstnameAlt)) 
                                r.CorrWords.Add(r.FirstnameAlt, r.Firstname);
                        }
                        else 
                            r.Firstname = r.FirstnameAlt;
                        r.FirstnameAlt = null;
                    }
                }
            }
            if (r.Firstname != null) 
            {
                string cc = _corrDublsChars(r.Firstname, 1, r.Gender);
                if (cc != null) 
                {
                    if (!r.CorrWords.ContainsKey(r.Firstname)) 
                        r.CorrWords.Add(r.Firstname, cc);
                    r.Firstname = cc;
                    r.Coef = (int)((r.Coef * 0.95));
                }
            }
            if (r.Middlename != null) 
            {
                string cc = _corrDublsChars(r.Middlename, 2, r.Gender);
                if (cc != null) 
                {
                    if (!r.CorrWords.ContainsKey(r.Middlename)) 
                        r.CorrWords.Add(r.Middlename, cc);
                    r.Middlename = cc;
                    r.Coef = (int)((r.Coef * 0.95));
                }
                Pullenti.Morph.MorphWordForm bb = Pullenti.Morph.MorphologyService.GetWordBaseInfo(r.Middlename, Pullenti.Morph.MorphLang.RU, true, true);
                if (!bb.Class.IsProperSecname && (r.Middlename.IndexOf('-') < 0)) 
                {
                    cc = Pullenti.Morph.MorphologyService.CorrectWord(r.Middlename, null);
                    if (cc != null) 
                    {
                        if (((r.Gender == 1 && m_MaleSecnames.Contains(cc))) || ((r.Gender == 2 && m_FemaleSecnames.Contains(cc)))) 
                        {
                            if (!r.CorrWords.ContainsKey(r.Middlename)) 
                                r.CorrWords.Add(r.Middlename, cc);
                            r.Middlename = cc;
                            r.Coef = (int)((r.Coef * 0.95));
                        }
                    }
                }
            }
            if (r.Lastname != null) 
            {
                int glas = 0;
                foreach (char ch in r.Lastname) 
                {
                    if (Pullenti.Morph.LanguageHelper.IsCyrillicVowel(ch)) 
                        glas++;
                }
                if (glas == 0) 
                    r.Coef = (int)((r.Coef * 0.8));
                else if (glas > 2 && glas == r.Lastname.Length) 
                    r.Coef = (int)((r.Coef * 0.6));
                string cc = _corrDublsChars(r.Lastname, 3, r.Gender);
                if (cc != null) 
                {
                    if (!r.CorrWords.ContainsKey(r.Lastname)) 
                        r.CorrWords.Add(r.Lastname, cc);
                    r.Lastname = cc;
                    r.Coef = (int)((r.Coef * 0.95));
                }
            }
        }
        static string _corrDublsChars(string str, int typ, int gender)
        {
            if (str.Length < 4) 
                return null;
            int cou = 0;
            for (int i = str.Length - 2; i > 0; i--) 
            {
                if (str[i] != str[str.Length - 1]) 
                {
                    if (cou >= 2) 
                    {
                        string res = str.Substring(0, i + 2);
                        Pullenti.Morph.MorphWordForm bb = Pullenti.Morph.MorphologyService.GetWordBaseInfo(res, Pullenti.Morph.MorphLang.RU, true, true);
                        if (bb.Class.IsProper) 
                            return res;
                        string res0 = res.Substring(0, res.Length - 1);
                        bb = Pullenti.Morph.MorphologyService.GetWordBaseInfo(res0, Pullenti.Morph.MorphLang.RU, true, true);
                        if (bb.Class.IsProper) 
                            return res0;
                        return res;
                    }
                    break;
                }
                else 
                    cou++;
            }
            if (typ == 3) 
                return null;
            if (typ == 2) 
            {
                if (gender == 1 && !str.EndsWith("ИЧ")) 
                {
                    string str1 = str.Substring(0, str.Length - 1);
                    if (str1.EndsWith("ИЧ")) 
                        return str1;
                }
            }
            Pullenti.Morph.MorphWordForm bb0 = Pullenti.Morph.MorphologyService.GetWordBaseInfo(str, Pullenti.Morph.MorphLang.RU, true, true);
            if (typ == 1 && Pullenti.Morph.LanguageHelper.IsCyrillicVowel(str[str.Length - 1])) 
            {
                string str1 = str + "Й";
                if (m_MaleNames.Contains(str1)) 
                    return str1;
            }
            if (bb0.Class.IsProper) 
                return null;
            string str0 = str.Substring(0, str.Length - 1);
            bb0 = Pullenti.Morph.MorphologyService.GetWordBaseInfo(str0, Pullenti.Morph.MorphLang.RU, true, true);
            if (bb0.Class.IsProperName && typ == 1 && bb0.Number == Pullenti.Morph.MorphNumber.Singular) 
            {
                if (str0[str0.Length - 1] == str[str.Length - 1]) 
                    return str0;
            }
            if (bb0.Class.IsProperSecname && typ == 2 && bb0.Number == Pullenti.Morph.MorphNumber.Singular) 
                return str0;
            return null;
        }
        static List<string> m_MaleNames = new List<string>(new string[] {"АНДРЕЙ", "АЛЕКСЕЙ", "АЛЕКСАНДР", "АНАТОЛИЙ", "АРКАДИЙ", "АРТЕМ", "БОРИС", "ВАЛЕНТИН", "ВАЛЕРИЙ", "ВАСИЛИЙ", "ВИКТОР", "ВИТАЛИЙ", "ВЛАДИМИР", "ВЛАДИСЛАВ", "ВСЕВОЛОД", "ГЕННАДИЙ", "ГЕНАДИЙ", "ГЕОРГИЙ", "ГРИГОРИЙ", "ДЕНИС", "ДМИТРИЙ", "ЕВГЕНИЙ", "ЕГОР", "ИВАН", "ИГОРЬ", "КИРИЛЛ", "КОНСТАНТИН", "ЛЕОНИД", "ЛЕВ", "МАКСИМ", "МИХАИЛ", "НИКОЛАЙ", "НИКИТА", "ОЛЕГ", "ПАВЕЛ", "ПЕТР", "РОМАН", "СЕМЕН", "СЕРГЕЙ", "СТАНИСЛАВ", "СТЕПАН", "ФЕДОР", "ЭДУАРД", "ЮРИЙ", "ЯКОВ", "ЯРОСЛАВ"});
        static List<string> m_MaleSecnames = new List<string>(new string[] {"АЛЕКСЕЕВИЧ", "АЛЕКСАНДРОВИЧ", "АНАТОЛЬЕВИЧ", "АЛЬБЕРТОВИЧ", "АНДРЕЕВИЧ", "АРКАДЬЕВИЧ", "АРТЕМОВИЧ", "БОРИСОВИЧ", "ВАЛЕНТИНОВИЧ", "ВАЛЕРЬЕВИЧ", "ВАСИЛЬЕВИЧ", "ВИКТОРОВИЧ", "ВИТАЛЬЕВИЧ", "ВЛАДИМИРОВИЧ", "ВЛАДИСЛАВОВИЧ", "ВСЕВОЛОДОВИЧ", "ГЕННАДИЕВИЧ", "ГЕНАДИЕВИЧ", "ГЕОРГИЕВИЧ", "ГРИГОРЬЕВИЧ", "ДЕНИСОВИЧ", "ДМИТРИЕВИЧ", "ЕВГЕНЬЕВИЧ", "ЕГОРОВИЧ", "ИВАНОВИЧ", "ИГОРЕВИЧ", "ИЛЬИЧ", "КИРИЛЛОВИЧ", "КОНСТАНТИНОВИЧ", "ЛЕОНИДОВИЧ", "ЛЬВОВИЧ", "МАКСИМОВИЧ", "МИХАЙЛОВИЧ", "НИКОЛАЕВИЧ", "НИКИТИЧ", "ОЛЕГОВИЧ", "ПАВЛОВИЧ", "ПЕТРОВИЧ", "РОМАНОВИЧ", "СЕМЕНОВИЧ", "СЕРГЕЕВИЧ", "СТАНИСЛАВОВИЧ", "СТЕПАНОВИЧ", "ФЕДОРОВИЧ", "ЭДУАРДОВИЧ", "ЮРЬЕВИЧ", "ЯКОВЛЕВИЧ", "ЯРОСЛАВОВИЧ"});
        static List<string> m_FemaleSecnames = new List<string>(new string[] {"АЛЕКСЕЕВНА", "АЛЕКСАНДРОВНА", "АНАТОЛЬЕВНА", "АЛЬБЕРТОВНА", "АНДРЕЕВНА", "АРКАДЬЕВНА", "АРТЕМОВНА", "БОРИСОВНА", "ВАЛЕНТИНОВНА", "ВАЛЕРЬЕВНА", "ВАСИЛЬЕВНА", "ВИКТОРОВНА", "ВИТАЛЬЕВНА", "ВЛАДИМИРОВНА", "ВЛАДИСЛАВОВНА", "ВСЕВОЛОДОВНА", "ГЕННАДИЕВНА", "ГЕНАДИЕВНА", "ГЕОРГИЕВНА", "ГРИГОРЬЕВНА", "ДЕНИСОВНА", "ДМИТРИЕВНА", "ЕВГЕНЬЕВНА", "ЕГОРОВНА", "ИВАНОВНА", "ИГОРЕВНА", "ИЛЬИНИЧНА", "КИРИЛЛОВНА", "КОНСТАНТИНОВНА", "ЛЕОНИДОВНА", "ЛЬВОВНА", "МАКСИМОВНА", "МИХАЙЛОВНА", "НИКОЛАЕВНА", "НИКИТИЧНА", "ОЛЕГОВНА", "ПАВЛОВНА", "ПЕТРОВНА", "РОМАНОВНА", "СЕМЕНОВНА", "СЕРГЕЕВНА", "СТАНИСЛАВОВНА", "СТЕПАНОВНА", "ФЕДОРОВНА", "ЭДУАРДОВНА", "ЮРЬЕВНА", "ЯКОВЛЕВНА", "ЯРОСЛАВОВНА"});
        static PersonNormalNode m_Fio = new PersonNormalNode(false);
        static PersonNormalNode m_Iof = new PersonNormalNode(true);
        public static void Initialize()
        {
            _loadCorrMiddlaTails("слаовна$:славовна\nславона$:славовна\nславоич$:славович\nслаович$:славович\nвнана$:вна\nевана$:евна\nевнва$:евна\nевнаа$:евна\nевнна$:евна\nована$:овна\nовнва$:овна\nовнаа$:овна\nовнна$:овна\nевена$:евна\nевсна$:евна\nевона$:евна\nовена$:овна\nовсна$:овна\nовона$:овна\nовоич$:ович\nевича$:евич\nевичч$:евич\nевивч$:евич\nевиич$:евич\nеваич$:евич\nевнич$:евич\nовича$:ович\nовичч$:ович\nовивч$:ович\nовиич$:ович\nоваич$:ович\nовнич$:ович\nеана$:евна\nенва$:евна\nевнв$:евна\nевне$:евна\nевну$:евна\nевны$:евна\nевеа$:евна\nеван$:евна\nоана$:овна\nонва$:овна\nовнв$:овна\nовне$:овна\nовну$:овна\nовны$:овна\nовеа$:овна\nован$:овна\nевга$:евна\nовга$:овна\nвеич$:евич\nвоич$:ович\nивоч$:ович\nовоч$:ович\nевия$:евич\nевмч$:евич\nеивч$:евич\nеаич$:евич\nивеч$:евич\nевоч$:евич\nовия$:ович\nовмч$:ович\nоивч$:ович\nеаич$:ович\nевн$:евна\nена$:евна\nовн$:овна\nона$:овна\nеич$:евич\nови$:ович\nоич$:ович\nевч$:евич\nеви$:евич\nовч$:ович\nови$:ович\nишна$:ична\nофич$:ович\nефич$:евич\nева$:евна\nова$:овна");
        }
    }
}