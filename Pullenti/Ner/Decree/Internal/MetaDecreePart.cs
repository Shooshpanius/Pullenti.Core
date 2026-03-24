/*
 * SDK Pullenti Lingvo, version 4.33, fabruary 2026. Copyright (c) 2013-2026, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Decree.Internal
{
    internal class MetaDecreePart : Pullenti.Ner.Metadata.ReferentClass
    {
        public static void Initialize()
        {
            GlobalMeta = new MetaDecreePart();
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_OWNER, "Владелец", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_LOCALTYP, "Локальный тип", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_SECTION, "Раздел", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_SECTIONNAME, "Наименование раздела", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_SUBSECTION, "Подраздел", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_SUBSECTIONNAME, "Наименование подраздела", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_APPENDIX, "Приложение", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_APPENDIXNAME, "Наименование приложения", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_APPENDIX2, "Приложение к приложению", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_APPENDIX2NAME, "Наименование приложения к приложению", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_CHAPTER, "Глава", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_CHAPTERNAME, "Наименование главы", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_PREAMBLE, "Преамбула", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_CLAUSE, "Статья", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_CLAUSENAME, "Наименование статьи", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_PART, "Часть", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_DOCPART, "Часть документа", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_PARAGRAPH, "Параграф", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_SUBPARAGRAPH, "Подпараграф", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_ITEM, "Пункт", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_SUBITEM, "Подпункт", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_INDENTION, "Абзац", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_SUBINDENTION, "Подабзац", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_FORMULA, "Формула", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_FORM, "Форма", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_FORMNAME, "Наименование формы", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_LIST, "Лист", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_TABLE, "Таблица", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_TABLENAME, "Наименование таблицы", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_TABLECOLUMN, "Столбец таблицы", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_TABLEROW, "Строка таблицы", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_TABLEITEM, "Графа таблицы", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_TABLESUBITEM, "Прографка таблицы", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_SENTENCE, "Предложение", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_FOOTNOTE, "Сноска", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_SUBPROGRAM, "Подпрограмма", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_SUBPROGRAMNAME, "Наименование подпрограммы", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_ADDAGREE, "Допсоглашение", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_NOTICE, "Примечание", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_NAMEASITEM, "Наименование (как элемент структуры)", 0, 1);
            GlobalMeta.AddFeature(Pullenti.Ner.Decree.DecreePartReferent.ATTR_DESCRIPTION, "Дополнительное описание", 0, 0);
        }
        public override string Name
        {
            get
            {
                return Pullenti.Ner.Decree.DecreeReferent.OBJ_TYPENAME;
            }
        }
        public override string Caption
        {
            get
            {
                return "Ссылка на часть НПА";
            }
        }
        public static string PartImageId = "part";
        public static string PartLocImageId = "partloc";
        public override string GetImageId(Pullenti.Ner.Referent obj = null)
        {
            Pullenti.Ner.Decree.DecreePartReferent dpr = obj as Pullenti.Ner.Decree.DecreePartReferent;
            if (dpr != null) 
            {
                if (dpr.Owner == null) 
                    return PartLocImageId;
            }
            return PartImageId;
        }
        public static MetaDecreePart GlobalMeta;
    }
}