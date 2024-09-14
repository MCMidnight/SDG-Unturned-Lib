using System;

namespace SDG.Unturned
{
	/// <summary>
	/// Read commands from standard input, and write logs to standard output.
	/// </summary>
	// Token: 0x020003DF RID: 991
	public class ConsoleInputOutput : ConsoleInputOutputBase
	{
		// Token: 0x06001D86 RID: 7558 RVA: 0x0006C08C File Offset: 0x0006A28C
		public override void update()
		{
			base.update();
			this.inputFromConsole();
		}

		// Token: 0x06001D87 RID: 7559 RVA: 0x0006C09A File Offset: 0x0006A29A
		protected void clearLine()
		{
			Console.CursorLeft = 0;
			Console.Write(new string(' ', Console.BufferWidth));
			Console.CursorTop--;
			Console.CursorLeft = 0;
		}

		// Token: 0x06001D88 RID: 7560 RVA: 0x0006C0C5 File Offset: 0x0006A2C5
		protected void redrawInputLine()
		{
			if (Console.CursorLeft > 0)
			{
				this.clearLine();
			}
			Console.ForegroundColor = 15;
			Console.Write(this.pendingInput);
		}

		// Token: 0x06001D89 RID: 7561 RVA: 0x0006C0E7 File Offset: 0x0006A2E7
		protected override void outputToConsole(string value, ConsoleColor color)
		{
			if (Console.CursorLeft != 0)
			{
				this.clearLine();
			}
			base.outputToConsole(value, color);
			this.redrawInputLine();
		}

		/// <summary>
		/// Each Update we consume a key press from the console buffer if available.
		/// Unfortunately ReadLine is not an option without blocking output, so we maintain our own pending input.
		/// </summary>
		// Token: 0x06001D8A RID: 7562 RVA: 0x0006C104 File Offset: 0x0006A304
		protected virtual void inputFromConsole()
		{
			if (Console.KeyAvailable)
			{
				ConsoleKeyInfo keyInfo = Console.ReadKey();
				this.onConsoleInputKey(keyInfo);
			}
		}

		// Token: 0x06001D8B RID: 7563 RVA: 0x0006C128 File Offset: 0x0006A328
		protected virtual void onConsoleInputKey(ConsoleKeyInfo keyInfo)
		{
			ConsoleKey key = keyInfo.Key;
			if (key == 13)
			{
				this.onConsoleInputEnter();
				return;
			}
			if (key == 8)
			{
				this.onConsoleInputBackspace();
				return;
			}
			if (key == 27)
			{
				this.onConsoleInputEscape();
				return;
			}
			if (keyInfo.KeyChar != '\0')
			{
				this.pendingInput += keyInfo.KeyChar.ToString();
				this.redrawInputLine();
			}
		}

		// Token: 0x06001D8C RID: 7564 RVA: 0x0006C190 File Offset: 0x0006A390
		protected virtual void onConsoleInputEnter()
		{
			string text = this.pendingInput;
			this.pendingInput = string.Empty;
			this.clearLine();
			this.outputInformation(">" + text);
			base.notifyInputCommitted(text);
		}

		// Token: 0x06001D8D RID: 7565 RVA: 0x0006C1D0 File Offset: 0x0006A3D0
		protected virtual void onConsoleInputBackspace()
		{
			int length = this.pendingInput.Length;
			if (length != 0)
			{
				if (length != 1)
				{
					this.pendingInput = this.pendingInput.Substring(0, length - 1);
				}
				else
				{
					this.pendingInput = string.Empty;
				}
				this.redrawInputLine();
				return;
			}
		}

		// Token: 0x06001D8E RID: 7566 RVA: 0x0006C21B File Offset: 0x0006A41B
		protected virtual void onConsoleInputEscape()
		{
			if (this.pendingInput.Length < 1)
			{
				return;
			}
			this.pendingInput = string.Empty;
			this.clearLine();
		}

		// Token: 0x04000DD9 RID: 3545
		protected string pendingInput = string.Empty;
	}
}
