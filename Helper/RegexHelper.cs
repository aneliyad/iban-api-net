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
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace AS.IBAN.Helper
{
    /// <summary>
    /// A helper class for regular expressions.
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
    public class RegexHelper
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public RegexHelper() { }

        /// <summary>
        /// Checks if a string matches a regular expression and saves a possible replacement in out parameter.
        /// </summary>
        /// <param name="regexp">The given regular expression.</param>
        /// <param name="value">Value that could match the expression.</param>
        /// <param name="result">out-parameter. If value matches expression a possible replacement is saved in this string.</param>
        /// <returns>'True' if value matches expression, otherwise 'false'.</returns>
        public bool RegexpMatch(string regexp, string value, out string result)
        {
            result = string.Empty;

            string temp_regexp = string.Empty; ;
            string temp_replacement = string.Empty;

            //  Test if there should  be an replacement
            if (regexp.Contains(";"))
            {
                temp_regexp = regexp.Split(new char[] { ';' })[0];
                temp_replacement = regexp.Split(new char[] { ';' })[1];

                //  replace $x durch ${x}, because $100 is treated as group number 100 and not as group1 + 00
                temp_replacement = Regex.Replace(temp_replacement, @"\$(\d)", m => "${" + m.Groups[1] + "}");
            }
            else
                temp_regexp = regexp;

            Regex regExp = new Regex(temp_regexp);

            if (regExp.IsMatch(value))
            {
                if (!string.IsNullOrWhiteSpace(temp_replacement))
                    result = regExp.Replace(value, temp_replacement);
            }
            else
                return false;

            return true;
        }

        /// <summary>
        /// Checks if a string matches a regular expression
        /// </summary>
        /// <param name="regexp">The given regular expression.</param>
        /// <param name="value">Value that could match the expression.</param>
        /// <returns>'True' if value matches expression, otherwise 'false'.</returns>
        public bool RegexpMatch(string regexp, string value)
        {
            string temp_regexp = string.Empty; ;
            string temp_replacement = string.Empty;

            //  Test if there should  be an replacement
            if (regexp.Contains(";"))
            {
                temp_regexp = regexp.Split(new char[] { ';' })[0];
                temp_replacement = regexp.Split(new char[] { ';' })[1];

                //  replace $x durch ${x}, because $100 is treated as group number 100 and not as group1 + 00
                temp_replacement = Regex.Replace(temp_replacement, @"\$(\d)", m => "${" + m.Groups[1] + "}");
            }
            else
                temp_regexp = regexp;

            //  Check if regexp starts with \b
            if (!temp_regexp.StartsWith(@"\b"))
                temp_regexp = @"\b" + temp_regexp;

            //  Check if regexp ends with \Z
            if (!temp_regexp.EndsWith(@"\Z"))
                temp_regexp = temp_regexp + @"\Z";

            Regex regExp = new Regex(temp_regexp);

            if (regExp.IsMatch(value))
            {
                return true;
            }
            else
                return false;
        }
    }
}
