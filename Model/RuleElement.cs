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
    /// Class reprasenting a RuleElemt.
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
    public class RuleElement : IDisposable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public RuleElement()
        {
            Attributes = new List<KeyValuePair<string, string>>();
        }

        /// <summary>
        /// RuleType
        /// </summary>
        public RuleType RuleType { get; set; }

        /// <summary>
        /// Attributes
        /// </summary>
        public List<KeyValuePair<string, string>> Attributes { get; set; }

        /// <summary>
        /// Data
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Childs
        /// </summary>
        public IEnumerable<RuleElement> Childs { get; set; }

        /// <summary>
        /// Implements <see cref="System.IDisposable"/>
        /// </summary>
        public void Dispose()
        {
            Attributes.Clear();
            Attributes = null;
            Data = null;

            foreach (RuleElement element in Childs)
                element.Dispose();

            Childs = null;
        }
    }
}
