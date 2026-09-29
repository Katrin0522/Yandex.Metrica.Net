using System;
using System.Threading.Tasks;
using Yandex.Metrica.Aero;
using Yandex.Metrica.Patterns;

namespace Yandex.Metrica
{
	internal class Lifecycler : IExposable, ILifecycler
	{
		public bool IsBackgroundTask => false;

		public event EventHandler End = (object sender, EventArgs args) =>
		{
		};

		public event EventHandler Start = (object sender, EventArgs args) =>
		{
		};

		public event EventHandler Resume = (object sender, EventArgs args) =>
		{
		};

		public event EventHandler Suspend = (object sender, EventArgs args) =>
		{
		};

		public event EventHandler UnhandledException = (object sender, EventArgs args) =>
		{
		};

		~Lifecycler()
		{
			End(this, EventArgs.Empty);
		}

		public Lifecycler()
		{
			Start(this, EventArgs.Empty);
		}

		public void Expose()
		{
			TaskScheduler.UnobservedTaskException += (object sender, UnobservedTaskExceptionEventArgs args) =>
			{
				UnhandledException(this, args);
			};
			AppDomain.CurrentDomain.UnhandledException += (object sender, UnhandledExceptionEventArgs args) =>
			{
				UnhandledException(sender, args);
			};
		}
	}
}
