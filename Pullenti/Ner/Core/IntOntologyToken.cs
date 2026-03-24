/*
 * SDK Pullenti Lingvo, version 4.33, fabruary 2026. Copyright (c) 2013-2026, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */


namespace Pullenti.Ner.Core
{
    // Это привязка элемента внутренней отнологии к тексту
    public class IntOntologyToken : Pullenti.Ner.MetaToken
    {
        public IntOntologyToken(Pullenti.Ner.Token begin, Pullenti.Ner.Token end) : base(begin, end, null)
        {
        }
        public IntOntologyItem Item;
        public Termin Termin;
    }
}