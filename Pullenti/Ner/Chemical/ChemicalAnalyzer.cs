/*
 * SDK Pullenti Lingvo, version 4.31, august 2025. Copyright (c) 2013-2025, Pullenti. All rights reserved. 
 * Non-Commercial Freeware and Commercial Software.
 * This class is generated using the converter Unisharping (www.unisharping.ru) from Pullenti C# project. 
 * The latest version of the code is available on the site www.pullenti.ru
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Pullenti.Ner.Chemical
{
    /// <summary>
    /// Анализатор химических формул (специфический анализатор)
    /// </summary>
    public class ChemicalAnalyzer : Pullenti.Ner.Analyzer
    {
        /// <summary>
        /// Имя анализатора ("CHEMICAL")
        /// </summary>
        public const string ANALYZER_NAME = "CHEMICAL";
        public override string Name
        {
            get
            {
                return ANALYZER_NAME;
            }
        }
        public override string Caption
        {
            get
            {
                return "Химические формулы";
            }
        }
        public override string Description
        {
            get
            {
                return "Химические формулы и их текстовые аналоги";
            }
        }
        public override Pullenti.Ner.Analyzer Clone()
        {
            return new ChemicalAnalyzer();
        }
        /// <summary>
        /// Специфический анализатор
        /// </summary>
        public override bool IsSpecific
        {
            get
            {
                return true;
            }
        }
        public override ICollection<Pullenti.Ner.Metadata.ReferentClass> TypeSystem
        {
            get
            {
                return new Pullenti.Ner.Metadata.ReferentClass[] {MetaChemical.GlobalMeta};
            }
        }
        public override Dictionary<string, byte[]> Images
        {
            get
            {
                Dictionary<string, byte[]> res = new Dictionary<string, byte[]>();
                res.Add(MetaChemical.ImageId.ToString(), Pullenti.Ner.Core.Internal.ResourceHelper3.GetBytes("chemical.png"));
                return res;
            }
        }
        public override Pullenti.Ner.Referent CreateReferent(string type)
        {
            if (type == ChemicalFormulaReferent.OBJ_TYPENAME) 
                return new ChemicalFormulaReferent();
            return null;
        }
        public override int ProgressWeight
        {
            get
            {
                return 1;
            }
        }
        public override void Process(Pullenti.Ner.Core.AnalysisKit kit)
        {
            Pullenti.Ner.Core.AnalyzerData ad = kit.GetAnalyzerData(this);
            List<List<Pullenti.Ner.Chemical.Internal.ChemicalToken>> probs = new List<List<Pullenti.Ner.Chemical.Internal.ChemicalToken>>();
            for (Pullenti.Ner.Token t = kit.FirstToken; t != null; t = t.Next) 
            {
                List<Pullenti.Ner.Chemical.Internal.ChemicalToken> li = Pullenti.Ner.Chemical.Internal.ChemicalToken.TryParseList(t, 0);
                if (li == null || li.Count == 0) 
                    continue;
                t = li[li.Count - 1].EndToken;
                ChemicalFormulaReferent cf = Pullenti.Ner.Chemical.Internal.ChemicalToken.CreateReferent(li);
                if (cf == null) 
                {
                    probs.Add(li);
                    continue;
                }
                cf = ad.RegisterReferent(cf) as ChemicalFormulaReferent;
                Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(cf, li[0].BeginToken, t);
                kit.EmbedToken(rt);
                t = rt;
            }
            foreach (List<Pullenti.Ner.Chemical.Internal.ChemicalToken> pr in probs) 
            {
                ChemicalFormulaReferent cf = Pullenti.Ner.Chemical.Internal.ChemicalToken.CreateReferent(pr);
                if (cf == null) 
                    continue;
                cf = ad.RegisterReferent(cf) as ChemicalFormulaReferent;
                Pullenti.Ner.ReferentToken rt = new Pullenti.Ner.ReferentToken(cf, pr[0].BeginToken, pr[pr.Count - 1].EndToken);
                kit.EmbedToken(rt);
            }
        }
        public static void Initialize()
        {
            MetaChemical.Initialize();
            Pullenti.Ner.Chemical.Internal.ChemicalToken.Initialize();
            Pullenti.Ner.ProcessorService.RegisterAnalyzer(new ChemicalAnalyzer());
        }
    }
}