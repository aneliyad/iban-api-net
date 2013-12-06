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
    /// This class defines an iban specific exception class. It provides some special information about an exception.
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
    public class IbanException : Exception
    {
        /// <summary>
        /// ExceptionType
        /// </summary>
        public IbanExceptionType ExceptionType { get; set; }

        private string _exceptionMessage;

        /// <summary>
        /// ExceptionMessage
        /// </summary>
        public string ExceptionMessage { get { return _exceptionMessage; } }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="type">The type of the exception.</param>
        public IbanException(IbanExceptionType type) : base()
        {
            this.ExceptionType = type;

            this._exceptionMessage = this.ExceptionTypeToMessage(type);
        }

        /// <summary>
        /// Construcor.
        /// </summary>
        /// <param name="ex">The inner exception.</param>
        /// <param name="type">The type of the exception.</param>
        public IbanException(Exception ex, IbanExceptionType type) : base(ex.Message, ex)
        {
            this.ExceptionType = type;

            this._exceptionMessage = this.ExceptionTypeToMessage(type);
        }

        /// <summary>
        /// Gets a specific error message denpending to a type.
        /// </summary>
        /// <param name="type">The geiven type.</param>
        /// <returns>The error message.</returns>
        private string ExceptionTypeToMessage(IbanExceptionType type)
        {
            switch (type)
            {
                case IbanExceptionType.DataLoadFailure: return "Data could not be loaded";
                case IbanExceptionType.NoCalculation: return "No Iban Calculation for this accountnumber";
                case IbanExceptionType.BBANError: return "BBAN could not be generated.";
                case IbanExceptionType.IbanGeneratingError: return "IBAN could not be generated.";
                case IbanExceptionType.IbanGeneratingNotAllParameters: return "IBAN could not be generated because not all parameters were set.";
                case IbanExceptionType.IbanGeneratingFormatting: return "The generated IBAN does not match the required formatting of the country.";
                case IbanExceptionType.IbanGeneratingCheckDigit: return "IBAN could not be generated because check digit could not be calculated.";
                case IbanExceptionType.IbanValidatingFormatting: return "IBAN is not well formatted.";
                case IbanExceptionType.BankIdentNotValid: return "The bank ident does not fit the formatting rule.";
                case IbanExceptionType.IbanValidationIbanNeeded: return "For validating an iban please submit the iban code.";
                case IbanExceptionType.GetBicNoAllowedForCountry: return "Getting a bic is not supported for this country yet";
                default: return "An error occured!";
            }
        }
    }
}
