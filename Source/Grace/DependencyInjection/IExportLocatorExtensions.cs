﻿using System;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace Grace.DependencyInjection
{
	/// <summary>
	/// Extension methods for IExportLocator
	/// </summary>
	// ReSharper disable once InconsistentNaming
	public static class IExportLocatorExtensions
	{
		/// <summary>
		/// This method returns a summary of the exports contained in an ExportLocator
		/// </summary>
		/// <param name="locator">export locator to analyze</param>
		/// <param name="includeParent">include parent scope, false by default</param>
		/// <param name="consider">export filter to apply</param>
		/// <param name="injectionContext">injection context to use when filtering</param>
		/// <returns>diagnostic string</returns>
		public static string WhatDoIHave(this IExportLocator locator, bool includeParent = false, ExportStrategyFilter consider = null, IInjectionContext injectionContext = null)
		{
			StringBuilder builder = new StringBuilder();

			if (injectionContext == null)
			{
				injectionContext = locator.CreateContext();
			}

			builder.AppendLine(new string('-', 80));

			builder.AppendFormat("Exports for scope '{0}' with id {1}{2}",
										locator.ScopeName,
										locator.ScopeId,
										Environment.NewLine);

			foreach (IExportStrategy exportStrategy in locator.GetAllStrategies(consider))
			{
				builder.AppendLine(new string('-', 80));

				builder.AppendLine("Export Type: " + exportStrategy.ActivationType.FullName);

				foreach (string exportName in exportStrategy.ExportNames)
				{
					builder.AppendLine("As Name: " + exportName);
				}

				foreach (Type exportType in exportStrategy.ExportTypes)
				{
					builder.AppendLine("As Type: " + exportType);
				}

				if (exportStrategy.Key != null)
				{
					builder.AppendLine("Key: " + exportStrategy.Key);
				}
				else
				{
					builder.AppendLine("Key: null");
				}

				builder.AppendLine("Environement: " + exportStrategy.Environment);

				builder.AppendLine("Priority: " + exportStrategy.Priority);

				builder.AppendLine("Externally Owned: " + exportStrategy.ExternallyOwned);

				if (exportStrategy.Lifestyle != null)
				{
					builder.AppendLine("Lifestyle: " + exportStrategy.Lifestyle.GetType().Name);
				}
				else
				{
					builder.AppendLine("Lifestyle: Transient");
				}

				builder.AppendLine("MeetsCondition: " + exportStrategy.MeetsCondition(injectionContext));

				builder.AppendLine("Depends On");

				if (exportStrategy.DependsOn.Any())
				{
					foreach (ExportStrategyDependency exportStrategyDependency in exportStrategy.DependsOn)
					{
						builder.AppendLine();

						builder.AppendLine("\tDependency Type: " + exportStrategyDependency.DependencyType);

						builder.AppendLine("\tTarget Name: " + exportStrategyDependency.TargetName);

						if (exportStrategyDependency.ImportType != null)
						{
							builder.AppendLine("\tImport Type: " + exportStrategyDependency.ImportType.FullName);
						}
						else
						{
							builder.AppendLine("\tImport Type: null");
						}

						if (exportStrategyDependency.ImportName != null)
						{
							builder.AppendLine("\tImport Name: " + exportStrategyDependency.ImportName);
						}
						else
						{
							builder.AppendLine("\tImport Name: null");
						}

						builder.AppendLine("\tHas Filter: " + exportStrategyDependency.HasFilter);

						builder.AppendLine("\tHas Value Provider: " + exportStrategyDependency.HasValueProvider);
					}
				}
				else
				{
					builder.AppendLine("\tnone");
				}

				builder.AppendLine();
			}

			if (includeParent && locator is IInjectionScope)
			{
				IInjectionScope scope = locator as IInjectionScope;

				if (scope.ParentScope != null)
				{
					builder.Append(WhatDoIHave(scope.ParentScope, true, consider, injectionContext));
				}
			}

			return builder.ToString();
		}

		/// <summary>
		/// Locate an export by type with a key
		/// </summary>
		/// <typeparam name="T">type to locate</typeparam>
		/// <typeparam name="TKey">type of key</typeparam>
		/// <param name="locator">locator to use</param>
		/// <param name="key">key to use while locating</param>
		/// <param name="injectionContext">injection context to use</param>
		/// <param name="consider">filter method to use</param>
		/// <returns>export T</returns>
		public static T LocateByKey<T, TKey>(this IExportLocator locator,
			[NotNull] TKey key,
			IInjectionContext injectionContext = null,
			ExportStrategyFilter consider = null)
		{
			ExportStrategyFilter keyFilter = (context, strategy) => CompareKeyFunction(key, context, strategy);

			if (consider != null)
			{
				return locator.Locate<T>(injectionContext, keyFilter + consider);
			}

			return locator.Locate<T>(injectionContext, keyFilter);
		}

		/// <summary>
		/// Locate an export by name and key
		/// </summary>
		/// <typeparam name="TKey">key type</typeparam>
		/// <param name="locator">locator to use</param>
		/// <param name="exportName">name of export to locate</param>
		/// <param name="key">key to use during location</param>
		/// <param name="injectionContext">injection context</param>
		/// <param name="consider">filter method</param>
		/// <returns>export object, null if no object found</returns>
		public static object LocateByKey<TKey>(this IExportLocator locator,
			[NotNull] string exportName,
			[NotNull] TKey key,
			IInjectionContext injectionContext = null,
			ExportStrategyFilter consider = null)
		{
			ExportStrategyFilter keyFilter = (context, strategy) => CompareKeyFunction(key, context, strategy);

			if (consider != null)
			{
				return locator.Locate(exportName, injectionContext, new ExportStrategyFilterGroup(keyFilter, consider));
			}

			return locator.Locate(exportName, injectionContext, keyFilter);
		}

		/// <summary>
		/// Locate an export by type and key
		/// </summary>
		/// <typeparam name="TKey">key type to locate</typeparam>
		/// <param name="locator">locator to use</param>
		/// <param name="exportType">type to locate</param>
		/// <param name="key">key to use while locating</param>
		/// <param name="injectionContext">injection context</param>
		/// <param name="consider">filter to use while locating</param>
		/// <returns>export object, null if no export found</returns>
		public static object LocateByKey<TKey>(this IExportLocator locator,
			[NotNull] Type exportType,
			[NotNull] TKey key,
			IInjectionContext injectionContext = null,
			ExportStrategyFilter consider = null)
		{
			ExportStrategyFilter keyFilter = (context, strategy) => CompareKeyFunction(key, context, strategy);

			if (consider != null)
			{
				return locator.Locate(exportType, injectionContext, new ExportStrategyFilterGroup(keyFilter, consider));
			}

			return locator.Locate(exportType, injectionContext, keyFilter);
		}

		/// <summary>
		/// Function to compare a key to an export strategy
		/// </summary>
		/// <param name="key"></param>
		/// <param name="context"></param>
		/// <param name="strategy"></param>
		/// <returns></returns>
		public static bool CompareKeyFunction(object key, IInjectionContext context, IExportStrategy strategy)
		{
			if (key == null)
			{
				return strategy.Key == null;
			}

			return key.Equals(strategy.Key);
		}
	}
}