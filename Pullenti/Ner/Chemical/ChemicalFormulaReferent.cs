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

namespace Pullenti.Ner.Chemical
{
    /// <summary>
    /// Химическая формула
    /// </summary>
    public class ChemicalFormulaReferent : Pullenti.Ner.Referent
    {
        public ChemicalFormulaReferent() : base(OBJ_TYPENAME)
        {
            InstanceOf = MetaChemical.GlobalMeta;
        }
        /// <summary>
        /// Имя типа сущности TypeName ("CHEMICALFORMULA")
        /// </summary>
        public const string OBJ_TYPENAME = "CHEMICALFORMULA";
        /// <summary>
        /// Имя атрибута - значение самой формулы
        /// </summary>
        public const string ATTR_VALUE = "VALUE";
        /// <summary>
        /// Имя атрибута - наименование формулы
        /// </summary>
        public const string ATTR_NAME = "NAME";
        /// <summary>
        /// Значение формулы (например, H2O)
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
        /// Наименование формулы (например, "вода")
        /// </summary>
        public string Name
        {
            get
            {
                return this.GetStringValue(ATTR_NAME);
            }
            set
            {
                this.AddSlot(ATTR_NAME, value, true, 0);
            }
        }
        public override string ToStringEx(bool shortVariant, Pullenti.Morph.MorphLang lang = null, int lev = 0)
        {
            string res = Value;
            if (res == null) 
            {
                string nam = Name;
                if (nam == null) 
                    return "?";
                return nam.ToLower();
            }
            if (!shortVariant && Name != null) 
            {
                List<string> names = this.GetStringValues(ATTR_NAME);
                StringBuilder tmp = new StringBuilder();
                tmp.AppendFormat("{0} (", res);
                for (int i = 0; i < names.Count; i++) 
                {
                    if (i > 0) 
                        tmp.Append(", ");
                    tmp.Append(names[i].ToLower());
                }
                tmp.Append(")");
                res = tmp.ToString();
            }
            return res;
        }
        public override bool CanBeEquals(Pullenti.Ner.Referent obj, Pullenti.Ner.Core.ReferentsEqualType typ = Pullenti.Ner.Core.ReferentsEqualType.WithinOneText)
        {
            ChemicalFormulaReferent cf = obj as ChemicalFormulaReferent;
            if (cf == null) 
                return false;
            if (Value != null && cf.Value != null) 
                return Value == cf.Value;
            if (Name != null && cf.Name != null) 
                return Name == cf.Name;
            return false;
        }
        public override Pullenti.Ner.Core.IntOntologyItem CreateOntologyItem()
        {
            Pullenti.Ner.Core.IntOntologyItem oi = new Pullenti.Ner.Core.IntOntologyItem(this);
            oi.Termins.Add(new Pullenti.Ner.Core.Termin(Value ?? Name));
            return oi;
        }
    }
}