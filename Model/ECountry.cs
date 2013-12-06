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
    /// Enumeration of all possible countries (country code).
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
    public enum ECountry
    {
        /// <summary>
        /// Austria
        /// </summary>
        AT = 1,

        /// <summary>
        /// Belgium
        /// </summary>
        BE = 2,

        /// <summary>
        /// Bulgaria
        /// </summary>
        BG = 3,

        /// <summary>
        /// Croatia
        /// </summary>
        HR = 4,

        /// <summary>
        /// Cyprus
        /// </summary>
        CY = 5,

        /// <summary>
        /// Czech Republic
        /// </summary>
        CZ = 6,

        /// <summary>
        /// Denmark
        /// </summary>
        DK = 7,

        /// <summary>
        /// Estonia
        /// </summary>
        EE = 8,

        /// <summary>
        /// Finland
        /// </summary>
        FI = 9,

        /// <summary>
        /// France
        /// </summary>
        FR = 10,

        /// <summary>
        /// Germany
        /// </summary>
        DE = 11,

        /// <summary>
        /// Greece
        /// </summary>
        GR = 12,

        /// <summary>
        /// Hungary
        /// </summary>
        HU = 13,

        /// <summary>
        /// Iceland
        /// </summary>
        IS = 14,

        /// <summary>
        /// Ireland
        /// </summary>
        IE = 15,

        /// <summary>
        /// Italia
        /// </summary>
        IT = 16,

        /// <summary>
        /// Latvia
        /// </summary>
        LV = 17,

        /// <summary>
        /// Lichtenstein
        /// </summary>
        LI = 18,

        /// <summary>
        /// Lithuania
        /// </summary>
        LT = 19,

        /// <summary>
        /// Luxembourg
        /// </summary>
        LU = 20,

        /// <summary>
        /// Malta
        /// </summary>
        MT = 21,

        /// <summary>
        /// Monaco
        /// </summary>
        MC = 22,

        /// <summary>
        /// Netherlands
        /// </summary>
        NL = 23,

        /// <summary>
        /// Norway
        /// </summary>
        NO = 24,

        /// <summary>
        /// Poland
        /// </summary>
        PL = 25,

        /// <summary>
        /// Portugal
        /// </summary>
        PT = 26,

        /// <summary>
        /// Romania
        /// </summary>
        RO = 27,

        /// <summary>
        /// Slovakia
        /// </summary>
        SK = 28,

        /// <summary>
        /// Slovenia
        /// </summary>
        SI = 29,

        /// <summary>
        /// Spain
        /// </summary>
        ES = 30,

        /// <summary>
        /// Sweden
        /// </summary>
        SE = 31,

        /// <summary>
        /// Switzerland
        /// </summary>
        CH = 32,

        /// <summary>
        /// United Kingdom
        /// </summary>
        GB = 33
    }
}
