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
using AS.IBAN.Helper;
using AS.IBAN.Manager;
using AS.IBAN.Model;

namespace AS.IBAN
{
    /// <summary>
    /// Class that should be accessed for generating an iban code.
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
    public class IbanGenerator
    {
        private IIbanManager _manager;

        /// <summary>
        /// Constructor.
        /// </summary>
        public IbanGenerator() { }

        /// <summary>
        /// Synchronous method to generate an iban code.
        /// </summary>
        /// <param name="countryCode">Country for which the code should be generated.</param>
        /// <param name="bankIdent">Band ident for which the code should be generated.</param>
        /// <param name="accountNumber">Account number for which the code should be generated.</param>
        /// <returns>The generated iban code and bic code.</returns>
        public IbanBic GenerateIban(ECountry countryCode, string bankIdent, string accountNumber)
        {
            _manager = ContainerBootstrapper.Resolve<IIbanManager>(countryCode.ToString());

            return _manager.GenerateIban(countryCode, bankIdent, accountNumber);
        }

        /// <summary>
        /// Asynchronous method to generate an iban code.
        /// </summary>
        /// <param name="countryCode">Country for which the code should be generated.</param>
        /// <param name="bankIdent">Band ident for which the code should be generated.</param>
        /// <param name="accountNumber">Account number for which the code should be generated.</param>
        /// <returns>The generated iban code and bic code.</returns>
        public async Task<IbanBic> GenerateIbanAsync(ECountry countryCode, string bankIdent, string accountNumber)
        {
            _manager = ContainerBootstrapper.Resolve<IIbanManager>(countryCode.ToString());

            return await _manager.GenerateIbanAsync(countryCode, bankIdent, accountNumber);
        }
    }
}
