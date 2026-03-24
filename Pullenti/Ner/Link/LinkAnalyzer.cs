/*
 * SDK Pullenti Lingvo, version 4.33, fabruary 2026. Copyright (c) 2013-2026, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Ner.Link
{
    /// <summary>
    /// Анализатор связей (специфический анализатор)
    /// </summary>
    public class LinkAnalyzer : Pullenti.Ner.Analyzer
    {
        /// <summary>
        /// Имя анализатора ("LINK")
        /// </summary>
        public const string ANALYZER_NAME = "LINK";
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
                return "Связи";
            }
        }
        public override string Description
        {
            get
            {
                return "Связи выделенных сущностй (досье)";
            }
        }
        public override Pullenti.Ner.Analyzer Clone()
        {
            return new LinkAnalyzer();
        }
        /// <summary>
        /// Специфический анализатор
        /// </summary>
        public override bool IsSpecific
        {
            get
            {
                return true;
            }
        }
        public override ICollection<Pullenti.Ner.Metadata.ReferentClass> TypeSystem
        {
            get
            {
                return new Pullenti.Ner.Metadata.ReferentClass[] {MetaLink.GlobalMeta};
            }
        }
        public override Dictionary<string, byte[]> Images
        {
            get
            {
                Dictionary<string, byte[]> res = new Dictionary<string, byte[]>();
                res.Add(MetaLink.ImageId.ToString(), Pullenti.Ner.Core.Internal.ResourceHelper3.GetBytes("link.png"));
                return res;
            }
        }
        public override Pullenti.Ner.Referent CreateReferent(string type)
        {
            if (type == LinkReferent.OBJ_TYPENAME) 
                return new LinkReferent();
            return null;
        }
        public override int ProgressWeight
        {
            get
            {
                return 1;
            }
        }
        public override void Process(Pullenti.Ner.Core.AnalysisKit kit)
        {
            Pullenti.Ner.Core.AnalyzerData ad = kit.GetAnalyzerData(this);
            Pullenti.Ner.Token t0 = kit.FirstToken;
            int cou = 10;
            for (Pullenti.Ner.Token t = t0; t != null && cou > 0; t = t.Next) 
            {
                if (!t.IsNewlineBefore) 
                    continue;
                cou--;
                Pullenti.Ner.Core.ComplexNumToken num = Pullenti.Ner.Core.ComplexNumToken.TryParse(t, null, false, false);
                if (num != null) 
                    break;
                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt == null || !npt.IsNewlineAfter) 
                    continue;
                if (npt.EndToken.IsValue("ЗАПИСКА", null) || npt.EndToken.IsValue("СПРАВКА", null) || npt.EndToken.IsValue("ОТЧЕТ", null)) 
                {
                    t0 = npt.EndToken.Next;
                    break;
                }
            }
            Pullenti.Ner.Person.PersonReferent mainPers = null;
            Pullenti.Ner.Person.PersonReferent curPers = null;
            Pullenti.Ner.Org.OrganizationReferent curOrg = null;
            Pullenti.Ner.Resume.ResumeItemType curTyp = Pullenti.Ner.Resume.ResumeItemType.Undefined;
            List<Pullenti.Ner.ReferentToken> tmpOrgs = new List<Pullenti.Ner.ReferentToken>();
            for (Pullenti.Ner.Token t = t0; t != null; t = t.Next) 
            {
                bool nl = false;
                if (t.IsNewlineBefore) 
                {
                    nl = true;
                    Pullenti.Ner.Core.ComplexNumToken num = Pullenti.Ner.Core.ComplexNumToken.TryParse(t, null, false, false);
                    if (num != null && num.EndToken.Next != null) 
                    {
                        t = num.EndToken.Next;
                        nl = true;
                    }
                }
                if (curOrg != null && mainPers != null && curOrg.Occurrence.Count > 0) 
                {
                    if ((t.BeginChar - curOrg.Occurrence[curOrg.Occurrence.Count - 1].EndChar) > 100) 
                        curOrg = null;
                }
                Pullenti.Ner.Referent r = t.GetReferent();
                Pullenti.Ner.Person.PersonReferent p = r as Pullenti.Ner.Person.PersonReferent;
                if (p != null) 
                {
                    curOrg = null;
                    if (_isObject(p)) 
                    {
                        curPers = p;
                        continue;
                    }
                    if (p == mainPers || p == curPers) 
                        continue;
                    if (mainPers == null || nl) 
                    {
                        mainPers = p;
                        _processPerson(p, ad);
                        continue;
                    }
                    if (mainPers != null) 
                    {
                        string nam = p.GetStringValue(Pullenti.Ner.Person.PersonReferent.ATTR_FIRSTNAME);
                        if (nam == null || (nam.Length < 2)) 
                            continue;
                        _processPerson(p, ad);
                        LinkReferent li = new LinkReferent() { Object1 = mainPers, Object2 = p };
                        li = ad.RegisterReferent(li) as LinkReferent;
                        li.AddOccurenceOfRefTok(new Pullenti.Ner.ReferentToken(li, t, t));
                        continue;
                    }
                    curPers = p;
                    _processPerson(p, ad);
                    continue;
                }
                p = curPers ?? mainPers;
                if (p == null) 
                {
                    if (r is Pullenti.Ner.Org.OrganizationReferent) 
                    {
                        curOrg = r as Pullenti.Ner.Org.OrganizationReferent;
                        _setObject(r);
                        continue;
                    }
                    else if (curOrg == null) 
                        continue;
                }
                else if (((p.IsFemale && !p.IsMale)) || ((!p.IsFemale && p.IsMale))) 
                {
                }
                else 
                    Pullenti.Ner.Person.Internal.PersonHelper.CreateSex(p, t);
                Pullenti.Ner.Geo.GeoReferent geo = r as Pullenti.Ner.Geo.GeoReferent;
                if (geo != null && geo.IsCity && p != null) 
                {
                    for (Pullenti.Ner.Token tt = t.Previous; tt != null; tt = tt.Previous) 
                    {
                        Pullenti.Ner.TextToken te = tt as Pullenti.Ner.TextToken;
                        if (te != null) 
                        {
                            if (te.Term.StartsWith("УР") || te.Term.StartsWith("РОДИ") || te.Term.StartsWith("РОЖД")) 
                            {
                                _setObject(geo);
                                LinkReferent li = new LinkReferent() { Typ = LinkType.Born, Object1 = p, Object2 = geo };
                                li = ad.RegisterReferent(li) as LinkReferent;
                                li.AddOccurenceOfRefTok(new Pullenti.Ner.ReferentToken(li, tt, t));
                                break;
                            }
                        }
                        if (tt.GetReferent() is Pullenti.Ner.Date.DateReferent) 
                            continue;
                        if (tt.LengthChar > 1) 
                            break;
                    }
                }
                if (p != null && p.FindSlot(Pullenti.Ner.Person.PersonReferent.ATTR_BORN, null, true) == null && (t.GetReferent() is Pullenti.Ner.Date.DateReferent)) 
                {
                    for (Pullenti.Ner.Token tt = t.Previous; tt != null; tt = tt.Previous) 
                    {
                        Pullenti.Ner.TextToken te = tt as Pullenti.Ner.TextToken;
                        if (te != null) 
                        {
                            if (te.Term.StartsWith("УРОЖ") || te.Term.StartsWith("РОДИ") || te.Term.StartsWith("РОЖД")) 
                            {
                                p.AddSlot(Pullenti.Ner.Person.PersonReferent.ATTR_BORN, t.GetReferent(), false, 0);
                                break;
                            }
                        }
                        if (tt.GetReferent() is Pullenti.Ner.Geo.GeoReferent) 
                            continue;
                        if (tt.LengthChar > 1) 
                            break;
                    }
                    if (t.Next is Pullenti.Ner.TextToken) 
                    {
                        if ((t.Next as Pullenti.Ner.TextToken).Term.StartsWith("РОДИ") || (t.Next as Pullenti.Ner.TextToken).Term.StartsWith("РОЖД")) 
                            p.AddSlot(Pullenti.Ner.Person.PersonReferent.ATTR_BORN, t.GetReferent(), false, 0);
                    }
                }
                if (r is Pullenti.Ner.Phone.PhoneReferent) 
                {
                    _setObject(r);
                    LinkReferent li = new LinkReferent() { Typ = LinkType.Contact, Object2 = r };
                    if (curOrg != null) 
                        li.Object1 = curOrg;
                    else 
                        li.Object1 = p;
                    li = ad.RegisterReferent(li) as LinkReferent;
                    li.AddOccurenceOfRefTok(new Pullenti.Ner.ReferentToken(li, t, t));
                    continue;
                }
                if (r is Pullenti.Ner.Uri.UriReferent) 
                {
                    string sh = (r as Pullenti.Ner.Uri.UriReferent).Scheme;
                    if (sh == "http" || sh == "https" || sh == "www") 
                        continue;
                    _setObject(r);
                    LinkReferent li = new LinkReferent() { Typ = LinkType.Contact, Object1 = p, Object2 = r };
                    if (curOrg != null) 
                        li.Object1 = curOrg;
                    else 
                        li.Object1 = p;
                    li = ad.RegisterReferent(li) as LinkReferent;
                    li.AddOccurenceOfRefTok(new Pullenti.Ner.ReferentToken(li, t, t));
                    continue;
                }
                if (r is Pullenti.Ner.Person.PersonIdentityReferent) 
                {
                    if (p == null) 
                        continue;
                    _setObject(r);
                    LinkReferent li = new LinkReferent() { Typ = LinkType.Document, Object1 = p, Object2 = r };
                    foreach (Pullenti.Ner.Slot s in r.Slots) 
                    {
                        if (s.Value is Pullenti.Ner.Date.DateReferent) 
                        {
                            li.DateFrom = s.Value as Pullenti.Ner.Date.DateReferent;
                            break;
                        }
                    }
                    li = ad.RegisterReferent(li) as LinkReferent;
                    li.AddOccurenceOfRefTok(new Pullenti.Ner.ReferentToken(li, t, t));
                    continue;
                }
                if ((r is Pullenti.Ner.Address.StreetReferent) || (r is Pullenti.Ner.Address.AddressReferent)) 
                {
                    _setObject(r);
                    LinkReferent li = new LinkReferent() { Typ = LinkType.Address, Object2 = r };
                    if (curOrg != null) 
                        li.Object1 = curOrg;
                    else 
                        li.Object1 = p;
                    _corrAddrTyp(li, t);
                    li = ad.RegisterReferent(li) as LinkReferent;
                    li.AddOccurenceOfRefTok(new Pullenti.Ner.ReferentToken(li, t, t));
                    continue;
                }
                if (r != null && p != null) 
                {
                    if (r.TypeName == "WEAPON" || r.TypeName == "TRANSPORT") 
                    {
                        _setObject(r);
                        LinkReferent li = new LinkReferent() { Object1 = p, Object2 = r };
                        li = ad.RegisterReferent(li) as LinkReferent;
                        li.AddOccurenceOfRefTok(new Pullenti.Ner.ReferentToken(li, t, t));
                        continue;
                    }
                }
                if (t.IsValue("ОБРАЗОВАНИЕ", null)) 
                    curTyp = Pullenti.Ner.Resume.ResumeItemType.Study;
                if ((t.IsValue2("ОПЫТ", "РАБОТЫ") || t.IsValue2("МЕСТО", "РАБОТЫ") || t.IsValue2("ПЕРИОД", "РАБОТЫ")) || t.IsValue("РАБОТАТЬ", null)) 
                    curTyp = Pullenti.Ner.Resume.ResumeItemType.Organization;
                if (p == null) 
                    continue;
                if (t.IsNewlineBefore) 
                {
                    Pullenti.Ner.ReferentToken rt = Pullenti.Ner.Resume.ResumeAnalyzer._parseOrg(t, curTyp);
                    if (rt != null) 
                    {
                        curTyp = (rt.Referent as Pullenti.Ner.Resume.ResumeItemReferent).Typ;
                        LinkReferent li = _processResumeOrg(p, ad, rt);
                        if (li != null) 
                        {
                            ad.RegisterReferent(li);
                            li.AddOccurenceOfRefTok(rt);
                            curOrg = li.Object2 as Pullenti.Ner.Org.OrganizationReferent;
                        }
                        t = rt.EndToken;
                        continue;
                    }
                }
                tmpOrgs.Clear();
                Pullenti.Ner.Resume.ResumeAnalyzer._parseOrg2(t, null, tmpOrgs);
                if (tmpOrgs.Count > 0) 
                {
                    foreach (Pullenti.Ner.ReferentToken rt1 in tmpOrgs) 
                    {
                        LinkReferent li = _processResumeOrg(p, ad, rt1);
                        if (li != null) 
                        {
                            ad.RegisterReferent(li);
                            li.AddOccurenceOfRefTok(rt1);
                            curOrg = li.Object2 as Pullenti.Ner.Org.OrganizationReferent;
                        }
                        t = rt1.EndToken;
                    }
                    continue;
                }
                if (r is Pullenti.Ner.Org.OrganizationReferent) 
                {
                    curOrg = r as Pullenti.Ner.Org.OrganizationReferent;
                    _processOrg(r as Pullenti.Ner.Org.OrganizationReferent, ad, 0);
                    LinkReferent li = new LinkReferent() { Object1 = p, Object2 = r };
                    Pullenti.Ner.ReferentToken rt1 = _corrOrgTyp(t, t, li);
                    if (li.Typ != LinkType.Undefined || li.Param != null) 
                    {
                        li = ad.RegisterReferent(li) as LinkReferent;
                        li.AddOccurenceOfRefTok(rt1);
                    }
                    continue;
                }
                if (r is Pullenti.Ner.Person.PersonPropertyReferent) 
                {
                    LinkReferent li = _processPersonAttr(p, r as Pullenti.Ner.Person.PersonPropertyReferent, ad);
                    if (li != null) 
                    {
                        Pullenti.Ner.ReferentToken rt1 = _corrOrgTyp(t, t, li);
                        ad.RegisterReferent(li);
                        li.AddOccurenceOfRefTok(rt1);
                        curOrg = li.Object2 as Pullenti.Ner.Org.OrganizationReferent;
                    }
                    continue;
                }
                Pullenti.Ner.Person.Internal.PersonAttrToken pat = Pullenti.Ner.Person.Internal.PersonAttrToken.TryAttach(t, Pullenti.Ner.Person.Internal.PersonAttrToken.PersonAttrAttachAttrs.No);
                if (pat != null && pat.PropRef != null) 
                {
                    LinkReferent li = _processPersonAttr(p, pat.PropRef, ad);
                    if (li != null) 
                    {
                        Pullenti.Ner.ReferentToken rt1 = _corrOrgTyp(t, pat.EndToken, li);
                        ad.RegisterReferent(li);
                        li.AddOccurenceOfRefTok(rt1);
                        curOrg = li.Object2 as Pullenti.Ner.Org.OrganizationReferent;
                        t = pat.EndToken;
                        continue;
                    }
                }
            }
            foreach (Pullenti.Ner.Referent r in ad.Referents) 
            {
                LinkReferent li = r as LinkReferent;
                if (li == null) 
                    continue;
                if (li.Typ != LinkType.Born) 
                    continue;
                if (!(li.Object1 is Pullenti.Ner.Person.PersonReferent)) 
                    continue;
                Pullenti.Ner.Date.DateReferent dt = li.Object1.GetSlotValue(Pullenti.Ner.Person.PersonReferent.ATTR_BORN) as Pullenti.Ner.Date.DateReferent;
                if (dt != null && li.DateFrom == null) 
                    li.DateFrom = dt;
            }
        }
        static bool _isObject(Pullenti.Ner.Referent r)
        {
            return r.GetStringValue("OBJECT") == "true";
        }
        static void _setObject(Pullenti.Ner.Referent r)
        {
            r.AddSlot("OBJECT", "true", true, 0);
        }
        static void _processPerson(Pullenti.Ner.Person.PersonReferent p, Pullenti.Ner.Core.AnalyzerData ad)
        {
            if (_isObject(p)) 
                return;
            _setObject(p);
            for (int i = p.Slots.Count - 1; i >= 0; i--) 
            {
                Pullenti.Ner.Slot sl = p.Slots[i];
                Pullenti.Ner.Referent r = sl.Value as Pullenti.Ner.Referent;
                if ((r is Pullenti.Ner.Uri.UriReferent) || (r is Pullenti.Ner.Phone.PhoneReferent)) 
                {
                    _setObject(r);
                    ad.RegisterReferent(new LinkReferent() { Object1 = p, Object2 = r, Typ = LinkType.Contact });
                    p.Slots.RemoveAt(i);
                    continue;
                }
                if (r is Pullenti.Ner.Person.PersonPropertyReferent) 
                {
                    LinkReferent li = _processPersonAttr(p, r as Pullenti.Ner.Person.PersonPropertyReferent, ad);
                    if (li != null) 
                    {
                        p.Slots.RemoveAt(i);
                        ad.RegisterReferent(li);
                    }
                }
            }
        }
        static LinkReferent _processPersonAttr(Pullenti.Ner.Person.PersonReferent p, Pullenti.Ner.Person.PersonPropertyReferent pr, Pullenti.Ner.Core.AnalyzerData ad)
        {
            string nam = pr.Name;
            if (pr.Higher != null) 
            {
                string nam0 = Pullenti.Ner.Core.MiscHelper.GetTextMorphVarByCase(pr.Higher.Name, Pullenti.Morph.MorphCase.Genitive, false);
                nam = string.Format("{0} {1}", nam, nam0 ?? pr.Higher.Name);
                pr = pr.Higher;
            }
            LinkReferent li = new LinkReferent() { Object1 = p };
            Pullenti.Ner.Referent r = pr.GetSlotValue(Pullenti.Ner.Person.PersonPropertyReferent.ATTR_REF) as Pullenti.Ner.Referent;
            if (r is Pullenti.Ner.Org.OrganizationReferent) 
            {
                _processOrg(r as Pullenti.Ner.Org.OrganizationReferent, ad, 0);
                li.Object2 = r;
                if (pr.Kind == Pullenti.Ner.Person.PersonPropertyKind.Boss) 
                    li.Typ = LinkType.Work;
            }
            else if (r is Pullenti.Ner.Person.PersonReferent) 
            {
                _processPerson(r as Pullenti.Ner.Person.PersonReferent, ad);
                li.Object2 = r;
                if (pr.Kind == Pullenti.Ner.Person.PersonPropertyKind.Kin) 
                    li.Typ = LinkType.Family;
            }
            else if (r is Pullenti.Ner.Geo.GeoReferent) 
            {
                if ((r as Pullenti.Ner.Geo.GeoReferent).IsCity && nam == "уроженец") 
                {
                    _setObject(r);
                    li.Object2 = r;
                    li.Typ = LinkType.Born;
                    return li;
                }
                else 
                    return null;
            }
            else 
                return null;
            li.Param = nam;
            return li;
        }
        static LinkReferent _processResumeOrg(Pullenti.Ner.Person.PersonReferent p, Pullenti.Ner.Core.AnalyzerData ad, Pullenti.Ner.ReferentToken rt)
        {
            Pullenti.Ner.Resume.ResumeItemReferent rr = rt.Referent as Pullenti.Ner.Resume.ResumeItemReferent;
            if (!(rr.Ref is Pullenti.Ner.Org.OrganizationReferent)) 
                return null;
            _processOrg(rr.Ref as Pullenti.Ner.Org.OrganizationReferent, ad, 0);
            LinkReferent li = new LinkReferent() { Typ = (rr.Typ == Pullenti.Ner.Resume.ResumeItemType.Study ? LinkType.Study : LinkType.Work), Object1 = p, Object2 = rr.Ref };
            li.Param = rr.Value;
            Pullenti.Ner.Date.DateRangeReferent dr = rr.DateRange as Pullenti.Ner.Date.DateRangeReferent;
            if (dr != null) 
            {
                li.DateFrom = dr.DateFrom;
                li.DateTo = dr.DateTo;
            }
            if (rr.DateRange is Pullenti.Ner.Date.DateReferent) 
                li.DateFrom = (li.DateTo = rr.DateRange as Pullenti.Ner.Date.DateReferent);
            return li;
        }
        static void _processOrg(Pullenti.Ner.Org.OrganizationReferent org, Pullenti.Ner.Core.AnalyzerData ad, int lev = 0)
        {
            if (lev > 4) 
                return;
            if (_isObject(org)) 
                return;
            _setObject(org);
            if (org.Higher == null) 
                return;
            _processOrg(org.Higher, ad, lev + 1);
            ad.RegisterReferent(new LinkReferent() { Object1 = org, Object2 = org.Higher, Typ = LinkType.Unit });
            org.Higher = null;
        }
        static Pullenti.Ner.ReferentToken _corrOrgTyp(Pullenti.Ner.Token tb, Pullenti.Ner.Token te, LinkReferent li)
        {
            Pullenti.Ner.Date.DateReferent dt = null;
            Pullenti.Ner.Date.DateRangeReferent dr = null;
            int cou = 5;
            Pullenti.Ner.Token t0 = tb;
            Pullenti.Ner.Token t1 = te;
            for (Pullenti.Ner.Token tt = tb.Previous; tt != null && cou > 0; tt = tt.Previous) 
            {
                if (tt.IsNewlineAfter) 
                    break;
                if (tt.LengthChar == 1) 
                    continue;
                if (tt.GetReferent() is Pullenti.Ner.Date.DateReferent) 
                {
                    dt = tt.GetReferent() as Pullenti.Ner.Date.DateReferent;
                    t0 = tt;
                }
                else if (tt.GetReferent() is Pullenti.Ner.Date.DateRangeReferent) 
                    dr = tt.GetReferent() as Pullenti.Ner.Date.DateRangeReferent;
                else if (tt is Pullenti.Ner.ReferentToken) 
                    break;
                else if ((tt.IsValue("ОБРАЗОВАНИЕ", null) || tt.IsValue("ОКОНЧИТЬ", null) || tt.IsValue("ОБУЧАТЬСЯ", null)) || tt.IsValue("ЗАКОНЧИТЬ", null)) 
                    li.Typ = LinkType.Study;
                else if (tt.IsValue("РАБОТАТЬ", null) || tt.IsValue2("ПРОХОДИТЬ", "ПРАКТИКА")) 
                    li.Typ = LinkType.Work;
                else if (tt.IsValue("ПОСТУПИТЬ", null)) 
                {
                    if (tt.Next != null && tt.Next.IsValue2("НА", "РАБОТА")) 
                        li.Typ = LinkType.Work;
                    else 
                        li.Typ = LinkType.Study;
                }
                else 
                    cou--;
            }
            if (li.Typ != LinkType.Undefined && dt == null && dr == null) 
            {
                for (Pullenti.Ner.Token tt = te.Next; tt != null; tt = tt.Next) 
                {
                    if (tt.IsNewlineBefore) 
                        break;
                    if (Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(tt)) 
                        break;
                    if (tt.LengthChar == 1 || tt.IsValue("ПЕРИОД", null)) 
                        continue;
                    if (tt.GetReferent() is Pullenti.Ner.Date.DateReferent) 
                    {
                        dt = tt.GetReferent() as Pullenti.Ner.Date.DateReferent;
                        t1 = tt;
                        break;
                    }
                    else if (tt.GetReferent() is Pullenti.Ner.Date.DateRangeReferent) 
                    {
                        dr = tt.GetReferent() as Pullenti.Ner.Date.DateRangeReferent;
                        t1 = tt;
                        break;
                    }
                }
            }
            if (dr != null) 
            {
                li.DateFrom = dr.DateFrom;
                li.DateTo = dr.DateTo;
            }
            else if (dt != null) 
                li.DateFrom = (li.DateTo = dt);
            return new Pullenti.Ner.ReferentToken(li, t0, t1);
        }
        static void _corrAddrTyp(LinkReferent li, Pullenti.Ner.Token t)
        {
            bool isOrg = li.Object1 is Pullenti.Ner.Org.OrganizationReferent;
            string typ = _checkAddrTyp((t as Pullenti.Ner.ReferentToken).BeginToken, isOrg);
            if (typ != null) 
            {
                li.Param = typ;
                return;
            }
            for (Pullenti.Ner.Token tt = t.Previous; tt != null; tt = tt.Previous) 
            {
                typ = _checkAddrTyp(tt, isOrg);
                if (typ != null) 
                {
                    li.Param = typ;
                    return;
                }
                if (tt.IsNewlineBefore || Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(tt) || tt.IsComma) 
                    break;
            }
        }
        static string _checkAddrTyp(Pullenti.Ner.Token t, bool isOrg)
        {
            Pullenti.Ner.TextToken tt = t as Pullenti.Ner.TextToken;
            if (tt == null) 
                return null;
            if (tt.IsValue("АДРЕС", null)) 
                return _checkAddrTyp(t.Next, isOrg);
            if (tt.Term.StartsWith("ЮР") && isOrg) 
                return "юридический";
            if (tt.Term.StartsWith("ФАКТ") && isOrg) 
                return "фактический";
            if (!isOrg) 
            {
                if (tt.Term.StartsWith("ЗАРЕГ") || tt.Term.StartsWith("РЕГИСТР") || tt.Term.StartsWith("ПРОПИС")) 
                    return "регистрация";
                if (tt.Term.StartsWith("ФАКТИЧ") || tt.Term.StartsWith("ПРОЖИВ")) 
                    return "проживание";
            }
            return null;
        }
        static bool m_Initialized = false;
        static object m_Lock = new object();
        public static void Initialize()
        {
            lock (m_Lock) 
            {
                if (m_Initialized) 
                    return;
                m_Initialized = true;
                MetaLink.Initialize();
                Pullenti.Ner.ProcessorService.RegisterAnalyzer(new LinkAnalyzer());
            }
        }
    }
}