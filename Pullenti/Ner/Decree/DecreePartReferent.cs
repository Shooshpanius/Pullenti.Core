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

namespace Pullenti.Ner.Decree
{
    /// <summary>
    /// Сущность, представляющая ссылку на структурную часть НПА
    /// </summary>
    public class DecreePartReferent : Pullenti.Ner.Referent
    {
        public DecreePartReferent() : base(OBJ_TYPENAME)
        {
            InstanceOf = Pullenti.Ner.Decree.Internal.MetaDecreePart.GlobalMeta;
        }
        /// <summary>
        /// Имя типа сущности TypeName ("DECREEPART")
        /// </summary>
        public const string OBJ_TYPENAME = "DECREEPART";
        /// <summary>
        /// Имя атрибута - владелец (DecreeReferent)
        /// </summary>
        public const string ATTR_OWNER = "OWNER";
        /// <summary>
        /// Имя атрибута - тип локального владельца (ст.10 этого закона)
        /// </summary>
        public const string ATTR_LOCALTYP = "LOCALTYP";
        /// <summary>
        /// Имя атрибута - часть документа (например, часть 1 Налогового кодекса)
        /// </summary>
        public const string ATTR_DOCPART = "DOCPART";
        /// <summary>
        /// Имя атрибута - приложение (номер)
        /// </summary>
        public const string ATTR_APPENDIX = "APPENDIX";
        /// <summary>
        /// Имя атрибута - приложение (название)
        /// </summary>
        public const string ATTR_APPENDIXNAME = "APPENDIXNAME";
        /// <summary>
        /// Имя атрибута - приложение к приложению (номер)
        /// </summary>
        public const string ATTR_APPENDIX2 = "APPENDIX2";
        /// <summary>
        /// Имя атрибута - приложение к приложению (название)
        /// </summary>
        public const string ATTR_APPENDIX2NAME = "APPENDIX2NAME";
        /// <summary>
        /// Имя атрибута - раздел (номер)
        /// </summary>
        public const string ATTR_SECTION = "SECTION";
        /// <summary>
        /// Имя атрибута - раздел (название)
        /// </summary>
        public const string ATTR_SECTIONNAME = "SECTIONNAME";
        /// <summary>
        /// Имя атрибута - подраздел (номер)
        /// </summary>
        public const string ATTR_SUBSECTION = "SUBSECTION";
        /// <summary>
        /// Имя атрибута - подраздел (название)
        /// </summary>
        public const string ATTR_SUBSECTIONNAME = "SUBSECTIONNAME";
        /// <summary>
        /// Имя атрибута - глава (номер)
        /// </summary>
        public const string ATTR_CHAPTER = "CHAPTER";
        /// <summary>
        /// Имя атрибута - глава (название)
        /// </summary>
        public const string ATTR_CHAPTERNAME = "CHAPTERNAME";
        /// <summary>
        /// Имя атрибута - статья (номер)
        /// </summary>
        public const string ATTR_CLAUSE = "CLAUSE";
        /// <summary>
        /// Имя атрибута - статья (название)
        /// </summary>
        public const string ATTR_CLAUSENAME = "CLAUSENAME";
        /// <summary>
        /// Имя атрибута - параграф
        /// </summary>
        public const string ATTR_PARAGRAPH = "PARAGRAPH";
        /// <summary>
        /// Имя атрибута - подпараграф
        /// </summary>
        public const string ATTR_SUBPARAGRAPH = "SUBPARAGRAPH";
        /// <summary>
        /// Имя атрибута - часть статьи (не путать с частью документа!)
        /// </summary>
        public const string ATTR_PART = "PART";
        /// <summary>
        /// Имя атрибута - пункт
        /// </summary>
        public const string ATTR_ITEM = "ITEM";
        /// <summary>
        /// Имя атрибута - подпункт
        /// </summary>
        public const string ATTR_SUBITEM = "SUBITEM";
        /// <summary>
        /// Имя атрибута - абзац
        /// </summary>
        public const string ATTR_INDENTION = "INDENTION";
        /// <summary>
        /// Имя атрибута - подабзац
        /// </summary>
        public const string ATTR_SUBINDENTION = "SUBINDENTION";
        /// <summary>
        /// Имя атрибута - преамбула
        /// </summary>
        public const string ATTR_PREAMBLE = "PREAMPLE";
        /// <summary>
        /// Имя атрибута - примечание
        /// </summary>
        public const string ATTR_NOTICE = "NOTICE";
        /// <summary>
        /// Имя атрибута - сноска
        /// </summary>
        public const string ATTR_FOOTNOTE = "FOOTNOTE";
        /// <summary>
        /// Имя атрибута - формула
        /// </summary>
        public const string ATTR_FORMULA = "FORMULA";
        /// <summary>
        /// Имя атрибута - форма
        /// </summary>
        public const string ATTR_FORM = "FORM";
        /// <summary>
        /// Имя атрибута - наименование формы
        /// </summary>
        public const string ATTR_FORMNAME = "FORMNAME";
        /// <summary>
        /// Имя атрибута - лист
        /// </summary>
        public const string ATTR_LIST = "LIST";
        /// <summary>
        /// Имя атрибута - таблица
        /// </summary>
        public const string ATTR_TABLE = "TABLE";
        /// <summary>
        /// Имя атрибута - наименование таблицы
        /// </summary>
        public const string ATTR_TABLENAME = "TABLENAME";
        /// <summary>
        /// Тия атрибута - столбец таблицы
        /// </summary>
        public const string ATTR_TABLECOLUMN = "TABLECOLUMN";
        /// <summary>
        /// Тия атрибута - строка таблицы
        /// </summary>
        public const string ATTR_TABLEROW = "TABLEROW";
        /// <summary>
        /// Тия атрибута - графа таблицы (то ли строка, то ли столбец)
        /// </summary>
        public const string ATTR_TABLEITEM = "TABLEITEM";
        /// <summary>
        /// Тия атрибута - прографка таблицы
        /// </summary>
        public const string ATTR_TABLESUBITEM = "TABLESUBITEM";
        /// <summary>
        /// Имя атрибута - предложение
        /// </summary>
        public const string ATTR_SENTENCE = "SENTENCE";
        /// <summary>
        /// Имя атрибута - наименование как элемент (например, ссылка "внести изменение в наименование ст.20 ...")
        /// </summary>
        public const string ATTR_NAMEASITEM = "NAMEASITEM";
        /// <summary>
        /// Имя атрибута - подпрограмма
        /// </summary>
        public const string ATTR_SUBPROGRAM = "SUBPROGRAM";
        /// <summary>
        /// Имя атрибута - имя подпрограммы
        /// </summary>
        public const string ATTR_SUBPROGRAMNAME = "SUBPROGRAMNAME";
        /// <summary>
        /// Имя атрибута - дополнительное соглашение
        /// </summary>
        public const string ATTR_ADDAGREE = "ADDAGREE";
        /// <summary>
        /// Это что-то непонятное
        /// </summary>
        public const string ATTR_FREETEXT = "FREETEXT";
        /// <summary>
        /// Имя атрибута - страница
        /// </summary>
        public const string ATTR_PAGE = "PAGE";
        /// <summary>
        /// Имя атрибута - дополнительное описание
        /// </summary>
        public const string ATTR_DESCRIPTION = "DESCRIPTION";
        public override string ToStringEx(bool shortVariant, Pullenti.Morph.MorphLang lang = null, int lev = 0)
        {
            StringBuilder res = new StringBuilder();
            if (Freetext != null) 
                res.Append(Freetext);
            if (NameAsItem != null) 
                res.Append(" наименование");
            if (Formula != null) 
                res.AppendFormat(" формула {0}", Formula);
            if (SubIndention != null) 
                res.AppendFormat(" подабз.{0}", SubIndention);
            if (Indention != null) 
                res.AppendFormat(" абз.{0}", Indention);
            if (Footnote != null) 
                res.AppendFormat(" сноска {0}", Footnote);
            if (TableSubItem != null) 
                res.AppendFormat(" прографка \"{0}\"", TableSubItem);
            if (TableItem != null) 
            {
                if (TableItem.Length > 5) 
                    res.AppendFormat(" графа \"{0}\"", TableItem);
                else 
                    res.AppendFormat(" графа {0}", TableItem);
            }
            if (TableRow != null) 
            {
                if (TableRow.Length > 5) 
                    res.AppendFormat(" строка \"{0}\"", TableRow);
                else 
                    res.AppendFormat(" строка {0}", TableRow);
            }
            if (TableColumn != null) 
                res.AppendFormat(" столбец {0}", TableColumn);
            if (Table != null) 
            {
                res.AppendFormat(" таблица {0}", Table);
                if (TableName != null) 
                    res.AppendFormat(" \"{0}\"", TableName);
            }
            if (Sentence != null) 
                res.AppendFormat(" предложение {0}", Sentence);
            if (Notice != null) 
                res.AppendFormat(" прим.{0}", Notice);
            List<string> subs = this.GetStringValues(ATTR_SUBITEM);
            if (subs.Count > 0) 
            {
                foreach (string s in subs) 
                {
                    res.AppendFormat(" пп.{0}", s);
                }
            }
            if (Item != null) 
                res.AppendFormat(" п.{0}", Item);
            if (Part != null) 
                res.AppendFormat(" ч.{0}", Part);
            if (Preamble != null) 
                res.AppendFormat(" преамб.{0}", (Preamble == "0" ? "" : Preamble));
            if (List != null) 
                res.AppendFormat(" лист {0}", List);
            if (Form != null) 
            {
                res.AppendFormat(" форма {0}", Form);
                if (FormName != null) 
                    res.AppendFormat(" \"{0}\"", FormName);
            }
            if (Page != null) 
                res.AppendFormat(" стр.{0}", Page);
            if (Clause != null) 
            {
                res.AppendFormat(" ст.{0}", Clause);
                if (ClauseName != null) 
                    res.AppendFormat(" \"{0}\"", ClauseName);
            }
            if (SubParagraph != null) 
                res.AppendFormat(" подпар.{0}", SubParagraph);
            if (Paragraph != null) 
                res.AppendFormat(" пар.{0}", Paragraph);
            if (Chapter != null) 
            {
                res.AppendFormat(" гл.{0}", Chapter);
                if (ChapterName != null) 
                    res.AppendFormat(" \"{0}\"", ChapterName);
            }
            if (SubSection != null) 
            {
                res.AppendFormat(" подразд.{0}", SubSection);
                if (SubSectionName != null) 
                    res.AppendFormat(" \"{0}\"", SubSectionName);
            }
            if (Section != null) 
            {
                res.AppendFormat(" разд.{0}", Section);
                if (SectionName != null) 
                    res.AppendFormat(" \"{0}\"", SectionName);
            }
            if (DocPart != null) 
                res.AppendFormat(" док.часть {0}", DocPart);
            string app = Appendix;
            string app2 = Appendix2;
            if (app2 == null) 
            {
                if (app == "0") 
                    res.Append(" приложение");
                else if (app != null) 
                    res.AppendFormat(" приложение {0}", app);
                if (app != null && AppendixName != null) 
                    res.AppendFormat(" \"{0}\"", AppendixName);
            }
            else 
            {
                if (app2 == "0") 
                    res.Append(" приложение к");
                else if (app2 != null) 
                    res.AppendFormat(" приложение {0} к", app2);
                if (app == "0") 
                    res.Append(" приложению");
                else if (app != null) 
                    res.AppendFormat(" приложению {0}", app);
            }
            if (Subprogram != null) 
            {
                res.AppendFormat(" подпрограмма {0}", Subprogram);
                if (SubprogramName != null) 
                    res.AppendFormat(" \"{0}\"", SubprogramName);
            }
            if (Addagree != null) 
            {
                if (Addagree == "0") 
                    res.AppendFormat(" допсоглашение");
                else 
                    res.AppendFormat(" допсоглашение {0}", Addagree);
            }
            if (((Owner != null || res.Length > 0)) && !shortVariant) 
            {
                if (!shortVariant && Subprogram == null) 
                {
                    string s = Description;
                    if (s != null) 
                        res.AppendFormat(" ({0})", s);
                }
                if (Owner != null && (lev < 20)) 
                {
                    if (res.Length > 0) 
                        res.Append("; ");
                    res.Append(Owner.ToStringEx(shortVariant, lang, lev + 1));
                }
                else if (LocalTyp != null) 
                    res.AppendFormat("; {0}", Pullenti.Ner.Core.MiscHelper.ConvertFirstCharUpperAndOtherLower(LocalTyp));
            }
            return res.ToString().Trim();
        }
        /// <summary>
        /// Дополнительное описание (обычно после в круглых скобках)
        /// </summary>
        public string Description
        {
            get
            {
                return this.GetStringValue(ATTR_DESCRIPTION);
            }
            set
            {
                if (value != null) 
                    value = value.Trim();
                if (string.IsNullOrEmpty(value)) 
                    value = null;
                this.AddSlot(ATTR_DESCRIPTION, value, true, 0);
            }
        }
        /// <summary>
        /// Локальный тип (при ссылке на текущий документ)
        /// </summary>
        public string LocalTyp
        {
            get
            {
                return this.GetStringValue(ATTR_LOCALTYP);
            }
            set
            {
                this.AddSlot(ATTR_LOCALTYP, value, true, 0);
            }
        }
        public override Pullenti.Ner.Slot AddSlot(string attrName, object attrValue, bool clearOldValue, int statCount = 0)
        {
            string tag = null;
            if (attrValue is Pullenti.Ner.Decree.Internal.PartToken.PartValue) 
            {
                tag = (attrValue as Pullenti.Ner.Decree.Internal.PartToken.PartValue).SourceValue;
                attrValue = (attrValue as Pullenti.Ner.Decree.Internal.PartToken.PartValue).Value;
            }
            Pullenti.Ner.Slot s = base.AddSlot(attrName, attrValue, clearOldValue, statCount);
            if (tag != null) 
                s.Tag = tag;
            return s;
        }
        /// <summary>
        /// Статья (номер)
        /// </summary>
        public string Clause
        {
            get
            {
                return this.GetStringValue(ATTR_CLAUSE);
            }
            set
            {
                this.AddSlot(ATTR_CLAUSE, value, true, 0);
            }
        }
        /// <summary>
        /// Наименование статьи
        /// </summary>
        public string ClauseName
        {
            get
            {
                return this.GetStringValue(ATTR_CLAUSENAME);
            }
            set
            {
                this.AddSlot(ATTR_CLAUSENAME, value, true, 0);
            }
        }
        /// <summary>
        /// Часть статьи
        /// </summary>
        public string Part
        {
            get
            {
                return this.GetStringValue(ATTR_PART);
            }
            set
            {
                this.AddSlot(ATTR_PART, value, true, 0);
            }
        }
        /// <summary>
        /// Часть документа (например, часть 2 Налогового кодекса)
        /// </summary>
        public string DocPart
        {
            get
            {
                return this.GetStringValue(ATTR_DOCPART);
            }
            set
            {
                this.AddSlot(ATTR_DOCPART, value, true, 0);
            }
        }
        /// <summary>
        /// Раздел
        /// </summary>
        public string Section
        {
            get
            {
                return this.GetStringValue(ATTR_SECTION);
            }
            set
            {
                this.AddSlot(ATTR_SECTION, value, true, 0);
            }
        }
        /// <summary>
        /// Наименование раздела
        /// </summary>
        public string SectionName
        {
            get
            {
                return this.GetStringValue(ATTR_SECTIONNAME);
            }
            set
            {
                this.AddSlot(ATTR_SECTIONNAME, value, true, 0);
            }
        }
        /// <summary>
        /// Подраздел
        /// </summary>
        public string SubSection
        {
            get
            {
                return this.GetStringValue(ATTR_SUBSECTION);
            }
            set
            {
                this.AddSlot(ATTR_SUBSECTION, value, true, 0);
            }
        }
        /// <summary>
        /// Наименование подраздела
        /// </summary>
        public string SubSectionName
        {
            get
            {
                return this.GetStringValue(ATTR_SUBSECTIONNAME);
            }
            set
            {
                this.AddSlot(ATTR_SUBSECTIONNAME, value, true, 0);
            }
        }
        /// <summary>
        /// Приложение
        /// </summary>
        public string Appendix
        {
            get
            {
                return this.GetStringValue(ATTR_APPENDIX);
            }
            set
            {
                if (value != null && value.Length == 0) 
                    value = "0";
                this.AddSlot(ATTR_APPENDIX, value, true, 0);
            }
        }
        /// <summary>
        /// Наименование приложения
        /// </summary>
        public string AppendixName
        {
            get
            {
                return this.GetStringValue(ATTR_APPENDIXNAME);
            }
            set
            {
                this.AddSlot(ATTR_APPENDIXNAME, value, true, 0);
            }
        }
        /// <summary>
        /// Приложение к приложению
        /// </summary>
        public string Appendix2
        {
            get
            {
                return this.GetStringValue(ATTR_APPENDIX2);
            }
            set
            {
                if (value != null && value.Length == 0) 
                    value = "0";
                this.AddSlot(ATTR_APPENDIX2, value, true, 0);
            }
        }
        /// <summary>
        /// Наименование приложения к приложению
        /// </summary>
        public string Appendix2Name
        {
            get
            {
                return this.GetStringValue(ATTR_APPENDIX2NAME);
            }
            set
            {
                this.AddSlot(ATTR_APPENDIX2NAME, value, true, 0);
            }
        }
        /// <summary>
        /// Глава
        /// </summary>
        public string Chapter
        {
            get
            {
                return this.GetStringValue(ATTR_CHAPTER);
            }
            set
            {
                this.AddSlot(ATTR_CHAPTER, value, true, 0);
            }
        }
        /// <summary>
        /// Наименование главы
        /// </summary>
        public string ChapterName
        {
            get
            {
                return this.GetStringValue(ATTR_CHAPTERNAME);
            }
            set
            {
                this.AddSlot(ATTR_CHAPTERNAME, value, true, 0);
            }
        }
        /// <summary>
        /// Параграф
        /// </summary>
        public string Paragraph
        {
            get
            {
                return this.GetStringValue(ATTR_PARAGRAPH);
            }
            set
            {
                this.AddSlot(ATTR_PARAGRAPH, value, true, 0);
            }
        }
        /// <summary>
        /// Подпараграф
        /// </summary>
        public string SubParagraph
        {
            get
            {
                return this.GetStringValue(ATTR_SUBPARAGRAPH);
            }
            set
            {
                this.AddSlot(ATTR_SUBPARAGRAPH, value, true, 0);
            }
        }
        /// <summary>
        /// Пункт
        /// </summary>
        public string Item
        {
            get
            {
                return this.GetStringValue(ATTR_ITEM);
            }
            set
            {
                this.AddSlot(ATTR_ITEM, value, true, 0);
            }
        }
        /// <summary>
        /// Подпункт
        /// </summary>
        public string SubItem
        {
            get
            {
                return this.GetStringValue(ATTR_SUBITEM);
            }
            set
            {
                this.AddSlot(ATTR_SUBITEM, value, true, 0);
            }
        }
        /// <summary>
        /// Абзац
        /// </summary>
        public string Indention
        {
            get
            {
                return this.GetStringValue(ATTR_INDENTION);
            }
            set
            {
                this.AddSlot(ATTR_INDENTION, value, true, 0);
            }
        }
        /// <summary>
        /// Подабзац
        /// </summary>
        public string SubIndention
        {
            get
            {
                return this.GetStringValue(ATTR_SUBINDENTION);
            }
            set
            {
                this.AddSlot(ATTR_SUBINDENTION, value, true, 0);
            }
        }
        /// <summary>
        /// Преамбула
        /// </summary>
        public string Preamble
        {
            get
            {
                return this.GetStringValue(ATTR_PREAMBLE);
            }
            set
            {
                this.AddSlot(ATTR_PREAMBLE, value, true, 0);
            }
        }
        /// <summary>
        /// Наименование как структурный элемент
        /// </summary>
        public string NameAsItem
        {
            get
            {
                return this.GetStringValue(ATTR_NAMEASITEM);
            }
            set
            {
                this.AddSlot(ATTR_NAMEASITEM, value, true, 0);
            }
        }
        /// <summary>
        /// Что-то непонятное
        /// </summary>
        public string Freetext
        {
            get
            {
                return this.GetStringValue(ATTR_FREETEXT);
            }
            set
            {
                this.AddSlot(ATTR_FREETEXT, value, true, 0);
            }
        }
        /// <summary>
        /// Примечание
        /// </summary>
        public string Notice
        {
            get
            {
                return this.GetStringValue(ATTR_NOTICE);
            }
            set
            {
                if (value != null && value.Length == 0) 
                    value = "0";
                this.AddSlot(ATTR_NOTICE, value, true, 0);
            }
        }
        /// <summary>
        /// Сноска
        /// </summary>
        public string Footnote
        {
            get
            {
                return this.GetStringValue(ATTR_FOOTNOTE);
            }
            set
            {
                if (value != null && value.Length == 0) 
                    value = "0";
                this.AddSlot(ATTR_FOOTNOTE, value, true, 0);
            }
        }
        /// <summary>
        /// Формула
        /// </summary>
        public string Formula
        {
            get
            {
                return this.GetStringValue(ATTR_FORMULA);
            }
            set
            {
                if (value != null && value.Length == 0) 
                    value = "0";
                this.AddSlot(ATTR_FORMULA, value, true, 0);
            }
        }
        /// <summary>
        /// Форма
        /// </summary>
        public string Form
        {
            get
            {
                return this.GetStringValue(ATTR_FORM);
            }
            set
            {
                if (value != null && value.Length == 0) 
                    value = "0";
                this.AddSlot(ATTR_FORM, value, true, 0);
            }
        }
        /// <summary>
        /// Наименование формы
        /// </summary>
        public string FormName
        {
            get
            {
                return this.GetStringValue(ATTR_FORMNAME);
            }
            set
            {
                this.AddSlot(ATTR_FORMNAME, value, true, 0);
            }
        }
        /// <summary>
        /// Лист
        /// </summary>
        public string List
        {
            get
            {
                return this.GetStringValue(ATTR_LIST);
            }
            set
            {
                if (value != null && value.Length == 0) 
                    value = "0";
                this.AddSlot(ATTR_LIST, value, true, 0);
            }
        }
        /// <summary>
        /// Таблица
        /// </summary>
        public string Table
        {
            get
            {
                return this.GetStringValue(ATTR_TABLE);
            }
            set
            {
                if (value != null && value.Length == 0) 
                    value = "0";
                this.AddSlot(ATTR_TABLE, value, true, 0);
            }
        }
        /// <summary>
        /// Наименование таблицы
        /// </summary>
        public string TableName
        {
            get
            {
                return this.GetStringValue(ATTR_TABLENAME);
            }
            set
            {
                this.AddSlot(ATTR_TABLENAME, value, true, 0);
            }
        }
        /// <summary>
        /// Столбец таблицы
        /// </summary>
        public string TableColumn
        {
            get
            {
                return this.GetStringValue(ATTR_TABLECOLUMN);
            }
            set
            {
                if (value != null && value.Length == 0) 
                    value = "0";
                this.AddSlot(ATTR_TABLECOLUMN, value, true, 0);
            }
        }
        /// <summary>
        /// Строка таблицы
        /// </summary>
        public string TableRow
        {
            get
            {
                return this.GetStringValue(ATTR_TABLEROW);
            }
            set
            {
                if (value != null && value.Length == 0) 
                    value = "0";
                this.AddSlot(ATTR_TABLEROW, value, true, 0);
            }
        }
        /// <summary>
        /// Графа таблицы (то ли строка, то ли столбец)
        /// </summary>
        public string TableItem
        {
            get
            {
                return this.GetStringValue(ATTR_TABLEITEM);
            }
            set
            {
                if (value != null && value.Length == 0) 
                    value = "0";
                this.AddSlot(ATTR_TABLEITEM, value, true, 0);
            }
        }
        /// <summary>
        /// Прографка таблицы
        /// </summary>
        public string TableSubItem
        {
            get
            {
                return this.GetStringValue(ATTR_TABLESUBITEM);
            }
            set
            {
                if (value != null && value.Length == 0) 
                    value = "0";
                this.AddSlot(ATTR_TABLESUBITEM, value, true, 0);
            }
        }
        /// <summary>
        /// Предложение
        /// </summary>
        public string Sentence
        {
            get
            {
                return this.GetStringValue(ATTR_SENTENCE);
            }
            set
            {
                if (value != null && value.Length == 0) 
                    value = "0";
                this.AddSlot(ATTR_SENTENCE, value, true, 0);
            }
        }
        /// <summary>
        /// Страница
        /// </summary>
        public string Page
        {
            get
            {
                return this.GetStringValue(ATTR_PAGE);
            }
            set
            {
                this.AddSlot(ATTR_PAGE, value, true, 0);
            }
        }
        /// <summary>
        /// Подпрограмма
        /// </summary>
        public string Subprogram
        {
            get
            {
                return this.GetStringValue(ATTR_SUBPROGRAM);
            }
            set
            {
                this.AddSlot(ATTR_SUBPROGRAM, value, true, 0);
            }
        }
        /// <summary>
        /// Наименование подпрограммы
        /// </summary>
        public string SubprogramName
        {
            get
            {
                return this.GetStringValue(ATTR_SUBPROGRAMNAME);
            }
            set
            {
                this.AddSlot(ATTR_SUBPROGRAMNAME, value, true, 0);
            }
        }
        /// <summary>
        /// Дополнительное соглашение
        /// </summary>
        public string Addagree
        {
            get
            {
                return this.GetStringValue(ATTR_ADDAGREE);
            }
            set
            {
                this.AddSlot(ATTR_ADDAGREE, value, true, 0);
            }
        }
        /// <summary>
        /// НПА - владелец
        /// </summary>
        public DecreeReferent Owner
        {
            get
            {
                DecreeReferent res = this.GetSlotValue(ATTR_OWNER) as DecreeReferent;
                if (res == null) 
                    return null;
                return res;
            }
            set
            {
                this.AddSlot(ATTR_OWNER, value, true, 0);
                if (value != null && LocalTyp != null) 
                    LocalTyp = null;
            }
        }
        public override Pullenti.Ner.Referent ParentReferent
        {
            get
            {
                return Owner;
            }
        }
        public override void MergeSlots(Pullenti.Ner.Referent obj, bool mergeStatistic = true)
        {
            base.MergeSlots(obj, mergeStatistic);
            if (Owner != null && LocalTyp != null) 
                LocalTyp = null;
        }
        int _getLevel(string typ)
        {
            if (typ == ATTR_ADDAGREE || typ == ATTR_SUBPROGRAM) 
                return 0;
            if (typ == ATTR_DOCPART) 
                return 1;
            if (typ == ATTR_APPENDIX || typ == ATTR_APPENDIXNAME) 
                return 1;
            if (typ == ATTR_APPENDIX2 || typ == ATTR_APPENDIX2NAME) 
                return 2;
            if (typ == ATTR_SUBPROGRAM || typ == ATTR_SUBPROGRAMNAME) 
                return 2;
            if (typ == ATTR_SECTION || typ == ATTR_SECTIONNAME) 
                return 2;
            if (typ == ATTR_SUBSECTION || typ == ATTR_SUBSECTIONNAME) 
                return 3;
            if (typ == ATTR_CHAPTER || typ == ATTR_CHAPTERNAME) 
                return 4;
            if (typ == ATTR_PARAGRAPH) 
                return 5;
            if (typ == ATTR_SUBPARAGRAPH) 
                return 6;
            if (typ == ATTR_PAGE) 
                return 6;
            if (typ == ATTR_CLAUSE) 
                return 7;
            if (typ == ATTR_FORM || typ == ATTR_FORMNAME) 
                return 8;
            if (typ == ATTR_TABLE || typ == ATTR_TABLENAME) 
                return 8;
            if (typ == ATTR_LIST) 
                return 9;
            if (typ == ATTR_PREAMBLE) 
                return 9;
            if (typ == ATTR_PART) 
                return 9;
            if (typ == ATTR_NOTICE) 
                return 10;
            if (typ == ATTR_ITEM) 
                return 11;
            if (typ == ATTR_TABLEROW) 
                return 11;
            if (typ == ATTR_TABLEITEM) 
                return 11;
            if (typ == ATTR_TABLESUBITEM) 
                return 12;
            if (typ == ATTR_SUBITEM) 
                return 12;
            if (typ == ATTR_INDENTION) 
                return 13;
            if (typ == ATTR_SUBINDENTION) 
                return 14;
            if (typ == ATTR_SENTENCE) 
                return 15;
            if (typ == ATTR_FORMULA) 
                return 15;
            if (typ == ATTR_TABLECOLUMN) 
                return 17;
            if (typ == ATTR_FOOTNOTE) 
                return 17;
            if (typ == ATTR_NAMEASITEM) 
                return 18;
            if (typ == ATTR_FREETEXT) 
                return 19;
            return -1;
        }
        bool _hasLessLevelAttr(string typ)
        {
            int l = this._getLevel(typ);
            if (l < 0) 
                return false;
            foreach (Pullenti.Ner.Slot s in Slots) 
            {
                int l1 = this._getLevel(s.TypeName);
                if (l1 >= 0 && l1 > l) 
                    return true;
            }
            return false;
        }
        internal void AddName(Pullenti.Ner.Decree.Internal.PartToken p, Pullenti.Ner.MetaToken val = null)
        {
            if (p == null || ((p.Name == null && val == null))) 
                return;
            string typ = null;
            string typ1 = null;
            if (p.Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Appendix) 
                typ = ATTR_APPENDIXNAME;
            else if (p.Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Appendix2) 
                typ = ATTR_APPENDIX2NAME;
            else if (p.Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Section) 
                typ = ATTR_SECTIONNAME;
            else if (p.Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.SubSection) 
                typ = ATTR_SUBSECTIONNAME;
            else if (p.Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Subprogram) 
                typ = ATTR_SUBPROGRAMNAME;
            else if (p.Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Chapter) 
                typ = ATTR_CHAPTERNAME;
            else if (p.Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Clause) 
                typ = ATTR_CLAUSENAME;
            else if (p.Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Form) 
                typ = ATTR_FORMNAME;
            else if (p.Typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Table) 
                typ = ATTR_TABLENAME;
            else 
                return;
            if (val == null) 
                val = p.Name;
            string nam = Pullenti.Ner.Core.MiscHelper.GetTextValueOfMetaToken(val, Pullenti.Ner.Core.GetTextAttr.FirstNounGroupToNominative);
            Pullenti.Ner.Slot s = this.AddSlot(typ, nam, true, 0);
            if (val != null) 
                s.Tag = val.GetSourceText();
            if (p.Values.Count == 0) 
                this.AddSlot(typ.Substring(0, typ.Length - 4), "0", true, 0);
        }
        internal void AddNamedLevelInfo(DecreePartReferent dp)
        {
            foreach (Pullenti.Ner.Slot s in dp.Slots) 
            {
                if (this._getLevel(s.TypeName) > 7 || s.TypeName == ATTR_DOCPART) 
                {
                }
                else 
                    this.AddSlot(s.TypeName, s.Value, false, 0);
            }
        }
        void _addSlot(DecreePartReferent dp, string attr)
        {
            Pullenti.Ner.Slot s = dp.FindSlot(attr, null, true);
            if (s == null) 
                return;
            Pullenti.Ner.Slot ss = this.AddSlot(attr, s.Value, true, 0);
            ss.Tag = s.Tag;
        }
        // Добавить информацию о вышележащих элементах
        internal void AddHighLevelInfo(DecreePartReferent dp)
        {
            if (dp.Addagree != null && Addagree == null) 
                Addagree = dp.Addagree;
            else if (dp.Addagree != Addagree) 
                return;
            if (dp.Appendix != null && Appendix == null) 
            {
                this._addSlot(dp, ATTR_APPENDIX);
                this._addSlot(dp, ATTR_APPENDIXNAME);
            }
            else if (Appendix != dp.Appendix) 
                return;
            else if (Appendix == "0" && AppendixName != dp.AppendixName) 
                return;
            if (dp.Appendix2 != null && Appendix2 == null) 
            {
                this._addSlot(dp, ATTR_APPENDIX2);
                this._addSlot(dp, ATTR_APPENDIX2NAME);
            }
            else if (Appendix2 != dp.Appendix2) 
                return;
            else if (Appendix2 == "0" && Appendix2Name != dp.Appendix2Name) 
                return;
            if (dp.Subprogram != null && Subprogram == null) 
            {
                this._addSlot(dp, ATTR_SUBPROGRAM);
                this._addSlot(dp, ATTR_SUBPROGRAMNAME);
            }
            else if (Subprogram != dp.Subprogram) 
                return;
            if (SubprogramName != dp.SubprogramName) 
                return;
            if (dp.DocPart != null && DocPart == null) 
                this._addSlot(dp, ATTR_DOCPART);
            else if (DocPart != dp.DocPart) 
            {
                if (dp.DocPart != null) 
                    return;
            }
            if (dp.Section != null && Section == null && this._hasLessLevelAttr(ATTR_SECTION)) 
            {
                this._addSlot(dp, ATTR_SECTION);
                this._addSlot(dp, ATTR_SECTIONNAME);
            }
            else if (Section != dp.Section) 
                return;
            else if (Section == "0" && SectionName != dp.SectionName) 
                return;
            if (dp.SubSection != null && SubSection == null && this._hasLessLevelAttr(ATTR_SUBSECTION)) 
            {
                this._addSlot(dp, ATTR_SUBSECTION);
                this._addSlot(dp, ATTR_SUBSECTIONNAME);
            }
            else if (SubSection != dp.SubSection) 
                return;
            else if (SubSection == "0" && SubSectionName != dp.SubSectionName) 
                return;
            if (dp.Chapter != null && Chapter == null && this._hasLessLevelAttr(ATTR_CHAPTER)) 
            {
                this._addSlot(dp, ATTR_CHAPTER);
                this._addSlot(dp, ATTR_CHAPTERNAME);
            }
            else if (dp.Chapter != Chapter) 
                return;
            else if (dp.Chapter == "0" && dp.ChapterName != ChapterName) 
                return;
            if (dp.Paragraph != null && Paragraph == null && this._hasLessLevelAttr(ATTR_PARAGRAPH)) 
                this._addSlot(dp, ATTR_PARAGRAPH);
            else if (Paragraph != dp.Paragraph) 
                return;
            if (dp.SubParagraph != null && SubParagraph == null && this._hasLessLevelAttr(ATTR_SUBPARAGRAPH)) 
                SubParagraph = dp.SubParagraph;
            else if (SubParagraph != dp.SubParagraph) 
                return;
            if (dp.Clause != null && Clause == null && ((this._hasLessLevelAttr(ATTR_CLAUSE) || (Slots.Count < 2)))) 
                this._addSlot(dp, ATTR_CLAUSE);
            else if (dp.Clause != Clause) 
                return;
            if (dp.Part != null && Part == null && this._hasLessLevelAttr(ATTR_PART)) 
                this._addSlot(dp, ATTR_PART);
            else if (dp.Part != Part) 
                return;
            if (dp.Form != null && Form == null && this._hasLessLevelAttr(ATTR_FORM)) 
            {
                this._addSlot(dp, ATTR_FORM);
                this._addSlot(dp, ATTR_FORMNAME);
            }
            if (dp.Table != null && Table == null && this._hasLessLevelAttr(ATTR_TABLE)) 
            {
                this._addSlot(dp, ATTR_TABLE);
                this._addSlot(dp, ATTR_TABLENAME);
            }
            if (dp.List != null && List == null && this._hasLessLevelAttr(ATTR_LIST)) 
                this._addSlot(dp, ATTR_LIST);
            if (dp.Item != null && Item == null && this._hasLessLevelAttr(ATTR_ITEM)) 
                this._addSlot(dp, ATTR_ITEM);
            else if (dp.Item != Item) 
            {
                if ((dp.TableItem != null && dp.Item == null && TableRow == null) && Item != null) 
                {
                    TableRow = Item;
                    Item = null;
                }
                else if ((dp.Item != null && Item == null && TableRow == null) && ((dp.TableItem != null || TableItem != null))) 
                    TableRow = dp.Item;
                else 
                    return;
            }
            if (dp.TableRow != null && TableRow == null && ((this._hasLessLevelAttr(ATTR_TABLEROW) || TableItem != null))) 
                this._addSlot(dp, ATTR_TABLEROW);
            if (dp.SubItem != null && SubItem == null && this._hasLessLevelAttr(ATTR_SUBITEM)) 
                this._addSlot(dp, ATTR_SUBITEM);
            else if (dp.SubItem != SubItem) 
                return;
            if (dp.Notice != null && Notice == null && this._hasLessLevelAttr(ATTR_NOTICE)) 
                this._addSlot(dp, ATTR_NOTICE);
            else if (dp.Notice != Notice) 
                return;
            if (dp.Preamble != null && Preamble == null) 
                this._addSlot(dp, ATTR_PREAMBLE);
            if (dp.Indention != null && Indention == null && this._hasLessLevelAttr(ATTR_INDENTION)) 
                this._addSlot(dp, ATTR_INDENTION);
            if (dp.TableColumn != null && TableColumn == null && this._hasLessLevelAttr(ATTR_TABLECOLUMN)) 
                this._addSlot(dp, ATTR_TABLECOLUMN);
            if (dp.TableItem != null && TableItem == null && ((this._hasLessLevelAttr(ATTR_TABLEITEM) || TableRow != null || Item != null))) 
                this._addSlot(dp, ATTR_TABLEITEM);
            if (dp.TableSubItem != null && TableSubItem == null && ((this._hasLessLevelAttr(ATTR_TABLESUBITEM) || TableRow != null || Item != null))) 
                this._addSlot(dp, ATTR_TABLESUBITEM);
        }
        // Проверить, что все элементы находятся на более низком уровне, чем у аргумента
        internal bool IsAllItemsLessLevel(Pullenti.Ner.Referent upperParts, bool ignoreEquals)
        {
            if (upperParts is DecreeReferent) 
                return true;
            int maxLev = 0;
            string maxTyp = null;
            foreach (Pullenti.Ner.Slot s in Slots) 
            {
                int l = this._getLevel(s.TypeName);
                if (l > maxLev) 
                {
                    maxLev = l;
                    maxTyp = s.TypeName;
                }
            }
            foreach (Pullenti.Ner.Slot s in Slots) 
            {
                int l = this._getLevel(s.TypeName);
                if (l < 0) 
                    continue;
                if (upperParts.FindSlot(s.TypeName, null, true) != null) 
                {
                    if (ignoreEquals) 
                    {
                        if (l == maxLev) 
                            continue;
                    }
                    if (upperParts.FindSlot(s.TypeName, s.Value, true) == null) 
                        return false;
                    continue;
                }
                foreach (Pullenti.Ner.Slot ss in upperParts.Slots) 
                {
                    int ll = this._getLevel(ss.TypeName);
                    if (ll >= l) 
                        return false;
                }
            }
            return true;
        }
        // удалить все значения уровня больше или равно
        internal void RemoveSlots(Pullenti.Ner.Decree.Internal.PartToken.ItemType typ)
        {
            int l0 = this._getLevel(Pullenti.Ner.Decree.Internal.PartToken._getAttrNameByTyp(typ));
            if (l0 <= 0) 
                return;
            for (int i = Slots.Count - 1; i >= 0; i--) 
            {
                int l = this._getLevel(Slots[i].TypeName);
                if (l >= l0) 
                {
                    if (((typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.TableItem || typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.TableSubItem)) && ((Slots[i].TypeName == "TABLEROW" || Slots[i].TypeName == "ITEM"))) 
                    {
                    }
                    else if (((typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.TableRow || typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Item)) && Slots[i].TypeName == "TABLEITEM") 
                    {
                    }
                    else 
                        Slots.RemoveAt(i);
                }
            }
        }
        internal bool IsAllItemsOverThisLevel(Pullenti.Ner.Decree.Internal.PartToken.ItemType typ)
        {
            int l0 = this._getLevel(Pullenti.Ner.Decree.Internal.PartToken._getAttrNameByTyp(typ));
            if (l0 <= 0) 
                return false;
            foreach (Pullenti.Ner.Slot s in Slots) 
            {
                int l = this._getLevel(s.TypeName);
                if (l <= 0) 
                    continue;
                if (l >= l0) 
                {
                    if (typ == Pullenti.Ner.Decree.Internal.PartToken.ItemType.Footnote && l > l0) 
                    {
                    }
                    else 
                        return false;
                }
            }
            return true;
        }
        internal int GetMinLevel()
        {
            int min = 0;
            foreach (Pullenti.Ner.Slot s in Slots) 
            {
                int l = this._getLevel(s.TypeName);
                if (l <= 0) 
                    continue;
                if (min == 0) 
                    min = l;
                else if (min > l) 
                    min = l;
            }
            return min;
        }
        public override bool CanBeEquals(Pullenti.Ner.Referent obj, Pullenti.Ner.Core.ReferentsEqualType typ)
        {
            bool b = this._CanBeEquals(obj, typ, false);
            return b;
        }
        bool _CanBeEquals(Pullenti.Ner.Referent obj, Pullenti.Ner.Core.ReferentsEqualType typ, bool ignoreGeo)
        {
            DecreePartReferent dr = obj as DecreePartReferent;
            if (dr == null) 
                return false;
            if (Owner != null && dr.Owner != null) 
            {
                if (Owner != dr.Owner) 
                    return false;
            }
            else if (typ == Pullenti.Ner.Core.ReferentsEqualType.DifferentTexts) 
                return false;
            else 
            {
                string ty1 = (Owner == null ? LocalTyp : Owner.Typ);
                string ty2 = (dr.Owner == null ? dr.LocalTyp : dr.Owner.Typ);
                if (ty1 != ty2) 
                {
                    ty1 = (Owner == null ? LocalTyp : Owner.Typ0);
                    ty2 = (dr.Owner == null ? dr.LocalTyp : dr.Owner.Typ0);
                    if (ty1 != ty2) 
                        return false;
                }
            }
            if (Clause != dr.Clause) 
            {
                if (typ == Pullenti.Ner.Core.ReferentsEqualType.ForMerging && ((Clause == null || dr.Clause == null))) 
                {
                }
                else 
                    return false;
            }
            else if (Clause == "0" && ClauseName != dr.ClauseName) 
                return false;
            if (Part != dr.Part) 
            {
                if (typ == Pullenti.Ner.Core.ReferentsEqualType.ForMerging && ((Part == null || dr.Part == null))) 
                {
                }
                else 
                    return false;
            }
            if (Paragraph != dr.Paragraph) 
            {
                if (typ == Pullenti.Ner.Core.ReferentsEqualType.ForMerging && ((Paragraph == null || dr.Paragraph == null))) 
                {
                }
                else 
                    return false;
            }
            if (SubParagraph != dr.SubParagraph) 
            {
                if (typ == Pullenti.Ner.Core.ReferentsEqualType.ForMerging && ((SubParagraph == null || dr.SubParagraph == null))) 
                {
                }
                else 
                    return false;
            }
            if (NameAsItem != dr.NameAsItem) 
                return false;
            if (Item != dr.Item) 
            {
                if (typ == Pullenti.Ner.Core.ReferentsEqualType.ForMerging && ((Item == null || dr.Item == null))) 
                {
                }
                else 
                    return false;
            }
            if (Freetext != dr.Freetext) 
                return false;
            if (SubItem != dr.SubItem) 
            {
                if (typ == Pullenti.Ner.Core.ReferentsEqualType.ForMerging && ((SubItem == null || dr.SubItem == null))) 
                {
                }
                else 
                    return false;
            }
            if (Notice != dr.Notice) 
            {
                if (typ == Pullenti.Ner.Core.ReferentsEqualType.ForMerging && ((Notice == null || dr.Notice == null))) 
                {
                }
                else 
                    return false;
            }
            if (Footnote != dr.Footnote) 
            {
                if (typ == Pullenti.Ner.Core.ReferentsEqualType.ForMerging && ((Footnote == null || dr.Footnote == null))) 
                {
                }
                else 
                    return false;
            }
            if (Form != dr.Form) 
            {
                if (typ == Pullenti.Ner.Core.ReferentsEqualType.ForMerging && ((Form == null || dr.Form == null))) 
                {
                }
                else 
                    return false;
            }
            if (Form == "0" && FormName != dr.FormName) 
                return false;
            if (List != dr.List) 
            {
                if (typ == Pullenti.Ner.Core.ReferentsEqualType.ForMerging && ((List == null || dr.List == null))) 
                {
                }
                else 
                    return false;
            }
            if (Table != dr.Table) 
            {
                if (typ == Pullenti.Ner.Core.ReferentsEqualType.ForMerging && ((Table == null || dr.Table == null))) 
                {
                }
                else 
                    return false;
            }
            if (Table == "0" && TableName != dr.TableName) 
                return false;
            if (TableColumn != dr.TableColumn) 
            {
                if (typ == Pullenti.Ner.Core.ReferentsEqualType.ForMerging && ((TableColumn == null || dr.TableColumn == null))) 
                {
                }
                else 
                    return false;
            }
            if (TableRow != dr.TableRow) 
            {
                if (typ == Pullenti.Ner.Core.ReferentsEqualType.ForMerging && ((TableRow == null || dr.TableRow == null))) 
                {
                }
                else 
                    return false;
            }
            if (TableItem != dr.TableItem) 
            {
                if (typ == Pullenti.Ner.Core.ReferentsEqualType.ForMerging && ((TableItem == null || dr.TableItem == null))) 
                {
                }
                else 
                    return false;
            }
            if (TableSubItem != dr.TableSubItem) 
            {
                if (typ == Pullenti.Ner.Core.ReferentsEqualType.ForMerging && ((TableSubItem == null || dr.TableSubItem == null))) 
                {
                }
                else 
                    return false;
            }
            if (Formula != dr.Formula) 
            {
                if (typ == Pullenti.Ner.Core.ReferentsEqualType.ForMerging && ((Formula == null || dr.Formula == null))) 
                {
                }
                else 
                    return false;
            }
            if (Sentence != dr.Sentence) 
            {
                if (typ == Pullenti.Ner.Core.ReferentsEqualType.ForMerging && ((Sentence == null || dr.Sentence == null))) 
                {
                }
                else 
                    return false;
            }
            if (Indention != dr.Indention) 
            {
                if (typ == Pullenti.Ner.Core.ReferentsEqualType.ForMerging && ((Indention == null || dr.Indention == null))) 
                {
                }
                else 
                    return false;
            }
            if (SubIndention != dr.SubIndention) 
            {
                if (typ == Pullenti.Ner.Core.ReferentsEqualType.ForMerging && ((SubIndention == null || dr.SubIndention == null))) 
                {
                }
                else 
                    return false;
            }
            if (Appendix != dr.Appendix) 
            {
                if (Appendix != null && dr.Appendix != null) 
                    return false;
                if (Clause == null && Paragraph == null && Item == null) 
                    return false;
                if (typ != Pullenti.Ner.Core.ReferentsEqualType.ForMerging) 
                    return false;
            }
            if (AppendixName != dr.AppendixName && AppendixName != null && dr.AppendixName != null) 
                return false;
            if (Appendix2 != dr.Appendix2) 
            {
                if (Appendix2 != null && dr.Appendix2 != null) 
                    return false;
                if (Clause == null && Paragraph == null && Item == null) 
                    return false;
                if (typ != Pullenti.Ner.Core.ReferentsEqualType.ForMerging) 
                    return false;
            }
            if (Appendix2Name != dr.Appendix2Name) 
                return false;
            if (Chapter != dr.Chapter) 
            {
                if (Chapter != null && dr.Chapter != null) 
                    return false;
                if (Clause == null && Paragraph == null && Item == null) 
                    return false;
                if (typ != Pullenti.Ner.Core.ReferentsEqualType.ForMerging) 
                    return false;
            }
            if (Chapter == "0" && ChapterName != dr.ChapterName) 
                return false;
            if (Section != dr.Section) 
            {
                if (Section != null && dr.Section != null) 
                    return false;
                if ((Clause == null && Paragraph == null && Item == null) && SubSection == null) 
                    return false;
                if (typ != Pullenti.Ner.Core.ReferentsEqualType.ForMerging) 
                    return false;
            }
            if (Section == "0" && SectionName != dr.SectionName) 
                return false;
            if (SubSection != dr.SubSection) 
            {
                if (SubSection != null && dr.SubSection != null) 
                    return false;
                if (Clause == null && Paragraph == null && Item == null) 
                    return false;
                if (typ != Pullenti.Ner.Core.ReferentsEqualType.ForMerging) 
                    return false;
            }
            if (SubSection == "0" && SubSectionName != dr.SubSectionName) 
                return false;
            if (Subprogram != null || dr.Subprogram != null) 
            {
                if (Subprogram != dr.Subprogram) 
                    return false;
                if (SubprogramName != dr.SubprogramName) 
                    return false;
                return true;
            }
            if (Addagree != null || dr.Addagree != null) 
            {
                if (Addagree != dr.Addagree) 
                    return false;
            }
            if (DocPart != dr.DocPart) 
            {
                if (typ == Pullenti.Ner.Core.ReferentsEqualType.ForMerging && ((DocPart == null || dr.DocPart == null))) 
                {
                }
                else 
                    return false;
            }
            if (Page != dr.Page) 
                return false;
            return true;
        }
        public static DecreePartReferent CreateRangeReferent(DecreePartReferent min, DecreePartReferent max)
        {
            DecreePartReferent res = min.Clone() as DecreePartReferent;
            int cou = 0;
            foreach (Pullenti.Ner.Slot s in res.Slots) 
            {
                Pullenti.Ner.Slot ss = max.FindSlot(s.TypeName, null, true);
                if (ss == null) 
                    return null;
                if (ss.Value == s.Value) 
                    continue;
                if (max.FindSlot(s.TypeName, s.Value, true) != null) 
                    continue;
                if ((++cou) > 1) 
                    return null;
                res.UploadSlot(s, string.Format("{0}-{1}", s.Value, ss.Value));
            }
            if (cou != 1) 
                return null;
            return res;
        }
    }
}