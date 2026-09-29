using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yandex.Metrica.Aero
{
	internal static class Sugar
	{
		public static T Of<T>(this object o)
		{
			return (T)o;
		}

		public static T As<T>(this object o) where T : class
		{
			return o as T;
		}

		public static bool Is<T>(this object o)
		{
			return o is T;
		}

		public static bool IsNull(this object o)
		{
			return o == null;
		}

		public static bool IsNotNull(this object o)
		{
			return o != null;
		}

		public static bool IsNullOrEmpty(this string value)
		{
			return string.IsNullOrEmpty(value);
		}

		public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
		{
			if (collection != null)
			{
				return !collection.Any();
			}
			return true;
		}

		public static StringBuilder Append<T>(this StringBuilder builder, params T[] args)
		{
			foreach (T val in args)
			{
				builder.Append(val);
			}
			return builder;
		}

		public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
		{
			foreach (T item in collection)
			{
				action(item);
			}
		}

		public static TException Try<TException>(Action action) where TException : Exception
		{
			try
			{
				action();
				return null;
			}
			catch (TException result)
			{
				return result;
			}
		}

		public static Exception Catch<TException>(this Exception exception, Action<Exception> action) where TException : Exception
		{
			if (exception.As<TException>() != null)
			{
				action(exception);
			}
			return exception;
		}

		public static Exception Try(Action action)
		{
			return Try<Exception>(action);
		}

		public static Exception Catch(this Exception e, Action<Exception> action)
		{
			return e.Catch<Exception>(action);
		}

		public static void Finally(this Exception e, Action action)
		{
			action();
		}

		public static TResult With<TSource, TResult>(this TSource source, Func<TSource, TResult> action) where TSource : class
		{
			if (source != null)
			{
				return action(source);
			}
			return default(TResult);
		}

		public static TSource Do<TSource>(this TSource source, Action<TSource> action) where TSource : class
		{
			if (source != null)
			{
				action(source);
			}
			return source;
		}

		public static IEnumerable<T> Turn<T>(this IList<T> items, int skip, int turnsCount = 0)
		{
			bool flag = skip < 0;
			int count = items.Count;
			skip = (flag ? (count + skip) : skip);
			int take;
			if (turnsCount != 0)
			{
				take = count * turnsCount;
			}
			else
			{
				take = (flag ? (-skip - 1) : (count - skip));
			}
			return items.Ring(skip, take);
		}

		public static IEnumerable<T> Ring<T>(this IList<T> items, int skip, int take)
		{
			bool reverse = take < 0;
			int count = items.Count;
			skip = ((skip < 0) ? (count + skip) : skip);
			skip = ((skip < count) ? skip : (skip % count));
			take = (reverse ? (-take) : take);
			for (int i = 0; i < take; i++)
			{
				int num = ((i < count) ? i : (i % count));
				int num2 = (reverse ? (skip - num) : (skip + num));
				num2 = ((num2 < 0) ? (count + num2) : num2);
				num2 = ((num2 < count) ? num2 : (num2 % count));
				yield return items[num2];
			}
		}

		public static IEnumerable<T> SkipByRing<T>(this IEnumerable<T> source, int count)
		{
			int originalCount = 0;
			bool flag = count < 0;
			count = (flag ? (-count) : count);
			source = (flag ? source.Reverse() : source);
			do
			{
				if (originalCount > 0)
				{
					count %= originalCount;
				}
				foreach (T item in source)
				{
					originalCount++;
					if (count > 0)
					{
						count--;
					}
					else
					{
						yield return item;
					}
				}
			}
			while (count != 0);
		}

		public static IEnumerable<T> TakeByRing<T>(this IEnumerable<T> source, int count)
		{
			bool flag = count < 0;
			count = (flag ? (-count) : count);
			source = (flag ? source.Reverse() : source);
			do
			{
				foreach (T item in source)
				{
					if (count > 0)
					{
						count--;
						yield return item;
					}
				}
			}
			while (count != 0);
		}

		public static IEnumerable<T> SliceByRing<T>(this IEnumerable<T> source, int skipCount, int takeCount)
		{
			int originalCount = 0;
			bool flag = skipCount < 0;
			bool flag2 = takeCount < 0;
			skipCount = (flag ? (-skipCount) : skipCount);
			takeCount = (flag2 ? (-takeCount) : takeCount);
			source = (flag2 ? source.Reverse() : source);
			if (flag ^ flag2)
			{
				int num = source.Count();
				skipCount = num - skipCount % num;
			}
			do
			{
				if (originalCount > 0)
				{
					skipCount %= originalCount;
				}
				foreach (T item in source)
				{
					originalCount++;
					if (skipCount > 0)
					{
						skipCount--;
					}
					else if (takeCount > 0)
					{
						takeCount--;
						yield return item;
					}
				}
			}
			while (takeCount != 0);
		}
	}
}
