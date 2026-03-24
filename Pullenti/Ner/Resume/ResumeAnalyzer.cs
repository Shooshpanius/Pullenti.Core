/*
 * SDK Pullenti Lingvo, version 4.33, fabruary 2026. Copyright (c) 2013-2026, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Ner.Resume
{
    /// <summary>
    /// Анализатор резюме (специфический анализатор)
    /// </summary>
    public class ResumeAnalyzer : Pullenti.Ner.Analyzer
    {
        /// <summary>
        /// Имя анализатора ("RESUME")
        /// </summary>
        public const string ANALYZER_NAME = "RESUME";
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
                return "Резюме";
            }
        }
        public override string Description
        {
            get
            {
                return "Текст содержит одно резюме";
            }
        }
        public override Pullenti.Ner.Analyzer Clone()
        {
            return new ResumeAnalyzer();
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
                return new Pullenti.Ner.Metadata.ReferentClass[] {MetaResume.GlobalMeta};
            }
        }
        public override Dictionary<string, byte[]> Images
        {
            get
            {
                Dictionary<string, byte[]> res = new Dictionary<string, byte[]>();
                res.Add(MetaResume.ImageId.ToString(), Pullenti.Ner.Core.Internal.ResourceHelper3.GetBytes("resume.png"));
                return res;
            }
        }
        public override Pullenti.Ner.Referent CreateReferent(string type)
        {
            if (type == ResumeItemReferent.OBJ_TYPENAME) 
                return new ResumeItemReferent();
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
            bool hasSex = false;
            bool hasMoney = false;
            bool hasPos = false;
            bool hasSpec = false;
            bool hasSkills = false;
            bool hasExp = false;
            bool hasEdu = false;
            bool hasAbout = false;
            Pullenti.Ner.ReferentToken rt;
            Pullenti.Ner.Person.PersonReferent pers0 = null;
            int cou = 0;
            for (Pullenti.Ner.Token t = kit.FirstToken; t != null; t = t.Next) 
            {
                Pullenti.Ner.Token tt = Pullenti.Ner.Core.MiscHelper.CheckImage(t);
                if (tt != null) 
                {
                    t = tt;
                    continue;
                }
                Pullenti.Ner.Person.PersonReferent pers = t.GetReferent() as Pullenti.Ner.Person.PersonReferent;
                if (pers != null) 
                {
                    if (!t.IsNewlineBefore && cou > 10) 
                        break;
                    if (cou > 10 && (t as Pullenti.Ner.ReferentToken).BeginToken.IsValue("ИП", null)) 
                        break;
                    rt = new Pullenti.Ner.ReferentToken(ad.RegisterReferent(new ResumeItemReferent() { Typ = ResumeItemType.Person, Ref = pers }), t, t);
                    kit.EmbedToken(rt);
                    pers0 = pers;
                    break;
                }
                if (t.LengthChar > 1) 
                    cou++;
            }
            bool opyt = false;
            bool hasEduOrg = false;
            ResumeItemType curTyp = ResumeItemType.Undefined;
            for (Pullenti.Ner.Token t = kit.FirstToken; t != null; t = (t == null ? null : t.Next)) 
            {
                if (pers0 != null) 
                {
                    if ((t.GetReferent() is Pullenti.Ner.Uri.UriReferent) || (t.GetReferent() is Pullenti.Ner.Phone.PhoneReferent) || (t.GetReferent() is Pullenti.Ner.Address.AddressReferent)) 
                    {
                        Pullenti.Ner.Uri.UriReferent uri = t.GetReferent() as Pullenti.Ner.Uri.UriReferent;
                        if (uri != null) 
                        {
                            if (uri.Scheme != "mailto" && uri.Scheme != "telegram") 
                                continue;
                            if (uri.Value.Contains("rating@mail.ru")) 
                                continue;
                        }
                        rt = new Pullenti.Ner.ReferentToken(ad.RegisterReferent(new ResumeItemReferent() { Typ = ResumeItemType.Contact, Ref = t.GetReferent() }), t, t);
                        kit.EmbedToken(rt);
                        continue;
                    }
                }
                if (pers0 != null && !opyt) 
                {
                    if ((t.GetReferent() is Pullenti.Ner.Geo.GeoReferent) || (t.GetReferent() is Pullenti.Ner.Address.StreetReferent)) 
                    {
                        Pullenti.Ner.Geo.GeoReferent geo = t.GetReferent() as Pullenti.Ner.Geo.GeoReferent;
                        if (geo != null && !geo.IsCity) 
                            continue;
                        rt = new Pullenti.Ner.ReferentToken(ad.RegisterReferent(new ResumeItemReferent() { Typ = ResumeItemType.Contact, Ref = t.GetReferent() }), t, t);
                        kit.EmbedToken(rt);
                        continue;
                    }
                }
                if (t.IsValue("КОНТАКТЫ", null)) 
                {
                }
                if (t.IsValue2("ДАТА", "РОЖДЕНИЯ") || t.IsValue("РОДИТЬСЯ", null)) 
                {
                    for (Pullenti.Ner.Token tt = t.Next; tt != null; tt = tt.Next) 
                    {
                        Pullenti.Ner.Referent r = tt.GetReferent();
                        if (r is Pullenti.Ner.Date.DateReferent) 
                        {
                            if (pers0 != null) 
                                pers0.AddSlot(Pullenti.Ner.Person.PersonReferent.ATTR_BORN, r, false, 0);
                            else 
                            {
                            }
                            break;
                        }
                        if (tt.IsValue("РОЖДЕНИЕ", null)) 
                            continue;
                        if (tt.GetReferent() is Pullenti.Ner.Geo.GeoReferent) 
                            continue;
                        if (tt.LengthChar > 1) 
                            break;
                    }
                    continue;
                }
                if (t.GetReferent() is Pullenti.Ner.Person.PersonIdentityReferent) 
                {
                    rt = new Pullenti.Ner.ReferentToken(ad.RegisterReferent(new ResumeItemReferent() { Typ = ResumeItemType.Document, Ref = t.GetReferent() }), t, t);
                    kit.EmbedToken(rt);
                    t = rt;
                    continue;
                }
                if (!t.IsNewlineBefore) 
                    continue;
                if (t.IsTableControlChar && t.Next != null) 
                    t = t.Next;
                if (t.IsValue("ОБРАЗОВАНИЕ", null)) 
                    curTyp = ResumeItemType.Study;
                if (t.IsValue2("ОПЫТ", "РАБОТЫ") || t.IsValue2("МЕСТО", "РАБОТЫ") || t.IsValue2("ПЕРИОД", "РАБОТЫ")) 
                {
                    opyt = true;
                    curTyp = ResumeItemType.Organization;
                }
                rt = _parseOrg(t, curTyp);
                if (rt != null) 
                {
                    rt.Referent = ad.RegisterReferent(rt.Referent);
                    rt.Referent.Tag = rt;
                    kit.EmbedToken(rt);
                    curTyp = (rt.Referent as ResumeItemReferent).Typ;
                    if (curTyp == ResumeItemType.Study) 
                        hasEduOrg = true;
                    t = rt;
                    continue;
                }
                rt = _parseOrg2(t, ad, null);
                if (rt != null) 
                {
                    t = rt;
                    curTyp = ResumeItemType.Organization;
                    continue;
                }
                if (!hasSex) 
                {
                    rt = _parseSex(t, ad);
                    if (rt != null) 
                    {
                        hasSex = true;
                        t = rt;
                        continue;
                    }
                }
                if (_checkGeo(t)) 
                    continue;
                if (!hasMoney && (t.GetReferent() is Pullenti.Ner.Money.MoneyReferent)) 
                {
                    ResumeItemReferent money = new ResumeItemReferent();
                    hasMoney = true;
                    money.Typ = ResumeItemType.Money;
                    money.Ref = t.GetReferent();
                    rt = new Pullenti.Ner.ReferentToken(ad.RegisterReferent(money), t, t);
                    kit.EmbedToken(rt);
                    t = rt;
                    continue;
                }
                if (!hasExp) 
                {
                    rt = _parseExperience(t, ad);
                    if (rt != null) 
                    {
                        hasExp = true;
                        t = rt;
                        continue;
                    }
                }
                if (!hasSpec && t.IsValue("СПЕЦИАЛИЗАЦИЯ", null)) 
                {
                    if (t.Next != null && t.Next.IsChar(':')) 
                        t = t.Next;
                    rt = _parseList(t.Next, ad, ResumeItemType.Speciality);
                    if (rt != null) 
                    {
                        hasSpec = true;
                        t = rt;
                        continue;
                    }
                }
                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt != null && ((npt.EndToken.IsValue("НАВЫК", null) || npt.EndToken.IsValue("КОМПЕТЕНЦИЯ", null)))) 
                {
                    Pullenti.Ner.Token tt = npt.EndToken.Next;
                    if (tt != null && tt.IsValue2("И", "ДОСТИЖЕНИЕ")) 
                        tt = tt.Next.Next;
                    rt = _parseList(tt, ad, ResumeItemType.Skill);
                    if (rt != null) 
                    {
                        hasSkills = true;
                        t = rt;
                        continue;
                    }
                }
                if (!hasAbout && (((t.IsValue2("О", "МНЕ") || t.IsValue2("О", "СЕБЕ") || t.IsValue2("ЛИЧНЫЕ", "КАЧЕСТВА")) || t.IsValue2("ДОПОЛНИТЕЛЬНЫЕ", "СВЕДЕНИЯ")))) 
                {
                    rt = _parseAboutMe(t.Next.Next, ad);
                    if (rt != null) 
                    {
                        hasAbout = true;
                        t = rt;
                        continue;
                    }
                }
                if (!hasSpec && hasSex && !hasPos) 
                {
                    rt = _parseList(t, ad, ResumeItemType.Position);
                    if (rt != null) 
                    {
                        hasPos = true;
                        t = rt;
                        continue;
                    }
                }
                if (!hasEdu) 
                {
                    Pullenti.Ner.MetaToken mt = _parseEducation(t);
                    if (mt != null) 
                    {
                        curTyp = ResumeItemType.Study;
                        ResumeItemReferent edu = new ResumeItemReferent();
                        hasEdu = true;
                        edu.Typ = ResumeItemType.Education;
                        edu.Value = mt.Tag as string;
                        rt = new Pullenti.Ner.ReferentToken(ad.RegisterReferent(edu), mt.BeginToken, mt.EndToken);
                        kit.EmbedToken(rt);
                        t = rt;
                        continue;
                    }
                }
                rt = _parseDriving(t, ad);
                if (rt != null) 
                {
                    t = rt;
                    continue;
                }
            }
            if (!hasEduOrg) 
            {
                for (Pullenti.Ner.Token t = kit.FirstToken; t != null; t = t.Next) 
                {
                    Pullenti.Ner.Org.OrganizationReferent org = t.GetReferent() as Pullenti.Ner.Org.OrganizationReferent;
                    if (org == null) 
                        continue;
                    if (org.Profiles.Contains(Pullenti.Ner.Org.OrgProfile.Education) && !(org.Tag is ResumeItemReferent)) 
                    {
                        Pullenti.Ner.Referent it = ad.RegisterReferent(new ResumeItemReferent() { Typ = ResumeItemType.Study, Ref = org });
                        rt = new Pullenti.Ner.ReferentToken(it, t, t);
                        kit.EmbedToken(rt);
                        break;
                    }
                }
            }
        }
        static Pullenti.Ner.ReferentToken _parseSex(Pullenti.Ner.Token t, Pullenti.Ner.Core.AnalyzerData ad)
        {
            if (t.IsValue2("ПОЛНЫХ", "ЛЕТ")) 
            {
                t = t.Next.Next;
                if (t != null && t.IsCharOf(":-")) 
                    t = t.Next;
                if (t is Pullenti.Ner.NumberToken) 
                {
                    ResumeItemReferent age = new ResumeItemReferent();
                    age.Typ = ResumeItemType.Age;
                    age.Value = (t as Pullenti.Ner.NumberToken).Value;
                    Pullenti.Ner.ReferentToken rt0 = new Pullenti.Ner.ReferentToken(ad.RegisterReferent(age), t, t);
                    t.Kit.EmbedToken(rt0);
                    return rt0;
                }
            }
            if (!t.IsValue("МУЖЧИНА", null) && !t.IsValue("ЖЕНЩИНА", null)) 
                return null;
            ResumeItemReferent sex = new ResumeItemReferent();
            sex.Typ = ResumeItemType.Sex;
            sex.Value = (t.IsValue("МУЖЧИНА", null) ? "муж" : "жен");
            Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(ad.RegisterReferent(sex), t, t);
            t.Kit.EmbedToken(rt);
            t = rt;
            for (Pullenti.Ner.Token tt = t.Next; tt != null; tt = tt.Next) 
            {
                if (tt.IsNewlineBefore) 
                    break;
                if ((tt is Pullenti.Ner.NumberToken) && tt.Next != null) 
                {
                    if (tt.Next.IsValue("ГОД", null) || tt.Next.IsValue("ЛЕТ", null)) 
                    {
                        ResumeItemReferent age = new ResumeItemReferent();
                        age.Typ = ResumeItemType.Age;
                        age.Value = (tt as Pullenti.Ner.NumberToken).Value;
                        rt = new Pullenti.Ner.ReferentToken(ad.RegisterReferent(age), tt, tt.Next);
                        t.Kit.EmbedToken(rt);
                        t = rt;
                        break;
                    }
                }
            }
            return rt;
        }
        static Pullenti.Ner.ReferentToken _parseExperience(Pullenti.Ner.Token t, Pullenti.Ner.Core.AnalyzerData ad)
        {
            if (!t.IsValue2("ОПЫТ", "РАБОТЫ")) 
                return null;
            for (Pullenti.Ner.Token tt = t.Next; tt != null; tt = tt.Next) 
            {
                if (tt.IsNewlineBefore && !tt.Previous.IsCharOf(":-")) 
                    break;
                if ((tt is Pullenti.Ner.NumberToken) && tt.Next != null) 
                {
                    if (tt.Next.IsValue("ГОД", null) || tt.Next.IsValue("ЛЕТ", null) || tt.Next.IsValue("МЕСЯЦ", null)) 
                    {
                        ResumeItemReferent experience = new ResumeItemReferent();
                        experience.Typ = ResumeItemType.Experience;
                        experience.Value = (tt as Pullenti.Ner.NumberToken).Value;
                        if (tt.Next.IsValue("МЕСЯЦ", null)) 
                            experience.Value = Pullenti.Ner.Core.NumberHelper.DoubleToString(Math.Round((tt as Pullenti.Ner.NumberToken).RealValue / 12, 1));
                        Pullenti.Ner.Token tt1 = tt.Next;
                        if ((tt1.Next is Pullenti.Ner.NumberToken) && tt1.Next.Next != null && tt1.Next.Next.IsValue("МЕСЯЦ", null)) 
                        {
                            double d = Math.Round((tt as Pullenti.Ner.NumberToken).RealValue + (((tt1.Next as Pullenti.Ner.NumberToken).RealValue / 12)), 1);
                            experience.Value = Pullenti.Ner.Core.NumberHelper.DoubleToString(d);
                            tt1 = tt1.Next.Next;
                        }
                        Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(ad.RegisterReferent(experience), tt, tt1);
                        t.Kit.EmbedToken(rt);
                        return rt;
                    }
                }
            }
            return null;
        }
        static Pullenti.Ner.MetaToken _parseEducation(Pullenti.Ner.Token t)
        {
            bool hi = false;
            bool middl = false;
            bool prof = false;
            bool spec = false;
            bool tech = false;
            bool neok = false;
            bool keyword = false;
            Pullenti.Ner.Token t0 = t;
            Pullenti.Ner.Token t1 = t;
            for (; t != null; t = t.Next) 
            {
                if (t0 != t && t.IsNewlineBefore) 
                {
                    if (t.Previous != null && t.Previous.IsCharOf(":-")) 
                    {
                    }
                    else 
                        break;
                }
                if (t.IsValue("СРЕДНИЙ", null) || t.IsValue("СРЕДНЕ", null) || t.IsValue("СРЕДН", null)) 
                    middl = true;
                else if (t.IsValue("ВЫСШИЙ", null) || t.IsValue("ВЫСШ", null)) 
                    hi = true;
                else if (t.IsValue("НЕОКОНЧЕННЫЙ", null)) 
                    neok = true;
                else if (t.IsValue("ПРОФЕССИОНАЛЬНЫЙ", null) || t.IsValue("ПРОФ", null) || t.IsValue("ПРОФИЛЬНЫЙ", null)) 
                    prof = true;
                else if ((t.IsValue("СПЕЦИАЛЬНЫЙ", null) || t.IsValue("СПЕЦ", null) || t.IsValue2("ПО", "СПЕЦИАЛЬНОСТЬ")) || t.IsValue2("ПО", "НАПРАВЛЕНИЕ")) 
                    spec = true;
                else if ((t.IsValue("ТЕХНИЧЕСКИЙ", null) || t.IsValue("ТЕХ", null) || t.IsValue("ТЕХН", null)) || t.IsValue("ТЕХНИЧ", null)) 
                    tech = true;
                else if (t.IsValue("ОБРАЗОВАНИЕ", null)) 
                {
                    keyword = true;
                    t1 = t;
                }
                else if (t.LengthChar > 1) 
                    break;
            }
            if (!keyword) 
                return null;
            if (!hi && !middl) 
            {
                if ((spec || prof || tech) || neok) 
                    middl = true;
                else 
                    return null;
            }
            string val = (hi ? "ВО" : "СО");
            if (spec) 
                val += ",спец";
            if (prof) 
                val += ",проф";
            if (tech) 
                val += ",тех";
            if (neok) 
                val += ",неоконч";
            return new Pullenti.Ner.MetaToken(t0, t1) { Tag = val };
        }
        static Pullenti.Ner.MetaToken _parseMoral(Pullenti.Ner.Token t)
        {
            Pullenti.Ner.Core.TerminToken tok = Pullenti.Ner.Vacance.Internal.VacanceToken.m_Termins.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
            if (tok == null || tok.Termin.Tag2 != null) 
                return null;
            Pullenti.Ner.Vacance.Internal.VacanceTokenType ty = (Pullenti.Ner.Vacance.Internal.VacanceTokenType)tok.Termin.Tag;
            if (ty != Pullenti.Ner.Vacance.Internal.VacanceTokenType.Moral) 
                return null;
            string val = string.Format("{0}{1}", tok.Termin.CanonicText[0], tok.Termin.CanonicText.Substring(1).ToLower());
            Pullenti.Ner.Token t1 = tok.EndToken;
            for (Pullenti.Ner.Token tt = tok.EndToken.Next; tt != null; tt = tt.Next) 
            {
                if (tt.WhitespacesBeforeCount > 2) 
                    break;
                if (Pullenti.Ner.Vacance.Internal.VacanceToken.m_Termins.TryParse(tt, Pullenti.Ner.Core.TerminParseAttr.No) != null) 
                    break;
                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt, Pullenti.Ner.Core.NounPhraseParseAttr.ParsePreposition | Pullenti.Ner.Core.NounPhraseParseAttr.ParsePronouns, 0, null);
                if (npt == null) 
                    break;
                tt = (t1 = npt.EndToken);
            }
            if (t1.EndChar > tok.EndChar) 
                val = string.Format("{0} {1}", val, Pullenti.Ner.Core.MiscHelper.GetTextValue(tok.EndToken.Next, t1, Pullenti.Ner.Core.GetTextAttr.KeepQuotes | Pullenti.Ner.Core.GetTextAttr.KeepRegister));
            return new Pullenti.Ner.MetaToken(t, t1) { Tag = val };
        }
        static Pullenti.Ner.ReferentToken _parseDriving(Pullenti.Ner.Token t, Pullenti.Ner.Core.AnalyzerData ad)
        {
            if (t == null) 
                return null;
            Pullenti.Ner.Token t1 = null;
            if (t.IsValue("НАЛИЧИЕ", null) && t.Next != null) 
                t = t.Next;
            if ((t.IsValue2("ВОДИТЕЛЬСКИЕ", "ПРАВА") || t.IsValue2("ПРАВА", "КАТЕГОРИИ") || t.IsValue2("ВОДИТЕЛЬСКОЕ", "УДОСТОВЕРЕНИЕ")) || t.IsValue2("УДОСТОВЕРЕНИЕ", "ВОДИТЕЛЯ") || t.IsValue2("ПРАВА", "ВОДИТЕЛЯ")) 
                t1 = t.Next.Next;
            if (t1 == null) 
                return null;
            Pullenti.Ner.Token t0 = t;
            string val = null;
            for (t = t1; t != null; t = t.Next) 
            {
                if ((t.IsHiphen || t.IsTableControlChar || t.IsCharOf(":.")) || t.IsValue("КАТЕГОРИЯ", null) || t.IsValue("КАТ", null)) 
                    continue;
                if ((t is Pullenti.Ner.TextToken) && t.LengthChar <= 3 && t.Chars.IsLetter) 
                {
                    val = (t as Pullenti.Ner.TextToken).Term;
                    t1 = t;
                    for (t = t.Next; t != null; t = t.Next) 
                    {
                        if (t.WhitespacesBeforeCount > 2) 
                            break;
                        else if (t.IsChar('.') || t.IsCommaAnd) 
                            continue;
                        else if (t.LengthChar == 1 && t.Chars.IsAllUpper && t.Chars.IsLetter) 
                        {
                            val = string.Format("{0}{1}", val, (t as Pullenti.Ner.TextToken).Term);
                            t1 = t;
                        }
                        else 
                            break;
                    }
                    val = val.Replace("А", "A").Replace("В", "B").Replace("С", "C");
                    break;
                }
                break;
            }
            if (val == null) 
                return null;
            ResumeItemReferent drv = new ResumeItemReferent();
            drv.Typ = ResumeItemType.DrivingLicense;
            drv.Value = val;
            Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(ad.RegisterReferent(drv), t0, t1);
            t0.Kit.EmbedToken(rt);
            return rt;
        }
        static Pullenti.Ner.MetaToken _parseOnto(Pullenti.Ner.Token t)
        {
            if (t == null) 
                return null;
            if (t.Kit.Ontology == null) 
                return null;
            List<Pullenti.Ner.Core.IntOntologyToken> lii = t.Kit.Ontology.AttachToken(ANALYZER_NAME, t);
            if (lii == null || lii.Count == 0) 
                return null;
            if (!(lii[0].Item.Referent is ResumeItemReferent)) 
                return null;
            string val = (lii[0].Item.Referent as ResumeItemReferent).Value;
            val = string.Format("{0}{1}", val[0], val.Substring(1).ToLower());
            return new Pullenti.Ner.MetaToken(t, lii[0].EndToken) { Tag = val };
        }
        public static bool _startOfItem(Pullenti.Ner.Token t)
        {
            if (t == null) 
                return false;
            if (t.IsTableControlChar && t.Next != null) 
                t = t.Next;
            if (t.IsNewlineBefore) 
            {
            }
            else if (t.Previous != null && t.Previous.IsTableControlChar && t.Previous.IsNewlineBefore) 
            {
            }
            else 
                return false;
            if (t.IsValue2("О", "МНЕ") || t.IsValue2("О", "СЕБЕ") || t.IsValue2("ДОПОЛНИТЕЛЬНЫЕ", "СВЕДЕНИЯ")) 
                return true;
            if (t.IsValue("ИНТЕРЕСНЫ", null)) 
                return true;
            Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
            if (npt != null) 
            {
                Pullenti.Ner.Token tt = npt.EndToken;
                if ((((tt.IsValue("КОНТАКТ", null) || tt.IsValue("ТЕЛЕФОН", null) || tt.IsValue("РЕЗЮМЕ", null)) || tt.IsValue("ГРАФИК", null) || tt.IsValue("ЗАНЯТОСТЬ", null)) || tt.IsValue("ОПЫТ", null) || tt.IsValue("ОБРАЗОВАНИЕ", null)) || tt.IsValue("ИНФОРМАЦИЯ", null)) 
                    return true;
                if (tt.IsValue("НАВЫК", null)) 
                {
                    if (t.IsValue("КЛЮЧЕВОЙ", null) || t.IsValue("ПРОФЕССИОНАЛЬНЫЙ", null)) 
                        return true;
                }
                if (tt.IsValue("КОМПЕТЕНЦИЯ", null)) 
                    return true;
            }
            return false;
        }
        static Pullenti.Ner.ReferentToken _parseList(Pullenti.Ner.Token t, Pullenti.Ner.Core.AnalyzerData ad, ResumeItemType typ)
        {
            Pullenti.Ner.ReferentToken rt = null;
            ResumeItemReferent spec;
            for (; t != null; t = t.Next) 
            {
                if (!t.IsChar(':') && !t.IsTableControlChar) 
                    break;
            }
            if (t == null) 
                return null;
            Pullenti.Ner.Token t0 = t;
            for (; t != null; t = t.Next) 
            {
                if (t.IsNewlineBefore) 
                {
                    if (t.NewlinesBeforeCount > 1 && t != t0) 
                        break;
                    if (_startOfItem(t)) 
                        break;
                    if (t == t0 && typ == ResumeItemType.Position) 
                    {
                    }
                    else if (typ == ResumeItemType.Skill || typ == ResumeItemType.Speciality) 
                    {
                    }
                    else 
                        break;
                }
                if (t.IsCharOf(";,")) 
                    continue;
                if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(t, true, false)) 
                {
                    Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(t, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                    if (br != null) 
                    {
                        spec = new ResumeItemReferent();
                        spec.Typ = typ;
                        spec.Value = _corrValue(Pullenti.Ner.Core.MiscHelper.GetTextValue(t.Next, br.EndToken.Previous, Pullenti.Ner.Core.GetTextAttr.KeepQuotes | Pullenti.Ner.Core.GetTextAttr.KeepRegister));
                        rt = new Pullenti.Ner.ReferentToken(ad.RegisterReferent(spec), t, br.EndToken);
                        t.Kit.EmbedToken(rt);
                        t = rt;
                        continue;
                    }
                }
                if (typ == ResumeItemType.Skill) 
                {
                    Pullenti.Ner.Core.TerminToken tok = Pullenti.Ner.Vacance.Internal.VacanceToken.m_SkillItems.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
                    if (tok != null) 
                    {
                        spec = new ResumeItemReferent();
                        spec.Typ = typ;
                        spec.Value = tok.Termin.CanonicText;
                        rt = new Pullenti.Ner.ReferentToken(ad.RegisterReferent(spec), t, tok.EndToken);
                        t.Kit.EmbedToken(rt);
                        t = rt;
                        continue;
                    }
                }
                Pullenti.Ner.Token t1 = t;
                for (Pullenti.Ner.Token tt = t.Next; tt != null; tt = tt.Next) 
                {
                    if (tt.IsNewlineBefore) 
                    {
                        if (tt.Previous.IsHiphen) 
                            continue;
                        break;
                    }
                    if (tt.IsCharOf(";,") || Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(tt)) 
                        break;
                    if (tt.IsChar('(')) 
                    {
                        Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(tt, Pullenti.Ner.Core.BracketParseAttr.No, 100);
                        if (br != null) 
                            tt = br.EndToken;
                    }
                    t1 = tt;
                }
                if (t1 == null) 
                    break;
                if (t1.Next == null) 
                    return null;
                if (typ == ResumeItemType.Position) 
                {
                    bool ok = false;
                    for (Pullenti.Ner.Token tt = t1.Next; tt != null; tt = tt.Next) 
                    {
                        if (tt.IsNewlineBefore) 
                        {
                            if (tt.GetReferent() is Pullenti.Ner.Money.MoneyReferent) 
                                ok = true;
                            else if (tt.IsValue("СПЕЦИАЛИЗАЦИЯ", null)) 
                                ok = true;
                            break;
                        }
                    }
                    if (!ok) 
                        return null;
                }
                Pullenti.Ner.ReferentToken rt1 = _parseDriving(t, ad);
                if (rt1 != null) 
                {
                    t = rt1;
                    rt = rt1;
                    continue;
                }
                Pullenti.Ner.MetaToken mt = _parseMoral(t);
                if (mt != null) 
                {
                    ResumeItemReferent mor = new ResumeItemReferent();
                    mor.Typ = ResumeItemType.Moral;
                    mor.Value = _corrValue(mt.Tag as string);
                    rt = new Pullenti.Ner.ReferentToken(ad.RegisterReferent(mor), t, mt.EndToken);
                }
                else 
                {
                    spec = new ResumeItemReferent();
                    spec.Typ = typ;
                    if ((t.LengthChar == 1 && (t is Pullenti.Ner.TextToken) && !t.Chars.IsLetter) && (t.EndChar < t1.BeginChar)) 
                        t = t.Next;
                    for (; t != null; t = t.Next) 
                    {
                        if (t.IsValue("НАВЫК", null) || t.IsTableControlChar || ((t.LengthChar == 1 && (t is Pullenti.Ner.TextToken) && !t.Chars.IsLetter))) 
                        {
                        }
                        else 
                            break;
                    }
                    if (t == null) 
                        break;
                    if (t.EndChar > t1.EndChar) 
                    {
                        t = t1;
                        continue;
                    }
                    spec.Value = _corrValue(Pullenti.Ner.Core.MiscHelper.GetTextValue(t, t1, Pullenti.Ner.Core.GetTextAttr.KeepQuotes | Pullenti.Ner.Core.GetTextAttr.KeepRegister));
                    if (spec.Value == "другое" || spec.Value == "языки" || spec.Value == "курсы") 
                    {
                        t = t1;
                        continue;
                    }
                    for (Pullenti.Ner.Token tt = t; tt != null && (tt.EndChar < t1.EndChar); tt = tt.Next) 
                    {
                        Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(tt, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                        if (npt != null && npt.Adjectives.Count > 0) 
                        {
                            if (npt.EndToken.IsValue("УРОВЕНЬ", null) || npt.EndToken.IsValue("НАВЫК", null) || npt.EndToken.IsValue("ЗНАНИЕ", null)) 
                            {
                                if (tt != t) 
                                {
                                    spec.Misc = npt.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false).ToLower();
                                    if (tt.Previous.IsCharOf("-(")) 
                                        tt = tt.Previous;
                                    if (tt.Previous.BeginChar >= t.BeginChar) 
                                    {
                                        Pullenti.Ner.Core.TerminToken tok = Pullenti.Ner.Vacance.Internal.VacanceToken.m_SkillItems.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
                                        if (tok != null) 
                                            spec.Value = tok.Termin.CanonicText;
                                        else 
                                            spec.Value = _corrValue(Pullenti.Ner.Core.MiscHelper.GetTextValue(t, tt.Previous, Pullenti.Ner.Core.GetTextAttr.KeepQuotes | Pullenti.Ner.Core.GetTextAttr.KeepRegister));
                                    }
                                }
                                else if (npt.EndChar < t1.EndChar) 
                                {
                                    spec.Misc = npt.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false).ToLower();
                                    Pullenti.Ner.Core.TerminToken tok = Pullenti.Ner.Vacance.Internal.VacanceToken.m_SkillItems.TryParse(npt.EndToken.Next, Pullenti.Ner.Core.TerminParseAttr.No);
                                    if (tok != null) 
                                        spec.Value = tok.Termin.CanonicText;
                                    else 
                                        spec.Value = _corrValue(Pullenti.Ner.Core.MiscHelper.GetTextValue(npt.EndToken.Next, t1, Pullenti.Ner.Core.GetTextAttr.FirstNounGroupToNominative | Pullenti.Ner.Core.GetTextAttr.KeepQuotes | Pullenti.Ner.Core.GetTextAttr.KeepRegister));
                                }
                                break;
                            }
                        }
                    }
                    if (string.IsNullOrEmpty(spec.Value)) 
                    {
                        t = t1;
                        continue;
                    }
                    rt = new Pullenti.Ner.ReferentToken(ad.RegisterReferent(spec), t, t1);
                }
                t.Kit.EmbedToken(rt);
                t = rt;
            }
            return rt;
        }
        static string _corrValue(string val)
        {
            if (string.IsNullOrEmpty(val) || (val.Length < 3)) 
                return val;
            val = val.Replace(" - ", "-");
            if (val.EndsWith(".")) 
                val = val.Substring(0, val.Length - 1);
            if (char.IsUpper(val[0]) && char.IsLower(val[1]) && ((int)val[0]) > 0x100) 
                val = string.Format("{0}{1}", char.ToLower(val[0]), val.Substring(1));
            return val;
        }
        static Pullenti.Ner.ReferentToken _parseAboutMe(Pullenti.Ner.Token t, Pullenti.Ner.Core.AnalyzerData ad)
        {
            Pullenti.Ner.Token t0 = t;
            Pullenti.Ner.ReferentToken rt = null;
            for (; t != null; t = t.Next) 
            {
                if (t.IsNewlineBefore) 
                {
                    if (_parseEducation(t) != null) 
                        break;
                    if (_startOfItem(t)) 
                        break;
                }
                Pullenti.Ner.MetaToken mt = _parseMoral(t);
                if (mt != null) 
                {
                    ResumeItemReferent mor = new ResumeItemReferent();
                    mor.Typ = ResumeItemType.Moral;
                    mor.Value = _corrValue(mt.Tag as string);
                    rt = new Pullenti.Ner.ReferentToken(ad.RegisterReferent(mor), t, mt.EndToken);
                    t.Kit.EmbedToken(rt);
                    t = rt;
                    continue;
                }
                mt = _parseOnto(t);
                if (mt != null) 
                {
                    ResumeItemReferent mor = new ResumeItemReferent();
                    mor.Typ = ResumeItemType.Skill;
                    mor.Value = _corrValue(mt.Tag as string);
                    rt = new Pullenti.Ner.ReferentToken(ad.RegisterReferent(mor), t, mt.EndToken);
                    t.Kit.EmbedToken(rt);
                    t = rt;
                    continue;
                }
            }
            return rt;
        }
        public static Pullenti.Ner.ReferentToken _parseOrg(Pullenti.Ner.Token t, ResumeItemType typ)
        {
            Pullenti.Ner.Token t0 = t;
            if (t == null) 
                return null;
            for (; t != null; t = t.Next) 
            {
                if (((t.IsCharOf(":-") || t.IsTableControlChar || t.IsValue("ПЕРИОД", null)) || t.IsValue("РАБОТА", null) || t.IsValue("ВРЕМЯ", null)) || t.IsValue("МЕСТО", null)) 
                {
                }
                else if (((t is Pullenti.Ner.NumberToken) && t.IsNewlineBefore && t.Next != null) && t.Next.IsCharOf(".)")) 
                    t = t.Next;
                else 
                    break;
            }
            if (t == null) 
                return null;
            Pullenti.Ner.Org.OrganizationReferent org = null;
            Pullenti.Ner.Person.PersonPropertyReferent post = null;
            Pullenti.Ner.Person.Internal.PersonAttrToken postAttr = null;
            Pullenti.Ner.Referent dt = t.GetReferent();
            Pullenti.Ner.Token t1 = null;
            if ((dt is Pullenti.Ner.Date.DateReferent) || (dt is Pullenti.Ner.Date.DateRangeReferent)) 
            {
            }
            else if ((dt is Pullenti.Ner.Org.OrganizationReferent) && t0.IsNewlineBefore && t.Next != null) 
            {
                org = dt as Pullenti.Ner.Org.OrganizationReferent;
                dt = null;
                t = t.Next;
                if (t.IsChar('(') && t.Next != null) 
                    t = t.Next;
                if ((t.GetReferent() is Pullenti.Ner.Date.DateReferent) || (t.GetReferent() is Pullenti.Ner.Date.DateRangeReferent)) 
                {
                    dt = t.GetReferent();
                    t1 = t;
                    t = t.Next;
                    if (t != null && t.IsChar(')')) 
                    {
                        t1 = t;
                        t = t.Next;
                    }
                }
                Pullenti.Ner.Person.Internal.PersonAttrToken pat = Pullenti.Ner.Person.Internal.PersonAttrToken.TryAttach(t, Pullenti.Ner.Person.Internal.PersonAttrToken.PersonAttrAttachAttrs.No);
                if (pat == null && t != null && (t.GetReferent() is Pullenti.Ner.Person.PersonPropertyReferent)) 
                    pat = new Pullenti.Ner.Person.Internal.PersonAttrToken(t, t) { PropRef = t.GetReferent() as Pullenti.Ner.Person.PersonPropertyReferent };
                if (pat == null || pat.PropRef == null) 
                {
                    if (dt != null && typ != ResumeItemType.Undefined) 
                    {
                    }
                    else 
                        return null;
                }
                else 
                {
                    postAttr = pat;
                    dt = pat.PropRef.GetSlotValue(Pullenti.Ner.Person.PersonPropertyReferent.ATTR_REF) as Pullenti.Ner.Referent;
                    if (dt == null && pat.PropRef.Higher != null) 
                        dt = pat.PropRef.Higher.GetSlotValue(Pullenti.Ner.Person.PersonPropertyReferent.ATTR_REF) as Pullenti.Ner.Referent;
                    t1 = pat.EndToken;
                    if ((dt is Pullenti.Ner.Date.DateReferent) || (dt is Pullenti.Ner.Date.DateRangeReferent)) 
                    {
                        pat.PropRef.AddSlot(Pullenti.Ner.Person.PersonPropertyReferent.ATTR_REF, null, true, 0);
                        if (pat.PropRef.Higher != null) 
                            pat.PropRef.Higher.AddSlot(Pullenti.Ner.Person.PersonPropertyReferent.ATTR_REF, null, true, 0);
                    }
                    else if (dt == null) 
                    {
                        if (pat.EndToken.Next == null) 
                            return null;
                        dt = pat.EndToken.Next.GetReferent();
                        if ((dt is Pullenti.Ner.Date.DateReferent) || (dt is Pullenti.Ner.Date.DateRangeReferent)) 
                        {
                        }
                        else 
                            return null;
                        t1 = pat.EndToken.Next;
                    }
                }
            }
            else 
                return null;
            int cou = 0;
            if (t1 == null) 
            {
                for (t = t.Next; t != null && (cou < 3); t = t.Next) 
                {
                    if (((post != null || postAttr != null)) && org != null) 
                        break;
                    if (t.IsNewlineBefore) 
                        cou++;
                    Pullenti.Ner.Org.OrganizationReferent r = t.GetReferent() as Pullenti.Ner.Org.OrganizationReferent;
                    if (r != null) 
                    {
                        if (org != null) 
                        {
                            if (r.Higher == org || ((r.Higher != null && r.Higher == org))) 
                            {
                            }
                            else 
                                break;
                        }
                        org = r;
                        t1 = t;
                        cou = 0;
                        continue;
                    }
                    Pullenti.Ner.Person.PersonPropertyReferent p = t.GetReferent() as Pullenti.Ner.Person.PersonPropertyReferent;
                    if (p != null && post == null) 
                    {
                        post = p;
                        t1 = t;
                        cou = 0;
                        continue;
                    }
                    if (post == null && postAttr == null) 
                    {
                        if (t.IsValue("ТЕХНИК", null)) 
                        {
                        }
                        Pullenti.Ner.Person.Internal.PersonAttrToken pat = Pullenti.Ner.Person.Internal.PersonAttrToken.TryAttach(t, Pullenti.Ner.Person.Internal.PersonAttrToken.PersonAttrAttachAttrs.No);
                        if (pat != null && pat.PropRef != null) 
                        {
                            postAttr = pat;
                            t = (t1 = pat.EndToken);
                            if (org == null) 
                            {
                                if (pat.HigherPropRef != null) 
                                    org = pat.HigherPropRef.PropRef.GetSlotValue(Pullenti.Ner.Person.PersonPropertyReferent.ATTR_REF) as Pullenti.Ner.Org.OrganizationReferent;
                                else 
                                    org = pat.PropRef.GetSlotValue(Pullenti.Ner.Person.PersonPropertyReferent.ATTR_REF) as Pullenti.Ner.Org.OrganizationReferent;
                            }
                            cou = 0;
                            continue;
                        }
                    }
                    if ((t.GetReferent() is Pullenti.Ner.Date.DateReferent) || (t.GetReferent() is Pullenti.Ner.Date.DateRangeReferent)) 
                        break;
                    if ((t is Pullenti.Ner.NumberToken) && t.IsNewlineBefore) 
                    {
                        if (t.Next != null && t.Next.IsCharOf(".)")) 
                            break;
                        Pullenti.Ner.NumberToken num = t as Pullenti.Ner.NumberToken;
                        if (num.IntValue != null) 
                        {
                            if (num.IntValue.Value >= 2000 && num.IntValue <= 2050) 
                                break;
                        }
                    }
                }
            }
            if (org == null) 
                return null;
            if (typ == ResumeItemType.Undefined && org.Profiles.Contains(Pullenti.Ner.Org.OrgProfile.Education)) 
                typ = ResumeItemType.Study;
            ResumeItemReferent res = new ResumeItemReferent() { Typ = (typ == ResumeItemType.Undefined ? ResumeItemType.Organization : typ) };
            res.DateRange = dt;
            res.Ref = org;
            if (post == null && postAttr != null) 
                post = postAttr.PropRef;
            if (post != null) 
                res.Value = post.ToStringEx(true, null, 0);
            return new Pullenti.Ner.ReferentToken(res, t0, t1);
        }
        public static Pullenti.Ner.ReferentToken _parseOrg2(Pullenti.Ner.Token t, Pullenti.Ner.Core.AnalyzerData ad, List<Pullenti.Ner.ReferentToken> links = null)
        {
            if (t == null) 
                return null;
            if (!t.IsValue2("ОПЫТ", "РАБОТЫ")) 
                return null;
            for (t = t.Next; t != null; t = t.Next) 
            {
                if (t.IsNewlineBefore) 
                    break;
            }
            if (t == null) 
                return null;
            Pullenti.Ner.ReferentToken res = null;
            for (; t != null; t = t.Next) 
            {
                if (_startOfItem(t)) 
                    break;
                if (!t.IsNewlineBefore) 
                    continue;
                Pullenti.Ner.Person.PersonPropertyReferent prop = t.GetReferent() as Pullenti.Ner.Person.PersonPropertyReferent;
                Pullenti.Ner.Org.OrganizationReferent org = null;
                if (prop == null) 
                {
                    Pullenti.Ner.Person.Internal.PersonAttrToken pr = Pullenti.Ner.Person.Internal.PersonAttrToken.TryAttach(t, Pullenti.Ner.Person.Internal.PersonAttrToken.PersonAttrAttachAttrs.No);
                    if (pr != null) 
                    {
                        prop = pr.PropRef;
                        t = pr.EndToken;
                    }
                }
                if (prop == null) 
                {
                    if (res == null) 
                        break;
                    continue;
                }
                Pullenti.Ner.Token t0 = t;
                if (t.Next != null && ((t.Next.IsValue("В", null) || t.Next.IsCharOf("-:,")))) 
                    t = t.Next;
                org = prop.GetSlotValue(Pullenti.Ner.Person.PersonPropertyReferent.ATTR_REF) as Pullenti.Ner.Org.OrganizationReferent;
                if (org == null) 
                    org = t.GetReferent() as Pullenti.Ner.Org.OrganizationReferent;
                if (org == null) 
                    break;
                Pullenti.Ner.ReferentToken dtTok = null;
                for (t = t.Next; t != null; t = t.Next) 
                {
                    if ((t.GetReferent() is Pullenti.Ner.Date.DateReferent) || (t.GetReferent() is Pullenti.Ner.Date.DateRangeReferent)) 
                    {
                        dtTok = t as Pullenti.Ner.ReferentToken;
                        break;
                    }
                    if (t.LengthChar > 1) 
                        break;
                }
                if (dtTok == null) 
                    break;
                ResumeItemReferent it = new ResumeItemReferent() { Typ = ResumeItemType.Organization };
                it.DateRange = dtTok.Referent;
                it.Ref = org;
                it.Value = prop.ToStringEx(true, null, 0);
                org.Tag = it;
                if (links == null) 
                {
                    res = new Pullenti.Ner.ReferentToken(ad.RegisterReferent(it), t0, dtTok);
                    t0.Kit.EmbedToken(res);
                    t = res;
                }
                else 
                {
                    links.Add(new Pullenti.Ner.ReferentToken(it, t0, dtTok));
                    t = dtTok;
                }
            }
            return res;
        }
        static bool _checkGeo(Pullenti.Ner.Token t)
        {
            if (t == null) 
                return false;
            if (t.IsValue2("УКАЗАН", "ПРИМЕРНЫЙ")) 
                return true;
            for (Pullenti.Ner.Token tt = t; tt != null; tt = tt.Next) 
            {
                if (tt != t && tt.IsNewlineBefore) 
                    break;
                Pullenti.Ner.Referent r = tt.GetReferent();
                if ((r is Pullenti.Ner.Geo.GeoReferent) || (r is Pullenti.Ner.Address.StreetReferent) || (r is Pullenti.Ner.Address.AddressReferent)) 
                    return true;
                if (tt.IsValue("ГОТОВ", null) || tt.IsValue("ПЕРЕЕЗД", null) || tt.IsValue("КОМАНДИРОВКА", null)) 
                    return true;
            }
            return false;
        }
        public override Pullenti.Ner.ReferentToken ProcessOntologyItem(Pullenti.Ner.Token begin)
        {
            for (Pullenti.Ner.Token t = begin; t != null; t = t.Next) 
            {
                if (t.Next == null) 
                {
                    ResumeItemReferent re = new ResumeItemReferent();
                    re.Value = Pullenti.Ner.Core.MiscHelper.GetTextValue(begin, t, Pullenti.Ner.Core.GetTextAttr.No);
                    return new Pullenti.Ner.ReferentToken(re, begin, t);
                }
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
                MetaResume.Initialize();
                Pullenti.Ner.Vacance.Internal.VacanceToken.Initialize();
                Pullenti.Ner.ProcessorService.RegisterAnalyzer(new ResumeAnalyzer());
            }
        }
    }
}