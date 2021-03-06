﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Grace.DependencyInjection.Conditions;
using Grace.DependencyInjection.Lifestyle;

namespace Grace.DependencyInjection
{
	/// <summary>
	/// This inteface allows you to configure an export strategy
	/// </summary>
	public interface IFluentExportStrategyConfiguration
	{
		/// <summary>
		/// Defines the priority to export at
		/// </summary>
		/// <param name="priority">priority for export</param>
		/// <returns>configuration object</returns>
		IFluentExportStrategyConfiguration WithPriority(int priority);

		/// <summary>
		/// Export under a particular key
		/// </summary>
		/// <param name="key">key to associate with export</param>
		/// <returns>configuration object</returns>
		IFluentExportStrategyConfiguration WithKey(object key);

		/// <summary>
		/// Export as a particular type
		/// </summary>
		/// <param name="exportType">type to export as</param>
		/// <returns>configuration object</returns>
		IFluentExportStrategyConfiguration As(Type exportType);

		/// <summary>
		/// Export as a particular type
		/// </summary>
		/// <returns>configuration object</returns>
		IFluentExportStrategyConfiguration As<T>();

		/// <summary>
		/// Export the type by it's interfaces
		/// </summary>
		/// <returns>configuration object</returns>
		IFluentExportStrategyConfiguration ByInterfaces();

		/// <summary>
		/// Defines which environment this export should be exported in
		/// </summary>
		/// <param name="environment"></param>
		/// <returns>configuration object</returns>
		IFluentExportStrategyConfiguration InEnvironment(ExportEnvironment environment);

		/// <summary>
		/// Export this type as a particular name
		/// </summary>
		/// <param name="name"></param>
		/// <returns>configuration object</returns>
		IFluentExportStrategyConfiguration AsName(string name);

		/// <summary>
		/// Export will be treated as a singleton for the lifetime of the container
		/// </summary>
		/// <returns>configuration object</returns>
		IFluentExportStrategyConfiguration AndSingleton();

		/// <summary>
		/// Export will be treated as a singleton for the lifetime of the scope
		/// </summary>
		/// <returns>configuration object</returns>
		IFluentExportStrategyConfiguration AndSingletonPerScope();

		/// <summary>
		/// Exports will be trated as a singleton using a weak reference
		/// </summary>
		/// <returns>configuration object</returns>
		IFluentExportStrategyConfiguration AndWeakSingleton();

		/// <summary>
		/// Mark the export as externally owned, doing so will absolve the container of having to call Dispose when done
		/// </summary>
		/// <returns>configuration object</returns>
		IFluentExportStrategyConfiguration ExternallyOwned();

		/// <summary>
		/// Allows you to specify an import constructor
		/// </summary>
		/// <param name="constructorInfo">ConstrcutorInfo object to use during construction</param>
		/// <returns>configuration object</returns>
		IFluentExportStrategyConfiguration ImportConstructor(ConstructorInfo constructorInfo);

		/// <summary>
		/// Mark a property for import and specify if its required
		/// </summary>
		IFluentImportPropertyConfiguration ImportProperty(string propertyName);

		/// <summary>
		/// Mark a property for import and specify if its required
		/// </summary>
		/// <param name="methodName">name of method to import</param>
		/// <returns>configuration object</returns>
		IFluentImportMethodConfiguration ImportMethod(string methodName);

		/// <summary>
		/// Export a specific property under a particular name
		/// </summary>
		/// <param name="propertyName">name of property to export</param>
		/// <returns>configuration object</returns>
		IFluentExportPropertyConfiguration ExportProperty(string propertyName);

		/// <summary>
		/// Method to call when activation is done
		/// </summary>
		/// <param name="activationMethod">name of method to activate</param>
		/// <returns></returns>
		IFluentExportStrategyConfiguration ActivationMethod(string activationMethod);

		/// <summary>
		/// Specify a custom Lifestyle container for export.
		/// </summary>
		/// <param name="lifestyle">Lifestyle container for the export</param>
		/// <returns>configuration object</returns>
		IFluentExportStrategyConfiguration UsingLifestyle(ILifestyle lifestyle);

