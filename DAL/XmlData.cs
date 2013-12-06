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
using System.Xml;
using System.Xml.Linq;
using System.Reflection;
using System.IO;
using AS.IBAN.Helper;
using System.Xml.Schema;
using AS.IBAN.Model;

namespace AS.IBAN.DAL
{
    /// <summary>
    /// An abstract class that defines methods for all data service classes.
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
    public abstract class XmlData : IDataServiceDefault
    {
        private string _fileNameIbanFormat = string.Empty;

        /// <summary>
        /// XmlNamespace
        /// </summary>
        public XNamespace XmlNamespace { get { return _xmlNamespace; } }
        private XNamespace _xmlNamespace = string.Empty;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="fileNameIbanFormat">Filename of xml file containing iban formats of each country.</param>
        public XmlData(string fileNameIbanFormat)
        {
            _fileNameIbanFormat = fileNameIbanFormat;
        }

        /// <summary>
        /// Gets XElements out of an specific xml file depending on attributes.
        /// </summary>
        /// <param name="filename">The filename of the xml file.</param>
        /// <param name="rootElementName">Name of the root element from which the data should be loaded.</param>
        /// <param name="keyValue">A KeyValuePair which contains the element key (for example "id") and the 
        /// value for that key (for example "1") to specify which element should be loaded.</param>
        /// <returns>A List of XElement objects that we're found in the file.</returns>
        /// <exception cref="IbanException">
        /// <list type="table">
        ///     <item>
        ///         <term><see cref="RuleType"/></term>
        ///         <description>DataLoadFailure</description>
        ///         <description>When data could not be loaded.</description>
        ///     </item>
        /// </list>
        /// </exception>
        public IEnumerable<XElement> LoadData(string filename, string rootElementName, KeyValuePair<string, string>? keyValue)
        {
            try
            {
                string xml = string.Empty;
                string xsd = string.Empty;

                //  Load XML-Data from Assembly
                var resource = (from ress in Assembly.GetExecutingAssembly().GetManifestResourceNames()
                                where ress.EndsWith(filename)
                                select ress);

                if (resource.Count() == 1)
                {
                    //  File was found in assembly
                    //  get the RessourceStream
                    Stream xmlStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource.First());
                    byte[] buffer = new byte[xmlStream.Length];
                    xmlStream.Read(buffer, 0, buffer.Length);

                    //  save content as string
                    xml = Encoding.UTF8.GetString(buffer);
                }

                if (string.IsNullOrEmpty(xml))
                {
                    throw new IbanException(IbanExceptionType.DataLoadFailure);
                }

                //  replace unecessarry chars
                xml = xml.Replace(System.Environment.NewLine, "");
                xml = xml.Replace("\t", "");
                xml = xml.Substring(xml.IndexOf(">") + 1);

                //  Create an XDoc from the xml-string
                XDocument xDoc = XDocument.Parse(xml);

                //  Get the namespace of the root element
                _xmlNamespace = xDoc.Root.Name.Namespace;

                //  validate xml
                this.ValidateXml(xDoc, filename);

                //  check if value is set, otherwise get all elements with keyword
                IEnumerable<XElement> xmlElements;
                if (!keyValue.HasValue)
                {
                    xmlElements = (from el in xDoc.Descendants(_xmlNamespace + rootElementName)
                                   select el);
                }
                else
                {
                    xmlElements = (from el in xDoc.Descendants(_xmlNamespace + rootElementName)
                                   where el.Attribute(keyValue.Value.Key).Value.Equals(keyValue.Value.Value)
                                   select el);
                }

                return xmlElements;
            }
            catch (Exception ex)
            {
                throw new IbanException(ex, IbanExceptionType.DataLoadFailure);
            }
        }

        /// <summary>
        /// Checks if a xml file is valid to it's defined schema (xsd file).
        /// </summary>
        /// <param name="xDoc">The given XDocument (contains the xml elements).</param>
        /// <param name="filenameXml">The filename of the xml file.</param>
        /// <exception cref="IbanException">
        /// <list type="table">
        ///     <item>
        ///         <term><see cref="RuleType"/></term>
        ///         <description>DataLoadFailure</description>
        ///         <description>When iban calculation is not allowed.</description>
        ///     </item>
        /// </list>
        /// </exception>
        private void ValidateXml(XDocument xDoc, string filenameXml)
        {
            string xsd = string.Empty;

            string filenameXsd = filenameXml.Replace(".xml", ".xsd");

            //  Load XSD
            var resource = (from ress in Assembly.GetExecutingAssembly().GetManifestResourceNames()
                            where ress.EndsWith(filenameXsd)
                        select ress);

            if (resource.Count() == 1)
            {
                //  File was found in assembly
                //  get the RessourceStream
                Stream xsdStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource.First());
                byte[] buffer = new byte[xsdStream.Length];
                xsdStream.Read(buffer, 0, buffer.Length);

                //  save content as string
                xsd = Encoding.UTF8.GetString(buffer);
            }

            if (string.IsNullOrEmpty(xsd))
            {
                throw new IbanException(IbanExceptionType.DataLoadFailure);
            }

            //  Create SchemaSet of xsd-string
            XmlSchemaSet schema = new XmlSchemaSet();
            schema.Add(xDoc.Root.Name.Namespace.NamespaceName, XmlReader.Create(new StringReader(xsd)));

            //  Check if xml is valid
            bool validationError = false;
            xDoc.Validate(schema, (o, e) =>
            {
                validationError = true;
            });

            //  throw Exception when an error occurs
            if (validationError)
                throw new IbanException(IbanExceptionType.DataLoadFailure);
        }

        #region Country
        /// <summary>
        /// Loads a Country to a given country code.
        /// </summary>
        /// <param name="country_code">The given country code.</param>
        /// <returns>The loaded country.</returns>
        public Country LoadCountry(ECountry country_code)
        {
            //  Load searched format from file
            IEnumerable<XElement> format = LoadData(_fileNameIbanFormat, "country", new KeyValuePair<string, string>("countryCode", country_code.ToString()));

            //  convert data in class object
            var result = (from ibanFormat in format
                          select new Country
                          {
                              AccountNumberLength = ibanFormat.Element(XmlNamespace + "ktoIdentLength").Value,
                              BankIdentLength = ibanFormat.Element(XmlNamespace + "bankIdentLength").Value,
                              CountryCode = country_code.ToString(),
                              Name = ibanFormat.Element(XmlNamespace + "name").Value,
                              RegExp = ibanFormat.Element(XmlNamespace + "regexp").Value,
                              CountryType = country_code
                          });

            return result.First();
        }
        #endregion
    }
}
