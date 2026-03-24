/*
 * SDK Pullenti Lingvo, version 4.33, fabruary 2026. Copyright (c) 2013-2026, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Chemical
{
    internal class MetaChemical : Pullenti.Ner.Metadata.ReferentClass
    {
        public static void Initialize()
        {
            GlobalMeta = new MetaChemical();
            GlobalMeta.AddFeature(ChemicalFormulaReferent.ATTR_VALUE, "Формула", 0, 1);
            GlobalMeta.AddFeature(ChemicalFormulaReferent.ATTR_NAME, "Текстовое определение", 0, 0);
        }
        public static Pullenti.Ner.Metadata.Feature Types;
        public override string Name
        {
            get
            {
                return ChemicalFormulaReferent.OBJ_TYPENAME;
            }
        }
        public override string Caption
        {
            get
            {
                return "Химическая формула";
            }
        }
        public static string ImageId = "chemical";
        public override string GetImageId(Pullenti.Ner.Referent obj = null)
        {
            return ImageId;
        }
        public static MetaChemical GlobalMeta;
    }
}