		/// <summary>
		/// Adds a condition to the export
		/// </summary>
		/// <param name="conditionDelegate">export condition delegate</param>
		/// <returns>configuration object</returns>
		IFluentExportStrategyConfiguration When(ExportConditionDelegate conditionDelegate);

		/// <summary>
		/// Adds a condition to the export
		/// </summary>
		/// <param name="conditionDelegate">export condition delegate</param>
		/// <returns>configuration object</returns>
		IFluentExportStrategyConfiguration Unless(ExportConditionDelegate conditionDelegate);

		/// <summary>
		/// Adds a condition to the export
		/// </summary>
		/// <param name="condition">condition for export</param>
		/// <returns>configuration object</returns>
		IFluentExportStrategyConfiguration AndCondition(IExportCondition condition);

		/// <summary>
		/// Marks all properties on the object for injection
		/// Note: Only public writeable properties will be imported
		/// </summary>
		/// <param name="required">are all the properties required</param>
		/// <returns>configuration object</returns>
		IFluentExportStrategyConfiguration AutoWireProperties(bool required = false);

		/// <summary>
		/// Adds metadata to an export
		/// </summary>
		/// <param name="metadataName">metadata name</param>
		/// <param name="metadataValue">metadata value</param>
		/// <returns>configuration object</returns>
		IFluentExportStrategyConfiguration WithMetadata(string metadataName, object metadataValue);

		/// <summary>
		/// You can provide a cleanup method to be called 
		/// </summary>
		/// <param name="disposalCleanupDelegate"></param>
		/// <returns>configuration object</returns>
		IFluentExportStrategyConfiguration DisposalCleanupDelegate(BeforeDisposalCleanupDelegate disposalCleanupDelegate);

		/// <summary>
		/// Allows you to add custom activation logic to process before the object is returned.
		/// </summary>
		/// <param name="enrichWithDelegate"></param>
		/// <returns>configuration object</returns>
		IFluentExportStrategyConfiguration EnrichWith(EnrichWithDelegate enrichWithDelegate);

		/// <summary>
		/// Add a specific value for a particuar parameter in the constructor
		/// </summary>
		/// <typeparam name="TParam">type of parameter</typeparam>
		/// <param name="paramValue">Func(T) value for the parameter</param>
		/// <returns>configuration object</returns>
		IFluentWithCtorConfiguration<TParam> WithCtorParam<TParam>(Func<TParam> paramValue = null);

		/// <summary>
		/// Add a specific value for a particuar parameter in the constructor
		/// </summary>
		/// <typeparam name="TParam">type of parameter</typeparam>
		/// <param name="paramValue">Func(IInjectionScope, IInjectionContext, T) value for the parameter</param>
		/// <returns>configuration object</returns>
		IFluentWithCtorConfiguration<TParam> WithCtorParam<TParam>(Func<IInjectionScope, IInjectionContext, TParam> paramValue);

		/// <summary>
		/// Adds a constructor param of type TParam to the constructor
		/// </summary>
		/// <typeparam name="TParam"></typeparam>
		/// <typeparam name="TItem"></typeparam>
		/// <returns></returns>
		IFluentWithCtorCollectionConfiguration<TItem> WithCtorParamCollection<TParam, TItem>()
			where TParam : IEnumerable<TItem>;
	}

	/// <summary>
	/// This interface allows you to configure an export strategy for type T
	/// </summary>
	/// <typeparam name="T">type to export</typeparam>
	public interface IFluentExportStrategyConfiguration<T>
	{
		/// <summary>
		/// Export the type with the specified priority
		/// </summary>
		/// <param name="priority"></param>
		/// <returns></returns>
		IFluentExportStrategyConfiguration<T> WithPriority(int priority);

		/// <summary>
		/// Mark the export as externally owned, doing so will absolve the container of having to call Dispose when done
		/// </summary>
		/// <returns>configuration object</returns>
		IFluentExportStrategyConfiguration<T> ExternallyOwned();

		/// <summary>
		/// Export as a specific type (usually an interface)
		/// </summary>
		/// <typeparam name="TExportType"></typeparam>
		/// <returns></returns>
		IFluentExportStrategyConfiguration<T> As<TExportType>();

