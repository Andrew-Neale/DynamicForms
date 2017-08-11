using System;
using Microsoft.Practices.Unity;

namespace DynamicForms.Helpers
{
	public static class IoC
	{
		public static UnityContainer Container { get; private set; }

		static IoC()
		{
			Container = new UnityContainer();
		}

		/// <summary>
		/// Convenience method allowing registration of an interface/concrete pair that
		/// will have a singleton lifespan.
		/// </summary>
		/// <param name="container">The UnityContainer in which to register this type.</param>
		/// <typeparam name="TFrom">The interface.</typeparam>
		/// <typeparam name="TTo">The concrete type.</typeparam>
		public static void RegisterSingleton<TFrom, TTo>(this UnityContainer container) where TTo : TFrom
		{
			container.RegisterType<TFrom, TTo>(new ContainerControlledLifetimeManager());
		}
	}
}
