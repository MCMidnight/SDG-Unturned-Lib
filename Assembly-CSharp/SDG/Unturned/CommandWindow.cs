using System;
using System.Collections.Generic;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020003DE RID: 990
	public class CommandWindow
	{
		/// <summary>
		/// Broadcasts after dedicated server name changes.
		/// Command IO interface binds to this rather than having a title-specific method.
		/// </summary>
		// Token: 0x14000075 RID: 117
		// (add) Token: 0x06001D6C RID: 7532 RVA: 0x0006B9B8 File Offset: 0x00069BB8
		// (remove) Token: 0x06001D6D RID: 7533 RVA: 0x0006B9F0 File Offset: 0x00069BF0
		public event CommandWindowTitleChanged onTitleChanged;

		// Token: 0x17000615 RID: 1557
		// (get) Token: 0x06001D6E RID: 7534 RVA: 0x0006BA25 File Offset: 0x00069C25
		// (set) Token: 0x06001D6F RID: 7535 RVA: 0x0006BA2D File Offset: 0x00069C2D
		public string title
		{
			get
			{
				return this._title;
			}
			set
			{
				this._title = value;
				CommandWindowTitleChanged commandWindowTitleChanged = this.onTitleChanged;
				if (commandWindowTitleChanged == null)
				{
					return;
				}
				commandWindowTitleChanged(this._title);
			}
		}

		/// <summary>
		/// Log white information.
		/// </summary>
		// Token: 0x06001D70 RID: 7536 RVA: 0x0006BA4C File Offset: 0x00069C4C
		public static void Log(object text)
		{
			if (text == null)
			{
				return;
			}
			if (CommandWindow.insideExplicitLogging)
			{
				return;
			}
			try
			{
				CommandWindow.insideExplicitLogging = true;
				UnturnedLog.info(text);
				CommandWindow commandWindow = Dedicator.commandWindow;
				if (commandWindow != null)
				{
					commandWindow.internalLogInformation(text.ToString());
				}
			}
			finally
			{
				CommandWindow.insideExplicitLogging = false;
			}
		}

		// Token: 0x06001D71 RID: 7537 RVA: 0x0006BAA4 File Offset: 0x00069CA4
		public static void LogFormat(string format, params object[] args)
		{
			CommandWindow.Log(string.Format(format, args));
		}

		/// <summary>
		/// Log yellow warning.
		/// </summary>
		// Token: 0x06001D72 RID: 7538 RVA: 0x0006BAB4 File Offset: 0x00069CB4
		public static void LogWarning(object text)
		{
			if (text == null)
			{
				return;
			}
			if (CommandWindow.insideExplicitLogging)
			{
				return;
			}
			try
			{
				CommandWindow.insideExplicitLogging = true;
				UnturnedLog.warn(text);
				CommandWindow commandWindow = Dedicator.commandWindow;
				if (commandWindow != null)
				{
					commandWindow.internalLogWarning(text.ToString());
				}
			}
			finally
			{
				CommandWindow.insideExplicitLogging = false;
			}
		}

		// Token: 0x06001D73 RID: 7539 RVA: 0x0006BB0C File Offset: 0x00069D0C
		public static void LogWarningFormat(string format, params object[] args)
		{
			CommandWindow.LogWarning(string.Format(format, args));
		}

		/// <summary>
		/// Log red error.
		/// </summary>
		// Token: 0x06001D74 RID: 7540 RVA: 0x0006BB1C File Offset: 0x00069D1C
		public static void LogError(object text)
		{
			if (text == null)
			{
				return;
			}
			if (CommandWindow.insideExplicitLogging)
			{
				return;
			}
			try
			{
				CommandWindow.insideExplicitLogging = true;
				UnturnedLog.error(text);
				CommandWindow commandWindow = Dedicator.commandWindow;
				if (commandWindow != null)
				{
					commandWindow.internalLogError(text.ToString());
				}
			}
			finally
			{
				CommandWindow.insideExplicitLogging = false;
			}
		}

		// Token: 0x06001D75 RID: 7541 RVA: 0x0006BB74 File Offset: 0x00069D74
		public static void LogErrorFormat(string format, params object[] args)
		{
			CommandWindow.LogError(string.Format(format, args));
		}

		/// <summary>
		/// Print white message to console.
		/// </summary>
		// Token: 0x06001D76 RID: 7542 RVA: 0x0006BB84 File Offset: 0x00069D84
		private void internalLogInformation(string information)
		{
			try
			{
				CommandWindowOutputted commandWindowOutputted = CommandWindow.onCommandWindowOutputted;
				if (commandWindowOutputted != null)
				{
					commandWindowOutputted(information, 15);
				}
			}
			catch (Exception exception)
			{
				this.HandleException("Plugin threw an exception from info onCommandWindowOutputted:", exception);
			}
			foreach (ICommandInputOutput commandInputOutput in this.ioHandlers)
			{
				try
				{
					commandInputOutput.outputInformation(information);
				}
				catch (Exception exception2)
				{
					this.HandleException(string.Format("Command IO handler {0} threw an exception from outputInformation:", commandInputOutput), exception2);
				}
			}
		}

		/// <summary>
		/// Print yellow message to console.
		/// </summary>
		// Token: 0x06001D77 RID: 7543 RVA: 0x0006BC2C File Offset: 0x00069E2C
		private void internalLogWarning(string warning)
		{
			try
			{
				CommandWindowOutputted commandWindowOutputted = CommandWindow.onCommandWindowOutputted;
				if (commandWindowOutputted != null)
				{
					commandWindowOutputted(warning, 14);
				}
			}
			catch (Exception exception)
			{
				this.HandleException("Plugin threw an exception from warning onCommandWindowOutputted:", exception);
			}
			foreach (ICommandInputOutput commandInputOutput in this.ioHandlers)
			{
				try
				{
					commandInputOutput.outputWarning(warning);
				}
				catch (Exception exception2)
				{
					this.HandleException(string.Format("Command IO handler {0} threw an exception from outputWarning:", commandInputOutput), exception2);
				}
			}
		}

		/// <summary>
		/// Print red message to console.
		/// </summary>
		// Token: 0x06001D78 RID: 7544 RVA: 0x0006BCD4 File Offset: 0x00069ED4
		private void internalLogError(string error)
		{
			try
			{
				CommandWindowOutputted commandWindowOutputted = CommandWindow.onCommandWindowOutputted;
				if (commandWindowOutputted != null)
				{
					commandWindowOutputted(error, 12);
				}
			}
			catch (Exception exception)
			{
				this.HandleException("Plugin threw an exception from error onCommandWindowOutputted:", exception);
			}
			foreach (ICommandInputOutput commandInputOutput in this.ioHandlers)
			{
				try
				{
					commandInputOutput.outputError(error);
				}
				catch (Exception exception2)
				{
					this.HandleException(string.Format("Command IO handler {0} threw an exception from outputError:", commandInputOutput), exception2);
				}
			}
		}

		// Token: 0x06001D79 RID: 7545 RVA: 0x0006BD7C File Offset: 0x00069F7C
		private void onInputCommitted(string input)
		{
			bool flag = true;
			try
			{
				CommandWindowInputted commandWindowInputted = CommandWindow.onCommandWindowInputted;
				if (commandWindowInputted != null)
				{
					commandWindowInputted(input, ref flag);
				}
			}
			catch (Exception exception)
			{
				this.HandleException("Plugin threw an exception from onCommandWindowInputted:", exception);
			}
			if (flag && !Commander.execute(CSteamID.Nil, input))
			{
				CommandWindow.LogErrorFormat("Unable to match \"{0}\" with any built-in commands", new object[]
				{
					input
				});
			}
		}

		/// <summary>
		/// Cannot use UnturnedLog here because it may recursively call CommandWindow if another exception is thrown.
		/// </summary>
		// Token: 0x06001D7A RID: 7546 RVA: 0x0006BDE4 File Offset: 0x00069FE4
		private void HandleException(string message, Exception exception)
		{
			Logs.printLine(message);
			do
			{
				Logs.printLine(exception.Message);
				Logs.printLine(exception.StackTrace);
				exception = exception.InnerException;
			}
			while (exception != null);
		}

		/// <summary>
		/// Called during Unity Update loop.
		/// </summary>
		// Token: 0x06001D7B RID: 7547 RVA: 0x0006BE10 File Offset: 0x0006A010
		public void update()
		{
			foreach (ICommandInputOutput commandInputOutput in this.ioHandlers)
			{
				try
				{
					commandInputOutput.update();
				}
				catch (Exception exception)
				{
					this.HandleException(string.Format("Command IO handler {0} threw an exception from update:", commandInputOutput), exception);
				}
			}
		}

		// Token: 0x06001D7C RID: 7548 RVA: 0x0006BE88 File Offset: 0x0006A088
		private void initializeIOHandler(ICommandInputOutput handler)
		{
			try
			{
				handler.initialize(this);
			}
			catch (Exception e)
			{
				UnturnedLog.exception(e);
			}
			handler.inputCommitted += this.onInputCommitted;
		}

		// Token: 0x06001D7D RID: 7549 RVA: 0x0006BEC8 File Offset: 0x0006A0C8
		private void shutdownIOHandler(ICommandInputOutput handler)
		{
			handler.inputCommitted -= this.onInputCommitted;
			try
			{
				handler.shutdown(this);
			}
			catch (Exception e)
			{
				UnturnedLog.exception(e);
			}
		}

		/// <summary>
		/// Called during OnApplicationQuit.
		/// </summary>
		// Token: 0x06001D7E RID: 7550 RVA: 0x0006BF08 File Offset: 0x0006A108
		public void shutdown()
		{
			List<ICommandInputOutput> list = new List<ICommandInputOutput>(this.ioHandlers);
			this.ioHandlers.Clear();
			foreach (ICommandInputOutput handler in list)
			{
				this.shutdownIOHandler(handler);
			}
		}

		/// <summary>
		/// Helper for plugins that want to replace the default without the shouldCreateDefaultConsole flag.
		/// </summary>
		// Token: 0x06001D7F RID: 7551 RVA: 0x0006BF6C File Offset: 0x0006A16C
		public void removeDefaultIOHandler()
		{
			if (this.defaultIOHandler != null)
			{
				this.removeIOHandler(this.defaultIOHandler);
				this.defaultIOHandler = null;
			}
		}

		// Token: 0x06001D80 RID: 7552 RVA: 0x0006BF89 File Offset: 0x0006A189
		public void removeIOHandler(ICommandInputOutput handler)
		{
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			this.ioHandlers.RemoveFast(handler);
			this.shutdownIOHandler(handler);
		}

		// Token: 0x06001D81 RID: 7553 RVA: 0x0006BFAD File Offset: 0x0006A1AD
		public void addIOHandler(ICommandInputOutput handler)
		{
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			if (this.ioHandlers.Contains(handler))
			{
				throw new NotSupportedException("handler already registered");
			}
			this.ioHandlers.Add(handler);
			this.initializeIOHandler(handler);
		}

		// Token: 0x06001D82 RID: 7554 RVA: 0x0006BFE9 File Offset: 0x0006A1E9
		[Obsolete("Use addIOHandler instead (multiple simultaneous handlers now supported)")]
		public void setIOHandler(ICommandInputOutput newHandler)
		{
			this.addIOHandler(newHandler);
		}

		// Token: 0x06001D83 RID: 7555 RVA: 0x0006BFF2 File Offset: 0x0006A1F2
		protected ICommandInputOutput createDefaultIOHandler()
		{
			if (!CommandWindow.shouldCreateDefaultConsole)
			{
				return null;
			}
			if (CommandWindow.shouldCreateLegacyConsole)
			{
				return new WindowsConsoleInputOutput();
			}
			return new ThreadedWindowsConsoleInputOutput();
		}

		// Token: 0x06001D84 RID: 7556 RVA: 0x0006C019 File Offset: 0x0006A219
		public CommandWindow()
		{
			this.defaultIOHandler = this.createDefaultIOHandler();
			if (this.defaultIOHandler != null)
			{
				this.addIOHandler(this.defaultIOHandler);
			}
		}

		// Token: 0x04000DCC RID: 3532
		public static CommandWindowInputted onCommandWindowInputted;

		// Token: 0x04000DCD RID: 3533
		public static CommandWindowOutputted onCommandWindowOutputted;

		/// <summary>
		/// Should the default console I/O handler be created?
		/// Plugins can disable on the command line when overriding handler.
		/// </summary>
		// Token: 0x04000DCF RID: 3535
		public static CommandLineFlag shouldCreateDefaultConsole = new CommandLineFlag(true, "-NoDefaultConsole");

		/// <summary>
		/// Should the legacy blocking (game thread) console be created?
		/// </summary>
		// Token: 0x04000DD0 RID: 3536
		private static CommandLineFlag shouldCreateLegacyConsole = new CommandLineFlag(false, "-LegacyConsole");

		// Token: 0x04000DD1 RID: 3537
		private string _title;

		// Token: 0x04000DD2 RID: 3538
		public static bool shouldLogChat = true;

		// Token: 0x04000DD3 RID: 3539
		public static bool shouldLogJoinLeave = true;

		// Token: 0x04000DD4 RID: 3540
		public static bool shouldLogDeaths = true;

		// Token: 0x04000DD5 RID: 3541
		public static bool shouldLogAnticheat = false;

		// Token: 0x04000DD6 RID: 3542
		private static bool insideExplicitLogging = false;

		// Token: 0x04000DD7 RID: 3543
		private List<ICommandInputOutput> ioHandlers = new List<ICommandInputOutput>();

		// Token: 0x04000DD8 RID: 3544
		private ICommandInputOutput defaultIOHandler;
	}
}
