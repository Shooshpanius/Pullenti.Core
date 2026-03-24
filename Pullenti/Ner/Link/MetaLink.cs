/*
 * SDK Pullenti Lingvo, version 4.33, fabruary 2026. Copyright (c) 2013-2026, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Link
{
    internal class MetaLink : Pullenti.Ner.Metadata.ReferentClass
    {
        public static void Initialize()
        {
            GlobalMeta = new MetaLink();
            Types = GlobalMeta.AddFeature(LinkReferent.ATTR_TYPE, "Тип", 1, 1);
            Types.AddValue(LinkType.Born.ToString().ToLower(), "Рождение", null, null);
            Types.AddValue(LinkType.Family.ToString().ToLower(), "Родственник", null, null);
            Types.AddValue(LinkType.Study.ToString().ToLower(), "Учеба", null, null);
            Types.AddValue(LinkType.Work.ToString().ToLower(), "Работа", null, null);
            Types.AddValue(LinkType.Contact.ToString().ToLower(), "Контакт", null, null);
            Types.AddValue(LinkType.Document.ToString().ToLower(), "Документ", null, null);
            Types.AddValue(LinkType.Address.ToString().ToLower(), "Локация", null, null);
            GlobalMeta.AddFeature(LinkReferent.ATTR_PARAM, "Параметр", 0, 1);
            GlobalMeta.AddFeature(LinkReferent.ATTR_OBJECT1, "Первый объект", 1, 1);
            GlobalMeta.AddFeature(LinkReferent.ATTR_OBJECT2, "Второй объект", 1, 1);
            GlobalMeta.AddFeature(LinkReferent.ATTR_DATEFROM, "Начало интервала", 0, 1);
            GlobalMeta.AddFeature(LinkReferent.ATTR_DATETO, "Окончание интервала", 0, 1);
        }
        public static Pullenti.Ner.Metadata.Feature Types;
        public override string Name
        {
            get
            {
                return LinkReferent.OBJ_TYPENAME;
            }
        }
        public override string Caption
        {
            get
            {
                return "Связь сущностей";
            }
        }
        public static string ImageId = "link";
        public override string GetImageId(Pullenti.Ner.Referent obj = null)
        {
            return ImageId;
        }
        public static MetaLink GlobalMeta;
    }
}