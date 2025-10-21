/*
 * SDK Pullenti Lingvo, version 4.31, august 2025. Copyright (c) 2013-2025, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Core
{
    /// <summary>
    /// Результат сравнения номеров
    /// </summary>
    public enum ComplexNumCompareType : int
    {
        /// <summary>
        /// Несравнимы
        /// </summary>
        Uncomparable = 0,
        /// <summary>
        /// Равны
        /// </summary>
        Equals = 1,
        /// <summary>
        /// Первый раньше второго
        /// </summary>
        Less = 2,
        /// <summary>
        /// Второй раньше первого
        /// </summary>
        Great = 3,
        /// <summary>
        /// Не помню, что это...
        /// </summary>
        Comparable = 4,
    }
}