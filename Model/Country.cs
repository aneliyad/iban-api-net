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
    /// Class reprasenting a country.
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
    public class Country : IDisposable
    {
        /// <summary>
        /// CountryType
        /// </summary>
        public ECountry CountryType { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        private string country_code = string.Empty;

        /// <summary>
        /// CountryCode
        /// </summary>
        public string CountryCode { get { return country_code;} set { this.country_code = value.ToUpper(); } }

        /// <summary>
        /// RegExp
        /// </summary>
        public string RegExp { get; set; }

        /// <summary>
        /// BankIdentLength
        /// </summary>
        public string BankIdentLength { get; set; }

        /// <summary>
        /// AccountNumberLength
        /// </summary>
        public string AccountNumberLength { get; set; }

        /// <summary>
        /// Compares two country objects.
        /// </summary>
        /// <param name="obj">The country object the current object should be compared to.</param>
        /// <returns>'True' if the two object are equal, otherwise 'false'.</returns>
        public override bool Equals(object obj)
        {
            Country country = (Country)obj;

            if (this.AccountNumberLength.Equals(country.AccountNumberLength)
                && this.BankIdentLength.Equals(country.BankIdentLength)
                && this.CountryCode.Equals(country.CountryCode)
                && this.CountryType.Equals(country.CountryType)
                && this.Name.Equals(country.Name)
                && this.RegExp.Equals(country.RegExp))
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

        /// <summary>
        /// Implements <see cref="System.IDisposable.Dispose()"/>
        /// </summary>
        public void Dispose()
        {
            Name = null;
            country_code = null;
            CountryCode = null;
            RegExp = null;
            BankIdentLength = null;
            AccountNumberLength = null;
        }
    }
}