		/// <summary>
		/// Export as a specific type (usually an interface)
		/// </summary>
		/// <param name="exportType"></param>
		/// <returns></returns>
		IFluentExportStrategyConfiguration<T> As(Type exportType);

		/// <summary>
		/// Export the type by the interfaces it implements
		/// </summary>
		/// <returns></returns>
		IFluentExportStrategyConfiguration<T> ByInterfaces();

		/// <summary>
		/// Export type in this Environment (ExportEnvironement is a flag so it can be or'd)
		/// </summary>
		/// <param name="environment"></param>
		/// <returns></returns>
		IFluentExportStrategyConfiguration<T> InEnvironment(ExportEnvironment environment);

		/// <summary>
		/// Export the type under the specified name
		/// </summary>
		/// <param name="name">name to export under</param>
		/// <returns></returns>
		IFluentExportStrategyConfiguration<T> AsName(string name);

		/// <summary>
		/// Export will be treated as a singleton for the lifetime of the container
		/// </summary>
		/// <returns></returns>
		IFluentExportStrategyConfiguration<T> AndSingleton();

		/// <summary>
		/// Export will be treated as a singleton for the lifetime of the scope
		/// </summary>
		/// <returns></returns>
		IFluentExportStrategyConfiguration<T> AndSingletonPerScope();

		/// <summary>
		/// Exports will be trated as a singleton using a weak reference
		/// </summary>
		/// <returns></returns>
		IFluentExportStrategyConfiguration<T> AndWeakSingleton();

		/// <summary>
		/// Attach a key to the export
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		IFluentExportStrategyConfiguration<T> WithKey(object key);

		/// <summary>
		/// This method allows you to specify which constructor to use ( () => new MyTypeName("Specific", "Constructor") )
		/// </summary>
		/// <param name="constructorExpression">constructor expression ( () => new MyTypeName("Specific", "Constructor") )</param>
		/// <returns></returns>
		IFluentExportStrategyConfiguration<T> ImportConstructor(Expression<Action> constructorExpression);

		/// <summary>
		/// Mark a property for Import (using dependency injection container)
		/// </summary>
		/// <returns></returns>
		IFluentImportPropertyConfiguration<T, TProp> ImportProperty<TProp>(Expression<Func<T, TProp>> property);

		/// <summary>
		/// Import a property as a collection allowing for you to specify the sort order for the import
		/// </summary>
		/// <typeparam name="TItem"></typeparam>
		/// <param name="property"></param>
		/// <returns></returns>
		IFluentImportPropertyCollectionConfiguration<T, TItem> ImportCollectionProperty<TItem>(
			Expression<Func<T, IEnumerable<TItem>>> property);

		/// <summary>
		/// Mark a method to be called upon activation passing in an arguement that has be located using the IoC
		/// </summary>
		/// <param name="method">method to import</param>
		/// <returns></returns>
		IFluentImportMethodConfiguration<T> ImportMethod(
			Expression<Action<T>> method);

		/// <summary>
		/// Export a property to be imported by other exports
		/// </summary>
		/// <typeparam name="TProp"></typeparam>
		/// <param name="property"></param>
		/// <returns></returns>
		IFluentExportPropertyConfiguration<T, TProp> ExportProperty<TProp>(Expression<Func<T, TProp>> property);

		/// <summary>
		/// Mark a particular Action() as the activation action
		/// </summary>
		/// <param name="activationMethod"></param>
		/// <returns></returns>
		IFluentExportStrategyConfiguration<T> ActivationMethod(Expression<Action<T>> activationMethod);

		/// <summary>
		/// Specify a custom life cycle container for the export
		/// </summary>
		/// <param name="container"></param>
		/// <returns></returns>
		IFluentExportStrategyConfiguration<T> UsingLifestyleContainer(ILifestyle container);

		/// <summary>
		/// Adds a condition to the export
		/// </summary>
		/// <param name="conditionDelegate"></param>
		IFluentExportStrategyConfiguration<T> When(ExportConditionDelegate conditionDelegate);

