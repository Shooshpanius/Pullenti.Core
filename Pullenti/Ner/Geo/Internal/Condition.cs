/*
 * SDK Pullenti Lingvo, version 4.33, fabruary 2026. Copyright (c) 2013-2026, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Text;

namespace Pullenti.Ner.Geo.Internal
{
    class Condition
    {
        public Pullenti.Ner.Token GeoBeforeToken;
        public bool PureGeoBefore;
        public override string ToString()
        {
            StringBuilder tmp = new StringBuilder();
            if (GeoBeforeToken != null) 
                tmp.AppendFormat("GeoBefore={0}", GeoBeforeToken);
            return tmp.ToString();
        }
        public bool Check()
        {
            if (GeoBeforeToken != null) 
            {
                if (MiscLocationHelper.CheckGeoObjectBefore(GeoBeforeToken, PureGeoBefore)) 
                    return true;
                if (MiscLocationHelper.IsUserParamQuiry(GeoBeforeToken)) 
                    return true;
            }
            return false;
        }
    }
}