/*
 * SDK Pullenti Lingvo, version 4.33, fabruary 2026. Copyright (c) 2013-2026, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;

namespace Pullenti.Ner.Link
{
    /// <summary>
    /// Тип связи
    /// </summary>
    public enum LinkType : int
    {
        /// <summary>
        /// Не определено
        /// </summary>
        Undefined,
        /// <summary>
        /// Рождение
        /// </summary>
        Born,
        /// <summary>
        /// Родственные
        /// </summary>
        Family,
        /// <summary>
        /// Обучение
        /// </summary>
        Study,
        /// <summary>
        /// Работа
        /// </summary>
        Work,
        /// <summary>
        /// Контакт
        /// </summary>
        Contact,
        /// <summary>
        /// Паспорт и т.п.
        /// </summary>
        Document,
        /// <summary>
        /// Адрес, локация
        /// </summary>
        Address,
        /// <summary>
        /// Подразделение
        /// </summary>
        Unit,
    }
}