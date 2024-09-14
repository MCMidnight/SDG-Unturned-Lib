using System;
using System.Text;
using System.Threading;

namespace SDG.Unturned
{
	/// <summary>
	/// Read commands from standard input, and write logs to standard output.
	/// </summary>
	// Token: 0x020003E0 RID: 992
	public class ConsoleInputOutputBase : ICommandInputOutput
	{
		// Token: 0x06001D90 RID: 7568 RVA: 0x0006C250 File Offset: 0x0006A450
		public ConsoleInputOutputBase()
		{
			this.shouldRedirectInput = ConsoleInputOutputBase.defaultShouldRedirectInput;
			this.shouldRedirectOutput = ConsoleInputOutputBase.defaultShouldRedirectOutput;
			this.shouldProxyRedirectedOutput = ConsoleInputOutputBase.defaultShouldProxyRedirectedOutput;
		}

		// Token: 0x06001D91 RID: 7569 RVA: 0x0006C288 File Offset: 0x0006A488
		public virtual void initialize(CommandWindow commandWindow)
		{
			this.desiredTitle = string.Empty;
			commandWindow.onTitleChanged += this.onTitleChanged;
			this.onTitleChanged(commandWindow.title);
			Console.CancelKeyPress += new ConsoleCancelEventHandler(this.handleCancelEvent);
			if (this.shouldRedirectInput)
			{
				this.inputRedirector = new ConsoleInputRedirector();
				this.inputRedirector.enable();
			}
			if (this.shouldRedirectOutput)
			{
				this.outputRedirector = new ConsoleOutputRedirector();
				this.outputRedirector.enable(this.shouldProxyRedirectedOutput);
			}
			string format = "Console output encoding: {0}";
			object[] array = new object[1];
			int num = 0;
			Encoding outputEncoding = Console.OutputEncoding;
			array[num] = ((outputEncoding != null) ? outputEncoding.EncodingName : null);
			UnturnedLog.info(format, array);
			Console.OutputEncoding = new UTF8Encoding(false);
		}

		// Token: 0x06001D92 RID: 7570 RVA: 0x0006C344 File Offset: 0x0006A544
		public virtual void shutdown(CommandWindow commandWindow)
		{
			commandWindow.onTitleChanged -= this.onTitleChanged;
			Console.CancelKeyPress -= new ConsoleCancelEventHandler(this.handleCancelEvent);
			if (this.inputRedirector != null)
			{
				this.inputRedirector.disable();
				this.inputRedirector = null;
			}
			if (this.outputRedirector != null)
			{
				this.outputRedirector.disable();
				this.outputRedirector = null;
			}
		}

		// Token: 0x06001D93 RID: 7571 RVA: 0x0006C3AA File Offset: 0x0006A5AA
		public virtual void update()
		{
			if (this.wantsToTerminate > 0 && !this.isTerminating)
			{
				this.isTerminating = true;
				this.handleTermination();
			}
		}

		// Token: 0x14000076 RID: 118
		// (add) Token: 0x06001D94 RID: 7572 RVA: 0x0006C3CC File Offset: 0x0006A5CC
		// (remove) Token: 0x06001D95 RID: 7573 RVA: 0x0006C404 File Offset: 0x0006A604
		public event CommandInputHandler inputCommitted;

		// Token: 0x06001D96 RID: 7574 RVA: 0x0006C439 File Offset: 0x0006A639
		public virtual void outputInformation(string information)
		{
			this.outputToConsole(information, 7);
		}

		// Token: 0x06001D97 RID: 7575 RVA: 0x0006C443 File Offset: 0x0006A643
		public virtual void outputWarning(string warning)
		{
			this.outputToConsole(warning, 14);
		}

		// Token: 0x06001D98 RID: 7576 RVA: 0x0006C44E File Offset: 0x0006A64E
		public virtual void outputError(string error)
		{
			this.outputToConsole(error, 12);
		}

		// Token: 0x06001D99 RID: 7577 RVA: 0x0006C459 File Offset: 0x0006A659
		protected virtual void outputToConsole(string value, ConsoleColor color)
		{
			Console.ForegroundColor = color;
			Console.WriteLine(value);
		}

		/// <summary>
		/// Broadcast the inputCommited event.
		/// </summary>
		// Token: 0x06001D9A RID: 7578 RVA: 0x0006C467 File Offset: 0x0006A667
		protected void notifyInputCommitted(string input)
		{
			CommandInputHandler commandInputHandler = this.inputCommitted;
			if (commandInputHandler == null)
			{
				return;
			}
			commandInputHandler(input);
		}

		/// <summary>
		/// Synchronize console's title bar text.
		/// Virtual because at one point Win32 SetTitleText was required.
		/// </summary>
		// Token: 0x06001D9B RID: 7579 RVA: 0x0006C47A File Offset: 0x0006A67A
		protected virtual void synchronizeTitle(string title)
		{
			Console.Title = this.desiredTitle;
		}

		// Token: 0x06001D9C RID: 7580 RVA: 0x0006C487 File Offset: 0x0006A687
		protected virtual void onTitleChanged(string title)
		{
			this.desiredTitle = title;
			if (!string.IsNullOrEmpty(this.desiredTitle))
			{
				this.synchronizeTitle(this.desiredTitle);
			}
		}

		/// <summary>
		/// Intercept the Ctrl-C or Ctrl-Break termination.
		/// </summary>
		// Token: 0x06001D9D RID: 7581 RVA: 0x0006C4A9 File Offset: 0x0006A6A9
		protected virtual void handleCancelEvent(object sender, ConsoleCancelEventArgs args)
		{
			args.Cancel = true;
			Interlocked.Exchange(ref this.wantsToTerminate, 1);
		}

		/// <summary>
		/// Handle Ctrl-C or Ctrl-Break on the game thread.
		/// </summary>
		// Token: 0x06001D9E RID: 7582 RVA: 0x0006C4BF File Offset: 0x0006A6BF
		protected virtual void handleTermination()
		{
			CommandWindow.Log("Handling SIGINT or SIGBREAK by requesting a graceful shutdown");
			Provider.shutdown();
		}

		// Token: 0x04000DDA RID: 3546
		public static CommandLineFlag defaultShouldRedirectInput = new CommandLineFlag(true, "-NoRedirectConsoleInput");

		// Token: 0x04000DDB RID: 3547
		public static CommandLineFlag defaultShouldRedirectOutput = new CommandLineFlag(true, "-NoRedirectConsoleOutput");

		// Token: 0x04000DDC RID: 3548
		public static CommandLineFlag defaultShouldProxyRedirectedOutput = new CommandLineFlag(false, "-ProxyRedirectedConsoleOutput");

		// Token: 0x04000DDE RID: 3550
		public bool shouldRedirectInput;

		// Token: 0x04000DDF RID: 3551
		public bool shouldRedirectOutput;

		// Token: 0x04000DE0 RID: 3552
		public bool shouldProxyRedirectedOutput;

		/// <summary>
		/// Has Ctrl-C or Ctrl-Break signal been received?
		/// </summary>
		// Token: 0x04000DE1 RID: 3553
		protected int wantsToTerminate;

		/// <summary>
		/// Is the Ctrl-C or Ctrl-Break signal being handled?
		/// </summary>
		// Token: 0x04000DE2 RID: 3554
		protected bool isTerminating;

		// Token: 0x04000DE3 RID: 3555
		protected string desiredTitle;

		// Token: 0x04000DE4 RID: 3556
		private ConsoleInputRedirector inputRedirector;

		// Token: 0x04000DE5 RID: 3557
		private ConsoleOutputRedirector outputRedirector;
	}
}
