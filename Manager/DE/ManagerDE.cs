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
using Microsoft.Practices.Unity;

namespace AS.IBAN.Manager.DE
{
    /// <summary>
    /// The german manager class.
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
    public class ManagerDE : IbanManager, IIbanManager
    {
        IDataService _dataService;
        RegexHelper _regexpHelper;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="dataservice">An object that implements IDataService interface.</param>
        public ManagerDE(IDataService dataservice) : base(dataservice)
        {
            _regexpHelper = new RegexHelper();
            _dataService = dataservice;
            
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

            //  generate iban and compare to given iban
            IbanBic generated = this.GenerateIban(iban.Country.CountryType, iban.Bank.BankIdentification, iban.AccountNumber);

            if (iban.Equals(generated.IBAN))
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
        ///         <description>NoCalculation</description>
        ///         <description>When iban calculation is not allowed.</description>
        ///     </item>
        /// </list>
        /// </exception>
        public IbanBic GenerateIban(ECountry countryCode, string bankIdent, string accountNumber)
        {
            Country country = null;
            Bank bank = null;
            Rule rule = null;

            IbanBic result = null;

            //  1. load country
            country = _dataService.LoadCountry(countryCode);

            //  1.2 Check bank ident
            if (!this.CheckBankIdent(country, bankIdent))
                throw new IbanException(IbanExceptionType.BankIdentNotValid);

            //  2. load bank
            bank = _dataService.LoadBank(bankIdent);

            //  3. load rule
            rule = _dataService.LoadRule(bank.Rule);

            //  4. take care of rule
            result = this.ConsiderRule(bank, rule, country, accountNumber);

            //  4.1 check if account number fits maximum length
            result.IBAN.AccountNumber = this.CheckAccountNumber(country, result.IBAN.AccountNumber);

            //  5. Generate iban
            result.IBAN.BBAN = this.GetBBAN(result.IBAN);
            result.IBAN.Country = country;
            result.IBAN.CheckDigit = (98 - this.Modulo97(result.IBAN)).ToString("00");
            result.IBAN.IBAN = this.GetIban(result.IBAN);

            return result;
        }

        /// <summary>
        /// Dispose method.
        /// </summary>
        public void Dispose()
        {
            _dataService = null;
            _regexpHelper = null;
        }

        #region private helpers
        /// <summary>
        /// Considers rules. When there are rules that match to bank ident or account number, those rules take effect.
        /// </summary>
        /// <param name="bank">The bank, for which the rules depend to.</param>
        /// <param name="rule">The rules.</param>
        /// <param name="country">The country the rules depend to.</param>
        /// <param name="accountNumber">The account number for which the rules should be checked for.</param>
        /// <returns>The generated result.</returns>
        /// <exception cref="IbanException">
        /// <list type="table">
        ///     <item>
        ///         <term><see cref="RuleType"/></term>
        ///         <description>NoCalculation</description>
        ///         <description>When iban calculation is not allowed.</description>
        ///     </item>
        /// </list>
        /// </exception>
        private IbanBic ConsiderRule(Bank bank, Rule rule, Country country, string accountNumber)
        {
            string temp_bank_ident = bank.BankIdentification;
            string temp_account_number = accountNumber;
            string temp_bic = bank.BIC.Bic;
            Iban iban = new Iban();
            IbanBic result = new IbanBic();

            //  Check if rule is 000000
            if (rule.RuleID.Equals("000000"))
            {
                iban.Bank = new Bank();
                iban.Bank.BankIdentification = bank.BankIdentification;
                iban.AccountNumber = accountNumber;

                result.IBAN = iban;
                result.BIC = new BankIdentifierCode() { Bic = temp_bic };

                return result;
            }

            //  Check if rule is 000100
            if (rule.RuleID.Equals("000100"))
                throw new IbanException(IbanExceptionType.NoCalculation);

            //  delete leading 0s in account number
            temp_account_number = temp_account_number.TrimStart(new char[] { '0' });

            //  consider each possible rule
            //  1. No Calculation
            if ((from count in rule.RuleElements
                 where count.RuleType == RuleType.No_Calculation
                 && (from bi in count.Childs
                     where bi.RuleType == RuleType.Kto_Number_Range
                     && _regexpHelper.RegexpMatch(bi.Attributes.Where(t => t.Key.Equals("blz")).Select(t => t.Value).First(), temp_bank_ident)
                     && _regexpHelper.RegexpMatch(bi.Data, temp_account_number)
                     select bi).Count() == 1
                 select count).Count() > 0)
            {
                //  No calculation
                throw new IbanException(IbanExceptionType.NoCalculation);
            }

            //  2. Kto Mapping
            if ((from count in rule.RuleElements
                    where count.RuleType == RuleType.Mappings_Kto
                    && _regexpHelper.RegexpMatch(count.Attributes.Where(t => t.Key.Equals("blz")).Select(t => t.Value).First(), temp_bank_ident)
                    && (from mapp in count.Childs
                        where mapp.RuleType == RuleType.Mapping
                        && _regexpHelper.RegexpMatch(mapp.Attributes.Where(t => t.Key.Equals("from")).Select(t => t.Value).First(), temp_account_number)
                        select mapp).Count() == 1
                    select count).Count() == 1)
            {
                var kto_mapping = (from kto in rule.RuleElements
                                    where kto.RuleType == RuleType.Mappings_Kto
                                    && _regexpHelper.RegexpMatch(kto.Attributes.Where(t => t.Key.Equals("blz")).Select(t => t.Value).First(), temp_bank_ident)
                                    && (from mapp in kto.Childs
                                        where mapp.RuleType == RuleType.Mapping
                                        && _regexpHelper.RegexpMatch(mapp.Attributes.Where(t => t.Key.Equals("from")).Select(t => t.Value).First(), temp_account_number)
                                        select mapp).Count() == 1
                                    select kto).First();

                //  account number
                temp_account_number = (from mapp in kto_mapping.Childs
                                        where mapp.RuleType == RuleType.Mapping
                                        && _regexpHelper.RegexpMatch(mapp.Attributes.Where(t => t.Key.Equals("from")).Select(t => t.Value).First(), temp_account_number)
                                        select mapp.Data).First();

                //  should also bank ident be mapped?
                if (kto_mapping.Attributes.Where(t => t.Key.Equals("blz_new")).Select(t => t.Value).Count() == 1)
                {
                    //  there should also be a bank ident mapping
                    temp_bank_ident = kto_mapping.Attributes.Where(t => t.Key.Equals("blz_new")).Select(t => t.Value).First();

                    //  if bank ident has changed, get new BIC
                    using (Bank bank_temp = _dataService.LoadBank(temp_bank_ident))
                    {
                        temp_bic = bank_temp.BIC.Bic;
                    }
                }
            }

            //  3. KtoKr Mapping
            if ((from count in rule.RuleElements
                    where count.RuleType == RuleType.Mappings_KtoKr
                    && _regexpHelper.RegexpMatch(count.Attributes.Where(t => t.Key.Equals("kto")).Select(t => t.Value).First(), temp_account_number)
                    && (from mapp in count.Childs
                        where mapp.RuleType == RuleType.Mapping
                        && mapp.Attributes.Where(t => t.Key.Equals("from")).Select(t => t.Value).First().Equals(temp_account_number.Substring(0, 3))
                        select mapp).Count() == 1
                    select count).Count() == 1)
            {
                var ktokr_mapping = (from ktokr in rule.RuleElements
                                        where ktokr.RuleType == RuleType.Mappings_KtoKr
                                        && _regexpHelper.RegexpMatch(ktokr.Attributes.Where(t => t.Key.Equals("kto")).Select(t => t.Value).First(), temp_account_number)
                                        && (from mapp in ktokr.Childs
                                            where mapp.RuleType == RuleType.Mapping
                                            && mapp.Attributes.Where(t => t.Key.Equals("from")).Select(t => t.Value).First().Equals(temp_account_number.Substring(0, 3))
                                            select mapp).Count() == 1
                                        select ktokr).First();

                temp_bank_ident = (from mapp in ktokr_mapping.Childs
                                    where mapp.RuleType == RuleType.Mapping
                                    && mapp.Attributes.Where(t => t.Key.Equals("from")).Select(t => t.Value).First().Equals(temp_account_number.Substring(0, 3))
                                    select mapp.Data).First();

                //  if bank ident has changed, get new BIC
                using (Bank bank_temp = _dataService.LoadBank(temp_bank_ident))
                {
                    temp_bic = bank_temp.BIC.Bic;
                }
            }

            //  4. Bank-Ident Mapping
            if ((from count in rule.RuleElements
                    where count.RuleType == RuleType.Mappings_Blz
                    && (from mapp in count.Childs
                        where mapp.RuleType == RuleType.Mapping
                        && _regexpHelper.RegexpMatch(mapp.Attributes.Where(t => t.Key.Equals("from")).Select(t => t.Value).First(), temp_bank_ident)
                        select mapp).Count() == 1
                    select count).Count() == 1)
            {
                var bankident_mapping = (from bi in rule.RuleElements
                                            where bi.RuleType == RuleType.Mappings_Blz
                                                && (from mapp in bi.Childs
                                                    where mapp.RuleType == RuleType.Mapping
                                                    && _regexpHelper.RegexpMatch(mapp.Attributes.Where(t => t.Key.Equals("from")).Select(t => t.Value).First(), temp_bank_ident)
                                                    select mapp).Count() == 1
                                            select bi).First();

                temp_bank_ident = (from mapp in bankident_mapping.Childs
                                    where mapp.RuleType == RuleType.Mapping
                                    && _regexpHelper.RegexpMatch(mapp.Attributes.Where(t => t.Key.Equals("from")).Select(t => t.Value).First(), temp_bank_ident)
                                    select mapp.Data).First();

                //  if bank ident has changed, get new BIC
                using (Bank bank_temp = _dataService.LoadBank(temp_bank_ident))
                {
                    temp_bic = bank_temp.BIC.Bic;
                }
            }

            //  5. Kto Mod
            if ((from count in rule.RuleElements
                    where count.RuleType == RuleType.Modification_Kto
                    && (from mapp in count.Childs
                        where mapp.RuleType == RuleType.Modification
                        && _regexpHelper.RegexpMatch(mapp.Attributes.Where(t => t.Key.Equals("blz")).Select(t => t.Value).First(), temp_bank_ident)
                        && _regexpHelper.RegexpMatch(mapp.Data, temp_account_number)
                        select mapp).Count() == 1
                    select count).Count() == 1)
            {
                var kto_mod = (from kto in rule.RuleElements
                                where kto.RuleType == RuleType.Modification_Kto
                                && (from mapp in kto.Childs
                                    where mapp.RuleType == RuleType.Modification
                                    && _regexpHelper.RegexpMatch(mapp.Attributes.Where(t => t.Key.Equals("blz")).Select(t => t.Value).First(), temp_bank_ident)
                                    && _regexpHelper.RegexpMatch(mapp.Data, temp_account_number)
                                    select mapp).Count() == 1
                                select kto).First();

                var mod = (from mapp in kto_mod.Childs
                            where mapp.RuleType == RuleType.Modification
                            && _regexpHelper.RegexpMatch(mapp.Attributes.Where(t => t.Key.Equals("blz")).Select(t => t.Value).First(), temp_bank_ident)
                            && _regexpHelper.RegexpMatch(mapp.Data, temp_account_number)
                            select mapp).First();

                _regexpHelper.RegexpMatch(mod.Data, temp_account_number, out temp_account_number);
            }

            //  6. BIC mapping
            if ((from count in rule.RuleElements
                 where count.RuleType == RuleType.Mappings_Bic
                 && (from mapp in count.Childs
                     where mapp.RuleType == RuleType.Mapping
                     && _regexpHelper.RegexpMatch(mapp.Attributes.Where(t => t.Key.Equals("blz")).Select(t => t.Value).First(), bank.BankIdentification)
                     select mapp).Count() == 1
                 select count).Count() == 1)
            {
                var mapp_bic = (from mbic in rule.RuleElements
                            where mbic.RuleType == RuleType.Mappings_Bic
                            && (from mapp in mbic.Childs
                                where mapp.RuleType == RuleType.Mapping
                                && _regexpHelper.RegexpMatch(mapp.Attributes.Where(t => t.Key.Equals("blz")).Select(t => t.Value).First(), bank.BankIdentification)
                                select mapp).Count() == 1
                            select mbic).First();

                temp_bic = (from bic in mapp_bic.Childs
                            where bic.RuleType == RuleType.Mapping
                             && _regexpHelper.RegexpMatch(bic.Attributes.Where(t => t.Key.Equals("blz")).Select(t => t.Value).First(), bank.BankIdentification)
                            select bic.Data).First();
            }

            iban.AccountNumber = temp_account_number;

            //  if bank ident changed, reload bank
            if (!temp_bank_ident.Equals(bank.BankIdentification))
            {
                bank = _dataService.LoadBank(temp_bank_ident);
            }

            iban.Bank = bank;

            result.IBAN = iban;
            result.BIC = new BankIdentifierCode() { Bic = temp_bic };

            return result;
        }
        #endregion


        #region Asynchronous
        /// <summary>
        /// Validates a given iban code asynchroun.
        /// </summary>
        /// <param name="sIban">The given iban code as <see cref="System.String"/></param>
        /// <returns>'True' if iban code is valid, 'false' otherwise.</returns>
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
        /// Generates an iban code.
        /// </summary>
        /// <param name="countryCode">The country for which the iban should be generated.</param>
        /// <param name="bankIdent">The bank ident for which the iban should be generated.</param>
        /// <param name="accountNumber">The account number for which the iban should be generated.</param>
        /// <returns>The generated result.</returns>
        public async Task<IbanBic> GenerateIbanAsync(ECountry countryCode, string bankIdent, string accountNumber)
        {
            IbanBic result = null;

            await Task.Run(() =>
                {
                    result = this.GenerateIban(countryCode, bankIdent, accountNumber);
                });

            return result;
        }
        #endregion

        /// <summary>
        /// Synchronous method to get a bic to a given iban code.
        /// </summary>
        /// <param name="sIban">The given ibanc code.</param>
        /// <returns>The BIC that belongs to given iban.</returns>
        public BankIdentifierCode GetBic(string sIban)
        {
            //  convert string to object
            Iban iban = this.ConvertStringToIban(sIban);

            //  generate iban and compare to given iban
            IbanBic generated = this.GenerateIban(iban.Country.CountryType, iban.Bank.BankIdentification, iban.AccountNumber);

            return generated.BIC;
        }

        /// <summary>
        /// Asynchronous method to get a bic to a given iban code.
        /// </summary>
        /// <param name="sIban">The given ibanc code.</param>
        /// <returns>The BIC that belongs to given iban.</returns>
        public async Task<BankIdentifierCode> GetBicAsync(string sIban)
        {
            BankIdentifierCode result = new BankIdentifierCode();

            await Task.Run(() =>
            {
                result = this.GetBic(sIban);
            });

            return result;
        }
    }
}