		/// <summary>
		/// Adds a condition to the export
		/// </summary>
		/// <param name="conditionDelegate"></param>
		IFluentExportStrategyConfiguration<T> Unless(ExportConditionDelegate conditionDelegate);

		/// <summary>
		/// Adds a condition to the export
		/// </summary>
		/// <param name="condition"></param>
		IFluentExportStrategyConfiguration<T> AndCondition(IExportCondition condition);

		/// <summary>
		/// Applies a new WhenInjectedInto condition on the export, using the export only when injecting into the specified class
		/// </summary>
		/// <typeparam name="TInjected"></typeparam>
		/// <returns></returns>
		IFluentExportStrategyConfiguration<T> WhenInjectedInto<TInjected>();

		/// <summary>
		/// Applies a WhenClassHas condition, using the export only if injecting into a class that is attributed with TAttr
		/// </summary>
		/// <typeparam name="TAttr"></typeparam>
		/// <returns></returns>
		IFluentExportStrategyConfiguration<T> WhenClassHas<TAttr>();

		/// <summary>
		/// Applies a WhenMemberHas condition, using the export only if injecting into a class that is attributed with TAttr
		/// </summary>
		/// <typeparam name="TAttr"></typeparam>
		/// <returns></returns>
		IFluentExportStrategyConfiguration<T> WhenMemberHas<TAttr>();

		/// <summary>
		/// Applies a WhenTargetHas condition, using the export only if the Property or Parameter is attributed with TAttr
		/// </summary>
		/// <typeparam name="TAttr"></typeparam>
		/// <returns></returns>
		IFluentExportStrategyConfiguration<T> WhenTargetHas<TAttr>();

		/// <summary>
		/// Sets up all public writable properties on the type to be injected
		/// </summary>
		/// <param name="required">are the properties required</param>
		/// <returns></returns>
		IFluentExportStrategyConfiguration<T> AutoWireProperties(bool required = false);

		/// <summary>
		/// Adds metadata to an export
		/// </summary>
		/// <param name="metadataName"></param>
		/// <param name="metadataValue"></param>
		/// <returns></returns>
		IFluentExportStrategyConfiguration<T> WithMetadata(string metadataName, object metadataValue);

		/// <summary>
		/// Add a value to be used for constructor parameters
		/// </summary>
		/// <typeparam name="TParam">type of parameter</typeparam>
		/// <param name="paramValue">Func(TParam) for the parameter</param>
		/// <returns></returns>
		IFluentWithCtorConfiguration<T, TParam> WithCtorParam<TParam>(Func<TParam> paramValue = null);

		/// <summary>
		/// Add a specific value for a particuar parameter in the constructor
		/// </summary>
		/// <typeparam name="TParam">type of parameter</typeparam>
		/// <param name="paramValue">Func(IInjectionScope, IInjectionContext, T) value for the parameter</param>
		/// <returns>configuration object</returns>
		IFluentWithCtorConfiguration<T, TParam> WithCtorParam<TParam>(
			Func<IInjectionScope, IInjectionContext, TParam> paramValue);

		/// <summary>
		/// Import a collection allowing you to specify a filter and a sort order
		/// </summary>
		/// <typeparam name="TParam"></typeparam>
		/// <typeparam name="TItem"></typeparam>
		/// <returns></returns>
		IFluentWithCtorCollectionConfiguration<T, TItem> WithCtorCollectionParam<TParam, TItem>()
			where TParam : IEnumerable<TItem>;

		/// <summary>
		/// You can provide a cleanup method to be called 
		/// </summary>
		/// <param name="disposalCleanupDelegate"></param>
		/// <returns></returns>
		IFluentExportStrategyConfiguration<T> DisposalCleanupDelegate(BeforeDisposalCleanupDelegate disposalCleanupDelegate);

		/// <summary>
		/// Allows you to add custom activation logic to process before the object is returned.
		/// </summary>
		/// <param name="enrichWithDelegate"></param>
		/// <returns></returns>
		IFluentExportStrategyConfiguration<T> EnrichWith(EnrichWithDelegate enrichWithDelegate);
	}
}