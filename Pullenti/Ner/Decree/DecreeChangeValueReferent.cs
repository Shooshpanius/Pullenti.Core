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

namespace Pullenti.Ner.Decree
{
    /// <summary>
    /// Значение изменения структурного элемента НПА
    /// </summary>
    public class DecreeChangeValueReferent : Pullenti.Ner.Referent
    {
        public DecreeChangeValueReferent() : base(OBJ_TYPENAME)
        {
            InstanceOf = Pullenti.Ner.Decree.Internal.MetaDecreeChangeValue.GlobalMeta;
        }
        /// <summary>
        /// Имя типа сущности TypeName ("DECREECHANGEVALUE")
        /// </summary>
        public const string OBJ_TYPENAME = "DECREECHANGEVALUE";
        /// <summary>
        /// Имя атрибута - тип (DecreeChangeValueKind)
        /// </summary>
        public const string ATTR_KIND = "KIND";
        /// <summary>
        /// Имя атрибута - значение
        /// </summary>
        public const string ATTR_VALUE = "VALUE";
        /// <summary>
        /// Имя атрибута - новый структурный элемент
        /// </summary>
        public const string ATTR_NEWITEM = "NEWITEM";
        /// <summary>
        /// Начальная позиция текста (сразу за кавычкой)
        /// </summary>
        public const string ATTR_BEGINCHAR = "BEGIN";
        /// <summary>
        /// Конечная позиция текста (сразу перед закрывающей кавычкой)
        /// </summary>
        public const string ATTR_ENDCHAR = "END";
        public override string ToStringEx(bool shortVariant, Pullenti.Morph.MorphLang lang = null, int lev = 0)
        {
            StringBuilder res = new StringBuilder();
            List<string> nws = NewItems;
            if (nws.Count > 0) 
            {
                foreach (string p in nws) 
                {
                    DecreePartReferent dpr = new DecreePartReferent();
                    int ii = p.IndexOf(' ');
                    if (ii < 0) 
                        dpr.AddSlot(p, "", false, 0);
                    else 
                        dpr.AddSlot(p.Substring(0, ii), p.Substring(ii + 1), false, 0);
                    res.AppendFormat(" новый '{0}'", dpr.ToStringEx(true, null, 0));
                }
            }
            if (Kind != DecreeChangeValueKind.Undefined) 
                res.AppendFormat(" {0}", Pullenti.Ner.Decree.Internal.MetaDecreeChangeValue.KindFeature.ConvertInnerValueToOuterValue(Kind.ToString(), lang).ToString().ToLower());
            List<string> vals = this.GetStringValues(ATTR_VALUE);
            if (vals != null && vals.Count > 0) 
            {
                res.Append(" ");
                for (int i = 0; i < vals.Count; i++) 
                {
                    if (i > 0) 
                        res.Append(", ");
                    string val = vals[i];
                    if (val.Length > 100) 
                        val = val.Substring(0, 100) + "...";
                    res.AppendFormat("'{0}'", val);
                    res.Replace('\n', ' ');
                    res.Replace('\r', ' ');
                }
            }
            return res.ToString().Trim();
        }
        /// <summary>
        /// Тип значение
        /// </summary>
        public DecreeChangeValueKind Kind
        {
            get
            {
                string s = this.GetStringValue(ATTR_KIND);
                if (s == null) 
                    return DecreeChangeValueKind.Undefined;
                try 
                {
                    if (s == "Footnote") 
                        return DecreeChangeValueKind.Undefined;
                    object res = Enum.Parse(typeof(DecreeChangeValueKind), s, true);
                    if (res is DecreeChangeValueKind) 
                        return (DecreeChangeValueKind)res;
                }
                catch(Exception ex1830) 
                {
                }
                return DecreeChangeValueKind.Undefined;
            }
            set
            {
                if (value != DecreeChangeValueKind.Undefined) 
                    this.AddSlot(ATTR_KIND, value.ToString(), true, 0);
            }
        }
        /// <summary>
        /// Значение
        /// </summary>
        public string Value
        {
            get
            {
                return this.GetStringValue(ATTR_VALUE);
            }
            set
            {
                this.AddSlot(ATTR_VALUE, value, true, 0);
            }
        }
        /// <summary>
        /// Новые структурные элементы, которые добавляются этим значением 
        /// (дополнить ... статьями 10.1 и 10.2 следующего содержания)
        /// </summary>
        public List<string> NewItems
        {
            get
            {
                List<string> res = new List<string>();
                foreach (Pullenti.Ner.Slot s in Slots) 
                {
                    if (s.TypeName == ATTR_NEWITEM && (s.Value is string)) 
                        res.Add(s.Value as string);
                }
                return res;
            }
        }
        /// <summary>
        /// Начальная позиция текста (сразу за кавычкой)
        /// </summary>
        public int BeginChar
        {
            get
            {
                string val = this.GetStringValue(ATTR_BEGINCHAR);
                if (val == null) 
                    return 0;
                int n;
                if (int.TryParse(val, out n)) 
                    return n;
                return 0;
            }
            set
            {
                this.AddSlot(ATTR_BEGINCHAR, value.ToString(), true, 0);
            }
        }
        /// <summary>
        /// Конечная позиция текста (сразу перед закрывающей кавычкой)
        /// </summary>
        public int EndChar
        {
            get
            {
                string val = this.GetStringValue(ATTR_ENDCHAR);
                if (val == null) 
                    return 0;
                int n;
                if (int.TryParse(val, out n)) 
                    return n;
                return 0;
            }
            set
            {
                this.AddSlot(ATTR_ENDCHAR, value.ToString(), true, 0);
            }
        }
        public override bool CanBeEquals(Pullenti.Ner.Referent obj, Pullenti.Ner.Core.ReferentsEqualType typ = Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)
        {
            return obj == this;
        }
    }
}