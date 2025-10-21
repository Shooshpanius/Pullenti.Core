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

namespace Pullenti.Ner.Chemical.Internal
{
    class ChemicalToken : Pullenti.Ner.MetaToken
    {
        public List<ChemicalToken> Subtokens = null;
        public char Bracket = (char)0;
        public bool HiphenBefore;
        public string Name = null;
        public List<ChemicalUnit> Items = null;
        public int Num;
        public bool IsDoubt;
        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            if (HiphenBefore) 
                res.Append("-");
            if (Name != null) 
                res.Append(Name);
            if (Subtokens != null) 
            {
                res.Append(Bracket);
                foreach (ChemicalToken s in Subtokens) 
                {
                    res.Append(s.ToString());
                }
                res.Append((Bracket == '[' ? ']' : ')'));
            }
            if (Items != null && Name == null) 
            {
                foreach (ChemicalUnit u in Items) 
                {
                    res.Append(u.Mnem[0]);
                    if (u.Mnem.Length > 1) 
                        res.Append(char.ToLower(u.Mnem[1]));
                }
            }
            if (Num > 0) 
                res.Append(Num);
            return res.ToString();
        }
        public ChemicalToken(Pullenti.Ner.Token b, Pullenti.Ner.Token e) : base(b, e, null)
        {
        }
        public static List<ChemicalToken> TryParseList(Pullenti.Ner.Token t, int lev = 0)
        {
            Initialize();
            List<ChemicalToken> res = null;
            for (Pullenti.Ner.Token tt = t; tt != null; tt = tt.Next) 
            {
                ChemicalToken it = TryParse(tt, (res == null || res.Count == 0 ? null : res[res.Count - 1]), lev);
                if (it != null) 
                {
                    if (res == null) 
                        res = new List<ChemicalToken>();
                    if (it.Name == null && res.Count > 0 && it.IsWhitespaceBefore) 
                    {
                        if (res[res.Count - 1].Name == null) 
                            break;
                    }
                    res.Add(it);
                    tt = it.EndToken;
                    continue;
                }
                if ((tt.IsValue("ИЛИ", null) && res != null && res.Count == 1) && res[0].Name != null) 
                {
                    it = TryParse(tt.Next, null, lev);
                    if (it != null && it.Name != null) 
                    {
                        res.Add(it);
                        tt = it.EndToken;
                        continue;
                    }
                }
                break;
            }
            return res;
        }
        static ChemicalToken TryParse(Pullenti.Ner.Token t, ChemicalToken prev, int lev)
        {
            if (lev > 3) 
                return null;
            Pullenti.Ner.TextToken tt = t as Pullenti.Ner.TextToken;
            if (tt == null) 
                return null;
            if (tt.IsCharOf("[(")) 
            {
                List<ChemicalToken> li = TryParseList(tt.Next, lev + 1);
                if (li == null || li.Count == 0) 
                    return null;
                Pullenti.Ner.Token ee = li[li.Count - 1].EndToken.Next;
                if (ee == null || !ee.IsCharOf("])")) 
                    return null;
                ChemicalToken ct = new ChemicalToken(t, ee) { Subtokens = li };
                if (tt.IsChar('(')) 
                    ct.Bracket = '(';
                else 
                    ct.Bracket = '[';
                ct._addNum();
                return ct;
            }
            if (tt.IsHiphen) 
            {
                if (prev == null) 
                    return null;
                bool mustBeName = false;
                if (tt.IsWhitespaceBefore || tt.IsWhitespaceAfter) 
                {
                    if (prev.Name != null) 
                    {
                    }
                    else 
                        mustBeName = true;
                }
                ChemicalToken ct = TryParse(tt.Next, null, lev + 1);
                if (ct == null) 
                    return null;
                if (mustBeName && ct.Name == null) 
                    return null;
                if (prev.Name != null && ct.Name != null) 
                    return null;
                if (prev.Name == null && ct.Name == null) 
                    ct.HiphenBefore = true;
                ct.BeginToken = tt;
                return ct;
            }
            if (tt.Chars.IsLetter && !tt.Chars.IsAllLower) 
            {
                string str = tt.GetSourceText();
                List<ChemicalUnit> cus = null;
                int i;
                for (i = 0; i < str.Length; i++) 
                {
                    char ch0 = str[i];
                    if (char.IsLower(ch0)) 
                        break;
                    if (((int)ch0) > 0x80) 
                    {
                        ch0 = Pullenti.Morph.LanguageHelper.GetLatForCyr(ch0);
                        if (ch0 == 0) 
                            break;
                    }
                    string mnem = string.Format("{0}", ch0);
                    if (((i + 1) < str.Length) && char.IsLower(str[i + 1])) 
                    {
                        char ch1 = char.ToUpper(str[i + 1]);
                        if (((int)ch1) > 0x80) 
                        {
                            ch1 = Pullenti.Morph.LanguageHelper.GetLatForCyr(ch1);
                            if (ch1 == 0) 
                                break;
                        }
                        mnem = string.Format("{0}{1}", ch0, ch1);
                        i++;
                    }
                    ChemicalUnit cu;
                    if (!m_UnitsByMnem.TryGetValue(mnem, out cu)) 
                        break;
                    if (cus == null) 
                        cus = new List<ChemicalUnit>();
                    cus.Add(cu);
                }
                if (i >= str.Length && cus != null) 
                {
                    ChemicalToken ct = new ChemicalToken(t, t) { Items = cus };
                    ct._addNum();
                    if (ct.Num == 0 && ((!t.GetMorphClassInDictionary().IsUndefined || (t.LengthChar < 2)))) 
                        ct.IsDoubt = true;
                    for (int ii = 0; ii < (ct.Items.Count - 1); ii++) 
                    {
                        for (int jj = ii + 1; jj < ct.Items.Count; jj++) 
                        {
                            if (ct.Items[ii] == ct.Items[jj]) 
                                ct.IsDoubt = true;
                        }
                    }
                    if (ct.Items.Count > 6) 
                        ct.IsDoubt = true;
                    return ct;
                }
            }
            Pullenti.Ner.Token tt1 = null;
            for (t = tt; t != null; t = t.Next) 
            {
                Pullenti.Ner.Core.TerminToken tok = m_Termins.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
                if (tok != null) 
                {
                    tt1 = (t = tok.EndToken);
                    continue;
                }
                Pullenti.Ner.Core.NounPhraseToken npt = Pullenti.Ner.Core.NounPhraseHelper.TryParse(t, Pullenti.Ner.Core.NounPhraseParseAttr.No, 0, null);
                if (npt != null) 
                {
                    bool ok = false;
                    for (Pullenti.Ner.Token ttt = npt.BeginToken; ttt != null && ttt.EndChar <= npt.EndChar; ttt = ttt.Next) 
                    {
                        if (_canBePartName(ttt as Pullenti.Ner.TextToken)) 
                            ok = true;
                    }
                    if (ok) 
                    {
                        tt1 = (t = npt.EndToken);
                        continue;
                    }
                }
                if (_canBePartName(t as Pullenti.Ner.TextToken)) 
                {
                    tt1 = t;
                    continue;
                }
                break;
            }
            if (tt1 != null) 
            {
                ChemicalToken ci = new ChemicalToken(tt, tt1);
                ci.Name = Pullenti.Ner.Core.MiscHelper.GetTextValue(tt, tt1, Pullenti.Ner.Core.GetTextAttr.FirstNounGroupToNominative);
                if (tt == tt1) 
                {
                    ci.IsDoubt = true;
                    Pullenti.Ner.Core.TerminToken tok = m_Termins.TryParse(tt, Pullenti.Ner.Core.TerminParseAttr.No);
                    if (tok != null && (tok.Termin.Tag is ChemicalUnit)) 
                        ci.IsDoubt = false;
                }
                if (tt.Previous != null && tt.Previous.IsValue("ФОРМУЛА", null)) 
                    ci.IsDoubt = false;
                return ci;
            }
            return null;
        }
        static bool _canBePartName(Pullenti.Ner.TextToken t)
        {
            if (t == null) 
                return false;
            Pullenti.Ner.Core.TerminToken tok = m_Termins.TryParse(t, Pullenti.Ner.Core.TerminParseAttr.No);
            if (tok != null) 
                return true;
            string val = t.GetNormalCaseText(null, Pullenti.Morph.MorphNumber.Undefined, Pullenti.Morph.MorphGender.Undefined, false);
            Pullenti.Morph.MorphClass mc = t.GetMorphClassInDictionary();
            if (((((val.EndsWith("ИД") || val.EndsWith("ОЛ") || val.EndsWith("КИСЬ")) || val.EndsWith("ТАН") || val.EndsWith("ОЗА")) || val.EndsWith("ИН") || val.EndsWith("АТ")) || val.EndsWith("СИД") || val.StartsWith("ДИ")) || val.StartsWith("ТРИ") || val.StartsWith("ЧЕТЫР")) 
            {
                if (mc.IsUndefined) 
                    return true;
                if ((((val.EndsWith("КИСЬ") || val.EndsWith("ТОРИД") || val.EndsWith("ЦЕТАТ")) || val.EndsWith("РАЗИН") || val.EndsWith("КСИД")) || val.EndsWith("ФИД") || val.EndsWith("РИД")) || val.EndsWith("ОНАТ")) 
                    return true;
            }
            if ((((((val.StartsWith("ГЕКСА") || val.StartsWith("ДЕКА") || val.StartsWith("ТЕТРА")) || val.StartsWith("ПЕНТА") || val.StartsWith("ГЕПТА")) || val.StartsWith("ОКТА") || val.StartsWith("НОНА")) || val.StartsWith("УНДЕКА") || val.StartsWith("ДОДЕКА")) || val.StartsWith("ЭЙКОЗА") || val.StartsWith("ГЕКТА")) || val.StartsWith("КИЛА") || val.StartsWith("МИРИА")) 
                return true;
            return false;
        }
        public static Pullenti.Ner.Chemical.ChemicalFormulaReferent CreateReferent(List<ChemicalToken> li)
        {
            if (li.Count == 1) 
            {
                if (li[0].IsDoubt) 
                    return null;
                if (li[0].Bracket != 0) 
                    return null;
                if (((li[0].Items != null && li[0].Items.Count == 1)) || (((li[0].LengthChar < 5) || li[0].IsDoubt))) 
                {
                    bool ok = false;
                    int cou = 40;
                    for (Pullenti.Ner.Token tt = li[0].BeginToken.Previous; tt != null && cou > 0; tt = tt.Previous,cou--) 
                    {
                        if (tt.GetReferent() is Pullenti.Ner.Chemical.ChemicalFormulaReferent) 
                        {
                            ok = true;
                            break;
                        }
                        if (m_Termins.TryParse(tt, Pullenti.Ner.Core.TerminParseAttr.No) != null || m_Keywords.TryParse(tt, Pullenti.Ner.Core.TerminParseAttr.No) != null) 
                        {
                            ok = true;
                            break;
                        }
                    }
                    cou = 40;
                    if (!ok) 
                    {
                        for (Pullenti.Ner.Token tt = li[li.Count - 1].EndToken.Next; tt != null && cou > 0; tt = tt.Next,cou--) 
                        {
                            if (tt.GetReferent() is Pullenti.Ner.Chemical.ChemicalFormulaReferent) 
                            {
                                ok = true;
                                break;
                            }
                            if (m_Termins.TryParse(tt, Pullenti.Ner.Core.TerminParseAttr.No) != null || m_Keywords.TryParse(tt, Pullenti.Ner.Core.TerminParseAttr.No) != null) 
                            {
                                ok = true;
                                break;
                            }
                        }
                    }
                    if (!ok) 
                        return null;
                }
            }
            Pullenti.Ner.Chemical.ChemicalFormulaReferent res = new Pullenti.Ner.Chemical.ChemicalFormulaReferent();
            string val = null;
            for (int i = 0; i < li.Count; i++) 
            {
                if (li[i].Name != null) 
                    res.AddSlot(Pullenti.Ner.Chemical.ChemicalFormulaReferent.ATTR_NAME, li[i].Name, false, 0);
                else if (val == null) 
                    val = li[i].ToString();
                else 
                    val += li[i].ToString();
            }
            if (val != null) 
                res.Value = val;
            return res;
        }
        void _addNum()
        {
            if (EndToken.IsWhitespaceAfter) 
                return;
            if (EndToken.Next is Pullenti.Ner.NumberToken) 
            {
                EndToken = EndToken.Next;
                Num = (EndToken as Pullenti.Ner.NumberToken).IntValue.Value;
                return;
            }
            if (EndToken.Next.LengthChar == 1) 
            {
                for (int i = 2; i <= 9; i++) 
                {
                    if (EndToken.Next.IsChar((char)((0x2080 + i)))) 
                    {
                        EndToken = EndToken.Next;
                        Num = 2;
                        return;
                    }
                }
            }
            if ((EndToken.Next != null && EndToken.Next.IsCharOf("<[") && (EndToken.Next.Next is Pullenti.Ner.NumberToken)) && EndToken.Next.Next.Next != null && EndToken.Next.Next.Next.IsCharOf(">]")) 
            {
                Num = (EndToken.Next.Next as Pullenti.Ner.NumberToken).IntValue.Value;
                EndToken = EndToken.Next.Next.Next;
                return;
            }
        }
        public static void Initialize()
        {
            if (Units != null) 
                return;
            m_Termins = new Pullenti.Ner.Core.TerminCollection();
            m_Keywords = new Pullenti.Ner.Core.TerminCollection();
            m_UnitsByMnem = new Dictionary<string, ChemicalUnit>();
            Units = new List<ChemicalUnit>();
            string all = (((((("Водород,H;Гелий,He;Литий,Li;Бериллий,Be;Бор,B;Углерод,C;Азот,N;Кислород,O;Фтор,F;" + "Неон,Ne;Натрий,Na;Магний,Mg;Алюминий,Al;Кремний,Si;Фосфор,P;Сера,S;Хлор,Cl;Аргон,Ar;Калий,K;" + "Кальций,Ca;Скандий,Sc;Титан,Ti;Ванадий,V;Хром,Cr;Марганец,Mn;Железо,Fe;Кобальт,Co;Никель,Ni;") + "Медь,Cu;Цинк,Zn;Галлий,Ga;Германий,Ge;Мышьяк,As;Селен,Se;Бром,Br;Криптон,Kr;Рубидий,Rb;" + "Стронций,Sr;Иттрий,Y;Цирконий,Zr;Ниобий,Nb;Молибден,Mo;Технеций,Tc;Рутений,Ru;Родий,Rh;") + "Палладий,Pd;Серебро,Ag;Кадмий,Cd;Индий,In;Олово,Sn;Сурьма,Sb;Теллур,Te;Иод,I;Ксенон,Xe;" + "Цезий,Cs;Барий,Ba;Лантан,La;Церий,Ce;Празеодим,Pr;Неодим,Nd;Прометий,Pm;Самарий,Sm;") + "Европий,Eu;Гадолиний,Gd;Тербий,Tb;Диспрозий,Dy;Гольмий,Ho;Эрбий,Er;Тулий,Tm;Иттербий,Yb;" + "Лютеций,Lu;Гафний,Hf;Тантал,Ta;Вольфрам,W;Рений,Re;Осмий,Os;Иридий,Ir;Платина,Pt;") + "Золото,Au;Ртуть,Hg;Таллий,Tl;Свинец,Pb;Висмут,Bi;Полоний,Po;Астат,At;Радон,Rn;Франций,Fr;" + "Радий,Ra;Актиний,Ac;Торий,Th;Протактиний,Pa;Уран,U;Нептуний,Np;Плутоний,Pu;Америций,Am;") + "Кюрий,Cm;Берклий,Bk;Калифорний,Cf;Эйнштейний,Es;Фермий,Fm;Менделевий,Md;Нобелий,No;Лоуренсий,Lr;" + "Резерфордий,Rf;Дубний,Db;Сиборгий,Sg;Борий,Bh;Хассий,Hs;Мейтнерий,Mt;Дармштадтий,Ds;Рентгений,Rg;") + "Коперниций,Cn;Нихоний,Nh;Флеровий,Fl;Московий,Mc;Ливерморий,Lv;Теннессин,Ts;Оганесон,Og";
            foreach (string p in all.ToUpper().Split(';')) 
            {
                int i = p.IndexOf(',');
                if (i < 0) 
                    continue;
                ChemicalUnit ci = new ChemicalUnit(p.Substring(i + 1), p.Substring(0, i));
                Units.Add(ci);
                m_Termins.Add(new Pullenti.Ner.Core.Termin(ci.NameCyr) { Tag = ci });
                m_UnitsByMnem.Add(ci.Mnem, ci);
            }
            foreach (string s in new string[] {"КИСЛОТА", "РАСТВОР", "СПИРТ", "ВОДА", "СОЛЬ", "АММИАК", "БУТАН", "БЕНЗОЛ", "КЕРОСИН", "АМИН", "СКИПИДАР", "ОКИСЬ", "ГИДРИТ", "АММОНИЙ", "ПЕРЕКИСЬ", "КАРБОНАТ"}) 
            {
                m_Termins.Add(new Pullenti.Ner.Core.Termin(s));
            }
            foreach (string s in new string[] {"МЕТАЛЛ", "ГАЗ", "ТОПЛИВО", "МОНОТОПЛИВО", "СМЕСЬ", "ХИМИЧЕСКИЙ", "МОЛЕКУЛА", "АТОМ", "МОЛЕКУЛЯРНЫЙ", "АТОМАРНЫЙ"}) 
            {
                m_Keywords.Add(new Pullenti.Ner.Core.Termin(s));
            }
        }
        static Pullenti.Ner.Core.TerminCollection m_Termins;
        static Pullenti.Ner.Core.TerminCollection m_Keywords;
        static Dictionary<string, ChemicalUnit> m_UnitsByMnem;
        public static List<ChemicalUnit> Units;
    }
}