/*
 * Copyright 2013 AVENTUM SOLUTIONS GmbH (http://www.aventum-solutions.de)
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *   http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * -------------------------------------------------------------------------------
 * 
 * You need any changes or additional functions and you haven't time or knowledge?
 * Don't hesitate to contact us. We can help you.
 * http://www.aventum-solutions.de
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Reflection;
using System.IO;
using AS.IBAN.Helper;
using AS.IBAN.Model;

namespace AS.IBAN.DAL.DE
{
    /// <summary>
    /// The xml data service implementation for german iban.
    /// <list type="table">
    ///     <item>
    ///         <term>Version</term>
    ///         <description>1.0  Nov. 2013</description>
    ///     </item>
    ///     <item>
    ///         <term>Author</term>
    ///         <description>AVENTUM SOLUTIONS GmbH (<a target="_blank" href="http://www.aventum-solutions.de">http://www.aventum-solutions.de</a>)</description>
    ///     </item>
    /// </list>
    /// </summary>
    public class XmlDataServiceDE : XmlData, IDataService
    {
        private string _fileNameBank = string.Empty;
        private string _fileNameRule = string.Empty;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="fileNameBank">Filename of xml file containing german banks.</param>
        /// <param name="fileNameIbanFormat">Filename of xml file containing iban formats of each country.</param>
        /// <param name="fileNameRule">Filename of xml file containing german iban rules.</param>
        public XmlDataServiceDE(string fileNameBank, string fileNameIbanFormat, string fileNameRule) : base(fileNameIbanFormat)
        {
            _fileNameBank = fileNameBank;
            _fileNameRule = fileNameRule;
        }

        #region Bank
        /// <summary>
        /// Loads a bank object.
        /// </summary>
        /// <param name="bankIdent">The bank ident of the bank that should be loaded.</param>
        /// <returns>The loaded bank.</returns>
        public Bank LoadBank(string bankIdent)
        {
            //  1. load bank
            IEnumerable<XElement> bankData;

            bankData = LoadData(_fileNameBank, "bank", new KeyValuePair<string, string>("blz", "_" + bankIdent));
            
            Bank bank = new Bank();

            bank = (from ba in bankData
                    select new Bank
                    {
                        BankIdentification = bankIdent,
                        Name = ba.Element(XmlNamespace + "name").Value,
                        Rule = ba.Element(XmlNamespace + "rule").Value,
                        BIC = new BankIdentifierCode() { Bic = ba.Element(XmlNamespace + "bic").Value }
                    }).First();
            
            return bank;
        }
        #endregion

        #region Rule
        /// <summary>
        /// Loads a rule object.
        /// </summary>
        /// <param name="ruleID">The rule of of the rule that should be loaded.</param>
        /// <returns>The loaded rule.</returns>
        public Rule LoadRule(string ruleID)
        {
            //  Load searched rule from file and get all elements
            IEnumerable<XElement> rules = LoadData(_fileNameRule, "rule", new KeyValuePair<string, string>("id", "_" + ruleID));

            //  new rule
            Rule rule = new Rule();
            rule.RuleID = ruleID;
            rule.RuleElements = new List<RuleElement>();

            if (rules.Elements().Count() > 0)
            {
                rules = rules.Elements();

                rule.RuleElements = this.GetRuleElements(rules);
            }

            return rule;
        }
        #endregion

        /// <summary>
        /// Gets all rule elements out of an XElement.
        /// </summary>
        /// <param name="elements">The XElement which includes the rule elements.</param>
        /// <returns>List of rule elements.</returns>
        private IEnumerable<RuleElement> GetRuleElements(IEnumerable<XElement> elements)
        {
            List<RuleElement> ruleElements = new List<RuleElement>();

            foreach (XElement element in elements)
            {
                RuleElement ruleElement = new RuleElement();
                ruleElement.RuleType = this.GetRuleElementType(element.Name.LocalName);

                //  Any Childs?
                if (element.Descendants().Count() > 0)
                    ruleElement.Childs = this.GetRuleElements(element.Descendants());
                else
                    ruleElement.Data = element.Value;

                //  Key and value?
                if (element.HasAttributes)
                {
                    foreach (XAttribute attribute in element.Attributes())
                    {
                        ruleElement.Attributes.Add(new KeyValuePair<string, string>(attribute.Name.LocalName,attribute.Value));
                    }
                }

                var test = ruleElement.Attributes.Where(t => t.Key.Equals("")).Select(t => t.Value);

                ruleElements.Add(ruleElement);
            }

            return ruleElements;
        }

        /// <summary>
        /// Gets a RuleType-object for a given key.
        /// </summary>
        /// <param name="key">The given Key.</param>
        /// <returns>The corresponding RuleType.</returns>
        private RuleType GetRuleElementType(string key)
        {
            switch (key)
            {
                case "no_calculation": return RuleType.No_Calculation;
                case "kto_number_range": return RuleType.Kto_Number_Range;
                case "mappings_kto": return RuleType.Mappings_Kto;
                case "mapping": return RuleType.Mapping;
                case "modification_kto": return RuleType.Modification_Kto;
                case "modification": return RuleType.Modification;
                case "mappings_blz": return RuleType.Mappings_Blz;
                case "mappings_ktokr": return RuleType.Mappings_KtoKr;
                case "mappings_bic": return RuleType.Mappings_Bic;
                default: return RuleType.No_Calculation;
            }
        }
    }
}
