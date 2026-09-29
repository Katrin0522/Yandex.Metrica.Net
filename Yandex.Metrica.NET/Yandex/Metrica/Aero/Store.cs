using System;
using System.Collections.Generic;
using System.Linq;

namespace Yandex.Metrica.Aero
{
	internal static class Store
	{
		private static readonly object GlobalLock = new object();

		private static readonly Dictionary<Type, object> LocalLocks = new Dictionary<Type, object>();

		public static readonly Dictionary<Type, object> Container = new Dictionary<Type, object>();

		public static TItem Get<TItem>(params object[] constructorArgs) where TItem : class
		{
			Type typeFromHandle = typeof(TItem);
			object value;
			if (Container.TryGetValue(typeFromHandle, out value))
			{
				return (TItem)value;
			}
			object value2;
			lock (GlobalLock)
			{
				if (!LocalLocks.TryGetValue(typeFromHandle, out value2))
				{
					LocalLocks.Add(typeFromHandle, value2 = new object());
				}
			}
			lock (value2)
			{
				if (!Container.TryGetValue(typeFromHandle, out value))
				{
					Container.Add(typeFromHandle, value = ReviveItem<TItem>(constructorArgs));
				}
			}
			return (TItem)value;
		}

		private static TItem ReviveItem<TItem>(params object[] constructorArgs) where TItem : class
		{
			TItem val = Memory.ActiveBox.Revive<TItem>(null, constructorArgs);
			IExposable obj = val as IExposable;
			if (obj != null)
			{
				obj.Expose();
				return val;
			}
			return val;
		}

		public static void Snapshot()
		{
			Container.Values.ToArray().ForEach((object i) =>
			{
				Memory.ActiveBox.Keep(i);
			});
		}

		public static void Snapshot(this object item)
		{
			Memory.ActiveBox.Keep(item);
		}
	}
}
