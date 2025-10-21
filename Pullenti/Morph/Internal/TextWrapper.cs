/*
 * SDK Pullenti Lingvo, version 4.31, august 2025. Copyright (c) 2013-2025, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Morph.Internal
{
    // Введено для ускорения Питона!
    class TextWrapper
    {
        public TextWrapper(string txt, bool toUpper)
        {
            if (toUpper && txt != null) 
                Text = txt.ToUpper();
            else 
                Text = txt;
            Length = (txt == null ? 0 : txt.Length);
            Chars = new CharsList(txt);
        }
        public override string ToString()
        {
            return Text.ToString();
        }
        public class CharsList
        {
            public CharsList(string txt)
            {
                Text = txt;
                Count = (txt == null ? 0 : txt.Length);
            }
            public string Text;
            public int Count;
            public Pullenti.Morph.Internal.UnicodeInfo this[int ind]
            {
                get
                {
                    return Pullenti.Morph.Internal.UnicodeInfo.GetChar(Text[ind]);
                }
            }
        }

        public CharsList Chars;
        public string Text;
        public int Length;
    }
}