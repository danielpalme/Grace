﻿using System;
using System.Collections.Generic;
using Grace.DependencyInjection.Lifestyle;

namespace Grace.DependencyInjection.Impl
{
	/// <summary>
	/// Export Strategy for creating List(T) objects
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ListExportStrategy<T> : IExportStrategy
	{
		/// <summary>
		/// Activate the export
		/// </summary>
		/// <param name="exportInjectionScope"></param>
		/// <param name="context"></param>
		/// <param name="consider"></param>
		/// <returns></returns>
		public object Activate(IInjectionScope exportInjectionScope, IInjectionContext context, ExportStrategyFilter consider)
		{
			return exportInjectionScope.LocateAll<T>(context, consider);
		}

		/// <summary>
		/// Dispose the Export strategy
		/// </summary>
		public void Dispose()
		{
		}

		/// <summary>
		/// Initialize the export, caled by the container
		/// </summary>
		public void Initialize()
		{
		}

		/// <summary>
		/// This is type that will be activated, can be used for filtering
		/// </summary>
		public Type ActivationType
		{
			get { return typeof(List<T>); }
		}

		/// <summary>
		/// Usually the type.FullName, used for blacklisting purposes
		/// </summary>
		public string ActivationName
		{
			get { return typeof(List<T>).FullName; }
		}

		/// <summary>
		/// When considering an export should it be filtered out.
		/// True by default, usually it's only false for special export types like Array ad List
		/// </summary>
		public bool AllowingFiltering
		{
			get { return false; }
		}

		/// <summary>
		/// Attributes associated with the export strategy. 
		/// Note: do not return null. Return an empty enumerable if there are none
		/// </summary>
		public IEnumerable<Attribute> Attributes
		{
			get { return new Attribute[0]; }
		}

		/// <summary>
		/// The scope that owns this export
		/// </summary>
		public IInjectionScope OwningScope { get; set; }

		/// <summary>
		/// Export Key
		/// </summary>
		public object Key
		{
			get { return null; }
		}

		/// <summary>
		/// Names this strategy should be known as.
		/// </summary>
		public IEnumerable<string> ExportNames
		{
			get { return new string[0]; }
		}

		/// <summary>
		/// Types this strategy should be known as
		/// </summary>
		public IEnumerable<Type> ExportTypes
		{
			get
			{
				yield return typeof(IEnumerable<T>);
				yield return typeof(ICollection<T>);
				yield return typeof(IList<T>);
				yield return typeof(List<T>);
			}
		}

		/// <summary>
		/// What environement is this strategy being exported under.
		/// </summary>
		public ExportEnvironment Environment
		{
			get { return ExportEnvironment.Any; }
		}

		/// <summary>
		/// What export priority is this being exported as
		/// </summary>
		public int Priority
		{
			get { return -1; }
		}

		/// <summary>
		/// ILifestyle associated with export
		/// </summary>
		public ILifestyle Lifestyle
		{
			get { return null; }
		}

		/// <summary>
		/// Does this export have any conditions, this is important when setting up the strategy
		/// </summary>
		public bool HasConditions
		{
			get { return false; }
		}

		/// <summary>
		/// Are the object produced by this export externally owned
		/// </summary>
		public bool ExternallyOwned
		{
			get { return false; }
		}

		/// <summary>
		/// Does this export meet the conditions to be used
		/// </summary>
		/// <param name="injectionContext"></param>
		/// <returns></returns>
		public bool MeetsCondition(IInjectionContext injectionContext)
		{
			return true;
		}

		/// <summary>
		/// No secondary strategies
		/// </summary>
		/// <returns></returns>
		public IEnumerable<IExportStrategy> SecondaryStrategies()
		{
			yield break;
		}

		/// <summary>
		/// No enrichment
		/// </summary>
		/// <param name="enrichWithDelegate"></param>
		public void EnrichWithDelegate(EnrichWithDelegate enrichWithDelegate)
		{
		}

		/// <summary>
		/// no dependencies
		/// </summary>
		public IEnumerable<ExportStrategyDependency> DependsOn
		{
			get { yield break; }
		}

		/// <summary>
		/// Metadata associated with this strategy
		/// </summary>
		public IExportMetadata Metadata
		{
			get { return new ExportMetadata(Key, new Dictionary<string, object>()); }
		}

		public override bool Equals(object obj)
		{
			ListExportStrategy<T> strategy = obj as ListExportStrategy<T>;

			if (strategy != null && strategy.OwningScope == OwningScope)
			{
				return true;
			}

			return false;
		}

		public override int GetHashCode()
		{
			return ActivationName.GetHashCode();
		}
	}
}