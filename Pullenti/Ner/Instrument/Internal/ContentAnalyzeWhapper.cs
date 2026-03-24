/*
 * SDK Pullenti Lingvo, version 4.33, fabruary 2026. Copyright (c) 2013-2026, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Ner.Instrument.Internal
{
    class ContentAnalyzeWhapper
    {
        public Pullenti.Ner.Decree.DecreeKind DocTyp = Pullenti.Ner.Decree.DecreeKind.Undefined;
        public FragToken TopDoc;
        public List<InstrToken1> Lines;
        public Pullenti.Ner.Instrument.InstrumentKind CitKind;
        public void Analyze(FragToken root, FragToken topDoc, bool isCitat, Pullenti.Ner.Instrument.InstrumentKind rootKind)
        {
            TopDoc = topDoc;
            CitKind = rootKind;
            List<InstrToken1> lines = new List<InstrToken1>();
            List<InstrToken1> footnotes = new List<InstrToken1>();
            InstrToken1 curFootnote = null;
            int directives = 0;
            int parts = 0;
            if (topDoc != null && topDoc.m_Doc != null) 
            {
                string ty = topDoc.m_Doc.Typ;
                if (ty != null) 
                {
                    if ((ty.Contains("КОДЕКС") || ty.Contains("ЗАКОН") || ty.Contains("КОНСТИТУЦИЯ")) || ty.Contains("КОНСТИТУЦІЯ")) 
                        DocTyp = Pullenti.Ner.Decree.DecreeKind.Kodex;
                    else if (ty.Contains("ДОГОВОР") || ty.Contains("ДОГОВІР") || ty.Contains("КОНТРАКТ")) 
                        DocTyp = Pullenti.Ner.Decree.DecreeKind.Contract;
                    else if (ty.Contains("ЗАДАНИЕ")) 
                        DocTyp = Pullenti.Ner.Decree.DecreeKind.Tz;
                }
            }
            for (Pullenti.Ner.Token t = root.BeginToken; t != null; t = t.Next) 
            {
                if (t.BeginChar > root.EndToken.EndChar) 
                    break;
                Pullenti.Ner.Decree.DecreePartReferent dpr = t.GetReferent() as Pullenti.Ner.Decree.DecreePartReferent;
                if (dpr != null && dpr.LocalTyp != null && (((dpr.Chapter != null || dpr.Clause != null || dpr.Section != null) || dpr.SubSection != null))) 
                    t = t.Kit.DebedToken(t);
                if (lines.Count == 38) 
                {
                }
                InstrToken1 lt = InstrToken1.Parse(t, false, topDoc, 0, (lines.Count > 0 ? lines[lines.Count - 1] : null), isCitat && t == root.BeginToken, root.EndToken.EndChar, false, false);
                if (lt != null && lt.HasManySpecChars && lt.IsPureHiphenLine) 
                {
                    if (lt.EndToken.Next != null && lt.EndToken.Next.IsChar('<')) 
                    {
                        lt = InstrToken1.Parse(lt.EndToken.Next, false, topDoc, 0, (lines.Count > 0 ? lines[lines.Count - 1] : null), isCitat && t == root.BeginToken, root.EndToken.EndChar, false, false);
                        if (lt != null && lt.Typ == InstrToken1.Types.Footnote) 
                            lt.BeginToken = t;
                        else 
                        {
                            t = lt.EndToken;
                            continue;
                        }
                    }
                }
                if ((t.IsTableControlChar && DocTyp != Pullenti.Ner.Decree.DecreeKind.Contract && DocTyp != Pullenti.Ner.Decree.DecreeKind.Tz) && ((lt == null || lt.Typ != InstrToken1.Types.Subsection))) 
                {
                    curFootnote = null;
                    List<Pullenti.Ner.Core.TableRowToken> rows = Pullenti.Ner.Core.TableHelper.TryParseRows(t, 0, true, false);
                    if (rows != null) 
                        lt = new InstrToken1(t, rows[rows.Count - 1].EndToken) { Typ = InstrToken1.Types.Line };
                }
                if (lt == null) 
                    continue;
                if (lt.Typ == InstrToken1.Types.Appendix) 
                {
                }
                if (lt.NumTyp == NumberTypes.Digit && lt.Numbers.Count == 1 && lt.Numbers[0] == "30") 
                {
                }
                if (lt.Typ == InstrToken1.Types.Editions) 
                {
                    if ((!lt.IsNewlineAfter && lt.EndToken.Next != null && lt.EndToken.Next.IsNewlineAfter) && (lt.EndToken.Next is Pullenti.Ner.TextToken) && !lt.EndToken.Next.Chars.IsLetter) 
                        lt.EndToken = lt.EndToken.Next;
                }
                if (lt.Numbers.Count > 0) 
                {
                }
                if (lines.Count == 0 && rootKind != Pullenti.Ner.Instrument.InstrumentKind.Undefined) 
                {
                    if ((rootKind == Pullenti.Ner.Instrument.InstrumentKind.Indention || rootKind == Pullenti.Ner.Instrument.InstrumentKind.Item || rootKind == Pullenti.Ner.Instrument.InstrumentKind.Subitem) || rootKind == Pullenti.Ner.Instrument.InstrumentKind.ClausePart) 
                        lt.Typ = InstrToken1.Types.Line;
                    else if (rootKind == Pullenti.Ner.Instrument.InstrumentKind.Chapter) 
                        lt.Typ = InstrToken1.Types.Chapter;
                    else if (rootKind == Pullenti.Ner.Instrument.InstrumentKind.Clause) 
                        lt.Typ = InstrToken1.Types.Clause;
                    else if (rootKind == Pullenti.Ner.Instrument.InstrumentKind.Section) 
                        lt.Typ = InstrToken1.Types.Section;
                    else if (rootKind == Pullenti.Ner.Instrument.InstrumentKind.Subsection) 
                        lt.Typ = InstrToken1.Types.Subsection;
                    else if (rootKind == Pullenti.Ner.Instrument.InstrumentKind.DocPart) 
                        lt.Typ = InstrToken1.Types.DocPart;
                }
                if (curFootnote != null && lt.Typ != InstrToken1.Types.Editions) 
                {
                    if (lt.Typ != InstrToken1.Types.Line) 
                        curFootnote = null;
                    else if (lt.Numbers.Count > 0) 
                        curFootnote = null;
                    else if (lt.NewlinesBeforeCount > 1) 
                    {
                        if (curFootnote.EndToken.IsChar(':')) 
                        {
                        }
                        else 
                            curFootnote = null;
                    }
                }
                if (lt.Typ == InstrToken1.Types.Clause && lt.FirstNumber == 103) 
                {
                }
                if (lt.EndChar > root.EndChar) 
                    lt.EndToken = root.EndToken;
                if (lt.Typ == InstrToken1.Types.Directive) 
                    directives++;
                if (((lt.NumTyp == NumberTypes.Letter && lt.Numbers.Count == 1 && lt.MinNumber == null) && lt.LastNumber > 1 && rootKind != Pullenti.Ner.Instrument.InstrumentKind.Subitem) && rootKind != Pullenti.Ner.Instrument.InstrumentKind.Item) 
                {
                    bool ok = false;
                    for (int i = lines.Count - 1; i >= 0; i--) 
                    {
                        if (lines[i].NumTyp == lt.NumTyp) 
                        {
                            int j = lt.LastNumber - lines[i].LastNumber;
                            ok = j == 1 || j == 2;
                            break;
                        }
                    }
                    if (!ok) 
                    {
                        lt.NumTyp = NumberTypes.Undefined;
                        lt.Numbers.Clear();
                    }
                }
                if (lt.TypContainerRank > 0 && !lt.IsNumDoubt) 
                    parts++;
                if (lt.Typ != InstrToken1.Types.Footnote) 
                {
                    if (curFootnote == null) 
                        lines.Add(lt);
                    else 
                    {
                        if (curFootnote.SubItems == null) 
                            curFootnote.SubItems = new List<InstrToken1>();
                        curFootnote.SubItems.Add(lt);
                    }
                }
                else 
                {
                    footnotes.Add(lt);
                    curFootnote = lt;
                }
                t = lt.EndToken;
            }
            ListHelper.CorrectIndex(lines);
            ListHelper.CorrectAppList(lines);
            this._correctForms(lines);
            if (directives > 0 && directives > parts) 
                this._analizeContentWithDirectives(root, lines, topDoc != null && topDoc.m_Doc != null && topDoc.m_Doc.CaseNumber != null);
            else 
                this._analizeContentWithContainers(root, lines, 0, topDoc);
            this._analizePreamble(root);
            root._analizeTables();
            if (DocTyp == Pullenti.Ner.Decree.DecreeKind.Contract) 
            {
            }
            else 
                this._correctKodexParts(root);
            this._analizeSections(root);
            this._correctNames(root, null);
            EditionHelper.AnalizeEditions(root);
            if (DocTyp == Pullenti.Ner.Decree.DecreeKind.Contract) 
                ContractHelper.CorrectDummyNewlines(root);
            ListHelper.Analyze(root);
            if (rootKind == Pullenti.Ner.Instrument.InstrumentKind.ClausePart || rootKind == Pullenti.Ner.Instrument.InstrumentKind.Item || rootKind == Pullenti.Ner.Instrument.InstrumentKind.Subitem) 
            {
                foreach (FragToken ch in root.Children) 
                {
                    if (ch.Kind == Pullenti.Ner.Instrument.InstrumentKind.Item) 
                    {
                        if (rootKind == Pullenti.Ner.Instrument.InstrumentKind.ClausePart) 
                        {
                            ch.Kind = Pullenti.Ner.Instrument.InstrumentKind.ClausePart;
                            foreach (FragToken chh in ch.Children) 
                            {
                                if (chh.Kind == Pullenti.Ner.Instrument.InstrumentKind.Subitem) 
                                    chh.Kind = Pullenti.Ner.Instrument.InstrumentKind.Item;
                            }
                        }
                        else if (rootKind == Pullenti.Ner.Instrument.InstrumentKind.Subitem) 
                            ch.Kind = Pullenti.Ner.Instrument.InstrumentKind.Subitem;
                    }
                }
            }
            this._postCorrect(root, lines);
            foreach (InstrToken1 fn in footnotes) 
            {
                if (!this._addFootnote(root, fn, true)) 
                {
                    FragToken fnn = new FragToken(fn.BeginToken, fn.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Footnote };
                    NumberingHelper.CreateNumber(fnn, fn);
                    fnn.CheckExpired();
                    root.Children.Add(fnn);
                }
            }
            for (int i = 1; i < root.Children.Count; i++) 
            {
                FragToken ch = root.Children[i];
                if (ch.Kind == Pullenti.Ner.Instrument.InstrumentKind.Content && ch.Children.Count == 0 && root.Children[i - 1].EndChar >= ch.EndChar) 
                {
                    root.Children.RemoveAt(i);
                    i--;
                }
            }
        }
        void _postCorrect(FragToken root, List<InstrToken1> lines)
        {
            foreach (FragToken ch in root.Children) 
            {
                this._postCorrect(ch, lines);
            }
            if (root.Children.Count > 0) 
            {
                if (root.EndChar < root.Children[root.Children.Count - 1].EndChar) 
                    root.EndToken = root.Children[root.Children.Count - 1].EndToken;
                if (root.BeginChar > root.Children[0].BeginChar) 
                    root.BeginToken = root.Children[0].BeginToken;
            }
        }
        void _correctForms(List<InstrToken1> lines)
        {
            for (int i = 0; i < lines.Count; i++) 
            {
                InstrToken1 line = lines[i];
                if (line.Typ != InstrToken1.Types.Form) 
                    continue;
                int j;
                for (j = i + 1; j < lines.Count; ) 
                {
                    InstrToken1 line1 = lines[j];
                    if ((line1.Typ != InstrToken1.Types.Line && line1.Typ != InstrToken1.Types.FirstLine && line1.Typ != InstrToken1.Types.Signs) && line1.Typ != InstrToken1.Types.Comment && line1.Typ != InstrToken1.Types.Editions) 
                        break;
                    if (line.SubItems == null) 
                        line.SubItems = new List<InstrToken1>();
                    line.SubItems.Add(line1);
                    lines.RemoveAt(j);
                }
            }
        }
        FragToken _createFootnote(InstrToken1 fn)
        {
            FragToken fnn = new FragToken(fn.BeginToken, fn.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Footnote };
            NumberingHelper.CreateNumber(fnn, fn);
            fnn.CheckExpired();
            if (fn.SubItems != null && fn.SubItems.Count > 0) 
            {
                if ((fn.EndChar - fn.NumEndToken.EndChar) > 4) 
                    fnn.Children.Add(new FragToken(fn.NumEndToken.Next, fn.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Indention });
                this._analizeContentWithoutContainers(fnn, fn.SubItems, true, false, false);
                fnn.EndToken = fn.SubItems[fn.SubItems.Count - 1].EndToken;
            }
            return fnn;
        }
        bool _addFootnote(FragToken root, InstrToken1 fn, bool notLast = false)
        {
            if (fn.BeginChar < root.BeginChar) 
                return false;
            if ((((root.Kind == Pullenti.Ner.Instrument.InstrumentKind.Editions || root.Kind == Pullenti.Ner.Instrument.InstrumentKind.Comment || root.Kind == Pullenti.Ner.Instrument.InstrumentKind.Number) || root.Kind == Pullenti.Ner.Instrument.InstrumentKind.Head || root.Kind == Pullenti.Ner.Instrument.InstrumentKind.Tail) || root.Kind == Pullenti.Ner.Instrument.InstrumentKind.Typ || root.Kind == Pullenti.Ner.Instrument.InstrumentKind.Keyword) || root.Kind == Pullenti.Ner.Instrument.InstrumentKind.Footnote) 
                return false;
            for (int i = 0; i < (root.Children.Count - 1); i++) 
            {
                if (root.Children[i].BeginChar < fn.BeginChar) 
                {
                    FragToken ch0 = root.Children[i];
                    if (fn.EndChar < root.Children[i + 1].BeginChar) 
                    {
                        if (ch0.Kind == Pullenti.Ner.Instrument.InstrumentKind.Table || ch0.Kind == Pullenti.Ner.Instrument.InstrumentKind.Footnote || ch0.Kind == Pullenti.Ner.Instrument.InstrumentKind.Notice) 
                        {
                            FragToken fnn = this._createFootnote(fn);
                            root.Children.Insert(i + 1, fnn);
                            return true;
                        }
                        if (this._addFootnote(ch0, fn, true)) 
                            return true;
                        return false;
                    }
                    FragToken ch1 = root.Children[i + 1];
                    if (fn.EndChar <= ch1.EndChar) 
                    {
                        FragToken fnn = this._createFootnote(fn);
                        if (fn.BeginChar > ch1.BeginChar) 
                        {
                            if (this._addFootnote(ch1, fn, true)) 
                                return true;
                            root.Children.Insert(i + 2, fnn);
                            ch1.EndToken = fn.BeginToken.Previous;
                        }
                        else 
                        {
                            root.Children.Insert(i + 1, fnn);
                            if (fn.EndChar == ch1.EndChar) 
                                root.Children.RemoveAt(i + 2);
                            else if (ch1.BeginChar < fn.EndToken.Next.BeginChar) 
                                ch1.BeginToken = fn.EndToken.Next;
                        }
                        return true;
                    }
                }
            }
            if (root.Children.Count > 0) 
            {
                FragToken ch1 = root.Children[root.Children.Count - 1];
                if (ch1.Kind == Pullenti.Ner.Instrument.InstrumentKind.Table || ch1.Kind == Pullenti.Ner.Instrument.InstrumentKind.Footnote || ch1.Kind == Pullenti.Ner.Instrument.InstrumentKind.Notice) 
                {
                    FragToken fnn = this._createFootnote(fn);
                    root.Children.Add(fnn);
                    return true;
                }
                if (this._addFootnote(ch1, fn, true)) 
                    return true;
            }
            if (root.Kind != Pullenti.Ner.Instrument.InstrumentKind.Content) 
            {
                if (fn.BeginChar > root.EndChar && notLast) 
                {
                    FragToken fnn = this._createFootnote(fn);
                    root.Children.Add(fnn);
                    return true;
                }
            }
            return false;
        }
        void _analizeContentWithContainers(FragToken root, List<InstrToken1> lines, int topLevel, FragToken topDoc)
        {
            List<InstrToken1> nums = new List<InstrToken1>();
            int k = 0;
            int lev = 100;
            InstrToken1 li0 = null;
            int koef = 0;
            if (root.Kind == Pullenti.Ner.Instrument.InstrumentKind.Clause) 
            {
                int split = -1;
                int cou = 0;
                string suf = null;
                for (int i = 1; i < (lines.Count - 1); i++) 
                {
                    InstrToken1 line = lines[i];
                    if (line.Typ != InstrToken1.Types.Notice) 
                        continue;
                    if ((line.BeginToken is Pullenti.Ner.TextToken) && (line.BeginToken as Pullenti.Ner.TextToken).Term == "ПРИМЕЧАНИЯ" && line.EndToken.IsChar(':')) 
                    {
                        if (lines[i + 1].Numbers.Count == 1 && lines[i + 1].Numbers[0] == "1") 
                        {
                            split = i;
                            cou++;
                            suf = lines[i + 1].NumSuffix;
                        }
                    }
                }
                if (cou == 1) 
                {
                    List<InstrToken1> lines0 = new List<InstrToken1>(lines);
                    lines0.RemoveRange(split, lines.Count - split);
                    this._analizeContentWithContainers(root, lines0, topLevel, topDoc);
                    lines0.Clear();
                    for (int i = split + 1; i < lines.Count; i++) 
                    {
                        InstrToken1 line = lines[i];
                        if (line.Typ == InstrToken1.Types.Line && line.Numbers.Count == 1 && line.NumSuffix == suf) 
                            line.Typ = InstrToken1.Types.Notice;
                        lines0.Add(line);
                    }
                    this._analizeContentWithContainers(root, lines0, topLevel, topDoc);
                    return;
                }
            }
            if (((root.Kind == Pullenti.Ner.Instrument.InstrumentKind.Paragraph && lines.Count > 10 && lines[0].Typ == InstrToken1.Types.Line) && lines[0].Numbers.Count > 0 && !lines[0].HasVerb) && lines[1].Typ == InstrToken1.Types.Clause) 
            {
                nums.Add(lines[0]);
                for (int ii = 2; ii < (lines.Count - 1); ii++) 
                {
                    InstrToken1 ch = lines[ii];
                    if (ch.Typ != InstrToken1.Types.Line || ch.HasVerb || ch.Numbers.Count != nums[0].Numbers.Count) 
                        continue;
                    InstrToken1 la = nums[nums.Count - 1];
                    if (NumberingHelper.CalcDelta(la, ch, false) != 1) 
                        continue;
                    if (la.NumTyp != ch.NumTyp || la.NumSuffix != ch.NumSuffix) 
                        continue;
                    if (ch.EndToken.IsChar('.')) 
                    {
                        if (!la.EndToken.IsChar('.')) 
                            continue;
                    }
                    bool hasClause = false;
                    for (int jj = ii + 1; jj < lines.Count; jj++) 
                    {
                        if (lines[jj].Typ == InstrToken1.Types.Clause) 
                        {
                            hasClause = true;
                            break;
                        }
                        else if (lines[jj].Typ != InstrToken1.Types.Comment && lines[jj].Typ != InstrToken1.Types.Editions) 
                            break;
                    }
                    if (hasClause) 
                        nums.Add(ch);
                }
                if (nums.Count < 2) 
                    nums.Clear();
                else 
                {
                    koef = 2;
                    foreach (InstrToken1 nn in nums) 
                    {
                        nn.Typ = InstrToken1.Types.Subparagraph;
                        lev = nn.TypContainerRank;
                    }
                }
            }
            if (nums.Count == 0) 
            {
                nums.Clear();
                foreach (InstrToken1 li in lines) 
                {
                    if (li.Typ == InstrToken1.Types.Comment || li.Typ == InstrToken1.Types.Editions) 
                        continue;
                    if (li0 == null) 
                        li0 = li;
                    k++;
                    if (li.Typ == InstrToken1.Types.DocPart && li.Numbers.Count == 0) 
                        continue;
                    if (li.TypContainerRank > topLevel) 
                    {
                        if (li.TypContainerRank < lev) 
                        {
                            if (nums.Count > 2 && li.Numbers.Count == 0) 
                            {
                            }
                            else if (k > 20) 
                            {
                            }
                            else 
                            {
                                lev = li.TypContainerRank;
                                nums.Clear();
                            }
                        }
                        if (li.TypContainerRank == lev) 
                            nums.Add(li);
                    }
                }
                for (int i = 0; i < nums.Count; i++) 
                {
                    int d0 = (i > 0 ? NumberingHelper.CalcDelta(nums[i - 1], nums[i], true) : 0);
                    int d1 = ((i + 1) < nums.Count ? NumberingHelper.CalcDelta(nums[i], nums[i + 1], true) : 0);
                    int d01 = (i > 0 && ((i + 1) < nums.Count) ? NumberingHelper.CalcDelta(nums[i - 1], nums[i + 1], true) : 0);
                    if (d0 == 1) 
                    {
                        if (d1 == 1) 
                            continue;
                        if (d01 == 1 && !nums[i + 1].IsNumDoubt && nums[i].IsNumDoubt) 
                        {
                            nums.RemoveAt(i);
                            i--;
                        }
                        continue;
                    }
                    if (d01 == 1 && nums[i].IsNumDoubt) 
                    {
                        nums.RemoveAt(i);
                        i--;
                        continue;
                    }
                }
                for (int i = 1; i < nums.Count; i++) 
                {
                    int d = NumberingHelper.CalcDelta(nums[i - 1], nums[i], true);
                    if (d == 1) 
                        koef += 2;
                    else if (d == 2) 
                        koef++;
                    else if (d <= 0) 
                        koef--;
                }
                if (nums.Count > 0) 
                {
                    bool hasNumBefore = false;
                    foreach (InstrToken1 li in lines) 
                    {
                        if (li == nums[0]) 
                            break;
                        else if (li.Numbers.Count > 0) 
                            hasNumBefore = true;
                    }
                    if (!hasNumBefore && ((nums[0].LastNumber == 1 || ((nums[0] == li0 && nums[0].NumSuffix != null))))) 
                        koef += 2;
                    else if (nums[0].Typ == InstrToken1.Types.Clause && nums[0] == li0) 
                        koef += 2;
                }
            }
            bool isChapters = false;
            if (nums.Count == 0) 
            {
                int chaps = 0;
                int nons = 0;
                int clauses = 0;
                for (int i = 0; i < lines.Count; i++) 
                {
                    InstrToken1 li = lines[i];
                    if (li.Typ == InstrToken1.Types.Chapter) 
                    {
                        nums.Add(li);
                        chaps++;
                        lev = li.TypContainerRank;
                    }
                    else if (li.Typ == InstrToken1.Types.Line && li.TitleTyp != InstrToken1.StdTitleType.Undefined) 
                    {
                        nums.Add(li);
                        nons++;
                    }
                    else if (li.Typ == InstrToken1.Types.Clause) 
                        clauses++;
                }
                if (chaps == 0) 
                    nums.Clear();
                else 
                {
                    koef += 2;
                    isChapters = true;
                }
            }
            if ((nums.Count == 0 && topDoc != null && topDoc.m_Doc != null) && topDoc.m_Doc.Typ == "ГОСТ") 
            {
                for (int i = 0; i < lines.Count; i++) 
                {
                    InstrToken1 li = lines[i];
                    if (li.NumTyp != NumberTypes.Digit) 
                        continue;
                    if (li.HasVerb) 
                        continue;
                    if (nums.Count > 0) 
                    {
                        int d = NumberingHelper.CalcDelta(nums[nums.Count - 1], li, true);
                        if (d == 1 || d == 2) 
                            nums.Add(li);
                    }
                    else 
                        nums.Add(li);
                }
                if (nums.Count > 1) 
                {
                    koef = nums.Count;
                    isChapters = true;
                }
            }
            if (koef < 2) 
            {
                if (topLevel < InstrToken1._calcRank(InstrToken1.Types.Chapter)) 
                {
                    if (this._analizeChapterWithoutKeywords(root, lines, topDoc)) 
                        return;
                }
                this._analizeContentWithoutContainers(root, lines, false, false, false);
                return;
            }
            int n = 0;
            int names = 0;
            FragToken fr = null;
            List<InstrToken1> blk = new List<InstrToken1>();
            for (int i = 0; i <= lines.Count; i++) 
            {
                InstrToken1 li = (i < lines.Count ? lines[i] : null);
                if (li == null || (((n < nums.Count) && li == nums[n]))) 
                {
                    if (blk.Count > 0) 
                    {
                        if (fr == null) 
                        {
                            fr = new FragToken(blk[0].BeginToken, blk[blk.Count - 1].EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Content };
                            if (blk.Count == 1) 
                                fr.Itok = blk[0];
                            root.Children.Add(fr);
                        }
                        fr.EndToken = blk[blk.Count - 1].EndToken;
                        this._analizeContentWithContainers(fr, blk, lev, topDoc);
                        blk.Clear();
                        fr = null;
                    }
                }
                if (li == null) 
                    break;
                if ((n < nums.Count) && li == nums[n]) 
                {
                    n++;
                    fr = new FragToken(li.BeginToken, li.EndToken) { Itok = li, IsExpired = li.IsExpired };
                    root.Children.Add(fr);
                    if (li.Typ == InstrToken1.Types.DocPart) 
                        fr.Kind = Pullenti.Ner.Instrument.InstrumentKind.DocPart;
                    else if (li.Typ == InstrToken1.Types.ClausePart) 
                        fr.Kind = Pullenti.Ner.Instrument.InstrumentKind.ClausePart;
                    else if (li.Typ == InstrToken1.Types.Section) 
                        fr.Kind = Pullenti.Ner.Instrument.InstrumentKind.Section;
                    else if (li.Typ == InstrToken1.Types.Subsection) 
                        fr.Kind = Pullenti.Ner.Instrument.InstrumentKind.Subsection;
                    else if (li.Typ == InstrToken1.Types.Paragraph) 
                        fr.Kind = Pullenti.Ner.Instrument.InstrumentKind.Paragraph;
                    else if (li.Typ == InstrToken1.Types.Subparagraph) 
                        fr.Kind = Pullenti.Ner.Instrument.InstrumentKind.Subparagraph;
                    else if (li.Typ == InstrToken1.Types.Chapter) 
                        fr.Kind = Pullenti.Ner.Instrument.InstrumentKind.Chapter;
                    else if (li.Typ == InstrToken1.Types.Clause) 
                        fr.Kind = Pullenti.Ner.Instrument.InstrumentKind.Clause;
                    else if (li.Typ == InstrToken1.Types.Notice) 
                        fr.Kind = Pullenti.Ner.Instrument.InstrumentKind.Notice;
                    else if (li.Typ == InstrToken1.Types.Form) 
                        fr.Kind = Pullenti.Ner.Instrument.InstrumentKind.Form;
                    else if (isChapters) 
                        fr.Kind = Pullenti.Ner.Instrument.InstrumentKind.Chapter;
                    if (li.BeginToken != li.NumBeginToken && li.NumBeginToken != null) 
                        fr.Children.Add(new FragToken(li.BeginToken, li.NumBeginToken.Previous) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Keyword, DefVal2 = true, Itok = li });
                    NumberingHelper.CreateNumber(fr, li);
                    if (fr.Kind == Pullenti.Ner.Instrument.InstrumentKind.Clause && fr.Number == 33) 
                    {
                    }
                    if (li.NumEndToken != li.EndToken && li.NumEndToken != null && !li.IsExpired) 
                    {
                        if (!li.AllUpper && ((((li.HasVerb && names == 0 && li.EndToken.IsCharOf(".:,"))) || li.EndToken.IsChar(':')))) 
                            fr.Children.Add(new FragToken(li.NumEndToken.Next, li.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Content });
                        else 
                        {
                            FragToken frName = new FragToken(li.NumEndToken.Next, li.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Name, DefVal2 = true, Itok = li };
                            fr.Children.Add(frName);
                            fr.Name = FragToken.GetRestoredNameMT(frName, false);
                            i = correctName(fr, frName, lines, i);
                            names++;
                        }
                    }
                    else if (li.TitleTyp != InstrToken1.StdTitleType.Undefined) 
                    {
                        FragToken frName = new FragToken(li.BeginToken, li.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Name, DefVal2 = true, Itok = li };
                        fr.Children.Add(frName);
                        fr.Name = FragToken.GetRestoredNameMT(frName, false);
                        i = correctName(fr, frName, lines, i);
                        names++;
                    }
                    else if (((((i + 1) < lines.Count) && lines[i + 1].Numbers.Count == 0 && !lines[i + 1].HasVerb) && !lines[i + 1].HasManySpecChars && lines[i + 1].Typ != InstrToken1.Types.Editions) && lines[i + 1].Typ != InstrToken1.Types.Comment) 
                    {
                        if ((lines[i + 1].AllUpper || ((((i + 2) < lines.Count) && lines[i + 2].Numbers.Count > 0)) || (lines[i + 1].BeginToken.IsChar('['))) || lines[i].EndToken.IsChar('.')) 
                        {
                            i++;
                            li = lines[i];
                            fr.EndToken = li.EndToken;
                            FragToken frName = new FragToken(li.BeginToken, li.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Name, DefVal2 = true, Itok = li };
                            fr.Children.Add(frName);
                            fr.Name = FragToken.GetRestoredNameMT(frName, false);
                            i = correctName(fr, frName, lines, i);
                            names++;
                        }
                    }
                    continue;
                }
                if (li.Typ == InstrToken1.Types.Editions && blk.Count == 0 && fr != null) 
                    fr.Children.Add(new FragToken(li.BeginToken, li.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Editions });
                else 
                    blk.Add(li);
            }
        }
        public static int correctName(FragToken fr, FragToken frName, List<InstrToken1> lines, int i)
        {
            if ((i + 1) >= lines.Count) 
                return i;
            InstrToken1 li = lines[i];
            if (li.Typ == InstrToken1.Types.Chapter) 
            {
            }
            int endBracket = 0;
            for (Pullenti.Ner.Token tt = fr.BeginToken; tt != null && (tt.EndChar < fr.EndChar); tt = tt.Next) 
            {
                if (Pullenti.Ner.Core.BracketHelper.CanBeStartOfSequence(tt, false, false)) 
                {
                    Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(tt, Pullenti.Ner.Core.BracketParseAttr.CanBeManyLines, 100);
                    if (br != null && br.EndChar > fr.EndChar) 
                        endBracket = br.EndChar;
                }
            }
            if (fr.Name != null && (fr.Name.Length < 300)) 
            {
                for (; (i + 1) < lines.Count; i++) 
                {
                    if (fr.Name.Length > 500) 
                        break;
                    InstrToken1 lii = lines[i + 1];
                    if (lii.Numbers.Count > 0 || lii.Typ != InstrToken1.Types.Line) 
                        break;
                    if (lii.EndToken.IsChar(':')) 
                        break;
                    if (li.EndToken.IsCharOf(";")) 
                        break;
                    if (li.BeginToken.IsTableControlChar) 
                        break;
                    if (lii.HasVerb) 
                        break;
                    if (li.EndToken.IsChar('.')) 
                    {
                        if (lii.AllUpper && li.AllUpper) 
                        {
                        }
                        else 
                            break;
                    }
                    if (li.AllUpper && !lii.AllUpper) 
                        break;
                    if (lii.BeginToken.Previous.IsTableControlChar) 
                        break;
                    if (endBracket > 0 && (lii.BeginChar < endBracket)) 
                    {
                    }
                    else if (lii.BeginToken.Previous.IsCommaAnd) 
                    {
                    }
                    else if (lii.IsBoldFont && li.IsBoldFont && li.IsItalicFont == lii.IsItalicFont) 
                    {
                    }
                    else 
                    {
                        if ((li.LengthChar < (lii.LengthChar / 2)) && lii.HasVerb) 
                            break;
                        if (li.HasManySpecChars) 
                            break;
                        if (lii.BeginToken.WhitespacesBeforeCount > 15) 
                            break;
                        if (lii.BeginToken.IsValue("НЕТ", null) || lii.BeginToken.IsValue("НЕ", null) || lii.BeginToken.IsValue("ОТСУТСТВОВАТЬ", null)) 
                            break;
                        if (!(lii.BeginToken is Pullenti.Ner.TextToken)) 
                            break;
                        if (lii.BeginToken.IsChar('(')) 
                        {
                            Pullenti.Ner.Core.BracketSequenceToken br = Pullenti.Ner.Core.BracketHelper.TryParse(lii.BeginToken, Pullenti.Ner.Core.BracketParseAttr.CanBeManyLines, 30);
                            if (br != null && br.IsNewlineAfter) 
                            {
                                fr.EndToken = (frName.EndToken = br.EndToken);
                                frName.DefVal2 = true;
                                fr.Name = FragToken.GetRestoredNameMT(frName, false);
                                for (; i < lines.Count; i++) 
                                {
                                    if (lines[i].BeginChar > fr.EndChar) 
                                        break;
                                }
                                return i - 1;
                            }
                        }
                        Pullenti.Morph.MorphClass mc = lii.BeginToken.GetMorphClassInDictionary();
                        if (mc.IsUndefined) 
                            break;
                        Pullenti.Ner.Token tt = lii.BeginToken;
                        while (tt is Pullenti.Ner.MetaToken) 
                        {
                            tt = (tt as Pullenti.Ner.MetaToken).BeginToken;
                        }
                        if (tt.Chars.IsCapitalUpper || !tt.Chars.IsLetter || mc.IsPreposition) 
                        {
                            if (!li.EndToken.IsChar(',') && !li.EndToken.IsHiphen && !li.EndToken.Morph.Class.IsConjunction) 
                            {
                                if (!mc.IsPreposition) 
                                    break;
                                if (li.HasVerb) 
                                    break;
                                if (!li.AllUpper) 
                                    break;
                            }
                        }
                    }
                    li = lii;
                    fr.EndToken = (frName.EndToken = li.EndToken);
                    frName.DefVal2 = true;
                    fr.Name = FragToken.GetRestoredNameMT(frName, false);
                }
            }
            return i;
        }
        bool _analizeChapterWithoutKeywords(FragToken root, List<InstrToken1> lines, FragToken topDoc)
        {
            List<InstrToken1> nums = NumberingHelper.ExtractMainSequence(lines, true, false);
            bool isContractStruct = false;
            if (nums == null || nums[0].Numbers.Count != 1 || nums[0].Numbers[0] != "1") 
            {
                if (DocTyp == Pullenti.Ner.Decree.DecreeKind.Contract) 
                {
                    List<InstrToken1> nums1 = new List<InstrToken1>();
                    string num0 = "1";
                    bool ok = true;
                    for (int i = 1; i < lines.Count; i++) 
                    {
                        InstrToken1 li = lines[i];
                        InstrToken1 li0 = lines[i - 1];
                        if ((nums1.Count > 0 && nums1[0].TitleTyp == InstrToken1.StdTitleType.Subject && nums1[0].Numbers.Count == 0) && nums1[0].AllUpper) 
                        {
                            if (li0.Numbers.Count <= 1 && ((li0.AllUpper || li0.TitleTyp != InstrToken1.StdTitleType.Undefined))) 
                                nums1.Add(li0);
                            continue;
                        }
                        if (li.Numbers.Count == 2 && li.Numbers[0] == num0 && li.Numbers[1] == "1") 
                        {
                            if (li0.Numbers.Count == 0 && !li0.BeginToken.Chars.IsAllLower) 
                            {
                            }
                            else if (li0.Numbers.Count == 1 && li0.Numbers[0] == num0) 
                            {
                            }
                            else 
                            {
                                ok = false;
                                break;
                            }
                            nums1.Add(li0);
                            num0 = ((nums1.Count + 1)).ToString();
                            continue;
                        }
                        if (li0.TitleTyp != InstrToken1.StdTitleType.Undefined || ((li0.Numbers.Count == 1 && li0.Numbers[0] == num0))) 
                        {
                            nums1.Add(li0);
                            num0 = ((nums1.Count + 1)).ToString();
                        }
                    }
                    if (ok && nums1.Count > 1) 
                    {
                        nums = nums1;
                        isContractStruct = true;
                    }
                }
            }
            if (nums == null) 
                return false;
            if (nums.Count > 500) 
                return false;
            int n = 0;
            int err = 0;
            FragToken fr = null;
            List<InstrToken1> blk = new List<InstrToken1>();
            List<FragToken> childs = new List<FragToken>();
            for (int i = 0; i <= lines.Count; i++) 
            {
                InstrToken1 li = (i < lines.Count ? lines[i] : null);
                if (li == null || (((n < nums.Count) && li == nums[n])) || ((n >= nums.Count && li.TitleTyp != InstrToken1.StdTitleType.Undefined))) 
                {
                    if (blk.Count > 0) 
                    {
                        if (fr == null) 
                        {
                            fr = new FragToken(blk[0].BeginToken, blk[blk.Count - 1].EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Content };
                            if (blk.Count == 1) 
                                fr.Itok = blk[0];
                            childs.Add(fr);
                        }
                        fr.EndToken = blk[blk.Count - 1].EndToken;
                        this._analizeContentWithoutContainers(fr, blk, false, false, false);
                        blk.Clear();
                        fr = null;
                    }
                }
                if (li == null) 
                    break;
                if ((n < nums.Count) && li == nums[n]) 
                {
                    n++;
                    if (!li.AllUpper && li.HasVerb) 
                    {
                        if (li.NumTyp == NumberTypes.Roman) 
                        {
                        }
                        else 
                        {
                            blk.Add(li);
                            continue;
                        }
                    }
                    fr = new FragToken(li.BeginToken, li.EndToken) { Itok = li };
                    childs.Add(fr);
                    fr.Kind = Pullenti.Ner.Instrument.InstrumentKind.Chapter;
                    NumberingHelper.CreateNumber(fr, li);
                    if (li.NumEndToken != li.EndToken && li.NumEndToken != null) 
                    {
                        if (li.HasManySpecChars) 
                            fr.Children.Add(new FragToken(li.NumEndToken.Next, li.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Content, Itok = li });
                        else 
                        {
                            FragToken frName = new FragToken(li.NumEndToken.Next, li.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Name, DefVal2 = true, Itok = li };
                            fr.Children.Add(frName);
                            fr.Name = FragToken.GetRestoredNameMT(frName, false);
                            i = correctName(fr, frName, lines, i);
                        }
                    }
                    else if (isContractStruct) 
                    {
                        FragToken frName = new FragToken(li.BeginToken, li.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Name, DefVal2 = true, Itok = li };
                        fr.Children.Add(frName);
                        fr.Name = FragToken.GetRestoredNameMT(frName, false);
                    }
                    continue;
                }
                else if (n >= nums.Count && li.TitleTyp != InstrToken1.StdTitleType.Undefined) 
                {
                    fr = new FragToken(li.BeginToken, li.EndToken) { Itok = li };
                    fr.Kind = childs[childs.Count - 1].Kind;
                    childs.Add(fr);
                    FragToken frName = new FragToken(li.BeginToken, li.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Name, DefVal2 = true, Itok = li };
                    fr.Children.Add(frName);
                    fr.Name = FragToken.GetRestoredNameMT(frName, false);
                    i = correctName(fr, frName, lines, i);
                    continue;
                }
                if (blk.Count == 0 && li.HasManySpecChars) 
                    err++;
                blk.Add(li);
            }
            int coef = -err;
            for (int i = 0; i < childs.Count; i++) 
            {
                FragToken chap = childs[i];
                if (i == 0 && chap.Number == 0 && chap.LengthChar > 1000) 
                    coef -= 1;
                else 
                {
                    string nam = chap.Name;
                    if (nam == null) 
                    {
                        if (i == 0 && chap.Kind == Pullenti.Ner.Instrument.InstrumentKind.Content) 
                        {
                        }
                        else 
                            coef--;
                    }
                    else if (nam.Length > 300) 
                        coef -= (nam.Length / 300);
                    else 
                    {
                        coef += 1;
                        int len = chap.LengthChar - nam.Length;
                        if (len > 200) 
                            coef += 1;
                        else if (chap.Children.Count < 3) 
                            coef--;
                        if (root.Kind == Pullenti.Ner.Instrument.InstrumentKind.Section) 
                            coef += 1;
                    }
                }
                foreach (FragToken ch in chap.Children) 
                {
                    if (ch.Kind == Pullenti.Ner.Instrument.InstrumentKind.Name) 
                    {
                        if (ch.EndToken.IsCharOf(":;")) 
                            coef -= 2;
                        break;
                    }
                    if (ch.Number == 0) 
                        continue;
                    if (ch.Itok == null) 
                        break;
                    break;
                }
            }
            if (coef < 3) 
            {
                if (err > 2) 
                    return true;
                return false;
            }
            root.Children.AddRange(childs);
            if (topDoc != null && topDoc.m_Doc != null && topDoc.m_Doc.Typ != null) 
            {
                string ty = topDoc.m_Doc.Typ;
                if (DocTyp == Pullenti.Ner.Decree.DecreeKind.Contract) 
                {
                    bool ok = true;
                    foreach (FragToken ch in childs) 
                    {
                        if (ch.Kind == Pullenti.Ner.Instrument.InstrumentKind.Chapter) 
                        {
                            foreach (FragToken chh in ch.Children) 
                            {
                                if (chh.Kind == Pullenti.Ner.Instrument.InstrumentKind.Clause) 
                                    ok = false;
                            }
                        }
                    }
                    if (ok) 
                    {
                        foreach (FragToken ch in childs) 
                        {
                            if (ch.Kind == Pullenti.Ner.Instrument.InstrumentKind.Chapter) 
                                ch.Kind = Pullenti.Ner.Instrument.InstrumentKind.Clause;
                        }
                    }
                }
            }
            return true;
        }
        bool _addForm(FragToken root, InstrToken1 li)
        {
            if (li.SubItems == null || li.Numbers.Count == 0) 
                return false;
            FragToken fr = new FragToken(li.BeginToken, li.SubItems[li.SubItems.Count - 1].EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Form, Itok = li };
            NumberingHelper.CreateNumber(fr, li);
            int i = 0;
            FragToken frName = null;
            if ((li.NumEndToken.EndChar + 10) < li.EndChar) 
                frName = new FragToken(li.NumEndToken.Next, li.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Name, DefVal2 = true, Itok = li };
            else if (li.SubItems[0].HasManySpecChars) 
            {
            }
            else 
            {
                frName = new FragToken(li.SubItems[0].BeginToken, li.SubItems[0].EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Name, DefVal2 = true, Itok = li.SubItems[0] };
                i = 1;
            }
            if (frName != null) 
            {
                fr.Children.Add(frName);
                fr.Name = FragToken.GetRestoredNameMT(frName, false);
                i = correctName(fr, frName, li.SubItems, 1);
            }
            root.Children.Add(fr);
            if (i < li.SubItems.Count) 
                fr.Children.Add(new FragToken(li.SubItems[i].BeginToken, fr.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Content });
            return true;
        }
        void _addCommentOrEdition(FragToken fr, InstrToken1 li)
        {
            if (li.Typ == InstrToken1.Types.Comment) 
                fr.Children.Add(new FragToken(li.BeginToken, li.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Comment, Itok = li });
            else if (li.Typ == InstrToken1.Types.Editions) 
            {
                FragToken edt = new FragToken(li.BeginToken, li.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Editions, Itok = li };
                fr.Children.Add(edt);
                edt.Referents = new List<Pullenti.Ner.Referent>();
                for (Pullenti.Ner.Token tt = li.BeginToken; tt != null; tt = tt.Next) 
                {
                    if (tt.EndChar > li.EndToken.EndChar) 
                        break;
                    Pullenti.Ner.Decree.DecreeReferent dr = tt.GetReferent() as Pullenti.Ner.Decree.DecreeReferent;
                    if (dr != null) 
                    {
                        if (!edt.Referents.Contains(dr)) 
                            edt.Referents.Add(dr);
                    }
                }
            }
        }
        void _analizeContentWithoutContainers(FragToken root, List<InstrToken1> lines, bool isSubitem, bool isPreamble = false, bool isKodex = false)
        {
            if (root.Kind == Pullenti.Ner.Instrument.InstrumentKind.Chapter) 
            {
            }
            if (root.Kind == Pullenti.Ner.Instrument.InstrumentKind.Clause || ((root.Kind == Pullenti.Ner.Instrument.InstrumentKind.Chapter && DocTyp == Pullenti.Ner.Decree.DecreeKind.Contract))) 
            {
                if (root.Number == 8) 
                {
                }
                while (lines.Count > 0) 
                {
                    if (lines[0].Typ == InstrToken1.Types.Comment || lines[0].Typ == InstrToken1.Types.Editions) 
                    {
                        this._addCommentOrEdition(root, lines[0]);
                        lines.RemoveAt(0);
                    }
                    else 
                        break;
                }
                if (lines.Count == 0) 
                    return;
                if ((lines.Count > 2 && lines[0].Numbers.Count == 0 && lines[0].EndToken.IsCharOf(":")) && lines[1].Numbers.Count > 0) 
                {
                }
                if (lines[0].Numbers.Count == 0 && DocTyp != Pullenti.Ner.Decree.DecreeKind.Contract) 
                {
                    List<FragToken> parts = new List<FragToken>();
                    List<InstrToken1> tmp = new List<InstrToken1>();
                    FragToken part = null;
                    for (int ii = 0; ii < lines.Count; ii++) 
                    {
                        InstrToken1 li = lines[ii];
                        if ((ii > 0 && li.Numbers.Count == 0 && li.Typ != InstrToken1.Types.Editions) && li.Typ != InstrToken1.Types.Comment && part != null) 
                        {
                            if (Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(li.BeginToken)) 
                            {
                                bool end = true;
                                for (int j = ii - 1; j >= 0; j--) 
                                {
                                    if (lines[j].Typ != InstrToken1.Types.Comment && lines[j].Typ != InstrToken1.Types.Editions) 
                                    {
                                        Pullenti.Ner.Token tt = lines[j].EndToken;
                                        if (!tt.IsCharOf(".")) 
                                        {
                                            if (tt.NewlinesAfterCount < 2) 
                                                end = false;
                                            else if (tt.IsCharOf(":,;")) 
                                                end = false;
                                        }
                                        break;
                                    }
                                }
                                if (end) 
                                {
                                    this._analizeContentWithoutContainers(part, tmp, false, false, isKodex);
                                    tmp.Clear();
                                    part = null;
                                }
                            }
                        }
                        if (part == null) 
                        {
                            part = new FragToken(li.BeginToken, li.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.ClausePart, Number = parts.Count + 1 };
                            parts.Add(part);
                        }
                        if (li.EndChar > part.EndChar) 
                            part.EndToken = li.EndToken;
                        tmp.Add(li);
                    }
                    if (part != null && tmp.Count > 0) 
                        this._analizeContentWithoutContainers(part, tmp, false, false, isKodex);
                    bool ok = true;
                    if (root.Kind != Pullenti.Ner.Instrument.InstrumentKind.Clause) 
                    {
                        int num = 0;
                        int tot = 0;
                        foreach (FragToken p in parts) 
                        {
                            foreach (FragToken ch in p.Children) 
                            {
                                if (ch.Number > 0) 
                                    num++;
                                tot++;
                            }
                        }
                        if ((tot / 2) > num) 
                            ok = false;
                    }
                    else 
                    {
                        List<InstrToken1> nums1 = NumberingHelper.ExtractMainSequence(lines, false, DocTyp != Pullenti.Ner.Decree.DecreeKind.Contract);
                        if (nums1 != null && nums1.Count > 1 && (lines.IndexOf(nums1[0]) < 2)) 
                        {
                            int ii = lines.IndexOf(nums1[nums1.Count - 1]);
                            if (ii >= (lines.Count - (((lines.Count * 3) / nums1.Count)))) 
                                ok = false;
                        }
                    }
                    if (ok) 
                    {
                        foreach (FragToken p in parts) 
                        {
                            NumberingHelper.CorrectChildNumbers(root, p.Children);
                        }
                        if (parts.Count > 1) 
                        {
                            root.Children.AddRange(parts);
                            return;
                        }
                        else if (parts.Count == 1) 
                        {
                            root.Children.AddRange(parts[0].Children);
                            return;
                        }
                    }
                }
            }
            if (root.Number == 11 && root.SubNumber == 2) 
            {
            }
            List<FragToken> notices = new List<FragToken>();
            for (int ii = 0; ii < lines.Count; ii++) 
            {
                if (lines[ii].Typ == InstrToken1.Types.Notice) 
                {
                    InstrToken1 li = lines[ii];
                    if (li.Numbers.Count == 0) 
                    {
                        if (((((ii + 1) < lines.Count) && lines[ii + 1].Numbers.Count == 1 && lines[ii + 1].Numbers[0] == "1") && (li.BeginToken is Pullenti.Ner.TextToken) && (li.BeginToken as Pullenti.Ner.TextToken).Term == "ПРИМЕЧАНИЯ") && (li.LengthChar < 13)) 
                        {
                            List<InstrToken1> lines2 = new List<InstrToken1>(lines);
                            lines2.RemoveRange(0, ii + 1);
                            for (int jj = 0; jj < lines2.Count; jj++) 
                            {
                                if (lines2[jj].Typ == InstrToken1.Types.Table || lines2[jj].BeginToken.IsTableControlChar) 
                                {
                                    lines2.RemoveRange(jj, lines2.Count - jj);
                                    break;
                                }
                            }
                            FragToken root0 = new FragToken(li.BeginToken, li.EndToken);
                            this._analizeContentWithoutContainers(root0, lines2, true, false, false);
                            if (root0.Children.Count > 0) 
                            {
                                foreach (FragToken ch in root0.Children) 
                                {
                                    notices.Add(ch);
                                    ch.Kind = Pullenti.Ner.Instrument.InstrumentKind.Notice;
                                    foreach (FragToken chh in ch.Children) 
                                    {
                                        if (chh.Kind == Pullenti.Ner.Instrument.InstrumentKind.Subitem) 
                                            chh.Kind = Pullenti.Ner.Instrument.InstrumentKind.Item;
                                    }
                                }
                            }
                            lines.RemoveRange(ii, lines2.Count);
                        }
                        else 
                        {
                            li = lines[ii];
                            FragToken not = new FragToken(li.BeginToken, li.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Notice, Itok = li };
                            notices.Add(not);
                            if (li.NumBeginToken != null && li.BeginToken != li.NumBeginToken) 
                                not.Children.Add(new FragToken(li.BeginToken, li.NumBeginToken.Previous) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Keyword, DefVal2 = true });
                            if (li.Numbers.Count > 0) 
                                NumberingHelper.CreateNumber(not, li);
                            List<InstrToken1> li0 = new List<InstrToken1>();
                            InstrToken1 num = null;
                            for (int jj = ii + 1; jj < lines.Count; jj++) 
                            {
                                InstrToken1 li1 = lines[jj];
                                if (li1.Typ == InstrToken1.Types.Notice) 
                                    break;
                                else if (li1.Cnum == null) 
                                    li0.Add(li1);
                                else 
                                {
                                    if (num == null) 
                                    {
                                        if (!li1.Cnum.IsOne) 
                                            break;
                                    }
                                    else if (!num.CheckNext(li1, false)) 
                                        break;
                                    num = li1;
                                    li0.Add(li1);
                                }
                            }
                            if (not.Children.Count > 0) 
                                not.Children.Add(new FragToken(li.NumEndToken ?? li.BeginToken, li.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Content, Itok = li });
                            if (li0.Count > 0) 
                            {
                                this._analizeContentWithoutContainers(not, li0, false, false, false);
                                not.EndToken = li0[li0.Count - 1].EndToken;
                            }
                            lines.RemoveRange(ii, li0.Count + 1);
                            ii--;
                        }
                    }
                }
            }
            List<InstrToken1> nums = NumberingHelper.ExtractMainSequence(lines, DocTyp != Pullenti.Ner.Decree.DecreeKind.Contract || TopDoc.Kind == Pullenti.Ner.Instrument.InstrumentKind.Appendix, DocTyp != Pullenti.Ner.Decree.DecreeKind.Contract);
            if (lines.Count > 5) 
            {
            }
            if (isKodex && nums != null) 
            {
                int errCou = 0;
                foreach (InstrToken1 nu in nums) 
                {
                    if (nu.NumSuffix != null && nu.NumSuffix != ")" && nu.NumSuffix != ".") 
                        errCou++;
                }
                if (errCou > 0) 
                {
                    if (errCou > (nums.Count / 2)) 
                        nums = null;
                }
            }
            if (nums == null) 
            {
                FragToken last = (root.Children.Count > 0 ? root.Children[root.Children.Count - 1] : null);
                foreach (InstrToken1 li in lines) 
                {
                    if (li.Typ == InstrToken1.Types.Comment || li.Typ == InstrToken1.Types.Editions) 
                    {
                        this._addCommentOrEdition(root, li);
                        last = null;
                        continue;
                    }
                    if (li.Typ == InstrToken1.Types.Form) 
                    {
                        if (this._addForm(root, li)) 
                            continue;
                    }
                    if (li.Typ == InstrToken1.Types.Index) 
                    {
                        FragToken ind = new FragToken(li.BeginToken, li.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Index, Itok = li };
                        root.Children.Add(ind);
                        last = null;
                        Pullenti.Ner.Token tt = li.BeginToken;
                        if (!li.IndexNoKeyword) 
                        {
                            for (; tt != null && tt.EndChar <= li.EndChar; tt = tt.Next) 
                            {
                                if (tt.IsNewlineAfter) 
                                {
                                    ind.Children.Add(new FragToken(li.BeginToken, tt) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Name, DefVal = true });
                                    tt = tt.Next;
                                    break;
                                }
                            }
                        }
                        bool isTab = false;
                        for (; tt != null && tt.EndChar <= li.EndChar; tt = tt.Next) 
                        {
                            InstrToken1 it1 = InstrToken1.Parse(tt, true, null, 0, null, false, 0, false, true);
                            if (it1 == null) 
                                break;
                            if ((!isTab && it1.EndChar == li.EndChar && tt.IsTableControlChar) && it1.LengthChar > 100) 
                            {
                                InstrToken1 it2 = InstrToken1.Parse(tt.Next, true, null, 0, null, false, 0, false, true);
                                if (it2 == null) 
                                    break;
                                it1 = it2;
                                tt = tt.Next;
                            }
                            if (it1.Value == "СТР") 
                            {
                                tt = it1.EndToken;
                                continue;
                            }
                            if (tt.GetReferent() is Pullenti.Ner.Decree.DecreePartReferent) 
                            {
                                tt = tt.Kit.DebedToken(tt);
                                it1 = InstrToken1.Parse(tt, true, null, 0, null, false, 0, false, false);
                            }
                            if (it1.Typ == InstrToken1.Types.Appendix && !it1.IsNewlineAfter) 
                            {
                                for (Pullenti.Ner.Token ttt = it1.EndToken; ttt != null; ttt = ttt.Next) 
                                {
                                    if (ttt.IsTableControlChar || ttt.IsNewlineBefore) 
                                        break;
                                    it1.EndToken = ttt;
                                }
                            }
                            FragToken indItem = new FragToken(tt, it1.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.IndexItem };
                            ind.Children.Add(indItem);
                            FragToken nam = null;
                            if (it1.NumEndToken != null && it1.NumEndToken != it1.EndToken) 
                            {
                                if (it1.BeginToken != it1.NumBeginToken) 
                                    indItem.Children.Add(new FragToken(it1.BeginToken, it1.NumBeginToken.Previous) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Keyword, DefVal2 = true });
                                NumberingHelper.CreateNumber(indItem, it1);
                                indItem.Children.Add((nam = new FragToken(it1.NumEndToken.Next, it1.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Name, DefVal2 = true }));
                                InstrToken1 it2 = InstrToken1.Parse(it1.EndToken.Next, true, null, 0, null, false, 0, false, true);
                                if (((it2 != null && it2.Typ != InstrToken1.Types.Appendix && (it1.EndToken.Next is Pullenti.Ner.TextToken)) && it2.Numbers.Count == 0 && it2.TitleTyp == InstrToken1.StdTitleType.Undefined) && !it1.EndToken.Next.IsTableControlChar) 
                                {
                                    InstrToken1 it3 = InstrToken1.Parse(it2.EndToken.Next, true, null, 0, null, false, 0, false, true);
                                    if (it3 != null && it3.Numbers.Count > 0) 
                                    {
                                        nam.EndToken = it2.EndToken;
                                        nam.DefVal2 = true;
                                        indItem.EndToken = (it1.EndToken = it2.EndToken);
                                    }
                                }
                            }
                            else 
                                indItem.Children.Add((nam = new FragToken(it1.BeginToken, it1.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Name, DefVal2 = true }));
                            indItem.Name = FragToken.GetRestoredNameMT(nam, true);
                            string val = nam.Value as string;
                            if (val != null) 
                            {
                                while (val.Length > 4) 
                                {
                                    char ch = val[val.Length - 1];
                                    if ((ch == '.' || ch == '-' || char.IsDigit(ch)) || char.IsWhiteSpace(ch) || ch == ((char)7)) 
                                        val = val.Substring(0, val.Length - 1);
                                    else 
                                        break;
                                }
                                nam.Value = val;
                            }
                            tt = it1.EndToken;
                        }
                        continue;
                    }
                    if (last != null && last.Kind == Pullenti.Ner.Instrument.InstrumentKind.Content) 
                        last.EndToken = li.EndToken;
                    else 
                        root.Children.Add((last = new FragToken(li.BeginToken, li.EndToken) { Kind = (root.Kind == Pullenti.Ner.Instrument.InstrumentKind.Footnote ? Pullenti.Ner.Instrument.InstrumentKind.Indention : Pullenti.Ner.Instrument.InstrumentKind.Content), Itok = li }));
                }
                if (!isPreamble) 
                {
                    if ((root.Children.Count == 1 && root.Children[0].Kind == Pullenti.Ner.Instrument.InstrumentKind.Content && root.Kind == Pullenti.Ner.Instrument.InstrumentKind.Content) && ((root.Children[0].Itok == null || !root.Children[0].Itok.HasChanges))) 
                    {
                        if (root.Itok == null) 
                            root.Itok = root.Children[0].Itok;
                        root.Children.Clear();
                    }
                    else if (root.Children.Count == 1 && root.Children[0].Kind == Pullenti.Ner.Instrument.InstrumentKind.Comment && root.Kind == Pullenti.Ner.Instrument.InstrumentKind.Content) 
                    {
                        root.Children.Clear();
                        root.Kind = Pullenti.Ner.Instrument.InstrumentKind.Comment;
                    }
                }
                root.Children.AddRange(notices);
                return;
            }
            if (isSubitem) 
            {
            }
            int n = 0;
            FragToken fr = null;
            List<InstrToken1> blk = new List<InstrToken1>();
            int i;
            for (i = 0; i < lines.Count; i++) 
            {
                if (lines[i] == nums[0]) 
                    break;
                else 
                    blk.Add(lines[i]);
            }
            if (blk.Count > 0) 
                this._analizeContentWithoutContainers(root, blk, false, true, isKodex);
            for (; i < lines.Count; i++) 
            {
                InstrToken1 li = lines[i];
                int j;
                blk.Clear();
                n++;
                for (j = i + 1; j < lines.Count; j++) 
                {
                    if ((n < nums.Count) && lines[j] == nums[n]) 
                        break;
                    else if (n >= nums.Count && lines[j].TitleTyp != InstrToken1.StdTitleType.Undefined && lines[j].AllUpper) 
                        break;
                    else 
                        blk.Add(lines[j]);
                }
                fr = new FragToken(li.BeginToken, li.EndToken) { Itok = li };
                root.Children.Add(fr);
                fr.Kind = (root.Kind == Pullenti.Ner.Instrument.InstrumentKind.Notice ? Pullenti.Ner.Instrument.InstrumentKind.Item : (li.Typ == InstrToken1.Types.Notice ? Pullenti.Ner.Instrument.InstrumentKind.Notice : (isSubitem ? Pullenti.Ner.Instrument.InstrumentKind.Subitem : Pullenti.Ner.Instrument.InstrumentKind.Item)));
                NumberingHelper.CreateNumber(fr, li);
                if (li.NumEndToken != li.EndToken && li.NumEndToken != null) 
                    fr.Children.Add(new FragToken(li.NumEndToken.Next, li.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Content, Itok = li });
                else if (li.TitleTyp != InstrToken1.StdTitleType.Undefined && li.AllUpper) 
                {
                    fr.Kind = Pullenti.Ner.Instrument.InstrumentKind.Tail;
                    fr.Children.Add(new FragToken(li.BeginToken, li.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Name, DefVal = true });
                }
                if (blk.Count > 0) 
                {
                    fr.EndToken = blk[blk.Count - 1].EndToken;
                    this._analizeContentWithoutContainers(fr, blk, true, false, isKodex);
                }
                i = j - 1;
            }
            NumberingHelper.CorrectChildNumbers(root, root.Children);
            if (notices.Count > 0 && notices[notices.Count - 1].EndChar > root.EndChar) 
                root.EndToken = notices[notices.Count - 1].EndToken;
            _insertNotices(root, notices);
            if (notices.Count > 0) 
                root.Children.AddRange(notices);
        }
        static void _insertNotices(FragToken root, List<FragToken> notices)
        {
            if (root.Children.Count == 0 || notices.Count == 0) 
                return;
            if (root.Children.Count > 6) 
            {
            }
            for (int ii = notices.Count - 1; ii >= 0; ii--) 
            {
                FragToken not = notices[ii];
                for (int jj = 0; jj < (root.Children.Count - 1); jj++) 
                {
                    if ((root.Children[jj].EndChar < not.BeginChar) && (not.EndChar < root.Children[jj + 1].BeginChar)) 
                    {
                        root.Children.Insert(jj + 1, not);
                        notices.RemoveAt(ii);
                        break;
                    }
                }
            }
            if (notices.Count > 0) 
            {
                foreach (FragToken ch in root.Children) 
                {
                    _insertNotices(ch, notices);
                }
            }
        }
        static List<InstrToken1> _extractDirectiveSequence(List<InstrToken1> lines)
        {
            List<InstrToken1> res = new List<InstrToken1>();
            for (int i = 0; i < lines.Count; i++) 
            {
                if (lines[i].Typ == InstrToken1.Types.Directive) 
                {
                    int j;
                    for (j = i - 1; j >= 0; j--) 
                    {
                        InstrToken1 li = lines[j];
                        if (li.Typ == InstrToken1.Types.FirstLine) 
                        {
                            j--;
                            break;
                        }
                        if (li.BeginToken.IsValue("РУКОВОДСТВУЯСЬ", null) || li.BeginToken.IsValue("ИССЛЕДОВАВ", null)) 
                        {
                            j--;
                            break;
                        }
                        if (li.BeginToken.IsValue("НА", null) && li.BeginToken.Next != null && li.BeginToken.Next.IsValue("ОСНОВАНИЕ", null)) 
                        {
                            j--;
                            break;
                        }
                        if (li.Numbers.Count > 0) 
                            break;
                        if (li.Typ == InstrToken1.Types.Comment) 
                            continue;
                        if (li.Typ == InstrToken1.Types.Line) 
                            continue;
                        break;
                    }
                    res.Add(lines[j + 1]);
                }
            }
            if (res.Count == 0) 
                return null;
            if (res[0] != lines[0]) 
                res.Insert(0, lines[0]);
            return res;
        }
        void _analizeContentWithDirectives(FragToken root, List<InstrToken1> lines, bool isJus)
        {
            List<InstrToken1> dirSeq = _extractDirectiveSequence(lines);
            if (dirSeq == null) 
            {
                this._analizeContentWithoutContainers(root, lines, false, false, false);
                return;
            }
            if (dirSeq.Count > 1) 
            {
            }
            List<FragToken> parts = new List<FragToken>();
            int n = 0;
            int j;
            for (int i = 0; i < lines.Count; i++) 
            {
                if (lines[i] == dirSeq[n]) 
                {
                    List<InstrToken1> blk = new List<InstrToken1>();
                    for (j = i; j < lines.Count; j++) 
                    {
                        if (((n + 1) < dirSeq.Count) && dirSeq[n + 1] == lines[j]) 
                            break;
                        else 
                            blk.Add(lines[j]);
                    }
                    FragToken fr = this._createDirectivePart(blk);
                    if (fr != null) 
                        parts.Add(fr);
                    i = j - 1;
                    n++;
                }
            }
            if (parts.Count == 0) 
                return;
            if (parts.Count == 1 && parts[0].Children.Count > 0) 
            {
                root.Children.AddRange(parts[0].Children);
                return;
            }
            if (parts.Count > 2 || ((parts.Count > 1 && isJus))) 
            {
                if (parts[0].Name == null && parts[parts.Count - 1].Name != null) 
                {
                    if (parts[1].Name == "МОТИВИРОВОЧНАЯ" && !parts[0].IsNewlineAfter) 
                    {
                        parts[0].Children.AddRange(parts[1].Children);
                        parts[0].Name = parts[1].Name;
                        parts.RemoveAt(1);
                        if (parts[0].Children.Count > 1 && parts[0].Children[0].Kind == Pullenti.Ner.Instrument.InstrumentKind.Content && parts[0].Children[1].Kind == Pullenti.Ner.Instrument.InstrumentKind.Preamble) 
                        {
                            parts[0].Children[1].BeginToken = parts[0].Children[0].BeginToken;
                            parts[0].Children.RemoveAt(0);
                        }
                    }
                    else 
                    {
                        parts[0].Name = "ВВОДНАЯ";
                        parts[0].Kind = Pullenti.Ner.Instrument.InstrumentKind.DocPart;
                        if (parts[0].Children.Count == 0) 
                            parts[0].Children.Add(new FragToken(parts[0].BeginToken, parts[0].EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Content, Itok = parts[0].Itok });
                    }
                }
                for (int i = 0; i < (parts.Count - 1); i++) 
                {
                    if (parts[i].Name == "МОТИВИРОВОЧНАЯ" && parts[i + 1].Name == null) 
                    {
                        parts[i].Children.AddRange(parts[i + 1].Children);
                        parts.RemoveAt(i + 1);
                        i--;
                    }
                }
                bool hasNull = false;
                foreach (FragToken p in parts) 
                {
                    if (p.Name == null) 
                        hasNull = true;
                }
                if (!hasNull) 
                {
                    root.Children.AddRange(parts);
                    return;
                }
            }
            foreach (FragToken p in parts) 
            {
                if (p.Children.Count > 0) 
                    root.Children.AddRange(p.Children);
                else 
                    root.Children.Add(p);
            }
        }
        FragToken _createDirectivePart(List<InstrToken1> lines)
        {
            FragToken res = new FragToken(lines[0].BeginToken, lines[lines.Count - 1].EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.DocPart };
            List<InstrToken1> head = new List<InstrToken1>();
            int i;
            for (i = 0; i < lines.Count; i++) 
            {
                if (lines[i].Typ == InstrToken1.Types.Directive) 
                    break;
                else 
                    head.Add(lines[i]);
            }
            if (i >= lines.Count) 
            {
                this._analizeContentWithoutContainers(res, lines, false, false, false);
                return res;
            }
            if (head.Count > 0) 
            {
                FragToken frHead = new FragToken(head[0].BeginToken, head[head.Count - 1].EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Content };
                this._analizeContentWithoutContainers(frHead, head, false, false, false);
                res.Children.Add(frHead);
            }
            if (res.Children.Count == 1 && res.Children[0].Kind == Pullenti.Ner.Instrument.InstrumentKind.Content) 
                res.Children[0].Kind = Pullenti.Ner.Instrument.InstrumentKind.Preamble;
            res.Children.Add(new FragToken(lines[i].BeginToken, lines[i].EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Directive, Value = lines[i].Value, Itok = lines[i] });
            string vvv = lines[i].Value;
            if (vvv == "УСТАНОВЛЕНИЕ" || vvv == "ВСТАНОВЛЕННЯ") 
                res.Name = "МОТИВИРОВОЧНАЯ";
            else if (((((vvv == "ПОСТАНОВЛЕНИЕ" || vvv == "ОПРЕДЕЛЕНИЕ" || vvv == "ПРИГОВОР") || vvv == "ПРИКАЗ" || vvv == "РЕШЕНИЕ") || vvv == "ПОСТАНОВА" || vvv == "ВИЗНАЧЕННЯ") || vvv == "ВИРОК" || vvv == "НАКАЗ") || vvv == "РІШЕННЯ") 
                res.Name = "РЕЗОЛЮТИВНАЯ";
            lines.RemoveRange(0, i + 1);
            if (lines.Count > 0) 
                this._analizeContentWithoutContainers(res, lines, false, false, false);
            return res;
        }
        void _analizeSections(FragToken root)
        {
            for (int k = 0; k < 2; k++) 
            {
                List<int> secs = new List<int>();
                List<int> items = new List<int>();
                foreach (FragToken ch in root.Children) 
                {
                    if (ch.Kind == Pullenti.Ner.Instrument.InstrumentKind.Chapter || ch.Kind == Pullenti.Ner.Instrument.InstrumentKind.Clause) 
                    {
                        if (ch.Number == 0 || (ch.Children.Count < 2)) 
                            return;
                        List<FragToken> newChilds = new List<FragToken>();
                        int i = 0;
                        for (; i < ch.Children.Count; i++) 
                        {
                            if (ch.Children[i].Kind != Pullenti.Ner.Instrument.InstrumentKind.Number && ch.Children[i].Kind != Pullenti.Ner.Instrument.InstrumentKind.Name && ch.Children[i].Kind != Pullenti.Ner.Instrument.InstrumentKind.Keyword) 
                                break;
                            else 
                                newChilds.Add(ch.Children[i]);
                        }
                        if (i >= ch.Children.Count) 
                            return;
                        FragToken sect = null;
                        if (ch.Children[i].Kind != Pullenti.Ner.Instrument.InstrumentKind.Content) 
                        {
                            if (ch.Children[i].Kind != Pullenti.Ner.Instrument.InstrumentKind.Item) 
                                return;
                        }
                        else 
                        {
                            sect = new FragToken(ch.Children[i].BeginToken, ch.Children[i].EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Section, DefVal2 = true };
                            sect.Name = sect.Value as string;
                            sect.Value = null;
                            sect.Children.Add(new FragToken(sect.BeginToken, sect.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Name });
                            newChilds.Add(sect);
                            if ((ch.Children[i].WhitespacesBeforeCount < 15) || (ch.Children[i].WhitespacesAfterCount < 15)) 
                                return;
                            i++;
                            if (((i + 1) < ch.Children.Count) && ch.Children[i].Kind == Pullenti.Ner.Instrument.InstrumentKind.Comment) 
                                i++;
                        }
                        int j;
                        int its = 0;
                        for (j = i; j < ch.Children.Count; j++) 
                        {
                            if (ch.Children[j].Kind != Pullenti.Ner.Instrument.InstrumentKind.Item) 
                                return;
                            its++;
                            if (sect != null) 
                            {
                                sect.Children.Add(ch.Children[j]);
                                sect.EndToken = ch.Children[j].EndToken;
                            }
                            else 
                                newChilds.Add(ch.Children[j]);
                            if ((ch.Children[j].WhitespacesAfterCount < 15) || j == (ch.Children.Count - 1)) 
                                continue;
                            FragToken la = _getLastChild(ch.Children[j]);
                            if (la.WhitespacesAfterCount < 15) 
                                continue;
                            FragToken nextSect = null;
                            for (Pullenti.Ner.Token tt = la.EndToken; tt != null && tt.BeginChar > la.BeginChar; tt = tt.Previous) 
                            {
                                if (tt.IsNewlineBefore) 
                                {
                                    if (tt.Chars.IsCyrillicLetter && tt.Chars.IsAllLower) 
                                        continue;
                                    InstrToken1 it = InstrToken1.Parse(tt, true, null, 0, null, false, 0, false, false);
                                    if (it != null && it.Numbers.Count > 0) 
                                        break;
                                    if (tt.WhitespacesBeforeCount < 15) 
                                        continue;
                                    if ((tt.Previous.EndChar - la.BeginChar) < 20) 
                                        break;
                                    nextSect = new FragToken(tt, la.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Section, DefVal2 = true };
                                    nextSect.Name = nextSect.Value as string;
                                    nextSect.Value = null;
                                    nextSect.Children.Add(new FragToken(tt, la.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Name });
                                    break;
                                }
                            }
                            if (nextSect == null) 
                                continue;
                            if (sect == null) 
                                return;
                            if (k > 0) 
                            {
                                sect.EndToken = (la.EndToken = nextSect.BeginToken.Previous);
                                if (ch.Children[j].EndChar > la.EndChar) 
                                    ch.Children[j].EndToken = la.EndToken;
                            }
                            newChilds.Add(nextSect);
                            sect = nextSect;
                        }
                        if (k > 0) 
                            ch.Children = newChilds;
                        else 
                        {
                            items.Add(its);
                            secs.Add(newChilds.Count);
                        }
                    }
                }
                if (k > 0) 
                    break;
                if (secs.Count < 3) 
                    break;
                int allsecs = 0;
                int allits = 0;
                int okchapts = 0;
                for (int i = 0; i < items.Count; i++) 
                {
                    allits += items[i];
                    allsecs += secs[i];
                    if (secs[i] > 1) 
                        okchapts++;
                }
                float rr = ((float)allits) / allsecs;
                if (rr < 1.5) 
                    break;
                if (okchapts < (items.Count / 2)) 
                    break;
            }
        }
        static FragToken _getLastChild(FragToken fr)
        {
            if (fr.Children.Count == 0) 
                return fr;
            return _getLastChild(fr.Children[fr.Children.Count - 1]);
        }
        void _correctNames(FragToken root, FragToken parent)
        {
            int i;
            List<FragToken> frNams = null;
            for (i = 0; i < root.Children.Count; i++) 
            {
                FragToken ch = root.Children[i];
                if (ch.Kind != Pullenti.Ner.Instrument.InstrumentKind.Clause && ch.Kind != Pullenti.Ner.Instrument.InstrumentKind.Chapter) 
                    continue;
                if (ch.Name != null) 
                {
                    frNams = null;
                    break;
                }
                int j;
                bool namHas = false;
                for (j = 0; j < ch.Children.Count; j++) 
                {
                    FragToken chh = ch.Children[j];
                    if (chh.Kind == Pullenti.Ner.Instrument.InstrumentKind.Keyword || chh.Kind == Pullenti.Ner.Instrument.InstrumentKind.Number || chh.Kind == Pullenti.Ner.Instrument.InstrumentKind.Editions) 
                        continue;
                    if (chh.Kind == Pullenti.Ner.Instrument.InstrumentKind.Content || chh.Kind == Pullenti.Ner.Instrument.InstrumentKind.Indention || ((chh.Kind == Pullenti.Ner.Instrument.InstrumentKind.ClausePart && chh.Children.Count == 1))) 
                    {
                        if (chh.Itok == null) 
                            chh.Itok = InstrToken1.Parse(chh.BeginToken, true, null, 0, null, false, 0, false, false);
                        if (chh.Itok != null && !chh.Itok.HasVerb) 
                            namHas = true;
                    }
                    break;
                }
                if (!namHas) 
                {
                    frNams = null;
                    break;
                }
                if (frNams == null) 
                {
                    frNams = new List<FragToken>();
                    frNams.Add(ch);
                }
                else 
                {
                    if (frNams[frNams.Count - 1].Kind != ch.Kind) 
                    {
                        frNams = null;
                        break;
                    }
                    frNams.Add(ch);
                }
            }
            if (frNams != null) 
            {
                foreach (FragToken ch in frNams) 
                {
                    int j;
                    for (j = 0; j < ch.Children.Count; j++) 
                    {
                        FragToken chh = ch.Children[j];
                        if (chh.Kind == Pullenti.Ner.Instrument.InstrumentKind.Keyword || chh.Kind == Pullenti.Ner.Instrument.InstrumentKind.Number || chh.Kind == Pullenti.Ner.Instrument.InstrumentKind.Editions) 
                            continue;
                        if (chh.Kind == Pullenti.Ner.Instrument.InstrumentKind.Content || chh.Kind == Pullenti.Ner.Instrument.InstrumentKind.Indention || ((chh.Kind == Pullenti.Ner.Instrument.InstrumentKind.ClausePart && chh.Children.Count == 1))) 
                            break;
                    }
                    if (j >= ch.Children.Count) 
                        continue;
                    FragToken nam = ch.Children[j];
                    if (nam.Kind == Pullenti.Ner.Instrument.InstrumentKind.Indention || ((nam.Kind == Pullenti.Ner.Instrument.InstrumentKind.ClausePart && nam.Children.Count == 1))) 
                    {
                        nam.Number = 0;
                        int cou = 0;
                        for (int jj = j + 1; jj < ch.Children.Count; jj++) 
                        {
                            if (ch.Children[jj].Kind == nam.Kind) 
                            {
                                ch.Children[jj].Number--;
                                cou++;
                            }
                            else 
                                break;
                        }
                        if (cou == 1) 
                        {
                            for (int jj = j + 1; jj < ch.Children.Count; jj++) 
                            {
                                if (ch.Children[jj].Kind == nam.Kind) 
                                {
                                    bool empty = true;
                                    for (int k = jj + 1; k < ch.Children.Count; k++) 
                                    {
                                        if (ch.Children[k].Kind != Pullenti.Ner.Instrument.InstrumentKind.Editions && ch.Children[k].Kind != Pullenti.Ner.Instrument.InstrumentKind.Comment) 
                                        {
                                            empty = false;
                                            break;
                                        }
                                    }
                                    if (empty) 
                                    {
                                        if (ch.Children[jj].Kind == Pullenti.Ner.Instrument.InstrumentKind.Indention || ch.Children.Count == 0) 
                                        {
                                            ch.Children[jj].Kind = Pullenti.Ner.Instrument.InstrumentKind.Content;
                                            ch.Children[jj].Number = 0;
                                        }
                                        else 
                                        {
                                            FragToken ch0 = ch.Children[jj];
                                            ch.Children.RemoveAt(jj);
                                            ch.Children.InsertRange(jj, ch0.Children);
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    nam.Number = 0;
                    nam.Kind = Pullenti.Ner.Instrument.InstrumentKind.Name;
                    nam.DefVal2 = true;
                    nam.Children.Clear();
                    ch.Name = nam.Value as string;
                }
            }
            if (parent != null && parent.IsExpired) 
            {
            }
            else 
                root.CheckExpired();
            foreach (FragToken ch in root.Children) 
            {
                this._correctNames(ch, root);
            }
        }
        void _correctKodexParts(FragToken root)
        {
            if (root.Number == 2 && root.Kind == Pullenti.Ner.Instrument.InstrumentKind.Clause) 
            {
            }
            if (root.Number == 11 && root.Kind == Pullenti.Ner.Instrument.InstrumentKind.Item) 
            {
            }
            int i;
            for (i = 0; i < root.Children.Count; i++) 
            {
                Pullenti.Ner.Instrument.InstrumentKind ki = root.Children[i].Kind;
                if ((ki != Pullenti.Ner.Instrument.InstrumentKind.Keyword && ki != Pullenti.Ner.Instrument.InstrumentKind.Name && ki != Pullenti.Ner.Instrument.InstrumentKind.Number) && ki != Pullenti.Ner.Instrument.InstrumentKind.Comment && ki != Pullenti.Ner.Instrument.InstrumentKind.Editions) 
                    break;
            }
            if (i >= root.Children.Count) 
                return;
            int i0 = i;
            if (root.Kind == Pullenti.Ner.Instrument.InstrumentKind.Clause && DocTyp != Pullenti.Ner.Decree.DecreeKind.Contract) 
            {
                for (; i < root.Children.Count; i++) 
                {
                    FragToken ch = root.Children[i];
                    if (ch.Kind == Pullenti.Ner.Instrument.InstrumentKind.Item || ch.Kind == Pullenti.Ner.Instrument.InstrumentKind.Subitem || ((ch.Kind == Pullenti.Ner.Instrument.InstrumentKind.ClausePart && ch.Number > 0))) 
                    {
                        ch.Kind = Pullenti.Ner.Instrument.InstrumentKind.ClausePart;
                        foreach (FragToken chh in ch.Children) 
                        {
                            if (chh.Kind == Pullenti.Ner.Instrument.InstrumentKind.Subitem && chh.Number > 0) 
                            {
                                chh.Kind = Pullenti.Ner.Instrument.InstrumentKind.Item;
                                foreach (FragToken chhh in chh.Children) 
                                {
                                    if (chhh.Number > 0) 
                                        chhh.Kind = Pullenti.Ner.Instrument.InstrumentKind.Subitem;
                                }
                            }
                        }
                    }
                    else if (ch.Kind == Pullenti.Ner.Instrument.InstrumentKind.Content) 
                        break;
                }
            }
            if (i == i0 && root.Children[i0].Kind == Pullenti.Ner.Instrument.InstrumentKind.Content) 
            {
                for (i = i0 + 1; i < root.Children.Count; i++) 
                {
                    if (root.Children[i].Kind != Pullenti.Ner.Instrument.InstrumentKind.Editions && root.Children[i].Kind != Pullenti.Ner.Instrument.InstrumentKind.Comment) 
                        break;
                }
                if ((i < root.Children.Count) && ((((DocTyp == Pullenti.Ner.Decree.DecreeKind.Kodex || root.Kind == Pullenti.Ner.Instrument.InstrumentKind.Clause || root.Kind == Pullenti.Ner.Instrument.InstrumentKind.Item) || root.Kind == Pullenti.Ner.Instrument.InstrumentKind.Subitem || root.Kind == Pullenti.Ner.Instrument.InstrumentKind.Chapter) || root.Kind == Pullenti.Ner.Instrument.InstrumentKind.ClausePart))) 
                {
                    if (root.Children[i].Kind == Pullenti.Ner.Instrument.InstrumentKind.ListItem || root.Children[i].Kind == Pullenti.Ner.Instrument.InstrumentKind.Item || root.Children[i].Kind == Pullenti.Ner.Instrument.InstrumentKind.Subitem) 
                    {
                        int num = 1;
                        root.Children[i0].Kind = Pullenti.Ner.Instrument.InstrumentKind.Indention;
                        root.Children[i0].Number = num;
                        if (root.Children[i].Kind == Pullenti.Ner.Instrument.InstrumentKind.ListItem) 
                        {
                            for (; i < root.Children.Count; i++) 
                            {
                                if (root.Children[i].Kind == Pullenti.Ner.Instrument.InstrumentKind.ListItem) 
                                {
                                    root.Children[i].Kind = Pullenti.Ner.Instrument.InstrumentKind.Indention;
                                    root.Children[i].Number = ++num;
                                }
                                else if (root.Children[i].Kind != Pullenti.Ner.Instrument.InstrumentKind.Comment && root.Children[i].Kind != Pullenti.Ner.Instrument.InstrumentKind.Editions) 
                                    break;
                            }
                        }
                    }
                }
            }
            int inds = 1;
            for (i = i0; i < root.Children.Count; i++) 
            {
                FragToken ch = root.Children[i];
                if (ch.Kind == Pullenti.Ner.Instrument.InstrumentKind.Comment) 
                    continue;
                int err = -1;
                List<FragToken> lii = this._splitContentByIndents(ch, ref inds, out err);
                if (lii == null) 
                {
                    if (ch.Kind == Pullenti.Ner.Instrument.InstrumentKind.Editions) 
                        continue;
                    break;
                }
            }
            if (root.NumberString == "61") 
            {
            }
            if (inds > 2 && ((i >= root.Children.Count || root.Children[i].Kind != Pullenti.Ner.Instrument.InstrumentKind.Directive))) 
            {
                if (root.Number == 7 && root.Kind == Pullenti.Ner.Instrument.InstrumentKind.ClausePart) 
                {
                }
                int num = 1;
                bool errRegime = false;
                for (i = i0; i < root.Children.Count; i++) 
                {
                    FragToken chh = root.Children[i];
                    if (chh.Kind == Pullenti.Ner.Instrument.InstrumentKind.Comment) 
                        continue;
                    int err = -1;
                    List<FragToken> lii = this._splitContentByIndents(chh, ref num, out err);
                    if (lii == null) 
                    {
                        if (chh.Kind == Pullenti.Ner.Instrument.InstrumentKind.Editions) 
                            continue;
                        break;
                    }
                    if (lii.Count == 0) 
                        continue;
                    if (err > 0) 
                        errRegime = true;
                    else if (err == 0) 
                        errRegime = false;
                    if (errRegime) 
                    {
                        foreach (FragToken vv in lii) 
                        {
                            vv.IsError = true;
                        }
                    }
                    root.Children.RemoveAt(i);
                    root.Children.InsertRange(i, lii);
                    i += (lii.Count - 1);
                }
                num = 1;
                for (i = i0 + 1; i < root.Children.Count; i++) 
                {
                    FragToken ch = root.Children[i];
                    if (ch.Kind == Pullenti.Ner.Instrument.InstrumentKind.Comment || ch.Kind == Pullenti.Ner.Instrument.InstrumentKind.Editions) 
                        continue;
                    if (ch.Itok == null || ch.Itok.Numbers.Count != 1) 
                        break;
                    if (ch.Itok.FirstNumber != num) 
                        break;
                    num++;
                }
                if (num > 1 && i >= root.Children.Count) 
                {
                    for (i = i0 + 1; i < root.Children.Count; i++) 
                    {
                        FragToken ch = root.Children[i];
                        if (ch.Kind == Pullenti.Ner.Instrument.InstrumentKind.Comment || ch.Kind == Pullenti.Ner.Instrument.InstrumentKind.Editions) 
                            continue;
                        if (root.Kind == Pullenti.Ner.Instrument.InstrumentKind.ClausePart || root.Kind == Pullenti.Ner.Instrument.InstrumentKind.Clause) 
                            ch.Kind = Pullenti.Ner.Instrument.InstrumentKind.Item;
                        else if (root.Kind == Pullenti.Ner.Instrument.InstrumentKind.Item) 
                            ch.Kind = Pullenti.Ner.Instrument.InstrumentKind.Subitem;
                        else 
                            break;
                        NumberingHelper.CreateNumber(ch, ch.Itok);
                        if (ch.Children.Count == 1 && (ch.Children[0].EndChar < ch.EndChar)) 
                            ch.FillByContentChildren();
                    }
                }
            }
            for (i = 0; i < root.Children.Count; i++) 
            {
                FragToken root0 = root.Children[i];
                if (root0.Number == 0 || (root0.Children.Count < 2)) 
                    continue;
                FragToken ch = root0.Children[root0.Children.Count - 1];
                if (ch.Kind != Pullenti.Ner.Instrument.InstrumentKind.Editions) 
                    continue;
                int num = root0.Number + 1;
                int err = -1;
                List<FragToken> lii = this._splitContentByIndents(ch, ref num, out err);
                if (lii == null || lii.Count == 0 || lii[0].Kind != root0.Kind) 
                    continue;
                if (err > 0) 
                {
                    foreach (FragToken vv in lii) 
                    {
                        vv.IsError = true;
                    }
                }
                root0.Children.Remove(ch);
                root.Children.InsertRange(i + 1, lii);
                i += lii.Count;
            }
            foreach (FragToken ch in root.Children) 
            {
                this._correctKodexParts(ch);
            }
        }
        List<FragToken> _splitContentByIndents(FragToken fr, ref int num, out int err)
        {
            err = -1;
            if (fr.Kind != Pullenti.Ner.Instrument.InstrumentKind.Content && fr.Kind != Pullenti.Ner.Instrument.InstrumentKind.ListItem && fr.Kind != Pullenti.Ner.Instrument.InstrumentKind.Preamble) 
            {
                if (fr.Kind != Pullenti.Ner.Instrument.InstrumentKind.Editions) 
                    return null;
                if ((fr.BeginToken.IsValue("АБЗАЦ", null) || fr.BeginToken.IsValue("ЧАСТЬ", null) || fr.BeginToken.IsValue("ПУНКТ", null)) || fr.BeginToken.IsValue("ПОДПУНКТ", null)) 
                {
                    Pullenti.Ner.Token t = fr.BeginToken.Next;
                    Pullenti.Ner.NumberToken nnn = t as Pullenti.Ner.NumberToken;
                    if (nnn == null || nnn.IntValue == null) 
                        return null;
                    else if (nnn.IntValue.Value == num) 
                        err = 0;
                    else if (fr.BeginToken.IsValue("АБЗАЦ", null)) 
                    {
                        num = nnn.IntValue.Value;
                        err = 1;
                    }
                    else if (nnn.IntValue.Value != (num - 1)) 
                    {
                        err = 1;
                        return null;
                    }
                    int next = nnn.IntValue.Value;
                    t = t.Next;
                    if ((t != null && t.IsHiphen && (t.Next is Pullenti.Ner.NumberToken)) && (t.Next as Pullenti.Ner.NumberToken).IntValue != null) 
                    {
                        next = (t.Next as Pullenti.Ner.NumberToken).IntValue.Value;
                        t = t.Next.Next;
                        if (next <= num) 
                            return null;
                    }
                    if ((t == null || !t.IsValue("УТРАТИТЬ", "ВТРАТИТИ") || t.Next == null) || !t.Next.IsValue("СИЛА", "ЧИННІСТЬ")) 
                        return null;
                    List<FragToken> res0 = new List<FragToken>();
                    Pullenti.Ner.Instrument.InstrumentKind ki = Pullenti.Ner.Instrument.InstrumentKind.Indention;
                    if (fr.BeginToken.IsValue("ПУНКТ", null)) 
                        ki = Pullenti.Ner.Instrument.InstrumentKind.Item;
                    else if (fr.BeginToken.IsValue("ПОДПУНКТ", null)) 
                        ki = Pullenti.Ner.Instrument.InstrumentKind.Subitem;
                    else if (fr.BeginToken.IsValue("ЧАСТЬ", null)) 
                        ki = Pullenti.Ner.Instrument.InstrumentKind.ClausePart;
                    for (int i = num; i <= next; i++) 
                    {
                        res0.Add(new FragToken(fr.BeginToken, fr.EndToken) { Kind = ki, Number = i, IsExpired = true, Referents = fr.Referents });
                    }
                    num = next + 1;
                    return res0;
                }
                return new List<FragToken>();
            }
            if (fr.Children.Count > 0) 
                return null;
            if (fr.Itok == null) 
                fr.Itok = InstrToken1.Parse(fr.BeginToken, true, null, 0, null, false, 0, false, false);
            List<FragToken> res = new List<FragToken>();
            Pullenti.Ner.Token t0 = fr.BeginToken;
            for (Pullenti.Ner.Token tt = t0; tt != null && tt.EndChar <= fr.EndChar; tt = tt.Next) 
            {
                if (tt.EndChar == fr.EndChar) 
                {
                }
                else if (!tt.IsNewlineAfter) 
                    continue;
                else if (tt.IsTableControlChar) 
                    continue;
                else if (!Pullenti.Ner.Core.MiscHelper.CanBeStartOfSentence(tt.Next) && !tt.IsCharOf(":;.")) 
                {
                    if ((tt.Next != null && tt.Next.IsChar('[') && tt.Next.Next != null) && tt.Next.Next.IsValue("КАРТИНКА", null)) 
                    {
                    }
                    else if (tt.Next != null && tt.Next.IsValue("ГДЕ", null)) 
                    {
                    }
                    else if (tt.IsChar(']')) 
                    {
                    }
                    else if (tt.IsComma && tt.Previous.IsChar(']')) 
                    {
                    }
                    else if ((tt.Next != null && tt.Next.LengthChar == 1 && tt.Next.Next != null) && tt.Next.Next.IsHiphen) 
                    {
                    }
                    else 
                        continue;
                }
                FragToken re = new FragToken(t0, tt) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Indention, Number = num };
                num++;
                if (t0 == fr.BeginToken && tt == fr.EndToken) 
                    re.Itok = fr.Itok;
                if (re.Itok == null) 
                    re.Itok = InstrToken1.Parse(t0, true, null, 0, null, false, 0, false, false);
                if (res.Count > 100) 
                    return null;
                if (re.Itok.HasManySpecChars && re.Itok.IsPureHiphenLine) 
                    num--;
                else if (re.Itok.Typ == InstrToken1.Types.Footnote) 
                    num--;
                else 
                    res.Add(re);
                t0 = tt.Next;
            }
            return res;
        }
        void _analizePreamble(FragToken root)
        {
            int i;
            int cntCou = 0;
            FragToken ch = null;
            bool ok = false;
            if ((root.Children.Count > 1 && root.Children[0].Kind == Pullenti.Ner.Instrument.InstrumentKind.Content && root.Children[1].Number > 0) && root.Children[0].Children.Count > 0) 
            {
                for (i = 0; i < root.Children[0].Children.Count; i++) 
                {
                    FragToken ch2 = root.Children[0].Children[i];
                    if ((ch2.Kind != Pullenti.Ner.Instrument.InstrumentKind.Content && ch2.Kind != Pullenti.Ner.Instrument.InstrumentKind.Indention && ch2.Kind != Pullenti.Ner.Instrument.InstrumentKind.Comment) && ch2.Kind != Pullenti.Ner.Instrument.InstrumentKind.Editions) 
                        break;
                }
                if (i >= root.Children[0].Children.Count) 
                {
                    FragToken chh = root.Children[0];
                    root.Children.RemoveAt(0);
                    root.Children.InsertRange(0, chh.Children);
                }
            }
            for (i = 0; i < root.Children.Count; i++) 
            {
                ch = root.Children[i];
                if (ch.Kind == Pullenti.Ner.Instrument.InstrumentKind.Editions || ch.Kind == Pullenti.Ner.Instrument.InstrumentKind.Comment || ch.Kind == Pullenti.Ner.Instrument.InstrumentKind.Index) 
                    continue;
                if (ch.Kind == Pullenti.Ner.Instrument.InstrumentKind.Directive) 
                {
                    ok = true;
                    break;
                }
                if (ch.Itok != null && ch.Itok.HasChanges) 
                    break;
                if (ch.Kind == Pullenti.Ner.Instrument.InstrumentKind.Content && ch.Children.Count == 1 && ch.Children[0].Kind == Pullenti.Ner.Instrument.InstrumentKind.Index) 
                {
                    ch.Kind = Pullenti.Ner.Instrument.InstrumentKind.Index;
                    ch.Children = ch.Children[0].Children;
                    continue;
                }
                if (ch.Kind == Pullenti.Ner.Instrument.InstrumentKind.Content || ch.Kind == Pullenti.Ner.Instrument.InstrumentKind.Indention) 
                {
                    for (Pullenti.Ner.Token t = ch.BeginToken.Next; t != null && (t.EndChar < ch.EndChar); t = t.Next) 
                    {
                        if (t.IsNewlineBefore) 
                        {
                            if (t.Previous.IsCharOf(".:;") && t.Previous.Previous != null && ((t.Previous.Previous.IsValue("НИЖЕСЛЕДУЮЩИЙ", null) || t.Previous.Previous.IsValue("ДОГОВОР", null)))) 
                            {
                                InstrToken1 itt1 = InstrToken1.Parse(t, true, null, 0, null, false, 0, false, false);
                                if ((itt1 != null && !itt1.HasVerb && (itt1.EndChar < ch.EndChar)) && itt1.Typ != InstrToken1.Types.Appendix) 
                                {
                                    FragToken clau = new FragToken(t, ch.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Chapter };
                                    if (((i + 1) < root.Children.Count) && root.Children[i + 1].Kind == Pullenti.Ner.Instrument.InstrumentKind.Clause) 
                                        clau.Kind = Pullenti.Ner.Instrument.InstrumentKind.Clause;
                                    FragToken nam = new FragToken(t, itt1.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Name, DefVal2 = true, Itok = itt1 };
                                    clau.Children.Add(nam);
                                    clau.Name = FragToken.GetRestoredNameMT(nam, false);
                                    clau.Children.Add(new FragToken(itt1.EndToken.Next, ch.EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Content });
                                    ch.EndToken = t.Previous;
                                    root.Children.Insert(i + 1, clau);
                                }
                                break;
                            }
                        }
                    }
                    bool pream = false;
                    if (ch.BeginToken.IsValue("ПРЕАМБУЛА", null)) 
                        pream = true;
                    else if (ch.LengthChar > 1500) 
                        break;
                    cntCou++;
                    if (ch.EndToken.IsChar(':') || pream || ((ch.EndToken.Previous != null && ch.EndToken.Previous.IsValue("НИЖЕСЛЕДУЮЩИЙ", null)))) 
                    {
                        ok = true;
                        i++;
                        break;
                    }
                    continue;
                }
                break;
            }
            if (cntCou == 0 || cntCou > 3 || i >= root.Children.Count) 
                return;
            if (ch.Number > 0) 
                ok = true;
            if (!ok) 
                return;
            if (cntCou == 1) 
            {
                for (int j = 0; j < i; j++) 
                {
                    if (root.Children[j].Kind == Pullenti.Ner.Instrument.InstrumentKind.Content || root.Children[j].Kind == Pullenti.Ner.Instrument.InstrumentKind.Indention) 
                    {
                        root.Children[j].Kind = Pullenti.Ner.Instrument.InstrumentKind.Preamble;
                        if (root.Children[j].Children.Count == 1 && root.Children[j].Children[0].Kind == Pullenti.Ner.Instrument.InstrumentKind.Index) 
                        {
                            root.Children[j].Kind = Pullenti.Ner.Instrument.InstrumentKind.Index;
                            root.Children[j].Children = root.Children[j].Children[0].Children;
                        }
                    }
                }
            }
            else 
            {
                FragToken prm = new FragToken(root.Children[0].BeginToken, root.Children[i - 1].EndToken) { Kind = Pullenti.Ner.Instrument.InstrumentKind.Preamble };
                for (int j = 0; j < i; j++) 
                {
                    prm.Children.Add(root.Children[j]);
                }
                root.Children.RemoveRange(0, i);
                root.Children.Insert(0, prm);
            }
        }
    }
}