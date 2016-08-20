using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;
using OpenQA.Selenium;

namespace Ravitej.Automation.Common.Dependency
{
    /// <summary>
    /// Sealed class for everything that needs to be accessible across the entire application scope, both in test framework and page objects
    /// </summary>
    public sealed class AppWide
    {
        private static readonly UnityContainer UnityContainer;

        static AppWide()
        {
            // Create Unity Container
            UnityContainer = new UnityContainer();
        }

        private AppWide()
        {
        }

        /// <summary>
        /// Get an singleton instance of the AppWide object
        /// </summary>
        public static AppWide Instance { get; } = new AppWide();

        /// <summary>
        /// Get an instance of the Unity Container
        /// </summary>
        public UnityContainer Container => UnityContainer;

        /// <summary>
        /// Resolve an instance of a Type
        /// </summary>
        /// <typeparam name="TTypeToResolve"></typeparam>
        /// <returns></returns>
        public TTypeToResolve Resolve<TTypeToResolve>()
        {
            try
            {
                return UnityContainer.Resolve<TTypeToResolve>();
            }
            catch (Exception e)
            {
                if (e.InnerException is UnhandledAlertException)
                {
                    throw e.InnerException;
                }

                throw;
            }
        }

        /// <summary>
        /// Convert dictionary of parameters to Unity ParameterOverrides
        /// </summary>
        /// <param name="objectParams"></param>
        /// <returns></returns>
        private ParameterOverrides GetParameterOverrides(IEnumerable<KeyValuePair<string, object>> objectParams)
        {
            var parameterOverrides = new ParameterOverrides();
            foreach (KeyValuePair<string, object> item in objectParams)
            {
                parameterOverrides.Add(item.Key, item.Value);
            }

            return parameterOverrides;
        }

        /// <summary>
        /// Resolve an instance of a type using the passed in constructor arguments
        /// </summary>
        /// <typeparam name="TTypeToResolve"></typeparam>
        /// <param name="objectParams"></param>
        /// <returns></returns>
        public TTypeToResolve Resolve<TTypeToResolve>(IDictionary<string, object> objectParams)
        {
            try
            {
                // Get parameter overrides for Unity
                var parameterOverrides = GetParameterOverrides(objectParams);
                var objectInstance = UnityContainer.Resolve<TTypeToResolve>(parameterOverrides);
                return objectInstance;
            }
            catch (Exception e)
            {
                if (e.InnerException is UnhandledAlertException)
                {
                    throw e.InnerException;
                }

                throw;
            }
        }

        /// <summary>
        /// Register a type into the unity container
        /// </summary>
        /// <typeparam name="TTypeToRegister"></typeparam>
        /// <param name="typeObject"></param>
        public void RegisterType<TTypeToRegister>(Type typeObject)
        {
            UnityContainer.RegisterType(typeof(TTypeToRegister), typeObject);
        }

        /// <summary>
        /// Register a type into the unity container
        /// </summary>
        /// <typeparam name="TFrom"></typeparam>
        /// <typeparam name="TTo"></typeparam>
        public void RegisterType<TFrom, TTo>() where TTo : TFrom
        {
            UnityContainer.RegisterType<TFrom, TTo>();
        }

        /// <summary>
        /// Register a type into the unity container
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <param name="newInstance"></param>
        public void SetInstance<TType>(TType newInstance)
        {
            UnityContainer.RegisterInstance(typeof(TType), newInstance, new ContainerControlledLifetimeManager());
        }
    }
}
