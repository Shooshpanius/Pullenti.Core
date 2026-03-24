/*
 * SDK Pullenti Lingvo, version 4.33, fabruary 2026. Copyright (c) 2013-2026, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Text;

namespace Pullenti.Ner.Link
{
    /// <summary>
    /// Связь сущностей
    /// </summary>
    public class LinkReferent : Pullenti.Ner.Referent
    {
        public LinkReferent() : base(OBJ_TYPENAME)
        {
            InstanceOf = MetaLink.GlobalMeta;
        }
        /// <summary>
        /// Имя типа сущности TypeName ("LINK")
        /// </summary>
        public const string OBJ_TYPENAME = "LINK";
        /// <summary>
        /// Имя атрибута - тип связи
        /// </summary>
        public const string ATTR_TYPE = "TYPE";
        /// <summary>
        /// Имя атрибута - параметр связи (если есть)
        /// </summary>
        public const string ATTR_PARAM = "PARAM";
        /// <summary>
        /// Имя атрибута - ссылка на первую сущность связи
        /// </summary>
        public const string ATTR_OBJECT1 = "OBJECT1";
        /// <summary>
        /// Имя атрибута - ссылка на вторую сущность связи
        /// </summary>
        public const string ATTR_OBJECT2 = "OBJECT2";
        /// <summary>
        /// Имя атрибута - дата начала
        /// </summary>
        public const string ATTR_DATEFROM = "DATEFROM";
        /// <summary>
        /// Имя атрибута - дата окончания
        /// </summary>
        public const string ATTR_DATETO = "DATETO";
        /// <summary>
        /// Тип связи
        /// </summary>
        public LinkType Typ
        {
            get
            {
                string str = this.GetStringValue(ATTR_TYPE);
                if (str == null) 
                    return LinkType.Undefined;
                try 
                {
                    return (LinkType)Enum.Parse(typeof(LinkType), str, true);
                }
                catch(Exception ex3167) 
                {
                }
                return LinkType.Undefined;
            }
            set
            {
                this.AddSlot(ATTR_TYPE, value.ToString().ToLower(), true, 0);
            }
        }
        /// <summary>
        /// Тип связи (расшифровка)
        /// </summary>
        public string TypString
        {
            get
            {
                string val = this.GetStringValue(ATTR_TYPE);
                if (val == null) 
                    return null;
                string res = MetaLink.Types.ConvertInnerValueToOuterValue(val, null) ?? val;
                return res.ToLower();
            }
        }
        /// <summary>
        /// Дополнительный параметр связи (например, для работы это должность)
        /// </summary>
        public string Param
        {
            get
            {
                return this.GetStringValue(ATTR_PARAM);
            }
            set
            {
                this.AddSlot(ATTR_PARAM, value, true, 0);
            }
        }
        public override Pullenti.Ner.Referent ParentReferent
        {
            get
            {
                return Object1;
            }
        }
        /// <summary>
        /// Ссылка на первую сущность
        /// </summary>
        public Pullenti.Ner.Referent Object1
        {
            get
            {
                return this.GetSlotValue(ATTR_OBJECT1) as Pullenti.Ner.Referent;
            }
            set
            {
                this.AddSlot(ATTR_OBJECT1, value, true, 0);
            }
        }
        /// <summary>
        /// Ссылка на вторую сущность
        /// </summary>
        public Pullenti.Ner.Referent Object2
        {
            get
            {
                return this.GetSlotValue(ATTR_OBJECT2) as Pullenti.Ner.Referent;
            }
            set
            {
                this.AddSlot(ATTR_OBJECT2, value, true, 0);
            }
        }
        /// <summary>
        /// Дата начала связи
        /// </summary>
        public Pullenti.Ner.Date.DateReferent DateFrom
        {
            get
            {
                return this.GetSlotValue(ATTR_DATEFROM) as Pullenti.Ner.Date.DateReferent;
            }
            set
            {
                this.AddSlot(ATTR_DATEFROM, value, true, 0);
            }
        }
        /// <summary>
        /// Дата окончания связи
        /// </summary>
        public Pullenti.Ner.Date.DateReferent DateTo
        {
            get
            {
                return this.GetSlotValue(ATTR_DATETO) as Pullenti.Ner.Date.DateReferent;
            }
            set
            {
                this.AddSlot(ATTR_DATETO, value, true, 0);
            }
        }
        public override string ToStringEx(bool shortVariant, Pullenti.Morph.MorphLang lang = null, int lev = 0)
        {
            StringBuilder tmp = new StringBuilder();
            if (Typ != LinkType.Undefined) 
                tmp.AppendFormat("{0}", MetaLink.Types.ConvertInnerValueToOuterValue(this.GetStringValue(ATTR_TYPE), null));
            if (Param != null) 
            {
                if (tmp.Length > 0) 
                    tmp.Append(", ");
                tmp.Append(Param);
            }
            if (DateFrom != null && DateTo != null) 
            {
                if (tmp.Length > 0) 
                    tmp.Append(", ");
                if (DateFrom == DateTo) 
                    tmp.Append(DateFrom.ToStringEx(true, null, 0));
                else 
                    tmp.AppendFormat("с {0} по {1}", DateFrom.ToStringEx(true, null, 0), DateTo.ToStringEx(true, null, 0));
            }
            else if (DateFrom != null) 
            {
                if (tmp.Length > 0) 
                    tmp.Append(", ");
                tmp.AppendFormat("с {0}", DateFrom.ToStringEx(true, null, 0));
            }
            else if (DateTo != null) 
            {
                if (tmp.Length > 0) 
                    tmp.Append(", ");
                tmp.AppendFormat("по {0}", DateTo.ToStringEx(true, null, 0));
            }
            if (Object2 != null && !shortVariant) 
            {
                if (tmp.Length > 0) 
                    tmp.Append(", ");
                tmp.Append(Object2.ToStringEx(true, null, 0));
            }
            return tmp.ToString();
        }
        public override bool CanBeEquals(Pullenti.Ner.Referent obj, Pullenti.Ner.Core.ReferentsEqualType typ = Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)
        {
            LinkReferent lr = obj as LinkReferent;
            if (lr == null) 
                return false;
            if (lr.Object1 != Object1 || lr.Object2 != Object2) 
                return false;
            return true;
        }
    }
}