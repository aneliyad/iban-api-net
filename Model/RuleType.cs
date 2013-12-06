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
    /// Enumeration of all possible rule types.
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
    public enum RuleType
    {
        /// <summary>
        /// No_Calculation Type
        /// </summary>
        No_Calculation = 0,

        /// <summary>
        /// Mappings_Kto Type
        /// </summary>
        Mappings_Kto = 1,

        /// <summary>
        /// Mappings_KtoKr Type
        /// </summary>
        Mappings_KtoKr = 2,

        /// <summary>
        /// Mappings_Blz Type
        /// </summary>
        Mappings_Blz = 3,

        /// <summary>
        /// Modification_Kto Type
        /// </summary>
        Modification_Kto = 4,

        /// <summary>
        /// Mapping Type
        /// </summary>
        Mapping = 5,

        /// <summary>
        /// Kto_Number_Range Type
        /// </summary>
        Kto_Number_Range = 6,

        /// <summary>
        /// Modification Type
        /// </summary>
        Modification = 7,

        /// <summary>
        /// Mappings_Bic Type
        /// </summary>
        Mappings_Bic = 8
    }
}
