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

namespace AS.IBAN.Manager
{
    /// <summary>
    /// Interface for an iban manager. Defines methods that an iban manager should implement.
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
    public interface IIbanManager : IDisposable
    {
        //  synchronous

        /// <summary>
        /// Synchronous method to validate an given iban code.
        /// </summary>
        /// <param name="sIban">The given i ban code as string, which should be validated.</param>
        /// <returns>'True' if iban code is valid, otherwise 'false'.</returns>
        bool ValidateIban(string sIban);
        
        /// <summary>
        /// Synchronous method to generates an iban code and get the bic for that code.
        /// </summary>
        /// <param name="countryCode">The country for which the iban should be generated.</param>
        /// <param name="bankIdent">The bank ident for which the iban should be generated.</param>
        /// <param name="accountNumber">The account number for which the iban should be generated.</param>
        /// <returns>The generated result which contains iban and bic.</returns>
        IbanBic GenerateIban(ECountry countryCode, string bankIdent, string accountNumber);

        /// <summary>
        /// Synchronous method to get a bic to a given iban code.
        /// </summary>
        /// <param name="sIban">The given ibanc code.</param>
        /// <returns>The BIC that belongs to given iban.</returns>
        BankIdentifierCode GetBic(string sIban);


        //  asynchronous

        /// <summary>
        /// Asynchronous method to validate an given iban code.
        /// </summary>
        /// <param name="sIban">The given i ban code as string, which should be validated.</param>
        /// <returns>'True' if iban code is valid, otherwise 'false'.</returns>
        Task<bool> ValidateIbanAsync(string sIban);

        /// <summary>
        /// Asynchronous method to generates an iban code and get the bic for that code.
        /// </summary>
        /// <param name="countryCode">The country for which the iban should be generated.</param>
        /// <param name="bankIdent">The bank ident for which the iban should be generated.</param>
        /// <param name="accountNumber">The account number for which the iban should be generated.</param>
        /// <returns>The generated result which contains iban and bic.</returns>
        Task<IbanBic> GenerateIbanAsync(ECountry countryCode, string bankIdent, string accountNumber);

        /// <summary>
        /// Asynchronous method to get a bic to a given iban code.
        /// </summary>
        /// <param name="sIban">The given ibanc code.</param>
        /// <returns>The BIC that belongs to given iban.</returns>
        Task<BankIdentifierCode> GetBicAsync(string sIban);
    }
}
