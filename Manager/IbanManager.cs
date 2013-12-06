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
using AS.IBAN.Helper;
using AS.IBAN.DAL;

namespace AS.IBAN.Manager
{
    /// <summary>
    /// An abstract class that defines methods for all iban manager classes.
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
    public abstract class IbanManager
    {
        private IDataServiceDefault _dataService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="dataService">An object that implements IDataServiceDefault interface.</param>
        public IbanManager(IDataServiceDefault dataService )
        {
            _dataService = dataService;
        }

        /// <summary>
        /// Calculates the modulo of 97 for a iban.
        /// </summary>
        /// <param name="iban">The given iban for which the modulo 97 should be calculated.</param>
        /// <returns>The calculated result.</returns>
        /// <exception cref="IbanException">
        /// <list type="table">
        ///     <item>
        ///         <term><see cref="RuleType"/></term>
        ///         <description>IbanGeneratingCheckDigit</description>
        ///         <description>When check digit could not be calculated.</description>
        ///     </item>
        /// </list>
        /// </exception>
        protected int Modulo97(Iban iban)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(iban.CheckDigit))
                    iban.CheckDigit = "00";

                string input = this.BbanToNumber(iban.BBAN) + CountryCodeToNumber(iban.Country.CountryCode) + iban.CheckDigit;
                string output = string.Empty;

                for (int i = 0; i < input.Length; i++)
                {
                    if (output.Length < 9)
                        output += input[i];
                    else
                    {
                        output = (Int32.Parse(output) % 97).ToString() + input[i];
                    }
                }

