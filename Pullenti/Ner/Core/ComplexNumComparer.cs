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

namespace Pullenti.Ner.Core
{
    /// <summary>
    /// Сравнение номеров (функцией Process)
    /// </summary>
    public class ComplexNumComparer
    {
        /// <summary>
        /// Сюда запишется результат
        /// </summary>
        public ComplexNumCompareType Typ;
        /// <summary>
        /// Ранг сравнения
        /// </summary>
        public double Rank;
        /// <summary>
        /// Дельта между номерами, если номера сравниваемые
        /// </summary>
        public int Delta;
        /// <summary>
        /// Это если разных уровней, но могут следовать друг за другом (1 - 1.1 - 2)
        /// </summary>
        public bool CanFollow;
        /// <summary>
        /// Первый сравниваемый номер
        /// </summary>
        public ComplexNumToken First;
        /// <summary>
        /// Второй сравниваемый номер
        /// </summary>
        public ComplexNumToken Second;
        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            if (Rank > 0) 
                res.AppendFormat("{0}: ", Rank);
            res.Append(First);
            if (Typ == ComplexNumCompareType.Uncomparable) 
                res.Append(" ?? ");
            else if (Typ == ComplexNumCompareType.Equals) 
                res.Append(" == ");
            else if (Typ == ComplexNumCompareType.Less) 
                res.Append(" < ");
            else if (Typ == ComplexNumCompareType.Great) 
                res.Append(" > ");
            res.Append(Second);
            if (Delta > 0) 
                res.AppendFormat(", Delt={0}", Delta);
            if (CanFollow) 
                res.AppendFormat(", follow");
            return res.ToString();
        }
        SingleNumComparer m_Comp = new SingleNumComparer();
        /// <summary>
        /// Сравнить два номера
        /// </summary>
        public void Process(ComplexNumToken fir, ComplexNumToken sec)
        {
            First = fir;
            Second = sec;
            Typ = ComplexNumCompareType.Equals;
            Delta = 0;
            Rank = 1;
            if (fir.Prefix != sec.Prefix || fir.Suffix != sec.Suffix) 
            {
                if (((fir.Suffix == null && sec.Suffix == ".")) || ((fir.Suffix == "." && sec.Suffix == null))) 
                    Rank *= 0.98;
                else 
                    Rank *= 0.8;
            }
            int i;
            for (i = 0; (i < fir.Nums.Count) && (i < sec.Nums.Count); i++) 
            {
                SingleNumToken n1 = fir.Nums[i];
                SingleNumToken n2 = sec.Nums[i];
                m_Comp.Process(n1, n2);
                if (m_Comp.Typ == ComplexNumCompareType.Uncomparable) 
                {
                    Typ = ComplexNumCompareType.Uncomparable;
                    Rank = 0;
                    return;
                }
                if (m_Comp.Typ == ComplexNumCompareType.Equals && fir.Nums.Count == sec.Nums.Count) 
                {
                    Rank *= m_Comp.Rank;
                    continue;
                }
                if ((i + 1) == fir.Nums.Count && (i + 1) == sec.Nums.Count) 
                {
                    Typ = m_Comp.Typ;
                    Rank *= m_Comp.Rank;
                    Delta = m_Comp.Delta;
                }
                else if (((i + 1) < fir.Nums.Count) && ((i + 1) < sec.Nums.Count)) 
                {
                    Typ = ComplexNumCompareType.Uncomparable;
                    Rank = 0;
                    break;
                }
                else if ((i + 1) == fir.Nums.Count) 
                {
                    if (m_Comp.Typ == ComplexNumCompareType.Equals) 
                    {
                        Typ = ComplexNumCompareType.Less;
                        Rank *= m_Comp.Rank;
                        if (sec.Nums.Count == (fir.Nums.Count + 1) && sec.Nums[i + 1].IsOne) 
                        {
                            CanFollow = true;
                            Rank /= 2;
                        }
                        else 
                            Rank /= 2;
                        break;
                    }
                    else if (m_Comp.Typ == ComplexNumCompareType.Less) 
                    {
                        Typ = ComplexNumCompareType.Uncomparable;
                        Rank = 0;
                        break;
                    }
                    else 
                    {
                        Typ = ComplexNumCompareType.Great;
                        Rank *= m_Comp.Rank;
                        if (m_Comp.Delta != 1) 
                            Rank /= 2;
                        break;
                    }
                }
                else if ((i + 1) == sec.Nums.Count) 
                {
                    if (m_Comp.Typ == ComplexNumCompareType.Equals) 
                    {
                        Typ = ComplexNumCompareType.Great;
                        Rank *= m_Comp.Rank;
                        if (fir.Nums.Count == (sec.Nums.Count + 1) && fir.Nums[i + 1].IsOne) 
                            Rank /= 2;
                        else 
                            Rank /= 2;
                        break;
                    }
                    else if (m_Comp.Typ == ComplexNumCompareType.Less) 
                    {
                        Typ = ComplexNumCompareType.Less;
                        Rank *= m_Comp.Rank;
                        if (m_Comp.Delta != 1) 
                            Rank /= 2;
                        else 
                            CanFollow = true;
                        break;
                    }
                    else 
                    {
                        Typ = ComplexNumCompareType.Uncomparable;
                        Rank = 0;
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// Выделить главную подпоследовательность верхнего уровня из последовательности номеров
        /// </summary>
        /// <param name="nums">исходная последовательность</param>
        /// <return>результат или null</return>
        public static List<ComplexNumToken> GetMainSequence(List<ComplexNumToken> nums)
        {
            if (nums == null || (nums.Count < 2)) 
                return null;
            List<ComplexNumToken> res = new List<ComplexNumToken>();
            res.Add(nums[0]);
            ComplexNumComparer cmp = new ComplexNumComparer();
            for (int i = 1; i < nums.Count; i++) 
            {
                double max = (double)0;
                int ii = -1;
                ComplexNumToken num0 = res[res.Count - 1];
                for (int j = i; j < nums.Count; j++) 
                {
                    if (j == 40) 
                    {
                    }
                    cmp.Process(num0, nums[j]);
                    if (cmp.Typ != ComplexNumCompareType.Less) 
                        continue;
                    if (cmp.Delta != 1) 
                        continue;
                    if (cmp.Rank > max) 
                    {
                        max = cmp.Rank;
                        ii = j;
                    }
                }
                if (max < 0.9) 
                    continue;
                i = ii;
                res.Add(nums[ii]);
            }
            if (res.Count > 1) 
                return res;
            return null;
        }
    }
}