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
    /// Class that should be accessed to validate an iban code.
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
    public class IbanValidator
    {
        private IIbanManager _manager;

        /// <summary>
        /// Constructor.
        /// </summary>
        public IbanValidator(){}

        /// <summary>
        /// Synchronous method to validate an iban code.
        /// </summary>
        /// <param name="iban">The given iban code.</param>
        /// <returns>'True' if iban is valid, otherwise 'false'.</returns>
        public bool ValidateIban(string iban)
        {
            if (string.IsNullOrWhiteSpace(iban))
                throw new IbanException(IbanExceptionType.IbanValidationIbanNeeded);

            _manager = ContainerBootstrapper.Resolve<IIbanManager>(iban.Substring(0, 2));

            return _manager.ValidateIban(iban);
        }

        /// <summary>
        /// Asynchronous method to validate an iban code.
        /// </summary>
        /// <param name="iban">The given iban code.</param>
        /// <returns>'True' if iban is valid, otherwise 'false'.</returns>
        public async Task<bool> ValidateIbanAsync(string iban)
        {
            if (string.IsNullOrWhiteSpace(iban))
                throw new IbanException(IbanExceptionType.IbanValidationIbanNeeded);

            _manager = ContainerBootstrapper.Resolve<IIbanManager>(iban.Substring(0, 2));

            return await _manager.ValidateIbanAsync(iban);
        }
    }
}