                return Int32.Parse(output) % 97;
            }
            catch (Exception ex)
            {
                throw new IbanException(ex, IbanExceptionType.IbanGeneratingCheckDigit);
            }
        }

        /// <summary>
        /// Convcerts bban with letters into bban of numbers.
        /// </summary>
        /// <param name="bban">The given bban.</param>
        /// <returns>The converted bban.</returns>
        private string BbanToNumber(string bban)
        {
            long lbban = 0;

            if (Int64.TryParse(bban, out lbban))
                return bban;

            string result = string.Empty;

            for (int i = 0; i < bban.Length; i++)
            {
                if (Int64.TryParse(bban[i].ToString(), out lbban))
                    result += bban[i];
                else
                    result += (int)bban[i] - 55;
            }

            return result;
        }

        /// <summary>
        /// Converts a country code into a string of numbers.
        /// </summary>
        /// <param name="country_code">The given country code.</param>
        /// <returns>The converted country code.</returns>
        private string CountryCodeToNumber(string country_code)
        {
            string result = string.Empty;

            for (int i = 0; i < country_code.Length; i++)
            {
                result += (int)country_code[i] - 55;
            }

            return result;
        }

        /// <summary>
        /// Gets the bban out of an iban.
        /// </summary>
        /// <param name="iban">The given iban.</param>
        /// <returns>The bban.</returns>
        /// <exception cref="IbanException">
        /// <list type="table">
        ///     <item>
        ///         <term><see cref="RuleType"/></term>
        ///         <description>BBANError</description>
        ///         <description>When bban could not be calculated.</description>
        ///     </item>
        /// </list>
        /// </exception>
        protected string GetBBAN(Iban iban)
        {
            if (string.IsNullOrWhiteSpace(iban.AccountNumber) || string.IsNullOrWhiteSpace(iban.Bank.BankIdentification))
            {
                throw new IbanException(IbanExceptionType.BBANError);
            }

            string bban = iban.Bank.BankIdentification + iban.AccountNumber;

            return bban;
        }

        /// <summary>
        /// Gets the iban code out of a Iban object (which does not contain the iban code yet).
        /// </summary>
        /// <param name="iban">The given iban object.</param>
        /// <returns>The iban code as string.</returns>
        /// /// <exception cref="IbanException">
        /// <list type="table">
        ///     <item>
        ///         <term><see cref="RuleType"/></term>
        ///         <description>IbanGeneratingNotAllParameters</description>
        ///         <description>If not all parameters are available for iban converting.</description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="RuleType"/></term>
        ///         <description>IbanGeneratingFormatting</description>
        ///         <description>If iban format is not valid.</description>
        ///     </item>
        /// </list>
        /// </exception>
        protected string GetIban(Iban iban)
        {
            //  Check if all necessary parameters are set
            if (string.IsNullOrWhiteSpace(iban.AccountNumber)
                || string.IsNullOrWhiteSpace(iban.Bank.BankIdentification)
                || string.IsNullOrWhiteSpace(iban.BBAN)
                || string.IsNullOrWhiteSpace(iban.CheckDigit)
                || iban.Country == null
                || string.IsNullOrWhiteSpace(iban.Country.CountryCode))
            {
                throw new IbanException(IbanExceptionType.IbanGeneratingNotAllParameters);
            }

            //  iban formatting: CountryCode || check digit || bank ident || account number
            string IBAN = iban.Country.CountryCode + iban.CheckDigit + iban.BBAN;

            iban.IBAN = IBAN;

            //  check if iban is correct formatted
            if (!this.CheckIbanFormatting(iban))
            {
                //  iban not well formatted -> error
                throw new IbanException(IbanExceptionType.IbanGeneratingFormatting);
            }

            return IBAN;
        }

        /// <summary>
        /// Checks if a iban code fits to the country format rules.
        /// </summary>
        /// <param name="iban">The given iban.</param>
        /// <returns>'True' if the format is ok, otherwise 'false'.</returns>
        private bool CheckIbanFormatting(Iban iban)
        {
            RegexHelper regexpheler = new RegexHelper();

            if (regexpheler.RegexpMatch(iban.Country.RegExp, iban.IBAN))
                return true;

            return false;
        }

        /// <summary>
        /// Checks if a band ident fits the country format rules.
        /// </summary>
        /// <param name="country">The given country.</param>
        /// <param name="bankIdent">The given bank ident</param>
        /// <returns>'True' if the format is ok, otherwise 'false'.</returns>
        protected bool CheckBankIdent(Country country, string bankIdent)
        {
            if (Int32.Parse(country.BankIdentLength) != bankIdent.Length)
                return false;

            return true;
        }

        /// <summary>
        /// Checks the account number. If it's shorter than defined in the country format rules a '0' is prefixed 
        /// until it fits the required length.
        /// </summary>
        /// <param name="country">The given country.</param>
        /// <param name="accountNumber">The given account number</param>
        /// <returns>The account number fit to country format.</returns>
        protected string CheckAccountNumber(Country country, string accountNumber)
        {
            //  check if account number fits maximum length
            while (accountNumber.Length < Int32.Parse(country.AccountNumberLength))
            {
                accountNumber = "0" + accountNumber;
            }

            return accountNumber;
        }

        /// <summary>
        /// Converts a given iban as string into a Iban object.
        /// </summary>
        /// <param name="iban">The given iban as string.</param>
        /// <returns>The converted iban object.</returns>
        /// <exception cref="IbanException">
        /// <list type="table">
        ///     <item>
        ///         <term><see cref="RuleType"/></term>
        ///         <description>IbanValidatingFormatting</description>
        ///         <description>If iban format is not valid.</description>
        ///     </item>
        /// </list>
        /// </exception>
        protected Iban ConvertStringToIban(string iban)
        {
            try
            {
                Iban converted = new Iban
                {
                    Country = _dataService.LoadCountry((ECountry)Enum.Parse(typeof(ECountry), iban.Substring(0, 2), true)),
                    CheckDigit = iban.Substring(2, 2),
                    IBAN = iban
                };

                converted.Bank = new Bank();

                // get bank ident
                converted.Bank.BankIdentification = iban.Substring(4, Int32.Parse(converted.Country.BankIdentLength));

                //  get account number
                converted.AccountNumber = iban.Substring(4 + Int32.Parse(converted.Country.BankIdentLength), Int32.Parse(converted.Country.AccountNumberLength));

                //  check, if iban is well formatted
                if (!this.CheckIbanFormatting(converted))
                    throw new IbanException(IbanExceptionType.IbanValidatingFormatting);

                converted.BBAN = this.GetBBAN(converted);

                return converted;
            }
            catch (IbanException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new IbanException(ex, IbanExceptionType.IbanValidatingFormatting);
            }
        }
    }
}
