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

namespace AS.IBAN.Helper
{
    /// <summary>
    /// Defines possible exception types that could erase during process.
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
    public enum IbanExceptionType
    {
        /// <summary>
        /// Failure during data load.
        /// </summary>
        DataLoadFailure = 0,

        /// <summary>
        /// No iban calculation allowed
        /// </summary>
        NoCalculation = 1,

        /// <summary>
        /// Failure during process of getting the bban
        /// </summary>
        BBANError = 2,

        /// <summary>
        /// Failure during iban generating
        /// </summary>
        IbanGeneratingError = 3,

        /// <summary>
        /// Not all required parameters for iban genrating where submitted
        /// </summary>
        IbanGeneratingNotAllParameters = 4,

        /// <summary>
        /// Iban code formatting failure during process of generating
        /// </summary>
        IbanGeneratingFormatting = 5,

        /// <summary>
        /// Check digit failure
        /// </summary>
        IbanGeneratingCheckDigit = 6,

        /// <summary>
        /// Iban code formatting failure during process of validating
        /// </summary>
        IbanValidatingFormatting = 7,

        /// <summary>
        /// A bank ident is not valid
        /// </summary>
        BankIdentNotValid = 8,

        /// <summary>
        /// For iban validation an iban code is needed
        /// </summary>
        IbanValidationIbanNeeded = 9,

        /// <summary>
        /// Getting a bic is not supported yet for this country
        /// </summary>
        GetBicNoAllowedForCountry = 10
    }
}
