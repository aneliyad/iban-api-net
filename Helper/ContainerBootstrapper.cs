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

using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AS.IBAN.Manager;
using AS.IBAN.DAL;
using AS.IBAN.Model;

namespace AS.IBAN.Helper
{
    /// <summary>
    /// This class wrapps the unity container.
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
    public class ContainerBootstrapper
    {
        private static bool _initalized = false;
        private static IUnityContainer _container;

        /// <summary>
        /// Container
        /// </summary>
        public static IUnityContainer Container
        {
            get
            {
                return _container;
            }

            private set
            {
                _container = value;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        static ContainerBootstrapper()
        {
            Initialize();
        }

        /// <summary>
        /// This Method initalizes the unity container.
        /// </summary>
        private static void Initialize()
        {
            //  If container already was initialized, don't do it again
            if (_initalized)
                return;

            var container = new UnityContainer();

            //  Default
            container.RegisterType<IDataService, DAL.Default.XmlDataServiceDefault>(new InjectionConstructor("iban_format.xml"));
            container.RegisterType<IIbanManager, Manager.Default.ManagerDefault>(new InjectionConstructor(container.Resolve<IDataService>()));

            container.RegisterType<IDataService, DAL.DE.XmlDataServiceDE>(ECountry.DE.ToString(), new InjectionConstructor("banks_german.xml", "iban_format.xml", "iban_rules_german.xml"));
            container.RegisterType<IIbanManager, Manager.DE.ManagerDE>(ECountry.DE.ToString(), new InjectionConstructor(container.Resolve<IDataService>(ECountry.DE.ToString())));

            Container = container;

            _initalized = true;
        }

        /// <summary>
        /// Resolves the type parameter T to an instance of the appropriate type.
        /// </summary>
        /// <typeparam name="T">Type of object to return</typeparam>
        /// <returns>The resolved Type.</returns>
        public static T Resolve<T>()
        {
            T ret = default(T);

            if (Container.IsRegistered(typeof(T)))
            {
                ret = Container.Resolve<T>();
            }

            return ret;
        }

        /// <summary>
        /// Resolves the type parameter T to an instance of the appropriate type depending on a special name.
        /// </summary>
        /// <typeparam name="T">Type of object to return</typeparam>
        /// <param name="name">The special name.</param>
        /// <returns>The resolved Type.</returns>
        public static T Resolve<T>(string name)
        {
            T ret = default(T);

            if (Container.IsRegistered(typeof(T),name))
            {
                ret = Container.Resolve<T>(name);
            }
            else
            {
                if (Container.IsRegistered(typeof(T)))
                {
                    ret = Container.Resolve<T>();
                }
            }

            return ret;
        }
    }
}
