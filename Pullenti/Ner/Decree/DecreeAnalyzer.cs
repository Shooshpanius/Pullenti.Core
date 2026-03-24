/*
 * SDK Pullenti Lingvo, version 4.33, fabruary 2026. Copyright (c) 2013-2026, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Ner.Decree
{
    /// <summary>
    /// Анализатор ссылок на НПА
    /// </summary>
    public class DecreeAnalyzer : Pullenti.Ner.Analyzer
    {
        internal static List<Pullenti.Ner.ReferentToken> TryAttach(List<Pullenti.Ner.Decree.Internal.DecreeToken> dts, Pullenti.Ner.Decree.Internal.DecreeToken baseTyp, Pullenti.Ner.Core.AnalyzerData ad)
        {
            if (ad == null) 
                ad = GetData(dts[0]);
            if (ad.Level > 2) 
                return null;
            ad.Level++;
            List<Pullenti.Ner.ReferentToken> res = _TryAttach(dts, baseTyp, false, ad);
            if (res == null && ((dts.Count == 1 || ((dts.Count == 2 && dts[1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Org)))) && dts[0].TypRefBack) 
            {
                for (Pullenti.Ner.Token t = dts[0].BeginToken.Previous; t != null; t = t.Previous) 
                {
                    DecreeReferent dr = t.GetReferent() as DecreeReferent;
                    if (dr == null) 
                        continue;
                    if (t.Tag is string) 
                    {
                        if ((t.Tag as string) == "ignored") 
                            continue;
                    }
                    if (dr.CheckTypename(dts[0].Value)) 
                    {
                    }
                    else if (dr.Owner != null && dr.Owner.CheckTypename(dts[0].Value)) 
                        dr = dr.Owner;
                    else 
                        continue;
                    if (dts.Count == 2 && dts[1].Ref != null) 
                    {
                        Pullenti.Ner.Referent org = dr.GetSlotValue(DecreeReferent.ATTR_SOURCE) as Pullenti.Ner.Referent;
                        if (org != null) 
                        {
                            if (!org.CanBeEquals(dts[1].Ref.Referent, Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)) 
                                continue;
                        }
                    }
                    res = new List<Pullenti.Ner.ReferentToken>();
                    res.Add(new Pullenti.Ner.ReferentToken(dr, dts[0].BeginToken, dts[dts.Count - 1].EndToken));
                    break;
                }
            }
            ad.Level--;
            return res;
        }
        static List<Pullenti.Ner.ReferentToken> _TryAttach(List<Pullenti.Ner.Decree.Internal.DecreeToken> dts, Pullenti.Ner.Decree.Internal.DecreeToken baseTyp, bool afterDecree, Pullenti.Ner.Core.AnalyzerData ad)
        {
            if (dts == null || (dts.Count < 1)) 
                return null;
            if (dts[0].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Edition && dts.Count > 1) 
                dts.RemoveAt(0);
            if (dts.Count == 2 && dts[0].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Org && dts[1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) 
                return null;
            if (dts.Count == 2 && dts[0].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ && dts[1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number) 
            {
                if ((dts[0].LengthChar < 3) && !dts[0].IsWhitespaceAfter && dts[0].IsDoubtful) 
                    return null;
                if (dts[0].IsNewlineAfter) 
                    return null;
            }
            if (dts.Count == 1) 
            {
                if (dts[0].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.DecreeRef && dts[0].Ref != null) 
                {
                    if (baseTyp != null) 
                    {
                        Pullenti.Ner.Referent re = dts[0].Ref.GetReferent();
                        DecreeReferent dre = re as DecreeReferent;
                        if (dre == null && (re is DecreePartReferent)) 
                            dre = (re as DecreePartReferent).Owner;
                        if (dre != null) 
                        {
                            if (!dre.CheckTypename(baseTyp.Value)) 
                                return null;
                        }
                    }
                    List<Pullenti.Ner.ReferentToken> reli = new List<Pullenti.Ner.ReferentToken>();
                    reli.Add(new Pullenti.Ner.ReferentToken(dts[0].Ref.Referent, dts[0].BeginToken, dts[0].EndToken));
                    return reli;
                }
            }
            DecreeReferent dec0 = null;
            bool kodeks = false;
            bool canbesingle = false;
            int maxEmpty = 30;
            for (Pullenti.Ner.Token t = dts[0].BeginToken.Previous; t != null; t = t.Previous) 
            {
                if (t.IsCommaAnd) 
                    continue;
                if (t.IsChar(')')) 
                {
                    int cou = 0;
                    for (t = t.Previous; t != null; t = t.Previous) 
                    {
                        if (t.IsChar('(')) 
                            break;
                        else if ((++cou) > 200) 
                            break;
                    }
                    if (t != null && t.IsChar('(')) 
                        continue;
                    break;
                }
                if ((--maxEmpty) < 0) 
                    break;
                if (!t.Chars.IsLetter) 
                    continue;
                dec0 = t.GetReferent() as DecreeReferent;
                if (dec0 != null) 
                {
                    if (Pullenti.Ner.Decree.Internal.DecreeToken.GetKind(dec0.Typ, null) == DecreeKind.Kodex) 
                        kodeks = true;
                    else if (dec0.Kind == DecreeKind.Publisher) 
                        dec0 = null;
                }
                break;
            }
            DecreeReferent dec = new DecreeReferent();
            int i = 0;
            Pullenti.Ner.MorphCollection morph = null;
            bool isNounDoubt = false;
            Pullenti.Ner.Decree.Internal.DecreeToken numTok = null;
            Pullenti.Ner.Decree.Internal.DecreeToken typTok = null;
            int nl = 0;
            for (i = 0; i < dts.Count; i++) 
            {
                if (dts[i].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) 
                {
                    if (dts[i].Value == null) 
                        break;
                    if (dts[i].Value.StartsWith("ПРОЕКТ")) 
                    {
                        if (i > 0) 
                            return null;
                    }
                    if (dts[i].IsNewlineBefore) 
                    {
                        if (dec.Date != null || dec.Number != null) 
                            break;
                    }
                    if (dec.Typ != null) 
                    {
                        if (((dec.Typ == "РЕШЕНИЕ" || dec.Typ == "РІШЕННЯ")) && dts[i].Value == "ПРОТОКОЛ") 
                        {
                        }
                        else if (((dec.Typ == dts[i].Value || dts[i].Value == "ТЕХНИЧЕСКИЙ РЕГЛАМЕНТ")) && dec.Typ == "ГОСТ") 
                        {
                            if (((i + 1) < dts.Count) && dts[i + 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number) 
                            {
                                if (dec.Number != null) 
                                    break;
                                if (i > 0 && dts[i - 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number) 
                                    break;
                            }
                            continue;
                        }
                        else if (dts[i].Value == dec.Typ) 
                        {
                            if ((i + 1) >= dts.Count || dts[i + 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Name) 
                                continue;
                            else if ((((i + 1) < dts.Count) && dts[i + 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number && dts[i - 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Name) && dec.Number == null) 
                            {
                            }
                            else 
                                break;
                        }
                        else 
                            break;
                    }
                    DecreeKind ki = Pullenti.Ner.Decree.Internal.DecreeToken.GetKind(dts[i].Value, null);
                    if (ki == DecreeKind.Standard) 
                    {
                        if (i > 0) 
                        {
                            if (dts.Count == 2 && dts[0].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number && dts[i].Value == "ТЕХНИЧЕСКИЕ УСЛОВИЯ") 
                            {
                            }
                            else 
                                return null;
                        }
                    }
                    if (ki == DecreeKind.Kodex) 
                    {
                        if (i > 0) 
                            break;
                        if (dts[i].Value != "ОСНОВЫ ЗАКОНОДАТЕЛЬСТВА" && dts[i].Value != "ОСНОВИ ЗАКОНОДАВСТВА") 
                            kodeks = true;
                        else 
                            kodeks = false;
                    }
                    else 
                        kodeks = false;
                    morph = dts[i].Morph;
                    dec.AddTyp(dts[i]);
                    if (typTok == null) 
                        typTok = dts[i];
                    if (dts[i].CanBeSingleDecree) 
                        canbesingle = true;
                    if (dts[i].FullValue != null) 
                        dec.AddNameStr(dts[i].FullValue, dts[i]);
                    isNounDoubt = dts[i].IsDoubtful;
                    if (isNounDoubt && i == 0) 
                    {
                        if (Pullenti.Ner.Decree.Internal.PartToken.IsPartBefore(dts[i].BeginToken)) 
                            isNounDoubt = false;
                    }
                    if (dts[i].Ref != null) 
                    {
                        if (dec.FindSlot(DecreeReferent.ATTR_GEO, null, true) == null) 
                        {
                            Pullenti.Ner.Slot ss = dec.AddSlot(DecreeReferent.ATTR_GEO, dts[i].Ref.Referent, false, 0);
                            ss.AddAnnotation(dts[i].Ref);
                            dec.AddExtReferent(dts[i].Ref);
                        }
                    }
                    if (dts[i].SrcRef != null) 
                    {
                        Pullenti.Ner.Slot ss = dec.AddSlot(DecreeReferent.ATTR_SOURCE, dts[i].SrcRef, false, 0);
                        ss.AddAnnotation(dts[i]);
                    }
                }
                else if (dts[i].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Date) 
                {
                    if (dec.Date != null) 
                        break;
                    if (kodeks) 
                    {
                        if (i > 0 && dts[i - 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number) 
                        {
                        }
                        else if (((i + 1) < dts.Count) && dts[i + 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number) 
                        {
                        }
                        else 
                            break;
                    }
                    if (i == (dts.Count - 1)) 
                    {
                        if (!dts[i].BeginToken.IsValue("ОТ", "ВІД")) 
                        {
                            DecreeKind ty = Pullenti.Ner.Decree.Internal.DecreeToken.GetKind(dec.Typ, null);
                            if ((ty == DecreeKind.Konvention || ty == DecreeKind.Contract || ty == DecreeKind.License) || dec.Typ0 == "ПИСЬМО" || dec.Typ0 == "ЛИСТ") 
                            {
                            }
                            else 
                                break;
                        }
                    }
                    dec.AddDate(dts[i]);
                    dec.AddExtReferent(dts[i].Ref);
                }
                else if (dts[i].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.DateRange) 
                {
                    if (dec.Kind != DecreeKind.Program) 
                        break;
                    dec.AddDate(dts[i]);
                    dec.AddExtReferent(dts[i].Ref);
                }
                else if (dts[i].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Edition) 
                {
                    if (dts[i].IsNewlineBefore && !dts[i].BeginToken.Chars.IsAllLower && !dts[i].BeginToken.IsChar('(')) 
                        break;
                    if (((i + 2) < dts.Count) && dts[i + 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) 
                        break;
                }
                else if (dts[i].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number) 
                {
                    if (kodeks) 
                    {
                        if (((i + 1) < dts.Count) && dts[i + 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Date) 
                        {
                        }
                        else if (i > 0 && dts[i - 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Date) 
                        {
                        }
                        else 
                            break;
                    }
                    numTok = dts[i];
                    if (dts[i].IsDelo) 
                    {
                        if (dec.CaseNumber != null) 
                            break;
                        Pullenti.Ner.Slot ss = dec.AddSlot(DecreeReferent.ATTR_CASENUMBER, dts[i].Value, true, 0);
                        ss.AddAnnotation(dts[i]);
                        continue;
                    }
                    if (dec.Number != null) 
                    {
                        if (i > 2 && ((dts[i - 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Owner || dts[i - 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Org)) && dts[i - 2].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number) 
                        {
                        }
                        else 
                            break;
                    }
                    if (dts[i].IsNewlineBefore) 
                    {
                        if (dec.Typ == null && dec0 == null) 
                            break;
                    }
                    if (Pullenti.Morph.LanguageHelper.EndsWith(dts[i].Value, "ФЗ")) 
                        dec.Typ = "ФЕДЕРАЛЬНЫЙ ЗАКОН";
                    if (Pullenti.Morph.LanguageHelper.EndsWith(dts[i].Value, "ФКЗ")) 
                        dec.Typ = "ФЕДЕРАЛЬНЫЙ КОНСТИТУЦИОННЫЙ ЗАКОН";
                    if (dts[i].Value != null && dts[i].Value.StartsWith("ПР", StringComparison.OrdinalIgnoreCase) && dec.Typ == null) 
                        dec.Typ = "ПОРУЧЕНИЕ";
                    if (dec.Typ == null) 
                    {
                        if (dec0 == null && !afterDecree && baseTyp == null) 
                            break;
                    }
                    dec.AddNumber(dts[i]);
                    if (dts[i].Children != null) 
                    {
                        int cou = 0;
                        foreach (Pullenti.Ner.Slot s in dec.Slots) 
                        {
                            if (s.TypeName == DecreeReferent.ATTR_SOURCE) 
                                cou++;
                        }
                        if (cou == (dts[i].Children.Count + 1)) 
                        {
                            foreach (Pullenti.Ner.Decree.Internal.DecreeToken dd in dts[i].Children) 
                            {
                                dec.AddNumber(dd);
                            }
                            dts[i].Children = null;
                        }
                    }
                    continue;
                }
                else if (dts[i].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Name) 
                {
                    if (dec.Typ == null && dec.Number == null && dec0 == null) 
                    {
                        if (dts[i].TypKind == DecreeKind.Classifier || dts[i].TypKind == DecreeKind.Standard) 
                        {
                        }
                        else if ((i == 0 && ((i + 1) < dts.Count) && dts[i + 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) && (dts[i + 1].WhitespacesBeforeCount < 3)) 
                        {
                        }
                        else 
                            break;
                    }
                    if (dec.GetStringValue(DecreeReferent.ATTR_NAME) != null) 
                    {
                        if (kodeks) 
                            break;
                        if (i > 0 && dts[i - 1].EndToken.Next == dts[i].BeginToken) 
                        {
                        }
                        else if (dts[i].TypKind == DecreeKind.Classifier) 
                        {
                        }
                        else 
                            break;
                    }
                    Pullenti.Ner.Token tt0 = dts[i].BeginToken;
                    Pullenti.Ner.Token tt1 = dts[i].EndToken;
                    if ((i > 0 && dts[i - 1].IsNewlineAfter && ((i + 1) < dts.Count)) && dts[i + 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ && (dts[i + 1].WhitespacesBeforeCount < 3)) 
                        break;
                    if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(tt0, true, false) && Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(tt1, true, tt0, false)) 
                    {
                        tt0 = tt0.Next;
                        tt1 = tt1.Previous;
                        for (Pullenti.Ner.Token tt = tt0; tt != null && (tt.EndChar < tt1.EndChar); tt = tt.Next) 
                        {
                            if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(tt, true, false)) 
                            {
                                Pullenti.Ner.Core.BracketSequenceToken br1 = Pullenti.Ner.Core.BracketHelper.TryParse(tt, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                                if (br1 != null && br1.EndToken == dts[i].EndToken) 
                                {
                                    tt1 = tt1.Next;
                                    break;
                                }
                            }
                        }
                    }
                    string nam = Pullenti.Ner.Core.MiscHelper.GetTextValue(tt0, tt1, Pullenti.Ner.Core.GetTextAttr.RestoreRegister);
                    if (kodeks && !nam.ToUpper().Contains("КОДЕКС")) 
                        nam = "Кодекс " + nam;
                    if (dts[i].FullValue != null) 
                        dec.AddNameStr(dts[i].FullValue, dts[i]);
                    dec.AddNameStr(nam, (tt0.BeginChar <= tt1.BeginChar ? new Pullenti.Ner.MetaToken(tt0, tt1) : null));
                }
                else if (dts[i].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Between) 
                {
                    if (dec.Kind != DecreeKind.Contract) 
                        break;
                    foreach (Pullenti.Ner.Decree.Internal.DecreeToken chh in dts[i].Children) 
                    {
                        Pullenti.Ner.Slot ss = dec.AddSlot(DecreeReferent.ATTR_SOURCE, chh.Ref.Referent, false, 0);
                        ss.Tag = chh.GetSourceText();
                        ss.AddAnnotation(chh);
                        if (chh.Ref.Referent is Pullenti.Ner.Person.PersonPropertyReferent) 
                            dec.AddExtReferent(chh.Ref);
                    }
                }
                else if (dts[i].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Owner) 
                {
                    if (kodeks) 
                        break;
                    if (dec.Name != null) 
                        break;
                    if (((i == 0 || i == (dts.Count - 1))) && dts[i].BeginToken.Chars.IsAllLower) 
                        break;
                    if (i == 0 && dts.Count > 1 && dts[1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) 
                        break;
                    if (dec.FindSlot(DecreeReferent.ATTR_SOURCE, null, true) != null) 
                    {
                    }
                    if (dts[i].Ref != null) 
                    {
                        DecreeKind ty = Pullenti.Ner.Decree.Internal.DecreeToken.GetKind(dec.Typ, null);
                        if (ty == DecreeKind.Ustav) 
                        {
                            if (!(dts[i].Ref.Referent is Pullenti.Ner.Org.OrganizationReferent)) 
                                break;
                        }
                        Pullenti.Ner.Slot ss = dec.AddSlot(DecreeReferent.ATTR_SOURCE, dts[i].Ref.Referent, false, 0);
                        ss.Tag = dts[i].GetSourceText();
                        ss.AddAnnotation(dts[i]);
                        if (dts[i].Ref.Referent is Pullenti.Ner.Person.PersonPropertyReferent) 
                            dec.AddExtReferent(dts[i].Ref);
                    }
                    else 
                    {
                        Pullenti.Ner.Slot ss = dec.AddSlot(DecreeReferent.ATTR_SOURCE, Pullenti.Ner.Core.MiscHelper.ConvertFirstCharUpperAndOtherLower(dts[i].Value), false, 0);
                        ss.Tag = dts[i].GetSourceText();
                        ss.AddAnnotation(dts[i]);
                    }
                }
                else if (dts[i].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Org) 
                {
                    if (kodeks) 
                    {
                        if (i != 1 || dts[i].WhitespacesBeforeCount > 3) 
                            break;
                        if (dts.Count == 2 || ((dts.Count == 3 && dts[2].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Misc))) 
                        {
                        }
                        else 
                            break;
                        isNounDoubt = false;
                    }
                    else if (dec.Name != null) 
                        break;
                    if (dec.FindSlot(DecreeReferent.ATTR_SOURCE, null, true) != null) 
                    {
                        if (i > 2 && dts[i - 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number && ((dts[i - 2].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Org || dts[i - 2].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Owner))) 
                        {
                        }
                        else if (dts[i].BeginToken.Previous != null && dts[i].BeginToken.Previous.IsAnd) 
                        {
                        }
                        else if (i > 0 && ((dts[i - 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Owner || dts[i - 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Org))) 
                        {
                        }
                        else if (i > 0 && dts[i - 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ && dts[i - 1].SrcRef == dec.GetSlotValue(DecreeReferent.ATTR_SOURCE)) 
                        {
                        }
                        else 
                            break;
                    }
                    Pullenti.Ner.ReferentToken ttt = dts[i].BeginToken as Pullenti.Ner.ReferentToken;
                    if (ttt != null && ttt == dts[i].EndToken && ttt.BeginToken != ttt.EndToken) 
                    {
                        if ((ttt.BeginToken.GetReferent() is Pullenti.Ner.Org.OrganizationReferent) && ttt.BeginToken.Next.IsComma) 
                        {
                            Pullenti.Ner.Slot sl1 = dec.AddSlot(DecreeReferent.ATTR_SOURCE, ttt.BeginToken.GetReferent(), false, 0);
                            sl1.Tag = ttt.BeginToken.GetSourceText();
                            sl1.AddAnnotation(ttt.BeginToken as Pullenti.Ner.ReferentToken);
                        }
                    }
                    Pullenti.Ner.Slot sl = dec.AddSlot(DecreeReferent.ATTR_SOURCE, dts[i].Ref.Referent, false, 0);
                    sl.Tag = dts[i].GetSourceText();
                    sl.AddAnnotation(dts[i]);
                    if (((i + 2) < dts.Count) && dts[i + 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Unknown && (dts[i + 1].WhitespacesBeforeCount < 2)) 
                    {
                        if (dts[i + 2].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number || dts[i + 2].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Date) 
                        {
                            sl.Tag = (new Pullenti.Ner.MetaToken(dts[i].BeginToken, dts[i + 1].EndToken)).GetSourceText();
                            i++;
                        }
                    }
                }
                else if (dts[i].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Terr) 
                {
                    if (dec.FindSlot(DecreeReferent.ATTR_GEO, null, true) != null) 
                        break;
                    if (i > 0 && dts[i - 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Name) 
                        break;
                    if (dts[i].IsNewlineBefore && ((i + 1) < dts.Count) && dts[i + 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Date) 
                        break;
                    Pullenti.Ner.Slot ss = dec.AddSlot(DecreeReferent.ATTR_GEO, dts[i].Ref.Referent, false, 0);
                    ss.AddAnnotation(dts[i]);
                }
                else if (dts[i].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Unknown) 
                {
                    if (dec.FindSlot(DecreeReferent.ATTR_SOURCE, null, true) != null) 
                        break;
                    if (kodeks) 
                        break;
                    if ((dec.Kind == DecreeKind.Contract && i == 1 && ((i + 1) < dts.Count)) && dts[i + 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number) 
                    {
                        dec.AddNameStr(Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(dts[i], Pullenti.Ner.Core.GetTextAttr.KeepRegister), dts[i]);
                        continue;
                    }
                    if (i == 0) 
                    {
                        if (dec0 == null && !afterDecree) 
                            break;
                        bool ok1 = false;
                        if (((i + 1) < dts.Count) && dts[i + 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number) 
                            ok1 = true;
                        else if (((i + 2) < dts.Count) && dts[i + 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Terr && dts[i + 2].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number) 
                            ok1 = true;
                        if (!ok1) 
                            break;
                    }
                    else if (dts[i - 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Owner || dts[i - 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Org) 
                        continue;
                    if ((i + 1) >= dts.Count) 
                        break;
                    if (dts[0].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ && dts[0].IsDoubtful) 
                        break;
                    if (dts[i + 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number || dts[i + 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Date || dts[i + 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Name) 
                    {
                        Pullenti.Ner.Slot ss = dec.AddSlot(DecreeReferent.ATTR_SOURCE, dts[i].Value, false, 0);
                        ss.Tag = dts[i].GetSourceText();
                        ss.AddAnnotation(dts[i]);
                        continue;
                    }
                    if (dts[i + 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Terr) 
                    {
                        Pullenti.Ner.Slot ss = dec.AddSlot(DecreeReferent.ATTR_SOURCE, dts[i].Value, false, 0);
                        ss.Tag = dts[i].GetSourceText();
                        ss.AddAnnotation(dts[i]);
                        continue;
                    }
                    if (dts[i + 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Owner) 
                    {
                        string s = Pullenti.Ner.Core.MiscHelper.GetTextValue(dts[i].BeginToken, dts[i + 1].EndToken, Pullenti.Ner.Core.GetTextAttr.FirstNounGroupToNominative);
                        dts[i].EndToken = dts[i + 1].EndToken;
                        Pullenti.Ner.Slot ss = dec.AddSlot(DecreeReferent.ATTR_SOURCE, s, false, 0);
                        ss.Tag = dts[i].GetSourceText();
                        ss.AddAnnotation(dts[i]);
                        i++;
                        continue;
                    }
                    break;
                }
                else if (dts[i].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Misc) 
                {
                    if (i == 0 || kodeks) 
                        break;
                    if ((i + 1) >= dts.Count) 
                    {
                        if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(dts[i].EndToken.Next, true, false)) 
                            continue;
                        if (i > 0 && dts[i - 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number) 
                        {
                            if (Pullenti.Ner.Decree.Internal.DecreeToken.TryAttachName(dts[i].EndToken.Next, null, true, false, false) != null) 
                                continue;
                        }
                    }
                    else if (dts[i + 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Name || dts[i + 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number || dts[i + 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Date) 
                        continue;
                    break;
                }
                else 
                    break;
            }
            if (i == 0) 
                return null;
            if (dec.Typ == null || ((dec0 != null && dts[0].Typ != Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ))) 
            {
                if (dec0 != null) 
                {
                    if (dec.Number == null && dec.Date == null && dec.FindSlot(DecreeReferent.ATTR_NAME, null, true) == null) 
                        return null;
                    if (dec.Typ == null) 
                        dec.Typ = dec0.Typ;
                    if (dec.FindSlot(DecreeReferent.ATTR_GEO, null, true) == null) 
                        dec.AddSlot(DecreeReferent.ATTR_GEO, dec0.GetStringValue(DecreeReferent.ATTR_GEO), false, 0);
                    if (dec.FindSlot(DecreeReferent.ATTR_DATE, null, true) == null && dec0.Date != null) 
                        dec.AddSlot(DecreeReferent.ATTR_DATE, dec0.GetSlotValue(DecreeReferent.ATTR_DATE), false, 0);
                    Pullenti.Ner.Slot sl;
                    if (dec.FindSlot(DecreeReferent.ATTR_SOURCE, null, true) == null) 
                    {
                        if ((((sl = dec0.FindSlot(DecreeReferent.ATTR_SOURCE, null, true)))) != null) 
                        {
                            Pullenti.Ner.Slot ss = dec.AddSlot(DecreeReferent.ATTR_SOURCE, sl.Value, false, 0);
                            ss.Tag = sl.Tag;
                            ss.MergeOccurence(sl);
                        }
                    }
                }
                else if (baseTyp != null && afterDecree) 
                    dec.AddTyp(baseTyp);
                else 
                    return null;
            }
            Pullenti.Ner.Token et = dts[i - 1].EndToken;
            if ((((!afterDecree && dts.Count == i && i == 3) && dts[0].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ && dts[i - 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number) && dec.FindSlot(DecreeReferent.ATTR_SOURCE, null, true) != null && et.Next != null) && et.Next.IsComma && dec.Number != null) 
            {
                for (Pullenti.Ner.Token tt = et.Next; tt != null; tt = tt.Next) 
                {
                    if (!tt.IsChar(',')) 
                        break;
                    List<Pullenti.Ner.Decree.Internal.DecreeToken> ddd = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttachList(tt.Next, dts[0], 10, false);
                    if (ddd == null || (ddd.Count < 2) || ddd[0].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) 
                        break;
                    bool hasNum = false;
                    foreach (Pullenti.Ner.Decree.Internal.DecreeToken d in ddd) 
                    {
                        if (d.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number) 
                            hasNum = true;
                        else if (d.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) 
                        {
                            hasNum = false;
                            break;
                        }
                    }
                    if (!hasNum) 
                        break;
                    List<Pullenti.Ner.ReferentToken> rtt = _TryAttach(ddd, dts[0], true, ad);
                    if (rtt == null) 
                        break;
                    dec.MergeSlots(rtt[0].Referent, true);
                    et = (tt = rtt[0].EndToken);
                }
            }
            if (((et.Next != null && et.Next.IsChar('<') && (et.Next.Next is Pullenti.Ner.ReferentToken)) && et.Next.Next.Next != null && et.Next.Next.Next.IsChar('>')) && et.Next.Next.GetReferent().TypeName == "URI") 
                et = et.Next.Next.Next;
            string num = dec.Number;
            if ((dec.FindSlot(DecreeReferent.ATTR_NAME, null, true) == null && (i < dts.Count) && dts[i].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) && dec.Kind == DecreeKind.Project) 
            {
                List<Pullenti.Ner.Decree.Internal.DecreeToken> dts1 = new List<Pullenti.Ner.Decree.Internal.DecreeToken>(dts);
                dts1.RemoveRange(0, i);
                List<Pullenti.Ner.ReferentToken> rt1 = _TryAttach(dts1, null, true, ad);
                if (rt1 != null) 
                {
                    dec.AddNameStr(Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(rt1[0], Pullenti.Ner.Core.GetTextAttr.KeepRegister), rt1[0]);
                    et = rt1[0].EndToken;
                }
            }
            if (dec.Typ == "РУКОВОДЯЩИЙ ДОКУМЕНТ" && !et.IsNewlineAfter) 
            {
                Pullenti.Ner.Decree.Internal.DecreeToken typ1 = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(et.Next, null, false);
                if (typ1 != null && typ1.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ && typ1.Value == dec.Typ) 
                {
                    Pullenti.Ner.Token tt = typ1.EndToken.Next;
                    for (; tt != null; tt = tt.Next) 
                    {
                        if (tt.IsChar('.') || tt.IsHiphen || tt.IsValue("ОТРАСЛЬ", null)) 
                        {
                        }
                        else 
                            break;
                    }
                    Pullenti.Ner.Decree.Internal.DecreeToken dn = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttachName(tt, dec.Typ, false, false, false);
                    if (dn != null) 
                    {
                        if (dn.FullValue != null) 
                            dec.AddNameStr(dn.FullValue, dn);
                        if (dn.Value != null) 
                            dec.AddNameStr(dn.Value, dn);
                        et = dn.EndToken;
                    }
                }
            }
            if (dec.FindSlot(DecreeReferent.ATTR_NAME, null, true) == null && !kodeks && et.Next != null) 
            {
                Pullenti.Ner.Decree.Internal.DecreeToken dn = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttachName((et.Next.IsChar(':') ? et.Next.Next : et.Next), dec.Typ, false, false, false);
                if (dn != null && et.Next.Chars.IsAllLower && num != null) 
                {
                    if (ad != null) 
                    {
                        foreach (Pullenti.Ner.Referent r in ad.Referents) 
                        {
                            if (r.FindSlot(DecreeReferent.ATTR_NUMBER, num, true) != null) 
                            {
                                if (r.CanBeEquals(dec, Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)) 
                                {
                                    if (r.FindSlot(DecreeReferent.ATTR_NAME, dn.Value, true) == null) 
                                        dn = null;
                                    break;
                                }
                            }
                        }
                    }
                }
                if (dts[i - 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ && dn != null && et.IsNewlineAfter) 
                    dn = null;
                if (dn != null) 
                {
                    if (dec.Kind == DecreeKind.Program) 
                    {
                        dn.BeginToken = dts[0].BeginToken;
                        dn.FullValue = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(dn, Pullenti.Ner.Core.GetTextAttr.FirstNounGroupToNominativeSingle | Pullenti.Ner.Core.GetTextAttr.KeepRegister);
                        if (dn.FullValue.StartsWith("ГОСПРОГРАММА", StringComparison.OrdinalIgnoreCase)) 
                            dn.FullValue = "Государственная программа" + dn.FullValue.Substring(12);
                    }
                    if (dec.Kind == DecreeKind.Program) 
                    {
                        for (Pullenti.Ner.Token tt1 = dn.EndToken.Previous; tt1 != null && tt1.BeginChar > dn.BeginChar; tt1 = (tt1 == null ? null : tt1.Previous)) 
                        {
                            if (tt1.IsChar(')') && tt1.Previous != null) 
                                tt1 = tt1.Previous;
                            if (tt1.GetReferent() is Pullenti.Ner.Date.DateRangeReferent) 
                                dec.AddSlot(DecreeReferent.ATTR_DATE, tt1.GetReferent(), false, 0);
                            else if ((tt1.GetReferent() is Pullenti.Ner.Date.DateReferent) && tt1.Previous != null && tt1.Previous.IsValue("ДО", null)) 
                            {
                                Pullenti.Ner.ReferentToken rt11 = tt1.Kit.ProcessReferent("DATE", tt1.Previous, null);
                                if (rt11 != null && (rt11.Referent is Pullenti.Ner.Date.DateRangeReferent)) 
                                {
                                    dec.AddSlot(DecreeReferent.ATTR_DATE, rt11.Referent, false, 0);
                                    dec.AddExtReferent(rt11);
                                    tt1 = tt1.Previous;
                                }
                                else 
                                    break;
                            }
                            else if ((tt1.GetReferent() is Pullenti.Ner.Date.DateReferent) && tt1.Previous != null && ((tt1.Previous.IsValue("НА", null) || tt1.Previous.IsValue("В", null)))) 
                            {
                                dec.AddSlot(DecreeReferent.ATTR_DATE, tt1.GetReferent(), false, 0);
                                tt1 = tt1.Previous;
                            }
                            else 
                                break;
                            for (tt1 = tt1.Previous; tt1 != null && tt1.BeginChar > dn.BeginChar; tt1 = (tt1 == null ? null : tt1.Previous)) 
                            {
                                if (tt1.Morph.Class.IsConjunction || tt1.Morph.Class.IsPreposition) 
                                    continue;
                                if (tt1.IsValue("ПЕРИОД", "ПЕРІОД") || tt1.IsValue("ПЕРСПЕКТИВА", null)) 
                                    continue;
                                if (tt1.IsChar('(')) 
                                    continue;
                                break;
                            }
                            if (tt1 != null && tt1.EndChar > dn.BeginChar) 
                            {
                                if (dn.FullValue == null) 
                                    dn.FullValue = dn.Value;
                                dn.Value = Pullenti.Ner.Core.MiscHelper.GetTextValue(dn.BeginToken, tt1, Pullenti.Ner.Core.GetTextAttr.FirstNounGroupToNominativeSingle | Pullenti.Ner.Core.GetTextAttr.KeepRegister);
                            }
                            tt1 = tt1.Next;
                        }
                    }
                    if (dn.FullValue != null) 
                        dec.AddNameStr(dn.FullValue, dn);
                    if ((dts.Count == 1 && dec.Kind != DecreeKind.Program && dts[0].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) && dn.BeginToken.Morph.Case.IsGenitive && !dn.BeginToken.Morph.Class.IsPreposition) 
                    {
                        string str = Pullenti.Ner.Core.MiscHelper.GetTextValue(dts[0].BeginToken, dn.EndToken, Pullenti.Ner.Core.GetTextAttr.FirstNounGroupToNominative);
                        dec.AddNameStr(str, new Pullenti.Ner.MetaToken(dts[0].BeginToken, dn.EndToken));
                    }
                    else 
                        dec.AddNameStr(dn.Value, new Pullenti.Ner.MetaToken(dts[0].BeginToken, dn.EndToken));
                    et = dn.EndToken;
                    bool br = false;
                    for (Pullenti.Ner.Token tt = et.Next; tt != null; tt = tt.Next) 
                    {
                        if (tt.IsChar('(')) 
                        {
                            br = true;
                            continue;
                        }
                        if (tt.IsChar(')') && br) 
                        {
                            et = tt;
                            continue;
                        }
                        if ((tt.GetReferent() is Pullenti.Ner.Date.DateRangeReferent) && dec.Kind == DecreeKind.Program) 
                        {
                            dec.AddSlot(DecreeReferent.ATTR_DATE, tt.GetReferent(), false, 0);
                            et = tt;
                            continue;
                        }
                        dn = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(tt, null, false);
                        if (dn == null) 
                            break;
                        if (dn.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Date && dec.Date == null) 
                        {
                            if (dec.AddDate(dn)) 
                            {
                                et = (tt = dn.EndToken);
                                continue;
                            }
                        }
                        if (dn.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number && dec.Number == null) 
                        {
                            dec.AddNumber(dn);
                            et = (tt = dn.EndToken);
                            continue;
                        }
                        if (dn.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.DateRange && dec.Kind == DecreeKind.Program) 
                        {
                            if (dec.AddDate(dn)) 
                            {
                                et = (tt = dn.EndToken);
                                continue;
                            }
                        }
                        if (dn.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Terr && dec.FindSlot(DecreeReferent.ATTR_GEO, null, true) == null && dn.Ref != null) 
                        {
                            dec.AddSlot(DecreeReferent.ATTR_GEO, dn.Ref.Referent, false, 0);
                            et = (tt = dn.EndToken);
                            continue;
                        }
                        break;
                    }
                }
            }
            if (dec.FindSlot(DecreeReferent.ATTR_SOURCE, null, true) == null) 
            {
                Pullenti.Ner.Token tt0 = dts[0].BeginToken.Previous;
                if ((tt0 != null && tt0.IsValue("В", "У") && tt0.Previous != null) && (tt0.Previous.GetReferent() is Pullenti.Ner.Org.OrganizationReferent)) 
                {
                    Pullenti.Ner.Slot ss = dec.AddSlot(DecreeReferent.ATTR_SOURCE, tt0.Previous.GetReferent(), false, 0);
                    ss.AddAnnotation(tt0.Previous as Pullenti.Ner.ReferentToken);
                }
            }
            if (!canbesingle && !dec.CheckCorrection(isNounDoubt)) 
            {
                string ty = dec.Typ;
                Pullenti.Ner.Slot sl = null;
                if (dec0 != null && dec.Date != null && dec.FindSlot(DecreeReferent.ATTR_SOURCE, null, true) == null) 
                    sl = dec0.FindSlot(DecreeReferent.ATTR_SOURCE, null, true);
                if (sl != null && (((((ty == "ПОСТАНОВЛЕНИЕ" || ty == "ПОСТАНОВА" || ty == "ОПРЕДЕЛЕНИЕ") || ty == "ВИЗНАЧЕННЯ" || ty == "РЕШЕНИЕ") || ty == "РІШЕННЯ" || ty == "ПРИГОВОР") || ty == "ВИРОК"))) 
                    dec.AddSlot(sl.TypeName, sl.Value, false, 0).Tag = sl.Tag;
                else 
                {
                    int eqDecs = 0;
                    DecreeReferent dr0 = null;
                    if (num != null) 
                    {
                        if (ad != null) 
                        {
                            foreach (Pullenti.Ner.Referent r in ad.Referents) 
                            {
                                if (r.FindSlot(DecreeReferent.ATTR_NUMBER, num, true) != null) 
                                {
                                    if (r.CanBeEquals(dec, Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)) 
                                    {
                                        eqDecs++;
                                        dr0 = r as DecreeReferent;
                                    }
                                }
                            }
                        }
                    }
                    if (eqDecs == 1) 
                        dec.MergeSlots(dr0, true);
                    else 
                    {
                        bool ok1 = false;
                        if (num != null) 
                        {
                            for (Pullenti.Ner.Token tt = dts[0].BeginToken.Previous; tt != null; tt = tt.Previous) 
                            {
                                if (tt.IsCharOf(":,") || tt.IsHiphen || Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(tt, false, false)) 
                                {
                                }
                                else 
                                {
                                    if (tt.IsValue("ДАЛЕЕ", "ДАЛІ") || tt.IsValue("УТВЕРЖДЕННЫЙ", null)) 
                                        ok1 = true;
                                    break;
                                }
                            }
                        }
                        if (!ok1) 
                            return null;
                    }
                }
            }
            Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(dec, dts[0].BeginToken, et);
            if (dec.Slots.Count == 2 && dec.Slots[0].TypeName == DecreeReferent.ATTR_TYPE && dec.Slots[1].TypeName == DecreeReferent.ATTR_NAME) 
            {
                bool err = true;
                for (Pullenti.Ner.Token tt = rt.BeginToken; tt != null && tt.EndChar <= rt.EndChar; tt = tt.Next) 
                {
                    if (tt.GetReferent() is Pullenti.Ner.Geo.GeoReferent) 
                    {
                    }
                    else if ((tt is Pullenti.Ner.TextToken) && tt.Chars.IsLetter && !tt.Chars.IsAllLower) 
                    {
                        err = false;
                        break;
                    }
                }
                if (err) 
                    return null;
            }
            if (morph != null) 
                rt.Morph = morph;
            if (rt.Chars.IsAllLower) 
            {
                if (dec.Typ0 == "ДЕКЛАРАЦИЯ" || dec.Typ0 == "ДЕКЛАРАЦІЯ") 
                    return null;
                if (((dec.Typ0 == "КОНСТИТУЦИЯ" || dec.Typ0 == "КОНСТИТУЦІЯ")) && rt.BeginToken == rt.EndToken) 
                {
                    bool ok1 = false;
                    int cou = 10;
                    for (Pullenti.Ner.Token tt = rt.BeginToken.Previous; tt != null && cou > 0; tt = tt.Previous,cou--) 
                    {
                        if (tt.IsNewlineAfter) 
                            break;
                        Pullenti.Ner.Decree.Internal.PartToken pt = Pullenti.Ner.Decree.Internal.PartToken.TryAttach(tt, null, false, false);
                        if (pt != null && pt.Typ != Pullenti.Ner.Decree.Internal.PartToken.ItemType.Prefix && pt.EndToken.Next == rt.BeginToken) 
                        {
                            ok1 = true;
                            break;
                        }
                    }
                    if (!ok1) 
                        return null;
                }
            }
            if (num != null && ((num.IndexOf('/') > 0 || num.IndexOf(',') > 0))) 
            {
                int cou = 0;
                foreach (Pullenti.Ner.Slot s in dec.Slots) 
                {
                    if (s.TypeName == DecreeReferent.ATTR_NUMBER) 
                        cou++;
                }
                if (cou == 1) 
                {
                    int owns = 0;
                    foreach (Pullenti.Ner.Slot s in dec.Slots) 
                    {
                        if (s.TypeName == DecreeReferent.ATTR_SOURCE) 
                            owns++;
                    }
                    if (owns > 1) 
                    {
                        string[] nums = num.Split('/');
                        string[] nums2 = num.Split(',');
                        string strNum = null;
                        for (int ii = 0; ii < dts.Count; ii++) 
                        {
                            if (dts[ii].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number) 
                            {
                                strNum = dts[ii].GetSourceText();
                                break;
                            }
                        }
                        if (nums2.Length == owns && owns > 1) 
                        {
                            dec.AddSlot(DecreeReferent.ATTR_NUMBER, null, true, 0);
                            foreach (string n in nums2) 
                            {
                                dec.AddSlot(DecreeReferent.ATTR_NUMBER, n.Trim(), false, 0).Tag = strNum;
                            }
                        }
                        else if (nums.Length == owns && owns > 1) 
                        {
                            dec.AddSlot(DecreeReferent.ATTR_NUMBER, null, true, 0);
                            foreach (string n in nums) 
                            {
                                dec.AddSlot(DecreeReferent.ATTR_NUMBER, n.Trim(), false, 0).Tag = strNum;
                            }
                        }
                    }
                }
            }
            if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(rt.BeginToken.Previous, true, false) && Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(rt.EndToken.Next, true, rt.BeginToken.Previous, false)) 
            {
                rt.BeginToken = rt.BeginToken.Previous;
                rt.EndToken = rt.EndToken.Next;
                List<Pullenti.Ner.Decree.Internal.DecreeToken> dts1 = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttachList(rt.EndToken.Next, null, 10, false);
                if (dts1 != null && dts1[0].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Date && dec.FindSlot(DecreeReferent.ATTR_DATE, null, true) == null) 
                {
                    dec.AddDate(dts1[0]);
                    rt.EndToken = dts1[0].EndToken;
                    if (dts1.Count > 1 && dts1[1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number && dec.Number == null) 
                    {
                        dec.AddNumber(dts1[1]);
                        rt.EndToken = dts1[1].EndToken;
                    }
                }
            }
            if (dec.Kind == DecreeKind.Standard && dec.Name == null && Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(rt.EndToken.Next, true, false)) 
            {
                Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(rt.EndToken.Next, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                if (br != null) 
                {
                    dec.AddNameStr(Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(br, Pullenti.Ner.Core.GetTextAttr.KeepRegister), br);
                    rt.EndToken = br.EndToken;
                }
            }
            if (dec.Kind == DecreeKind.Program && dec.FindSlot(DecreeReferent.ATTR_DATE, null, true) == null) 
            {
                if (rt.BeginToken.Previous != null && rt.BeginToken.Previous.IsValue("ПАСПОРТ", null)) 
                {
                    int cou = 0;
                    for (Pullenti.Ner.Token tt = rt.EndToken.Next; tt != null && (cou < 1000); tt = (tt == null ? null : tt.Next)) 
                    {
                        if (tt.IsValue("СРОК", "ТЕРМІН") && tt.Next != null && tt.Next.IsValue("РЕАЛИЗАЦИЯ", "РЕАЛІЗАЦІЯ")) 
                        {
                        }
                        else 
                            continue;
                        tt = tt.Next.Next;
                        if (tt == null) 
                            break;
                        Pullenti.Ner.Decree.Internal.DecreeToken dtok = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(tt, null, false);
                        if (dtok != null && dtok.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ && ((dtok.Value == "ПРОГРАММА" || dtok.Value == "ПРОГРАМА"))) 
                            tt = dtok.EndToken.Next;
                        for (; tt != null; tt = tt.Next) 
                        {
                            if (tt.IsHiphen || tt.IsTableControlChar || tt.IsValue("ПРОГРАММА", "ПРОГРАМА")) 
                            {
                            }
                            else if (tt.GetReferent() is Pullenti.Ner.Date.DateRangeReferent) 
                            {
                                dec.AddSlot(DecreeReferent.ATTR_DATE, tt.GetReferent(), false, 0);
                                break;
                            }
                            else 
                                break;
                        }
                        break;
                    }
                }
            }
            if (rt.EndToken.Next != null && rt.EndToken.Next.IsChar('(')) 
            {
                Pullenti.Ner.Date.DateReferent dt = null;
                bool ok = false;
                for (Pullenti.Ner.Token tt = rt.EndToken.Next.Next; tt != null; tt = tt.Next) 
                {
                    Pullenti.Ner.Referent r = tt.GetReferent();
                    if (r is Pullenti.Ner.Geo.GeoReferent) 
                        continue;
                    if (r is Pullenti.Ner.Date.DateReferent) 
                    {
                        dt = r as Pullenti.Ner.Date.DateReferent;
                        continue;
                    }
                    if (tt.Morph.Class.IsPreposition) 
                        continue;
                    if (tt.Morph.Class.IsVerb) 
                        continue;
                    if (tt.IsChar(')') && dt != null) 
                    {
                        dec.AddSlot(DecreeReferent.ATTR_DATE, dt, false, 0);
                        rt.EndToken = tt;
                        ok = true;
                    }
                    break;
                }
                if (!ok) 
                {
                    List<Pullenti.Ner.Decree.Internal.DecreeToken> dts1 = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttachList(rt.EndToken.Next.Next, null, 10, false);
                    if (dts1 != null) 
                    {
                        List<Pullenti.Ner.ReferentToken> rtt = _TryAttach(dts1, typTok, true, ad);
                        if ((rtt != null && rtt.Count == 1 && rtt[0].EndToken.Next != null) && rtt[0].EndToken.Next.IsChar(')')) 
                        {
                            DecreeReferent dec1 = rtt[0].Referent as DecreeReferent;
                            ok = dec.Typ == dec1.Typ;
                            if ((ok && dec1.Number != null && dec.Number != null) && dec.Number != dec1.Number) 
                                ok = false;
                            if ((ok && dec1.Date != null && dec.Date != null) && dec.Date.Value != dec1.Date.Value) 
                                ok = false;
                            if (ok) 
                            {
                                dec.MergeSlots(rtt[0].Referent, true);
                                rt.EndToken = rtt[0].EndToken.Next;
                            }
                        }
                    }
                    else if (dec.Kind == DecreeKind.Classifier) 
                    {
                        Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(rt.EndToken.Next, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                        if (br != null) 
                            rt.EndToken = br.EndToken;
                    }
                }
            }
            if (dec.FindSlot(DecreeReferent.ATTR_NAME, null, true) == null && !rt.IsNewlineBefore) 
            {
                Pullenti.Ner.Token tt0 = null;
                int cou = 40;
                for (Pullenti.Ner.Token tt = rt.BeginToken.Previous; tt != null && cou > 0; tt = tt.Previous,cou--) 
                {
                    if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(tt, true, false)) 
                        tt0 = tt;
                    else if ((tt is Pullenti.Ner.TextToken) && tt.Chars.IsLetter) 
                        tt0 = tt;
                    if (tt.IsNewlineBefore) 
                        break;
                }
                Pullenti.Ner.Decree.Internal.DecreeToken nam = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttachName(tt0, dec.Typ, false, false, false);
                if (nam != null && (nam.EndChar < rt.BeginChar)) 
                {
                    if (nam.EndToken.Next == rt.BeginToken || ((nam.EndToken.Next.IsComma && nam.EndToken.Next.Next == rt.BeginToken))) 
                    {
                        rt.BeginToken = tt0;
                        dec.AddNameStr(nam.Value, nam);
                    }
                }
            }
            if (dec.FindSlot(DecreeReferent.ATTR_NAME, null, true) == null && !rt.IsNewlineAfter) 
            {
                if (rt.IsNewlineBefore || ((rt.BeginToken.Previous != null && rt.BeginToken.Previous.LengthChar == 1 && ((rt.BeginToken.Previous.IsNewlineBefore || rt.BeginToken.Previous.IsTableControlChar))))) 
                {
                    if (dec.Kind == DecreeKind.Standard && dec.Number != null) 
                    {
                        Pullenti.Ner.Decree.Internal.DecreeToken nam = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttachNameToEndOfLine(rt.EndToken.Next, false);
                        if (nam != null) 
                        {
                            rt.EndToken = nam.EndToken;
                            dec.AddNameStr(nam.Value, nam);
                        }
                    }
                }
            }
            List<Pullenti.Ner.ReferentToken> rtLi = new List<Pullenti.Ner.ReferentToken>();
            if (((i + 1) < dts.Count) && dts[i].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Edition && !dts[i].IsNewlineBefore) 
            {
                dts.RemoveRange(0, i + 1);
                List<Pullenti.Ner.ReferentToken> ed = _TryAttach(dts, baseTyp, true, ad);
                if (ed != null && ed.Count > 0) 
                {
                    rtLi.AddRange(ed);
                    foreach (Pullenti.Ner.ReferentToken e in ed) 
                    {
                        dec.AddSlot(DecreeReferent.ATTR_EDITION, e.Referent, false, 0);
                    }
                    rt.EndToken = ed[ed.Count - 1].EndToken;
                }
            }
            else if (((i < (dts.Count - 1)) && i > 0 && dts[i - 1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Edition) && !dts[i - 1].IsNewlineBefore) 
            {
                dts.RemoveRange(0, i);
                List<Pullenti.Ner.ReferentToken> ed = _TryAttach(dts, baseTyp, true, ad);
                if (ed != null && ed.Count > 0) 
                {
                    rtLi.AddRange(ed);
                    foreach (Pullenti.Ner.ReferentToken e in ed) 
                    {
                        dec.AddSlot(DecreeReferent.ATTR_EDITION, e.Referent, false, 0);
                    }
                    rt.EndToken = ed[ed.Count - 1].EndToken;
                }
            }
            if (dec.Kind == DecreeKind.Law || dec.Kind == DecreeKind.Order) 
            {
            }
            else 
            {
                Pullenti.Ner.ReferentToken rt22 = DecreeAnalyzer._tryAttachApproved(rt.EndToken.Next, ad, true, false, null);
                if (rt22 != null) 
                {
                    rt.EndToken = rt22.EndToken;
                    DecreeReferent dr00 = rt22.Referent as DecreeReferent;
                    if (dr00.Typ == null) 
                    {
                        foreach (Pullenti.Ner.Slot s in dr00.Slots) 
                        {
                            if (s.TypeName == DecreeReferent.ATTR_DATE || s.TypeName == DecreeReferent.ATTR_SOURCE) 
                            {
                                if (dec.FindSlot(s.TypeName, null, true) == null) 
                                    dec.AddSlot(s.TypeName, s.Value, false, 0);
                            }
                        }
                        dr00 = null;
                    }
                    if (dr00 != null) 
                    {
                        rtLi.Add(rt22);
                        dec.AddSlot(DecreeReferent.ATTR_OWNER, rt22.Referent, false, 0);
                    }
                }
            }
            if (dts[0].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ && dts[0].TypKind == DecreeKind.Classifier) 
            {
                if (((rt.BeginToken.Previous != null && rt.BeginToken.Previous.IsChar(')') && (rt.BeginToken.Previous.Previous is Pullenti.Ner.TextToken)) && rt.BeginToken.Previous.Previous.Chars.IsAllUpper && rt.BeginToken.Previous.Previous.Previous != null) && rt.BeginToken.Previous.Previous.Previous.IsChar('(')) 
                {
                    rt.BeginToken = rt.BeginToken.Previous.Previous.Previous;
                    dec.AddNameStr((rt.BeginToken.Next as Pullenti.Ner.TextToken).Term, new Pullenti.Ner.MetaToken(rt.BeginToken.Next, rt.BeginToken.Next));
                }
            }
            rtLi.Add(rt);
            if (numTok != null && numTok.Children != null) 
            {
                Pullenti.Ner.Token end = rt.EndToken;
                rt.EndToken = numTok.Children[0].BeginToken.Previous;
                if (rt.EndToken.IsCommaAnd) 
                    rt.EndToken = rt.EndToken.Previous;
                for (int ii = 0; ii < numTok.Children.Count; ii++) 
                {
                    DecreeReferent dr1 = new DecreeReferent();
                    foreach (Pullenti.Ner.Slot s in rt.Referent.Slots) 
                    {
                        if (s.TypeName == DecreeReferent.ATTR_NUMBER) 
                            dr1.AddSlot(s.TypeName, numTok.Children[ii].Value, false, 0).Tag = numTok.Children[ii].GetSourceText();
                        else 
                        {
                            Pullenti.Ner.Slot ss = dr1.AddSlot(s.TypeName, s.Value, false, 0);
                            if (ss != null) 
                                ss.Tag = s.Tag;
                        }
                    }
                    Pullenti.Ner.ReferentToken rt1 = new Pullenti.Ner.ReferentToken(dr1, numTok.Children[ii].BeginToken, numTok.Children[ii].EndToken);
                    if (ii == (numTok.Children.Count - 1)) 
                        rt1.EndToken = end;
                    rtLi.Add(rt1);
                }
            }
            if ((dts.Count == 2 && dts[0].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ && dts[0].TypKind == DecreeKind.Standard) && dts[1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number) 
            {
                for (Pullenti.Ner.Token ttt = dts[1].EndToken.Next; ttt != null; ttt = ttt.Next) 
                {
                    if (!ttt.IsCommaAnd) 
                        break;
                    Pullenti.Ner.Decree.Internal.DecreeToken nu = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(ttt.Next, dts[0], false);
                    if (nu == null || nu.Typ != Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number) 
                        break;
                    DecreeReferent dr1 = new DecreeReferent();
                    dr1.Typ = dec.Typ;
                    dr1.AddNumber(nu);
                    rtLi.Add(new Pullenti.Ner.ReferentToken(dr1, ttt.Next, nu.EndToken));
                    if (!ttt.IsComma) 
                        break;
                    ttt = nu.EndToken;
                }
            }
            return rtLi;
        }
        void _processPartsAndChanges(Pullenti.Ner.Core.AnalysisKit kit, Pullenti.Ner.Decree.Internal.DecreeToken baseTyp)
        {
            Pullenti.Ner.Core.AnalyzerData ad = kit.GetAnalyzerData(this);
            List<DecreePartReferent> undefinedDecrees = new List<DecreePartReferent>();
            Pullenti.Ner.ReferentToken rootChange = null;
            Pullenti.Ner.Decree.Internal.DecreeChangeToken rootChangeQues = null;
            DecreeChangeReferent lastChange = null;
            DecreeReferent changeDecree = null;
            DecreePartReferent changePart = new DecreePartReferent();
            List<DecreePartReferent> changeStack = new List<DecreePartReferent>();
            int expireRegime = 0;
            int hasStartChange = 0;
            Pullenti.Ner.Decree.Internal.DecreeChangeToken lastAppend2 = null;
            bool hasChangeKeyword = false;
            for (Pullenti.Ner.Token t = kit.FirstToken; t != null; t = t.Next) 
            {
                if (t.IsIgnored) 
                    continue;
                List<Pullenti.Ner.Decree.Internal.PartToken> dts = null;
                if (t.IsNewlineBefore && t.IsChar('(')) 
                {
                }
                Pullenti.Ner.Decree.Internal.DecreeChangeToken dcht = null;
                bool ok1 = t.IsNewlineBefore;
                bool ignNl = false;
                if (!ok1) 
                {
                    if (rootChange != null && t.Previous != null && t.Previous.IsChar(':')) 
                        ok1 = true;
                }
                if (!ok1) 
                {
                    if ((t.Previous is Pullenti.Ner.ReferentToken) && (t.Previous.GetReferent() is DecreeChangeReferent)) 
                    {
                        ok1 = true;
                        ignNl = expireRegime > 0;
                        for (; t.Next != null; t = t.Next) 
                        {
                            if (t.IsCharOf(";,:.")) 
                            {
                            }
                            else if (t.IsAnd && !t.IsNewlineBefore && !t.Next.IsCharOf(").>")) 
                            {
                            }
                            else if (t.IsValue2("А", "ТАКЖЕ") && t.Next.Next != null) 
                                t = t.Next;
                            else 
                                break;
                        }
                    }
                }
                if (!ok1 && t.Previous != null && t.Previous.IsTableControlChar) 
                {
                    if (t.Previous.IsNewlineBefore || rootChange != null) 
                        ok1 = true;
                }
                if (ok1) 
                {
                    if (t.IsValue("Е", null) && t.Next != null && t.Next.IsChar(')')) 
                    {
                    }
                    if (t.IsValue2("ДОПОЛНИТЕЛЬНОЕ", "СОГЛАШЕНИЕ")) 
                        hasChangeKeyword = true;
                    if ((t is Pullenti.Ner.NumberToken) && (t as Pullenti.Ner.NumberToken).Value == "15") 
                    {
                    }
                    dcht = Pullenti.Ner.Decree.Internal.DecreeChangeToken.TryAttach(t, (rootChange == null ? null : rootChange.Referent as DecreeChangeReferent), false, changeStack, false, false, null, ignNl);
                    if (dcht != null && t.IsValue("ПРИЛОЖЕНИЕ", null)) 
                    {
                        Pullenti.Ner.Instrument.Internal.InstrToken1 line = Pullenti.Ner.Instrument.Internal.InstrToken1.Parse(t, true, null, 0, null, false, 0, false, false);
                        if (line != null && (line.LengthChar < 30)) 
                            dcht = null;
                    }
                    if (((dcht != null && dcht.Parts != null && dcht.Parts.Count == 1) && dcht.BeginToken == dcht.Parts[0].BeginToken && dcht.EndToken == dcht.Parts[0].EndToken) && dcht.IsNewlineAfter && dcht.Parts[0].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Clause) 
                    {
                        dcht = null;
                        rootChange = null;
                        changePart.Slots.Clear();
                        expireRegime = 0;
                        changeStack.Clear();
                    }
                    if (dcht != null) 
                    {
                        if (dcht.HasChangeKeyword) 
                            hasChangeKeyword = true;
                        if (dcht.ActKind2 == DecreeChangeKind.Append && dcht.Parts != null) 
                            lastAppend2 = dcht;
                        else if (lastAppend2 != null && dcht.Parts != null && dcht.ActKind == DecreeChangeKind.Undefined) 
                        {
                            dcht.NewParts = dcht.Parts;
                            dcht.Parts = lastAppend2.Parts;
                            dcht.RealPart = lastAppend2.RealPart;
                            if (dcht.RealPart != null) 
                                dcht.Parts = null;
                            dcht.ActKind = DecreeChangeKind.Append;
                        }
                    }
                }
                if (dcht != null && ((dcht.IsStart || dcht.ChangeVal != null)) && !dcht.Ignorable) 
                {
                    if (dcht.Typ == Pullenti.Ner.Decree.Internal.DecreeChangeTokenTyp.StartMultu) 
                    {
                        if (rootChange != null && dcht.Decree == null && dcht.Parts != null) 
                            dcht.Typ = Pullenti.Ner.Decree.Internal.DecreeChangeTokenTyp.Single;
                        else 
                        {
                            expireRegime = 0;
                            hasStartChange = 3;
                            lastAppend2 = null;
                            rootChange = null;
                            changePart.Slots.Clear();
                            changeDecree = null;
                        }
                    }
                    else if (dcht.Typ == Pullenti.Ner.Decree.Internal.DecreeChangeTokenTyp.Single) 
                    {
                        Pullenti.Ner.Decree.Internal.DecreeChangeToken dcht1 = Pullenti.Ner.Decree.Internal.DecreeChangeToken.TryAttach(dcht.EndToken.Next, (rootChange == null ? null : rootChange.Referent as DecreeChangeReferent), false, changeStack, false, false, null, false);
                        if (dcht1 != null && dcht1.IsStart && !dcht1.Ignorable) 
                        {
                            hasStartChange = 2;
                            if (dcht.DecreeTok != null && dcht.Decree != null) 
                            {
                                Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(dcht.Decree, dcht.DecreeTok.BeginToken, dcht.DecreeTok.EndToken);
                                kit.EmbedToken(rt);
                                t = rt;
                                if (dcht.EndChar == t.EndChar) 
                                    dcht.EndToken = t;
                            }
                        }
                    }
                    else if (dcht.Typ == Pullenti.Ner.Decree.Internal.DecreeChangeTokenTyp.StartSingle && dcht.Decree != null && expireRegime == 0) 
                    {
                        bool ok = (dcht.HasChangeKeyword || lastChange != null || hasChangeKeyword) || dcht.ChangeVal != null || dcht.ActKind != DecreeChangeKind.Undefined;
                        if (!ok && t.GetReferent() != dcht.Decree) 
                        {
                            for (Pullenti.Ner.Token tt2 = dcht.EndToken.Next; tt2 != null; tt2 = tt2.Next) 
                            {
                                Pullenti.Ner.Decree.Internal.DecreeChangeToken next = Pullenti.Ner.Decree.Internal.DecreeChangeToken.TryAttach(tt2, null, true, null, false, false, null, false);
                                if (next != null) 
                                {
                                    if (next.ChangeVal != null || next.ActKind != DecreeChangeKind.Undefined) 
                                        ok = true;
                                    break;
                                }
                                if (tt2.IsComma) 
                                    continue;
                                if (tt2.IsValue("ВНЕСТИ", null)) 
                                    ok = true;
                                break;
                            }
                        }
                        if (ok) 
                        {
                            hasStartChange = 2;
                            hasChangeKeyword = true;
                            if (dcht.DecreeTok != null) 
                            {
                                Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(dcht.Decree, dcht.DecreeTok.BeginToken, dcht.DecreeTok.EndToken);
                                kit.EmbedToken(rt);
                                t = rt;
                                if (dcht.EndChar == t.EndChar) 
                                    dcht.EndToken = t;
                            }
                        }
                        else 
                            dts = dcht.Parts ?? dcht.NewParts;
                    }
                    if (dcht.Typ == Pullenti.Ner.Decree.Internal.DecreeChangeTokenTyp.StartSingle && rootChange != null && dcht.Decree == null) 
                        hasStartChange = 2;
                    else if ((dcht.Typ == Pullenti.Ner.Decree.Internal.DecreeChangeTokenTyp.Single && dcht.Decree != null && dcht.EndToken.IsChar(':')) && dcht.IsNewlineAfter) 
                        hasStartChange = 2;
                    if (hasStartChange <= 0 && expireRegime == 0) 
                    {
                        dts = Pullenti.Ner.Decree.Internal.PartToken.TryAttachList(t, false, 40);
                        changeStack.Clear();
                        if (dcht.ActKind == DecreeChangeKind.Expire) 
                            expireRegime = 1;
                        else if (dcht.ActKind == DecreeChangeKind.Suspend) 
                            expireRegime = 2;
                        if (!dcht.Ignorable) 
                            dts = dcht.Parts ?? dcht.NewParts;
                    }
                    else 
                    {
                        if (dcht.Decree != null) 
                        {
                            if (changeDecree != dcht.Decree) 
                                rootChange = null;
                            changeDecree = dcht.Decree;
                            changePart.Slots.Clear();
                            changeStack.Clear();
                        }
                        if (dcht.ActKind == DecreeChangeKind.Expire) 
                            expireRegime = 1;
                        else if (dcht.ActKind == DecreeChangeKind.Suspend) 
                            expireRegime = 2;
                        else if (dcht.ActKind == DecreeChangeKind.ExpireChanges) 
                            expireRegime = 3;
                        if (!dcht.Ignorable) 
                        {
                            dts = dcht.Parts;
                            if ((dts == null && dcht.NewParts != null && dcht.ActKind == DecreeChangeKind.Undefined) && dcht.ChangeVal == null) 
                                dts = dcht.NewParts;
                        }
                    }
                }
                else 
                {
                    dts = Pullenti.Ner.Decree.Internal.PartToken.TryAttachList(t, false, 40);
                    if ((dcht == null && t.IsNewlineBefore && !t.IsCharOf("[")) && !t.IsTableControlChar) 
                    {
                        bool keepExpire = false;
                        if (expireRegime > 0) 
                        {
                            Pullenti.Ner.Token tt3 = t;
                            for (int kk = 0; kk < 3; kk++) 
                            {
                                Pullenti.Ner.Instrument.Internal.InstrToken1 line = Pullenti.Ner.Instrument.Internal.InstrToken1.Parse(tt3, true, null, 0, null, false, 0, false, false);
                                if (line == null) 
                                    break;
                                if (line.Numbers.Count > 0) 
                                {
                                    Pullenti.Ner.Decree.Internal.DecreeChangeToken ddd = Pullenti.Ner.Decree.Internal.DecreeChangeToken.TryAttach(tt3, null, false, null, false, false, null, false);
                                    if (ddd != null) 
                                    {
                                        if (ddd.IsNewlineAfter) 
                                            keepExpire = true;
                                        else if (ddd.EndToken.Next.IsCharOf(";.")) 
                                            keepExpire = true;
                                    }
                                }
                                tt3 = line.EndToken.Next;
                            }
                        }
                        if (!keepExpire) 
                        {
                            expireRegime = 0;
                            hasStartChange--;
                        }
                    }
                }
                if (dts != null && dts.Count > 0 && rootChange != null) 
                {
                    if (dts[0].BeginChar < rootChange.BeginChar) 
                        dts = null;
                }
                if (dts != null && dts.Count == 1) 
                {
                    if ((dts[0].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Form || dts[0].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Formula || dts[0].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Name) || dts[0].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Table || dts[0].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.List) 
                    {
                        if (rootChange == null && ((rootChangeQues == null || dts[0].Values.Count == 0))) 
                            dts = null;
                    }
                }
                if (dts != null && dts.Count == 2 && dts[0].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Prefix) 
                {
                    if ((dts[1].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Form || dts[1].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Formula || dts[1].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Name) || dts[1].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Table || dts[1].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.List) 
                    {
                        if (rootChange == null && ((rootChangeQues == null || dts[0].Values.Count == 0))) 
                            dts = null;
                    }
                }
                if (dts != null && dts.Count >= 1) 
                {
                    if (dts[0].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Form || dts[0].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Table || dts[0].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.List) 
                    {
                        if (rootChange == null && ((rootChangeQues == null || dts[0].Values.Count == 0))) 
                            dts = null;
                    }
                }
                if (dcht != null && dts != null && !dcht.Ignorable) 
                {
                    foreach (Pullenti.Ner.Decree.Internal.PartToken pp in dts) 
                    {
                        if (pp.Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Appendix && pp.Values.Count == 0) 
                        {
                            if (!string.IsNullOrEmpty(changePart.Appendix)) 
                                pp.Values.Add(new Pullenti.Ner.Decree.Internal.PartToken.PartValue(pp.BeginToken, pp.EndToken) { Value = changePart.Appendix });
                        }
                        else 
                            changePart.RemoveSlots(pp.Typ);
                    }
                }
                if (dcht != null && dcht.NewParts != null) 
                {
                    foreach (Pullenti.Ner.Decree.Internal.PartToken pp in dcht.NewParts) 
                    {
                        changePart.RemoveSlots(pp.Typ);
                    }
                }
                if (changePart.Preamble != null && changePart.Slots.Count > 2) 
                    changePart.Preamble = null;
                if (changePart.NameAsItem != null) 
                    changePart.NameAsItem = null;
                if (changePart.Freetext != null) 
                    changePart.Freetext = null;
                List<Pullenti.Ner.MetaToken> rts = TryAttachParts(dts, baseTyp, (hasStartChange > 0 ? changeDecree : null), rootChange != null);
                if (rts != null) 
                {
                }
                if (dcht != null && dcht.AddParts != null) 
                {
                    foreach (List<Pullenti.Ner.Decree.Internal.PartToken> dts2 in dcht.AddParts) 
                    {
                        List<Pullenti.Ner.MetaToken> rts2 = TryAttachParts(dts2, baseTyp, (hasStartChange > 0 ? changeDecree : null), rootChange != null);
                        if (rts2 == null || rts2.Count == 0) 
                            continue;
                        if (rts == null) 
                            rts = rts2;
                        else 
                            rts.AddRange(rts2);
                    }
                }
                List<DecreePartReferent> dprs = null;
                Dictionary<DecreePartReferent, DecreePartReferent> diaps = null;
                Dictionary<int, Pullenti.Ner.Token> begs = null;
                Dictionary<int, Pullenti.Ner.Token> ends = null;
                if (rts != null) 
                {
                    foreach (Pullenti.Ner.MetaToken kp in rts) 
                    {
                        List<DecreePartReferent> dprList = kp.Tag as List<DecreePartReferent>;
                        if (dprList == null) 
                            continue;
                        for (int i = 0; i < dprList.Count; i++) 
                        {
                            DecreePartReferent dr = dprList[i];
                            if (dr.Owner == null && dr.Clause != null && dr.LocalTyp == null) 
                            {
                                if (!undefinedDecrees.Contains(dr)) 
                                    undefinedDecrees.Add(dr);
                            }
                            if ((dr.Clause == null && dr.DocPart != null && dr.Part == null) && changePart.Clause != null) 
                            {
                                dr.Part = dr.DocPart;
                                dr.DocPart = null;
                            }
                            if (dr.Owner != null && dr.Clause != null) 
                                undefinedDecrees.Clear();
                            bool ignLast = false;
                            if (dts.Count == 1) 
                            {
                                if ((dts[0].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Sentence || dts[0].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Indention || dts[0].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Formula) || dts[0].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Preamble) 
                                    ignLast = true;
                            }
                            if (dcht != null && !dcht.Ignorable) 
                                dr.AddHighLevelInfo(changePart);
                            if (lastChange != null && lastChange.Owners.Count > 0) 
                            {
                                DecreePartReferent dr0 = lastChange.Owners[0] as DecreePartReferent;
                                if (dr0 != null && dr.Owner == dr0.Owner) 
                                {
                                    if (dr0.Appendix != null && dr.Appendix != null && dr0.Appendix != dr.Appendix) 
                                    {
                                        lastChange = null;
                                        if (changeStack.Count == 1) 
                                            changeStack.Insert(0, dr);
                                    }
                                    else 
                                    {
                                        int mle = dr.GetMinLevel();
                                        if (mle == 1 && dr.DocPart != null && dr0.DocPart == null) 
                                        {
                                            dr.Part = dr.DocPart;
                                            dr.DocPart = null;
                                            mle = dr.GetMinLevel();
                                        }
                                        if (mle == 0 || mle <= Pullenti.Ner.Decree.Internal.PartToken._getRank(Pullenti.Ner.Decree.Internal.PartToken.ItemType.Clause)) 
                                        {
                                        }
                                        else if (dr.Indention != null && dr.Slots.Count == 2) 
                                            dr.AddHighLevelInfo(dr0);
                                    }
                                }
                            }
                            if (dr.TableColumn != null) 
                            {
                                if (rootChange != null) 
                                    dr.Table = "0";
                                else 
                                    continue;
                            }
                            if (dr.TableRow != null || dr.TableItem != null || dr.TableSubItem != null) 
                            {
                                if (dr.Table == null) 
                                {
                                    if (rootChange != null) 
                                        dr.Table = "0";
                                    else 
                                        continue;
                                }
                            }
                            dr = ad.RegisterReferent(dr) as DecreePartReferent;
                            if (rts.Count == 1 && dprList.Count == 1) 
                            {
                                if (lastAppend2 != null && lastAppend2.Parts == dts) 
                                    lastAppend2.RealPart = dr;
                                if (dcht != null && dcht.Parts == dts) 
                                    dcht.RealPart = dr;
                                if (dcht != null && dcht.Alias != null && dr.Appendix != null) 
                                {
                                    Pullenti.Ner.Core.TerminCollection partAliases = null;
                                    object oo;
                                    if (!kit.MiscData.TryGetValue("partAliases", out oo)) 
                                        kit.MiscData.Add("partAliases", (partAliases = new Pullenti.Ner.Core.TerminCollection()));
                                    else 
                                        partAliases = (Pullenti.Ner.Core.TerminCollection)oo;
                                    Pullenti.Ner.Core.Termin term = new Pullenti.Ner.Core.Termin();
                                    term.InitBy(dcht.Alias.BeginToken, dcht.Alias.EndToken.Previous, dr, false);
                                    partAliases.Add(term);
                                }
                                if (changeDecree != null) 
                                {
                                    if (dr.Owner != null && dr.Owner != changeDecree) 
                                    {
                                    }
                                    else if (changeStack.Count == 0 || ((dcht != null && ((dcht.Typ == Pullenti.Ner.Decree.Internal.DecreeChangeTokenTyp.Single || dcht.Typ == Pullenti.Ner.Decree.Internal.DecreeChangeTokenTyp.StartSingle))))) 
                                    {
                                        changePart = dr.Clone() as DecreePartReferent;
                                        changePart.NameAsItem = null;
                                        changePart.Preamble = null;
                                        changeStack.Insert(0, changePart);
                                    }
                                }
                            }
                            if (dprs == null) 
                                dprs = new List<DecreePartReferent>();
                            dprs.Add(dr);
                            Pullenti.Ner.ReferentToken rt;
                            if (i == 0) 
                                rt = new Pullenti.Ner.ReferentToken(dr, kp.BeginToken, kp.EndToken) { Morph = kp.Morph };
                            else 
                                rt = new Pullenti.Ner.ReferentToken(dr, t, t);
                            kit.EmbedToken(rt);
                            t = rt;
                            if (dprs.Count > 1 && t.Previous != null && ((t.Previous.IsHiphen || t.Previous.IsValue("ПО", null)))) 
                            {
                                if (diaps == null) 
                                    diaps = new Dictionary<DecreePartReferent, DecreePartReferent>();
                                if (!diaps.ContainsKey(dprs[dprs.Count - 2])) 
                                    diaps.Add(dprs[dprs.Count - 2], dprs[dprs.Count - 1]);
                            }
                            if (begs == null) 
                                begs = new Dictionary<int, Pullenti.Ner.Token>();
                            if (!begs.ContainsKey(t.BeginChar)) 
                                begs.Add(t.BeginChar, t);
                            else 
                                begs[t.BeginChar] = t;
                            if (ends == null) 
                                ends = new Dictionary<int, Pullenti.Ner.Token>();
                            if (!ends.ContainsKey(t.EndChar)) 
                                ends.Add(t.EndChar, t);
                            else 
                                ends[t.EndChar] = t;
                            if (dcht != null && !dcht.Ignorable) 
                            {
                                if (dcht.BeginChar == t.BeginChar) 
                                    dcht.BeginToken = t;
                                if (dcht.EndChar == t.EndChar) 
                                    dcht.EndToken = t;
                                if (t.EndChar > dcht.EndChar) 
                                    dcht.EndToken = t;
                                dcht.RealPart = dr;
                            }
                        }
                    }
                }
                if (dts != null && dts.Count > 0 && dts[dts.Count - 1].EndChar > t.EndChar) 
                    t = dts[dts.Count - 1].EndToken;
                if (dcht != null && ((hasStartChange > 0 || expireRegime > 0)) && !dcht.Ignorable) 
                {
                    if (dcht.EndChar > t.EndChar) 
                        t = dcht.EndToken;
                    List<Pullenti.Ner.ReferentToken> chrt = null;
                    if ((dcht.Typ == Pullenti.Ner.Decree.Internal.DecreeChangeTokenTyp.Single && dcht.Decree != null && dcht.EndToken.IsChar(':')) && dcht.ActKind == DecreeChangeKind.Undefined) 
                    {
                        if (rootChange == null || changeDecree == null) 
                            dcht.Typ = Pullenti.Ner.Decree.Internal.DecreeChangeTokenTyp.StartMultu;
                        else if (changeDecree != dcht.Decree) 
                            dcht.Typ = Pullenti.Ner.Decree.Internal.DecreeChangeTokenTyp.StartMultu;
                    }
                    if (dcht.Typ == Pullenti.Ner.Decree.Internal.DecreeChangeTokenTyp.StartMultu && ((rootChangeQues == null || rootChangeQues.Decree != dcht.Decree))) 
                    {
                        rootChange = null;
                        changeStack.Clear();
                        if (dcht.Decree != null) 
                        {
                            while (dcht.AddDecrees != null && dcht.AddDecrees.Count > 0) 
                            {
                                DecreeReferent d0 = dcht.AddDecrees[0];
                                if (d0.Owner != null) 
                                {
                                    if (dcht.Decree.Owner == d0.Owner) 
                                    {
                                        dcht.Decree = d0.Owner;
                                        dcht.AddDecrees.RemoveAt(0);
                                        continue;
                                    }
                                    if (d0.Owner == dcht.Decree || d0 == dcht.Decree) 
                                    {
                                        dcht.AddDecrees.RemoveAt(0);
                                        continue;
                                    }
                                }
                                else if (dcht.Decree.Owner == d0) 
                                {
                                    dcht.Decree = d0.Owner;
                                    dcht.AddDecrees.RemoveAt(0);
                                    continue;
                                }
                                break;
                            }
                            changeDecree = dcht.Decree;
                        }
                        if (dprs != null && dprs.Count > 0) 
                        {
                            if (changeDecree == null && dprs[0].Owner != null) 
                                changeDecree = dprs[0].Owner;
                            changeStack.Insert(0, dprs[0]);
                            changePart = dprs[0].Clone() as DecreePartReferent;
                        }
                        if (((changeStack.Count > 0 || dcht.Decree != null)) && expireRegime == 0) 
                        {
                            DecreeChangeReferent rch = ad.RegisterReferent(new DecreeChangeReferent() { Kind = DecreeChangeKind.Container }) as DecreeChangeReferent;
                            if (changeStack.Count > 0) 
                                rch.AddSlot(DecreeChangeReferent.ATTR_OWNER, changeStack[0], false, 0);
                            else 
                            {
                                rch.AddSlot(DecreeChangeReferent.ATTR_OWNER, dcht.Decree, false, 0);
                                if (dcht.AddDecrees != null) 
                                {
                                    foreach (DecreeReferent d in dcht.AddDecrees) 
                                    {
                                        rch.AddSlot(DecreeChangeReferent.ATTR_OWNER, d, false, 0);
                                    }
                                }
                            }
                            if (dcht.AppExtChanges != null && rch.Value == null) 
                            {
                                DecreeChangeValueReferent appCh = new DecreeChangeValueReferent() { Kind = DecreeChangeValueKind.ExtAppendix };
                                appCh.Value = (dcht.AppExtChanges.Values.Count == 0 ? "" : dcht.AppExtChanges.Values[0].ToString());
                                rch.Value = ad.RegisterReferent(appCh) as DecreeChangeValueReferent;
                            }
                            rootChange = new Pullenti.Ner.ReferentToken(rch, dcht.BeginToken, dcht.EndToken);
                            if (rootChange.EndToken.IsChar(':')) 
                                rootChange.EndToken = rootChange.EndToken.Previous;
                            for (Pullenti.Ner.Token tt0 = dcht.BeginToken.Previous; tt0 != null; tt0 = tt0.Previous) 
                            {
                                if (tt0.IsNewlineBefore && (dcht.BeginChar - tt0.BeginChar) > 30) 
                                {
                                    Pullenti.Ner.Decree.Internal.DecreeChangeToken dcht0 = Pullenti.Ner.Decree.Internal.DecreeChangeToken.TryAttach(tt0, null, false, null, false, false, null, false);
                                    if ((dcht0 != null && dcht0.Typ == Pullenti.Ner.Decree.Internal.DecreeChangeTokenTyp.StartSingle && dcht0.Decree != null) && dcht.Decree != null) 
                                    {
                                        if (dcht0.Decree.CanBeEquals(dcht.Decree, Pullenti.Ner.Core.ReferentsEqualType.WithinOneText) || dcht0.Decree.CanBeEquals(dcht.Decree.Owner, Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)) 
                                            rootChange.BeginToken = dcht0.BeginToken;
                                    }
                                    break;
                                }
                            }
                            kit.EmbedToken(rootChange);
                            t = rootChange;
                            if (t.Next != null && t.Next.IsChar(':')) 
                                t = t.Next;
                        }
                        if (expireRegime == 0) 
                        {
                            if (t.IsNewlineAfter) 
                                continue;
                            Pullenti.Ner.Decree.Internal.DecreeChangeToken tt2 = Pullenti.Ner.Decree.Internal.DecreeChangeToken.TryAttach(t.Next, null, false, null, false, false, null, false);
                            if (tt2 != null && tt2.ActKind != DecreeChangeKind.Undefined) 
                            {
                            }
                            else 
                                continue;
                        }
                    }
                    if (dcht.Typ == Pullenti.Ner.Decree.Internal.DecreeChangeTokenTyp.Single && dprs != null && dprs.Count == 1) 
                    {
                        while (changeStack.Count > 0) 
                        {
                            if (dprs[0].IsAllItemsLessLevel(changeStack[0], true)) 
                                break;
                            else 
                                changeStack.RemoveAt(0);
                        }
                        changeStack.Insert(0, dprs[0]);
                        if (dprs[0].Owner != null && changeDecree != dprs[0].Owner) 
                        {
                            changeDecree = dprs[0].Owner;
                            changeStack.Clear();
                            changeStack.Insert(0, dprs[0]);
                        }
                        changePart = dprs[0].Clone() as DecreePartReferent;
                        continue;
                    }
                    if (dprs == null && dcht.RealPart != null) 
                    {
                        dprs = new List<DecreePartReferent>();
                        dprs.Add(dcht.RealPart);
                    }
                    if ((dprs == null && dcht.ActKind == DecreeChangeKind.Remove && changeStack.Count > 0) && (changeStack[0] is DecreePartReferent)) 
                    {
                        dprs = new List<DecreePartReferent>();
                        dprs.Add(changeStack[0] as DecreePartReferent);
                    }
                    DecreePartReferent prevPart = null;
                    DecreeChangeReferent prev = null;
                    for (Pullenti.Ner.Token tt2 = dcht.BeginToken.Previous; tt2 != null; tt2 = tt2.Previous) 
                    {
                        if (tt2.GetReferent() is DecreePartReferent) 
                        {
                            if (changeStack.Count > 0 && changeStack[0].ParentReferent == (tt2.GetReferent() as DecreePartReferent).ParentReferent) 
                            {
                                prevPart = tt2.GetReferent() as DecreePartReferent;
                                break;
                            }
                            if (changeStack.Count == 0 && (tt2.GetReferent() as DecreePartReferent).ParentReferent == changeDecree) 
                            {
                                prevPart = tt2.GetReferent() as DecreePartReferent;
                                break;
                            }
                        }
                        if (tt2.LengthChar == 1 || (tt2 is Pullenti.Ner.NumberToken)) 
                            continue;
                        prev = tt2.GetReferent() as DecreeChangeReferent;
                        if (prev != null) 
                        {
                            if (prev.Kind == DecreeChangeKind.New) 
                                continue;
                            prevPart = prev.GetSlotValue(DecreeChangeReferent.ATTR_OWNER) as DecreePartReferent;
                            if (prevPart != null && ((prevPart.NameAsItem != null || prevPart.Preamble != null || prevPart.Freetext != null))) 
                            {
                                prevPart = null;
                                continue;
                            }
                            if (rootChange != null && prev != rootChange.Referent && prevPart == null) 
                                changeStack.Clear();
                        }
                        break;
                    }
                    if (dcht.HasAnafor && dcht.Parts == null && prevPart != null) 
                        dcht.RealPart = prevPart;
                    if (dcht.Parts == null && dcht.NewParts != null && dcht.ActKind == DecreeChangeKind.Undefined) 
                    {
                        if (lastChange != null && lastChange.Kind == DecreeChangeKind.Append) 
                        {
                            dcht.ActKind = DecreeChangeKind.Append;
                            dcht.RealPart = lastChange.ParentReferent as DecreePartReferent;
                        }
                        else if (lastAppend2 != null && lastAppend2.RealPart != null) 
                        {
                            dcht.ActKind = DecreeChangeKind.Append;
                            dcht.RealPart = lastAppend2.RealPart;
                        }
                    }
                    if (dprs != null && dprs.Count > 0) 
                    {
                        if (dcht.InPart != null && dcht.RealPart != null && dprs[0] == dcht.RealPart) 
                        {
                            Pullenti.Ner.Decree.Internal.PartToken ppp = Pullenti.Ner.Decree.Internal.PartToken.TryAttach(dcht.InPart, null, false, true);
                            if (ppp != null) 
                            {
                                DecreePartReferent rp = dcht.RealPart.Clone() as DecreePartReferent;
                                rp.AddSlot(Pullenti.Ner.Decree.Internal.PartToken._getAttrNameByTyp(ppp.Typ), "?", false, 0);
                                dcht.RealPart = ad.RegisterReferent(rp) as DecreePartReferent;
                                dprs[0] = dcht.RealPart;
                            }
                        }
                        chrt = Pullenti.Ner.Decree.Internal.DecreeChangeToken.AttachReferents(dprs[0], dcht);
                        if (chrt == null && expireRegime > 0) 
                        {
                            chrt = new List<Pullenti.Ner.ReferentToken>();
                            DecreeChangeReferent dcr = new DecreeChangeReferent() { Kind = (expireRegime == 1 ? DecreeChangeKind.Expire : DecreeChangeKind.Suspend) };
                            chrt.Add(new Pullenti.Ner.ReferentToken(dcr, dcht.BeginToken, dcht.EndToken));
                        }
                        if ((((chrt == null && dcht.ActKind == DecreeChangeKind.Undefined && dcht.ActKind2 == DecreeChangeKind.Undefined) && prev != null && prev.Kind == DecreeChangeKind.Append) && dcht.ChangeVal != null && prevPart != null) && dcht.RealPart == null) 
                        {
                            dcht.ActKind = DecreeChangeKind.Append;
                            if (dcht.NewParts == null) 
                                dcht.NewParts = dcht.Parts;
                            dcht.RealPart = prevPart;
                            dcht.Parts = null;
                            chrt = Pullenti.Ner.Decree.Internal.DecreeChangeToken.AttachReferents(prevPart, dcht);
                        }
                    }
                    else if (dcht.ActKind != DecreeChangeKind.Expire) 
                    {
                        bool ee = false;
                        if (dcht.PartTyp == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Undefined) 
                        {
                            if (changeStack.Count == 0) 
                            {
                                ee = true;
                                chrt = Pullenti.Ner.Decree.Internal.DecreeChangeToken.AttachReferents(changeDecree, dcht);
                            }
                            else if (prevPart != null) 
                            {
                                ee = true;
                                chrt = Pullenti.Ner.Decree.Internal.DecreeChangeToken.AttachReferents(prevPart, dcht);
                            }
                        }
                        if (!ee && changeDecree != null) 
                        {
                            Pullenti.Ner.Referent dr = (Pullenti.Ner.Referent)changeDecree;
                            if (prevPart != null && prevPart.Owner == dr && prevPart.IsAllItemsOverThisLevel(dcht.PartTyp)) 
                                dr = prevPart;
                            else if (rootChange != null) 
                            {
                                for (Pullenti.Ner.Token tt2 = t.Previous; tt2 != null && tt2.BeginChar > rootChange.EndChar; tt2 = tt2.Previous) 
                                {
                                    Pullenti.Ner.Referent rr = tt2.GetReferent();
                                    if (rr == null) 
                                        continue;
                                    DecreePartReferent pp = rr as DecreePartReferent;
                                    if (pp == null && (rr is DecreeChangeReferent)) 
                                        pp = rr.GetSlotValue(DecreePartReferent.ATTR_OWNER) as DecreePartReferent;
                                    if (pp == null) 
                                        continue;
                                    if (pp.Owner != null && pp.Owner.Kind == DecreeKind.Publisher) 
                                        continue;
                                    pp = pp.Clone() as DecreePartReferent;
                                    Pullenti.Ner.Slot sl = pp.FindSlot(Pullenti.Ner.Decree.Internal.PartToken._getAttrNameByTyp(dcht.PartTyp), null, true);
                                    if (sl != null) 
                                        pp.Slots.Remove(sl);
                                    if (pp.Slots.Count > 1 && pp.IsAllItemsOverThisLevel(dcht.PartTyp)) 
                                    {
                                        dr = ad.RegisterReferent(pp) as DecreePartReferent;
                                        break;
                                    }
                                }
                            }
                            if (dr == changeDecree && changePart.Slots.Count > 1 && dcht.ActKind == DecreeChangeKind.Append) 
                            {
                                if (changePart.IsAllItemsOverThisLevel(dcht.PartTyp)) 
                                    dr = ad.RegisterReferent(changePart) as DecreePartReferent;
                            }
                            if ((dr is DecreeReferent) && changePart.Appendix != null) 
                                dr = ad.RegisterReferent(changePart) as DecreePartReferent;
                            ee = true;
                            chrt = Pullenti.Ner.Decree.Internal.DecreeChangeToken.AttachReferents(dr, dcht);
                        }
                        if (lastChange != null && !ee && lastChange.Owners.Count > 0) 
                            chrt = Pullenti.Ner.Decree.Internal.DecreeChangeToken.AttachReferents(lastChange.Owners[0], dcht);
                    }
                    if ((dprs == null && chrt == null && ((dcht.Typ == Pullenti.Ner.Decree.Internal.DecreeChangeTokenTyp.Value || dcht.ChangeVal != null))) && changeStack.Count > 0) 
                        chrt = Pullenti.Ner.Decree.Internal.DecreeChangeToken.AttachReferents((dcht.RealPart != null ? dcht.RealPart : (prevPart == null || (changeStack[0] is DecreePartReferent) ? changeStack[0] : prevPart)), dcht);
                    if (chrt == null && ((expireRegime > 0 || dcht.ActKind == DecreeChangeKind.Expire || dcht.ActKind == DecreeChangeKind.Suspend)) && dprs == null) 
                    {
                        chrt = new List<Pullenti.Ner.ReferentToken>();
                        if (dcht.Decree != null) 
                        {
                            DecreeChangeReferent dcr = new DecreeChangeReferent() { Kind = DecreeChangeKind.Expire };
                            dcr.AddSlot(DecreeChangeReferent.ATTR_OWNER, dcht.Decree, false, 0);
                            chrt.Add(new Pullenti.Ner.ReferentToken(dcr, dcht.BeginToken, dcht.EndToken));
                        }
                        for (Pullenti.Ner.Token tt = dcht.EndToken.Next; tt != null; tt = tt.Next) 
                        {
                            if (tt.Next == null) 
                                break;
                            if (tt.IsCharOf("(<")) 
                            {
                                Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(tt, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                                if (br != null) 
                                {
                                    tt = br.EndToken;
                                    chrt[chrt.Count - 1].EndToken = tt;
                                    continue;
                                }
                            }
                            if (!tt.IsCommaAnd && !tt.IsChar(';')) 
                            {
                                if (chrt.Count > 0) 
                                    break;
                            }
                            else 
                                tt = tt.Next;
                            if (tt.GetReferent() is DecreeReferent) 
                            {
                                DecreeChangeReferent dcr = new DecreeChangeReferent() { Kind = (expireRegime == 1 || dcht.ActKind == DecreeChangeKind.Expire ? DecreeChangeKind.Expire : DecreeChangeKind.Suspend) };
                                dcr.AddSlot(DecreeChangeReferent.ATTR_OWNER, tt.GetReferent(), false, 0);
                                Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(dcr, tt, tt);
                                if (tt.Next != null && tt.Next.IsChar('(')) 
                                {
                                    Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(tt.Next, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                                    if (br != null) 
                                        rt.EndToken = (tt = br.EndToken);
                                }
                                chrt.Add(rt);
                                continue;
                            }
                            break;
                        }
                    }
                    if (chrt == null && dcht.Decree != null && rootChange == null) 
                    {
                        if (rootChangeQues != null && rootChangeQues.Decree == dcht.Decree && rootChangeQues.Typ == Pullenti.Ner.Decree.Internal.DecreeChangeTokenTyp.StartMultu) 
                        {
                        }
                        else 
                            rootChangeQues = dcht;
                    }
                    if (chrt != null) 
                    {
                        if (((dprs != null && dprs.Count == 2 && chrt.Count == 2) && (chrt[0].Referent is DecreeChangeValueReferent) && (chrt[1].Referent is DecreeChangeReferent)) && (chrt[1].Referent as DecreeChangeReferent).Kind == DecreeChangeKind.New) 
                        {
                            List<Pullenti.Ner.ReferentToken> vals = Pullenti.Ner.Decree.Internal.DecreeChangeToken.SplitValue(chrt[0], dprs[0]);
                            List<DecreePartReferent> diap = null;
                            if (diaps == null) 
                            {
                                diap = new List<DecreePartReferent>();
                                diap.Add(dprs[0]);
                                diap.Add(dprs[1]);
                            }
                            else if (diaps.Count == 1) 
                            {
                                foreach (KeyValuePair<DecreePartReferent, DecreePartReferent> kp in diaps) 
                                {
                                    diap = Pullenti.Ner.Decree.Internal.PartToken.TryCreateBetween(kp.Key, kp.Value);
                                    if (diap != null) 
                                    {
                                        diap.Insert(0, dprs[0]);
                                        diap.Add(dprs[1]);
                                    }
                                    break;
                                }
                            }
                            if (diap != null && vals != null && vals.Count == diap.Count) 
                            {
                                for (int i = 0; i < diap.Count; i++) 
                                {
                                    DecreeChangeReferent drc = new DecreeChangeReferent();
                                    drc.Kind = DecreeChangeKind.New;
                                    if (i > 0 && ((i + 1) < diap.Count)) 
                                        diap[i] = ad.RegisterReferent(diap[i]) as DecreePartReferent;
                                    drc.AddSlot(DecreeChangeReferent.ATTR_OWNER, diap[i], false, 0);
                                    drc.AddSlot(DecreeChangeReferent.ATTR_VALUE, vals[i].Referent, false, 0);
                                    Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(drc, vals[i].BeginToken, vals[i].EndToken);
                                    if (i == 0) 
                                        rt.BeginToken = chrt[1].BeginToken;
                                    else 
                                        drc.AddSlot("SPLIT", "true", false, 0);
                                    vals.Add(rt);
                                }
                                chrt = vals;
                                diaps = null;
                                dprs = null;
                            }
                        }
                        foreach (Pullenti.Ner.ReferentToken rt in chrt) 
                        {
                            rt.Referent = ad.RegisterReferent(rt.Referent);
                            if (rt.Referent is DecreeChangeReferent) 
                            {
                                lastChange = rt.Referent as DecreeChangeReferent;
                                if (dprs != null && dprs.Count > 0) 
                                {
                                    if ((rt.Referent as DecreeChangeReferent).Kind != DecreeChangeKind.New) 
                                        changePart = dprs[0].Clone() as DecreePartReferent;
                                    int ii;
                                    for (ii = 0; ii < (dprs.Count - 1); ii++) 
                                    {
                                        lastChange.AddSlot(DecreeChangeReferent.ATTR_OWNER, dprs[ii], false, 0);
                                    }
                                    if (diaps != null) 
                                    {
                                        foreach (KeyValuePair<DecreePartReferent, DecreePartReferent> kp in diaps) 
                                        {
                                            List<DecreePartReferent> diap = Pullenti.Ner.Decree.Internal.PartToken.TryCreateBetween(kp.Key, kp.Value);
                                            if (diap != null && diap.Count > 0) 
                                            {
                                                kp.Key.AddSlot("RANGE", "START", false, 0);
                                                kp.Value.AddSlot("RANGE", "END", false, 0);
                                                foreach (DecreePartReferent d in diap) 
                                                {
                                                    d.AddSlot("RANGE", "MIDDLE", false, 0);
                                                    Pullenti.Ner.Referent dd = ad.RegisterReferent(d);
                                                    lastChange.AddSlot(DecreeChangeReferent.ATTR_OWNER, dd, false, 0);
                                                }
                                            }
                                        }
                                    }
                                    for (; ii < dprs.Count; ii++) 
                                    {
                                        lastChange.AddSlot(DecreeChangeReferent.ATTR_OWNER, dprs[ii], false, 0);
                                    }
                                }
                            }
                            if (rt.BeginToken == null || rt.EndToken == null) 
                                continue;
                            if (begs != null && begs.ContainsKey(rt.BeginChar)) 
                                rt.BeginToken = begs[rt.BeginChar];
                            if (ends != null && ends.ContainsKey(rt.EndChar)) 
                                rt.EndToken = ends[rt.EndChar];
                            if (rootChange == null && rootChangeQues != null) 
                            {
                                DecreeChangeReferent rch = ad.RegisterReferent(new DecreeChangeReferent() { Kind = DecreeChangeKind.Container }) as DecreeChangeReferent;
                                rch.AddSlot(DecreeChangeReferent.ATTR_OWNER, rootChangeQues.Decree, false, 0);
                                rootChange = new Pullenti.Ner.ReferentToken(rch, rootChangeQues.BeginToken, rootChangeQues.EndToken);
                                if (rootChange.EndToken.IsChar(':')) 
                                    rootChange.EndToken = rootChange.EndToken.Previous;
                                kit.EmbedToken(rootChange);
                                rootChangeQues = null;
                            }
                            if (rootChange != null && (rt.Referent is DecreeChangeReferent)) 
                                rootChange.Referent.AddSlot(DecreeChangeReferent.ATTR_CHILD, rt.Referent, false, 0);
                            if (changePart.TableSubItem != null) 
                                changePart.TableSubItem = null;
                            else 
                                changePart.TableItem = null;
                            kit.EmbedToken(rt);
                            t = rt;
                            if (begs == null) 
                                begs = new Dictionary<int, Pullenti.Ner.Token>();
                            if (!begs.ContainsKey(t.BeginChar)) 
                                begs.Add(t.BeginChar, t);
                            else 
                                begs[t.BeginChar] = t;
                            if (ends == null) 
                                ends = new Dictionary<int, Pullenti.Ner.Token>();
                            if (!ends.ContainsKey(t.EndChar)) 
                                ends.Add(t.EndChar, t);
                            else 
                                ends[t.EndChar] = t;
                        }
                    }
                }
            }
            DecreeChangeReferent own = null;
            for (Pullenti.Ner.Token t = kit.FirstToken; t != null; t = t.Next) 
            {
                DecreeChangeReferent ch = t.GetReferent() as DecreeChangeReferent;
                if (ch == null) 
                    continue;
                Pullenti.Ner.ReferentToken t0 = t as Pullenti.Ner.ReferentToken;
                t = t0;
                if (ch.Kind == DecreeChangeKind.Container) 
                {
                    if (ch.Children.Count == 0) 
                        continue;
                    own = ch;
                    DecreeReferent dec = ch.GetSlotValue(DecreeChangeReferent.ATTR_OWNER) as DecreeReferent;
                    if (dec != null && dec.Kind == DecreeKind.Contract) 
                        continue;
                }
                Pullenti.Ner.Token tt = t.Next;
                bool notAttachVal = false;
                bool isPsevdoApp = false;
                while (tt != null) 
                {
                    for (; tt != null; tt = tt.Next) 
                    {
                        if (tt.IsNewlineBefore) 
                            break;
                    }
                    if (tt == null) 
                        break;
                    Pullenti.Ner.Decree.Internal.PartToken pp = Pullenti.Ner.Decree.Internal.PartToken.TryAttach(tt, null, false, false);
                    if (pp != null) 
                    {
                        Pullenti.Ner.Token tte = pp.EndToken.Next;
                        if (tte != null && tte.IsCharOf(".)")) 
                            tte = tte.Next;
                        if (tte == null || tte.IsNewlineAfter) 
                        {
                            tt = null;
                            break;
                        }
                        if (tte.IsValue2("О", "ВНЕСЕНИИ")) 
                        {
                            tt = null;
                            break;
                        }
                    }
                    if (tt.IsValue("ПРИЛОЖЕНИЕ", null)) 
                    {
                        Pullenti.Ner.Decree.Internal.PartToken pp2 = Pullenti.Ner.Decree.Internal.PartToken.TryAttach(tt, null, false, false);
                        if (pp2 != null) 
                        {
                            if (pp2.IsNewlineAfter || ((pp2.EndToken.Next != null && pp2.EndToken.Next.IsValue("К", null)))) 
                            {
                                bool stop = true;
                                Pullenti.Ner.Referent ooo = ch.GetSlotValue(DecreeChangeReferent.ATTR_OWNER) as Pullenti.Ner.Referent;
                                if ((ooo is DecreeReferent) && tt.Previous == t0) 
                                {
                                    stop = false;
                                    Pullenti.Ner.MetaToken mtt = t0.EndToken as Pullenti.Ner.MetaToken;
                                    if (mtt != null) 
                                    {
                                        if (Pullenti.Ner.Core.BracketHelper.IsBracket(mtt.EndToken, false) || Pullenti.Ner.Core.BracketHelper.IsBracket(mtt.EndToken.Previous, false)) 
                                            stop = true;
                                        else 
                                            isPsevdoApp = true;
                                    }
                                }
                                if (stop) 
                                {
                                    tt = null;
                                    break;
                                }
                            }
                        }
                    }
                    Pullenti.Ner.Token ttt = tt;
                    Pullenti.Ner.Core.ComplexNumToken num = Pullenti.Ner.Core.ComplexNumToken.TryParse(tt, null, false, false);
                    if (num != null) 
                        ttt = num.EndToken.Next;
                    if (ttt == null) 
                        break;
                    if (ttt.GetReferent() is DecreeChangeReferent) 
                    {
                        tt = null;
                        break;
                    }
                    Pullenti.Ner.Decree.Internal.DecreeChangeToken dcr = Pullenti.Ner.Decree.Internal.DecreeChangeToken.TryAttach(tt, ch, false, null, false, false, null, false);
                    if (dcr != null) 
                    {
                        if (dcr.IsNewlineAfter) 
                        {
                            if (tt == dcr.EndToken) 
                                tt = dcr.EndToken.Next;
                            else 
                                tt = dcr.EndToken;
                            continue;
                        }
                        if (dcr.EndToken.Next != null && dcr.EndToken.Next.IsChar(':') && dcr.EndToken.IsWhitespaceAfter) 
                        {
                            tt = dcr.EndToken.Next;
                            continue;
                        }
                        if (dcr.ActKind != DecreeChangeKind.Undefined) 
                            notAttachVal = true;
                    }
                    tt = ttt;
                    break;
                }
                if (tt == null) 
                    continue;
                t = tt;
                Pullenti.Ner.Token t1 = null;
                DecreeChangeReferent chh = null;
                bool toChangeRegime = false;
                DecreePartReferent realPart = null;
                for (; tt != null; tt = tt.Next) 
                {
                    chh = tt.GetReferent() as DecreeChangeReferent;
                    if (chh != null) 
                    {
                        if (tt.IsNewlineBefore) 
                            t1 = tt.Previous;
                        break;
                    }
                    if (realPart == null && (tt.GetReferent() is DecreePartReferent) && !tt.Morph.Case.IsInstrumental) 
                        realPart = tt.GetReferent() as DecreePartReferent;
                    if (tt.IsValue3("ВСТУПАТЬ", "В", "СИЛУ")) 
                        break;
                    if (!tt.IsNewlineBefore) 
                        continue;
                    t1 = tt.Previous;
                    if (tt.IsValue("НАИМЕНОВАНИЕ", null)) 
                        continue;
                    Pullenti.Ner.Instrument.Internal.InstrToken1 iii = Pullenti.Ner.Instrument.Internal.InstrToken1.Parse(tt, true, null, 0, null, false, 0, false, false);
                    if (iii != null) 
                    {
                        if (iii.Typ == Pullenti.Ner.Instrument.Internal.InstrToken1.Types.Editions) 
                            break;
                        if (iii.IsExpired) 
                            break;
                        if (tt.IsTableControlChar) 
                        {
                            t1 = (tt = iii.EndToken);
                            continue;
                        }
                    }
                    if (tt.IsValue("ИЗМЕНЕНИЯ", null)) 
                    {
                        if (tt.IsNewlineAfter) 
                            break;
                        if (t.Next != null && tt.Next.IsComma) 
                            break;
                    }
                    if (tt.IsValue("ПРИЛОЖЕНИЕ", null) || tt.IsValue("УТВЕРЖДЕНО", null)) 
                    {
                        if (tt == t0.Next && isPsevdoApp) 
                        {
                        }
                        else 
                            break;
                    }
                    if (tt.IsValue("ПЕРЕЧЕНЬ", null) && tt.IsNewlineAfter) 
                        break;
                    Pullenti.Ner.Token ttt = tt;
                    Pullenti.Ner.Core.ComplexNumToken num = Pullenti.Ner.Core.ComplexNumToken.TryParse(tt, null, false, false);
                    if (num != null) 
                        ttt = num.EndToken.Next;
                    if (ttt == null) 
                        break;
                    chh = ttt.GetReferent() as DecreeChangeReferent;
                    if (chh != null) 
                        break;
                    if (toChangeRegime) 
                        continue;
                    Pullenti.Ner.Decree.Internal.DecreeChangeToken dcr = Pullenti.Ner.Decree.Internal.DecreeChangeToken.TryAttach(tt, null, false, null, false, false, null, false);
                    if (dcr != null) 
                    {
                        if (dcr.IsNewlineAfter) 
                            break;
                        if (dcr.EndToken.Next != null && dcr.EndToken.Next.IsChar(':') && dcr.EndToken.IsWhitespaceAfter) 
                            break;
                        if (dcr.Typ == Pullenti.Ner.Decree.Internal.DecreeChangeTokenTyp.StartSingle) 
                        {
                            if (dcr.ActKind != DecreeChangeKind.Undefined) 
                            {
                                toChangeRegime = true;
                                continue;
                            }
                            if (dcr.ChangeVal != null) 
                            {
                                if ((dcr.BeginChar + 5) < dcr.ChangeVal.BeginChar) 
                                    toChangeRegime = true;
                                tt = dcr.EndToken;
                                continue;
                            }
                            if (dcr.RealPart == null) 
                                break;
                            realPart = dcr.RealPart;
                            if (dcr.EndToken.Next != null) 
                            {
                                if (dcr.EndToken.Next.IsValue2("УТРАТИТЬ", "СИЛУ")) 
                                    break;
                            }
                        }
                    }
                }
                if (t1 == null || chh == null) 
                    continue;
                if (chh.Kind == DecreeChangeKind.Container) 
                {
                    if (chh.Children.Count == 0) 
                        continue;
                }
                if (t1.IsCharOf(".;")) 
                    t1 = t1.Previous;
                if ((t1.EndChar - t.BeginChar) < 20) 
                    continue;
                bool not = false;
                for (Pullenti.Ner.Token tt2 = t1; tt2 != null && tt2.BeginChar >= t.BeginChar; tt2 = tt2.Previous) 
                {
                    if (tt2.GetReferent() is DecreePartReferent) 
                    {
                        not = true;
                        break;
                    }
                    else if ((tt2 is Pullenti.Ner.NumberToken) || tt2.LengthChar == 1) 
                    {
                    }
                    else 
                        break;
                }
                if (not) 
                    continue;
                Pullenti.Ner.Token t00 = t;
                for (; t00 != null; t00 = t00.Previous) 
                {
                    if (t00.IsNewlineBefore) 
                        break;
                }
                bool ok = false;
                for (Pullenti.Ner.Token ttt = t00; ttt != null && ttt.EndChar <= t1.EndChar; ttt = ttt.Next) 
                {
                    Pullenti.Ner.Token ttt2 = Pullenti.Ner.Core.MiscHelper.CheckImage(ttt);
                    if (ttt2 != null) 
                    {
                        ttt = ttt2;
                        continue;
                    }
                    if (ttt.IsValue("ДОКУМЕНТ", null)) 
                        break;
                    if ((ttt.LengthChar < 4) || (ttt is Pullenti.Ner.NumberToken)) 
                        continue;
                    ok = true;
                    break;
                }
                if (!ok) 
                    continue;
                if (((!toChangeRegime && !notAttachVal && Pullenti.Ner.Core.BracketHelper.IsBracket(t1, false)) && ((ch.Kind == DecreeChangeKind.Append || ch.Kind == DecreeChangeKind.New)) && (((t0.EndChar + 7) >= t00.BeginChar || t0.EndToken.Next == t || t0.EndToken.Next.Next == t))) && (t0.EndToken.GetReferent() is DecreeChangeValueReferent)) 
                {
                    DecreeChangeValueReferent val = t0.EndToken.GetReferent() as DecreeChangeValueReferent;
                    val.EndChar = t1.BeginChar - 1;
                    if (!Pullenti.Ner.Core.BracketHelper.IsBracket(t1, true)) 
                        val.EndChar = t1.BeginChar;
                    val.Occurrence[0].EndChar = t1.EndChar;
                    val.Value = kit.Sofa.Text.Substring(val.BeginChar, (val.EndChar + 1) - val.BeginChar);
                    ch.Occurrence[0].EndChar = t1.EndChar;
                    Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(ch, t0, t1);
                    t.Kit.EmbedToken(rt);
                    t = rt;
                }
                else if (own != null) 
                {
                    DecreeChangeReferent ch1 = new DecreeChangeReferent();
                    ch1.Kind = DecreeChangeKind.Error;
                    if (realPart != null) 
                        ch1.AddSlot(DecreeChangeReferent.ATTR_OWNER, realPart, false, 0);
                    own.AddSlot(DecreeChangeReferent.ATTR_CHILD, ch1, false, 0);
                    Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(ad.RegisterReferent(ch1), t, t1);
                    t.Kit.EmbedToken(rt);
                    t = rt;
                }
            }
            Pullenti.Ner.Token lastChangeToken = null;
            List<DecreeChangeValueReferent> extVals = new List<DecreeChangeValueReferent>();
            for (Pullenti.Ner.Token t = kit.FirstToken; t != null; t = t.Next) 
            {
                DecreeChangeReferent ch = t.GetReferent() as DecreeChangeReferent;
                if (ch == null) 
                    continue;
                lastChangeToken = t;
                foreach (Pullenti.Ner.Slot s in ch.Slots) 
                {
                    if (s.Value is DecreeChangeValueReferent) 
                    {
                        DecreeChangeValueReferent v = s.Value as DecreeChangeValueReferent;
                        if (v.Kind == DecreeChangeValueKind.ExtAppendix) 
                            extVals.Add(v);
                    }
                }
            }
            if (extVals.Count > 0) 
            {
                Pullenti.Ner.Token t = lastChangeToken.Next;
                for (; t != null; t = t.Next) 
                {
                    if (!t.IsNewlineBefore) 
                        continue;
                    if (extVals.Count == 0) 
                        break;
                    if ((t is Pullenti.Ner.TextToken) && (t as Pullenti.Ner.TextToken).Term == "ПРИЛОЖЕНИЕ" && !t.Chars.IsAllLower) 
                    {
                    }
                    else 
                        continue;
                    string val = extVals[0].Value ?? "";
                    Pullenti.Ner.Token tt = Pullenti.Ner.Core.MiscHelper.CheckNumberPrefix(t.Next) ?? t.Next;
                    if (tt == null) 
                        break;
                    if (tt is Pullenti.Ner.NumberToken) 
                    {
                        if ((tt as Pullenti.Ner.NumberToken).Value != val) 
                            continue;
                        tt = tt.Next;
                    }
                    else if (!string.IsNullOrEmpty(val)) 
                        continue;
                    for (; tt != null; tt = tt.Next) 
                    {
                        if (Pullenti.Ner.Core.BracketHelper.IsBracket(tt, true)) 
                            break;
                        if (tt.IsValue("ПРИЛОЖЕНИЕ", null)) 
                            break;
                        Pullenti.Ner.Instrument.Internal.InstrToken1 line = Pullenti.Ner.Instrument.Internal.InstrToken1.Parse(tt, true, null, 0, null, false, 0, false, false);
                        if (line == null) 
                            break;
                        if (tt.Chars.IsAllLower || tt.IsValue("К", null) || tt.IsValue("ОТ", null)) 
                        {
                            tt = line.EndToken;
                            continue;
                        }
                        break;
                    }
                    if (tt == null || !tt.IsNewlineBefore) 
                        continue;
                    Pullenti.Ner.Token tt0 = tt;
                    Pullenti.Ner.Token tt1 = tt;
                    for (tt = tt.Next; tt != null; tt = tt.Next) 
                    {
                        if (!tt.IsNewlineBefore) 
                        {
                            tt1 = tt;
                            continue;
                        }
                        if ((tt is Pullenti.Ner.TextToken) && (tt as Pullenti.Ner.TextToken).Term == "ПРИЛОЖЕНИЕ" && !tt.Chars.IsAllLower) 
                        {
                        }
                        else 
                        {
                            tt1 = tt;
                            continue;
                        }
                        tt = Pullenti.Ner.Core.MiscHelper.CheckNumberPrefix(tt.Next) ?? tt.Next;
                        if (tt == null) 
                            break;
                        bool stop = true;
                        foreach (string ni in extVals[0].NewItems) 
                        {
                            if (ni.StartsWith("APPENDIX ")) 
                            {
                                string val2 = ni.Substring(9);
                                if ((tt is Pullenti.Ner.NumberToken) && (tt as Pullenti.Ner.NumberToken).Value == val2) 
                                    stop = false;
                            }
                        }
                        if (stop) 
                            break;
                    }
                    if (Pullenti.Ner.Core.BracketHelper.IsBracket(tt0, true)) 
                    {
                        tt0 = tt0.Next;
                        if (Pullenti.Ner.Core.BracketHelper.IsBracket(tt1, true)) 
                            tt1 = tt1.Previous;
                        else if (tt1.IsCharOf(";.") && Pullenti.Ner.Core.BracketHelper.IsBracket(tt1.Previous, true)) 
                            tt1 = tt1.Previous.Previous;
                    }
                    if (tt0.EndChar >= tt1.BeginChar) 
                        continue;
                    extVals[0].Kind = DecreeChangeValueKind.Text;
                    extVals[0].BeginChar = tt0.BeginChar;
                    extVals[0].EndChar = tt1.EndChar;
                    Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(extVals[0], tt0, tt1);
                    extVals[0].Value = rt.GetSourceText();
                    extVals[0].Occurrence.Clear();
                    kit.EmbedToken(rt);
                    t = rt;
                    extVals.RemoveAt(0);
                }
            }
        }
        internal static Pullenti.Ner.Token CanBeStartOfAttachApproved(Pullenti.Ner.Token t)
        {
            if (!(t is Pullenti.Ner.TextToken)) 
                return null;
            if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t, true, false)) 
            {
                if (t.Next == null) 
                    return null;
                if (t.Next.Chars.IsAllLower || !t.Next.Chars.IsLetter) 
                    return null;
                return t;
            }
            if (t is Pullenti.Ner.NumberToken) 
                return null;
            Pullenti.Ner.Token tt = Pullenti.Ner.Decree.Internal.DecreeToken.IsKeyword(t, false);
            bool doubt = false;
            if (tt == null && ((t.IsValue("СОСТАВ", null) || t.IsValue("СРОК", null)))) 
            {
                if (t.Previous != null && t.Previous.IsValue("В", null)) 
                {
                    tt = t;
                    doubt = true;
                }
            }
            Pullenti.Ner.Token keyTok = tt;
            if (tt != null) 
            {
                if (t.IsValue("НАСТОЯЩИЙ", null)) 
                    return null;
                if (t.Chars.IsAllLower && ((t.IsValue("УСЛОВИЕ", null) || t.IsValue("ПОЛОЖЕНИЕ", null)))) 
                    return null;
                if (t.Previous != null && t.Previous.IsValue("НАСТОЯЩИЙ", null)) 
                    return null;
                Pullenti.Ner.Token tt0 = Pullenti.Ner.Decree.Internal.DecreeToken.IsKeyword(t.Previous, false);
                if (tt0 == tt) 
                    return null;
                if (t.Previous != null && Pullenti.Ner.Decree.Internal.DecreeToken.IsKeyword(t.Previous.Previous, false) == tt) 
                    return null;
                if (t.IsValue("ПЕРЕЧЕНЬ", null) && t.IsNewlineBefore && t.Morph.Case.IsNominative) 
                    return null;
                Pullenti.Morph.MorphClass mc0 = t.GetMorphClassInDictionary();
                if (mc0.IsPronoun) 
                    return null;
                tt = t;
            }
            bool changes = false;
            if (tt == null || tt.Next == null) 
            {
                if ((t.IsValue("ИЗМЕНЕНИЕ", null) && (((t.Previous is Pullenti.Ner.NumberToken) || t.IsNewlineBefore)) && t.Next != null) && t.Next.IsComma) 
                {
                    Pullenti.Ner.Token tt1 = t.Next.Next;
                    if (tt1.IsValue3("КОТОРЫЙ", "ВНОСИТЬСЯ", "В")) 
                    {
                        if (!t.IsNewlineBefore || t.Chars.IsAllLower) 
                            return t;
                        tt = tt1.Next.Next.Next;
                        changes = true;
                    }
                    else if (tt1.IsValue2("ВНОСИМЫЙ", "В")) 
                    {
                        tt1 = tt1.Next.Next;
                        changes = true;
                    }
                    else 
                        return null;
                }
                else 
                    return null;
            }
            else 
            {
                int cou2 = 7;
                if (doubt) 
                    cou2 = 2;
                for (Pullenti.Ner.Token tt2 = t.Previous; tt2 != null && cou2 > 0; tt2 = tt2.Previous,cou2--) 
                {
                    if ((tt2.IsValue("ВНЕСТИ", null) || tt2.IsValue("ВНОСИТЬ", null) || tt2.IsValue("ИЗМЕНЕНИЕ", null)) || tt2.IsValue("ДОПОЛНИТЬ", null) || tt2.IsValue("ИЗЛОЖИТЬ", null)) 
                    {
                        changes = true;
                        break;
                    }
                    if ((((tt2 is Pullenti.Ner.NumberToken) || tt2.LengthChar == 1)) && tt2 != t.Previous) 
                        cou2++;
                    Pullenti.Ner.Decree.Internal.PartToken pp = Pullenti.Ner.Decree.Internal.PartToken.TryAttach(tt2, null, false, false);
                    if (pp != null && pp.EndToken.Next == t) 
                    {
                        changes = true;
                        break;
                    }
                }
                for (Pullenti.Ner.Token tt2 = t.Next; tt2 != null; tt2 = tt2.Next) 
                {
                    if (Pullenti.Ner.Decree.Internal.DecreeToken.IsKeyword(t, false) != null) 
                        break;
                    if ((tt2.IsComma && tt2.Next != null && tt2.Next.IsValue("УТВЕРЖДЕННЫЙ", null)) && keyTok != null) 
                        return tt;
                    if (tt2.IsNewlineBefore) 
                        break;
                }
            }
            if (tt == null) 
                return null;
            if (!changes) 
            {
                if (!(t is Pullenti.Ner.TextToken) || !t.Chars.IsCyrillicLetter || keyTok == null) 
                    return null;
                if (t.Chars.IsAllLower) 
                {
                    if (t.Previous != null) 
                    {
                        if (t.Previous.IsValue("В", null) || t.Previous.IsValue("К", null)) 
                        {
                            if (keyTok.Next != null && keyTok.Next.IsComma) 
                                return null;
                            return tt;
                        }
                    }
                    return null;
                }
            }
            return tt;
        }
        static Pullenti.Ner.Token _checkChangesTo(Pullenti.Ner.Token t)
        {
            if (t != null && ((t.IsValue("ПЕРЕЧЕНЬ", null) || t.IsValue("СПИСОК", null)))) 
                t = t.Next;
            if (t == null || !t.IsValue("ИЗМЕНЕНИЕ", null)) 
                return null;
            for (t = t.Next; t != null; t = t.Next) 
            {
                if ((t.IsComma || t.IsValue("КОТОРЫЙ", null) || t.IsValue("ВНОСИТЬСЯ", null)) || t.IsValue("ВНОСИМЫЙ", null) || t.IsValue("В", null)) 
                    continue;
                if (t.GetReferent() is DecreeReferent) 
                    return t;
                if (t is Pullenti.Ner.ReferentToken) 
                    continue;
                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.ParsePreposition, 0, null);
                if (npt != null) 
                {
                    if (npt.EndToken.IsValue("РЕАЛИЗАЦИЯ", null) || npt.EndToken.IsValue("АКТ", null) || npt.EndToken.IsValue("ВОПРОС", null)) 
                    {
                        t = npt.EndToken;
                        continue;
                    }
                }
                if (t.Previous.IsValue("В", null)) 
                    return t;
                break;
            }
            return null;
        }
        internal static Pullenti.Ner.ReferentToken TryAttachApproved(Pullenti.Ner.Token t, Pullenti.Ner.Core.AnalyzerData ad, Pullenti.Ner.Core.TerminCollection aliases = null, Pullenti.Ner.Core.TerminCollection attachednames = null, bool partRegime = false)
        {
            if (t == null) 
                return null;
            Pullenti.Ner.Core.BracketSequenceToken br = null;
            if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t, true, false)) 
                br = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 100);
            else if ((t.Previous is Pullenti.Ner.TextToken) && t.Previous.LengthChar == 1 && Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t.Previous, true, false)) 
                br = Pullenti.Ner.Core.BracketHelper.TryParse(t.Previous, Pullenti.Ner.Core.BracketParseAttr.No, 100);
            if (br != null) 
            {
                if (br.LengthChar < 20) 
                    return null;
                Pullenti.Ner.ReferentToken rt0 = _tryAttachApproved(br.EndToken.Next, ad, false, true, aliases);
                if (rt0 != null) 
                {
                    DecreeReferent dr = rt0.Referent as DecreeReferent;
                    rt0.BeginToken = br.BeginToken;
                    string nam = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(br, Pullenti.Ner.Core.GetTextAttr.RestoreRegister);
                    if (dr.Typ == null) 
                    {
                        Pullenti.Ner.Decree.Internal.DecreeToken dt = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(br.BeginToken.Next, null, false);
                        if (dt != null && dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) 
                        {
                            dr.AddTyp(dt);
                            if (dt.EndToken.Next != null && dt.EndToken.Next.IsValue("О", null)) 
                            {
                                nam = Pullenti.Ner.Core.MiscHelper.GetTextValue(dt.EndToken.Next, br.EndToken, Pullenti.Ner.Core.GetTextAttr.RestoreRegister);
                                br.BeginToken = dt.EndToken.Next;
                            }
                        }
                    }
                    if (nam != null) 
                        dr.AddNameStr(nam, br);
                    return rt0;
                }
                if (br.BeginToken == t.Previous) 
                    return null;
            }
            if ((t is Pullenti.Ner.TextToken) && (t as Pullenti.Ner.TextToken).Term == "ПЕРЕЧЕНЬ") 
            {
            }
            Pullenti.Ner.Core.TerminToken atok = (attachednames == null ? null : attachednames.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No));
            if (atok != null) 
            {
                if (t.IsNewlineBefore && atok.IsNewlineAfter && !t.Chars.IsAllLower) 
                    return null;
                if (t.IsNewlineBefore && t.IsValue("ИЗМЕНЕНИЕ", null)) 
                    return null;
            }
            Pullenti.Ner.Token t0 = t;
            Pullenti.Ner.Token tt = CanBeStartOfAttachApproved(t);
            if (tt == null) 
            {
                if (atok == null) 
                    return null;
                tt = t;
            }
            else 
                t = tt;
            Pullenti.Ner.Token keyTok = Pullenti.Ner.Decree.Internal.DecreeToken.IsKeyword(t, false);
            bool doubt = false;
            if (keyTok == null && ((t.IsValue("СОСТАВ", null) || t.IsValue("СРОК", null) || t.IsValue("ФОРМА", null)))) 
            {
                keyTok = t;
                doubt = true;
            }
            if (t.IsValue("ГОСУДАРСТВЕННЫЙ", null)) 
            {
            }
            if ((t.Next is Pullenti.Ner.TextToken) && (t.Next as Pullenti.Ner.TextToken).IsPureVerb) 
                return null;
            Pullenti.Ner.Token chTok = _checkChangesTo(t);
            if (chTok != null && t.IsValue("ИЗМЕНЕНИЕ", null)) 
            {
                if (t.Previous != null && t.Previous.IsValue("ПЕРЕЧЕНЬ", null)) 
                    return null;
            }
            if (chTok != null && (chTok.GetReferent() is DecreeReferent)) 
            {
                DecreeReferent dr = chTok.GetReferent() as DecreeReferent;
                if (dr.Owner != null) 
                    chTok.Kit.DebedToken(chTok);
            }
            int cou = 0;
            Pullenti.Ner.MetaToken alias = null;
            Pullenti.Ner.Token aliasT0 = null;
            bool hasNl = false;
            bool hasOrg = false;
            bool hasVerb = false;
            Pullenti.Ner.Token lastTok = null;
            bool keywordAfter = chTok != null;
            bool hasWich = false;
            bool hasRegisterAfter = false;
            bool accord = false;
            Pullenti.Ner.Token changeStartTok = null;
            if ((t.Previous != null && t.Previous.IsValue("В", null) && t.Previous.Previous != null) && t.Previous.Previous.IsValue("ВНОСИТЬСЯ", null)) 
                keywordAfter = true;
            else if ((t.Previous != null && t.Previous.IsValue("ТЕКСТ", null) && t.Previous.Previous != null) && t.Previous.Previous.IsValue("В", null)) 
                keywordAfter = true;
            for (tt = tt.Next; tt != null; tt = tt.Next) 
            {
                if (atok != null) 
                {
                    if (tt.EndChar <= atok.EndChar) 
                        continue;
                }
                if (tt.IsValue("УСТАНОВЛЕННЫЙ", null)) 
                {
                }
                if ((++cou) > 100) 
                    break;
                lastTok = tt;
                if (tt.IsCharOf(";.") || Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(tt, false, null, false) || tt.IsHiphen) 
                {
                    if (keyTok != null && keyTok == tt.Previous && !keyTok.IsValue("СОСТАВ", null)) 
                    {
                        if (tt.Next == null) 
                            return null;
                        Pullenti.Morph.MorphClass mc0 = tt.Next.GetMorphClassInDictionary();
                        if (mc0.IsAdjective) 
                        {
                        }
                        else 
                            return null;
                    }
                }
                if (tt.IsTableControlChar) 
                    break;
                if (tt.IsHiphen && tt.Previous == keyTok) 
                    break;
                if (tt.IsComma && tt.Previous == keyTok && tt.Next != null) 
                {
                    if (tt.Next.IsValue("КАСАЮЩИЙСЯ", null)) 
                        break;
                    if (tt.Next.IsValue("КОТОРЫЙ", null) || tt.Next.GetMorphClassInDictionary().IsVerb) 
                    {
                    }
                    else 
                    {
                        bool ok = false;
                        for (Pullenti.Ner.Token ttt = tt.Next; ttt != null; ttt = ttt.Next) 
                        {
                            Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(ttt, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                            if (npt == null) 
                                break;
                            if (ttt.Previous.IsAnd) 
                            {
                                ok = true;
                                tt = npt.EndToken;
                                break;
                            }
                            ttt = npt.EndToken.Next;
                            if (ttt == null || !ttt.IsCommaAnd) 
                                break;
                        }
                        if (ok) 
                            continue;
                    }
                }
                if (tt.IsValue3("В", "СООТВЕТСТВИИ", "С")) 
                    accord = true;
                if ((tt is Pullenti.Ner.TextToken) && !tt.Chars.IsLetter && tt.LengthChar == 1) 
                {
                    if (!Pullenti.Ner.Core.BracketHelper.IsBracket(tt, false) && !tt.IsCharOf(",.:;№\\/") && !tt.IsHiphen) 
                        break;
                }
                if ((!partRegime && tt.IsComma && tt.Next != null) && tt.Next.IsAnd && Pullenti.Ner.Decree.Internal.DecreeToken.IsKeyword(tt.Next.Next, false) != null) 
                {
                    Pullenti.Ner.ReferentToken next = TryAttachApproved(tt.Next.Next, ad, aliases, attachednames, false);
                    if (next != null && next.Referent.GetSlotValue(DecreeReferent.ATTR_OWNER) != null) 
                    {
                        string nam = Pullenti.Ner.Core.MiscHelper.GetTextValue(t, tt.Previous, Pullenti.Ner.Core.GetTextAttr.FirstNounGroupToNominative | Pullenti.Ner.Core.GetTextAttr.RestoreRegister);
                        if (nam == null || (nam.Length < 20) || nam.Length > 1000) 
                            return null;
                        Pullenti.Ner.Decree.Internal.DecreeToken dt = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(t, null, false);
                        DecreeReferent dec0 = new DecreeReferent();
                        if (dt != null && dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) 
                            dec0.AddTyp(dt);
                        dec0.AddNameStr(nam, new Pullenti.Ner.MetaToken(t, tt.Previous));
                        dec0.Owner = (next.Referent as DecreeReferent).Owner;
                        return new Pullenti.Ner.ReferentToken(dec0, t, tt) { Tag = alias };
                    }
                }
                Pullenti.Ner.Decree.Internal.PartToken appNum = null;
                Pullenti.Ner.Token appBefore = null;
                DecreeReferent appOwn = null;
                bool isBr = false;
                if ((tt.IsValue2("ЯВЛЯЮЩИЙСЯ", "ПРИЛОЖЕНИЕМ") || tt.IsValue2("ПРЕДУСМОТРЕННЫЙ", "ПРИЛОЖЕНИЕМ") || tt.IsValue3("ПРИВЕДЕННЫЙ", "В", "ПРИЛОЖЕНИИ")) || ((tt.IsChar('(') && tt.Next != null && tt.Next.IsValue("ПРИЛОЖЕНИЕ", null)))) 
                {
                    appBefore = tt.Previous;
                    keywordAfter = true;
                    if (tt.IsValue("ПРИВЕДЕННЫЙ", null)) 
                        tt = tt.Next;
                    Pullenti.Ner.Decree.Internal.PartToken pp = Pullenti.Ner.Decree.Internal.PartToken.TryAttach(tt.Next, null, false, false);
                    if (pp == null) 
                        pp = Pullenti.Ner.Decree.Internal.PartToken.TryAttach(tt.Next, null, false, true);
                    if (pp == null) 
                        break;
                    Pullenti.Ner.Token tt2 = pp.EndToken.Next;
                    if (tt2 != null && tt2.IsValue("К", null)) 
                        tt2 = tt2.Next;
                    isBr = tt.IsChar('(');
                    if (tt2 != null && (tt2.GetReferent() is DecreeReferent)) 
                    {
                        List<Pullenti.Ner.Decree.Internal.PartToken> pps = Pullenti.Ner.Decree.Internal.PartToken.TryAttachList(t, false, 40);
                        if (pps != null && pps.Count > 0) 
                            break;
                        appNum = pp;
                        tt = tt2;
                    }
                    else if (tt2 != null && tt2.IsValue("УКАЗАННЫЙ", null)) 
                    {
                        Pullenti.Ner.Token tt3 = Pullenti.Ner.Decree.Internal.DecreeToken.IsKeyword(tt2.Next, true);
                        if (tt3 == null) 
                            break;
                        string norm = tt3.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false);
                        int cou4 = 500;
                        for (Pullenti.Ner.Token tt4 = t.Previous; tt4 != null && cou4 > 0; tt4 = tt4.Previous,cou4--) 
                        {
                            DecreeReferent dd = tt4.GetReferent() as DecreeReferent;
                            if (dd == null) 
                                continue;
                            if (dd.CheckTypename(norm)) 
                            {
                                appOwn = dd;
                                tt = tt3;
                                appNum = pp;
                                break;
                            }
                        }
                        if (appOwn == null) 
                            break;
                    }
                    else if (partRegime) 
                    {
                        appNum = pp;
                        tt = tt2;
                    }
                    else 
                        break;
                }
                if (tt.IsValue("ПРЕДУСМОТРЕННЫЙ", null)) 
                {
                    List<Pullenti.Ner.Decree.Internal.PartToken> pps = Pullenti.Ner.Decree.Internal.PartToken.TryAttachList(tt.Next, false, 40);
                    if (pps != null && pps.Count > 0) 
                    {
                        if (pps[pps.Count - 1].Decree != null) 
                        {
                            tt = pps[pps.Count - 1].EndToken;
                            continue;
                        }
                        Pullenti.Ner.Token tt2 = pps[pps.Count - 1].EndToken.Next;
                        if (tt2 != null && (tt2.GetReferent() is DecreeReferent)) 
                        {
                            tt = tt2;
                            continue;
                        }
                        if (tt2 != null) 
                        {
                            Pullenti.Ner.Decree.Internal.DecreeToken dd = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(tt2, null, false);
                            if (dd != null && dd.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) 
                            {
                                tt = dd.EndToken;
                                continue;
                            }
                        }
                    }
                }
                if (tt.IsComma && tt.Next != null && tt.Next.IsValue("УСТАНОВЛЕННЫЙ", null)) 
                    tt = tt.Next;
                if (tt.IsValue("УСТАНОВЛЕННЫЙ", null) && tt.Next != null) 
                {
                    if (Pullenti.Ner.Decree.Internal.DecreeToken.IsKeyword(tt.Next, false) != null || (tt.Next.GetReferent() is DecreeReferent)) 
                    {
                        Pullenti.Ner.Token ttt2;
                        for (ttt2 = tt.Next; ttt2 != null; ttt2 = ttt2.Next) 
                        {
                            if (ttt2.IsCommaAnd) 
                                continue;
                            if (ttt2.GetReferent() is DecreeReferent) 
                                continue;
                            Pullenti.Ner.Decree.Internal.DecreeToken ddd = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(ttt2, null, false);
                            if (ddd == null) 
                                break;
                            if (ddd.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ && ttt2 != tt.Next) 
                                break;
                            ttt2 = ddd.EndToken;
                        }
                        if (ttt2 != null && ttt2.IsValue("УТВЕРЖДЕННЫЙ", null)) 
                            tt = ttt2;
                    }
                }
                if (tt.IsValue("СЛЕДУЮЩИЙ", null)) 
                {
                    Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                    if (npt != null && npt.EndToken.Next != null && npt.EndToken.Next.IsCharOf(":;")) 
                    {
                        keywordAfter = true;
                        if (keyTok != null && keyTok.IsValue("УСТАВ", null)) 
                            break;
                        if (npt.EndToken.IsValue("ИЗМЕНЕНИЕ", null)) 
                            break;
                        if (changeStartTok != null) 
                            break;
                        return null;
                    }
                }
                if (tt.IsValue("КОТОРЫЙ", null)) 
                    hasWich = true;
                if (tt.IsNewlineBefore) 
                {
                    if (atok != null) 
                    {
                        if ((tt is Pullenti.Ner.TextToken) && tt.Chars.IsAllLower) 
                        {
                        }
                        else 
                            break;
                    }
                    if (tt.NewlinesBeforeCount > 2) 
                        break;
                    if (Pullenti.Ner.Core.MiscHelper.CheckImage(tt) != null) 
                        break;
                    if (Pullenti.Ner.Decree.Internal.DecreeChangeToken._canBeChangeToken(tt, 0, false, false)) 
                        break;
                    if (tt is Pullenti.Ner.NumberToken) 
                        break;
                    if (tt.Next != null && tt.Next.IsCharOf(".)")) 
                        break;
                    if (tt.Previous != null && tt.Previous.IsCharOf(";.")) 
                        break;
                    if ((tt is Pullenti.Ner.TextToken) && (tt as Pullenti.Ner.TextToken).Term == "ИСТОЧНИК") 
                        break;
                    if (tt.IsValue("ГАРАНТ", null) || tt.IsValue("КОНСУЛЬТАНТПЛЮС", null)) 
                        break;
                    if (tt.IsChar('(')) 
                    {
                    }
                    else if (!tt.Chars.IsAllUpper && tt.Previous.Chars.IsAllUpper) 
                        break;
                    hasNl = true;
                    Pullenti.Ner.Decree.Internal.PartToken pp = Pullenti.Ner.Decree.Internal.PartToken.TryAttach(tt, null, false, false);
                    if (pp != null && !tt.Chars.IsAllLower) 
                        return null;
                    if (tt.GetReferent() is DecreeReferent) 
                        break;
                    if (tt.NewlinesBeforeCount > 1) 
                    {
                        if (!tt.Chars.IsAllLower) 
                            break;
                    }
                }
                if ((((tt is Pullenti.Ner.NumberToken) && (tt as Pullenti.Ner.NumberToken).Value == "1")) || tt.IsValue("ДРУГОЙ", null) || tt.IsValue("ИНОЙ", null)) 
                {
                    if (tt.Next != null && tt.Next.IsValue("СТОРОНА", null)) 
                        return null;
                }
                if (tt.IsCharOf(":;")) 
                {
                    if (hasNl) 
                        break;
                    if (keyTok != null && tt == keyTok.Next) 
                    {
                        if (changeStartTok != null) 
                            break;
                        return null;
                    }
                    if (tt.IsNewlineAfter) 
                    {
                        if (t.Previous != null && t.Previous.IsValue("В", null)) 
                            break;
                    }
                }
                if (tt.IsValue2("В", "РЕДАКЦИИ")) 
                {
                    tt = tt.Next;
                    continue;
                }
                if (tt.IsChar('(') && tt.Next != null) 
                {
                    if (tt.Previous == keyTok) 
                        break;
                    DecreeReferent dr1 = tt.Next.GetReferent() as DecreeReferent;
                    if (dr1 != null) 
                    {
                        if (dr1.Kind == DecreeKind.Publisher) 
                        {
                            hasRegisterAfter = true;
                            break;
                        }
                    }
                    DecreePartReferent pr1 = tt.Next.GetReferent() as DecreePartReferent;
                    if (pr1 != null && pr1.Owner != null && pr1.Owner.Kind == DecreeKind.Publisher) 
                    {
                        hasRegisterAfter = true;
                        break;
                    }
                    Pullenti.Ner.Token tt0 = tt;
                    Pullenti.Ner.MetaToken mt = (partRegime ? null : _checkAliasAfter(tt));
                    if (mt != null) 
                    {
                        aliasT0 = tt;
                        alias = mt;
                        tt = mt.EndToken;
                        if (tt.Next != null && tt.Next.IsCommaAnd && (t.EndChar < tt0.Previous.BeginChar)) 
                        {
                            Pullenti.Ner.ReferentToken next = TryAttachApproved(tt.Next.Next, ad, aliases, null, false);
                            if (next != null) 
                            {
                                string nam = Pullenti.Ner.Core.MiscHelper.GetTextValue(t, tt0.Previous, Pullenti.Ner.Core.GetTextAttr.FirstNounGroupToNominative | Pullenti.Ner.Core.GetTextAttr.RestoreRegister);
                                if (nam == null || (nam.Length < 20) || nam.Length > 1000) 
                                    return null;
                                Pullenti.Ner.Decree.Internal.DecreeToken dt = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(t, null, false);
                                DecreeReferent dec0 = new DecreeReferent();
                                if (dt != null && dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) 
                                    dec0.AddTyp(dt);
                                dec0.AddNameStr(nam, new Pullenti.Ner.MetaToken(t, tt0.Previous));
                                dec0.Owner = (next.Referent as DecreeReferent).Owner;
                                return new Pullenti.Ner.ReferentToken(dec0, t, tt) { Tag = alias };
                            }
                        }
                        if (atok != null) 
                            break;
                        continue;
                    }
                }
                Pullenti.Ner.Token tok2 = Pullenti.Ner.Decree.Internal.DecreeToken.IsKeyword(tt, false);
                if (tok2 != null) 
                {
                    if (t.Previous != null && t.Previous.IsValue("В", null) && !hasVerb) 
                    {
                    }
                    else if (!hasVerb) 
                    {
                    }
                    else if (!hasWich) 
                        break;
                }
                Pullenti.Ner.Referent r = tt.Previous.GetReferent();
                if (r != null && r.TypeName == "ORGANIZATION" && tt == t.Next) 
                    hasOrg = true;
                if ((!partRegime && (tt.GetReferent() is DecreeReferent) && (tt.GetReferent() as DecreeReferent).Owner != null) && keyTok != null) 
                {
                    DecreeReferent dec0 = tt.GetReferent() as DecreeReferent;
                    List<Pullenti.Ner.Decree.Internal.DecreeToken> dtt = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttachList((tt as Pullenti.Ner.ReferentToken).BeginToken, null, 10, false);
                    if (dtt != null && dtt.Count > 1) 
                    {
                        Pullenti.Ner.ReferentToken rt00 = _tryAttachApproved(dtt[dtt.Count - 1].EndToken.Next, ad, true, false, aliases);
                        if (rt00 != null && dec0.Owner.CanBeEquals((rt00.Referent as DecreeReferent).Owner, Pullenti.Ner.Core.ReferentsEqualType.WithinOneText) && rt00.EndChar == tt.EndChar) 
                        {
                            Pullenti.Ner.Token tt1 = tt.Kit.DebedToken(tt);
                            dec0.Owner = null;
                            tt = dtt[dtt.Count - 1].EndToken.Next;
                        }
                    }
                }
                Pullenti.Ner.ReferentToken rt0 = (partRegime ? null : _tryAttachApproved(tt, ad, true, false, aliases));
                if (rt0 == null && appNum != null) 
                {
                    rt0 = new Pullenti.Ner.ReferentToken(null, tt, tt);
                    DecreeReferent own = appOwn ?? (tt.GetReferent() as DecreeReferent);
                    if (own != null && own.Owner != null && !partRegime) 
                        own = own.Owner;
                    DecreeReferent ddd = new DecreeReferent() { Owner = own };
                    rt0.Referent = ddd;
                    ddd.AddSlot(DecreeReferent.ATTR_TYPE, "ПРИЛОЖЕНИЕ", false, 0);
                    if (appNum != null && appNum.Values.Count == 1) 
                        ddd.AddSlot(DecreeReferent.ATTR_NUMBER, appNum.Values[0].ToString(), false, 0);
                }
                if (rt0 != null) 
                {
                    if (tt.IsChar('(') && tt.Next.IsValue("ВВЕСТИ", null)) 
                        return null;
                    Pullenti.Ner.Token t1 = (appBefore == null ? tt.Previous : appBefore);
                    if (t1.IsComma) 
                        t1 = t1.Previous;
                    if (aliasT0 != null) 
                        t1 = aliasT0.Previous;
                    if (t1.IsValue("РЕДАКЦИЯ", null)) 
                        t1 = t1.Previous;
                    if (t1.IsValue("В", null)) 
                        t1 = t1.Previous;
                    if (t1.IsComma) 
                        t1 = t1.Previous;
                    Pullenti.Ner.Token t00 = t;
                    if (t == t1) 
                        return null;
                    string nam = Pullenti.Ner.Core.MiscHelper.GetTextValue(t, t1, Pullenti.Ner.Core.GetTextAttr.FirstNounGroupToNominative | Pullenti.Ner.Core.GetTextAttr.RestoreRegister);
                    if (nam == null) 
                        return null;
                    if (nam.Length > 1000) 
                        return null;
                    if (t.Next == t1 && (nam.Length < 20)) 
                        return null;
                    Pullenti.Ner.Decree.Internal.DecreeToken dt = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(t, null, false);
                    DecreeReferent dec = rt0.Referent as DecreeReferent;
                    if (dt != null && dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ && (rt0.Referent as DecreeReferent).Typ == null) 
                    {
                        dec.AddTyp(dt);
                        if (dt.EndToken.Next != null && dt.EndToken.Next.IsValue("О", "ПРО")) 
                        {
                            dec.AddNameStr(nam, new Pullenti.Ner.MetaToken(t00, t1));
                            t00 = dt.EndToken.Next;
                            nam = Pullenti.Ner.Core.MiscHelper.GetTextValue(t00, t1, Pullenti.Ner.Core.GetTextAttr.RestoreRegister);
                        }
                        else if (t.IsValue("КОНСТИТУЦИЯ", null)) 
                            nam = null;
                    }
                    if (nam != null) 
                        dec.AddNameStr(nam, (t00.BeginChar <= t1.BeginChar ? new Pullenti.Ner.MetaToken(t00, t1) : null));
                    if (dec.Slots.Count < 2) 
                        return null;
                    rt0.BeginToken = t;
                    rt0.Tag = alias;
                    if (dec.FindSlot(DecreeReferent.ATTR_SOURCE, null, true) == null) 
                    {
                        if (t.Previous != null && t.Previous.IsValue("В", null) && (t.Previous.Previous is Pullenti.Ner.ReferentToken)) 
                        {
                            if (t.Previous.Previous.GetReferent() is Pullenti.Ner.Org.OrganizationReferent) 
                            {
                                Pullenti.Ner.Slot ss = dec.AddSlot(DecreeReferent.ATTR_SOURCE, t.Previous.Previous.GetReferent(), false, 0);
                                ss.AddAnnotation(t.Previous.Previous as Pullenti.Ner.ReferentToken);
                            }
                        }
                    }
                    if (isBr && rt0.EndToken.Next != null && rt0.EndToken.Next.IsChar(')')) 
                        rt0.EndToken = rt0.EndToken.Next;
                    return rt0;
                }
                Pullenti.Ner.Core.BracketSequenceToken br1 = Pullenti.Ner.Core.BracketHelper.TryParse(tt, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                if (br1 != null) 
                {
                    tt = br1.EndToken;
                    continue;
                }
                if (tt.IsCharOf(".;")) 
                    break;
                if (tt.IsNewlineBefore && tt.Previous != null && tt.Previous.IsChar('.')) 
                    break;
                if (changeStartTok == null && !accord && Pullenti.Ner.Decree.Internal.DecreeChangeToken._canBeChangeToken(tt, 0, true, false)) 
                {
                    if (atok != null) 
                        break;
                    changeStartTok = tt;
                    if (tt.Previous != null && tt.Previous.IsValue("В", null)) 
                        changeStartTok = tt.Previous;
                }
                bool noVerb = false;
                if ((tt is Pullenti.Ner.TextToken) && (tt as Pullenti.Ner.TextToken).IsPureVerb && !hasWich) 
                {
                    hasVerb = true;
                    if (tt.IsValue2("ИЗЛОЖИТЬ", "В")) 
                        break;
                    if (tt.Previous.IsValue("КОТОРЫЙ", null)) 
                        noVerb = true;
                    else if (tt.Morph.ContainsAttr("инф.", null)) 
                        noVerb = true;
                    else if (tt.IsValue("ПРИМЕНЯТЬСЯ", null)) 
                        noVerb = true;
                    else 
                        break;
                }
                Pullenti.Morph.MorphClass mc = tt.GetMorphClassInDictionary();
                if (((mc.IsVerb && !mc.IsAdjective && !mc.IsPreposition) && !mc.IsNoun && !noVerb) && tt.Morph.Case.IsUndefined) 
                {
                    if ((tt is Pullenti.Ner.TextToken) && (tt as Pullenti.Ner.TextToken).Term.EndsWith("СЯ")) 
                    {
                    }
                    else 
                        hasVerb = true;
                }
                if (hasOrg) 
                    break;
                if (tt.IsComma && tt.Next != null && tt.Next.IsValue("УТВЕРЖДЕННЫЙ", null)) 
                    break;
                if (tt.Previous.IsComma && Pullenti.Ner.Decree.Internal.DecreeChangeToken._canBeChangeToken(tt, 0, true, false)) 
                    break;
            }
            if (partRegime) 
                return null;
            if (atok != null) 
            {
                DecreeReferent dd = new DecreeReferent();
                dd.AddNameStr(atok.Termin.CanonicText, atok);
                Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(dd, t0, (alias == null ? atok.EndToken : alias.EndToken)) { Tag = alias };
                return rt;
            }
            if (changeStartTok != null) 
                tt = changeStartTok;
            if (((tt != null || ((lastTok != null && (cou < 30))))) && ((keyTok != null || keywordAfter)) && ((!t.Chars.IsAllLower || ((t.Previous != null && t.Previous.IsValue("В", null)))))) 
            {
                if (t.Previous != null && t.Previous.IsValue("ТЕКСТ", null)) 
                {
                    if (t.Previous.Previous != null && t.Previous.Previous.IsValue("В", null)) 
                    {
                    }
                    else 
                        return null;
                }
                if (t.Chars.IsAllLower && !keywordAfter && !hasRegisterAfter) 
                    return null;
                Pullenti.Ner.Decree.Internal.PartToken pt = Pullenti.Ner.Decree.Internal.PartToken.TryAttach(t, null, false, false);
                if (pt != null && pt.Values.Count > 0) 
                    return null;
                if (aliases != null) 
                {
                    Pullenti.Ner.Core.TerminToken tok = aliases.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
                    if (tok != null) 
                        return new Pullenti.Ner.ReferentToken(tok.Termin.Tag as Pullenti.Ner.Referent, t, tok.EndToken);
                }
                cou = 500;
                if (atok != null) 
                    cou = 50000;
                if (tt == null) 
                    tt = lastTok.Previous;
                if (t.Previous == null || t.BeginChar >= tt.Previous.BeginChar) 
                    return null;
                if (((t.IsNewlineBefore || ((t.Previous != null && t.Previous.IsChar('.'))))) && tt.IsNewlineBefore) 
                    return null;
                Pullenti.Ner.Decree.Internal.DecreeToken ty = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(t, null, false);
                if (ty != null && ty.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) 
                {
                    if (t.Morph.Number == Pullenti.Morph.MorphNumber.Plural) 
                        return null;
                }
                string typ = (keyTok == null ? null : keyTok.GetNormalCaseText(Pullenti.Morph.MorphClass.Noun, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false));
                if (tt.Previous.IsCharOf(".;:,")) 
                    tt = tt.Previous;
                if (t.IsNewlineBefore) 
                {
                    if (t.IsValue("ОСНОВЫ", null)) 
                    {
                    }
                    if (tt.IsNewlineBefore) 
                        return null;
                }
                string nam = null;
                string nam2 = null;
                nam = Pullenti.Ner.Core.MiscHelper.GetTextValue(t, tt.Previous, Pullenti.Ner.Core.GetTextAttr.FirstNounGroupToNominative);
                bool ignore = false;
                for (Pullenti.Ner.Token ttt = t.Previous; ttt != null && cou > 0; ttt = ttt.Previous,cou--) 
                {
                    if (typ == null) 
                        break;
                    if (ttt is Pullenti.Ner.TextToken) 
                    {
                        if ((ttt as Pullenti.Ner.TextToken).Term == "ПРИКАЗЫВАЮ") 
                            ignore = true;
                    }
                    if (ttt.IsValue2("В", "СООТВЕТСТВИИ")) 
                        ignore = false;
                    if (ignore) 
                        continue;
                    DecreeReferent dr = ttt.GetReferent() as DecreeReferent;
                    if (dr == null) 
                        continue;
                    if (dr.Kind == DecreeKind.Publisher) 
                        continue;
                    if (dr.Kind == DecreeKind.Kodex || dr.Typ == "КОНСТИТУЦИЯ") 
                        continue;
                    if (!dr.CheckTypename(typ)) 
                    {
                        bool ok = hasRegisterAfter;
                        if ((lastTok != null && lastTok.IsChar(':') && lastTok.IsNewlineAfter) && !doubt) 
                            ok = true;
                        if (ttt.Next.IsValue("УТВЕРДИТЬ", null) && ((t.BeginChar - ttt.Next.EndChar) < 30)) 
                            ok = true;
                        if (ok) 
                        {
                            DecreeReferent dd = new DecreeReferent();
                            dd.AddNameStr(nam2 ?? nam, new Pullenti.Ner.MetaToken(t, tt.Previous));
                            if (dr.Owner != null) 
                                dd.Owner = dr.Owner;
                            else if (dr.Typ != null) 
                                dd.Owner = dr;
                            return new Pullenti.Ner.ReferentToken(dd, t, tt.Previous);
                        }
                        continue;
                    }
                    if (dr.FindSlot(DecreeReferent.ATTR_NAME, nam, true) != null) 
                        return new Pullenti.Ner.ReferentToken(dr, t, tt.Previous);
                    if (nam2 != null && dr.FindSlot(DecreeReferent.ATTR_NAME, nam2, true) != null) 
                        return new Pullenti.Ner.ReferentToken(dr, t, tt.Previous);
                    string nnn = nam;
                    for (int k = 1; k < 5; k++) 
                    {
                        int ii = nnn.LastIndexOf(',');
                        if (ii < 0) 
                            break;
                        nnn = nnn.Substring(0, ii).Trim();
                        if (dr.FindSlot(DecreeReferent.ATTR_NAME, nnn, true) == null) 
                            continue;
                        for (Pullenti.Ner.Token tte = tt.Previous; tte != null; tte = tte.Previous) 
                        {
                            if (tte.IsComma) 
                            {
                                if ((--k) == 0) 
                                {
                                    if (t.BeginChar <= tte.Previous.BeginChar) 
                                        return new Pullenti.Ner.ReferentToken(dr, t, tte.Previous);
                                }
                            }
                        }
                        break;
                    }
                    if (dr.Name != null) 
                    {
                        Pullenti.Ner.Core.Termin term = new Pullenti.Ner.Core.Termin(dr.Name);
                        Pullenti.Ner.Core.TerminToken tok = term.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
                        if (tok != null) 
                            return new Pullenti.Ner.ReferentToken(dr, t, tok.EndToken);
                        string nam0 = dr.Name;
                        for (int i = 0; (i < nam0.Length) && (i < nam.Length); i++) 
                        {
                            if (nam0[i] != nam[i]) 
                            {
                                if (i >= 50) 
                                    return new Pullenti.Ner.ReferentToken(dr, t, tt.Previous);
                                break;
                            }
                        }
                    }
                    if (ty == null) 
                    {
                        DecreeReferent dd = new DecreeReferent();
                        dd.AddNameStr(nam2 ?? nam, new Pullenti.Ner.MetaToken(t, tt.Previous));
                        if (dr.Owner != null) 
                            dd.Owner = dr.Owner;
                        else if (dr.Typ != null) 
                            dd.Owner = dr;
                        return new Pullenti.Ner.ReferentToken(dd, t, tt.Previous);
                    }
                }
                if (keyTok != null && keyTok.IsValue("УСТАВ", null) && keyTok.Next == tt.Previous) 
                {
                    if ((tt.Previous.GetReferent() is Pullenti.Ner.Geo.GeoReferent) || (tt.Previous.GetReferent() is Pullenti.Ner.Org.OrganizationReferent)) 
                    {
                        DecreeReferent dr = new DecreeReferent();
                        dr.AddSlot(DecreeReferent.ATTR_TYPE, "УСТАВ", false, 0);
                        dr.AddSlot(DecreeReferent.ATTR_NAME, nam, false, 0);
                        return new Pullenti.Ner.ReferentToken(dr, t, tt.Previous);
                    }
                }
                bool ok1 = false;
                if ((((t0 is Pullenti.Ner.TextToken) && (t0 as Pullenti.Ner.TextToken).Term == "ИЗМЕНЕНИЯ" && !t0.Chars.IsAllLower) && t0.IsNewlineBefore && keywordAfter) && tt.IsNewlineBefore) 
                    ok1 = true;
                else if (keyTok != null) 
                {
                    Pullenti.Ner.Token ttt2 = t0.Previous;
                    for (; ttt2 != null; ttt2 = ttt2.Previous) 
                    {
                        if ((ttt2.IsValue("ТЕКСТ", null) || ttt2.IsValue("В", null) || ttt2.IsCharOf(".)")) || (ttt2 is Pullenti.Ner.NumberToken)) 
                        {
                        }
                        else 
                        {
                            if (ttt2.IsNewlineAfter) 
                                ok1 = true;
                            break;
                        }
                    }
                }
                if (ok1 && Pullenti.Ner.Decree.Internal.DecreeChangeToken._canBeChangeToken(tt, 0, false, false)) 
                {
                    if (keyTok != null) 
                    {
                        if (keyTok.IsValue("АКТ", null) && keyTok.Morph.Number == Pullenti.Morph.MorphNumber.Plural) 
                            return null;
                    }
                    if (Pullenti.Ner.Decree.Internal.DecreeChangeToken._tryParseSpecialPart0(t, false) != null) 
                        return null;
                    DecreeReferent dr = new DecreeReferent();
                    dr.AddSlot(DecreeReferent.ATTR_NAME, nam, false, 0);
                    return new Pullenti.Ner.ReferentToken(dr, t, tt.Previous);
                }
            }
            return null;
        }
        internal static Pullenti.Ner.ReferentToken _tryAttachApproved(Pullenti.Ner.Token t, Pullenti.Ner.Core.AnalyzerData ad, bool mustBeComma = true, bool nameInBrackets = false, Pullenti.Ner.Core.TerminCollection aliases = null)
        {
            if (t == null) 
                return null;
            if (t.GetReferent() is DecreeReferent) 
            {
                Pullenti.Ner.ReferentToken rt0 = _tryAttachApproved((t as Pullenti.Ner.ReferentToken).BeginToken, ad, true, false, null);
                if (rt0 != null && rt0.EndChar == t.EndChar) 
                {
                    if (rt0.Referent.Slots.Count <= 1) 
                        return new Pullenti.Ner.ReferentToken(rt0.Referent, t, t);
                    DecreeReferent dr0 = new DecreeReferent();
                    dr0.Owner = ((ad == null ? rt0.Referent : ad.RegisterReferent(rt0.Referent))) as DecreeReferent;
                    return new Pullenti.Ner.ReferentToken(dr0, t, t);
                }
            }
            if (t.Next == null) 
                return null;
            Pullenti.Ner.Token t0 = t;
            if (!t.IsCharOf("(,") && !t.IsHiphen) 
            {
                if (t.Previous != null && t.Previous.IsComma) 
                {
                }
                else if (mustBeComma) 
                    return null;
            }
            else 
                t = t.Next;
            bool ok = false;
            for (; t != null; t = t.Next) 
            {
                if (t.IsCommaAnd || t.Morph.Class.IsPreposition) 
                    continue;
                if ((t.GetReferent() is Pullenti.Ner.Geo.GeoReferent) && (t.GetReferent() as Pullenti.Ner.Geo.GeoReferent).IsCity) 
                    continue;
                if ((((((((t.IsValue("УТВ", null) || t.IsValue("УТВЕРЖДАТЬ", "СТВЕРДЖУВАТИ") || t.IsValue("УТВЕРДИТЬ", "ЗАТВЕРДИТИ")) || t.IsValue("УТВЕРЖДЕННЫЙ", "ЗАТВЕРДЖЕНИЙ") || t.IsValue("РАТИФИЦИРОВАННЫЙ", null)) || t.IsValue("ЗАТВЕРДЖУВАТИ", null) || t.IsValue("СТВЕРДИТИ", null)) || t.IsValue("ЗАТВЕРДИТИ", null) || t.IsValue("ПРИНЯТЬ", "ПРИЙНЯТИ")) || t.IsValue("ПРИНЯТЫЙ", "ПРИЙНЯТИЙ") || t.IsValue("ВВОДИТЬ", "ВВОДИТИ")) || t.IsValue("ВВЕСТИ", null) || t.IsValue("ВВЕДЕННЫЙ", "ВВЕДЕНИЙ")) || t.IsValue("ПОДПИСАТЬ", "ПІДПИСАТИ") || t.IsValue("ПОДПИСЫВАТЬ", "ПІДПИСУВАТИ")) || t.IsValue("ЗАКЛЮЧИТЬ", "УКЛАСТИ") || t.IsValue("ЗАКЛЮЧАТЬ", "УКЛАДАТИ")) 
                {
                    ok = true;
                    if (t.Next != null && t.Next.IsChar('.')) 
                        t = t.Next;
                }
                else if (t.IsValue("УСТАНОВЛЕННЫЙ", null)) 
                {
                    Pullenti.Ner.Token tt = t.Previous;
                    if (tt != null && tt.IsComma) 
                        tt = tt.Previous;
                    if (tt != null) 
                    {
                        if (tt.IsValue("ПОРЯДОК", null)) 
                            break;
                    }
                    ok = true;
                }
                else if (t.IsValue("ДЕЙСТВИЕ", null) || t.IsValue("ДІЯ", null)) 
                {
                }
                else if (t.IsValue("ОПРЕДЕЛЕННЫЙ", null) && t.Next != null && (t.Next.GetReferent() is DecreeReferent)) 
                {
                    ok = true;
                    t = t.Next;
                    break;
                }
                else if (nameInBrackets && t0.IsHiphen) 
                {
                    ok = true;
                    break;
                }
                else if (t.IsValue2("ПРИЛАГАЕМЫЙ", "К")) 
                {
                    ok = true;
                    t = t.Next;
                }
                else 
                    break;
            }
            if (!ok) 
                return null;
            if (t == null) 
                return null;
            if (t.GetReferent() is DecreeReferent) 
            {
                DecreeReferent dr = new DecreeReferent();
                dr.Owner = t.GetReferent() as DecreeReferent;
                if (dr.Owner == null) 
                    return null;
                Pullenti.Ner.ReferentToken res0 = new Pullenti.Ner.ReferentToken(dr, t, t);
                if (t0.IsChar('(') && t.Next != null && t.Next.IsChar(')')) 
                    res0.EndToken = t.Next;
                return res0;
            }
            Pullenti.Ner.Core.AnalysisKit kit = t.Kit;
            object olev = null;
            int lev = 0;
            if (!kit.MiscData.TryGetValue("dovr", out olev)) 
                kit.MiscData.Add("dovr", (lev = 1));
            else 
            {
                lev = (int)olev;
                if (lev > 2) 
                    return null;
                lev++;
                kit.MiscData["dovr"] = lev;
            }
            try 
            {
                List<Pullenti.Ner.Decree.Internal.DecreeToken> dts = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttachList(t, null, 10, false);
                if (dts == null) 
                    return null;
                List<Pullenti.Ner.ReferentToken> rt = TryAttach(dts, null, ad);
                if (rt == null) 
                {
                    int hasDate = 0;
                    int hasNum = 0;
                    int hasOwn = 0;
                    int hasTyp = 0;
                    int ii;
                    for (ii = 0; ii < dts.Count; ii++) 
                    {
                        if (dts[ii].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number) 
                            hasNum++;
                        else if ((dts[ii].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Date && dts[ii].Ref != null && (dts[ii].Ref.Referent is Pullenti.Ner.Date.DateReferent)) && (dts[ii].Ref.Referent as Pullenti.Ner.Date.DateReferent).Dt != null) 
                            hasDate++;
                        else if (dts[ii].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Owner || dts[ii].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Org) 
                            hasOwn++;
                        else if (dts[ii].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) 
                            hasTyp++;
                        else 
                            break;
                    }
                    if (ii >= dts.Count && hasOwn > 0 && ((hasDate == 1 || hasNum == 1))) 
                    {
                        DecreeReferent dr = new DecreeReferent();
                        foreach (Pullenti.Ner.Decree.Internal.DecreeToken dt in dts) 
                        {
                            if (dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Date) 
                                dr.AddDate(dt);
                            else if (dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number) 
                                dr.AddNumber(dt);
                            else if (dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) 
                                dr.AddSlot(DecreeReferent.ATTR_TYPE, dt.Value, false, 0);
                            else 
                            {
                                object val = (object)dt.Value;
                                if (dt.Ref != null && dt.Ref.Referent != null) 
                                    val = dt.Ref.Referent;
                                Pullenti.Ner.Slot s = dr.AddSlot(DecreeReferent.ATTR_SOURCE, val, false, 0);
                                s.Tag = dt.GetSourceText();
                                s.AddAnnotation(dt);
                                if (dt.Ref != null && (dt.Ref.Referent is Pullenti.Ner.Person.PersonPropertyReferent)) 
                                    dr.AddExtReferent(dt.Ref);
                            }
                        }
                        rt = new List<Pullenti.Ner.ReferentToken>();
                        rt.Add(new Pullenti.Ner.ReferentToken(dr, dts[0].BeginToken, dts[dts.Count - 1].EndToken));
                    }
                }
                if (((rt == null && dts.Count == 1 && dts[0].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Date) && dts[0].Ref != null && (dts[0].Ref.Referent is Pullenti.Ner.Date.DateReferent)) && (dts[0].Ref.Referent as Pullenti.Ner.Date.DateReferent).Dt != null) 
                {
                    DecreeReferent dr = new DecreeReferent();
                    dr.AddDate(dts[0]);
                    rt = new List<Pullenti.Ner.ReferentToken>();
                    rt.Add(new Pullenti.Ner.ReferentToken(dr, dts[0].BeginToken, dts[dts.Count - 1].EndToken));
                }
                if ((rt == null && dts.Count == 1 && dts[0].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) && aliases != null) 
                {
                    Pullenti.Ner.Core.TerminToken tok = aliases.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
                    if (tok != null) 
                    {
                        DecreeReferent dr = new DecreeReferent();
                        dr.Owner = tok.Termin.Tag as DecreeReferent;
                        rt = new List<Pullenti.Ner.ReferentToken>();
                        rt.Add(new Pullenti.Ner.ReferentToken(dr, dts[0].BeginToken, tok.EndToken));
                    }
                }
                if (rt == null && dts.Count == 1 && dts[0].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) 
                {
                    Pullenti.Ner.Token tt2 = dts[0].EndToken.Next;
                    if (tt2 != null && tt2.IsCharOf(";:,")) 
                        tt2 = tt2.Next;
                    if (tt2 == null || tt2.IsNewlineBefore) 
                    {
                        for (tt2 = tt2.Previous; tt2 != null; tt2 = tt2.Previous) 
                        {
                            DecreeReferent dr1 = tt2.GetReferent() as DecreeReferent;
                            if (dr1 == null) 
                                continue;
                            if (dr1.Owner != null) 
                                dr1 = dr1.Owner;
                            if (dr1.Typ != dts[0].Value) 
                                continue;
                            DecreeReferent dr = new DecreeReferent();
                            dr.Owner = dr1;
                            rt = new List<Pullenti.Ner.ReferentToken>();
                            rt.Add(new Pullenti.Ner.ReferentToken(dr, dts[0].BeginToken, dts[0].EndToken));
                            break;
                        }
                    }
                }
                if (rt == null) 
                    return null;
                if (t0.IsChar('(') && rt[0].EndToken.Next != null && rt[0].EndToken.Next.IsChar(')')) 
                    rt[0].EndToken = rt[0].EndToken.Next;
                rt[0].BeginToken = t0;
                return rt[0];
            }
            finally
            {
                lev--;
                if (lev < 0) 
                    lev = 0;
                kit.MiscData["dovr"] = lev;
            }
        }
        List<Pullenti.Ner.ReferentToken> TryAttachPulishers(List<Pullenti.Ner.Decree.Internal.DecreeToken> dts)
        {
            int i = 0;
            Pullenti.Ner.Token t1 = null;
            Pullenti.Ner.Decree.Internal.DecreeToken typ = null;
            Pullenti.Ner.ReferentToken geo = null;
            Pullenti.Ner.ReferentToken org = null;
            Pullenti.Ner.Decree.Internal.DecreeToken date = null;
            for (i = 0; i < dts.Count; i++) 
            {
                if (dts[i].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ && Pullenti.Ner.Decree.Internal.DecreeToken.GetKind(dts[i].Value, null) == DecreeKind.Publisher) 
                {
                    if (typ != null) 
                        break;
                    typ = dts[i];
                    if (dts[i].Ref != null && (dts[i].Ref.GetReferent() is Pullenti.Ner.Geo.GeoReferent)) 
                        geo = dts[i].Ref;
                }
                else if (dts[i].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Terr) 
                {
                    geo = dts[i].Ref;
                    t1 = dts[i].EndToken;
                }
                else if (dts[i].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Date) 
                {
                    date = dts[i];
                    t1 = dts[i].EndToken;
                }
                else if (dts[i].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Org) 
                {
                    org = dts[i].Ref;
                    t1 = dts[i].EndToken;
                }
                else 
                    break;
            }
            if (typ == null) 
                return null;
            Pullenti.Ner.Token t = dts[i - 1].EndToken.Next;
            List<Pullenti.Ner.ReferentToken> res = new List<Pullenti.Ner.ReferentToken>();
            Pullenti.Ner.Decree.Internal.DecreeToken num = null;
            Pullenti.Ner.Token t0 = dts[0].BeginToken;
            if (Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(t, true, null, false)) 
            {
                t = t.Next;
                if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t0.Previous, true, false)) 
                    t0 = t0.Previous;
            }
            DecreeReferent pub0 = null;
            DecreePartReferent pubPart0 = null;
            for (; t != null; t = t.Next) 
            {
                if (t.IsCharOf(",;.") || t.IsAnd) 
                    continue;
                Pullenti.Ner.Decree.Internal.DecreeToken dt = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(t, dts[0], false);
                if ((dt == null && (t is Pullenti.Ner.NumberToken) && !t.IsNewlineBefore) && res.Count == 0) 
                    dt = new Pullenti.Ner.Decree.Internal.DecreeToken(t, t) { Typ = Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number, Value = (t as Pullenti.Ner.NumberToken).Value };
                if (dt != null) 
                {
                    if (dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number) 
                    {
                        num = dt;
                        pub0 = null;
                        pubPart0 = null;
                        if (t0 == null) 
                            t0 = t;
                        t1 = (t = dt.EndToken);
                        continue;
                    }
                    if (dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Date) 
                    {
                        if (t0 == null) 
                            t0 = t;
                        date = dt;
                        pub0 = null;
                        pubPart0 = null;
                        t1 = (t = dt.EndToken);
                        continue;
                    }
                    if (dt.Typ != Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Misc && t.LengthChar > 2) 
                        break;
                }
                Pullenti.Ner.Decree.Internal.PartToken pt = Pullenti.Ner.Decree.Internal.PartToken.TryAttach(t, null, false, false);
                if (pt == null && t.IsChar('(')) 
                {
                    pt = Pullenti.Ner.Decree.Internal.PartToken.TryAttach(t.Next, null, false, false);
                    if (pt != null) 
                    {
                        if (pt.EndToken.Next != null && pt.EndToken.Next.IsChar(')')) 
                            pt.EndToken = pt.EndToken.Next;
                        else 
                            pt = null;
                    }
                }
                if (pt != null) 
                {
                    if (pt.Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Page) 
                    {
                        t = pt.EndToken;
                        continue;
                    }
                    if (pt.Typ != Pullenti.Ner.Decree.Internal.PartToken.ItemType.Clause && pt.Typ != Pullenti.Ner.Decree.Internal.PartToken.ItemType.Part && pt.Typ != Pullenti.Ner.Decree.Internal.PartToken.ItemType.Page) 
                        break;
                    if (num == null) 
                        break;
                    if (pubPart0 != null) 
                    {
                        if (pt.Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Part && pubPart0.Part == null) 
                        {
                        }
                        else if (pt.Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Clause && pubPart0.Clause == null) 
                        {
                        }
                        else 
                            pubPart0 = null;
                    }
                    DecreeReferent pub = pub0;
                    DecreePartReferent pubPart = pubPart0;
                    if (pub == null) 
                    {
                        pub = new DecreeReferent();
                        pub.AddTyp(typ);
                        if (geo != null) 
                        {
                            Pullenti.Ner.Slot ss = pub.AddSlot(DecreeReferent.ATTR_GEO, geo.Referent, false, 0);
                            ss.AddAnnotation(geo);
                        }
                        if (org != null) 
                        {
                            Pullenti.Ner.Slot ss = pub.AddSlot(DecreeReferent.ATTR_SOURCE, org.Referent, false, 0);
                            ss.Tag = org.GetSourceText();
                            ss.AddAnnotation(org);
                        }
                        if (date != null) 
                            pub.AddDate(date);
                        pub.AddNumber(num);
                        res.Add(new Pullenti.Ner.ReferentToken(pub, t0 ?? t, pt.BeginToken.Previous));
                    }
                    if (pubPart == null) 
                    {
                        pubPart = new DecreePartReferent() { Owner = pub };
                        res.Add(new Pullenti.Ner.ReferentToken(pubPart, pt.BeginToken, pt.EndToken));
                    }
                    pub0 = pub;
                    if (pt.Values.Count == 1) 
                    {
                        if (pt.Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Clause) 
                            pubPart.AddSlot(DecreePartReferent.ATTR_CLAUSE, pt.Values[0].Value, false, 0).Tag = pt.Values[0].SourceValue;
                        else if (pt.Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Part) 
                            pubPart.AddSlot(DecreePartReferent.ATTR_PART, pt.Values[0].Value, false, 0).Tag = pt.Values[0].SourceValue;
                    }
                    else if (pt.Values.Count > 1) 
                    {
                        for (int ii = 0; ii < pt.Values.Count; ii++) 
                        {
                            if (ii > 0) 
                            {
                                pubPart = new DecreePartReferent() { Owner = pub };
                                res.Add(new Pullenti.Ner.ReferentToken(pubPart, pt.Values[ii].BeginToken, pt.Values[ii].EndToken));
                            }
                            else 
                                res[res.Count - 1].EndToken = pt.Values[ii].EndToken;
                            if (pt.Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Clause) 
                                pubPart.AddSlot(DecreePartReferent.ATTR_CLAUSE, pt.Values[ii].Value, false, 0).Tag = pt.Values[ii].SourceValue;
                            else if (pt.Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Part) 
                                pubPart.AddSlot(DecreePartReferent.ATTR_PART, pt.Values[ii].Value, false, 0).Tag = pt.Values[ii].SourceValue;
                        }
                    }
                    if (pubPart.Clause == "6878") 
                    {
                    }
                    pubPart0 = pubPart;
                    res[res.Count - 1].EndToken = pt.EndToken;
                    t0 = null;
                    t = pt.EndToken;
                    continue;
                }
                if (t is Pullenti.Ner.NumberToken) 
                {
                    Pullenti.Ner.ReferentToken rt = t.Kit.ProcessReferent("DATE", t, "YEAR");
                    if (rt != null) 
                    {
                        date = new Pullenti.Ner.Decree.Internal.DecreeToken(rt.BeginToken, rt.EndToken) { Typ = Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Date };
                        date.Ref = rt;
                        pub0 = null;
                        pubPart0 = null;
                        if (t0 == null) 
                            t0 = t;
                        t1 = (t = rt.EndToken);
                        continue;
                    }
                    if (t.Next != null && t.Next.IsChar(';')) 
                    {
                        if (pubPart0 != null && pubPart0.Clause != null && pub0 != null) 
                        {
                            DecreePartReferent pubPart = new DecreePartReferent();
                            foreach (Pullenti.Ner.Slot s in pubPart0.Slots) 
                            {
                                pubPart.AddSlot(s.TypeName, s.Value, false, 0);
                            }
                            pubPart0 = pubPart;
                            pubPart0.Clause = (t as Pullenti.Ner.NumberToken).Value.ToString();
                            res.Add(new Pullenti.Ner.ReferentToken(pubPart0, t, t));
                            continue;
                        }
                    }
                }
                if (((t is Pullenti.Ner.TextToken) && t.Chars.IsLetter && (t.LengthChar < 3)) && (t.Next is Pullenti.Ner.NumberToken)) 
                {
                    t = t.Next;
                    continue;
                }
                if ((t.IsChar('(') && t.Next != null && t.Next.Next != null) && t.Next.Next.IsChar(')')) 
                {
                    t = t.Next.Next;
                    continue;
                }
                break;
            }
            if ((res.Count == 0 && date != null && ((num != null || typ.Value.EndsWith("ГАЗЕТА")))) && t1 != null) 
            {
                DecreeReferent pub = new DecreeReferent();
                pub.AddTyp(typ);
                if (geo != null) 
                    pub.AddSlot(DecreeReferent.ATTR_GEO, geo.Referent, false, 0);
                if (org != null) 
                    pub.AddSlot(DecreeReferent.ATTR_SOURCE, org.Referent, false, 0).Tag = org.GetSourceText();
                if (date != null) 
                    pub.AddDate(date);
                pub.AddNumber(num);
                res.Add(new Pullenti.Ner.ReferentToken(pub, t0, t1));
            }
            return res;
        }
        internal static List<Pullenti.Ner.MetaToken> TryAttachParts(List<Pullenti.Ner.Decree.Internal.PartToken> parts, Pullenti.Ner.Decree.Internal.DecreeToken baseTyp, Pullenti.Ner.Referent _defOwner, bool inChanges = false)
        {
            if (parts == null || parts.Count == 0) 
                return null;
            int i;
            int j;
            if (baseTyp != null) 
            {
                if (baseTyp.Value == "ПОЛОЖЕНИЕ" || baseTyp.Value == "ОПРЕДЕЛЕНИЕ") 
                    baseTyp = null;
            }
            if (parts.Count == 1 && parts[0].Morph.Number == Pullenti.Morph.MorphNumber.Plural) 
            {
                if (parts[0].Values.Count == 0 && parts[0].Name == null) 
                    return null;
            }
            if ((parts.Count == 1 && parts[0].BeginToken.Previous != null && parts[0].BeginToken.Previous.IsChar('(')) && parts[0].BeginToken.Previous.IsNewlineBefore) 
            {
                Pullenti.Ner.Token tt2 = parts[0].EndToken.Next;
                if (tt2 != null) 
                {
                    if (tt2.IsValue("В", null) || tt2.IsValue("ВВЕДЕН", null)) 
                        return null;
                }
            }
            for (int jj = 0; jj < (parts.Count - 1); jj++) 
            {
                if (parts[jj].Decree == null) 
                    continue;
                bool br = false;
                if (parts[jj].DelimAfter) 
                    br = true;
                else 
                    for (int kk = jj + 1; kk < parts.Count; kk++) 
                    {
                        if (parts[kk].Decree != null && parts[kk].Decree != parts[jj].Decree) 
                            br = true;
                    }
                if (!br) 
                    continue;
                List<Pullenti.Ner.Decree.Internal.PartToken> parts1 = new List<Pullenti.Ner.Decree.Internal.PartToken>(parts);
                parts1.RemoveRange(jj + 1, parts.Count - jj - 1);
                List<Pullenti.Ner.MetaToken> res11 = TryAttachParts(parts1, baseTyp, _defOwner, inChanges);
                if (res11 == null) 
                    break;
                List<Pullenti.Ner.Decree.Internal.PartToken> parts2 = new List<Pullenti.Ner.Decree.Internal.PartToken>(parts);
                parts2.RemoveRange(0, jj + 1);
                List<Pullenti.Ner.MetaToken> res22 = TryAttachParts(parts2, baseTyp, _defOwner, inChanges);
                if (res22 != null) 
                    res11.AddRange(res22);
                return res11;
            }
            for (int jj = 0; jj < (parts.Count - 1); jj++) 
            {
                if (parts[jj].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Name && parts[jj].DelimAfter && ((jj == 0 || parts[jj - 1].DelimAfter))) 
                {
                    List<Pullenti.Ner.Decree.Internal.PartToken> parts0 = new List<Pullenti.Ner.Decree.Internal.PartToken>(parts);
                    if (jj > 0) 
                        parts0.RemoveRange(0, jj);
                    parts0.RemoveAt(0);
                    List<Pullenti.Ner.MetaToken> res11 = TryAttachParts(parts0, baseTyp, _defOwner, inChanges);
                    if (res11 != null) 
                    {
                        List<DecreePartReferent> named = new List<DecreePartReferent>();
                        List<string> snamed = new List<string>();
                        foreach (Pullenti.Ner.MetaToken rr in res11) 
                        {
                            List<DecreePartReferent> pli = rr.Tag as List<DecreePartReferent>;
                            if (pli != null) 
                            {
                                foreach (DecreePartReferent ppp in pli) 
                                {
                                    DecreePartReferent pp2 = new DecreePartReferent();
                                    pp2.NameAsItem = "0";
                                    pp2.AddNamedLevelInfo(ppp);
                                    if (!snamed.Contains(pp2.ToString())) 
                                    {
                                        named.Add(pp2);
                                        snamed.Add(pp2.ToString());
                                    }
                                }
                            }
                        }
                        res11.Insert(0, new Pullenti.Ner.MetaToken(parts[0].BeginToken, parts[0].EndToken));
                        res11[0].Tag = named;
                        if (jj > 0) 
                        {
                            parts0.Clear();
                            parts0.AddRange(parts);
                            parts0.RemoveRange(jj, parts.Count - jj);
                            List<Pullenti.Ner.MetaToken> res00 = TryAttachParts(parts0, baseTyp, _defOwner, false);
                            if (res00 != null) 
                                res11.InsertRange(0, res00);
                        }
                        return res11;
                    }
                }
            }
            if ((parts[0].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Name && parts[0].Morph.Number == Pullenti.Morph.MorphNumber.Plural && parts.Count > 2) && parts[1].DelimAfter) 
            {
                List<Pullenti.Ner.MetaToken> res0 = new List<Pullenti.Ner.MetaToken>();
                for (i = 1; i < parts.Count; i++) 
                {
                    List<Pullenti.Ner.Decree.Internal.PartToken> parts0 = new List<Pullenti.Ner.Decree.Internal.PartToken>();
                    for (j = i; j < parts.Count; j++) 
                    {
                        parts0.Add(parts[j]);
                        if (parts[j].DelimAfter) 
                            break;
                    }
                    i = j;
                    List<Pullenti.Ner.MetaToken> res11 = TryAttachParts(parts0, baseTyp, _defOwner, inChanges);
                    if (res11 != null) 
                    {
                        foreach (Pullenti.Ner.MetaToken rr in res11) 
                        {
                            List<DecreePartReferent> pli = rr.Tag as List<DecreePartReferent>;
                            if (pli != null) 
                            {
                                foreach (DecreePartReferent ppp in pli) 
                                {
                                    ppp.NameAsItem = "0";
                                }
                            }
                            if (rr.BeginToken == parts[1].BeginToken) 
                                rr.BeginToken = parts[0].BeginToken;
                            res0.Add(rr);
                        }
                    }
                }
                return res0;
            }
            if (parts.Count == 1 && parts[0].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Name && !inChanges) 
                return null;
            if ((parts.Count == 1 && parts[0].Decree == null && parts[0].IsNewlineBefore) && ((parts[0].IsNewlineAfter || ((parts[0].EndToken.IsValue("К", null) && parts[0].EndToken.IsNewlineBefore))))) 
            {
                if ((parts[0].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Appendix || parts[0].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Clause || parts[0].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Section) || parts[0].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Chapter) 
                    return null;
            }
            Pullenti.Ner.Token tt = parts[parts.Count - 1].EndToken.Next;
            if (_defOwner == null) 
                _defOwner = parts[parts.Count - 1].Decree;
            if (_defOwner != null && tt != null) 
            {
                if (Pullenti.Ner.Core.BracketHelper.IsBracket(tt, false)) 
                {
                    Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(tt, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                    if (br != null && br.EndToken.Next != null) 
                        tt = br.EndToken.Next;
                }
                if (tt.GetReferent() is DecreeReferent) 
                    _defOwner = null;
                else if (tt.IsValue("К", null) && tt.Next != null) 
                {
                    if (tt.Next.GetReferent() is DecreeReferent) 
                        _defOwner = null;
                }
            }
            if ((parts.Count == 1 && parts[0].Decree == null && ((parts[0].IsNewlineBefore || parts[0].BeginToken.Previous.IsTableControlChar))) && parts[0].BeginToken.Chars.IsLetter && !parts[0].BeginToken.Chars.IsAllLower) 
            {
                Pullenti.Ner.Token t1 = parts[0].EndToken.Next;
                Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t1, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                if (br != null) 
                    t1 = br.EndToken.Next;
                if (t1 != null && (t1.GetReferent() is DecreeReferent) && !parts[0].IsNewlineAfter) 
                {
                }
                else if ((t1 != null && t1.IsValue("К", null) && t1.Next != null) && (t1.Next.GetReferent() is DecreeReferent) && !parts[0].IsNewlineAfter) 
                {
                }
                else 
                {
                    Pullenti.Ner.Instrument.Internal.InstrToken1 li = Pullenti.Ner.Instrument.Internal.InstrToken1.Parse(parts[0].BeginToken, true, null, 0, null, false, 0, false, false);
                    if (li != null && (li.HasVerb)) 
                    {
                        if ((parts.Count == 1 && parts[0].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Part && parts[0].ToString().Contains("резолют")) && parts[0].IsNewlineBefore) 
                            return null;
                    }
                    else 
                        return null;
                }
            }
            ThisDecree thisDec = null;
            bool isProgram = false;
            bool isAddAgree = false;
            if (parts[parts.Count - 1].Typ != Pullenti.Ner.Decree.Internal.PartToken.ItemType.Subprogram && parts[parts.Count - 1].Typ != Pullenti.Ner.Decree.Internal.PartToken.ItemType.AddAgree) 
            {
                thisDec = ThisDecree.TryAttach(parts[parts.Count - 1], baseTyp);
                if (thisDec != null) 
                {
                    if (thisDec.CheckDecree(_defOwner as DecreeReferent)) 
                    {
                    }
                    else if ((_defOwner is DecreeReferent) && ((parts[0].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Appendix || parts[parts.Count - 1].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Appendix))) 
                    {
                    }
                    else 
                        _defOwner = null;
                }
                if (thisDec == null && _defOwner == null) 
                {
                    if (parts.Count == 1 && parts[0].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Appendix && parts[0].BeginToken == parts[0].EndToken) 
                        return null;
                    thisDec = ThisDecree.TryAttachBack(parts[0].BeginToken, baseTyp);
                }
                if (thisDec == null && parts[0].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Name && !inChanges) 
                    return null;
                if (thisDec == null) 
                {
                    foreach (Pullenti.Ner.Decree.Internal.PartToken p in parts) 
                    {
                        if (p.Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Part) 
                        {
                            bool hasClause = false;
                            foreach (Pullenti.Ner.Decree.Internal.PartToken pp in parts) 
                            {
                                if (pp != p) 
                                {
                                    if (Pullenti.Ner.Decree.Internal.PartToken._getRank(pp.Typ) >= Pullenti.Ner.Decree.Internal.PartToken._getRank(Pullenti.Ner.Decree.Internal.PartToken.ItemType.Clause)) 
                                        hasClause = true;
                                }
                            }
                            if (_defOwner is DecreePartReferent) 
                            {
                                if ((_defOwner as DecreePartReferent).Clause != null) 
                                    hasClause = true;
                            }
                            if (!hasClause) 
                                p.Typ = Pullenti.Ner.Decree.Internal.PartToken.ItemType.DocPart;
                            else if ((((p == parts[parts.Count - 1] && p.EndToken.Next != null && p.Values.Count == 1) && (p.EndToken.Next.GetReferent() is DecreeReferent) && (p.BeginToken is Pullenti.Ner.TextToken)) && (p.BeginToken as Pullenti.Ner.TextToken).Term == "ЧАСТИ" && (p.EndToken is Pullenti.Ner.NumberToken)) && p.BeginToken.Next == p.EndToken) 
                                p.Typ = Pullenti.Ner.Decree.Internal.PartToken.ItemType.DocPart;
                        }
                    }
                }
            }
            else if (parts[parts.Count - 1].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.AddAgree) 
                isAddAgree = true;
            else 
            {
                if (parts.Count > 1) 
                {
                }
                isProgram = true;
            }
            DecreeReferent defOwner = _defOwner as DecreeReferent;
            if (defOwner == null && thisDec != null && thisDec.Real != null) 
                defOwner = thisDec.Real;
            string docPart = null;
            if (_defOwner is DecreePartReferent) 
            {
                defOwner = (_defOwner as DecreePartReferent).Owner;
                docPart = (_defOwner as DecreePartReferent).DocPart;
            }
            string curTyp = null;
            Pullenti.Ner.Token curTypToken = null;
            if (parts[parts.Count - 1].EndToken.Next != null && parts[parts.Count - 1].EndToken.Next.IsValue("НАСТОЯЩИЙ", "СПРАВЖНІЙ")) 
            {
                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(parts[parts.Count - 1].EndToken.Next.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt != null) 
                {
                    curTyp = npt.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false);
                    curTypToken = npt.EndToken;
                    thisDec = null;
                }
            }
            List<Pullenti.Ner.ReferentToken> res = new List<Pullenti.Ner.ReferentToken>();
            bool hasPrefix = false;
            if (parts[0].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Prefix) 
            {
                parts.RemoveAt(0);
                hasPrefix = true;
                if (parts.Count == 0) 
                    return null;
            }
            if ((parts.Count == 1 && thisDec == null && parts[0].Typ != Pullenti.Ner.Decree.Internal.PartToken.ItemType.Subprogram) && parts[0].Typ != Pullenti.Ner.Decree.Internal.PartToken.ItemType.AddAgree) 
            {
                if (parts[0].IsDoubt) 
                    return null;
                if (parts[0].IsNewlineBefore && ((parts[0].Values.Count <= 1 || ((parts[0].Values.Count == 2 && parts[0].Values[0].EndToken.Next.IsHiphen))))) 
                {
                    Pullenti.Ner.Token tt1 = parts[0].EndToken;
                    if (tt1.Next == null) 
                        return null;
                    tt1 = tt1.Next;
                    if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(tt1, false, false)) 
                    {
                        Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(tt1, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                        if (br != null && br.EndToken.Next != null) 
                            tt1 = br.EndToken.Next;
                    }
                    if (tt1.IsChar(',')) 
                    {
                    }
                    else if (tt1.GetReferent() is DecreeReferent) 
                    {
                    }
                    else if (tt1.IsValue("К", null) && tt1.Next != null && (tt1.Next.GetReferent() is DecreeReferent)) 
                    {
                    }
                    else if (_checkOtherTyp(tt1, true) != null) 
                    {
                    }
                    else if (_defOwner == null) 
                        return null;
                    else if (Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(tt1)) 
                        return null;
                    else if (tt1.IsChar('.')) 
                        return null;
                }
            }
            List<Pullenti.Ner.Decree.Internal.PartToken> asc = new List<Pullenti.Ner.Decree.Internal.PartToken>();
            List<Pullenti.Ner.Decree.Internal.PartToken> desc = new List<Pullenti.Ner.Decree.Internal.PartToken>();
            List<Pullenti.Ner.Decree.Internal.PartToken.ItemType> typs = new List<Pullenti.Ner.Decree.Internal.PartToken.ItemType>();
            for (i = 1; i < (parts.Count - 1); i++) 
            {
                if (parts[i].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Sentence || parts[i].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Footnote) 
                {
                    parts.Add(parts[i]);
                    parts.RemoveAt(i);
                }
            }
            for (i = 0; i < (parts.Count - 2); i++) 
            {
                if ((parts[i].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Item && parts[i + 1].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Indention && parts[i + 2].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.SubItem) && !parts[i + 1].DelimAfter && !parts[i].DelimAfter) 
                {
                    Pullenti.Ner.Decree.Internal.PartToken pp = parts[i + 1];
                    parts[i + 1] = parts[i + 2];
                    parts[i + 2] = pp;
                    if (parts[i + 1].DelimAfter) 
                    {
                        parts[i + 1].DelimAfter = false;
                        pp.DelimAfter = true;
                    }
                }
            }
            int ascCount = 0;
            int descCount = 0;
            int terminators = 0;
            for (i = 0; i < (parts.Count - 1); i++) 
            {
                if (!parts[i].HasTerminator) 
                {
                    if (parts[i].CanBeNextNarrow(parts[i + 1], parts)) 
                    {
                        ascCount++;
                        if (parts[i].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Table && parts[i + 1].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Item) 
                        {
                            foreach (Pullenti.Ner.Decree.Internal.PartToken p in parts) 
                            {
                                if (p.Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.TableRow) 
                                {
                                    ascCount--;
                                    descCount++;
                                    break;
                                }
                            }
                        }
                    }
                    if (parts[i + 1].CanBeNextNarrow(parts[i], parts)) 
                        descCount++;
                }
                else if ((ascCount > 0 && parts[i].Values.Count == 1 && parts[i + 1].Values.Count == 1) && parts[i].CanBeNextNarrow(parts[i + 1], parts)) 
                    ascCount++;
                else if ((descCount > 0 && parts[i].Values.Count == 1 && parts[i + 1].Values.Count == 1) && parts[i + 1].CanBeNextNarrow(parts[i], parts)) 
                    descCount++;
                else 
                    terminators++;
            }
            if (terminators == 0 && ((((descCount > 0 && ascCount == 0)) || ((descCount == 0 && ascCount > 0))))) 
            {
                for (i = 0; i < (parts.Count - 1); i++) 
                {
                    parts[i].HasTerminator = false;
                }
            }
            for (i = 0; i < parts.Count; i++) 
            {
                if (parts[i].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Prefix) 
                    continue;
                asc.Clear();
                asc.Add(parts[i]);
                typs.Clear();
                typs.Add(parts[i].Typ);
                for (j = i + 1; j < parts.Count; j++) 
                {
                    if (((parts[j].Values.Count == 0 && parts[j].Typ != Pullenti.Ner.Decree.Internal.PartToken.ItemType.Appendix && parts[j].Typ != Pullenti.Ner.Decree.Internal.PartToken.ItemType.Preamble) && parts[j].Typ != Pullenti.Ner.Decree.Internal.PartToken.ItemType.Notice && parts[j].Typ != Pullenti.Ner.Decree.Internal.PartToken.ItemType.Table) && parts[j].Typ != Pullenti.Ner.Decree.Internal.PartToken.ItemType.Footnote && parts[j].Typ != Pullenti.Ner.Decree.Internal.PartToken.ItemType.Form) 
                        break;
                    else if (!typs.Contains(parts[j].Typ) && parts[j - 1].CanBeNextNarrow(parts[j], parts)) 
                    {
                        if (parts[j - 1].DelimAfter && terminators == 0) 
                        {
                            if (descCount > ascCount) 
                                break;
                            if (((j + 1) < parts.Count) && !parts[j].DelimAfter && !parts[j].HasTerminator) 
                                break;
                            if (parts[j - 1].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Item && parts[j].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.SubItem) 
                            {
                                if (parts[j].Values.Count > 0 && parts[j].Values[0].ToString().Contains(".")) 
                                    break;
                            }
                        }
                        asc.Add(parts[j]);
                        typs.Add(parts[j].Typ);
                        if (parts[j].HasTerminator) 
                            break;
                    }
                    else 
                        break;
                }
                desc.Clear();
                desc.Add(parts[i]);
                typs.Clear();
                typs.Add(parts[i].Typ);
                for (j = i + 1; j < parts.Count; j++) 
                {
                    if ((((parts[j].Values.Count == 0 && parts[j].Name == null && parts[j].Typ != Pullenti.Ner.Decree.Internal.PartToken.ItemType.Appendix) && parts[j].Typ != Pullenti.Ner.Decree.Internal.PartToken.ItemType.Preamble && parts[j].Typ != Pullenti.Ner.Decree.Internal.PartToken.ItemType.Notice) && parts[j].Typ != Pullenti.Ner.Decree.Internal.PartToken.ItemType.Footnote && parts[j].Typ != Pullenti.Ner.Decree.Internal.PartToken.ItemType.Table) && parts[j].Typ != Pullenti.Ner.Decree.Internal.PartToken.ItemType.Form) 
                        break;
                    else if (((!typs.Contains(parts[j].Typ) || parts[j].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.SubItem)) && parts[j].CanBeNextNarrow(parts[j - 1], parts)) 
                    {
                        if (parts[j - 1].DelimAfter && terminators == 0) 
                        {
                            if (descCount <= ascCount) 
                                break;
                        }
                        desc.Add(parts[j]);
                        typs.Add(parts[j].Typ);
                        if (parts[j].HasTerminator) 
                            break;
                    }
                    else if (((!typs.Contains(parts[j].Typ) && parts[j - 1].CanBeNextNarrow(parts[j], parts) && (j + 1) == (parts.Count - 1)) && parts[j + 1].CanBeNextNarrow(parts[j], parts) && parts[j + 1].CanBeNextNarrow(parts[j - 1], parts)) && !parts[j].HasTerminator) 
                    {
                        desc.Insert(desc.Count - 1, parts[j]);
                        typs.Add(parts[j].Typ);
                    }
                    else if (!typs.Contains(parts[j].Typ) && parts[j].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Sentence) 
                    {
                        desc.Insert(0, parts[j]);
                        typs.Add(parts[j].Typ);
                        break;
                    }
                    else if ((!typs.Contains(parts[j].Typ) && j == (parts.Count - 1) && parts[0].CanBeNextNarrow(parts[j], parts)) && j == 2) 
                    {
                        desc.Insert(0, parts[j]);
                        typs.Add(parts[j].Typ);
                        break;
                    }
                    else if ((parts[j].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Item && desc.Count > 1 && desc[desc.Count - 1].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Table) && desc[desc.Count - 2].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.TableRow) 
                    {
                        desc.Add(parts[j]);
                        typs.Add(parts[j].Typ);
                        break;
                    }
                    else 
                        break;
                }
                desc.Reverse();
                List<Pullenti.Ner.Decree.Internal.PartToken> li = (asc.Count < desc.Count ? desc : asc);
                for (j = 0; j < li.Count; j++) 
                {
                    li[j].Ind = 0;
                }
                while (true) 
                {
                    DecreePartReferent dr = new DecreePartReferent();
                    Pullenti.Ner.Token tt00 = parts[i].BeginToken;
                    Pullenti.Ner.Token tt11 = parts[(i + li.Count) - 1].EndToken;
                    for (int jj = i; jj < (i + li.Count); jj++) 
                    {
                        if (parts[jj].BeginChar < tt00.BeginChar) 
                            tt00 = parts[jj].BeginToken;
                        if (parts[jj].EndChar > tt11.EndChar) 
                            tt11 = parts[jj].EndToken;
                    }
                    Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(dr, tt00, tt11);
                    if (parts[i].Name != null) 
                        dr.AddName(parts[i], null);
                    rt.Morph = parts[i].Morph;
                    res.Add(rt);
                    List<Pullenti.Ner.Slot> slList = new List<Pullenti.Ner.Slot>();
                    foreach (Pullenti.Ner.Decree.Internal.PartToken p in li) 
                    {
                        if (p.Name != null) 
                            dr.AddName(p, null);
                        string nam = Pullenti.Ner.Decree.Internal.PartToken._getAttrNameByTyp(p.Typ);
                        if (nam != null) 
                        {
                            Pullenti.Ner.Slot sl = new Pullenti.Ner.Slot() { TypeName = nam, Tag = p, Count = 1 };
                            slList.Add(sl);
                            if (p.Ind < p.Values.Count) 
                            {
                                sl.Value = p.Values[p.Ind];
                                if (string.IsNullOrEmpty(p.Values[p.Ind].Value)) 
                                    sl.Value = "0";
                            }
                            else 
                                sl.Value = "0";
                        }
                        if (p.Ind > 0) 
                            rt.BeginToken = p.Values[p.Ind].BeginToken;
                        if ((p.Ind + 1) < p.Values.Count) 
                            rt.EndToken = p.Values[p.Ind].EndToken;
                    }
                    foreach (Pullenti.Ner.Decree.Internal.PartToken p in parts) 
                    {
                        foreach (Pullenti.Ner.Slot s in slList) 
                        {
                            if (s.Tag == p) 
                            {
                                dr.AddSlot(s.TypeName, s.Value, false, 0);
                                break;
                            }
                        }
                    }
                    for (j = li.Count - 1; j >= 0; j--) 
                    {
                        if ((++li[j].Ind) >= li[j].Values.Count) 
                            li[j].Ind = 0;
                        else 
                            break;
                    }
                    if (j < 0) 
                        break;
                }
                if (res.Count == 1) 
                {
                    for (int ii = i; ii < li.Count; ii++) 
                    {
                        if (res[0].BeginChar > li[ii].BeginChar) 
                            res[0].BeginToken = li[ii].BeginToken;
                        if (res[0].EndChar < li[ii].EndChar) 
                            res[0].EndToken = li[ii].EndToken;
                    }
                }
                i += (li.Count - 1);
            }
            if (res.Count == 0) 
                return null;
            for (j = res.Count - 1; j > 0; j--) 
            {
                DecreePartReferent d0 = res[j].Referent as DecreePartReferent;
                DecreePartReferent d = res[j - 1].Referent as DecreePartReferent;
                if (d0.Appendix != null && d.Appendix == null) 
                {
                    d.Appendix = d0.Appendix;
                    d.AppendixName = d0.AppendixName;
                }
                if (d0.Appendix2 != null && d.Appendix2 == null) 
                {
                    d.Appendix2 = d0.Appendix2;
                    d.Appendix2Name = d0.Appendix2Name;
                }
                if (d0.DocPart != null && d.DocPart == null) 
                    d.DocPart = d0.DocPart;
                else if (docPart != null && d.DocPart == null) 
                    d.DocPart = docPart;
                if (d0.Chapter != null && d.Chapter == null) 
                {
                    d.Chapter = d0.Chapter;
                    d.ChapterName = d0.ChapterName;
                }
                if (d0.Section != null && d.Section == null) 
                {
                    d.Section = d0.Section;
                    d.SectionName = d0.SectionName;
                }
                if (d0.SubSection != null && d.SubSection == null) 
                {
                    d.SubSection = d0.SubSection;
                    d.SubSectionName = d0.SubSectionName;
                }
                if (d0.Clause != null && d.Clause == null) 
                {
                    d.Clause = d0.Clause;
                    d.ClauseName = d0.ClauseName;
                }
                if (d0.Item != null && d.Item == null && ((d.SubItem != null || d.Indention != null))) 
                {
                    if (d0.Clause != null && d0.Clause != d.Clause) 
                    {
                    }
                    else 
                        d.Item = d0.Item;
                }
                if (d0.Form != null && d.Form == null) 
                {
                    d.Form = d0.Form;
                    d.FormName = d0.FormName;
                }
                if (d0.Table != null && d.Table == null) 
                {
                    d.Table = d0.Table;
                    d.TableName = d0.TableName;
                }
                if ((d0.SubItem != null && d.SubItem == null && d.Indention != null) && d.Item == d0.Item) 
                    d.SubItem = d0.SubItem;
            }
            if (res.Count == 1 && docPart != null && (res[0].Referent as DecreePartReferent).DocPart == null) 
                (res[0].Referent as DecreePartReferent).DocPart = docPart;
            if (((parts.Count > 2 && res.Count == 2 && parts[parts.Count - 2].DelimAfter) && parts[parts.Count - 2].Typ == parts[parts.Count - 1].Typ && res[1].Referent.Slots.Count == 1) && res[1].Morph.Case.IsGenitive && parts[parts.Count - 2].EndToken.Next.IsAnd) 
            {
                DecreePartReferent pp = res[0].Referent.Clone() as DecreePartReferent;
                string nam = Pullenti.Ner.Decree.Internal.PartToken._getAttrNameByTyp(parts[parts.Count - 1].Typ);
                Pullenti.Ner.Slot sl = res[1].Referent.FindSlot(nam, null, true);
                if (sl != null) 
                {
                    Pullenti.Ner.Slot sl1 = pp.FindSlot(nam, null, true);
                    if (sl1 != null) 
                    {
                        sl1.Value = sl.Value;
                        sl1.Tag = sl.Tag;
                        sl1.Count = sl.Count;
                        res[1].Referent = pp;
                    }
                }
            }
            tt = parts[i - 1].EndToken;
            DecreeReferent owner = defOwner;
            if (owner != null && !inChanges && parts[0].AnaforRef != null) 
            {
                for (Pullenti.Ner.Token tt3 = parts[0].BeginToken.Previous; tt3 != null; tt3 = tt3.Previous) 
                {
                    if (tt3.IsComma) 
                        continue;
                    if (tt3.GetReferent() is DecreeReferent) 
                        owner = tt3.GetReferent() as DecreeReferent;
                    break;
                }
            }
            Pullenti.Ner.Token te = tt.Next;
            if ((te != null && owner == null && te.IsChar('(')) && parts[0].Typ != Pullenti.Ner.Decree.Internal.PartToken.ItemType.Subprogram && parts[0].Typ != Pullenti.Ner.Decree.Internal.PartToken.ItemType.AddAgree) 
            {
                Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(te, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                if (br != null) 
                {
                    if (te.Next.Morph.Class.IsAdverb) 
                    {
                    }
                    else if (te.Next.GetReferent() is DecreeReferent) 
                    {
                        if (owner == null && te.Next.Next == br.EndToken) 
                        {
                            owner = te.Next.GetReferent() as DecreeReferent;
                            te = br.EndToken;
                        }
                    }
                    else 
                    {
                        string s = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(br, Pullenti.Ner.Core.GetTextAttr.KeepRegister);
                        if (s != null) 
                        {
                            Pullenti.Ner.ReferentToken rt = res[res.Count - 1];
                            rt.EndToken = br.EndToken;
                            te = br.EndToken.Next;
                        }
                    }
                }
            }
            if (te != null && te.IsCharOf(",;")) 
                te = te.Next;
            if (owner == null && (te is Pullenti.Ner.ReferentToken)) 
            {
                if ((((owner = te.GetReferent() as DecreeReferent))) != null) 
                    res[res.Count - 1].EndToken = te;
            }
            if (owner == null) 
            {
                for (j = 0; j < i; j++) 
                {
                    if ((((owner = parts[j].Decree))) != null) 
                        break;
                }
            }
            if (te != null && te.IsValue("К", null) && te.Next != null) 
            {
                if (te.Next.GetReferent() is DecreeReferent) 
                {
                    te = te.Next;
                    if (te.EndChar > res[res.Count - 1].EndChar) 
                        res[res.Count - 1].EndToken = te;
                    owner = te.GetReferent() as DecreeReferent;
                }
                else if ((owner != null && thisDec != null && thisDec.EndChar > te.EndChar) && (res[res.Count - 1].EndChar < thisDec.EndChar)) 
                {
                    if (thisDec.Typ != "ТИПОВОЙ ДОГОВОР") 
                        res[res.Count - 1].EndToken = thisDec.EndToken;
                }
            }
            if (owner == null && thisDec != null) 
            {
                Pullenti.Ner.Token tt0 = res[0].BeginToken;
                if (tt0.Previous != null && tt0.Previous.IsChar('(')) 
                    tt0 = tt0.Previous;
                if (tt0.Previous != null) 
                {
                    if ((((owner = tt0.Previous.GetReferent() as DecreeReferent))) != null) 
                    {
                        if (thisDec.Typ == owner.Typ0) 
                            thisDec = null;
                        else 
                            owner = null;
                    }
                }
            }
            if (owner == null && thisDec != null && thisDec.Real != null) 
                owner = thisDec.Real;
            if (owner != null && parts[0].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Subprogram && owner.Kind != DecreeKind.Program) 
                owner = null;
            if (owner != null && parts[0].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.AddAgree && owner.Kind != DecreeKind.Contract) 
                owner = null;
            DecreePartReferent ownerPaer = null;
            string locTyp = curTyp;
            if ((((thisDec == null || !thisDec.HasThisRef)) && !inChanges && locTyp == null) && owner == null) 
            {
                Pullenti.Ner.TextToken anaforRef = null;
                foreach (Pullenti.Ner.Decree.Internal.PartToken p in parts) 
                {
                    if ((((anaforRef = p.AnaforRef))) != null) 
                        break;
                }
                bool isChangeWordAfter = false;
                Pullenti.Ner.Token tt2 = res[res.Count - 1].EndToken.Next;
                if (tt2 != null) 
                {
                    if (((tt2.IsChar(':') || tt2.IsValue("ДОПОЛНИТЬ", null) || tt2.IsValue("СЛОВО", null)) || tt2.IsValue("ИСКЛЮЧИТЬ", null) || tt2.IsValue("ИЗЛОЖИТЬ", null)) || tt2.IsValue("СЧИТАТЬ", null) || tt2.IsValue("ПРИЗНАТЬ", null)) 
                        isChangeWordAfter = true;
                }
                tt2 = parts[0].BeginToken.Previous;
                if (tt2 != null) 
                {
                    if (((tt2.IsValue("ДОПОЛНИТЬ", null) || tt2.IsValue("ИСКЛЮЧИТЬ", null) || tt2.IsValue("ИЗЛОЖИТЬ", null)) || tt2.IsValue("СЧИТАТЬ", null) || tt2.IsValue("УСТАНОВЛЕННЫЙ", null)) || tt2.IsValue("ОПРЕДЕЛЕННЫЙ", null)) 
                        isChangeWordAfter = true;
                }
                int cou = 0;
                bool ugolDelo = false;
                int brackLevel = 0;
                Pullenti.Ner.Token bt = null;
                int coefBefore = 0;
                bool isOverBrr = false;
                if (parts[0].BeginToken.Previous != null && parts[0].BeginToken.Previous.IsChar('(')) 
                {
                    if (parts[parts.Count - 1].EndToken.Next != null && parts[parts.Count - 1].EndToken.Next.IsChar(')')) 
                    {
                        if (parts.Count == 1 && parts[0].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Appendix) 
                        {
                        }
                        else 
                        {
                            isOverBrr = true;
                            if (owner != null && _getDecree(parts[0].BeginToken.Previous.Previous) != null) 
                                owner = null;
                        }
                    }
                }
                for (tt = parts[0].BeginToken.Previous; tt != null; tt = tt.Previous,coefBefore++) 
                {
                    if (tt.IsNewlineAfter) 
                    {
                        coefBefore += 2;
                        if (((anaforRef == null && !isOverBrr && !ugolDelo) && !isChangeWordAfter && !isProgram) && !isAddAgree) 
                        {
                            if (thisDec == null) 
                            {
                                if (!tt.IsTableControlChar) 
                                {
                                    if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(tt.Next, false, false)) 
                                        break;
                                    if (Pullenti.Ner.Core.ComplexNumToken.TryParse(tt.Next, null, false, false) != null) 
                                        break;
                                    if (tt.IsCharOf(".;")) 
                                        break;
                                }
                            }
                        }
                    }
                    if (thisDec != null && thisDec.HasThisRef) 
                        break;
                    if (tt.IsTableControlChar) 
                        break;
                    if (tt.Morph.Class.IsPreposition) 
                    {
                        coefBefore--;
                        continue;
                    }
                    if (tt is Pullenti.Ner.TextToken) 
                    {
                        if (Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(tt, false, null, false)) 
                        {
                            brackLevel++;
                            continue;
                        }
                        if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(tt, false, false)) 
                        {
                            if (tt.IsChar('(') && tt == parts[0].BeginToken.Previous) 
                            {
                            }
                            else 
                            {
                                brackLevel--;
                                coefBefore--;
                            }
                            continue;
                        }
                    }
                    if (tt.IsNewlineBefore) 
                        brackLevel = 0;
                    if ((++cou) > 100) 
                    {
                        if (((ugolDelo || isProgram || isAddAgree) || anaforRef != null || thisDec != null) || isOverBrr) 
                        {
                            if (cou > 1000) 
                                break;
                        }
                        else if (isChangeWordAfter) 
                        {
                            if (cou > 250) 
                                break;
                        }
                        else 
                            break;
                    }
                    if (cou < 4) 
                    {
                        if (tt.IsValue("УГОЛОВНЫЙ", "КРИМІНАЛЬНИЙ") && tt.Next != null && tt.Next.IsValue("ДЕЛО", "СПРАВА")) 
                            ugolDelo = true;
                    }
                    if (tt.IsCharOf(".")) 
                    {
                        coefBefore += 50;
                        if (tt.IsNewlineAfter) 
                            coefBefore += 100;
                        continue;
                    }
                    if (brackLevel > 0) 
                        continue;
                    DecreeReferent dr = _getDecree(tt);
                    if (dr != null) 
                    {
                        if (dr.Kind == DecreeKind.Publisher) 
                        {
                            if ((tt.EndChar + 3) < parts[0].BeginChar) 
                                continue;
                        }
                        if (ugolDelo && ((dr.Name == "УГОЛОВНЫЙ КОДЕКС" || dr.Name == "КРИМІНАЛЬНИЙ КОДЕКС"))) 
                            coefBefore = 0;
                        if (dr.Kind == DecreeKind.Program) 
                        {
                            if (isProgram) 
                            {
                                bt = tt;
                                break;
                            }
                            else 
                                continue;
                        }
                        if (dr.Kind == DecreeKind.Contract) 
                        {
                            if (isAddAgree) 
                            {
                                bt = tt;
                                break;
                            }
                            else if (thisDec != null && ((dr.Typ == thisDec.Typ || dr.Typ0 == thisDec.Typ))) 
                            {
                                bt = tt;
                                break;
                            }
                            else 
                                continue;
                        }
                        if (thisDec != null) 
                        {
                            DecreePartReferent dpr = tt.GetReferent() as DecreePartReferent;
                            if (thisDec.Typ == dr.Typ || thisDec.Typ == dr.Typ0) 
                                thisDec.Real = dr;
                            else if (dr.Name != null && thisDec.Typ != null && dr.Name.StartsWith(thisDec.Typ.Substring(0, thisDec.Typ.Length - 1))) 
                            {
                                thisDec.Real = dr;
                                string[] words = dr.Name.Split(' ');
                                for (int ii = 1; ii < words.Length; ii++) 
                                {
                                    if (thisDec.EndToken.Next != null && thisDec.EndToken.Next.IsValue(words[ii], null)) 
                                        thisDec.EndToken = thisDec.EndToken.Next;
                                    else 
                                        break;
                                }
                            }
                            else if ((thisDec.HasOtherRef && dpr != null && dpr.Clause != null) && ((thisDec.Typ == "СТАТЬЯ" || thisDec.Typ == "СТАТТЯ"))) 
                            {
                                foreach (Pullenti.Ner.ReferentToken r in res) 
                                {
                                    DecreePartReferent dpr0 = r.Referent as DecreePartReferent;
                                    if (dpr0.Clause == null) 
                                    {
                                        dpr0.Clause = dpr.Clause;
                                        owner = (dpr0.Owner = dpr.Owner);
                                    }
                                }
                            }
                            else 
                                continue;
                        }
                        else if (isChangeWordAfter) 
                        {
                            if (owner == null) 
                                coefBefore = 0;
                            else if (owner == _getDecree(tt)) 
                                coefBefore = 0;
                        }
                        bt = tt;
                        break;
                    }
                    if (dr != null) 
                        continue;
                    if (tt.GetReferent() is DecreeChangeReferent) 
                        break;
                    DecreePartReferent dpr2 = tt.GetReferent() as DecreePartReferent;
                    if (dpr2 != null) 
                    {
                        if (thisDec != null) 
                        {
                            if (thisDec.Typ != dpr2.LocalTyp) 
                                continue;
                        }
                        bt = tt;
                        break;
                    }
                    Pullenti.Ner.Decree.Internal.DecreeToken dit = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(tt, null, false);
                    if (dit != null && dit.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) 
                    {
                        if (thisDec != null) 
                            continue;
                        if (dit.Chars.IsCapitalUpper || anaforRef != null) 
                        {
                            bt = tt;
                            break;
                        }
                    }
                }
                cou = 0;
                Pullenti.Ner.Token at = null;
                int coefAfter = 0;
                string alocTyp = null;
                Pullenti.Ner.Token tt0 = parts[parts.Count - 1].EndToken.Next;
                bool hasNewline = false;
                for (Pullenti.Ner.Token ttt = parts[parts.Count - 1].BeginToken; ttt.EndChar < parts[parts.Count - 1].EndChar; ttt = ttt.Next) 
                {
                    if (ttt.IsNewlineAfter) 
                        hasNewline = true;
                }
                for (tt = tt0; tt != null; tt = tt.Next,coefAfter++) 
                {
                    if (owner != null && coefAfter > 0) 
                        break;
                    if (tt.IsNewlineBefore) 
                        break;
                    if (tt.IsTableControlChar) 
                        break;
                    if (tt.IsValue("СМ", null)) 
                        break;
                    if (anaforRef != null) 
                        break;
                    if (thisDec != null) 
                    {
                        if (tt != tt0) 
                            break;
                        if (thisDec.Real != null) 
                            break;
                    }
                    if (Pullenti.Ner.Instrument.Internal.InstrToken._checkEntered(tt) != null) 
                        break;
                    if ((tt is Pullenti.Ner.TextToken) && (tt as Pullenti.Ner.TextToken).IsPureVerb) 
                        break;
                    if (tt.Morph.Class.IsPreposition || tt.IsCommaAnd) 
                    {
                        coefAfter--;
                        continue;
                    }
                    if (tt.Morph.Class.Equals(Pullenti.Morph.MorphClass.Verb)) 
                        break;
                    if (Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(tt, false, null, false)) 
                        break;
                    List<Pullenti.Ner.Decree.Internal.PartToken> pts = Pullenti.Ner.Decree.Internal.PartToken.TryAttachList(tt, false, 40);
                    if (pts != null) 
                    {
                        tt = pts[pts.Count - 1].EndToken;
                        coefAfter--;
                        Pullenti.Ner.Token ttnn = tt.Next;
                        if (ttnn != null && ttnn.IsChar('.')) 
                            ttnn = ttnn.Next;
                        Pullenti.Ner.Decree.Internal.DecreeToken dit = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(ttnn, null, false);
                        if (dit != null && dit.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) 
                        {
                            locTyp = dit.Value;
                            break;
                        }
                        continue;
                    }
                    if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(tt, false, false)) 
                    {
                        Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(tt, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                        if (br != null) 
                        {
                            coefAfter--;
                            tt = br.EndToken;
                            continue;
                        }
                    }
                    if ((++cou) > 100) 
                        break;
                    if (cou > 1 && hasNewline) 
                        break;
                    if (tt.IsCharOf(".")) 
                    {
                        coefAfter += 50;
                        if (tt.IsNewlineAfter) 
                            coefAfter += 100;
                        continue;
                    }
                    if (tt.GetReferent() is DecreeChangeReferent) 
                        break;
                    DecreeReferent dr = tt.GetReferent() as DecreeReferent;
                    if (dr != null && dr.Kind != DecreeKind.Publisher) 
                    {
                        if (dr.Kind == DecreeKind.Program) 
                        {
                            if (isProgram) 
                            {
                                at = tt;
                                break;
                            }
                            else 
                                continue;
                        }
                        if (dr.Kind == DecreeKind.Contract) 
                        {
                            if (isAddAgree) 
                            {
                                at = tt;
                                break;
                            }
                            else 
                                continue;
                        }
                        at = tt;
                        break;
                    }
                    if (isProgram || isAddAgree) 
                        break;
                    if (dr != null) 
                        continue;
                    Pullenti.Ner.Token tte2 = _checkOtherTyp(tt, tt == tt0);
                    if (tte2 != null) 
                    {
                        at = tte2;
                        if (tt == tt0 && thisDec != null && thisDec.Real == null) 
                        {
                            if (thisDec.Typ == (at.Tag as string)) 
                                at = null;
                            else 
                                thisDec = null;
                        }
                        break;
                    }
                }
                if (bt != null && at != null) 
                {
                    if (coefBefore < coefAfter) 
                        at = null;
                    else if ((bt is Pullenti.Ner.ReferentToken) && (at is Pullenti.Ner.TextToken)) 
                        at = null;
                    else 
                        bt = null;
                }
                if (owner == null) 
                {
                    if (at != null) 
                    {
                        owner = _getDecree(at);
                        if (at is Pullenti.Ner.TextToken) 
                        {
                            if (at.Tag is string) 
                                locTyp = at.Tag as string;
                            else 
                                locTyp = (at as Pullenti.Ner.TextToken).Lemma;
                        }
                    }
                    else if (bt != null) 
                    {
                        owner = _getDecree(bt);
                        ownerPaer = bt.GetReferent() as DecreePartReferent;
                        if (ownerPaer != null && locTyp == null) 
                            locTyp = ownerPaer.LocalTyp;
                    }
                }
                else if (coefAfter == 0 && at != null) 
                {
                    DecreeReferent owner1 = _getDecree(at);
                    if (owner1 != null) 
                        owner = owner1;
                }
                else if (coefBefore == 0 && bt != null) 
                {
                    owner = _getDecree(bt);
                    ownerPaer = bt.GetReferent() as DecreePartReferent;
                    if (ownerPaer != null && locTyp == null) 
                        locTyp = ownerPaer.LocalTyp;
                }
                if (((bt != null && parts.Count == 1 && parts[0].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.DocPart) && (bt.GetReferent() is DecreePartReferent) && (bt.GetReferent() as DecreePartReferent).Clause != null) && res.Count == 1 && owner == (bt.GetReferent() as DecreePartReferent).Owner) 
                {
                    foreach (Pullenti.Ner.Slot s in res[0].Referent.Slots) 
                    {
                        if (s.TypeName == DecreePartReferent.ATTR_DOCPART) 
                            s.TypeName = DecreePartReferent.ATTR_PART;
                    }
                    (res[0].Referent as DecreePartReferent).AddHighLevelInfo(bt.GetReferent() as DecreePartReferent);
                }
            }
            if (owner == null) 
            {
                if (thisDec == null && locTyp == null) 
                {
                    if ((parts.Count == 1 && parts[0].Values.Count == 1 && parts[0].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Appendix) && parts[0].BeginToken.Chars.IsCapitalUpper) 
                    {
                    }
                    else if ((parts[0].BeginToken.Previous != null && parts[0].BeginToken.Previous.IsChar('(') && parts[parts.Count - 1].EndToken.Next != null) && parts[parts.Count - 1].EndToken.Next.IsChar(')')) 
                    {
                        if (parts[0].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Page) 
                            return null;
                    }
                    else if (inChanges) 
                    {
                    }
                    else if (parts.Count > 1 && ((parts[0].Values.Count > 0 || parts[1].Values.Count > 0))) 
                    {
                    }
                    else if (((parts[0].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Clause || parts[0].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Item || parts[0].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Appendix)) && parts[0].Values.Count > 0) 
                    {
                        if (parts[0].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Item && parts[0].BeginToken.IsValue("П", null)) 
                        {
                            if (!(parts[0].Values[0].BeginToken is Pullenti.Ner.NumberToken)) 
                                return null;
                            if (!parts[0].IsWhitespaceBefore) 
                            {
                                if (!parts[0].BeginToken.Previous.IsCharOf("(,")) 
                                    return null;
                            }
                        }
                    }
                    else 
                        return null;
                }
                foreach (Pullenti.Ner.ReferentToken r in res) 
                {
                    DecreePartReferent dr = r.Referent as DecreePartReferent;
                    if (thisDec != null) 
                    {
                        dr.LocalTyp = thisDec.Typ;
                        if (thisDec.BeginChar > r.EndChar && r == res[res.Count - 1]) 
                            r.EndToken = thisDec.EndToken;
                    }
                    else if (locTyp != null) 
                    {
                        if (locTyp == "СТАТЬЯ" && dr.Clause != null) 
                        {
                        }
                        else if (locTyp == "ГЛАВА" && dr.Chapter != null) 
                        {
                        }
                        else if (locTyp == "ПАРАГРАФ" && dr.Paragraph != null) 
                        {
                        }
                        else if (locTyp == "ЧАСТЬ" && dr.Part != null) 
                        {
                        }
                        else 
                        {
                            dr.LocalTyp = locTyp;
                            if (r == res[res.Count - 1] && !r.IsNewlineAfter) 
                            {
                                Pullenti.Ner.Token ttt1 = r.EndToken.Next;
                                if (ttt1 != null && ttt1.IsComma) 
                                    ttt1 = ttt1.Next;
                                Pullenti.Ner.Token at = _checkOtherTyp(ttt1, true);
                                if (at != null && (at.Tag as string) == locTyp) 
                                    r.EndToken = at;
                            }
                        }
                    }
                }
            }
            else 
                foreach (Pullenti.Ner.ReferentToken r in res) 
                {
                    DecreePartReferent dr = r.Referent as DecreePartReferent;
                    dr.Owner = owner;
                    if (thisDec != null && thisDec.Real == owner) 
                    {
                        if (thisDec.BeginChar > r.EndChar && r == res[res.Count - 1]) 
                            r.EndToken = thisDec.EndToken;
                    }
                }
            if (res.Count > 0) 
            {
                Pullenti.Ner.ReferentToken rt = res[res.Count - 1];
                if (curTypToken != null && curTypToken.EndChar > rt.EndChar) 
                    rt.EndToken = curTypToken;
                tt = rt.EndToken.Next;
                if (owner != null && tt != null && tt.GetReferent() == owner) 
                {
                    rt.EndToken = tt;
                    tt = tt.Next;
                }
                if (tt != null && ((tt.IsHiphen || tt.IsChar(':')))) 
                    tt = tt.Next;
                Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(tt, (isProgram ? Pullenti.Ner.Core.BracketParseAttr.CanBeManyLines : Pullenti.Ner.Core.BracketParseAttr.No), 100);
                if (br != null && ((tt.IsNewlineBefore || br.OpenChar == '('))) 
                    br = null;
                if (br != null) 
                {
                    bool ok = true;
                    if (br.OpenChar == '(') 
                    {
                        if (parts[0].Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Subprogram) 
                            ok = false;
                        else if (Pullenti.Ner.Decree.Internal.PartToken.TryAttach(tt.Next, null, false, false) != null) 
                            ok = false;
                        else 
                            for (Pullenti.Ner.Token ttt = tt.Next; ttt != null && (ttt.EndChar < br.EndChar); ttt = ttt.Next) 
                            {
                                if (ttt == tt.Next && tt.Next.Morph.Class.IsAdverb) 
                                    ok = false;
                                if ((ttt.GetReferent() is DecreeReferent) || (ttt.GetReferent() is DecreePartReferent)) 
                                    ok = false;
                                if (ttt.IsValue("РЕДАКЦИЯ", null) && ttt == br.EndToken.Previous) 
                                    ok = false;
                            }
                    }
                    if (br.EndToken.Next != null) 
                    {
                        if (br.EndToken.Next.IsValue("ЗАМЕНИТЬ", null) || br.EndToken.Next.IsValue("ИСКЛЮЧИТЬ", null)) 
                            ok = false;
                    }
                    if (ok && (tt.Next.GetReferent() is DecreeReferent)) 
                        ok = false;
                    if (ok && (tt.Next is Pullenti.Ner.ReferentToken) && tt.IsChar('<')) 
                        ok = false;
                    if (ok) 
                    {
                        string s = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(br, Pullenti.Ner.Core.GetTextAttr.KeepRegister);
                        if (s != null) 
                        {
                            if (br.OpenChar == '(') 
                                (rt.Referent as DecreePartReferent).Description = s;
                            else 
                                (rt.Referent as DecreePartReferent).AddName(parts[parts.Count - 1], br);
                            rt.EndToken = br.EndToken;
                            if ((rt.EndToken.Next is Pullenti.Ner.ReferentToken) && rt.EndToken.Next.GetReferent() == owner) 
                                rt.EndToken = rt.EndToken.Next;
                        }
                    }
                }
                else if ((isProgram && parts[0].Values.Count > 0 && tt != null) && tt.IsTableControlChar && Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(tt.Next)) 
                {
                    for (Pullenti.Ner.Token tt1 = tt.Next; tt1 != null; tt1 = tt1.Next) 
                    {
                        if (tt1.IsTableControlChar) 
                        {
                            string s = Pullenti.Ner.Core.MiscHelper.GetTextValue(tt.Next, tt1.Previous, Pullenti.Ner.Core.GetTextAttr.KeepRegister);
                            if (s != null) 
                            {
                                (rt.Referent as DecreePartReferent).Description = s;
                                rt.EndToken = tt1;
                            }
                            break;
                        }
                        else if (tt1.IsNewlineBefore) 
                            break;
                    }
                }
                if (thisDec != null) 
                {
                    if (thisDec.EndChar > res[res.Count - 1].EndChar) 
                    {
                        if (thisDec.Typ != "ТИПОВОЙ ДОГОВОР") 
                            res[res.Count - 1].EndToken = thisDec.EndToken;
                    }
                }
            }
            if (ownerPaer != null && thisDec == null) 
            {
                for (int ii = 0; ii < res.Count; ii++) 
                {
                    (res[ii].Referent as DecreePartReferent).AddHighLevelInfo((ii == 0 ? ownerPaer : res[ii - 1].Referent as DecreePartReferent));
                }
            }
            if (res.Count == 1 && (res[0].Referent as DecreePartReferent).Description == null) 
            {
                if ((res[0].BeginToken.Previous != null && res[0].BeginToken.Previous.IsChar('(') && res[0].EndToken.Next != null) && res[0].EndToken.Next.IsChar(')')) 
                {
                    if (Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(res[0].BeginToken.Previous.Previous, false, null, false)) 
                    {
                        Pullenti.Ner.Token beg = null;
                        for (tt = res[0].BeginToken.Previous.Previous.Previous; tt != null; tt = tt.Previous) 
                        {
                            if (tt.IsNewlineAfter) 
                                break;
                            if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(tt, false, false)) 
                            {
                                Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(tt, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                                if (br != null && ((br.EndChar + 10) < res[0].BeginChar)) 
                                    break;
                                if (tt.Next.Chars.IsLetter && !tt.Next.Chars.IsAllLower) 
                                    beg = tt;
                            }
                        }
                        if (beg != null) 
                        {
                            if (Pullenti.Ner.Core.BracketHelper.IsBracket(res[0].BeginToken.Previous.Previous, true) && parts.Count == 1) 
                                (res[0].Referent as DecreePartReferent).AddName(parts[0], new Pullenti.Ner.MetaToken(beg, res[0].BeginToken.Previous.Previous));
                            else 
                                (res[0].Referent as DecreePartReferent).Description = Pullenti.Ner.Core.MiscHelper.GetTextValue(beg, res[0].BeginToken.Previous.Previous, Pullenti.Ner.Core.GetTextAttr.KeepRegister);
                            res[0].BeginToken = beg;
                            res[0].EndToken = res[0].EndToken.Next;
                        }
                    }
                }
            }
            if (isProgram) 
            {
                for (i = res.Count - 1; i >= 0; i--) 
                {
                    DecreePartReferent pa = res[i].Referent as DecreePartReferent;
                    if (pa.Subprogram == null) 
                        continue;
                    if (pa.Owner == null || pa.Owner.Kind != DecreeKind.Program) 
                        res.RemoveAt(i);
                }
            }
            if (isAddAgree) 
            {
                for (i = res.Count - 1; i >= 0; i--) 
                {
                    DecreePartReferent pa = res[i].Referent as DecreePartReferent;
                    if (pa.Addagree == null) 
                        continue;
                    if (pa.Owner == null || pa.Owner.Kind != DecreeKind.Contract) 
                        res.RemoveAt(i);
                }
            }
            if (!inChanges) 
            {
                for (i = res.Count - 1; i >= 0; i--) 
                {
                    DecreePartReferent pa = res[i].Referent as DecreePartReferent;
                    if (pa.NameAsItem == null && pa.Formula == null) 
                        continue;
                    if (pa.Slots.Count == 2 && pa.Owner == null) 
                        res.RemoveAt(i);
                }
            }
            if (res.Count == 1 && parts.Count == 1 && parts[0].AddNames != null) 
            {
                res[0].EndToken = parts[0].AddNames[0].BeginToken.Previous;
                foreach (Pullenti.Ner.MetaToken br in parts[0].AddNames) 
                {
                    DecreePartReferent dp = res[0].Referent.Clone() as DecreePartReferent;
                    dp.AddName(parts[0], br);
                    res.Add(new Pullenti.Ner.ReferentToken(dp, br.BeginToken, br.EndToken));
                }
            }
            foreach (Pullenti.Ner.ReferentToken r in res) 
            {
                DecreePartReferent p = r.Referent as DecreePartReferent;
                if (((p.TableItem != null || p.TableSubItem != null)) && p.Item != null && p.TableRow == null) 
                {
                    p.TableRow = p.Item;
                    p.Item = null;
                }
            }
            List<Pullenti.Ner.MetaToken> res1 = new List<Pullenti.Ner.MetaToken>();
            for (i = 0; i < res.Count; i++) 
            {
                List<DecreePartReferent> li = new List<DecreePartReferent>();
                for (j = i; j < res.Count; j++) 
                {
                    if (res[j].BeginToken != res[i].BeginToken) 
                        break;
                    else 
                        li.Add(res[j].Referent as DecreePartReferent);
                }
                Pullenti.Ner.Token et;
                if (j < res.Count) 
                    et = res[j].BeginToken.Previous;
                else 
                    et = res[res.Count - 1].EndToken;
                while (et != null && et.BeginChar > res[i].BeginChar) 
                {
                    if ((et.IsChar(',') || et.Morph.Class.IsConjunction || et.IsHiphen) || et.IsValue("ПО", null)) 
                        et = et.Previous;
                    else if (Pullenti.Ner.Core.MiscHelper.CheckNumberPrefix(et) != null) 
                        et = et.Previous;
                    else 
                        break;
                }
                res1.Add(new Pullenti.Ner.MetaToken(res[i].BeginToken, (et != null && et.BeginChar >= res[i].BeginChar ? et : res[i].BeginToken)) { Tag = li, Morph = res[i].Morph });
                i = j - 1;
            }
            return res1;
        }
        class ThisDecree : Pullenti.Ner.MetaToken
        {
            public ThisDecree(Pullenti.Ner.Token b, Pullenti.Ner.Token e) : base(b, e, null)
            {
            }
            public string Typ;
            public bool HasThisRef;
            public bool HasOtherRef;
            public Pullenti.Ner.Decree.DecreeReferent Real;
            public override string ToString()
            {
                return string.Format("{0} ({1})", Typ ?? "?", (HasThisRef ? "This" : (HasOtherRef ? "Other" : "?")));
            }
            public bool CheckDecree(Pullenti.Ner.Decree.DecreeReferent r)
            {
                if (r == null) 
                    return false;
                if (Real == r) 
                    return true;
                if (Typ != null) 
                {
                    if (r.Typ == Typ || r.Typ0 == Typ) 
                        return true;
                    if (r.Kind == Pullenti.Ner.Decree.DecreeKind.Kodex && Typ == "КОДЕКС") 
                        return true;
                    foreach (Pullenti.Ner.Slot s in r.Slots) 
                    {
                        if (s.TypeName == Pullenti.Ner.Decree.DecreeReferent.ATTR_NAME) 
                        {
                            string n = s.Value as string;
                            if (n.StartsWith(Typ, StringComparison.OrdinalIgnoreCase)) 
                                return true;
                            Pullenti.Ner.AnalysisResult arr = Pullenti.Ner.ProcessorService.EmptyProcessor.Process(new Pullenti.Ner.SourceOfAnalysis(n), null, null);
                            if (arr != null && arr.FirstToken != null) 
                            {
                                if (arr.FirstToken.IsValue(Typ, null)) 
                                    return true;
                                if (arr.FirstToken.Next != null) 
                                {
                                    if (arr.FirstToken.Next.IsValue(Typ, null)) 
                                        return true;
                                }
                            }
                        }
                    }
                }
                return false;
            }
            public static ThisDecree TryAttachBack(Pullenti.Ner.Token t, Pullenti.Ner.Decree.Internal.DecreeToken baseTyp)
            {
                if (t == null) 
                    return null;
                Pullenti.Ner.Token ukaz = null;
                for (Pullenti.Ner.Token tt = t; tt != null; tt = tt.Previous) 
                {
                    if (tt.IsCharOf(",") || tt.Morph.Class.IsPreposition || tt.Morph.Class.IsConjunction) 
                        continue;
                    if ((((((tt.IsValue("ОПРЕДЕЛЕННЫЙ", "ПЕВНИЙ") || tt.IsValue("ЗАДАННЫЙ", "ЗАДАНИЙ") || tt.IsValue("ПРЕДУСМОТРЕННЫЙ", "ПЕРЕДБАЧЕНИЙ")) || tt.IsValue("УКАЗАННЫЙ", "ЗАЗНАЧЕНИЙ") || tt.IsValue("ПЕРЕЧИСЛЕННЫЙ", "ПЕРЕРАХОВАНИЙ")) || tt.IsValue("ОПРЕДЕЛИТЬ", "ВИЗНАЧИТИ") || tt.IsValue("ОПРЕДЕЛЯТЬ", null)) || tt.IsValue("ЗАДАВАТЬ", "ЗАДАВАТИ") || tt.IsValue("ПРЕДУСМАТРИВАТЬ", "ПЕРЕДБАЧАТИ")) || tt.IsValue("УКАЗЫВАТЬ", "ВКАЗУВАТИ") || tt.IsValue("УКАЗАТЬ", "ВКАЗАТИ")) || tt.IsValue("СИЛА", "ЧИННІСТЬ")) 
                    {
                        ukaz = tt;
                        continue;
                    }
                    if (tt == t) 
                        continue;
                    Pullenti.Ner.Token ttt = Pullenti.Ner.Decree.Internal.DecreeToken.IsKeyword(tt, false);
                    if (tt != ttt || !(tt is Pullenti.Ner.TextToken)) 
                        break;
                    if (ttt.IsValue("УСЛОВИЕ", null) || ttt.IsValue("ПОЛОЖЕНИЕ", null) || ttt.IsValue("ОПРЕДЕЛЕНИЕ", null)) 
                        continue;
                    if (Pullenti.Ner.Decree.Internal.PartToken.TryAttach(tt, null, false, true) != null) 
                        break;
                    if (ttt.IsValue("ПОРЯДОК", null) && ukaz != null) 
                        return null;
                    ThisDecree res = new ThisDecree(tt, tt);
                    res.Typ = (tt as Pullenti.Ner.TextToken).Lemma;
                    t = tt.Previous;
                    if (t != null && ((t.Morph.Class.IsAdjective || t.Morph.Class.IsPronoun))) 
                    {
                        if (t.IsValue("НАСТОЯЩИЙ", "СПРАВЖНІЙ") || t.IsValue("ТЕКУЩИЙ", "ПОТОЧНИЙ") || t.IsValue("ДАННЫЙ", "ДАНИЙ")) 
                        {
                            res.HasThisRef = true;
                            res.BeginToken = t;
                        }
                        else if ((t.IsValue("ЭТОТ", "ЦЕЙ") || t.IsValue("ВЫШЕУКАЗАННЫЙ", "ВИЩЕВКАЗАНИЙ") || t.IsValue("УКАЗАННЫЙ", "ЗАЗНАЧЕНИЙ")) || t.IsValue("НАЗВАННЫЙ", "НАЗВАНИЙ")) 
                        {
                            res.HasOtherRef = true;
                            res.BeginToken = t;
                        }
                    }
                    if (!res.HasThisRef && tt.IsNewlineAfter) 
                        return null;
                    if (baseTyp != null && baseTyp.Value == res.Typ) 
                        res.HasThisRef = true;
                    return res;
                }
                if (ukaz != null) 
                {
                    if (baseTyp != null && baseTyp.Value != null && ((baseTyp.Value.Contains("ДОГОВОР") || baseTyp.Value.Contains("ДОГОВІР")))) 
                        return new ThisDecree(ukaz, ukaz) { HasThisRef = true, Typ = baseTyp.Value };
                }
                return null;
            }
            public static ThisDecree TryAttach(Pullenti.Ner.Decree.Internal.PartToken dtok, Pullenti.Ner.Decree.Internal.DecreeToken baseTyp)
            {
                if (dtok.Decree != null) 
                    return new ThisDecree(dtok.EndToken, dtok.EndToken) { Real = dtok.Decree };
                Pullenti.Ner.Token t = dtok.EndToken.Next;
                if (t == null) 
                    return null;
                if (t.IsNewlineBefore) 
                {
                    if (t.Chars.IsCyrillicLetter && t.Chars.IsAllLower) 
                    {
                    }
                    else 
                        return null;
                }
                Pullenti.Ner.Token t0 = t;
                if (t.IsChar('.') && t.Next != null && !t.IsNewlineAfter) 
                {
                    if (dtok.IsNewlineBefore) 
                        return null;
                    t = t.Next;
                }
                if (t.IsValue("К", null) && t.Next != null) 
                {
                    t = t.Next;
                    if (t.GetReferent() is Pullenti.Ner.Decree.DecreeReferent) 
                        return new ThisDecree(t, t) { Real = t.GetReferent() as Pullenti.Ner.Decree.DecreeReferent };
                }
                if (t != null && (t.GetReferent() is Pullenti.Ner.Decree.DecreeReferent)) 
                    return null;
                if (t.IsValue("НОВЫЙ", null)) 
                    return null;
                Pullenti.Ner.Token tt = Pullenti.Ner.Decree.Internal.DecreeToken.IsKeyword(t, false);
                bool br = false;
                if (tt == null && Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t, true, false)) 
                {
                    tt = Pullenti.Ner.Decree.Internal.DecreeToken.IsKeyword(t.Next, false);
                    if ((tt is Pullenti.Ner.TextToken) && Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(tt.Next, false, null, false)) 
                        br = true;
                    else 
                        tt = null;
                }
                if (!(tt is Pullenti.Ner.TextToken)) 
                {
                    if ((tt is Pullenti.Ner.ReferentToken) && (tt.GetReferent() is Pullenti.Ner.Decree.DecreeReferent)) 
                        return new ThisDecree(t, tt) { Real = tt.GetReferent() as Pullenti.Ner.Decree.DecreeReferent };
                    return null;
                }
                if (tt.Chars.IsAllLower) 
                {
                    if (Pullenti.Ner.Decree.Internal.DecreeToken.IsKeyword(tt, true) != null) 
                    {
                        if (tt != t && t.Chars.IsCapitalUpper) 
                        {
                        }
                        else 
                            return null;
                    }
                }
                if (!(t is Pullenti.Ner.TextToken)) 
                    return null;
                ThisDecree res = new ThisDecree(t0, (br ? tt.Next : tt));
                res.Typ = (tt as Pullenti.Ner.TextToken).Lemma;
                if (tt.Previous is Pullenti.Ner.TextToken) 
                {
                    Pullenti.Ner.Token tt1 = tt.Previous;
                    Pullenti.Morph.MorphClass mc = tt1.GetMorphClassInDictionary();
                    if (mc.IsAdjective && !mc.IsVerb && !tt1.IsValue("НАСТОЯЩИЙ", "СПРАВЖНІЙ")) 
                    {
                        Pullenti.Ner.Core.NounPhraseToken nnn = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt1, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                        if (nnn != null) 
                            res.Typ = nnn.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false);
                        if (tt1.Previous is Pullenti.Ner.TextToken) 
                        {
                            tt1 = tt1.Previous;
                            mc = tt1.GetMorphClassInDictionary();
                            if (mc.IsAdjective && !mc.IsVerb && !tt1.IsValue("НАСТОЯЩИЙ", "СПРАВЖНІЙ")) 
                            {
                                nnn = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt1, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                                if (nnn != null) 
                                    res.Typ = nnn.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false);
                            }
                        }
                    }
                }
                if (tt.IsChar('.') && (tt.Previous is Pullenti.Ner.TextToken)) 
                    res.Typ = (tt.Previous as Pullenti.Ner.TextToken).Lemma;
                if (t.Morph.Class.IsAdjective || t.Morph.Class.IsPronoun) 
                {
                    if (t.IsValue("НАСТОЯЩИЙ", "СПРАВЖНІЙ") || t.IsValue("ТЕКУЩИЙ", "ПОТОЧНИЙ") || t.IsValue("ДАННЫЙ", "ДАНИЙ")) 
                        res.HasThisRef = true;
                    else if ((t.IsValue("ЭТОТ", "ЦЕЙ") || t.IsValue("ВЫШЕУКАЗАННЫЙ", "ВИЩЕВКАЗАНИЙ") || t.IsValue("УКАЗАННЫЙ", "ЗАЗНАЧЕНИЙ")) || t.IsValue("НАЗВАННЫЙ", "НАЗВАНИЙ")) 
                        res.HasOtherRef = true;
                }
                if (!tt.IsNewlineAfter && !res.HasThisRef) 
                {
                    Pullenti.Ner.Decree.Internal.DecreeToken dt = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(tt.Next, null, false);
                    if (dt != null && dt.Typ != Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Misc) 
                    {
                        if (dt.Typ != Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Name && dt.Typ != Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Org && dt.Typ != Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Terr) 
                            return null;
                    }
                }
                if (baseTyp != null && baseTyp.Value == res.Typ) 
                    res.HasThisRef = true;
                return res;
            }
        }

        /// <summary>
        /// Имя анализатора ("DECREE")
        /// </summary>
        public const string ANALYZER_NAME = "DECREE";
        public override string Name
        {
            get
            {
                return ANALYZER_NAME;
            }
        }
        public override string Caption
        {
            get
            {
                return "Законы и указы";
            }
        }
        public override string Description
        {
            get
            {
                return "Законы, указы, постановления, распоряжения и т.п.";
            }
        }
        public override Pullenti.Ner.Analyzer Clone()
        {
            return new DecreeAnalyzer();
        }
        public override ICollection<Pullenti.Ner.Metadata.ReferentClass> TypeSystem
        {
            get
            {
                return new Pullenti.Ner.Metadata.ReferentClass[] {Pullenti.Ner.Decree.Internal.MetaDecree.GlobalMeta, Pullenti.Ner.Decree.Internal.MetaDecreePart.GlobalMeta, Pullenti.Ner.Decree.Internal.MetaDecreeChange.GlobalMeta, Pullenti.Ner.Decree.Internal.MetaDecreeChangeValue.GlobalMeta};
            }
        }
        public override Dictionary<string, byte[]> Images
        {
            get
            {
                Dictionary<string, byte[]> res = new Dictionary<string, byte[]>();
                res.Add(Pullenti.Ner.Decree.Internal.MetaDecree.DecreeImageId, Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("decree.png"));
                res.Add(Pullenti.Ner.Decree.Internal.MetaDecree.StandadrImageId, Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("decreestd.png"));
                res.Add(Pullenti.Ner.Decree.Internal.MetaDecreePart.PartImageId, Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("part.png"));
                res.Add(Pullenti.Ner.Decree.Internal.MetaDecreePart.PartLocImageId, Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("document_into.png"));
                res.Add(Pullenti.Ner.Decree.Internal.MetaDecree.PublishImageId, Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("publish.png"));
                res.Add(Pullenti.Ner.Decree.Internal.MetaDecreeChange.ImageId, Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("decreechange.png"));
                res.Add(Pullenti.Ner.Decree.Internal.MetaDecreeChangeValue.ImageId, Pullenti.Ner.Core.Internal.ResourceHelper.GetBytes("decreechangevalue.png"));
                return res;
            }
        }
        public override IEnumerable<string> UsedExternObjectTypes
        {
            get
            {
                return new string[] {Pullenti.Ner.Date.DateReferent.OBJ_TYPENAME, Pullenti.Ner.Geo.GeoReferent.OBJ_TYPENAME, Pullenti.Ner.Org.OrganizationReferent.OBJ_TYPENAME, Pullenti.Ner.Person.PersonReferent.OBJ_TYPENAME};
            }
        }
        public override Pullenti.Ner.Referent CreateReferent(string type)
        {
            if (type == DecreeReferent.OBJ_TYPENAME) 
                return new DecreeReferent();
            if (type == DecreePartReferent.OBJ_TYPENAME) 
                return new DecreePartReferent();
            if (type == DecreeChangeReferent.OBJ_TYPENAME) 
                return new DecreeChangeReferent();
            if (type == DecreeChangeValueReferent.OBJ_TYPENAME) 
                return new DecreeChangeValueReferent();
            return null;
        }
        public override int ProgressWeight
        {
            get
            {
                return 10;
            }
        }
        public static Pullenti.Ner.Core.AnalyzerData GetData(Pullenti.Ner.Token t)
        {
            if (t == null) 
                return null;
            return t.Kit.GetAnalyzerDataByAnalyzerName(ANALYZER_NAME);
        }
        public override void Process(Pullenti.Ner.Core.AnalysisKit kit)
        {
            Pullenti.Ner.Core.AnalyzerData ad = kit.GetAnalyzerData(this);
            Pullenti.Ner.Decree.Internal.DecreeToken baseTyp = null;
            Pullenti.Ner.Referent ref0 = null;
            for (Pullenti.Ner.Token t = kit.FirstToken; t != null; t = t.Next) 
            {
                Pullenti.Ner.Referent r = t.GetReferent();
                if (r == null) 
                    continue;
                if (!(r is Pullenti.Ner.Org.OrganizationReferent)) 
                    continue;
                Pullenti.Ner.ReferentToken rt = t as Pullenti.Ner.ReferentToken;
                if (t != null && Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(rt.BeginToken, true, false) && Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(rt.EndToken, true, null, false)) 
                {
                    t = kit.DebedToken(rt);
                    Pullenti.Ner.ReferentToken rt1 = new Pullenti.Ner.ReferentToken(rt.Referent, rt.BeginToken.Next, rt.EndToken.Previous);
                    if (rt.Referent.Occurrence.Count == 0) 
                        rt1.Referent = ad.RegisterReferent(rt.Referent);
                    kit.EmbedToken(rt1);
                    t = rt1;
                    continue;
                }
                if (!rt.BeginToken.Chars.IsAllUpper || rt.BeginToken.LengthChar > 4) 
                    continue;
                Pullenti.Ner.Decree.Internal.DecreeToken dtr = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(rt.BeginToken, null, false);
                if (dtr == null || dtr.TypKind != DecreeKind.Kodex) 
                    continue;
                if (rt.BeginToken == rt.EndToken) 
                {
                }
                else if (rt.BeginToken.Next == rt.EndToken && (rt.EndToken.GetReferent() is Pullenti.Ner.Geo.GeoReferent)) 
                {
                }
                else 
                    continue;
                int cou = 0;
                for (Pullenti.Ner.Token tt = t.Previous; tt != null; tt = tt.Previous) 
                {
                    Pullenti.Ner.Decree.Internal.PartToken pt = Pullenti.Ner.Decree.Internal.PartToken.TryAttach(tt, null, false, false);
                    if (pt != null && pt.Values.Count > 0) 
                    {
                        t = kit.DebedToken(rt);
                        break;
                    }
                    if ((++cou) > 10) 
                        break;
                }
            }
            List<string> keywords = new List<string>(new string[] {"ИЗЛОЖИТЬ", "ВВЕСТИ", "ДОПОЛНИТЬ", "УДАЛИТЬ", "РЕДАКЦИИ"});
            for (Pullenti.Ner.Token t = kit.FirstToken; t != null; t = t.Next) 
            {
                Pullenti.Ner.TextToken tt = t as Pullenti.Ner.TextToken;
                if (tt == null) 
                    continue;
                if (tt.Term == "СТАТЮ" || tt.Term == "СТАТИ" || tt.Term == "СТАТЯ") 
                {
                    if (tt.Next is Pullenti.Ner.NumberToken) 
                    {
                        tt.CorrectWord(tt.Term.Substring(0, 4) + "Ь" + tt.Term.Substring(tt.Term.Length - 1));
                        continue;
                    }
                }
                if (tt.Term == "СТАТЬЮ") 
                {
                    tt.Morph.RemoveItems(Pullenti.Morph.MorphCase.Accusative);
                    continue;
                }
                if (tt.Term == "СРОКИ" && tt.Previous != null && tt.Previous.IsValue("ПОСЛЕ", null)) 
                {
                    tt.CorrectWord("СТРОКИ");
                    continue;
                }
                if (tt.LengthChar < 6) 
                    continue;
                Pullenti.Morph.MorphClass mc = tt.GetMorphClassInDictionary();
                if (!mc.IsUndefined) 
                    continue;
                if (tt.Term.StartsWith("РЕД") && t.Previous != null && t.Previous.IsValue("СЛЕДУЮЩИЙ", null)) 
                {
                    tt.CorrectWord("РЕДАКЦИИ");
                    continue;
                }
                List<string> vars = Pullenti.Morph.MorphologyService.CorrectWordEx(tt.Term, null);
                if (vars == null || vars.Count == 0) 
                    continue;
                foreach (string v in vars) 
                {
                    if (keywords.Contains(v)) 
                    {
                        tt.CorrectWord(v);
                        break;
                    }
                }
            }
            Pullenti.Ner.NumberToken prevClause = null;
            for (Pullenti.Ner.Token t = kit.FirstToken; t != null; t = t.Next) 
            {
                t.Tag = null;
                Pullenti.Ner.TextToken tt = t as Pullenti.Ner.TextToken;
                if (tt == null) 
                    continue;
                if (tt.Term == "СТАТЬЯ" && !tt.Chars.IsAllLower && (tt.Next is Pullenti.Ner.NumberToken)) 
                {
                }
                else 
                    continue;
                Pullenti.Ner.NumberToken num = tt.Next as Pullenti.Ner.NumberToken;
                if (num.IntValue == null) 
                    continue;
                Pullenti.Ner.Token t1 = tt.Next;
                if (t1.Next != null && t1.Next.IsChar('.')) 
                    t1 = t1.Next;
                if (tt.IsNewlineBefore || ((tt.Previous != null && tt.Previous.IsTableControlChar))) 
                {
                    if (t1.IsNewlineAfter || ((t1.Next != null && t1.Next.IsTableControlChar))) 
                    {
                        if (prevClause == null) 
                            prevClause = num;
                        else if ((prevClause.IntValue.Value + 1) == num.IntValue.Value) 
                        {
                            tt.Tag = prevClause;
                            prevClause = num;
                        }
                        else 
                            prevClause = null;
                    }
                }
            }
            int lastDecDist = 0;
            int ignoreMaxPos = 0;
            for (Pullenti.Ner.Token t = kit.FirstToken; t != null; t = t.Next,lastDecDist++) 
            {
                if (t.IsNewlineBefore && (t is Pullenti.Ner.NumberToken) && (t as Pullenti.Ner.NumberToken).Value == "88") 
                {
                }
                if (t.IsIgnored) 
                    continue;
                if (t.IsValue2("СЛЕДУЮЩЕГО", "СОДЕРЖАНИЯ") || t.IsValue2("СЛЕДУЮЩЕЙ", "РЕДАКЦИИ") || ((t.IsValue2("НОВОЙ", "РЕДАКЦИИ") && t.Next.Next != null && t.Next.Next.IsChar(':')))) 
                {
                    Pullenti.Ner.Token tt = t.Next.Next;
                    if (tt != null && tt.IsChar(':')) 
                        tt = tt.Next;
                    if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(tt, true, false)) 
                    {
                        Pullenti.Ner.Decree.Internal.DecreeChangeToken txt = Pullenti.Ner.Decree.Internal.DecreeChangeToken._tryParseText(tt, false, false, null);
                        if (txt != null) 
                        {
                            t = txt.EndToken;
                            continue;
                        }
                    }
                }
                if (t.IsValue("СЛОВО", null) && Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t.Next, true, false)) 
                {
                    Pullenti.Ner.Decree.Internal.DecreeChangeToken txt = Pullenti.Ner.Decree.Internal.DecreeChangeToken._tryParseText(t.Next, false, false, null);
                    if (txt != null) 
                    {
                        if (txt.LengthChar > 500) 
                        {
                        }
                        else 
                            ignoreMaxPos = txt.EndChar;
                    }
                }
                if (t.IsValue("УТВЕРЖДЕННЫЙ", null)) 
                {
                }
                List<Pullenti.Ner.Decree.Internal.DecreeToken> dts = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttachList(t, null, 10, lastDecDist > 1000);
                if (dts == null || dts.Count == 0) 
                    continue;
                if (dts[0].IsNewlineAfter && dts[0].IsNewlineBefore) 
                {
                    bool ignore = false;
                    if (t == kit.FirstToken) 
                        ignore = true;
                    else if ((dts[0].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Org && dts.Count > 1 && dts[1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) && dts[1].IsWhitespaceAfter) 
                        ignore = true;
                    if (ignore) 
                    {
                        t = dts[dts.Count - 1].EndToken;
                        continue;
                    }
                }
                if (dts[0].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ && Pullenti.Ner.Decree.Internal.DecreeToken.GetKind(dts[0].Value, null) == DecreeKind.Publisher) 
                {
                    List<Pullenti.Ner.ReferentToken> rts = this.TryAttachPulishers(dts);
                    if (rts != null) 
                    {
                        for (int i = 0; i < rts.Count; i++) 
                        {
                            Pullenti.Ner.ReferentToken rtt = rts[i];
                            if (rtt.Referent is DecreePartReferent) 
                                (rtt.Referent as DecreePartReferent).Owner = ad.RegisterReferent((rtt.Referent as DecreePartReferent).Owner) as DecreeReferent;
                            rtt.Referent = ad.RegisterReferent(rtt.Referent);
                            if (rtt.BeginChar < ignoreMaxPos) 
                                rtt.Tag = "ignored";
                            kit.EmbedToken(rtt);
                            t = rtt;
                            if ((rtt.Referent is DecreeReferent) && ((i + 1) < rts.Count) && (rts[i + 1].Referent is DecreePartReferent)) 
                                rts[i + 1].BeginToken = t;
                            lastDecDist = 0;
                        }
                        Pullenti.Ner.Token tt1 = t.Next;
                        if (tt1 != null && tt1.IsChar(')')) 
                            tt1 = tt1.Next;
                    }
                    continue;
                }
                if (baseTyp == null) 
                {
                    foreach (Pullenti.Ner.Decree.Internal.DecreeToken dd in dts) 
                    {
                        if (dd.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) 
                        {
                            baseTyp = dd;
                            break;
                        }
                    }
                }
                if ((dts.Count == 1 && dts[0].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ && dts[0].Value != "КОНСТИТУЦИЯ") && !dts[0].TypRefBack) 
                    continue;
                if (dts[0].Value == "РЕГЛАМЕНТ") 
                {
                    if (dts.Count < 3) 
                        continue;
                    if (dts[1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) 
                        continue;
                }
                List<Pullenti.Ner.ReferentToken> rtli = TryAttach(dts, baseTyp, ad);
                bool multi = false;
                if ((rtli == null && dts.Count == 2 && dts[0].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) && ((dts[1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Org || dts[1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Owner))) 
                {
                    for (Pullenti.Ner.Token tt2 = dts[1].EndToken.Next; tt2 != null; tt2 = tt2.Next) 
                    {
                        if (tt2.IsCharOf(";:-")) 
                            continue;
                        if (tt2.IsNewlineBefore) 
                        {
                            Pullenti.Ner.Core.ComplexNumToken cn = Pullenti.Ner.Core.ComplexNumToken.TryParse(tt2, null, false, false);
                            if (cn != null) 
                                tt2 = cn.EndToken.Next;
                            if (tt2 == null) 
                                break;
                        }
                        List<Pullenti.Ner.Decree.Internal.DecreeToken> dts2 = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttachList(tt2, null, 4, false);
                        if (dts2 == null || dts2.Count != 3) 
                            break;
                        if (dts2[2].Typ != Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Name) 
                            break;
                        if (dts2[0].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number && dts2[1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Date) 
                        {
                        }
                        else if (dts2[1].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Number && dts2[0].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Date) 
                        {
                        }
                        else 
                            break;
                        List<Pullenti.Ner.Decree.Internal.DecreeToken> ddd = new List<Pullenti.Ner.Decree.Internal.DecreeToken>();
                        ddd.AddRange(dts);
                        ddd.AddRange(dts2);
                        List<Pullenti.Ner.ReferentToken> rtli2 = TryAttach(ddd, baseTyp, ad);
                        if (rtli2 == null || rtli2.Count != 1) 
                            break;
                        rtli2[0].BeginToken = tt2;
                        if (rtli == null) 
                            rtli = new List<Pullenti.Ner.ReferentToken>();
                        rtli.Add(rtli2[0]);
                        tt2 = rtli2[0].EndToken;
                        multi = true;
                    }
                }
                if (rtli != null) 
                {
                    for (int ii = 0; ii < rtli.Count; ii++) 
                    {
                        Pullenti.Ner.ReferentToken rt = rtli[ii];
                        lastDecDist = 0;
                        Pullenti.Ner.Referent dec0 = rt.Referent;
                        rt.Referent = ad.RegisterReferent(rt.Referent);
                        if (!multi) 
                        {
                            for (int jj = ii + 1; jj < rtli.Count; jj++) 
                            {
                                foreach (Pullenti.Ner.Slot s in rtli[jj].Referent.Slots) 
                                {
                                    if (s.Value == dec0) 
                                        s.Value = rt.Referent;
                                }
                            }
                        }
                        ref0 = rt.Referent;
                        kit.EmbedToken(rt);
                        t = rt;
                        if (rt.BeginChar < ignoreMaxPos) 
                            rt.Tag = "ignored";
                        if ((ii + 1) < rtli.Count) 
                        {
                            if (rt.EndToken.Next == rtli[ii + 1].BeginToken) 
                                rtli[ii + 1].BeginToken = rt;
                        }
                    }
                }
                else if (dts.Count == 1 && dts[0].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) 
                {
                    if (dts[0].Chars.IsCapitalUpper && !dts[0].IsDoubtful) 
                    {
                        lastDecDist = 0;
                        if (baseTyp != null && dts[0].Ref != null) 
                        {
                            DecreeReferent drr = dts[0].Ref.GetReferent() as DecreeReferent;
                            if (drr != null) 
                            {
                                if (baseTyp.Value == drr.Typ0 || baseTyp.Value == drr.Typ) 
                                    continue;
                            }
                        }
                        Pullenti.Ner.ReferentToken rt0 = Pullenti.Ner.Decree.Internal.DecreeToken._findBackTyp(dts[0].BeginToken.Previous, dts[0].Value);
                        if (rt0 != null) 
                        {
                            Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(rt0.Referent, dts[0].BeginToken, dts[0].EndToken);
                            kit.EmbedToken(rt);
                            t = rt;
                            rt.Tag = rt0.Referent;
                        }
                    }
                }
            }
            if (ad.Referents.Count > 0) 
            {
                for (Pullenti.Ner.Token t = kit.FirstToken; t != null; t = t.Next) 
                {
                    if (t.IsIgnored) 
                        continue;
                    DecreeReferent dr = t.GetReferent() as DecreeReferent;
                    if (dr == null) 
                        continue;
                    List<DecreeReferent> li = null;
                    for (Pullenti.Ner.Token tt = t.Next; tt != null; tt = tt.Next) 
                    {
                        if (!tt.IsCommaAnd) 
                            break;
                        if (tt.Next == null || !(tt.Next.GetReferent() is DecreeReferent)) 
                            break;
                        if (li == null) 
                        {
                            li = new List<DecreeReferent>();
                            li.Add(dr);
                        }
                        dr = tt.Next.GetReferent() as DecreeReferent;
                        li.Add(dr);
                        dr.Tag = null;
                        tt = tt.Next;
                        if (dr.Date != null) 
                        {
                            List<Pullenti.Ner.Decree.Internal.DecreeToken> dts = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttachList((tt as Pullenti.Ner.ReferentToken).BeginToken, null, 10, false);
                            if (dts != null) 
                            {
                                foreach (Pullenti.Ner.Decree.Internal.DecreeToken dt in dts) 
                                {
                                    if (dt.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Date) 
                                        dr.Tag = dr;
                                }
                            }
                        }
                    }
                    if (li == null) 
                        continue;
                    int i;
                    for (i = li.Count - 1; i > 0; i--) 
                    {
                        if (li[i].Typ == li[i - 1].Typ) 
                        {
                            if (li[i].Date != null && li[i].Tag != null && li[i - 1].Date == null) 
                                li[i - 1].AddSlot(DecreeReferent.ATTR_DATE, li[i].GetSlotValue(DecreeReferent.ATTR_DATE), false, 0);
                        }
                    }
                    for (i = 0; i < (li.Count - 1); i++) 
                    {
                        if (li[i].Typ == li[i + 1].Typ) 
                        {
                            Pullenti.Ner.Slot sl = li[i].FindSlot(DecreeReferent.ATTR_SOURCE, null, true);
                            if (sl != null && li[i + 1].FindSlot(DecreeReferent.ATTR_SOURCE, null, true) == null) 
                                li[i + 1].AddSlot(sl.TypeName, sl.Value, false, 0);
                        }
                    }
                    for (i = 0; i < li.Count; i++) 
                    {
                        if (li[i].Name != null) 
                            break;
                    }
                    if (i == (li.Count - 1)) 
                    {
                        for (i = li.Count - 1; i > 0; i--) 
                        {
                            if (li[i - 1].Typ == li[i].Typ) 
                                li[i - 1].AddName(li[i]);
                        }
                    }
                }
            }
            Pullenti.Ner.Core.TerminCollection aliases = new Pullenti.Ner.Core.TerminCollection();
            Pullenti.Ner.Core.TerminCollection attachednames = new Pullenti.Ner.Core.TerminCollection();
            for (Pullenti.Ner.Token t = kit.FirstToken; t != null; t = t.Next,lastDecDist++) 
            {
                if (t.IsIgnored) 
                    continue;
                if (t.IsValue2("СЛЕДУЮЩЕГО", "СОДЕРЖАНИЯ") || t.IsValue2("СЛЕДУЮЩЕЙ", "РЕДАКЦИИ") || ((t.IsValue2("НОВОЙ", "РЕДАКЦИИ") && t.Next.Next != null && t.Next.Next.IsChar(':')))) 
                {
                    Pullenti.Ner.Token tt = t.Next.Next;
                    if (tt != null && tt.IsChar(':')) 
                        tt = tt.Next;
                    if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(tt, true, false)) 
                    {
                        Pullenti.Ner.Decree.Internal.DecreeChangeToken txt = Pullenti.Ner.Decree.Internal.DecreeChangeToken._tryParseText(tt, false, false, null);
                        if (txt != null) 
                        {
                            t = txt.EndToken;
                            continue;
                        }
                    }
                }
                if (t.IsValue("СЛОВО", null) && Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t.Next, true, false)) 
                {
                    Pullenti.Ner.Decree.Internal.DecreeChangeToken txt = Pullenti.Ner.Decree.Internal.DecreeChangeToken._tryParseText(t.Next, false, false, null);
                    if (txt != null) 
                    {
                        if (txt.LengthChar > 500) 
                        {
                        }
                        else 
                        {
                            t = txt.EndToken;
                            continue;
                        }
                    }
                }
                Pullenti.Ner.Token checkAliasToken = null;
                Pullenti.Ner.ReferentToken rt = TryAttachApproved(t, ad, aliases, attachednames, false);
                if (t.IsValue("ВНЕСТИ", null)) 
                {
                }
                if (rt != null) 
                {
                    if (t.IsNewlineBefore && (t is Pullenti.Ner.TextToken) && (t as Pullenti.Ner.TextToken).Term == "ПРАВИЛА") 
                    {
                    }
                    rt.Referent = ad.RegisterReferent(rt.Referent);
                    kit.EmbedToken(rt);
                    t = rt;
                    if (rt.Tag is Pullenti.Ner.MetaToken) 
                    {
                        Pullenti.Ner.Core.Termin term = new Pullenti.Ner.Core.Termin();
                        term.InitBy((rt.Tag as Pullenti.Ner.MetaToken).BeginToken, (rt.Tag as Pullenti.Ner.MetaToken).EndToken.Previous, rt.Referent, false);
                        aliases.Add(term);
                    }
                    foreach (string nam in rt.Referent.GetStringValues(DecreeReferent.ATTR_NAME)) 
                    {
                        attachednames.Add(new Pullenti.Ner.Core.Termin(nam) { CanonicText = nam });
                    }
                }
                if (t.GetReferent() is DecreeReferent) 
                {
                    rt = t as Pullenti.Ner.ReferentToken;
                    if ((rt.Referent as DecreeReferent).Kind == DecreeKind.Publisher) 
                        continue;
                    if ((t.Next != null && t.Next.IsCharOf("(,") && (t.Next.Next is Pullenti.Ner.ReferentToken)) && (t.Next.Next.GetReferent() is DecreeReferent) && (t.Next.Next.GetReferent() as DecreeReferent).Kind == DecreeKind.Publisher) 
                    {
                        checkAliasToken = t.Next.Next.Next;
                        if (checkAliasToken != null && checkAliasToken.IsChar(')')) 
                            checkAliasToken = checkAliasToken.Next;
                    }
                }
                else if (t is Pullenti.Ner.TextToken) 
                {
                    Pullenti.Ner.Core.TerminToken tok = aliases.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
                    if (tok != null) 
                    {
                        DecreeReferent dec0 = tok.Termin.Tag as DecreeReferent;
                        Pullenti.Ner.ReferentToken rt0 = new Pullenti.Ner.ReferentToken(tok.Termin.Tag as Pullenti.Ner.Referent, tok.BeginToken, tok.EndToken);
                        if (dec0 != null && (rt0.EndToken.Next is Pullenti.Ner.ReferentToken) && (rt0.EndToken.Next.GetReferent() is Pullenti.Ner.Geo.GeoReferent)) 
                        {
                            Pullenti.Ner.Geo.GeoReferent geo0 = dec0.GetSlotValue(DecreeReferent.ATTR_GEO) as Pullenti.Ner.Geo.GeoReferent;
                            Pullenti.Ner.Geo.GeoReferent geo1 = rt0.EndToken.Next.GetReferent() as Pullenti.Ner.Geo.GeoReferent;
                            if (geo0 == null) 
                            {
                                dec0.AddSlot(DecreeReferent.ATTR_GEO, geo1, false, 0);
                                rt0.EndToken = rt0.EndToken.Next;
                            }
                            else if (geo0 == geo1) 
                                rt0.EndToken = rt0.EndToken.Next;
                            else 
                                continue;
                        }
                        if (rt0.IsNewlineBefore && rt0.IsNewlineAfter) 
                            continue;
                        kit.EmbedToken(rt0);
                        t = rt0;
                        rt0.MiscAttrs = 1;
                        lastDecDist = 0;
                        continue;
                    }
                    List<Pullenti.Ner.Decree.Internal.DecreeToken> dts = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttachList(t, null, 2, false);
                    if (dts != null && dts[0].Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) 
                    {
                        List<Pullenti.Ner.ReferentToken> rtli = TryAttach(dts, null, ad);
                        if (rtli != null && rtli.Count == 1) 
                        {
                            rt = rtli[0];
                            rt.Referent = ad.RegisterReferent(rt.Referent);
                            kit.EmbedToken(rt);
                            t = rt;
                        }
                    }
                }
                if (rt != null) 
                {
                    Pullenti.Ner.MetaToken mt = _checkAliasAfter(checkAliasToken ?? t.Next);
                    if (mt != null) 
                    {
                        if (mt.Tag is DecreeReferent) 
                        {
                            DecreeReferent dr0 = mt.Tag as DecreeReferent;
                            DecreeReferent dr1 = rt.Referent as DecreeReferent;
                            if (dr1 != dr0 && dr1.Owner != dr0) 
                            {
                                if (dr0.Number != null) 
                                {
                                    if (dr1.Number != null && ((dr1.Number == dr0.Number || dr0.Number.StartsWith(dr1.Number + "/")))) 
                                    {
                                    }
                                    else if (dr1.Owner != null && dr1.Owner.Number == dr0.Number) 
                                    {
                                    }
                                    else 
                                        continue;
                                }
                                kit.ReplaceReferent(mt.Tag as DecreeReferent, rt.Referent);
                            }
                            if (checkAliasToken == null) 
                            {
                                rt = new Pullenti.Ner.ReferentToken(t.GetReferent(), t, mt.EndToken);
                                kit.EmbedToken(rt);
                                t = rt;
                            }
                        }
                        else 
                        {
                            Pullenti.Ner.Token eTok = mt.EndToken.Previous;
                            if ((mt.BeginToken.Next != null && mt.BeginToken.Next.IsCommaAnd && mt.BeginToken.Next.Next == mt.EndToken.Previous.Previous) && mt.EndToken.Previous.IsValue("СООТВЕТСТВЕННО", null)) 
                            {
                                Pullenti.Ner.TextToken tt = mt.BeginToken.Next.Next as Pullenti.Ner.TextToken;
                                string nam = rt.Referent.GetStringValue("NAME") ?? "";
                                if (nam.StartsWith("ОБ УТВЕРЖДЕНИИ ") && tt != null) 
                                {
                                    Pullenti.Ner.AnalysisResult ar0 = Pullenti.Ner.ProcessorService.EmptyProcessor.Process(new Pullenti.Ner.SourceOfAnalysis(nam), null, null);
                                    nam = Pullenti.Ner.Core.MiscHelper.GetTextValue(ar0.FirstToken.Next.Next, ar0.FindTokenByPos(nam.Length - 1, null), Pullenti.Ner.Core.GetTextAttr.FirstNounGroupToNominative);
                                    if (nam.StartsWith(tt.Term)) 
                                    {
                                        DecreeReferent r1 = new DecreeReferent();
                                        r1.Owner = rt.Referent as DecreeReferent;
                                        r1.AddSlot(DecreeReferent.ATTR_NAME, nam, false, 0);
                                        r1 = ad.RegisterReferent(r1) as DecreeReferent;
                                        Pullenti.Ner.ReferentToken rt1 = new Pullenti.Ner.ReferentToken(r1, tt, tt);
                                        kit.EmbedToken(rt1);
                                        Pullenti.Ner.Core.Termin term1 = new Pullenti.Ner.Core.Termin();
                                        term1.InitBy(tt, tt, r1, false);
                                        aliases.Add(term1);
                                        eTok = mt.BeginToken.Next;
                                    }
                                }
                            }
                            Pullenti.Ner.Core.Termin term = new Pullenti.Ner.Core.Termin();
                            term.InitBy(mt.BeginToken, eTok, rt.Referent, false);
                            aliases.Add(term);
                            if (checkAliasToken == null) 
                            {
                                rt = new Pullenti.Ner.ReferentToken(t.GetReferent(), t, mt.EndToken);
                                kit.EmbedToken(rt);
                                t = rt;
                            }
                        }
                    }
                    else 
                    {
                        Pullenti.Ner.Core.TerminToken aa = aliases.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
                        if (aa != null) 
                            aa.Termin.Tag = rt.Referent;
                    }
                }
            }
            this._processPartsAndChanges(kit, baseTyp);
            for (Pullenti.Ner.Token t = kit.FirstToken; t != null; t = t.Next) 
            {
                if (t.Tag != null && (t is Pullenti.Ner.ReferentToken) && (t.Tag is DecreeReferent)) 
                {
                    t = kit.DebedToken(t);
                    if (t == null) 
                        break;
                }
            }
        }
        internal static Pullenti.Ner.MetaToken _checkAliasAfter(Pullenti.Ner.Token t)
        {
            if ((t != null && t.IsChar('<') && t.Next != null) && t.Next.Next != null && t.Next.Next.IsChar('>')) 
                t = t.Next.Next.Next;
            if (t == null || t.Next == null || !t.IsChar('(')) 
                return null;
            if (t.Next.GetReferent() != null) 
            {
                DecreeReferent dr = t.Next.GetReferent() as DecreeReferent;
                if (dr == null && (t.Next.GetReferent() is DecreePartReferent)) 
                    dr = (t.Next.GetReferent() as DecreePartReferent).Owner;
                if (dr != null && dr.Kind == DecreeKind.Publisher) 
                {
                    Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                    if (br != null && br.EndToken.Next != null && br.EndToken.Next.IsChar('(')) 
                    {
                        t = br.EndToken.Next;
                        if (t.Next == null) 
                            return null;
                    }
                }
            }
            t = t.Next;
            if (t.IsValue("ДАЛЕЕ", "ДАЛІ")) 
            {
            }
            else 
                return null;
            t = t.Next;
            if (t != null && !t.Chars.IsLetter) 
                t = t.Next;
            if (t == null) 
                return null;
            if ((t.GetReferent() is DecreeReferent) && t.Next != null && t.Next.IsChar(')')) 
                return new Pullenti.Ner.MetaToken(t, t.Next) { Tag = t.GetReferent() };
            Pullenti.Ner.Token t1 = null;
            int lev = 0;
            for (Pullenti.Ner.Token tt = t; tt != null; tt = tt.Next) 
            {
                if (tt.IsNewlineBefore) 
                    break;
                else if (tt.IsChar('(')) 
                    lev++;
                else if (tt.IsChar(')')) 
                {
                    lev--;
                    if (lev < 0) 
                    {
                        t1 = tt.Previous;
                        break;
                    }
                }
            }
            if (t1 == null) 
                return null;
            return new Pullenti.Ner.MetaToken(t, t1.Next);
        }
        internal static DecreeReferent _getDecree(Pullenti.Ner.Token t)
        {
            if (!(t is Pullenti.Ner.ReferentToken)) 
                return null;
            Pullenti.Ner.Referent r = t.GetReferent();
            if (r is DecreeReferent) 
                return r as DecreeReferent;
            if (r is DecreePartReferent) 
                return (r as DecreePartReferent).Owner;
            return null;
        }
        internal static Pullenti.Ner.Token _checkOtherTyp(Pullenti.Ner.Token t, bool first)
        {
            if (t == null) 
                return null;
            Pullenti.Ner.Decree.Internal.DecreeToken dit = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(t, null, false);
            Pullenti.Ner.Core.NounPhraseToken npt = null;
            if (dit == null) 
            {
                npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt != null && npt.BeginToken != npt.EndToken) 
                    dit = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(npt.EndToken, null, false);
            }
            if (dit != null && dit.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) 
            {
                if (dit.Chars.IsCapitalUpper || first) 
                {
                    dit.EndToken.Tag = dit.Value;
                    return dit.EndToken;
                }
                else 
                    return null;
            }
            if (npt != null) 
                t = npt.EndToken;
            if (t.Chars.IsCapitalUpper || first) 
            {
                if (t.Previous != null && t.Previous.IsChar('.') && !first) 
                    return null;
                Pullenti.Ner.Token tt = Pullenti.Ner.Decree.Internal.DecreeToken.IsKeyword(t, false);
                if (tt != null) 
                    return tt;
            }
            return null;
        }
        public override Pullenti.Ner.ReferentToken ProcessReferent(Pullenti.Ner.Token begin, string param)
        {
            Pullenti.Ner.ReferentToken rt = TryAttachApproved(begin, null, null, null, false);
            if (rt != null) 
                return rt;
            List<Pullenti.Ner.Decree.Internal.DecreeToken> dpli = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttachList(begin, null, 10, true);
            if (dpli != null) 
            {
                List<Pullenti.Ner.ReferentToken> lii = TryAttach(dpli, null, null);
                if (lii != null && lii.Count > 0) 
                    return lii[0];
            }
            Pullenti.Ner.Decree.Internal.DecreeToken dp = Pullenti.Ner.Decree.Internal.DecreeToken.TryAttach(begin, null, false);
            if (dp != null && dp.Typ == Pullenti.Ner.Decree.Internal.DecreeToken.ItemType.Typ) 
                return new Pullenti.Ner.ReferentToken(null, dp.BeginToken, dp.EndToken);
            return null;
        }
        static bool m_Inited;
        public static void Initialize()
        {
            if (m_Inited) 
                return;
            m_Inited = true;
            Pullenti.Ner.Decree.Internal.MetaDecree.Initialize();
            Pullenti.Ner.Decree.Internal.MetaDecreePart.Initialize();
            Pullenti.Ner.Decree.Internal.MetaDecreeChange.Initialize();
            Pullenti.Ner.Decree.Internal.MetaDecreeChangeValue.Initialize();
            try 
            {
                Pullenti.Ner.Core.Termin.AssignAllTextsAsNormal = false;
                Pullenti.Ner.Decree.Internal.DecreeChangeToken.Initialize();
                Pullenti.Ner.Decree.Internal.DecreeToken.Initialize();
                Pullenti.Ner.Core.Termin.AssignAllTextsAsNormal = false;
            }
            catch(Exception ex) 
            {
                throw new Exception(ex.Message, ex);
            }
            Pullenti.Ner.ProcessorService.RegisterAnalyzer(new DecreeAnalyzer());
        }
    }
}