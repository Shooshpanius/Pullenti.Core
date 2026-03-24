/*
 * SDK Pullenti Lingvo, version 4.33, fabruary 2026. Copyright (c) 2013-2026, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Chemical.Internal
{
    class ChemicalUnit
    {
        public ChemicalUnit(string mnem, string nam, string lat = null)
        {
            Mnem = mnem;
            NameCyr = nam;
            NameLat = lat;
        }
        public string NameCyr;
        public string NameLat;
        public string Mnem;
        public override string ToString()
        {
            return string.Format("{0} ({1})", Mnem, NameCyr);
        }
    }
}