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

namespace AS.IBAN.Model
{
    /// <summary>
    /// Class reprasenting a iban.
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
    public class Iban : IDisposable
    {
        /// <summary>
        /// IBAN
        /// </summary>
        public string IBAN { get; set; }

        /// <summary>
        /// BBAN
        /// </summary>
        public string BBAN { get; set; }

        /// <summary>
        /// Country
        /// </summary>
        public Country Country { get; set; }

        /// <summary>
        /// CheckDigit
        /// </summary>
        public string CheckDigit { get; set; }

        /// <summary>
        /// BankIdent
        /// </summary>
        public Bank Bank { get; set; }

        /// <summary>
        /// AccountNumber
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// Implements <see cref="System.IDisposable.Dispose()"/>
        /// </summary>
        public void Dispose()
        {
            IBAN = null;
            BBAN = null;
            Country.Dispose();
            CheckDigit = null;
            Bank.Dispose();
            AccountNumber = null;
        }

        /// <summary>
        /// Compares two iban objects.
        /// </summary>
        /// <param name="obj">The iban object the current object should be compared to.</param>
        /// <returns>'True' if the two object are equal, otherwise 'false'.</returns>
        public override bool Equals(object obj)
        {
            Iban iban = (Iban)obj;

            if (this.IBAN.Equals(iban.IBAN)
                && this.BBAN.Equals(iban.BBAN)
                && this.Country.Equals(iban.Country)
                && this.CheckDigit.Equals(iban.CheckDigit)
                && this.Bank.BankIdentification.Equals(iban.Bank.BankIdentification)
                && this.AccountNumber.Equals(iban.AccountNumber))
                return true;

            return false;
        }

        /// <summary>
        /// Overrides GetHashCode.
        /// </summary>
        /// <returns>The hash code for this object.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
