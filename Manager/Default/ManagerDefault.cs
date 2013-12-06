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
using AS.IBAN.DAL;
using AS.IBAN.Helper;

namespace AS.IBAN.Manager.Default
{
    /// <summary>
    /// The default manager class. This class is responsible if no language specific manager was found.
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
    public class ManagerDefault : IbanManager, IIbanManager
    {
        private IDataService _dataService;

        /// <summary>
        /// Constructor of class.
        /// </summary>
        /// <param name="dataService">The</param>
        public ManagerDefault(IDataService dataService) : base(dataService)
        {
            _dataService = dataService;
        }

        /// <summary>
        /// Synchronous method to validate an given iban code.
        /// </summary>
        /// <param name="sIban">The given i ban code as string, which should be validated.</param>
        /// <returns>'True' if iban code is valid, otherwise 'false'.</returns>
        public bool ValidateIban(string sIban)
        {
            //  convert string to object
            Iban iban = this.ConvertStringToIban(sIban);

            //  if modulo97 of iban is 1, the iban is valid
            if (this.Modulo97(iban) == 1)
                return true;

            return false;
        }

        /// <summary>
        /// Synchronous method to generates an iban code and get the bic for that code.
        /// </summary>
        /// <param name="countryCode">The country for which the iban should be generated.</param>
        /// <param name="bankIdent">The bank ident for which the iban should be generated.</param>
        /// <param name="accountNumber">The account number for which the iban should be generated.</param>
        /// <returns>The generated result which contains iban and bic.</returns>
        /// <exception cref="IbanException">
        /// <list type="table">
        ///     <item>
        ///         <term><see cref="RuleType"/></term>
        ///         <description>BankIdentNotValid</description>
        ///         <description>When a given bank id is not valid (no results in datastore).</description>
        ///     </item>
        /// </list>
        /// </exception>
        public IbanBic GenerateIban(ECountry countryCode, string bankIdent, string accountNumber)
        {
            Country country = null;
            Bank bank = null;

            IbanBic result = null;

            //  1. load country
            country = _dataService.LoadCountry(countryCode);

            //  1.2 Check bank ident
            if (!this.CheckBankIdent(country, bankIdent))
                throw new IbanException(IbanExceptionType.BankIdentNotValid);

            //  2. load bank
            bank = _dataService.LoadBank(bankIdent);

            //  3. create default Iban
            Iban iban = new Iban() { AccountNumber = accountNumber, Country = country };

            //  3.1 create default  bank
            iban.Bank = new Bank() { BankIdentification = bankIdent };

            //  4 check if account number fits maximum length
            result = new IbanBic();
            result.IBAN = iban;
            result.IBAN.AccountNumber = this.CheckAccountNumber(country, result.IBAN.AccountNumber);

            //  5. Generate iban
            result.IBAN.BBAN = this.GetBBAN(result.IBAN);
            result.IBAN.Country = country;
            result.IBAN.CheckDigit = (98 - this.Modulo97(result.IBAN)).ToString("00");
            result.IBAN.IBAN = this.GetIban(result.IBAN);

            //  6. bic is null
            result.BIC = null;

            return result;
        }

        /// <summary>
        /// Asynchronous method to validate an given iban code.
        /// </summary>
        /// <param name="sIban">The given i ban code as string, which should be validated.</param>
        /// <returns>'True' if iban code is valid, otherwise 'false'.</returns>
        public async Task<bool> ValidateIbanAsync(string sIban)
        {
            bool result = false;

            await Task.Run(() =>
                {
                    result = this.ValidateIban(sIban);
                });

            return result;
        }

        /// <summary>
        /// Asynchronous method to generates an iban code and get the bic for that code.
        /// </summary>
        /// <param name="countryCode">The country for which the iban should be generated.</param>
        /// <param name="bankIdent">The bank ident for which the iban should be generated.</param>
        /// <param name="accountNumber">The account number for which the iban should be generated.</param>
        /// <returns>The generated result which contains iban and bic.</returns>
        public async Task<IbanBic> GenerateIbanAsync(ECountry countryCode, string bankIdent, string accountNumber)
        {
            IbanBic result = null;

            await Task.Run(() =>
                {
                    result = this.GenerateIban(countryCode, bankIdent, accountNumber);
                });

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            _dataService = null;
        }

        /// <summary>
        /// Synchronous method to get a bic to a given iban code.
        /// </summary>
        /// <param name="sIban">The given ibanc code.</param>
        /// <returns>The BIC that belongs to given iban.</returns>
        public BankIdentifierCode GetBic(string sIban)
        {
            throw new IbanException(IbanExceptionType.GetBicNoAllowedForCountry);
        }

        /// <summary>
        /// Asynchronous method to get a bic to a given iban code.
        /// </summary>
        /// <param name="sIban">The given ibanc code.</param>
        /// <returns>The BIC that belongs to given iban.</returns>
        public Task<BankIdentifierCode> GetBicAsync(string sIban)
        {
            throw new IbanException(IbanExceptionType.GetBicNoAllowedForCountry);
        }
    }
}
