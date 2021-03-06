﻿using System;
using System.Collections.Generic;
using System.Reflection;

namespace Grace.DependencyInjection.Impl.DelegateFactory
{
	/// <summary>
	/// Export Strategy for any delegate that can match Func&lt;Targ1,TArg2,TReturn&gt;
	/// </summary>
	/// <typeparam name="TDelegate">delegate type to construct</typeparam>
	/// <typeparam name="TReturn">return type of delegate</typeparam>
	/// <typeparam name="TArg1">arguement one type</typeparam>
	/// <typeparam name="TArg2">arguement two type</typeparam>
	/// <typeparam name="TArg3">arguement three type</typeparam>
	/// <typeparam name="TArg4">arguement four type</typeparam>
	/// <typeparam name="TArg5">arguement five type</typeparam>
	public class GenericDelegateExportStrategy<TDelegate, TReturn, TArg1, TArg2, TArg3, TArg4, TArg5> :
		BaseGenericDelegateExportStrategy
	{
		private string[] argNames;

		/// <summary>
		/// Initialize strategy
		/// </summary>
		public override void Initialize()
		{
			List<string> argNamesList = new List<string>();
			MethodInfo invoke = typeof(TDelegate).GetTypeInfo().GetDeclaredMethod("Invoke");

			foreach (ParameterInfo parameterInfo in invoke.GetParameters())
			{
				argNamesList.Add(InjectionKernel.ImportTypeByName(parameterInfo.ParameterType) ? parameterInfo.Name : null);
			}

			argNames = argNamesList.ToArray();
		}

		/// <summary>
		/// Activate the strategy
		/// </summary>
		/// <param name="exportInjectionScope"></param>
		/// <param name="context"></param>
		/// <param name="consider"></param>
		/// <returns></returns>
		public override object Activate(IInjectionScope exportInjectionScope,
			IInjectionContext context,
			ExportStrategyFilter consider)
		{
			Helper newHelper = new Helper(context, consider, argNames[0], argNames[1], argNames[2], argNames[3], argNames[4]);

			return activateMethodInfo.CreateDelegate(typeof(TDelegate), newHelper);
		}

		/// <summary>
		/// Activation type
		/// </summary>
		public override Type ActivationType
		{
			get { return typeof(TDelegate); }
		}

		/// <summary>
		/// Activation Name
		/// </summary>
		public override string ActivationName
		{
			get { return typeof(TDelegate).FullName; }
		}

		/// <summary>
		/// Exported types
		/// </summary>
		public override IEnumerable<Type> ExportTypes
		{
			get { yield return typeof(TDelegate); }
		}

		#region helper

		private static readonly MethodInfo activateMethodInfo;

		static GenericDelegateExportStrategy()
		{
			activateMethodInfo = typeof(Helper).GetRuntimeMethod("Activate",
				new[] { typeof(TArg1), typeof(TArg2), typeof(TArg3), typeof(TArg4), typeof(TArg5) });
		}

		/// <summary>
		/// Helper class for binding the delegate to
		/// </summary>
		public class Helper
		{
			private readonly IInjectionContext injectionContext;
			private readonly IInjectionScope scope;
			private readonly IDisposalScope disposalScope;
			private readonly IInjectionTargetInfo targetInfo;
			private readonly string argName1;
			private readonly string argName2;
			private readonly string argName3;
			private readonly string argName4;
			private readonly string argName5;
			private readonly ExportStrategyFilter consider;

			/// <summary>
			/// Default Constructor
			/// </summary>
			/// <param name="injectionContext"></param>
			/// <param name="consider"></param>
			/// <param name="argName1"></param>
			/// <param name="argName2"></param>
			/// <param name="argName3"></param>
			/// <param name="argName4"></param>
			/// <param name="argName5"></param>
			public Helper(IInjectionContext injectionContext,
				ExportStrategyFilter consider,
				string argName1,
				string argName2,
				string argName3,
				string argName4,
				string argName5)
			{
				this.injectionContext = injectionContext;
				scope = injectionContext.RequestingScope;
				disposalScope = injectionContext.DisposalScope;
				targetInfo = injectionContext.TargetInfo;

				this.argName1 = argName1;
				this.argName2 = argName2;
				this.argName3 = argName3;
				this.argName4 = argName4;
				this.argName5 = argName5;
				this.consider = consider;
			}

			/// <summary>
			/// Activate method
			/// </summary>
			/// <param name="arg1"></param>
			/// <param name="arg2"></param>
			/// <param name="arg3"></param>
			/// <param name="arg4"></param>
			/// <param name="arg5"></param>
			/// <returns></returns>
			public TReturn Activate(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, TArg5 arg5)
			{
				IInjectionContext newInjectionContext = injectionContext.Clone();

				newInjectionContext.RequestingScope = scope;
				newInjectionContext.DisposalScope = disposalScope;
				newInjectionContext.TargetInfo = targetInfo;

				if (argName1 != null)
				{
					newInjectionContext.Export(argName1, (s, c) => arg1);
				}
				else
				{
					newInjectionContext.Export((s, c) => arg1);
				}

				if (argName2 != null)
				{
					newInjectionContext.Export(argName2, (s, c) => arg2);
				}
				else
				{
					newInjectionContext.Export((s, c) => arg2);
				}

				if (argName3 != null)
				{
					newInjectionContext.Export(argName3, (s, c) => arg3);
				}
				else
				{
					newInjectionContext.Export((s, c) => arg3);
				}

				if (argName4 != null)
				{
					newInjectionContext.Export(argName4, (s, c) => arg4);
				}
				else
				{
					newInjectionContext.Export((s, c) => arg4);
				}

				if (argName5 != null)
				{
					newInjectionContext.Export(argName5, (s, c) => arg5);
				}
				else
				{
					newInjectionContext.Export((s, c) => arg5);
				}

				return newInjectionContext.RequestingScope.Locate<TReturn>(newInjectionContext, consider);
			}
		}

		#endregion
	}
}