using System;
using System.Collections.Concurrent;
using System.Threading;

namespace SDG.Unturned
{
	// Token: 0x020003E7 RID: 999
	public class ThreadedConsoleInputOutput : ConsoleInputOutputBase
	{
		// Token: 0x06001DC6 RID: 7622 RVA: 0x0006C860 File Offset: 0x0006AA60
		public override void initialize(CommandWindow commandWindow)
		{
			base.initialize(commandWindow);
			this.inputThread = new Thread(new ThreadStart(this.consoleMain));
			this.inputThread.Start();
		}

		// Token: 0x06001DC7 RID: 7623 RVA: 0x0006C88B File Offset: 0x0006AA8B
		public override void shutdown(CommandWindow commandWindow)
		{
			base.shutdown(commandWindow);
			if (this.inputThread != null)
			{
				this.inputThread.Abort();
				this.inputThread = null;
			}
		}

		// Token: 0x06001DC8 RID: 7624 RVA: 0x0006C8B0 File Offset: 0x0006AAB0
		public override void update()
		{
			base.update();
			string input;
			while (this.pendingInputs.TryDequeue(ref input))
			{
				base.notifyInputCommitted(input);
			}
		}

		// Token: 0x06001DC9 RID: 7625 RVA: 0x0006C8DC File Offset: 0x0006AADC
		protected override void outputToConsole(string value, ConsoleColor color)
		{
			ThreadedConsoleInputOutput.PendingOutput pendingOutput = new ThreadedConsoleInputOutput.PendingOutput
			{
				value = value,
				color = color
			};
			this.pendingOutputs.Enqueue(pendingOutput);
		}

		// Token: 0x06001DCA RID: 7626 RVA: 0x0006C910 File Offset: 0x0006AB10
		private void consoleMain()
		{
			for (;;)
			{
				if (!this.pendingOutputs.IsEmpty)
				{
					ConsoleColor foregroundColor = Console.ForegroundColor;
					ThreadedConsoleInputOutput.PendingOutput pendingOutput;
					while (this.pendingOutputs.TryDequeue(ref pendingOutput))
					{
						Console.ForegroundColor = pendingOutput.color;
						Console.WriteLine(pendingOutput.value);
					}
					Console.ForegroundColor = foregroundColor;
				}
				if (Console.KeyAvailable)
				{
					string text = Console.ReadLine();
					if (!string.IsNullOrWhiteSpace(text))
					{
						this.pendingInputs.Enqueue(text);
					}
				}
				Thread.Sleep(10);
			}
		}

		// Token: 0x04000DF0 RID: 3568
		private Thread inputThread;

		// Token: 0x04000DF1 RID: 3569
		private ConcurrentQueue<string> pendingInputs = new ConcurrentQueue<string>();

		// Token: 0x04000DF2 RID: 3570
		private ConcurrentQueue<ThreadedConsoleInputOutput.PendingOutput> pendingOutputs = new ConcurrentQueue<ThreadedConsoleInputOutput.PendingOutput>();

		// Token: 0x0200092F RID: 2351
		private struct PendingOutput
		{
			// Token: 0x0400329D RID: 12957
			public string value;

			// Token: 0x0400329E RID: 12958
			public ConsoleColor color;
		}
	}
}
