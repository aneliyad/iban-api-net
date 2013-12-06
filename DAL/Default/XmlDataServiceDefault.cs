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
using AS.IBAN.Model;

namespace AS.IBAN.DAL.Default
{
    /// <summary>
    /// The default xml data service implementation. This class is responsible if no language specific data service was found.
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
    public class XmlDataServiceDefault : XmlData, IDataService
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fileNameIbanFormat">Filename of xml file containing all iban formats.</param>
        public XmlDataServiceDefault(string fileNameIbanFormat)
            : base(fileNameIbanFormat){}

        /// <summary>
        /// Loads a bank object.
        /// </summary>
        /// <param name="bankIdent">The bank ident of the bank that should be loaded.</param>
        /// <returns>The loaded bank.</returns>
        public Bank LoadBank(string bankIdent)
        {
            return new Bank() { BankIdentification = bankIdent };
        }

        /// <summary>
        /// Loads a rule object.
        /// </summary>
        /// <param name="ruleID">The rule of of the rule that should be loaded.</param>
        /// <returns>The loaded rule.</returns>
        /// <exception cref="NotImplementedException">No default rules.</exception>
        public Rule LoadRule(string ruleID)
        {
            throw new NotImplementedException();
        }
    }
}
