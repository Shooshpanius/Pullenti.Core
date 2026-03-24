/*
 * SDK Pullenti Lingvo, version 4.33, fabruary 2026. Copyright (c) 2013-2026, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Ner.Person
{
    /// <summary>
    /// Результат нормализации короткого текста, где только персона
    /// </summary>
    public class PersonNormalData
    {
        /// <summary>
        /// Создать и обработать текст
        /// </summary>
        /// <param name="txt">текст с ФИО персоны, если они в разных полях ввода, то разделять переходом на новую строку</param>
        public PersonNormalData(string txt)
        {
            if (txt != null) 
                Pullenti.Ner.Person.Internal.PersonNormalHelper.Analyze(this, txt, null);
        }
        /// <summary>
        /// Фамилия
        /// </summary>
        public string Lastname;
        /// <summary>
        /// Фамилия дополнительно (наприимер, до замужества)
        /// </summary>
        public string LastnameAlt;
        /// <summary>
        /// Имя
        /// </summary>
        public string Firstname;
        /// <summary>
        /// Имя альтернативное (исходное уменьшительное, когда в Firstname восстановленное полное)
        /// </summary>
        public string FirstnameAlt;
        /// <summary>
        /// Отчество
        /// </summary>
        public string Middlename;
        /// <summary>
        /// Пол (1 - мужчина, 2 - женщина)
        /// </summary>
        public int Gender = 0;
        /// <summary>
        /// Тип результата
        /// </summary>
        public PersonNormalResult ResTyp = PersonNormalResult.Undefined;
        /// <summary>
        /// Коэффициент качества (100 - идеальное)
        /// </summary>
        public int Coef;
        /// <summary>
        /// Сообщение об ошибке (если есть)
        /// </summary>
        public string ErrorMessage;
        /// <summary>
        /// Откорректированные слова. 
        /// Ключ - исходного слово, Значение - коррекция
        /// </summary>
        public Dictionary<string, string> CorrWords = new Dictionary<string, string>();
        public override string ToString()
        {
            return string.Format("{0} ({1}): {2} {3} {4}", Coef, ResTyp, Lastname ?? "", Firstname ?? "", Middlename ?? "");
        }
    }
}