/*
 * SDK Pullenti Lingvo, version 4.33, fabruary 2026. Copyright (c) 2013-2026, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Person
{
    /// <summary>
    /// Результат обработки
    /// </summary>
    public enum PersonNormalResult : int
    {
        /// <summary>
        /// Не выполнялось
        /// </summary>
        Undefined = 0,
        /// <summary>
        /// Это персона, проверка не требуется
        /// </summary>
        OK = 1,
        /// <summary>
        /// Необходима ручная проверка (коэффициент низкий)
        /// </summary>
        Manual = 2,
        /// <summary>
        /// Это не персона
        /// </summary>
        NotPerson = 3,
    }
}