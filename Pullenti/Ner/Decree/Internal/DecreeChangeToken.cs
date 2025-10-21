/*
 * SDK Pullenti Lingvo, version 4.31, august 2025. Copyright (c) 2013-2025, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Pullenti.Ner.Decree.Internal
{
    class DecreeChangeToken : Pullenti.Ner.MetaToken
    {
        public static List<Pullenti.Ner.ReferentToken> AttachReferents(Pullenti.Ner.Referent dpr, DecreeChangeToken tok0)
        {
            if (dpr == null || tok0 == null) 
                return null;
            Pullenti.Ner.Token tt0 = tok0.EndToken.Next;
            if (tt0 != null && tt0.IsCommaAnd && tok0.ActKind == Pullenti.Ner.Decree.DecreeChangeKind.Undefined) 
                tt0 = tt0.Next;
            if (tt0 != null && tt0.IsChar(':')) 
                tt0 = tt0.Next;
            List<DecreeChangeToken> toks = null;
            if (tt0 == null) 
            {
            }
            else if (tt0.Previous != null && tt0.Previous.IsChar(':')) 
            {
                if (tok0.ActKind2 == Pullenti.Ner.Decree.DecreeChangeKind.Append) 
                    toks = TryAttachList(tt0, true, tok0.IndentRegime);
            }
            else 
            {
                if (tt0.IsComma && tok0.Decree != null) 
                    tt0 = tt0.Next;
                toks = TryAttachList(tt0, false, tok0.IndentRegime);
            }
            if (toks == null) 
                toks = new List<DecreeChangeToken>();
            toks.Insert(0, tok0);
            List<Pullenti.Ner.Decree.DecreeChangeKind> kinds = new List<Pullenti.Ner.Decree.DecreeChangeKind>();
            foreach (DecreeChangeToken tok in toks) 
            {
                if (tok.ActKind != Pullenti.Ner.Decree.DecreeChangeKind.Undefined && !kinds.Contains(tok.ActKind)) 
                    kinds.Add(tok.ActKind);
            }
            List<Pullenti.Ner.ReferentToken> res = new List<Pullenti.Ner.ReferentToken>();
            Pullenti.Ner.Decree.DecreeChangeReferent dcr = new Pullenti.Ner.Decree.DecreeChangeReferent();
            if (tok0.RealPart != null && !(dpr is Pullenti.Ner.Decree.DecreePartReferent)) 
                dcr.AddSlot(Pullenti.Ner.Decree.DecreeChangeReferent.ATTR_OWNER, tok0.RealPart, false, 0);
            else 
                dcr.AddSlot(Pullenti.Ner.Decree.DecreeChangeReferent.ATTR_OWNER, dpr, false, 0);
            if (tok0.AddDecrees != null) 
            {
                foreach (Pullenti.Ner.Decree.DecreeReferent dd in tok0.AddDecrees) 
                {
                    Pullenti.Ner.Decree.DecreePartReferent pp = dpr as Pullenti.Ner.Decree.DecreePartReferent;
                    if (pp != null && pp.Preamble != null && pp.Slots.Count == 2) 
                    {
                        Pullenti.Ner.Decree.DecreePartReferent pp2 = new Pullenti.Ner.Decree.DecreePartReferent();
                        pp2.Owner = dd;
                        pp2.Preamble = pp.Preamble;
                        dcr.AddSlot(Pullenti.Ner.Decree.DecreeChangeReferent.ATTR_OWNER, pp2, false, 0);
                        res.Add(new Pullenti.Ner.ReferentToken(pp2, null, null));
                        continue;
                    }
                    dcr.AddSlot(Pullenti.Ner.Decree.DecreeChangeReferent.ATTR_OWNER, dd, false, 0);
                }
            }
            Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(dcr, tok0.BeginToken, tok0.EndToken);
            res.Add(rt);
            List<string> newItems = null;
            bool inTheEnd = false;
            while (true) 
            {
                for (int i = 0; i < toks.Count; i++) 
                {
                    DecreeChangeToken tok = toks[i];
                    if (tok.InTheEnd) 
                        inTheEnd = true;
                    if (i > 0 && tok.BeginToken.Previous != null && tok.BeginToken.Previous.IsChar(';')) 
                        break;
                    if ((i > 0 && tok.ActKind == Pullenti.Ner.Decree.DecreeChangeKind.Expire && tok.ChangeVal == null) && tok.Parts == null) 
                    {
                        if (dcr.Kind != Pullenti.Ner.Decree.DecreeChangeKind.Undefined) 
                            break;
                    }
                    rt.EndToken = tok.EndToken;
                    if (tok.Typ == DecreeChangeTokenTyp.AfterValue) 
                    {
                        if (tok.ChangeVal != null) 
                        {
                            dcr.AddSlot(Pullenti.Ner.Decree.DecreeChangeReferent.ATTR_PARAM, tok.ChangeVal, false, 0);
                            if (tok.EndChar > rt.EndChar) 
                                rt.EndToken = tok.EndToken;
                            res.Insert(res.Count - 1, new Pullenti.Ner.ReferentToken(tok.ChangeVal, tok.BeginToken, tok.EndToken));
                        }
                        continue;
                    }
                    if (tok.ActKind != Pullenti.Ner.Decree.DecreeChangeKind.Undefined) 
                    {
                        dcr.Kind = tok.ActKind;
                        if (tok.ActKind == Pullenti.Ner.Decree.DecreeChangeKind.Expire) 
                            break;
                    }
                    if (tok.ChangeVal == null && tok.AppExtChanges != null) 
                    {
                        tok.ChangeVal = new Pullenti.Ner.Decree.DecreeChangeValueReferent();
                        tok.ChangeVal.Kind = Pullenti.Ner.Decree.DecreeChangeValueKind.ExtAppendix;
                        tok.ChangeVal.Value = (tok.AppExtChanges.Values.Count == 0 ? "" : tok.AppExtChanges.Values[0].ToString());
                        res.Insert(res.Count - 1, new Pullenti.Ner.ReferentToken(tok.ChangeVal, tok.AppExtChanges.BeginToken, tok.AppExtChanges.EndToken));
                        rt.EndToken = tok.EndToken;
                    }
                    if (tok.ChangeVal != null) 
                    {
                        if (((i + 2) < toks.Count) && ((toks[i + 1].ActKind == Pullenti.Ner.Decree.DecreeChangeKind.Exchange || toks[i + 1].ActKind == Pullenti.Ner.Decree.DecreeChangeKind.New)) && toks[i + 2].ChangeVal != null) 
                        {
                            dcr.Param = tok.ChangeVal;
                            Pullenti.Ner.ReferentToken rt11 = new Pullenti.Ner.ReferentToken(tok.ChangeVal, tok.BeginToken, tok.EndToken);
                            if (tok.Parts != null && tok.Parts.Count > 0) 
                                rt11.BeginToken = tok.Parts[tok.Parts.Count - 1].EndToken.Next;
                            res.Insert(res.Count - 1, rt11);
                            dcr.Value = toks[i + 2].ChangeVal;
                            dcr.Kind = toks[i + 1].ActKind;
                            i += 2;
                            tok = toks[i];
                        }
                        else if (((i + 1) < toks.Count) && toks[i + 1].ChangeVal != null && dcr.Kind == Pullenti.Ner.Decree.DecreeChangeKind.Exchange) 
                        {
                            dcr.Param = tok.ChangeVal;
                            res.Insert(res.Count - 1, new Pullenti.Ner.ReferentToken(tok.ChangeVal, tok.BeginToken, tok.EndToken));
                            dcr.Value = toks[i + 1].ChangeVal;
                            i += 1;
                            tok = toks[i];
                        }
                        else if (dcr.Value == null) 
                        {
                            dcr.Value = tok.ChangeVal;
                            if (dcr.Kind == Pullenti.Ner.Decree.DecreeChangeKind.Undefined && ((i == (toks.Count - 1) || ((i == (toks.Count - 2) && toks[i + 1].Typ == DecreeChangeTokenTyp.Value))))) 
                            {
                                List<DecreeChangeToken> next = new List<DecreeChangeToken>();
                                for (Pullenti.Ner.Token ttt = toks[toks.Count - 1].EndToken.Next; ttt != null; ttt = ttt.Next) 
                                {
                                    if (ttt.IsCommaAnd) 
                                        continue;
                                    DecreeChangeToken ne = TryAttach(ttt, null, false, null, false, false, null);
                                    if (ne == null) 
                                        break;
                                    if (ne.Typ == DecreeChangeTokenTyp.Value && ne.ChangeVal != null) 
                                    {
                                        next.Add(ne);
                                        ttt = ne.EndToken;
                                        continue;
                                    }
                                    if (ne.Typ == DecreeChangeTokenTyp.Action && ne.ActKind == Pullenti.Ner.Decree.DecreeChangeKind.Remove) 
                                        next.Add(ne);
                                    break;
                                }
                                if (next.Count > 0 && next[next.Count - 1].Typ == DecreeChangeTokenTyp.Action) 
                                {
                                    dcr.Kind = Pullenti.Ner.Decree.DecreeChangeKind.Remove;
                                    res.Insert(res.Count - 1, new Pullenti.Ner.ReferentToken(tok.ChangeVal, tok.BeginToken, tok.EndToken));
                                    if (i < (toks.Count - 1)) 
                                        next.Insert(0, toks[i + 1]);
                                    for (int k = 0; k < (next.Count - 1); k++) 
                                    {
                                        DecreeChangeToken ne = next[k];
                                        res.Add(new Pullenti.Ner.ReferentToken(ne.ChangeVal, ne.BeginToken, ne.EndToken));
                                        dcr = new Pullenti.Ner.Decree.DecreeChangeReferent();
                                        dcr.Kind = Pullenti.Ner.Decree.DecreeChangeKind.Remove;
                                        dcr.Value = ne.ChangeVal;
                                        dcr.AddSlot(Pullenti.Ner.Decree.DecreeChangeReferent.ATTR_OWNER, dpr, false, 0);
                                        rt = new Pullenti.Ner.ReferentToken(dcr, ne.BeginToken, ne.EndToken);
                                        if (k == (next.Count - 2)) 
                                            rt.EndToken = next[next.Count - 1].EndToken;
                                        res.Add(rt);
                                    }
                                    break;
                                }
                            }
                        }
                        else if ((dcr.Value.Kind != Pullenti.Ner.Decree.DecreeChangeValueKind.Text && tok.ChangeVal.Kind == Pullenti.Ner.Decree.DecreeChangeValueKind.Text && tok.ChangeVal.Value != null) && dcr.Value.Value == null) 
                            dcr.Value.Value = tok.ChangeVal.Value;
                        else if (kinds.Count == 1 && kinds[0] == Pullenti.Ner.Decree.DecreeChangeKind.Remove) 
                            dcr.AddSlot(Pullenti.Ner.Decree.DecreeChangeReferent.ATTR_VALUE, tok.ChangeVal, false, 0);
                        else if (kinds.Count == 1 && kinds[0] == Pullenti.Ner.Decree.DecreeChangeKind.Append && dcr.Param == null) 
                        {
                            dcr.Param = dcr.Value;
                            dcr.Value = tok.ChangeVal;
                        }
                        else if (kinds.Count == 0 && dcr.Param == null) 
                            dcr.Param = tok.ChangeVal;
                        else 
                            dcr.Value = tok.ChangeVal;
                        if (tok.EndChar > rt.EndChar) 
                            rt.EndToken = tok.EndToken;
                        res.Insert(res.Count - 1, new Pullenti.Ner.ReferentToken(tok.ChangeVal, tok.BeginToken, tok.EndToken));
                        if (dcr.Kind == Pullenti.Ner.Decree.DecreeChangeKind.New) 
                            break;
                        if (dcr.Kind == Pullenti.Ner.Decree.DecreeChangeKind.Consider) 
                        {
                            if ((i + 2) < toks.Count) 
                            {
                                if (toks[i + 2].ActKind != Pullenti.Ner.Decree.DecreeChangeKind.Undefined || toks[i + 1].ActKind != Pullenti.Ner.Decree.DecreeChangeKind.Undefined) 
                                    break;
                                dcr = new Pullenti.Ner.Decree.DecreeChangeReferent();
                                dcr.AddSlot(Pullenti.Ner.Decree.DecreeChangeReferent.ATTR_OWNER, dpr, false, 0);
                                rt = new Pullenti.Ner.ReferentToken(dcr, toks[i + 1].BeginToken, toks[i + 1].EndToken);
                                res.Add(rt);
                                continue;
                            }
                            break;
                        }
                    }
                    if (dcr.Kind == Pullenti.Ner.Decree.DecreeChangeKind.Append && tok.NewParts != null) 
                    {
                        foreach (PartToken np in tok.NewParts) 
                        {
                            int rank = PartToken._getRank(np.Typ);
                            if (rank == 0) 
                                continue;
                            string eqLevVal = null;
                            if (dpr is Pullenti.Ner.Decree.DecreePartReferent) 
                            {
                                if (!(dpr as Pullenti.Ner.Decree.DecreePartReferent).IsAllItemsOverThisLevel(np.Typ)) 
                                {
                                    eqLevVal = dpr.GetStringValue(PartToken._getAttrNameByTyp(np.Typ));
                                    if (eqLevVal == null) 
                                        continue;
                                }
                            }
                            dcr.Kind = Pullenti.Ner.Decree.DecreeChangeKind.Append;
                            if (newItems == null) 
                                newItems = new List<string>();
                            string nam = PartToken._getAttrNameByTyp(np.Typ);
                            if (nam == null) 
                                continue;
                            if (np.Values.Count == 0) 
                            {
                                if (eqLevVal == null) 
                                    newItems.Add(nam);
                                else 
                                {
                                    int n;
                                    if (int.TryParse(eqLevVal, out n)) 
                                        newItems.Add(string.Format("{0} {1}", nam, n + 1));
                                    else 
                                        newItems.Add(nam);
                                }
                            }
                            else if (np.Values.Count == 2 && np.Values[0].EndToken.Next.IsHiphen) 
                            {
                                List<string> vv = Pullenti.Ner.Instrument.Internal.NumberingHelper.CreateDiap(np.Values[0].Value, np.Values[1].Value);
                                if (vv != null) 
                                {
                                    foreach (string v in vv) 
                                    {
                                        newItems.Add(string.Format("{0} {1}", nam, v));
                                    }
                                }
                            }
                            if (newItems.Count == 0) 
                            {
                                foreach (PartToken.PartValue v in np.Values) 
                                {
                                    newItems.Add(string.Format("{0} {1}///{2}", nam, v.Value, v.SourceValue));
                                }
                            }
                        }
                    }
                }
                if (dcr.Param != null && dcr.Value != null && dcr.Kind == Pullenti.Ner.Decree.DecreeChangeKind.Undefined) 
                    dcr.Kind = Pullenti.Ner.Decree.DecreeChangeKind.Error;
                if (!dcr.CheckCorrect()) 
                    return null;
                if (newItems != null && dcr.Value != null && dcr.Kind == Pullenti.Ner.Decree.DecreeChangeKind.Append) 
                {
                    foreach (string v in newItems) 
                    {
                        string vvv = v;
                        string src = null;
                        int ii = vvv.IndexOf("///");
                        if (ii > 0) 
                        {
                            vvv = vvv.Substring(0, ii);
                            src = v.Substring(ii + 3);
                        }
                        dcr.Value.AddSlot(Pullenti.Ner.Decree.DecreeChangeValueReferent.ATTR_NEWITEM, vvv, false, 0).Tag = src;
                    }
                }
                newItems = null;
                if (rt.EndToken.Next == null) 
                    break;
                if (!rt.EndToken.Next.IsCommaAnd) 
                    break;
                Pullenti.Ner.Token tt2 = rt.EndToken.Next.Next;
                if (tt2 == null) 
                    break;
                if (tt2.IsValue("А", null)) 
                    tt2 = tt2.Next;
                toks = TryAttachList(tt2, false, false);
                if (toks == null) 
                    break;
                Pullenti.Ner.Decree.DecreeChangeReferent dts1 = new Pullenti.Ner.Decree.DecreeChangeReferent();
                foreach (Pullenti.Ner.Referent o in dcr.Owners) 
                {
                    dts1.AddSlot(Pullenti.Ner.Decree.DecreeChangeReferent.ATTR_OWNER, o, false, 0);
                }
                rt = new Pullenti.Ner.ReferentToken(dts1, toks[0].BeginToken, toks[0].EndToken);
                res.Add(rt);
                dcr = dts1;
            }
            return res;
        }
        public static List<Pullenti.Ner.ReferentToken> SplitValue(Pullenti.Ner.ReferentToken rtVal, Pullenti.Ner.Referent own)
        {
            List<Pullenti.Ner.ReferentToken> res = new List<Pullenti.Ner.ReferentToken>();
            for (Pullenti.Ner.Token t = rtVal.BeginToken; t != null && (t.EndChar < rtVal.EndChar); t = t.Next) 
            {
                if (rtVal.BeginToken == t && Pullenti.Ner.Core.BracketHelper.IsBracket(t, true)) 
                    t = t.Next;
                Pullenti.Ner.Instrument.Internal.InstrToken1 line = Pullenti.Ner.Instrument.Internal.InstrToken1.Parse(t, true, null, 0, null, false, 0, false, false);
                if (line == null) 
                    break;
                Pullenti.Ner.Token t1 = line.EndToken;
                if ((t1.EndChar + 1) >= rtVal.EndChar) 
                {
                    line.EndToken = rtVal.EndToken;
                    for (; t1 != null; t1 = t1.Previous) 
                    {
                        if (t1.IsCharOf(".;")) 
                        {
                        }
                        else if (Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(t1, false, null, false)) 
                        {
                            t1 = t1.Previous;
                            break;
                        }
                        else 
                            break;
                    }
                }
                Pullenti.Ner.Decree.DecreeChangeValueReferent v = new Pullenti.Ner.Decree.DecreeChangeValueReferent();
                v.Kind = Pullenti.Ner.Decree.DecreeChangeValueKind.Text;
                v.Value = Pullenti.Ner.Core.MiscHelper.GetTextValue(t, t1, Pullenti.Ner.Core.GetTextAttr.KeepQuotes | Pullenti.Ner.Core.GetTextAttr.KeepRegister);
                v.BeginChar = (rtVal.Referent as Pullenti.Ner.Decree.DecreeChangeValueReferent).BeginChar;
                v.EndChar = (rtVal.Referent as Pullenti.Ner.Decree.DecreeChangeValueReferent).EndChar;
                Pullenti.Ner.ReferentToken rr = new Pullenti.Ner.ReferentToken(v, t, line.EndToken);
                res.Add(rr);
                t = line.EndToken;
            }
            if (res.Count > 0) 
            {
                Pullenti.Ner.ReferentToken rr = res[res.Count - 1];
                if (rr.EndToken.IsCharOf(";.,")) 
                    rr.EndToken = rr.EndToken.Previous;
            }
            return res;
        }
        public static DecreeChangeToken _tryParseText(Pullenti.Ner.Token xx1, bool abzacRegime, bool wordRegime = false, PartToken defPart = null)
        {
            DecreeChangeToken res = new DecreeChangeToken(xx1, xx1) { Typ = DecreeChangeTokenTyp.Value };
            res.ChangeVal = new Pullenti.Ner.Decree.DecreeChangeValueReferent() { Kind = Pullenti.Ner.Decree.DecreeChangeValueKind.Text };
            Pullenti.Ner.Token t0 = xx1;
            res.ChangeVal.BeginChar = xx1.BeginChar;
            if (Pullenti.Ner.Core.BracketHelper.IsBracket(xx1, true)) 
            {
                res.ChangeVal.BeginChar = xx1.EndChar + 1;
                t0 = xx1.Next;
                if (xx1.IsChar('<') && t0 != null && t0.IsChar('<')) 
                {
                    res.ChangeVal.BeginChar = t0.EndChar + 1;
                    t0 = t0.Next;
                }
            }
            Pullenti.Ner.Token doubt1 = null;
            Pullenti.Ner.Instrument.Internal.InstrToken1 clauseLast = null;
            Dictionary<PartToken.ItemType, string> pstack = new Dictionary<PartToken.ItemType, string>();
            int nls = 0;
            Dictionary<string, Pullenti.Ner.Core.ComplexNumToken> lastNums = new Dictionary<string, Pullenti.Ner.Core.ComplexNumToken>();
            Pullenti.Ner.Core.ComplexNumToken ln = Pullenti.Ner.Core.ComplexNumToken.TryParse(xx1.Next, null, false, false);
            if (ln != null) 
                lastNums.Add(ln.Suffix ?? "", ln);
            for (Pullenti.Ner.Token tt = xx1.Next; tt != null; tt = tt.Next) 
            {
                bool stop = false;
                bool isDoubt = false;
                int seqNum = -1;
                if (!tt.IsNewlineAfter && !tt.Next.IsChar((char)0x1E)) 
                {
                    if (tt == xx1.Next) 
                    {
                        PartToken part0 = PartToken.TryAttach(tt, null, false, false);
                        if (part0 != null && part0.Values.Count == 1) 
                        {
                            pstack.Add(part0.Typ, part0.Values[0].Value);
                            tt = part0.EndToken;
                            continue;
                        }
                    }
                    if (tt != null && tt.IsChar((char)0x1E)) 
                    {
                        List<Pullenti.Ner.Core.TableRowToken> rows = Pullenti.Ner.Core.TableHelper.TryParseRows(tt, 0, true, false);
                        if (rows != null && rows.Count > 1) 
                        {
                            tt = rows[rows.Count - 1].EndToken.Previous;
                            continue;
                        }
                    }
                    if (tt.IsCharOf(";.") || ((tt.IsValue("ИСКЛЮЧИТЬ", null) && wordRegime))) 
                    {
                        if (_checkEndBracket(tt.Previous)) 
                        {
                            if (!tt.IsNewlineAfter && tt.Next != null && tt.Next.IsChar('<')) 
                                continue;
                            isDoubt = true;
                            if (((tt.Next is Pullenti.Ner.TextToken) && tt.Next.LengthChar == 1 && tt.Next.Next != null) && tt.Next.Next.IsChar(')')) 
                                stop = true;
                            else if ((tt.Next is Pullenti.Ner.NumberToken) && tt.Next.Next != null && tt.Next.Next.IsCharOf(").")) 
                                stop = true;
                            else 
                            {
                                PartToken part0 = PartToken.TryAttach(tt.Next, null, false, false);
                                if (part0 != null && part0.Values.Count == 1) 
                                {
                                    if (part0.IsNewlineAfter && part0.Morph.Case.IsNominative && !tt.Next.Chars.IsAllLower) 
                                        stop = true;
                                    else 
                                    {
                                        Pullenti.Ner.Core.TerminToken tok1 = m_Terms.TryParse(part0.EndToken.Next, Pullenti.Ner.Core.TerminParseAttr.No);
                                        if (tok1 != null) 
                                            stop = true;
                                    }
                                }
                            }
                            if (tt.IsValue("ИСКЛЮЧИТЬ", null)) 
                            {
                                tt = tt.Previous;
                                stop = true;
                            }
                            if (!stop) 
                            {
                                if (_canBeChangeToken(tt.Next)) 
                                    stop = true;
                            }
                            if (!stop) 
                                continue;
                        }
                        else 
                            continue;
                    }
                    else if ((tt.IsCommaAnd && Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(tt.Previous, true, null, false) && !xx1.IsNewlineBefore) && nls == 0) 
                    {
                        DecreeChangeToken nn = TryAttach(tt.Next, null, true, null, false, false, null);
                        if (nn != null || m_Terms.TryParse(tt.Next, Pullenti.Ner.Core.TerminParseAttr.No) != null) 
                        {
                            stop = true;
                            tt = tt.Previous;
                        }
                        else if (tt.Next != null) 
                        {
                            bool ok = false;
                            if (tt.Next.IsValue("ПОСЛЕДНИЙ", null) || tt.Next.IsValue("ПРЕДПОСЛЕДНИЙ", null) || (((tt.Next is Pullenti.Ner.NumberToken) && tt.Next.Morph.Class.IsAdjective))) 
                                ok = true;
                            if (ok) 
                            {
                                stop = true;
                                tt = tt.Previous;
                            }
                        }
                    }
                    else if (Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(tt, true, null, false)) 
                    {
                        if (tt.IsWhitespaceAfter) 
                        {
                            Pullenti.Ner.Core.ComplexNumToken num = Pullenti.Ner.Core.ComplexNumToken.TryParse(tt.Next, null, false, false);
                            if (num != null) 
                                stop = true;
                            else if (_canBeChangeToken(tt.Next)) 
                                stop = true;
                            else 
                                continue;
                        }
                        else if (tt.Next.IsCharOf(";.") && tt.Previous.IsTableControlChar) 
                            stop = true;
                        else 
                            continue;
                    }
                    else 
                        continue;
                }
                else 
                {
                    if (tt.Next != null && tt.Next.IsValue2("АДМИНИСТРАТИВНАЯ", "ПРОЦЕДУРА")) 
                    {
                    }
                    if (tt.Next is Pullenti.Ner.ReferentToken) 
                    {
                    }
                    if (tt.Next != null && tt.Next.IsChar((char)0x1E)) 
                    {
                        List<Pullenti.Ner.Core.TableRowToken> rows = Pullenti.Ner.Core.TableHelper.TryParseRows(tt.Next, 0, true, false);
                        if (rows != null && rows.Count > 1) 
                        {
                            tt = rows[rows.Count - 1].EndToken.Previous;
                            continue;
                        }
                    }
                    if (tt.Next != null && tt.Next.IsTableControlChar) 
                        continue;
                    nls++;
                    Pullenti.Ner.Core.ComplexNumToken nnn = Pullenti.Ner.Core.ComplexNumToken.TryParse(tt.Next, null, true, false);
                    if (nnn != null) 
                    {
                        if (nnn.GetSourceText() == "11.") 
                        {
                        }
                        seqNum = 0;
                        Pullenti.Ner.Core.ComplexNumToken nnn0;
                        if (lastNums.TryGetValue(nnn.Suffix ?? "", out nnn0)) 
                        {
                            Pullenti.Ner.Core.ComplexNumComparer cmp = new Pullenti.Ner.Core.ComplexNumComparer();
                            cmp.Process(nnn0, nnn);
                            if (cmp.Typ == Pullenti.Ner.Core.ComplexNumCompareType.Less && ((cmp.Delta == 1 || cmp.Delta == 2))) 
                                seqNum = 1;
                            lastNums[nnn.Suffix ?? ""] = nnn;
                            if (seqNum == 1) 
                            {
                                DecreeChangeToken tok2 = TryAttach(tt.Next, null, false, null, false, false, null);
                                if (tok2 == null) 
                                    continue;
                            }
                        }
                        else 
                            lastNums.Add(nnn.Suffix ?? "", nnn);
                        if (tt.IsCharOf(".;") && tt.Next != null && seqNum != 1) 
                        {
                            DecreeChangeToken next1 = TryAttach(tt.Next, null, false, null, false, false, null);
                            if (next1 != null) 
                            {
                                if (next1.ActKind != Pullenti.Ner.Decree.DecreeChangeKind.Undefined) 
                                    stop = true;
                                else if (next1.Typ != DecreeChangeTokenTyp.Undefined && next1.Parts != null && seqNum != 1) 
                                    stop = true;
                                else if (next1.Decree != null && next1.EndToken.IsChar(':')) 
                                    stop = true;
                                else 
                                {
                                    DecreeChangeToken next2 = TryAttach(next1.EndToken.Next, null, false, null, false, false, null);
                                    if (next2 != null && next2.ActKind != Pullenti.Ner.Decree.DecreeChangeKind.Undefined) 
                                        stop = true;
                                }
                            }
                            else if (seqNum != 1) 
                            {
                                Pullenti.Ner.Token tt2 = nnn.EndToken.Next;
                                if (tt2 != null && tt2.IsValue("В", null) && DecreeToken.IsKeyword(tt2.Next, false) != null) 
                                    stop = true;
                            }
                        }
                        else if ((tt.IsComma && tt.Next != null && seqNum != 1) && Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(tt.Previous, true, null, false)) 
                        {
                            DecreeChangeToken next1 = TryAttach(tt.Next, null, false, null, false, false, null);
                            if (next1 != null) 
                            {
                                if (next1.ActKind != Pullenti.Ner.Decree.DecreeChangeKind.Undefined) 
                                    stop = true;
                                else if (next1.Typ != DecreeChangeTokenTyp.Undefined && next1.Parts != null && seqNum != 1) 
                                    stop = true;
                                else if (next1.Decree != null && next1.EndToken.IsChar(':')) 
                                    stop = true;
                            }
                        }
                    }
                    if (tt.Next == null) 
                    {
                    }
                }
                PartToken part = PartToken.TryAttach(tt.Next, null, false, false);
                if ((part != null && part.Values.Count == 1 && tt.IsNewlineAfter) && part.Morph.Case.IsNominative && !tt.Next.Chars.IsAllLower) 
                {
                    if (defPart != null && defPart.ToString() == part.ToString()) 
                    {
                        tt = part.EndToken;
                        continue;
                    }
                    if (defPart != null && defPart.Values.Count == 2 && (defPart.Values[0].IntValue < defPart.Values[1].IntValue)) 
                    {
                        if (part.Values[0].IntValue >= defPart.Values[0].IntValue && part.Values[0].IntValue <= defPart.Values[1].IntValue) 
                        {
                            tt = part.EndToken;
                            continue;
                        }
                    }
                    if (tt != tt.Next && pstack.Count == 0) 
                        stop = true;
                    else if (!pstack.ContainsKey(part.Typ)) 
                        pstack.Add(part.Typ, part.Values[0].Value);
                    else 
                    {
                        int n0;
                        int n1;
                        if (int.TryParse(pstack[part.Typ], out n0) && int.TryParse(part.Values[0].Value, out n1)) 
                        {
                            if ((n0 + 1) == n1) 
                                doubt1 = null;
                        }
                        pstack[part.Typ] = part.Values[0].Value;
                    }
                }
                Pullenti.Ner.Instrument.Internal.InstrToken1 instr = Pullenti.Ner.Instrument.Internal.InstrToken1.Parse(tt.Next, true, null, 0, null, false, 0, false, false);
                if (instr != null && instr.Typ == Pullenti.Ner.Instrument.Internal.InstrToken1.Types.Appendix) 
                {
                }
                if (tt.Next == null) 
                {
                    if (pstack.Count > 0) 
                        stop = true;
                    else if (_checkEndBracket(tt) || tt.IsCharOf(";.")) 
                        stop = true;
                }
                else if (instr != null && instr.Numbers.Count > 0 && !stop) 
                {
                    if (isDoubt) 
                        stop = true;
                    else if (abzacRegime && !Pullenti.Ner.Core.BracketHelper.IsBracket(xx1, true)) 
                        stop = true;
                    else if (!tt.IsCharOf(":")) 
                    {
                        DecreeChangeToken nn = TryAttach(tt.Next, null, false, null, false, false, null);
                        if (nn != null && nn.Parts != null) 
                        {
                            if (nn.EndToken.IsNewlineAfter || nn.ActKind != Pullenti.Ner.Decree.DecreeChangeKind.Undefined) 
                                stop = true;
                            else 
                            {
                                DecreeChangeToken nnn = TryAttach(nn.EndToken.Next, null, false, null, false, false, null);
                                if (nnn != null) 
                                {
                                    if (nnn.ChangeVal != null || nnn.ActKind != Pullenti.Ner.Decree.DecreeChangeKind.Undefined) 
                                        stop = true;
                                }
                            }
                        }
                    }
                }
                bool hasNext = false;
                DecreeChangeToken dcNext = TryAttach(tt.Next, null, false, null, true, false, null);
                if (dcNext == null) 
                    dcNext = TryAttach(tt.Next, null, true, null, true, false, null);
                if (tt.Next == null) 
                {
                }
                else if (tt.Next.IsValue("ПОСЛЕ", null)) 
                {
                }
                else if (dcNext != null && ((dcNext.IsStart || dcNext.ChangeVal != null || dcNext.Typ == DecreeChangeTokenTyp.Undefined))) 
                    hasNext = true;
                else 
                {
                    isDoubt = true;
                    PartToken pt = PartToken.TryAttach(tt.Next, null, false, false);
                    if (pt != null && pt.Typ == PartToken.ItemType.Clause && ((pt.IsNewlineAfter || ((pt.EndToken.Next != null && pt.EndToken.Next.IsChar('.')))))) 
                    {
                        isDoubt = false;
                        if (clauseLast != null && instr != null && Pullenti.Ner.Instrument.Internal.NumberingHelper.CalcDelta(clauseLast, instr, true) == 1) 
                            isDoubt = true;
                    }
                }
                if (instr != null && instr.Typ == Pullenti.Ner.Instrument.Internal.InstrToken1.Types.Clause) 
                    clauseLast = instr;
                if (isDoubt && instr != null) 
                {
                    for (Pullenti.Ner.Token ttt = tt; ttt != null && ttt.EndChar <= instr.EndChar; ttt = ttt.Next) 
                    {
                        if (ttt.IsValue("УТРАТИТЬ", "ВТРАТИТИ") && ttt.Next != null && ttt.Next.IsValue("СИЛА", "ЧИННІСТЬ")) 
                        {
                            isDoubt = false;
                            break;
                        }
                    }
                }
                if (Pullenti.Ner.Core.BracketHelper.IsBracket(tt.Next, true)) 
                {
                    if (tt.Next.IsNewlineAfter) 
                    {
                        tt = tt.Next;
                        stop = true;
                    }
                    else if (tt.Next.Next != null && tt.Next.Next.IsCharOf(":.") && tt.Next.Next.IsNewlineAfter) 
                    {
                        tt = tt.Next.Next;
                        stop = true;
                    }
                }
                res.EndToken = tt;
                Pullenti.Ner.Token tt1 = tt;
                int hcou = 0;
                Pullenti.Ner.Token tt3;
                for (tt3 = tt.Next; tt3 != null; tt3 = tt3.Next) 
                {
                    if (tt3.IsHiphen || tt3.IsChar('_')) 
                        hcou++;
                    else 
                        break;
                }
                if (hcou > 4) 
                {
                    if (tt3 == null) 
                        stop = true;
                    else if (DecreeToken.IsKeyword(tt3, false) != null) 
                        stop = true;
                    else if (tt3.GetReferent() != null && ((tt3.GetReferent().TypeName == "PERSON" || tt3.GetReferent().TypeName == "PERSONPROPERTY"))) 
                        stop = true;
                }
                if (tt1.Next != null && tt1.Next.IsValue("СТАТЬЯ", null) && !tt1.Next.Chars.IsAllLower) 
                {
                    PartToken pt = PartToken.TryAttach(tt1.Next, null, false, false);
                    if (pt != null && pt.IsNewlineAfter) 
                    {
                        DecreeChangeToken chr = DecreeChangeToken.TryAttach(pt.EndToken.Next, null, false, null, false, false, null);
                        if (chr != null && ((chr.Typ == DecreeChangeTokenTyp.StartMultu || chr.Typ == DecreeChangeTokenTyp.StartSingle))) 
                            stop = true;
                    }
                }
                Pullenti.Ner.Core.TerminToken tok = m_Terms.TryParse(tt1.Next, Pullenti.Ner.Core.TerminParseAttr.No);
                if (tok != null) 
                {
                    if (tok.EndToken == tt1.Next && tt1.Next.Morph.ContainsAttr("к.ф.", null)) 
                    {
                    }
                    else if (tok.Termin.Tag is Pullenti.Ner.Decree.DecreeChangeKind) 
                        stop = true;
                }
                else 
                {
                }
                if (tt1.Next != null) 
                {
                    if (tt1.Next.GetReferent() != null && ((tt1.Next.GetReferent().TypeName == "PERSON" || tt1.Next.GetReferent().TypeName == "PERSONPROPERTY"))) 
                    {
                        if ((tt1.Next as Pullenti.Ner.ReferentToken).BeginToken.Chars.IsAllLower) 
                        {
                        }
                        else if (tt1.Next.LengthChar < 10) 
                        {
                        }
                        else 
                            stop = true;
                    }
                }
                if (tt1.IsCharOf(";.") && tt1.IsNewlineAfter && seqNum == 0) 
                {
                    if (_checkEndBracket(tt1.Previous)) 
                        stop = true;
                }
                if (!stop && tt1.IsNewlineAfter) 
                {
                    Pullenti.Ner.Core.ComplexNumToken num = Pullenti.Ner.Core.ComplexNumToken.TryParse(tt1.Next, null, false, false);
                    if (num != null && ((num.Suffix != null || ((num.EndToken.IsValue("В", null) && num.Nums.Count > 1))))) 
                    {
                        if (((tt1.IsCharOf(";.") && Pullenti.Ner.Core.BracketHelper.IsBracket(tt1.Previous, true))) || ((tt1.Previous.IsCharOf(";.") && Pullenti.Ner.Core.BracketHelper.IsBracket(tt1, true)))) 
                        {
                            Pullenti.Ner.Token tt2 = num.EndToken.Next;
                            if (tt2 != null) 
                            {
                                if (tt2.IsValue("ВНЕСТИ", null) || tt2.IsValue("В", null) || tt2.IsValue("ДОПОЛНИТЬ", null)) 
                                    stop = true;
                                else 
                                {
                                    PartToken pp = PartToken.TryAttach(tt2, null, false, false);
                                    if (pp != null && pp.Typ != PartToken.ItemType.Prefix) 
                                        stop = true;
                                }
                            }
                            if (num.EndToken.IsValue("В", null) && num.Nums.Count > 1) 
                                stop = true;
                            if (tt1.Previous != null && tt1.Previous.Previous != null && tt1.Previous.Previous.IsChar(']')) 
                                stop = true;
                        }
                    }
                }
                if (tt1.IsCharOf(";.") && Pullenti.Ner.Core.BracketHelper.IsBracket(tt1.Next, false)) 
                {
                    if (tt1.Next.IsNewlineAfter) 
                    {
                        tt1 = tt1.Next;
                        stop = true;
                    }
                    else if (tt1.Next.Next.IsCharOf(";.")) 
                    {
                        tt1 = tt1.Next.Next;
                        stop = true;
                    }
                }
                if (stop) 
                {
                    bool brClose = false;
                    if (tt1.IsCharOf("<>")) 
                    {
                        if (tt.Next != null) 
                            tt = tt.Next;
                        continue;
                    }
                    if (tt1.IsCharOf(";.") && Pullenti.Ner.Core.BracketHelper.IsBracket(tt1.Previous, true)) 
                    {
                        res.EndToken = tt1.Previous;
                        tt1 = tt1.Previous.Previous;
                        brClose = true;
                        if (tt1.IsChar('>') && tt1.Next.IsChar('>')) 
                            tt1 = tt1.Previous;
                    }
                    else if (tt1.IsCharOf(";")) 
                    {
                        if (tt1.Previous.IsChar('.')) 
                            res.EndToken = (tt1 = tt1.Previous);
                    }
                    if (!brClose && Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(tt1, true, null, false)) 
                    {
                        tt1 = tt1.Previous;
                        brClose = true;
                        if (tt1.IsChar('>') && tt1.Next.IsChar('>')) 
                            tt1 = tt1.Previous;
                    }
                    if (tt1 == null || t0.BeginChar > tt1.BeginChar) 
                        return null;
                    res.ChangeVal.Value = (new Pullenti.Ner.MetaToken(t0, tt1)).GetSourceText();
                    if (tt1.Next == null) 
                        res.ChangeVal.EndChar = tt1.EndChar;
                    else 
                        res.ChangeVal.EndChar = tt1.Next.BeginChar - 1;
                    return res;
                }
                if (tt.IsChar(')')) 
                {
                    for (Pullenti.Ner.Token tt2 = tt.Previous; tt2 != null; tt2 = tt2.Previous) 
                    {
                        if (tt2.IsChar('(') && tt2.Previous != null && tt2.Previous.EndChar > res.BeginChar) 
                        {
                            tt1 = tt2.Previous;
                            break;
                        }
                        else if (tt2.IsNewlineAfter) 
                            break;
                    }
                }
                bool closePoint = false;
                if (tt1.IsCharOf(";.")) 
                {
                    closePoint = true;
                    tt1 = (res.EndToken = tt1.Previous);
                }
                if (tt1.IsCharOf(";.")) 
                {
                    closePoint = true;
                    tt1 = (res.EndToken = tt1.Previous);
                }
                bool closeBr = false;
                if (Pullenti.Ner.Core.BracketHelper.IsBracket(tt1, true) && !tt1.IsCharOf("]>")) 
                {
                    closeBr = true;
                    tt1 = tt1.Previous;
                }
                else if (_checkEndBracket(tt1)) 
                {
                }
                else if (tt1.Next == null) 
                {
                }
                else if (hasNext && tt1.IsTableControlChar) 
                {
                }
                else 
                    continue;
                if (isDoubt) 
                {
                    if (tt.IsCharOf("<>[]")) 
                        continue;
                    if (doubt1 == null) 
                        doubt1 = tt1;
                    if (!closePoint || !closeBr) 
                        continue;
                    if (!Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t0.Previous, true, false)) 
                        continue;
                    List<Pullenti.Ner.Token> brs = new List<Pullenti.Ner.Token>();
                    brs.Add(t0.Previous);
                    for (Pullenti.Ner.Token ttt = t0; ttt != null && ttt.EndChar <= tt1.Next.EndChar; ttt = ttt.Next) 
                    {
                        if (!Pullenti.Ner.Core.BracketHelper.IsBracket(ttt, false)) 
                            continue;
                        if (brs.Count > 0 && Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(ttt, false, brs[0], false)) 
                            brs.RemoveAt(0);
                        else 
                            brs.Insert(0, ttt);
                    }
                    if (brs.Count > 0) 
                    {
                        if ((instr != null && instr.Typ == Pullenti.Ner.Instrument.Internal.InstrToken1.Types.Appendix && closeBr) && closePoint) 
                        {
                        }
                        else 
                            continue;
                    }
                }
                if (tt1.BeginChar > xx1.EndChar) 
                {
                    for (; tt1.Next != null; tt1 = tt1.Next) 
                    {
                        if (!tt1.Next.IsTableControlChar) 
                            break;
                    }
                    res.ChangeVal.Value = (new Pullenti.Ner.MetaToken(t0, tt1)).GetSourceText();
                    if (tt1.Next == null) 
                        res.ChangeVal.EndChar = tt1.EndChar;
                    else 
                        res.ChangeVal.EndChar = tt1.Next.BeginChar - 1;
                    if (res.EndChar < tt1.EndChar) 
                        res.EndToken = tt1;
                    if (tt1.Next != null && Pullenti.Ner.Core.BracketHelper.IsBracket(tt1.Next, true)) 
                    {
                        if (tt1.Next.Next != null && tt1.Next.Next.IsCharOf(";.")) 
                            res.EndToken = tt1.Next.Next;
                        else if (tt1.Next.IsNewlineAfter) 
                            res.EndToken = tt1.Next;
                    }
                    return res;
                }
                break;
            }
            if (doubt1 != null) 
            {
                res.ChangeVal.Value = (new Pullenti.Ner.MetaToken(t0, doubt1)).GetSourceText();
                if (doubt1.Next == null) 
                    res.ChangeVal.EndChar = doubt1.EndChar;
                else 
                    res.ChangeVal.EndChar = doubt1.Next.BeginChar - 1;
                res.EndToken = doubt1;
                if (Pullenti.Ner.Core.BracketHelper.IsBracket(doubt1.Next, true)) 
                    res.EndToken = doubt1.Next;
                return res;
            }
            return null;
        }
        public static bool _checkEndBracket(Pullenti.Ner.Token t)
        {
            if (Pullenti.Ner.Core.BracketHelper.IsBracket(t, true)) 
            {
                if (t.IsChar('>') && t.Previous != null && t.Previous.IsChar('>')) 
                    return true;
                return !t.IsCharOf(">]");
            }
            if (t is Pullenti.Ner.MetaToken) 
            {
                if (Pullenti.Ner.Core.BracketHelper.IsBracket((t as Pullenti.Ner.MetaToken).EndToken, true)) 
                    return !(t as Pullenti.Ner.MetaToken).EndToken.IsCharOf(">]");
                if ((t as Pullenti.Ner.MetaToken).EndToken is Pullenti.Ner.MetaToken) 
                {
                    if (Pullenti.Ner.Core.BracketHelper.IsBracket(((t as Pullenti.Ner.MetaToken).EndToken as Pullenti.Ner.MetaToken).EndToken, true)) 
                        return !((t as Pullenti.Ner.MetaToken).EndToken as Pullenti.Ner.MetaToken).EndToken.IsCharOf(">]");
                }
            }
            return false;
        }
        public DecreeChangeToken(Pullenti.Ner.Token b, Pullenti.Ner.Token e) : base(b, e, null)
        {
        }
        public DecreeChangeTokenTyp Typ;
        public Pullenti.Ner.Decree.DecreeReferent Decree;
        public List<Pullenti.Ner.Decree.DecreeReferent> AddDecrees;
        public DecreeToken DecreeTok;
        public List<PartToken> Parts;
        public List<List<PartToken>> AddParts;
        public List<PartToken> NewParts;
        public PartToken AppExtChanges;
        public Pullenti.Ner.Decree.DecreePartReferent RealPart;
        public List<Pullenti.Ner.Decree.DecreePartReferent> AddRealParts;
        public Pullenti.Ner.Token InPart;
        public Pullenti.Ner.Decree.DecreeChangeValueReferent ChangeVal;
        public bool HasText;
        public bool Ignorable;
        public bool InTheEnd;
        public bool HasAnafor;
        public bool HasChangeKeyword;
        public Pullenti.Ner.Decree.DecreeChangeKind ActKind;
        public Pullenti.Ner.Decree.DecreeChangeKind ActKind2;
        public PartToken.ItemType PartTyp = PartToken.ItemType.Undefined;
        public bool IndentRegime
        {
            get
            {
                if (NewParts != null) 
                {
                    if (NewParts.Count == 1 && PartToken._getRank(NewParts[0].Typ) >= PartToken._getRank(PartToken.ItemType.Item)) 
                    {
                        if (NewParts[0].Values.Count > 1) 
                            return false;
                        return true;
                    }
                    return false;
                }
                if ((ActKind != Pullenti.Ner.Decree.DecreeChangeKind.Append && Parts != null && !Parts[0].DelimAfter) && Parts[0].Values.Count <= 1 && PartToken._getRank(Parts[0].Typ) >= PartToken._getRank(PartToken.ItemType.Item)) 
                    return true;
                return false;
            }
        }
        public override string ToString()
        {
            StringBuilder tmp = new StringBuilder();
            tmp.Append(Typ.ToString());
            if (ActKind != Pullenti.Ner.Decree.DecreeChangeKind.Undefined) 
                tmp.AppendFormat(" Kind={0}", ActKind.ToString());
            if (ActKind2 != Pullenti.Ner.Decree.DecreeChangeKind.Undefined) 
                tmp.AppendFormat(" Kind2={0}", ActKind2.ToString());
            if (HasText) 
                tmp.Append(" HasText");
            if (Ignorable) 
                tmp.Append(" Ignorable");
            if (InTheEnd) 
                tmp.Append(" InTheEnd");
            if (HasAnafor) 
                tmp.Append(" HasAnafor");
            if (InPart != null) 
                tmp.Append(" InPart");
            if (HasChangeKeyword) 
                tmp.Append(" HasChangeKeyword");
            if (AppExtChanges != null) 
                tmp.AppendFormat(" ExtChanges={0}", AppExtChanges);
            if (Parts != null) 
            {
                foreach (PartToken p in Parts) 
                {
                    tmp.AppendFormat(" {0}", p);
                }
            }
            if (RealPart != null) 
                tmp.AppendFormat(" RealPart={0}", RealPart.ToString());
            if (NewParts != null) 
            {
                foreach (PartToken p in NewParts) 
                {
                    tmp.AppendFormat(" New={0}", p);
                }
            }
            if (PartTyp != PartToken.ItemType.Undefined) 
                tmp.AppendFormat(" PTyp={0}", PartTyp.ToString());
            if (DecreeTok != null) 
                tmp.AppendFormat(" DecTok={0}", DecreeTok.ToString());
            if (Decree != null) 
                tmp.AppendFormat(" Ref={0}", Decree.ToStringEx(true, null, 0));
            if (ChangeVal != null) 
                tmp.AppendFormat(" ChangeVal={0}", ChangeVal.ToStringEx(true, null, 0));
            if (IndentRegime) 
                tmp.Append(" IndentRegime!");
            return tmp.ToString();
        }
        public bool IsStart
        {
            get
            {
                return Typ == DecreeChangeTokenTyp.StartSingle || Typ == DecreeChangeTokenTyp.StartMultu || Typ == DecreeChangeTokenTyp.Single;
            }
        }
        void _skipDecreeDummy()
        {
            bool ignoreDecree = false;
            for (Pullenti.Ner.Token tt = EndToken.Next; tt != null; tt = tt.Next) 
            {
                if (tt.IsCommaAnd) 
                    continue;
                if (tt.IsValue2("С", "ИЗМЕНЕНИЕ")) 
                {
                    tt = tt.Next.Next;
                    if (tt == null) 
                        break;
                    if (tt.IsComma) 
                        tt = tt.Next;
                    if (tt != null && tt.IsValue("ВНЕСЕННЫЙ", null)) 
                    {
                        tt = tt.Next;
                        ignoreDecree = true;
                    }
                    if (tt == null) 
                        break;
                    continue;
                }
                List<PartToken> li = PartToken.TryAttachList(tt, false, 40);
                if (li != null && li.Count > 0) 
                {
                    if (!ignoreDecree) 
                        break;
                    if (li[0].Morph.Case.IsInstrumental) 
                        break;
                    EndToken = (tt = li[li.Count - 1].EndToken);
                    continue;
                }
                if (tt.IsChar('(')) 
                {
                    Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(tt, Pullenti.Ner.Core.BracketParseAttr.CanBeManyLines, 3000);
                    if (br != null) 
                    {
                        EndToken = (tt = br.EndToken);
                        continue;
                    }
                }
                if (tt.GetReferent() is Pullenti.Ner.Decree.DecreeReferent) 
                {
                    if (!ignoreDecree) 
                        break;
                    EndToken = tt;
                    continue;
                }
                break;
            }
        }
        public static DecreeChangeToken TryAttach(Pullenti.Ner.Token t, Pullenti.Ner.Decree.DecreeChangeReferent main = null, bool ignoreNewlines = false, List<Pullenti.Ner.Decree.DecreePartReferent> changeStack = null, bool isInEdition = false, bool abzacRegime = false, PartToken defPart = null)
        {
            if (t == null) 
                return null;
            Pullenti.Ner.Token tt = t;
            bool isNewlineBefore = t.IsNewlineBefore;
            if ((!isNewlineBefore && t.Previous != null && t.Previous.IsChar(':')) && t.WhitespacesBeforeCount > 0) 
                isNewlineBefore = true;
            if ((t.Previous != null && ((t.Previous.IsCharOf(";.") || t.Previous.IsCommaAnd)) && t.Previous.Previous != null) && (t.Previous.Previous.GetReferent() is Pullenti.Ner.Decree.DecreeChangeReferent)) 
                isNewlineBefore = true;
            else if (t.Previous != null && (t.Previous.GetReferent() is Pullenti.Ner.Decree.DecreeChangeReferent)) 
                isNewlineBefore = true;
            if ((!isNewlineBefore && t.Previous != null && t.Previous.IsTableControlChar) && t.Previous.IsNewlineBefore) 
                isNewlineBefore = true;
            if ((isNewlineBefore && t.Previous != null && t.Previous.IsValue("РЕДАКЦИЯ", null)) && Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t, true, false)) 
                isNewlineBefore = false;
            if (isNewlineBefore && !ignoreNewlines) 
            {
                for (tt = t; tt != null; tt = tt.Next) 
                {
                    if (tt.IsChar('[')) 
                    {
                        Pullenti.Ner.Token ii = Pullenti.Ner.Core.MiscHelper.CheckImage(tt);
                        if (ii != null && !ii.IsNewlineAfter && ii.Next != null) 
                        {
                            tt = ii;
                            continue;
                        }
                    }
                    if ((tt == t && (tt is Pullenti.Ner.TextToken) && (((tt as Pullenti.Ner.TextToken).Term == "СТАТЬЯ" || (tt as Pullenti.Ner.TextToken).Term == "СТАТТЯ"))) && (tt.Next is Pullenti.Ner.NumberToken)) 
                    {
                        Pullenti.Ner.Token tt1 = tt.Next.Next;
                        if (tt1 != null && tt1.IsChar('.')) 
                        {
                            tt1 = tt1.Next;
                            if (tt1 != null && !tt1.IsNewlineBefore && tt1.IsValue("ВНЕСТИ", "УНЕСТИ")) 
                                continue;
                            if (tt1 != null && tt1.IsNewlineBefore) 
                                return null;
                            tt = tt1;
                        }
                        break;
                    }
                    else if (tt == t && PartToken.TryAttach(tt, null, false, false) != null) 
                    {
                        if ((tt is Pullenti.Ner.NumberToken) && tt.Next != null && tt.Next.IsChar(')')) 
                            tt = tt.Next.Next;
                        break;
                    }
                    else if ((tt is Pullenti.Ner.NumberToken) && (tt as Pullenti.Ner.NumberToken).Typ == Pullenti.Ner.NumberSpellingType.Digit) 
                    {
                    }
                    else if (tt.IsHiphen || tt.IsChar(')')) 
                    {
                    }
                    else if ((tt is Pullenti.Ner.TextToken) && !tt.Chars.IsLetter && !tt.IsWhitespaceBefore) 
                    {
                    }
                    else if (((tt is Pullenti.Ner.TextToken) && tt.LengthChar == 1 && (tt.Next is Pullenti.Ner.TextToken)) && !tt.Next.Chars.IsLetter) 
                    {
                    }
                    else 
                        break;
                }
            }
            if (tt == null) 
                return null;
            DecreeChangeToken res = null;
            Pullenti.Ner.Token te1 = null;
            bool hasChange = false;
            if ((tt is Pullenti.Ner.TextToken) && t.IsNewlineBefore && !ignoreNewlines) 
            {
                bool enter = false;
                if (tt.IsValue("ВНЕСТИ", "УНЕСТИ") && ((((tt.Next != null && tt.Next.IsValue("В", "ДО"))) || (tt as Pullenti.Ner.TextToken).Term == "ВНЕСТИ" || (tt as Pullenti.Ner.TextToken).Term == "УНЕСТИ"))) 
                {
                    te1 = tt;
                    if (tt.Next != null && tt.Next.IsValue("В", "ДО")) 
                        te1 = tt.Next;
                    enter = true;
                }
                else if (tt.IsValue("ИЗМЕНЕНИЕ", null) && tt.Next != null && tt.Next.IsComma) 
                {
                    hasChange = true;
                    for (Pullenti.Ner.Token ttt = tt.Next.Next; ttt != null; ttt = ttt.Next) 
                    {
                        if (ttt.IsValue("КОТОРЫЙ", null)) 
                            continue;
                        if (ttt.IsValue("ВНОСИТЬСЯ", null) || ttt.IsValue("ВНОСИМЫЙ", null)) 
                            continue;
                        if (ttt.IsValue("В", "ДО")) 
                        {
                            te1 = ttt;
                            break;
                        }
                        break;
                    }
                }
            }
            if (te1 != null) 
            {
                res = new DecreeChangeToken(tt, te1) { Typ = DecreeChangeTokenTyp.StartMultu, HasChangeKeyword = true };
                for (tt = te1.Next; tt != null; tt = tt.Next) 
                {
                    if (tt.IsNewlineBefore) 
                    {
                        if (Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(tt)) 
                            break;
                    }
                    if (tt.IsChar(':') && tt.WhitespacesAfterCount > 0) 
                    {
                        res.EndToken = tt;
                        break;
                    }
                    if (tt.GetReferent() is Pullenti.Ner.Decree.DecreeReferent) 
                    {
                        Pullenti.Ner.Decree.DecreeReferent dr = tt.GetReferent() as Pullenti.Ner.Decree.DecreeReferent;
                        if (res.Decree == null) 
                        {
                            if (dr.Kind == Pullenti.Ner.Decree.DecreeKind.Project) 
                                return null;
                            res.Decree = dr;
                        }
                        else 
                        {
                            if (res.AddDecrees == null) 
                                res.AddDecrees = new List<Pullenti.Ner.Decree.DecreeReferent>();
                            res.AddDecrees.Add(dr);
                        }
                        res.EndToken = tt;
                        res._skipDecreeDummy();
                        tt = res.EndToken;
                        continue;
                    }
                    if (hasChange && tt.IsValue2("СОГЛАСНО", "ПРИЛОЖЕНИЕ")) 
                    {
                        res.AppExtChanges = PartToken.TryAttach(tt.Next, null, false, false);
                        if (res.AppExtChanges != null && res.AppExtChanges.Values.Count == 1) 
                        {
                            res.EndToken = (tt = res.AppExtChanges.EndToken);
                            continue;
                        }
                    }
                    List<PartToken> li = PartToken.TryAttachList(tt, false, 40);
                    if (li != null && li.Count > 0) 
                    {
                        if (res.Parts != null) 
                            break;
                        res.Parts = li;
                        tt = (res.EndToken = li[li.Count - 1].EndToken);
                        continue;
                    }
                    if (tt.IsChar('(')) 
                    {
                        Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(tt, Pullenti.Ner.Core.BracketParseAttr.CanBeManyLines, 100);
                        if (br != null) 
                        {
                            tt = br.EndToken;
                            continue;
                        }
                    }
                    if (tt.IsNewlineBefore) 
                        break;
                    res.EndToken = tt;
                    if (tt.IsCommaAnd && tt.Next != null && (tt.Next.GetReferent() is Pullenti.Ner.Decree.DecreeReferent)) 
                        continue;
                    if (tt.IsChar(',') && hasChange) 
                    {
                        res.Typ = DecreeChangeTokenTyp.StartSingle;
                        if (res.Parts != null && tt.Next != null && tt.Next.IsValue("ИЗЛОЖИТЬ", null)) 
                        {
                            List<PartToken> li2 = PartToken.TryAttachList(tt.Next.Next, false, 40);
                            if (li2 != null && li2.Count == 1) 
                            {
                                li2[0].BeginToken = (li2[0].EndToken = res.Parts[res.Parts.Count - 1].EndToken);
                                res.Parts.Add(li2[0]);
                            }
                        }
                        break;
                    }
                    if (tt.IsValue("ИЗМЕНЕНИЕ", "ЗМІНА") || tt.IsValue("ДОПОЛНЕНИЕ", "ДОДАТОК") || tt.IsValue("ВНЕСТИ", null)) 
                        hasChange = true;
                    else if (tt.IsValue("СЛЕДУЮЩИЙ", "НАСТУПНИЙ")) 
                    {
                    }
                    else if (tt.IsValue("ТАКОЙ", "ТАКИЙ")) 
                    {
                    }
                    else if (tt.IsValue2("С", "ИЗМЕНЕНИЕ")) 
                    {
                        tt = tt.Next;
                        if (tt.Next != null && tt.Next.IsComma) 
                            tt = tt.Next;
                    }
                }
                if (!hasChange && !res.HasChangeKeyword) 
                    return null;
                if (res.Decree == null) 
                    return null;
                tt = res.EndToken.Next;
                res.HasChangeKeyword = true;
                if (res.Typ == DecreeChangeTokenTyp.StartSingle && res.Parts == null && tt != null) 
                {
                    if ((tt.IsValue("ИЗЛОЖИВ", "ВИКЛАВШИ") || tt.IsValue("ДОПОЛНИВ", "ДОПОВНИВШИ") || tt.IsValue("ИСКЛЮЧИВ", "ВИКЛЮЧИВШИ")) || tt.IsValue("ЗАМЕНИВ", "ЗАМІНИВШИ")) 
                    {
                        tt = tt.Next;
                        if (tt != null && tt.Morph.Class.IsPreposition) 
                            tt = tt.Next;
                        res.Parts = PartToken.TryAttachList(tt, false, 40);
                        if (res.Parts != null) 
                        {
                            tt = res.EndToken.Next;
                            if (tt.IsValue("ДОПОЛНИВ", "ДОПОВНИВШИ")) 
                                res.ActKind = Pullenti.Ner.Decree.DecreeChangeKind.Append;
                            else if (tt.IsValue("ИСКЛЮЧИВ", "ВИКЛЮЧИВШИ")) 
                                res.ActKind = Pullenti.Ner.Decree.DecreeChangeKind.Remove;
                            else if (tt.IsValue("ИЗЛОЖИВ", "ВИКЛАВШИ")) 
                                res.ActKind = Pullenti.Ner.Decree.DecreeChangeKind.New;
                            else if (tt.IsValue("ЗАМЕНИВ", "ЗАМІНИВШИ")) 
                                res.ActKind = Pullenti.Ner.Decree.DecreeChangeKind.Exchange;
                            res.EndToken = res.Parts[res.Parts.Count - 1].EndToken;
                            if (res.ActKind == Pullenti.Ner.Decree.DecreeChangeKind.Append) 
                            {
                                List<PartToken> pp = PartToken.TryAttachList(res.EndToken.Next, false, 40);
                                if (pp != null && pp.Count == 1) 
                                {
                                    res.NewParts = pp;
                                    res.EndToken = pp[0].EndToken;
                                }
                            }
                        }
                    }
                }
                return res;
            }
            Pullenti.Ner.Token ttExpire = null;
            bool isSuspend = false;
            if (!ignoreNewlines && isNewlineBefore) 
            {
                for (Pullenti.Ner.Token tt2 = tt; tt2 != null; tt2 = tt2.Next) 
                {
                    if (tt2 != tt && tt2.IsNewlineBefore) 
                        break;
                    if (tt2.GetReferent() != null) 
                        continue;
                    if (tt2.IsChar('(')) 
                    {
                        Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(tt2, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                        if (br != null) 
                        {
                            tt2 = br.EndToken;
                            continue;
                        }
                    }
                    DecreeToken dt = DecreeToken.TryAttach(tt2, null, false);
                    if (dt != null && dt.Typ == DecreeToken.ItemType.Typ) 
                    {
                        tt2 = dt.EndToken;
                        continue;
                    }
                    Pullenti.Morph.MorphClass mc = tt2.GetMorphClassInDictionary();
                    if (mc.IsPreposition || ((mc.IsAdjective && !mc.IsVerb)) || mc.IsPronoun) 
                        continue;
                    if (tt2.IsCommaAnd) 
                        continue;
                    if (tt2.IsValue("МОМЕНТ", null) || tt2.IsValue("ДЕНЬ", null)) 
                        continue;
                    if (tt2.IsValue3("ВСТУПЛЕНИЕ", "В", "СИЛУ")) 
                    {
                        tt2 = tt2.Next.Next;
                        continue;
                    }
                    if ((tt2.IsValue("ПРИЗНАТЬ", "ВИЗНАТИ") || tt2.IsValue("ПРИЗНАВАЕМЫЙ", null) || tt2.IsValue("СЧИТАТЬ", "ВВАЖАТИ")) || tt2.IsValue("ПЕРЕЧЕНЬ", null)) 
                    {
                        Pullenti.Ner.Token tt3 = tt2.Next;
                        if (tt3 != null && tt3.IsComma) 
                            tt3 = tt3.Next;
                        if (tt3 == null) 
                            continue;
                        if (tt3.IsValue("УТРАТИТЬ", "ВТРАТИТИ")) 
                        {
                            if (tt3.Next != null && tt3.Next.IsValue("СИЛА", "ЧИННІСТЬ")) 
                            {
                                ttExpire = tt3.Next;
                                break;
                            }
                        }
                    }
                    if (tt.IsValue("ПРИОСТАНОВИТЬ", null) && tt.Next != null) 
                    {
                        if (tt.Next.IsValue("ДЕЙСТВИЕ", null) || tt.Next.IsValue("С", null)) 
                        {
                            ttExpire = tt.Next;
                            isSuspend = true;
                            break;
                        }
                    }
                    break;
                }
            }
            if (ttExpire != null) 
            {
                res = new DecreeChangeToken(tt, ttExpire) { Typ = DecreeChangeTokenTyp.Action, ActKind = (isSuspend ? Pullenti.Ner.Decree.DecreeChangeKind.Suspend : Pullenti.Ner.Decree.DecreeChangeKind.Expire) };
                bool dayAfter = false;
                for (; tt != null; tt = tt.Next) 
                {
                    if (tt.IsChar(':') && tt.EndChar > ttExpire.EndChar) 
                    {
                        res.Typ = DecreeChangeTokenTyp.StartMultu;
                        if (tt.EndChar > res.EndChar) 
                            res.EndToken = tt;
                        break;
                    }
                    if (tt.IsValue2("ДЕНЬ", "ВВЕДЕНИЯ")) 
                        dayAfter = true;
                    if (tt.GetReferent() is Pullenti.Ner.Decree.DecreeReferent) 
                    {
                        if (!dayAfter) 
                        {
                            if (res.Decree != null) 
                                break;
                            if (tt.Previous != null && tt.Previous.IsValue("СОГЛАСНО", null)) 
                            {
                            }
                            else 
                            {
                                res.Typ = DecreeChangeTokenTyp.StartSingle;
                                res.Decree = tt.GetReferent() as Pullenti.Ner.Decree.DecreeReferent;
                            }
                        }
                        if (tt.EndChar > res.EndChar) 
                            res.EndToken = tt;
                        res._skipDecreeDummy();
                        tt = res.EndToken;
                        continue;
                    }
                    List<PartToken> li = PartToken.TryAttachList(tt, false, 40);
                    if (li != null && li.Count > 0 && !tt.IsValue("СИЛА", null)) 
                    {
                        if (!dayAfter) 
                        {
                            if (res.Parts != null) 
                                break;
                            if (tt.Previous != null && tt.Previous.IsValue("СОГЛАСНО", null)) 
                            {
                            }
                            else if (tt.IsValue("СОГЛАСНО", null)) 
                            {
                            }
                            else 
                            {
                                res.Typ = DecreeChangeTokenTyp.StartSingle;
                                res.Parts = li;
                            }
                        }
                        tt = li[li.Count - 1].EndToken;
                        if (tt.EndChar > res.EndChar) 
                            res.EndToken = tt;
                        continue;
                    }
                    if (tt.IsChar('(')) 
                    {
                        Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(tt, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                        if (br != null) 
                        {
                            tt = br.EndToken;
                            continue;
                        }
                    }
                    Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                    if (npt != null) 
                    {
                        if (npt.EndToken.IsValue("АКТ", null) || npt.EndToken.IsValue("ПОЛОЖЕНИЕ", null)) 
                        {
                            tt = npt.EndToken;
                            if (tt.EndChar > res.EndChar) 
                                res.EndToken = tt;
                            continue;
                        }
                    }
                    Pullenti.Ner.Referent r = tt.GetReferent();
                    if (r != null && r.TypeName == "ORGANIZATION") 
                    {
                        if (tt.EndChar > res.EndChar) 
                            res.EndToken = tt;
                        continue;
                    }
                    if (tt.IsNewlineBefore && tt.BeginChar > ttExpire.BeginChar) 
                    {
                        res.Typ = DecreeChangeTokenTyp.StartMultu;
                        break;
                    }
                    if (tt.IsValue("ПОЗИЦИЯ", null) && tt.Next != null && tt.Next.IsChar(':')) 
                    {
                        DecreeChangeToken d = _tryParseText(tt.Next.Next, false, false, null);
                        if (d != null) 
                        {
                            res.ActKind = Pullenti.Ner.Decree.DecreeChangeKind.Remove;
                            res.ChangeVal = d.ChangeVal;
                            res.Typ = DecreeChangeTokenTyp.Action;
                            tt = (res.EndToken = d.EndToken);
                            break;
                        }
                    }
                }
                return res;
            }
            if ((!ignoreNewlines && ((isNewlineBefore || tt == t)) && tt.IsValue("УТРАТИТЬ", "ВТРАТИТИ")) && tt.Next != null && tt.Next.IsValue("СИЛА", "ЧИННІСТЬ")) 
            {
                res = new DecreeChangeToken(tt, tt.Next) { Typ = DecreeChangeTokenTyp.Undefined };
                for (tt = tt.Next; tt != null; tt = tt.Next) 
                {
                    res.EndToken = tt;
                    if (tt.IsNewlineAfter) 
                        break;
                }
                return res;
            }
            if (!ignoreNewlines && isNewlineBefore && !tt.IsValue2("В", "КОНЦЕ")) 
            {
                if (tt.IsValue("СЛОВО", null)) 
                {
                }
                res = new DecreeChangeToken(tt, tt) { Typ = DecreeChangeTokenTyp.StartSingle };
                Pullenti.Ner.Token izlogit = null;
                bool specRegime = false;
                for (; tt != null; tt = tt.Next) 
                {
                    if (tt != res.BeginToken && tt.IsNewlineBefore) 
                        break;
                    if (tt.IsValue("В", null)) 
                    {
                        if (tt != res.BeginToken || !(tt is Pullenti.Ner.TextToken)) 
                            continue;
                        if (tt.Next != null && tt.Next.IsValue("ТЕКСТ", null)) 
                        {
                            res.EndToken = (tt = tt.Next);
                            res.HasText = true;
                        }
                        PartToken prr = PartToken.TryAttach(tt.Next, null, false, false);
                        if (prr != null) 
                            continue;
                        if (tt.Next != null && (tt.Next.GetReferent() is Pullenti.Ner.Decree.DecreePartReferent)) 
                        {
                            res.RealPart = tt.Next.GetReferent() as Pullenti.Ner.Decree.DecreePartReferent;
                            tt = tt.Next;
                            res.EndToken = tt;
                            continue;
                        }
                        bool hasKeyword = DecreeToken.IsKeyword(tt.Next, false) != null;
                        for (Pullenti.Ner.Token tt2 = tt.Next; tt2 != null; tt2 = tt2.Next) 
                        {
                            if (tt2.IsCharOf(":;") || tt2.IsNewlineBefore) 
                                break;
                            Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(tt2, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                            if (br != null) 
                            {
                                tt2 = br.EndToken;
                                continue;
                            }
                            PartToken pt = PartToken.TryAttach(tt2, null, false, false);
                            if (pt != null && pt.Typ == PartToken.ItemType.Appendix) 
                            {
                                if (res.Parts == null) 
                                {
                                    res.Parts = new List<PartToken>();
                                    res.Parts.Add(pt);
                                    tt2 = pt.EndToken;
                                    continue;
                                }
                            }
                            if (tt2.IsValue("УТВЕРЖДЕННЫЙ", null) && (tt2.Next is Pullenti.Ner.ReferentToken) && (tt2.Next.GetReferent() is Pullenti.Ner.Decree.DecreeReferent)) 
                            {
                                tt = tt2;
                                break;
                            }
                            if ((tt2.GetReferent() is Pullenti.Ner.Decree.DecreeReferent) && hasKeyword) 
                            {
                                tt = tt2.Previous;
                                break;
                            }
                        }
                        continue;
                    }
                    if (tt.IsValue("К", null) || tt.IsValue("ИЗ", null)) 
                        continue;
                    if (((tt.IsValue("ПЕРЕЧЕНЬ", "ПЕРЕЛІК") || tt.IsValue("СПИСОК", null))) && tt.Next != null && tt.Next.IsValue("ИЗМЕНЕНИЕ", "ЗМІНА")) 
                    {
                        if (tt == t) 
                            res.EndToken = tt.Next;
                        tt = tt.Next.Next;
                        res.Typ = DecreeChangeTokenTyp.StartMultu;
                        for (; tt != null && tt.Next != null; tt = tt.Next) 
                        {
                            if ((tt.Next.IsComma || tt.Next.IsValue("КОТОРЫЙ", null) || tt.Next.IsValue("ВНОСИТЬ", null)) || tt.Next.IsValue("ВНОСИМЫЙ", null)) 
                                res.EndToken = tt;
                            else 
                                break;
                        }
                        if (tt.IsNewlineBefore) 
                            return res;
                        continue;
                    }
                    if (tt.IsValue("ИЗМЕНЕНИЕ", null) && tt == t) 
                    {
                        res.Typ = DecreeChangeTokenTyp.StartMultu;
                        res.HasChangeKeyword = true;
                        for (; tt != null && tt.Next != null; tt = tt.Next) 
                        {
                            if ((tt.Next.IsComma || tt.Next.IsValue("КОТОРЫЙ", null) || tt.Next.IsValue("ВНОСИТЬ", null)) || tt.Next.IsValue("ВНОСИМЫЙ", null)) 
                                res.EndToken = tt;
                            else 
                                break;
                        }
                        continue;
                    }
                    if (tt.IsValue("ТЕКСТ", null)) 
                    {
                        PartToken pt = PartToken.TryAttach(tt.Next, null, false, true);
                        if (pt != null && pt.EndToken.Next != null && pt.EndToken.Next.IsValue("СЧИТАТЬ", "ВВАЖАТИ")) 
                        {
                            res.EndToken = pt.EndToken;
                            if (changeStack != null && changeStack.Count > 0) 
                                res.RealPart = changeStack[0];
                            res.ActKind = Pullenti.Ner.Decree.DecreeChangeKind.Consider;
                            res.PartTyp = pt.Typ;
                            res.HasText = true;
                            return res;
                        }
                    }
                    if (tt.IsValue2("И", "ТЕКСТ")) 
                    {
                        res.HasText = true;
                        res.EndToken = (tt = tt.Next);
                        continue;
                    }
                    if (tt.IsValue3("ПО", "ВСЕМУ", "ТЕКСТУ")) 
                    {
                        res.HasText = true;
                        res.EndToken = (tt = tt.Next.Next);
                        continue;
                    }
                    if (res.Parts == null && tt.IsValue("ДОПОЛНИТЬ", "ДОПОВНИТИ") && tt.Next != null) 
                    {
                        res.ActKind = Pullenti.Ner.Decree.DecreeChangeKind.Append;
                        if (tt.Next.GetReferent() is Pullenti.Ner.Decree.DecreeReferent) 
                            continue;
                        tt = tt.Next;
                        Pullenti.Ner.Token tt1 = DecreeToken.IsKeyword(tt, false);
                        if (tt1 == null || tt1.Morph.Case.IsInstrumental) 
                            tt1 = tt;
                        else 
                            tt1 = tt1.Next;
                        if (tt1 != null && tt1.IsValue("НОВЫЙ", "НОВИЙ")) 
                            tt1 = tt1.Next;
                        if (tt1 != null && tt1.Morph.Case.IsInstrumental) 
                        {
                            PartToken pt = PartToken.TryAttach(tt1, null, false, false);
                            if (pt == null) 
                                pt = PartToken.TryAttach(tt1, null, false, true);
                            if (pt != null && pt.Typ != PartToken.ItemType.Prefix) 
                            {
                                res.PartTyp = pt.Typ;
                                tt = (res.EndToken = pt.EndToken);
                                if (res.NewParts == null) 
                                    res.NewParts = new List<PartToken>();
                                res.NewParts.Add(pt);
                                if (tt.Next != null && tt.Next.IsAnd) 
                                {
                                    pt = PartToken.TryAttach(tt.Next.Next, null, false, false);
                                    if (pt == null) 
                                        pt = PartToken.TryAttach(tt.Next.Next, null, false, true);
                                    if (pt != null) 
                                    {
                                        res.NewParts.Add(pt);
                                        tt = (res.EndToken = pt.EndToken);
                                    }
                                }
                            }
                            continue;
                        }
                    }
                    if (tt.IsValue("ДОПОЛНИТЬ", "ДОПОВНИТИ") && tt.Next != null && tt.Next.IsChar(':')) 
                    {
                        res.ActKind2 = Pullenti.Ner.Decree.DecreeChangeKind.Append;
                        res.EndToken = tt.Next;
                        return res;
                    }
                    if (res.ActKind == Pullenti.Ner.Decree.DecreeChangeKind.Undefined && tt.IsValue("ИСКЛЮЧИТЬ", null) && !tt.Morph.ContainsAttr("п.вр.", null)) 
                    {
                        res.EndToken = tt;
                        res.ActKind = Pullenti.Ner.Decree.DecreeChangeKind.Remove;
                        continue;
                    }
                    if (tt.GetReferent() is Pullenti.Ner.Decree.DecreePartReferent) 
                    {
                        if (res.RealPart == null) 
                            res.RealPart = tt.GetReferent() as Pullenti.Ner.Decree.DecreePartReferent;
                        else 
                        {
                            if (res.AddRealParts == null) 
                                res.AddRealParts = new List<Pullenti.Ner.Decree.DecreePartReferent>();
                            res.AddRealParts.Add(tt.GetReferent() as Pullenti.Ner.Decree.DecreePartReferent);
                        }
                        res.EndToken = tt;
                        continue;
                    }
                    List<PartToken> li = PartToken.TryAttachList(tt, false, 40);
                    if (li == null && tt.IsValue("ПРИМЕЧАНИЕ", "ПРИМІТКА")) 
                    {
                        li = new List<PartToken>();
                        li.Add(new PartToken(tt, tt) { Typ = PartToken.ItemType.Notice });
                    }
                    if (li == null) 
                    {
                        if ((tt.IsValue("ЧАСТЬ", null) || tt.IsValue("ПУНКТ", null) || tt.IsValue("ПОДПУНКТ", null)) || tt.IsValue("АБЗАЦ", null)) 
                        {
                            li = PartToken.TryAttachList(tt.Next, false, 40);
                            if (li != null) 
                                res.InPart = tt;
                        }
                    }
                    if (li != null && li.Count > 0) 
                    {
                        if (li[0].Typ == PartToken.ItemType.Prefix) 
                            li = null;
                        else if (li[0].Typ == PartToken.ItemType.Table && li[0].Values.Count == 0 && main == null) 
                            li = null;
                        else if (res.ChangeVal != null) 
                            li = null;
                    }
                    if (li != null && li.Count > 0) 
                    {
                        if ((res.Parts != null && res.Parts.Count == 1 && res.Parts[0].Typ == PartToken.ItemType.Name) && res.HasText) 
                        {
                            res.Parts.AddRange(li);
                            res.EndToken = (tt = li[li.Count - 1].EndToken);
                            continue;
                        }
                        if ((li.Count == 1 && li[0].Morph.Case.IsInstrumental && !li[0].Morph.Case.IsAccusative) && res.NewParts == null) 
                        {
                            res.NewParts = new List<PartToken>();
                            res.NewParts.Add(li[0]);
                            res.EndToken = (tt = li[0].EndToken);
                            continue;
                        }
                        if (li.Count == 1 && PartToken._getRank(li[0].Typ) > 0 && tt == t) 
                        {
                            if (li[0].IsNewlineAfter) 
                                return null;
                            if (li[0].EndToken.Next != null && li[0].EndToken.Next.IsChar('.')) 
                                return null;
                        }
                        if (((li[li.Count - 1].Typ == PartToken.ItemType.Table || li[li.Count - 1].Typ == PartToken.ItemType.Form)) && li[li.Count - 1].Name == null) 
                        {
                            Pullenti.Ner.Token tt0 = li[li.Count - 1].EndToken.Next;
                            Pullenti.Ner.Token tt1 = null;
                            for (Pullenti.Ner.Token ttt2 = tt0; ttt2 != null; ttt2 = ttt2.Next) 
                            {
                                if (ttt2.IsComma) 
                                    continue;
                                if (ttt2 == tt0 && ttt2.IsChar('(')) 
                                    break;
                                if (ttt2.GetReferent() is Pullenti.Ner.Decree.DecreeReferent) 
                                    break;
                                if (m_Terms.TryParse(ttt2, Pullenti.Ner.Core.TerminParseAttr.No) != null) 
                                    break;
                                PartToken ptt = PartToken.TryAttach(ttt2, null, false, false);
                                if (ptt != null && ptt.Typ != PartToken.ItemType.Prefix) 
                                    break;
                                Pullenti.Morph.MorphClass mc = ttt2.GetMorphClassInDictionary();
                                if (!mc.IsPreposition) 
                                    tt1 = ttt2;
                            }
                            if (tt1 != null) 
                            {
                                li[li.Count - 1].EndToken = tt1;
                                li[li.Count - 1].Name = Pullenti.Ner.Core.MiscHelper.GetTextValue(tt0, tt1, Pullenti.Ner.Core.GetTextAttr.No);
                                tt1 = tt1.Next;
                                if (tt1 != null && tt1.IsComma) 
                                    tt1 = tt1.Next;
                                if (tt1 != null && tt1.IsValue("В", null)) 
                                {
                                    li[li.Count - 1].EndToken = tt1;
                                    tt1 = tt1.Next;
                                }
                                if (tt1 == null) 
                                    break;
                                List<PartToken> pp = PartToken.TryAttachList(tt1, false, 40);
                                if (pp != null && pp.Count > 0) 
                                    li.AddRange(pp);
                            }
                        }
                        if (res.ActKind != Pullenti.Ner.Decree.DecreeChangeKind.Append) 
                        {
                            if (res.Parts != null) 
                            {
                                if (res.Parts.Count == 1 && ((res.Parts[0].Typ == PartToken.ItemType.Appendix || res.Parts[0].Typ == PartToken.ItemType.DocPart))) 
                                {
                                    if (li.Count > 1 && PartToken._getRank(li[0].Typ) > PartToken._getRank(li[li.Count - 1].Typ)) 
                                        res.Parts.InsertRange(0, li);
                                    else 
                                        res.Parts.AddRange(li);
                                }
                                else if (tt.Previous == null) 
                                    break;
                                else if (tt.Previous.IsChar('(')) 
                                {
                                    res.Parts.AddRange(li);
                                    res.EndToken = (tt = li[li.Count - 1].EndToken);
                                    if (tt.Next != null && tt.Next.IsChar(')')) 
                                        res.EndToken = (tt = tt.Next);
                                    continue;
                                }
                                else 
                                {
                                    if (tt.Previous.IsCommaAnd) 
                                    {
                                    }
                                    else if (tt.Previous.IsValue("В", null) && tt.Previous.Previous != null && tt.Previous.Previous.IsCommaAnd) 
                                    {
                                    }
                                    else 
                                        break;
                                    if (res.AddParts == null) 
                                        res.AddParts = new List<List<PartToken>>();
                                    res.AddParts.Add(li);
                                }
                            }
                            else 
                            {
                                res.Parts = li;
                                Pullenti.Ner.Token ttt1 = li[li.Count - 1].EndToken;
                                if (ttt1.Next != null && ttt1.Next.IsValue("ПОСЛЕ", null)) 
                                {
                                    PartToken sss = PartToken.TryAttach(ttt1.Next.Next, li[li.Count - 1], false, false);
                                    if (sss != null) 
                                    {
                                        li.Add(sss);
                                        ttt1 = sss.EndToken;
                                    }
                                }
                                for (; ttt1 != null; ttt1 = ttt1.Next) 
                                {
                                    if (ttt1.IsNewlineBefore || ttt1.IsTableControlChar) 
                                        break;
                                    PartToken pp = PartToken.TryAttach(ttt1, null, false, false);
                                    if (pp != null && pp.Typ != PartToken.ItemType.Prefix) 
                                        break;
                                    if (ttt1.IsValue3("ВСТУПАТЬ", "В", "СИЛУ")) 
                                    {
                                        res.Ignorable = true;
                                        break;
                                    }
                                    if ((ttt1.IsValue3("ИЗЛОЖИТЬ", "В", "СЛЕДУЮЩЕЙ") || ttt1.IsValue3("ПРИНЯТЬ", "В", "СЛЕДУЮЩЕЙ") || ttt1.IsValue3("ИЗЛОЖИТЬ", "В", "НОВОЙ")) || ttt1.IsValue3("В", "СЛЕДУЮЩЕЙ", "РЕДАКЦИИ") || ttt1.IsValue3("В", "РЕДАКЦИИ", "ПРИЛОЖЕНИЯ")) 
                                    {
                                        izlogit = ttt1.Previous;
                                        break;
                                    }
                                    if (m_Terms.TryParse(ttt1, Pullenti.Ner.Core.TerminParseAttr.No) != null) 
                                        break;
                                }
                            }
                        }
                        else if (li.Count == 1 && li[0].Morph != null && li[0].Morph.Case.IsInstrumental) 
                        {
                            if (res.NewParts == null) 
                                res.NewParts = new List<PartToken>();
                            res.NewParts.Add(li[0]);
                        }
                        else if (res.Parts == null) 
                            res.Parts = li;
                        tt = (res.EndToken = li[li.Count - 1].EndToken);
                        continue;
                    }
                    if (tt.Morph.Class.IsNoun && changeStack != null && changeStack.Count > 0) 
                    {
                        PartToken pa = PartToken.TryAttach(tt, null, false, true);
                        if (pa != null) 
                        {
                            if (changeStack[0].GetStringValue(PartToken._getAttrNameByTyp(pa.Typ)) != null) 
                            {
                                res.RealPart = changeStack[0];
                                res.EndToken = tt;
                                continue;
                            }
                        }
                    }
                    if (res.ActKind == Pullenti.Ner.Decree.DecreeChangeKind.Append) 
                    {
                        PartToken pa = PartToken.TryAttach(tt, null, false, true);
                        if (pa != null && pa.Typ != PartToken.ItemType.Prefix) 
                        {
                            if (res.NewParts == null) 
                                res.NewParts = new List<PartToken>();
                            res.NewParts.Add(pa);
                            res.EndToken = pa.EndToken;
                            continue;
                        }
                    }
                    if (tt.GetReferent() is Pullenti.Ner.Decree.DecreeReferent) 
                    {
                        Pullenti.Ner.Decree.DecreeKind ki = (tt.GetReferent() as Pullenti.Ner.Decree.DecreeReferent).Kind;
                        if (ki == Pullenti.Ner.Decree.DecreeKind.Publisher) 
                            continue;
                        if (tt.Morph.Case.IsInstrumental) 
                        {
                            if (tt.IsNewlineBefore) 
                                return null;
                        }
                        if (res.Decree == null) 
                            res.Decree = tt.GetReferent() as Pullenti.Ner.Decree.DecreeReferent;
                        else if ((tt.GetReferent() as Pullenti.Ner.Decree.DecreeReferent) != res.Decree) 
                        {
                            if (res.AddDecrees == null) 
                                res.AddDecrees = new List<Pullenti.Ner.Decree.DecreeReferent>();
                            res.AddDecrees.Add(tt.GetReferent() as Pullenti.Ner.Decree.DecreeReferent);
                        }
                        res.EndToken = tt;
                        res._skipDecreeDummy();
                        tt = res.EndToken;
                        if (tt.WhitespacesAfterCount < 3) 
                        {
                            li = PartToken.TryAttachList(tt.Next, false, 40);
                            if (li != null && !li[0].Morph.Case.IsInstrumental) 
                            {
                                if (res.Parts == null) 
                                    res.Parts = li;
                                else 
                                    res.Parts.AddRange(li);
                                PartToken.Sort(res.Parts);
                                tt = (res.EndToken = li[li.Count - 1].EndToken);
                            }
                        }
                        continue;
                    }
                    PartToken pt0 = PartToken.TryAttach(tt, null, false, true);
                    if (pt0 != null && (pt0.Typ == PartToken.ItemType.Appendix) && pt0.Typ != PartToken.ItemType.Prefix) 
                    {
                        tt = (res.EndToken = pt0.EndToken);
                        res.PartTyp = pt0.Typ;
                        if (pt0.Typ == PartToken.ItemType.Appendix && res.Parts == null) 
                        {
                            res.Parts = new List<PartToken>();
                            res.Parts.Add(pt0);
                        }
                        continue;
                    }
                    if (res.ChangeVal == null && !isInEdition) 
                    {
                        DecreeChangeToken res1 = null;
                        if (tt.IsChar(':') && !Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(tt.Next, true, false)) 
                            res1 = null;
                        else 
                            res1 = TryAttach(tt, main, true, null, false, false, (res.NewParts != null && res.NewParts.Count == 1 ? res.NewParts[0] : null));
                        if (res1 != null && res1.Typ == DecreeChangeTokenTyp.Value && res1.ChangeVal != null) 
                        {
                            res.ChangeVal = res1.ChangeVal;
                            res.InTheEnd = res1.InTheEnd;
                            if (res.ActKind == Pullenti.Ner.Decree.DecreeChangeKind.Undefined) 
                                res.ActKind = res1.ActKind;
                            tt = (res.EndToken = res1.EndToken);
                            if (tt.Next != null && tt.Next.IsValue("К", null)) 
                                tt = tt.Next;
                            continue;
                        }
                        if (res1 != null && res1.ActKind == Pullenti.Ner.Decree.DecreeChangeKind.Consider) 
                            izlogit = null;
                        if (tt.IsValue("ПОСЛЕ", "ПІСЛЯ")) 
                        {
                            List<PartToken> pts0 = PartToken.TryAttachList(tt.Next, false, 40);
                            Pullenti.Ner.Token inPart = null;
                            if (pts0 == null && tt.Next != null) 
                            {
                                if ((tt.Next.IsValue("ЧАСТЬ", null) || tt.Next.IsValue("ПУНКТ", null) || tt.Next.IsValue("ПОДПУНКТ", null)) || tt.Next.IsValue("АБЗАЦ", null)) 
                                {
                                    pts0 = PartToken.TryAttachList(tt.Next.Next, false, 40);
                                    if (pts0 != null) 
                                        inPart = tt.Next;
                                }
                            }
                            if (pts0 != null && pts0.Count > 0 && pts0[0].Typ != PartToken.ItemType.Prefix) 
                            {
                                Pullenti.Ner.Token tt2 = pts0[pts0.Count - 1].EndToken.Next;
                                while (tt2 != null && ((tt2.IsValue("НОВЫЙ", null) || tt2.IsValue("ДОПОЛНИТЬ", null)))) 
                                {
                                    tt2 = tt2.Next;
                                }
                                PartToken pt2 = PartToken.TryAttach(tt2, null, true, false);
                                if (pt2 == null) 
                                    pt2 = PartToken.TryAttach(tt2, null, false, true);
                                if (pt2 != null && pt2.Typ == pts0[0].Typ) 
                                {
                                    if (pt2.Values.Count > 0) 
                                    {
                                        res.Parts = pts0;
                                        res.NewParts = new List<PartToken>();
                                        res.NewParts.Add(pt2);
                                        res.ActKind = Pullenti.Ner.Decree.DecreeChangeKind.Append;
                                        tt = (res.EndToken = pt2.EndToken);
                                        continue;
                                    }
                                    List<PartToken> pts1 = PartToken.TryAttachList(tt.Next, false, 40);
                                    if (pts1[0].AddValue(1)) 
                                    {
                                        res.Parts = pts0;
                                        res.NewParts = pts1;
                                        res.ActKind = Pullenti.Ner.Decree.DecreeChangeKind.Append;
                                        tt = (res.EndToken = pt2.EndToken);
                                        continue;
                                    }
                                }
                                if (res.Parts == null) 
                                {
                                    res.Parts = pts0;
                                    res.InPart = inPart;
                                }
                                tt = (res.EndToken = pts0[pts0.Count - 1].EndToken);
                                continue;
                            }
                        }
                            {
                                pt0 = PartToken.TryAttach(tt, null, false, true);
                                if ((pt0 != null && pt0.Typ != PartToken.ItemType.Prefix && res.NewParts == null) && pt0.Morph.Case.IsInstrumental) 
                                {
                                    res.NewParts = new List<PartToken>();
                                    res.NewParts.Add(pt0);
                                    tt = (res.EndToken = pt0.EndToken);
                                    continue;
                                }
                            }
                        if (tt.IsValue("ТЕКСТ", null) && tt.Previous != null && tt.Previous.IsValue("В", "У")) 
                            continue;
                        if (tt.IsValue2("ПО", "ТЕКСТУ")) 
                        {
                            tt = tt.Next;
                            continue;
                        }
                        if (tt.IsValue("ИЗМЕНЕНИЕ", "ЗМІНА")) 
                        {
                            res.EndToken = tt;
                            continue;
                        }
                        if (tt.IsValue("УКАЗАННЫЙ", null) || tt.IsValue("ВЫШЕУКАЗАННЫЙ", null)) 
                        {
                            Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                            if (npt != null) 
                            {
                                res.EndToken = (tt = npt.EndToken);
                                continue;
                            }
                        }
                        if (tt.IsChar(':') && tt.Next != null && tt.Next.Next != null) 
                        {
                            if ((tt.Next is Pullenti.Ner.NumberToken) || (tt.Next.LengthChar == 1)) 
                            {
                                if (tt.Next.Next.IsCharOf(".)")) 
                                    break;
                            }
                        }
                    }
                    if (tt != t && (res.Parts != null) && res.Decree == null) 
                    {
                        List<DecreeToken> dts = DecreeToken.TryAttachList(tt, null, 10, false);
                        if (dts != null && dts.Count > 0 && dts[0].Typ == DecreeToken.ItemType.Typ) 
                        {
                            tt = (res.EndToken = dts[dts.Count - 1].EndToken);
                            if (main != null && res.Decree == null && res.DecreeTok == null) 
                            {
                                Pullenti.Ner.Decree.DecreeReferent dec = null;
                                foreach (Pullenti.Ner.Referent v in main.Owners) 
                                {
                                    if (v is Pullenti.Ner.Decree.DecreeReferent) 
                                    {
                                        dec = v as Pullenti.Ner.Decree.DecreeReferent;
                                        break;
                                    }
                                    else if (v is Pullenti.Ner.Decree.DecreePartReferent) 
                                    {
                                        dec = (v as Pullenti.Ner.Decree.DecreePartReferent).Owner;
                                        if (dec != null) 
                                            break;
                                    }
                                }
                                if (dec != null && dec.Typ0 == dts[0].Value) 
                                {
                                    res.Decree = dec;
                                    res.DecreeTok = dts[0];
                                }
                            }
                            if (!tt.IsNewlineAfter) 
                            {
                                li = PartToken.TryAttachList(tt.Next, false, 40);
                                if (li != null && !li[0].Morph.Case.IsInstrumental) 
                                {
                                    if (res.Parts == null) 
                                        res.Parts = li;
                                    else 
                                        res.Parts.AddRange(li);
                                    PartToken.Sort(res.Parts);
                                    tt = (res.EndToken = li[li.Count - 1].EndToken);
                                }
                            }
                            continue;
                        }
                    }
                    if (tt == res.BeginToken && main != null) 
                    {
                        Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                        if (npt != null) 
                        {
                            Pullenti.Ner.Token tt1 = npt.EndToken.Next;
                            if ((tt1 != null && ((tt1.IsValue("ИЗЛОЖИТЬ", "ВИКЛАСТИ") || tt1.IsValue("ПРИНЯТЬ", null))) && tt1.Next != null) && tt1.Next.IsValue("В", null)) 
                            {
                                PartToken pt = new PartToken(tt, npt.EndToken) { Typ = PartToken.ItemType.Appendix };
                                pt.Name = npt.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false);
                                res.Parts = new List<PartToken>();
                                res.Parts.Add(pt);
                                res.EndToken = pt.EndToken;
                                break;
                            }
                        }
                    }
                    Pullenti.Ner.Token ttt = DecreeToken.IsKeyword(tt, false);
                    if (ttt != null && res.Parts == null) 
                    {
                        Pullenti.Ner.Token ttt0 = ttt;
                        for (; ttt != null; ttt = ttt.Next) 
                        {
                            if (Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(ttt)) 
                                break;
                            if (ttt.IsChar('(') && ttt.Next != null && ttt.Next.IsValue("ПРИЛОЖЕНИЕ", "ДОДАТОК")) 
                            {
                                if (ttt.IsNewlineBefore) 
                                    break;
                                Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(ttt, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                                if (br == null) 
                                    break;
                                PartToken pt = PartToken.TryAttach(ttt.Next, null, false, false);
                                if (pt == null) 
                                    PartToken.TryAttach(ttt.Next, null, false, true);
                                if (pt != null) 
                                {
                                    res.Parts = new List<PartToken>();
                                    res.Parts.Add(pt);
                                    tt = (res.EndToken = br.EndToken);
                                    break;
                                }
                            }
                            if (ttt.IsValue3("В", "СЛЕДУЮЩЕЙ", "РЕДАКЦИИ") || ttt.IsValue3("В", "РЕДАКЦИИ", "ПРИЛОЖЕНИЕ")) 
                            {
                                PartToken pt = new PartToken(tt, ttt.Previous);
                                pt.Typ = PartToken.ItemType.Chapter;
                                pt.Name = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(pt, Pullenti.Ner.Core.GetTextAttr.No);
                                res.Parts = new List<PartToken>();
                                res.Parts.Add(pt);
                                tt = (res.EndToken = ttt.Previous);
                                break;
                            }
                        }
                        if (res.Parts != null) 
                            continue;
                        if (res.ActKind == Pullenti.Ner.Decree.DecreeChangeKind.Append) 
                        {
                            tt = (res.EndToken = ttt0);
                            continue;
                        }
                        tt = ttt0;
                        if (tt.IsValue("НОРМА", null)) 
                        {
                            Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                            if (npt != null && npt.Morph.Case.IsGenitive) 
                            {
                                res.EndToken = (tt = npt.EndToken);
                                continue;
                            }
                        }
                        continue;
                    }
                    if (specRegime) 
                    {
                        res.EndToken = tt;
                        continue;
                    }
                    if (tt.IsCharOf(".-") && !tt.IsNewlineAfter) 
                    {
                        res.EndToken = tt;
                        continue;
                    }
                    if (tt.IsComma && res.Decree != null) 
                        continue;
                    if (tt.IsValue("ИЗЛОЖИТЬ", null) || tt.IsValue("ПРИНЯТЬ", null)) 
                    {
                        if (res.Parts == null) 
                        {
                            res.ActKind = Pullenti.Ner.Decree.DecreeChangeKind.New;
                            if (tt.Next != null && tt.Next.GetMorphClassInDictionary().IsPronoun) 
                            {
                                tt = tt.Next;
                                res.HasAnafor = true;
                            }
                            res.EndToken = tt;
                            continue;
                        }
                    }
                    if (tt.Next != null) 
                    {
                        Pullenti.Ner.Token tt2 = tt.Next;
                        if (tt.IsValue("СОГЛАСНО", null)) 
                            tt2 = tt;
                        if (tt2 != null && tt2.IsValue2("В", "РЕДАКЦИИ")) 
                            tt2 = tt2.Next.Next;
                        if (tt2 != null && ((tt2.IsValue2("СОГЛАСНО", "ПРИЛОЖЕНИЮ") || ((tt2.IsValue("ПРИЛОЖЕНИЯ", null) && res.ActKind == Pullenti.Ner.Decree.DecreeChangeKind.New))))) 
                        {
                            PartToken ppp = PartToken.TryAttach(tt2.Next, null, false, false);
                            if (ppp == null) 
                                ppp = PartToken.TryAttach(tt2.Next, null, true, false);
                            if (ppp == null) 
                                ppp = PartToken.TryAttach(tt2, null, true, false);
                            if (ppp != null && ppp.Typ == PartToken.ItemType.Appendix) 
                            {
                                res.AppExtChanges = ppp;
                                if (res.ActKind == Pullenti.Ner.Decree.DecreeChangeKind.Undefined) 
                                    res.ActKind = Pullenti.Ner.Decree.DecreeChangeKind.New;
                                res.EndToken = (tt = ppp.EndToken);
                                if (tt.Next != null && tt.Next.IsValue("К", null)) 
                                {
                                    Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt.Next.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                                    if (npt != null) 
                                        res.EndToken = (tt = npt.EndToken);
                                }
                                else if (tt.IsValue("К", null)) 
                                {
                                    Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt.Next, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                                    if (npt != null) 
                                        res.EndToken = (tt = npt.EndToken);
                                }
                                continue;
                            }
                        }
                    }
                    if (izlogit != null && izlogit.EndChar > tt.EndChar) 
                    {
                        res.EndToken = (tt = izlogit);
                        izlogit = null;
                        continue;
                    }
                    if (tt.IsCommaAnd && res.Parts != null) 
                    {
                        li = PartToken.TryAttachList(tt.Next, false, 40);
                        if (li != null && li.Count > 0 && li[0].Typ != PartToken.ItemType.Prefix) 
                        {
                            Pullenti.Ner.Token tt2 = li[li.Count - 1].EndToken.Next;
                            if (tt2 != null && (tt2.GetReferent() is Pullenti.Ner.Decree.DecreeReferent) && res.Decree != null) 
                            {
                            }
                            else 
                            {
                                res.Parts[res.Parts.Count - 1].DelimAfter = true;
                                res.Parts.AddRange(li);
                                res.EndToken = (tt = li[li.Count - 1].EndToken);
                                continue;
                            }
                        }
                    }
                    if (tt.IsCommaAnd) 
                        continue;
                    if (tt.Previous != null && tt.Previous.IsValue("К", null) && res.Parts != null) 
                    {
                        Pullenti.Ner.Token tt2 = DecreeToken.IsKeyword(tt, false);
                        if (tt2 != null && tt2.Next != null) 
                        {
                            li = PartToken.TryAttachList(tt2.Next, false, 40);
                            if (li != null) 
                            {
                                res.Parts.AddRange(li);
                                tt = (res.EndToken = li[li.Count - 1].EndToken);
                                continue;
                            }
                        }
                    }
                    if (tt.IsChar('<')) 
                    {
                    }
                    Pullenti.Ner.Core.NounPhraseToken npt1 = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt, Pullenti.Ner.Core.NounPhraseParseAttr.ParsePreposition, 0, null);
                    if (npt1 != null) 
                    {
                        if ((npt1.EndToken.IsValue("ПРИОРИТЕТ", null) || npt1.EndToken.IsValue("СФЕРА", null) || npt1.EndToken.IsValue("РЕАЛИЗАЦИЯ", null)) || npt1.EndToken.IsValue("СПИСОК", null) || npt1.EndToken.IsValue("ВОПРОС", null)) 
                        {
                            res.EndToken = (tt = npt1.EndToken);
                            if (tt.Next != null && (tt.Next.GetReferent() is Pullenti.Ner.Decree.DecreeReferent)) 
                                res.EndToken = (tt = tt.Next);
                            if (Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(tt.Next, false, null, false)) 
                                res.EndToken = (tt = tt.Next);
                            continue;
                        }
                    }
                    break;
                }
                if (((res.Ignorable || res.Parts != null || res.Decree != null) || res.RealPart != null || res.ActKind != Pullenti.Ner.Decree.DecreeChangeKind.Undefined) || res.ChangeVal != null) 
                {
                    if (res.EndToken.Next != null && res.EndToken.Next.IsChar(':')) 
                    {
                        bool nl = res.EndToken.Next.IsNewlineAfter;
                        if (!nl) 
                        {
                            PartToken pl = PartToken.TryAttach(res.EndToken.Next.Next, null, false, false);
                            if (pl != null) 
                                nl = true;
                        }
                        if (nl) 
                        {
                            res.Typ = DecreeChangeTokenTyp.Single;
                            res.EndToken = res.EndToken.Next;
                        }
                    }
                    return res;
                }
                if (res.BeginToken == tt) 
                {
                    Pullenti.Ner.Core.TerminToken tok1 = m_Terms.TryParse(tt, Pullenti.Ner.Core.TerminParseAttr.No);
                    if (tok1 != null) 
                    {
                    }
                    else if (tt.IsValue("ПОСЛЕ", null)) 
                    {
                    }
                    else 
                        return null;
                }
                else 
                    return null;
            }
            for (; tt != null; tt = tt.Next) 
            {
                if (tt.IsValue("В", null) && tt.Next != null && tt.Next.GetMorphClassInDictionary().IsPronoun) 
                    tt = tt.Next;
                else 
                    break;
            }
            if (tt == null) 
                return null;
            Pullenti.Ner.Core.TerminToken tok = m_Terms.TryParse(tt, Pullenti.Ner.Core.TerminParseAttr.No);
            if (tt.Morph.Class.IsAdjective && (((tt is Pullenti.Ner.NumberToken) || tt.IsValue("ПОСЛЕДНИЙ", "ОСТАННІЙ") || tt.IsValue("ПРЕДПОСЛЕДНИЙ", "ПЕРЕДОСТАННІЙ")))) 
            {
                Pullenti.Ner.Token tt2 = tt.Next;
                for (; tt2 != null; tt2 = tt2.Next) 
                {
                    if ((tt2 is Pullenti.Ner.NumberToken) || tt2.IsCommaAnd) 
                    {
                    }
                    else 
                        break;
                }
                tok = m_Terms.TryParse(tt2, Pullenti.Ner.Core.TerminParseAttr.No);
                if (tok != null && (tok.Termin.Tag is Pullenti.Ner.Decree.DecreeChangeValueKind)) 
                {
                }
                else 
                    tok = null;
            }
            bool afterValue = false;
            if (tok == null && tt.IsValue("ПОСЛЕ", null) && Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(tt.Next, true, false)) 
            {
                tok = new Pullenti.Ner.Core.TerminToken(tt, tt);
                tok.Termin = new Pullenti.Ner.Core.Termin("СЛОВО") { Tag = Pullenti.Ner.Decree.DecreeChangeValueKind.Words };
                afterValue = true;
            }
            if (tok == null && tt.IsValue("ПРЕДЛОЖЕНИЕ", null) && Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(tt.Next, true, false)) 
                tok = new Pullenti.Ner.Core.TerminToken(tt, tt) { Termin = m_Words };
            if (tok == null && Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(tt, true, false) && tt.Previous != null) 
            {
                if ((tt.Previous.IsValue("ПРЕДЛОЖЕНИЕ", null) || (tt.Previous.GetReferent() is Pullenti.Ner.Decree.DecreePartReferent) || tt.IsNewlineBefore) || tt.Previous.IsCharOf(".)")) 
                    tok = new Pullenti.Ner.Core.TerminToken(tt.Previous, tt.Previous) { Termin = m_Words };
            }
            if ((tok == null && tt.IsValue("ПРЕДЛОЖЕНИЕ", null) && tt.Next != null) && tt.Next.IsChar(':') && Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(tt.Next.Next, true, false)) 
                tok = new Pullenti.Ner.Core.TerminToken(tt, tt.Next) { Termin = m_Words };
            if (tok == null && tt.IsValue3("РАЗДЕЛ", "СЛЕДУЮЩЕГО", "СОДЕРЖАНИЯ")) 
                tok = new Pullenti.Ner.Core.TerminToken(tt, tt.Next.Next) { Termin = m_Words };
            if (tok != null) 
            {
                if (tok.Termin.Tag is Pullenti.Ner.Decree.DecreeChangeKind) 
                {
                    res = new DecreeChangeToken(tt, tok.EndToken) { Typ = DecreeChangeTokenTyp.Action, ActKind = (Pullenti.Ner.Decree.DecreeChangeKind)tok.Termin.Tag };
                    if (((res.ActKind == Pullenti.Ner.Decree.DecreeChangeKind.Append || res.ActKind == Pullenti.Ner.Decree.DecreeChangeKind.Consider)) && tok.EndToken.Next != null && tok.EndToken.Next.Morph.Case.IsInstrumental) 
                    {
                        Pullenti.Ner.Token tt4 = tok.EndToken.Next;
                        while (tt4 != null && ((tt4.IsValue("СЛЕДУЮЩИЙ", null) || tt4.IsValue("НОВЫЙ", null)))) 
                        {
                            tt4 = tt4.Next;
                        }
                        PartToken pt = PartToken.TryAttach(tt4, null, false, false);
                        if (pt == null) 
                            pt = PartToken.TryAttach(tt4, null, false, true);
                        if (pt != null && pt.Typ != PartToken.ItemType.Prefix) 
                        {
                            if (res.ActKind == Pullenti.Ner.Decree.DecreeChangeKind.Append) 
                            {
                                res.PartTyp = pt.Typ;
                                if (res.NewParts == null) 
                                    res.NewParts = new List<PartToken>();
                                res.NewParts.Add(pt);
                            }
                            else if (res.ActKind == Pullenti.Ner.Decree.DecreeChangeKind.Consider) 
                            {
                                res.ChangeVal = new Pullenti.Ner.Decree.DecreeChangeValueReferent();
                                res.ChangeVal.Value = pt.GetSourceText();
                            }
                            tt = (res.EndToken = pt.EndToken);
                            if (tt.Next != null && tt.Next.IsAnd && res.ActKind == Pullenti.Ner.Decree.DecreeChangeKind.Append) 
                            {
                                pt = PartToken.TryAttach(tt.Next.Next, null, false, false);
                                if (pt == null) 
                                    pt = PartToken.TryAttach(tt.Next.Next, null, false, true);
                                if (pt != null) 
                                {
                                    res.NewParts.Add(pt);
                                    tt = (res.EndToken = pt.EndToken);
                                }
                            }
                        }
                    }
                    if (res.ActKind == Pullenti.Ner.Decree.DecreeChangeKind.Consider && (tok.EndToken.Next is Pullenti.Ner.NumberToken) && (tok.WhitespacesAfterCount < 3)) 
                    {
                        for (Pullenti.Ner.Token tt3 = res.EndToken.Next; tt3 != null; tt3 = tt3.Next) 
                        {
                            if (tt3 is Pullenti.Ner.NumberToken) 
                            {
                                res.EndToken = tt3;
                                continue;
                            }
                            if (tt3.IsCommaAnd) 
                                continue;
                            PartToken pp = PartToken.TryAttach(tt3, null, false, true);
                            if (pp != null && pp.EndToken == tt3) 
                            {
                                res.EndToken = tt3;
                                continue;
                            }
                            break;
                        }
                        res.ChangeVal = new Pullenti.Ner.Decree.DecreeChangeValueReferent();
                        res.ChangeVal.Value = Pullenti.Ner.Core.MiscHelper.GetTextValue(tok.EndToken.Next, res.EndToken, Pullenti.Ner.Core.GetTextAttr.KeepRegister);
                    }
                    if (res.EndToken.Next != null && res.EndToken.Next.IsValue("СООТВЕТСТВЕННО", null)) 
                        res.EndToken = res.EndToken.Next;
                    return res;
                }
                if (tok.Termin.Tag is Pullenti.Ner.Decree.DecreeChangeValueKind) 
                {
                    res = new DecreeChangeToken(tt, (tok.EndChar > tt.BeginChar ? tok.EndToken : tt)) { Typ = DecreeChangeTokenTyp.Value };
                    res.ChangeVal = new Pullenti.Ner.Decree.DecreeChangeValueReferent();
                    res.ChangeVal.Kind = (Pullenti.Ner.Decree.DecreeChangeValueKind)tok.Termin.Tag;
                    if (afterValue) 
                        res.Typ = DecreeChangeTokenTyp.AfterValue;
                    if (res.ChangeVal.Kind == Pullenti.Ner.Decree.DecreeChangeValueKind.Words && (tok.Termin.Tag2 is string)) 
                    {
                        res.ChangeVal.Value = tok.Termin.Tag2 as string;
                        res.ChangeVal.BeginChar = tok.BeginChar;
                        res.ChangeVal.EndChar = tok.EndChar;
                        return res;
                    }
                    tt = tok.EndToken.Next;
                    if (tt == null) 
                        return null;
                    if (tt != null && ((tt.IsValue("ИЗЛОЖИТЬ", "ВИКЛАСТИ") || tt.IsValue("ПРИНЯТЬ", null))) && res.ActKind == Pullenti.Ner.Decree.DecreeChangeKind.Undefined) 
                    {
                        res.ActKind = Pullenti.Ner.Decree.DecreeChangeKind.New;
                        tt = tt.Next;
                        if (tt != null && tt.IsValue("В", null)) 
                            tt = tt.Next;
                    }
                    if ((tt != null && ((tt.IsValue("СЛЕДУЮЩИЙ", "НАСТУПНИЙ") || tt.IsValue("ТАКОЙ", "ТАКИЙ"))) && tt.Next != null) && ((tt.Next.IsValue("СОДЕРЖАНИЕ", "ЗМІСТ") || tt.Next.IsValue("СОДЕРЖИМОЕ", "ВМІСТ") || tt.Next.IsValue("РЕДАКЦИЯ", "РЕДАКЦІЯ")))) 
                        tt = tt.Next.Next;
                    else if (tt != null && tt.IsValue("РЕДАКЦИЯ", "РЕДАКЦІЯ")) 
                        tt = tt.Next;
                    if (tt != null && tt.IsChar(':')) 
                        tt = tt.Next;
                    if (tt != null) 
                        res.ChangeVal.BeginChar = tt.BeginChar;
                    if (res.BeginToken.IsValue("СЛОВО", null) && (tt is Pullenti.Ner.TextToken) && tt.LengthChar > 1) 
                    {
                        DecreeChangeToken next = TryAttach(tt.Next, null, true, null, false, false, null);
                        if (next != null) 
                        {
                            if (next.ActKind == Pullenti.Ner.Decree.DecreeChangeKind.Remove || next.ActKind == Pullenti.Ner.Decree.DecreeChangeKind.Exchange || next.ActKind == Pullenti.Ner.Decree.DecreeChangeKind.Append) 
                            {
                                res.ChangeVal.Value = tt.GetSourceText();
                                res.EndToken = tt;
                                res.ChangeVal.BeginChar = tt.BeginChar;
                                res.ChangeVal.EndChar = tt.EndChar;
                                return res;
                            }
                        }
                    }
                    if (tt.IsChar('<') && (tt.Next is Pullenti.Ner.TextToken)) 
                    {
                        if (tt.Next.Chars.IsLatinLetter || tt.Next.IsChar('/')) 
                            return null;
                    }
                    bool canBeStart = false;
                    if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(tt, true, false)) 
                        canBeStart = true;
                    else if (Pullenti.Ner.Core.BracketHelper.IsBracket(tt, true) && !tt.IsWhitespaceAfter) 
                        canBeStart = true;
                    else if ((tt is Pullenti.Ner.MetaToken) && Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence((tt as Pullenti.Ner.MetaToken).BeginToken, true, false)) 
                        canBeStart = true;
                    else if (tt != null && tt.IsNewlineBefore && tt.IsValue("ПРИЛОЖЕНИЕ", "ДОДАТОК")) 
                    {
                        if ((tt.Previous != null && tt.Previous.IsChar(':') && tt.Previous.Previous != null) && tt.Previous.Previous.IsValue("РЕДАКЦИЯ", "РЕДАКЦІЯ")) 
                            canBeStart = true;
                    }
                    else if (t.Previous != null && ((t.Previous.IsValue("ДОПОЛНИТЬ", null) || t.Previous.IsValue("ЗАМЕНИТЬ", null) || t.Previous.IsValue("ПОСЛЕ", null)))) 
                        canBeStart = true;
                    else if (tt.IsChar('«') && tt.Next != null && tt.Next.IsChar('»')) 
                    {
                        res.ChangeVal.Value = "";
                        res.EndToken = tt.Next;
                        res.ChangeVal.BeginChar = tt.EndChar + 1;
                        res.ChangeVal.EndChar = tt.Next.BeginChar - 1;
                        return res;
                    }
                    else if (tt.IsChar((char)0x1E)) 
                        canBeStart = true;
                    else if (tt.Next != null) 
                    {
                        int cou = 0;
                        for (Pullenti.Ner.Token tt2 = tt.Next; tt2 != null && (cou < 10); tt2 = tt2.Next,cou++) 
                        {
                            if (tt2.IsChar('»')) 
                            {
                                res.ChangeVal.Value = (new Pullenti.Ner.MetaToken(tt, tt2.Previous)).GetSourceText();
                                res.EndToken = tt2;
                                res.ChangeVal.EndChar = tt2.BeginChar - 1;
                                return res;
                            }
                            else if ((tt2.IsValue("ИСКЛЮЧИТЬ", null) || tt2.IsValue("ДОПОЛНИТЬ", null) || tt2.IsValue("ЗАМЕНИТЬ", null)) || tt2.IsValue("УДАЛИТЬ", null)) 
                                canBeStart = true;
                        }
                    }
                    if (canBeStart) 
                    {
                        Pullenti.Ner.Token ttt = tt;
                        if (Pullenti.Ner.Core.BracketHelper.IsBracket(tt, true)) 
                        {
                            ttt = tt.Next;
                            res.ChangeVal.BeginChar = ttt.BeginChar;
                            if (tt.IsChar('<') && ttt.IsChar('<') && ttt.Next != null) 
                            {
                                ttt = ttt.Next;
                                res.ChangeVal.BeginChar = ttt.EndChar + 1;
                            }
                        }
                        for (; ttt != null; ttt = ttt.Next) 
                        {
                            bool stop = false;
                            if (ttt.IsCharOf(".;")) 
                            {
                                stop = ttt.IsNewlineAfter;
                                if ((!stop && ttt.Next != null && ttt.Next.IsTableControlChar) && ttt.Next.IsNewlineAfter) 
                                    stop = true;
                                if (!stop && Pullenti.Ner.Core.BracketHelper.IsBracket(ttt.Previous, false)) 
                                {
                                    Pullenti.Ner.Core.ComplexNumToken num = Pullenti.Ner.Core.ComplexNumToken.TryParse(ttt.Next, null, false, false);
                                    if (num != null) 
                                        stop = true;
                                }
                                if (stop) 
                                {
                                    if (tt.Next.BeginChar > ttt.Previous.BeginChar) 
                                        break;
                                    res.ChangeVal.Value = (new Pullenti.Ner.MetaToken(tt.Next, ttt.Previous)).GetSourceText();
                                    res.EndToken = ttt.Previous;
                                    res.ChangeVal.EndChar = res.EndToken.EndChar;
                                    break;
                                }
                            }
                            if (ttt.IsChar((char)0x1E)) 
                            {
                                List<Pullenti.Ner.Core.TableRowToken> rows = Pullenti.Ner.Core.TableHelper.TryParseRows(ttt, 0, true, false);
                                if (rows != null) 
                                {
                                    ttt = rows[rows.Count - 1].EndToken;
                                    if (ttt.IsNewlineAfter) 
                                        stop = true;
                                    else 
                                        continue;
                                }
                            }
                            if (ttt.Next != null) 
                            {
                                if (ttt.Next.IsValue2("ДОПОЛНИТЬ", "СЛОВАМИ")) 
                                    stop = true;
                                else if (ttt.Next.IsValue("УДАЛИТЬ", null) || ttt.Next.IsValue("ИСКЛЮЧИТЬ", null)) 
                                {
                                    if (ttt.Next.Next == null || tt.Next.IsNewlineAfter) 
                                        stop = true;
                                    else if (ttt.Next.Next.IsCharOf(";")) 
                                        stop = true;
                                    else if (ttt.Next.Next.IsCharOf(".,") && tt.Next.Next.IsNewlineAfter) 
                                        stop = true;
                                }
                            }
                            if (ttt.IsNewlineAfter) 
                            {
                                if (_canBeChangeToken(ttt.Next)) 
                                    stop = true;
                            }
                            if (_checkEndBracket(ttt) || ttt.IsChar(']')) 
                            {
                                if (_canBeChangeToken(ttt.Next)) 
                                    stop = true;
                            }
                            else if (!stop) 
                                continue;
                            if (ttt.Next == null || stop) 
                            {
                            }
                            else if (ttt.IsNewlineAfter) 
                            {
                                if (ttt.Next.IsCommaAnd && ttt.Next.Next != null && (ttt.Next.Next.GetReferent() is Pullenti.Ner.Decree.DecreeReferent)) 
                                    continue;
                            }
                            else if (ttt.Next.IsValue("СООТВЕТСТВЕННО", null) || ttt.Next.IsValue2("В", "СООТВЕТСТВУЮЩИЙ")) 
                            {
                            }
                            else if (ttt.Next.IsCharOf(".;") && (((ttt.Next.IsNewlineAfter || TryAttach(ttt.Next.Next, main, false, changeStack, true, false, null) != null || PartToken.TryAttach(ttt.Next.Next, null, false, false) != null) || ttt.Next.Next.IsValue("В", null)))) 
                            {
                            }
                            else if (ttt.Next.IsCommaAnd && TryAttach(ttt.Next.Next, main, false, changeStack, true, false, null) != null) 
                            {
                            }
                            else if (ttt.Next.IsComma && ttt.Next.Next != null && ttt.Next.Next.IsValue("А", null)) 
                            {
                                Pullenti.Ner.Core.ConjunctionToken conj = Pullenti.Ner.Core.ConjunctionHelper.TryParse(ttt.Next.Next);
                                if (conj != null && TryAttach(conj.EndToken.Next, main, false, changeStack, true, false, null) != null) 
                                {
                                }
                                else 
                                    continue;
                            }
                            else if (TryAttach(ttt.Next, main, false, changeStack, true, false, null) != null || m_Terms.TryParse(ttt.Next, Pullenti.Ner.Core.TerminParseAttr.No) != null) 
                            {
                            }
                            else if ((ttt.Next.IsValue("ДОБАВИТЬ", null) || ttt.Next.IsValue("ДОПОЛНИТЬ", null) || ttt.Next.IsValue("ЗАМЕНИТЬ", null)) || ttt.Next.IsValue("УДАЛИТЬ", null)) 
                            {
                            }
                            else 
                                continue;
                            Pullenti.Ner.Token ttt1 = (Pullenti.Ner.Core.BracketHelper.IsBracket(ttt, true) && !ttt.IsCharOf("]>") ? ttt.Previous : ttt);
                            Pullenti.Ner.Token ttt0 = tt;
                            if (Pullenti.Ner.Core.BracketHelper.IsBracket(tt, true)) 
                            {
                                ttt0 = tt.Next;
                                if (ttt0 != null && tt.IsChar('<')) 
                                {
                                    if (ttt0.IsChar('<')) 
                                        ttt0 = ttt0.Next;
                                    else if (!tt.IsWhitespaceAfter && (ttt0 is Pullenti.Ner.TextToken) && ttt0.Chars.IsLatinLetter) 
                                        continue;
                                }
                            }
                            if (ttt1.IsChar('>') && ttt1.Previous != null && ttt1.Previous.IsChar('>')) 
                                ttt1 = ttt1.Previous.Previous;
                            string val = (new Pullenti.Ner.MetaToken(ttt0, ttt1)).GetSourceText();
                            res.EndToken = ttt;
                            res.ChangeVal.EndChar = ttt1.EndChar;
                            if ((ttt == tt && (ttt is Pullenti.Ner.MetaToken) && Pullenti.Ner.Core.BracketHelper.IsBracket((ttt as Pullenti.Ner.MetaToken).BeginToken, true)) && Pullenti.Ner.Core.BracketHelper.IsBracket((ttt as Pullenti.Ner.MetaToken).EndToken, true)) 
                            {
                                val = val.Substring(1, val.Length - 2);
                                res.ChangeVal.EndChar -= 1;
                            }
                            else if ((tt is Pullenti.Ner.MetaToken) && Pullenti.Ner.Core.BracketHelper.IsBracket((tt as Pullenti.Ner.MetaToken).BeginToken, true)) 
                                val = val.Substring(1, val.Length - 1);
                            res.ChangeVal.Value = val;
                            if (res.EndToken.Next != null) 
                            {
                                if (res.EndToken.Next.IsValue("СООТВЕТСТВЕННО", null)) 
                                    res.EndToken = res.EndToken.Next;
                            }
                            if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(tt, true, false) && Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(ttt, true, null, false)) 
                            {
                                List<string> vals = null;
                                SortedDictionary<int, int> bes = null;
                                Pullenti.Ner.Token pp;
                                Pullenti.Ner.Token pp0 = tt.Next;
                                for (pp = tt.Next; pp != null && pp.EndChar <= ttt.EndChar; pp = pp.Next) 
                                {
                                    if ((Pullenti.Ner.Core.BracketHelper.CanBeEndOfSequence(pp, false, null, false) && pp.Next != null && pp.Next.IsCommaAnd) && Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(pp.Next.Next, true, false) && (pp.Next.Next.EndChar < ttt.BeginChar)) 
                                    {
                                        if (vals == null) 
                                            vals = new List<string>();
                                        if (bes == null) 
                                            bes = new SortedDictionary<int, int>();
                                        vals.Add((new Pullenti.Ner.MetaToken(pp0, pp.Previous)).GetSourceText());
                                        bes.Add(pp0.BeginChar, pp.Previous.EndChar);
                                        pp = pp.Next.Next;
                                        pp0 = pp.Next;
                                    }
                                }
                                if (vals != null) 
                                {
                                    if (pp0 != null && (pp0.EndChar < ttt.BeginChar)) 
                                    {
                                        vals.Add((new Pullenti.Ner.MetaToken(pp0, ttt.Previous)).GetSourceText());
                                        bes.Add(pp0.BeginChar, ttt.Previous.EndChar);
                                    }
                                    Pullenti.Ner.Decree.DecreeChangeValueKind ki = res.ChangeVal.Kind;
                                    res.ChangeVal = new Pullenti.Ner.Decree.DecreeChangeValueReferent();
                                    res.ChangeVal.Kind = ki;
                                    int i = 0;
                                    foreach (KeyValuePair<int, int> kp in bes) 
                                    {
                                        res.ChangeVal.AddSlot(Pullenti.Ner.Decree.DecreeChangeReferent.ATTR_VALUE, vals[i], false, 0);
                                        i++;
                                        res.ChangeVal.AddSlot("BEGIN", kp.Key.ToString(), false, 0);
                                        res.ChangeVal.AddSlot("END", kp.Value.ToString(), false, 0);
                                    }
                                }
                            }
                            break;
                        }
                        if (canBeStart) 
                        {
                            if (res.ChangeVal.Value == null) 
                            {
                                if (tt.Kit.Level == 0) 
                                {
                                    tt.Kit.Level++;
                                    DecreeChangeToken txt = _tryParseText(tt, abzacRegime, false, null);
                                    tt.Kit.Level--;
                                    if (txt != null) 
                                    {
                                        txt.BeginToken = res.BeginToken;
                                        res = txt;
                                    }
                                }
                            }
                            else if (res.IsNewlineAfter || ((res.EndToken.Next != null && res.EndToken.Next.IsCharOf(";,.") && res.EndToken.Next.IsNewlineAfter))) 
                            {
                                int crlf = 0;
                                for (Pullenti.Ner.Token t3 = tt; t3 != null && (t3.EndChar < res.EndChar); t3 = t3.Next) 
                                {
                                    if (t3.IsNewlineAfter) 
                                        crlf++;
                                }
                                if (res.LengthChar > 200 || crlf > 1) 
                                {
                                    if (tt.Kit.Level == 0) 
                                    {
                                        tt.Kit.Level++;
                                        DecreeChangeToken txt = _tryParseText(tt, abzacRegime, false, null);
                                        tt.Kit.Level--;
                                        if (txt != null && ((res.ChangeVal.EndChar + 3) < txt.EndChar)) 
                                        {
                                            txt.BeginToken = res.BeginToken;
                                            res = txt;
                                        }
                                    }
                                }
                            }
                        }
                        if (res.ChangeVal.Value == null) 
                            return null;
                        if (res.ChangeVal.Kind == Pullenti.Ner.Decree.DecreeChangeValueKind.Words) 
                        {
                            tok = m_Terms.TryParse(res.EndToken.Next, Pullenti.Ner.Core.TerminParseAttr.No);
                            if (tok != null && (tok.Termin.Tag is Pullenti.Ner.Decree.DecreeChangeValueKind) && ((Pullenti.Ner.Decree.DecreeChangeValueKind)tok.Termin.Tag) == Pullenti.Ner.Decree.DecreeChangeValueKind.RobustWords) 
                            {
                                res.ChangeVal.Kind = Pullenti.Ner.Decree.DecreeChangeValueKind.RobustWords;
                                res.EndToken = tok.EndToken;
                                if (res.EndToken.Next != null) 
                                {
                                    if (res.EndToken.Next.IsValue2("И", "ЧИСЛЕ") || res.EndToken.Next.IsValue2("И", "ПАДЕЖЕ")) 
                                        res.EndToken = res.EndToken.Next.Next;
                                }
                            }
                        }
                    }
                    if (res.ChangeVal.Value == null) 
                        return null;
                    return res;
                }
            }
            int isNexChange = 0;
            if (t != null && t.IsValue("В", "У") && t.Next != null) 
            {
                t = t.Next;
                if (t.IsValue("РЕДАКЦИЯ", "РЕДАКЦІЯ") && t.Next != null) 
                {
                    isNexChange = 1;
                    t = t.Next;
                }
            }
            if (((t.IsValue("СЛЕДУЮЩИЙ", "НАСТУПНИЙ") || tt.IsValue("ТАКОЙ", "ТАКИЙ"))) && t.Next != null && ((t.Next.IsValue("СОДЕРЖАНИЕ", "ЗМІСТ") || t.Next.IsValue("СОДЕРЖИМОЕ", "ВМІСТ") || t.Next.IsValue("РЕДАКЦИЯ", "РЕДАКЦІЯ")))) 
            {
                isNexChange = 2;
                t = t.Next.Next;
            }
            if (t == null) 
                return null;
            if (t.IsChar(':') && t.Next != null) 
            {
                if (t.Previous != null && t.Previous.IsValue("РЕДАКЦИЯ", "РЕДАКЦІЯ")) 
                    isNexChange++;
                tt = (t = t.Next);
                if (isNexChange > 0) 
                    isNexChange++;
            }
            else if (t.Previous != null && t.Previous.IsValue("РЕДАКЦИЯ", null) && Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t, true, false)) 
                isNexChange++;
            if ((t == tt && t.Previous != null && t.Previous.IsChar(':')) && Pullenti.Ner.Core.BracketHelper.IsBracket(t, false) && !t.IsChar('(')) 
                isNexChange = 1;
            Pullenti.Ner.Token xx1 = t;
            while (xx1 != null && xx1.IsTableControlChar) 
            {
                xx1 = xx1.Next;
            }
            if (((isNexChange > 0 && (((Pullenti.Ner.Core.MiscHelper.CheckImage(xx1) != null || Pullenti.Ner.Core.BracketHelper.IsBracket(xx1, true) || abzacRegime) || Pullenti.Ner.Core.ComplexNumToken.TryParse(xx1, null, false, false) != null)))) || ((isNexChange > 1 && t.IsValue("ПРИЛОЖЕНИЕ", "ДОДАТОК")))) 
            {
                if (isInEdition) 
                {
                    res = new DecreeChangeToken(t, xx1) { Typ = DecreeChangeTokenTyp.Value };
                    res.ChangeVal = new Pullenti.Ner.Decree.DecreeChangeValueReferent() { Kind = Pullenti.Ner.Decree.DecreeChangeValueKind.Text };
                    return res;
                }
                res = _tryParseText(xx1, abzacRegime, false, defPart);
                if (res == null) 
                    return null;
                res.BeginToken = t;
                return res;
            }
            if (tt.IsValue("ПОСЛЕ", "ПІСЛЯ")) 
            {
                List<PartToken> pt = PartToken.TryAttachList(tt.Next, false, 40);
                if (pt != null && pt.Count > 0) 
                {
                    Pullenti.Ner.Token tt2 = pt[pt.Count - 1].EndToken.Next;
                    while (tt2 != null && ((tt2.IsValue("НОВЫЙ", null) || tt2.IsValue("ДОПОЛНИТЬ", null)))) 
                    {
                        tt2 = tt2.Next;
                    }
                    PartToken pt2 = PartToken.TryAttach(tt2, null, false, true);
                    if (pt2 != null && pt2.Typ == pt[0].Typ) 
                    {
                        if (pt2.Values.Count > 0) 
                        {
                            res = new DecreeChangeToken(tt, pt2.EndToken);
                            res.Parts = pt;
                            res.ActKind = Pullenti.Ner.Decree.DecreeChangeKind.Append;
                            res.NewParts = new List<PartToken>();
                            res.NewParts.Add(pt2);
                            return res;
                        }
                        List<PartToken> pt1 = PartToken.TryAttachList(tt.Next, false, 40);
                        if (pt1[0].AddValue(1)) 
                        {
                            res = new DecreeChangeToken(tt, pt2.EndToken);
                            res.Parts = pt;
                            res.ActKind = Pullenti.Ner.Decree.DecreeChangeKind.Append;
                            res.NewParts = new List<PartToken>();
                            res.NewParts.AddRange(pt1);
                            return res;
                        }
                    }
                }
                res = TryAttach(tt.Next, null, false, null, false, false, null);
                if (res != null && res.Typ == DecreeChangeTokenTyp.Value) 
                {
                    res.Typ = DecreeChangeTokenTyp.AfterValue;
                    res.BeginToken = tt;
                    return res;
                }
            }
            if (tt.IsValue2("В", "КОНЦЕ")) 
            {
                List<PartToken> pts = PartToken.TryAttachList(tt.Next.Next, true, 40);
                if (pts != null && pts.Count > 0) 
                {
                    res = TryAttach(pts[pts.Count - 1].EndToken.Next, null, false, null, false, false, null);
                    if (res != null && res.Parts == null) 
                    {
                        res.BeginToken = tt;
                        res.InTheEnd = true;
                        if (res.ChangeVal != null) 
                            res.ChangeVal.AddSlot("POSITION", "end", false, 0);
                        res.Parts = pts;
                        return res;
                    }
                }
            }
            return null;
        }
        public static bool _canBeChangeToken(Pullenti.Ner.Token t)
        {
            for (; t != null; t = t.Next) 
            {
                if (!t.IsCharOf(";.,")) 
                    break;
            }
            if (t == null) 
                return false;
            if (m_Terms.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No) != null) 
                return true;
            List<PartToken> ps = PartToken.TryAttachList(t, false, 40);
            if (ps != null && ps.Count > 0) 
            {
                Pullenti.Ner.Token tt1 = ps[ps.Count - 1].EndToken.Next;
                if (tt1 == null) 
                    return false;
                if (m_Terms.TryParse(tt1, Pullenti.Ner.Core.TerminParseAttr.No) != null) 
                    return true;
                if (tt1.IsValue("ПОСЛЕ", null)) 
                    return true;
                return false;
            }
            Pullenti.Ner.Core.ComplexNumToken num = Pullenti.Ner.Core.ComplexNumToken.TryParse(t, null, false, false);
            if (num != null) 
                return _canBeChangeToken(num.EndToken.Next);
            return false;
        }
        static List<DecreeChangeToken> TryAttachList(Pullenti.Ner.Token t, bool ignoreNewline = false, bool abzacRegime = false)
        {
            if (t == null) 
                return null;
            if (t.IsNewlineBefore && !ignoreNewline) 
                return null;
            DecreeChangeToken d0 = TryAttach(t, null, false, null, false, abzacRegime, null);
            if (d0 == null || d0.Typ == DecreeChangeTokenTyp.Undefined) 
            {
                int cou = 0;
                for (Pullenti.Ner.Token tt2 = t.Next; tt2 != null && (cou < 20); tt2 = tt2.Next,cou++) 
                {
                    if (tt2.IsNewlineBefore || Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(tt2)) 
                        break;
                    DecreeChangeToken d1 = TryAttach(tt2, null, false, null, false, false, null);
                    if (d1 != null && d1.Typ == DecreeChangeTokenTyp.Action && d1.ActKind == Pullenti.Ner.Decree.DecreeChangeKind.Expire) 
                    {
                        d0 = d1;
                        break;
                    }
                }
                if (d0 == null) 
                    return null;
            }
            List<DecreeChangeToken> res = new List<DecreeChangeToken>();
            res.Add(d0);
            t = d0.EndToken.Next;
            if (d0.IndentRegime) 
                abzacRegime = true;
            else if (d0.NewParts != null) 
                abzacRegime = false;
            for (; t != null; t = t.Next) 
            {
                DecreeChangeToken d = TryAttach(t, null, false, null, false, abzacRegime, null);
                if (t.IsNewlineBefore) 
                {
                    if ((t.IsValue("ПРИЛОЖЕНИЕ", "ДОДАТОК") && t.Previous != null && t.Previous.IsChar(':')) && t.Previous.Previous != null && t.Previous.Previous.IsValue("РЕДАКЦИЯ", "РЕДАКЦІЯ")) 
                    {
                    }
                    else if (t.Previous != null && ((t.Previous.IsChar(':') || t.Previous.IsValue("РЕДАКЦИЯ", "РЕДАКЦІЯ"))) && ((Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t, true, false) || t.IsChar((char)0x1E)))) 
                    {
                    }
                    else if (d0.ActKind == Pullenti.Ner.Decree.DecreeChangeKind.Exchange && t.IsValue("НА", null)) 
                    {
                    }
                    else if ((d != null && d.Typ == DecreeChangeTokenTyp.StartSingle && d.ChangeVal != null) && t.Morph.Case.IsInstrumental) 
                        d.Typ = DecreeChangeTokenTyp.Value;
                    else 
                        break;
                }
                if (d == null && t.IsChar('.') && !t.IsNewlineAfter) 
                {
                    if (res[res.Count - 1].ChangeVal != null) 
                        break;
                    continue;
                }
                if (d == null && t.Previous.IsValue("НА", null) && Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t, true, false)) 
                    d = _tryParseText(t, abzacRegime, false, null);
                if (d == null && t.IsValue("ПОЗИЦИЯ", null) && ((res[res.Count - 1].ActKind == Pullenti.Ner.Decree.DecreeChangeKind.Expire || res[res.Count - 1].ActKind == Pullenti.Ner.Decree.DecreeChangeKind.Remove))) 
                {
                    Pullenti.Ner.Token tt2 = t.Next;
                    if (tt2 != null && tt2.IsChar(':')) 
                        tt2 = tt2.Next;
                    d = _tryParseText(tt2, abzacRegime, false, null);
                    if (d != null) 
                        res[res.Count - 1].ActKind = Pullenti.Ner.Decree.DecreeChangeKind.Remove;
                }
                if (d == null && t.IsAnd) 
                {
                    DecreeChangeToken dd = TryAttach(t.Next, null, false, null, false, abzacRegime, null);
                    if (dd != null && dd.Typ == DecreeChangeTokenTyp.AfterValue && res[res.Count - 1].Typ == dd.Typ) 
                        d = dd;
                }
                if (d == null) 
                {
                    if (t.IsValue("НОВЫЙ", "НОВИЙ")) 
                        continue;
                    if (t.IsValue("НА", null)) 
                        continue;
                    if (t.IsValue2("ПО", "ТЕКСТУ")) 
                    {
                        t = t.Next;
                        continue;
                    }
                    if (t.IsValue2("К", "ОН")) 
                    {
                        t = t.Next;
                        continue;
                    }
                    if (t.IsValue("ЭТОТ", null)) 
                    {
                        Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.ParsePronouns, 0, null);
                        if (npt != null) 
                        {
                            t = npt.EndToken;
                            continue;
                        }
                    }
                    if (t.IsChar(':') && ((!t.IsNewlineAfter || res[res.Count - 1].ActKind == Pullenti.Ner.Decree.DecreeChangeKind.New))) 
                        continue;
                    if ((t is Pullenti.Ner.TextToken) && (t as Pullenti.Ner.TextToken).Term == "ТЕКСТОМ") 
                        continue;
                    List<PartToken> pts = PartToken.TryAttachList(t, false, 40);
                    if (pts != null) 
                    {
                        if (m_Terms.TryParse(pts[pts.Count - 1].EndToken.Next, Pullenti.Ner.Core.TerminParseAttr.No) != null) 
                        {
                        }
                        else 
                        {
                            d = new DecreeChangeToken(pts[0].BeginToken, pts[pts.Count - 1].EndToken) { Typ = DecreeChangeTokenTyp.Undefined };
                            if (t.Previous != null && t.Previous.IsValue("НОВЫЙ", "НОВИЙ")) 
                                d.NewParts = pts;
                            else 
                                d.Parts = pts;
                        }
                    }
                    else 
                    {
                        PartToken pt = PartToken.TryAttach(t, null, true, false);
                        if (pt == null) 
                            pt = PartToken.TryAttach(t, null, true, true);
                        if (pt != null) 
                        {
                            d = new DecreeChangeToken(pt.BeginToken, pt.EndToken);
                            if (t.Previous != null && t.Previous.IsValue("НОВЫЙ", "НОВИЙ")) 
                            {
                                d.NewParts = new List<PartToken>();
                                d.NewParts.Add(pt);
                            }
                            else 
                                d.PartTyp = pt.Typ;
                        }
                    }
                }
                if (d == null && res[res.Count - 1].ActKind == Pullenti.Ner.Decree.DecreeChangeKind.New) 
                {
                    if (Pullenti.Ner.Core.BracketHelper.IsBracket(t, true)) 
                        d = _tryParseText(t, abzacRegime, false, null);
                    else if (((t.Previous.IsChar(':') || t.Previous.IsValue("РЕДАКЦИЯ", null))) && (t is Pullenti.Ner.NumberToken)) 
                    {
                        Pullenti.Ner.Core.ComplexNumToken nn = Pullenti.Ner.Core.ComplexNumToken.TryParse(t, null, false, false);
                        if (nn != null && Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(nn.EndToken.Next, true, false)) 
                        {
                            d = _tryParseText(nn.EndToken.Next, abzacRegime, false, null);
                            if (d != null && d.ChangeVal != null) 
                            {
                                d.BeginToken = t;
                                d.ChangeVal.Value = string.Format("{0} {1}", nn.GetSourceText(), d.ChangeVal.Value);
                            }
                        }
                    }
                    else if (t.IsChar((char)0x1E)) 
                    {
                        List<Pullenti.Ner.Core.TableRowToken> rows = Pullenti.Ner.Core.TableHelper.TryParseRows(t, 0, true, false);
                        if (rows != null) 
                        {
                            d = new DecreeChangeToken(t, rows[rows.Count - 1].EndToken) { Typ = DecreeChangeTokenTyp.Value };
                            d.ChangeVal = new Pullenti.Ner.Decree.DecreeChangeValueReferent() { Kind = Pullenti.Ner.Decree.DecreeChangeValueKind.Text };
                            d.ChangeVal.BeginChar = t.BeginChar;
                            d.ChangeVal.EndChar = d.EndChar;
                            d.ChangeVal.Value = d.GetSourceText();
                            if (d.EndToken.Next != null && Pullenti.Ner.Core.BracketHelper.IsBracket(d.EndToken.Next, true)) 
                            {
                                if (d.EndToken.Next.IsNewlineAfter) 
                                    d.EndToken = d.EndToken.Next;
                                else if (d.EndToken.Next.Next.IsCharOf(".;")) 
                                    d.EndToken = d.EndToken.Next.Next;
                            }
                        }
                    }
                }
                if (d == null && res[res.Count - 1].ActKind == Pullenti.Ner.Decree.DecreeChangeKind.Exchange) 
                {
                    if (t.IsValue("НА", null) && t.Next != null) 
                        t = t.Next;
                    if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t, true, false)) 
                        d = _tryParseText(t, abzacRegime, false, null);
                }
                if (d == null) 
                    break;
                if (d.Typ == DecreeChangeTokenTyp.Single || d.Typ == DecreeChangeTokenTyp.StartMultu || d.Typ == DecreeChangeTokenTyp.StartSingle) 
                    break;
                res.Add(d);
                t = d.EndToken;
                if (d.IndentRegime) 
                    abzacRegime = true;
                else if (d.NewParts != null) 
                    abzacRegime = false;
            }
            if ((res.Count == 1 && res[0].ActKind == Pullenti.Ner.Decree.DecreeChangeKind.Consider && res[0].EndToken.Next != null) && res[0].EndToken.Next.IsCommaAnd && res[0].ChangeVal == null) 
            {
                List<DecreeChangeToken> next = TryAttachList(res[0].EndToken.Next.Next, true, abzacRegime);
                if ((next != null && next.Count == 2 && next[0].ActKind == Pullenti.Ner.Decree.DecreeChangeKind.New) && next[1].Typ == DecreeChangeTokenTyp.Value) 
                    return next;
            }
            return res;
        }
        public static Pullenti.Ner.Core.TerminCollection m_Terms;
        static Pullenti.Ner.Core.Termin m_Words;
        internal static void Initialize()
        {
            if (m_Terms != null) 
                return;
            m_Terms = new Pullenti.Ner.Core.TerminCollection();
            Pullenti.Ner.Core.Termin t;
            t = new Pullenti.Ner.Core.Termin("ИЗЛОЖИТЬ В СЛЕДУЮЩЕЙ РЕДАКЦИИ") { Tag = Pullenti.Ner.Decree.DecreeChangeKind.New };
            t.AddVariant("ПРИНЯТЬ В СЛЕДУЮЩЕЙ РЕДАКЦИИ", false);
            t.AddVariant("ИЗЛОЖИВ ЕГО В СЛЕДУЮЩЕЙ РЕДАКЦИИ", false);
            t.AddVariant("ИЗЛОЖИТЬ В РЕДАКЦИИ", false);
            t.AddVariant("ИЗЛОЖИТЬ", false);
            t.AddVariant("ПРИНЯТЬ", false);
            t.AddVariant("ИЗЛОЖИТЬ В НОВОЙ РЕДАКЦИИ", false);
            t.AddVariant("ЧИТАТЬ В СЛЕДУЮЩЕЙ РЕДАКЦИИ", false);
            m_Terms.Add(t);
            t = new Pullenti.Ner.Core.Termin("ВИКЛАСТИ В НАСТУПНІЙ РЕДАКЦІЇ", Pullenti.Morph.MorphLang.UA) { Tag = Pullenti.Ner.Decree.DecreeChangeKind.New };
            t.AddVariant("ВИКЛАВШИ В ТАКІЙ РЕДАКЦІЇ", false);
            t.AddVariant("ВИКЛАВШИ ЙОГО В НАСТУПНІЙ РЕДАКЦІЇ", false);
            t.AddVariant("ВИКЛАСТИ В РЕДАКЦІЇ", false);
            m_Terms.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПРИЗНАТЬ УТРАТИВШИМ СИЛУ") { Tag = Pullenti.Ner.Decree.DecreeChangeKind.Expire };
            t.AddVariant("СЧИТАТЬ УТРАТИВШИМ СИЛУ", false);
            m_Terms.Add(t);
            t = new Pullenti.Ner.Core.Termin("ВИЗНАТИ таким, що ВТРАТИВ ЧИННІСТЬ", Pullenti.Morph.MorphLang.UA) { Tag = Pullenti.Ner.Decree.DecreeChangeKind.Expire };
            t.AddVariant("ВВАЖАТИ таким, що ВТРАТИВ ЧИННІСТЬ", false);
            m_Terms.Add(t);
            t = new Pullenti.Ner.Core.Termin("ИСКЛЮЧИТЬ") { Tag = Pullenti.Ner.Decree.DecreeChangeKind.Remove };
            t.AddVariant("ИСКЛЮЧИВ ИЗ НЕГО", false);
            t.AddVariant("УДАЛИТЬ", false);
            t.AddVariant("УДАЛИВ ИЗ НЕГО", false);
            m_Terms.Add(t);
            t = new Pullenti.Ner.Core.Termin("ВИКЛЮЧИТИ", Pullenti.Morph.MorphLang.UA) { Tag = Pullenti.Ner.Decree.DecreeChangeKind.Remove };
            t.AddVariant("ВИКЛЮЧИВШИ З НЬОГО", false);
            m_Terms.Add(t);
            t = new Pullenti.Ner.Core.Termin("ПРИОСТАНОВИТЬ ДЕЙСТВИЕ") { Tag = Pullenti.Ner.Decree.DecreeChangeKind.Suspend };
            m_Terms.Add(t);
            t = new Pullenti.Ner.Core.Termin("СЧИТАТЬ") { Tag = Pullenti.Ner.Decree.DecreeChangeKind.Consider };
            t.AddVariant("СЧИТАТЬ СООТВЕТСТВЕННО", false);
            m_Terms.Add(t);
            t = new Pullenti.Ner.Core.Termin("ВВАЖАТИ", Pullenti.Morph.MorphLang.UA) { Tag = Pullenti.Ner.Decree.DecreeChangeKind.Consider };
            t.AddVariant("ВВАЖАТИ ВІДПОВІДНО", false);
            m_Terms.Add(t);
            t = new Pullenti.Ner.Core.Termin("ЗАМЕНИТЬ") { Tag = Pullenti.Ner.Decree.DecreeChangeKind.Exchange };
            t.AddVariant("ЗАМЕНИВ В НЕМ", false);
            t.AddVariant("ИСКЛЮЧИТЬ ЗАМЕНИТЬ", false);
            t.AddVariant("ИСКЛЮЧИТЬ И ЗАМЕНИТЬ", false);
            m_Terms.Add(t);
            t = new Pullenti.Ner.Core.Termin("ЗАМІНИТИ", Pullenti.Morph.MorphLang.UA) { Tag = Pullenti.Ner.Decree.DecreeChangeKind.Exchange };
            t.AddVariant("ЗАМІНИВШИ В НЬОМУ", false);
            m_Terms.Add(t);
            t = new Pullenti.Ner.Core.Termin("ДОПОЛНИТЬ") { Tag = Pullenti.Ner.Decree.DecreeChangeKind.Append };
            t.AddVariant("ДОПОЛНИВ ЕГО", false);
            t.AddVariant("ДОБАВИТЬ", false);
            m_Terms.Add(t);
            t = new Pullenti.Ner.Core.Termin("ДОПОВНИТИ", Pullenti.Morph.MorphLang.UA) { Tag = Pullenti.Ner.Decree.DecreeChangeKind.Append };
            t.AddVariant("ДОПОВНИВШИ ЙОГО", false);
            m_Terms.Add(t);
            t = new Pullenti.Ner.Core.Termin("СЛОВО") { Tag = Pullenti.Ner.Decree.DecreeChangeValueKind.Words };
            m_Words = t;
            t.AddVariant("АББРЕВИАТУРА", false);
            t.AddVariant("АБРЕВІАТУРА", false);
            t.AddVariant("ЗНАК СНОСКИ", false);
            t.AddVariant("ПЕРЕМЕННАЯ", false);
            t.AddVariant("МНОЖИТЕЛЬ", false);
            t.AddVariant("КОД", false);
            t.AddVariant("СОЮЗ", false);
            t.AddVariant("ТЕКСТ СЛЕДУЮЩЕГО СОДЕРЖАНИЯ", false);
            t.AddVariant("ДАТА", false);
            t.AddVariant("СТРОКА", false);
            m_Terms.Add(t);
            t = new Pullenti.Ner.Core.Termin("ОПРЕДЕЛЕНИЕ") { Tag = Pullenti.Ner.Decree.DecreeChangeValueKind.Definition, Tag2 = ";" };
            m_Terms.Add(t);
            t = new Pullenti.Ner.Core.Termin("ТОЧКА С ЗАПЯТОЙ") { Tag = Pullenti.Ner.Decree.DecreeChangeValueKind.Words, Tag2 = ";" };
            m_Terms.Add(t);
            t = new Pullenti.Ner.Core.Termin("ТОЧКА") { Tag = Pullenti.Ner.Decree.DecreeChangeValueKind.Words, Tag2 = "." };
            m_Terms.Add(t);
            t = new Pullenti.Ner.Core.Termin("ВОПРОСИТЕЛЬНЫЙ ЗНАК") { Tag = Pullenti.Ner.Decree.DecreeChangeValueKind.Words, Tag2 = "?" };
            t.AddVariant("ЗНАК ВОПРОСА", false);
            m_Terms.Add(t);
            t = new Pullenti.Ner.Core.Termin("ВОСКЛИЦАТЕЛЬНЫЙ ЗНАК") { Tag = Pullenti.Ner.Decree.DecreeChangeValueKind.Words, Tag2 = "!" };
            m_Terms.Add(t);
            t = new Pullenti.Ner.Core.Termin("ЦИФРА") { Tag = Pullenti.Ner.Decree.DecreeChangeValueKind.Numbers };
            t.AddVariant("ЧИСЛО", false);
            m_Terms.Add(t);
            t = new Pullenti.Ner.Core.Termin("БЛОК") { Tag = Pullenti.Ner.Decree.DecreeChangeValueKind.Block };
            t.AddVariant("БЛОК СО СЛОВАМИ", false);
            m_Terms.Add(t);
            t = new Pullenti.Ner.Core.Termin("БЛОК", Pullenti.Morph.MorphLang.UA) { Tag = Pullenti.Ner.Decree.DecreeChangeValueKind.Block };
            t.AddVariant("БЛОК ЗІ СЛОВАМИ", false);
            m_Terms.Add(t);
            t = new Pullenti.Ner.Core.Termin("В СООТВЕТСТВУЮЩИХ ЧИСЛЕ И ПАДЕЖЕ") { Tag = Pullenti.Ner.Decree.DecreeChangeValueKind.RobustWords };
            t.AddVariant("В СООТВЕТСТВУЮЩЕМ ПАДЕЖЕ", false);
            t.AddVariant("В СООТВЕТСТВУЮЩЕМ ПАДЕЖЕ И ЧИСЛЕ", false);
            t.AddVariant("В СООТВЕТСТВУЮЩЕМ ЧИСЛЕ", false);
            m_Terms.Add(t);
            t = new Pullenti.Ner.Core.Termin("У ВІДПОВІДНОМУ ЧИСЛІ ТА ВІДМІНКУ", Pullenti.Morph.MorphLang.UA) { Tag = Pullenti.Ner.Decree.DecreeChangeValueKind.RobustWords };
            t.AddVariant("У ВІДПОВІДНОМУ ВІДМІНКУ", false);
            t.AddVariant("У ВІДПОВІДНОМУ ЧИСЛІ", false);
            m_Terms.Add(t);
        }
    }
}