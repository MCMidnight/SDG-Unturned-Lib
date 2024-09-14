using System;

namespace SDG.Unturned
{
	/// <summary>
	/// Matches the console behavior prior to command IO refactor.
	/// </summary>
	// Token: 0x020003E6 RID: 998
	public class LegacyInputOutput : ICommandInputOutput
	{
		// Token: 0x06001DBC RID: 7612 RVA: 0x0006C7B5 File Offset: 0x0006A9B5
		public virtual void initialize(CommandWindow commandWindow)
		{
		}

		// Token: 0x06001DBD RID: 7613 RVA: 0x0006C7B7 File Offset: 0x0006A9B7
		public virtual void shutdown(CommandWindow commandWindow)
		{
		}

		// Token: 0x06001DBE RID: 7614 RVA: 0x0006C7B9 File Offset: 0x0006A9B9
		public virtual void update()
		{
		}

		// Token: 0x14000078 RID: 120
		// (add) Token: 0x06001DBF RID: 7615 RVA: 0x0006C7BC File Offset: 0x0006A9BC
		// (remove) Token: 0x06001DC0 RID: 7616 RVA: 0x0006C7F4 File Offset: 0x0006A9F4
		public event CommandInputHandler inputCommitted;

		// Token: 0x06001DC1 RID: 7617 RVA: 0x0006C829 File Offset: 0x0006AA29
		public virtual void outputInformation(string information)
		{
			this.outputToConsole(information, 15);
		}

		// Token: 0x06001DC2 RID: 7618 RVA: 0x0006C834 File Offset: 0x0006AA34
		public virtual void outputWarning(string warning)
		{
			this.outputToConsole(warning, 14);
		}

		// Token: 0x06001DC3 RID: 7619 RVA: 0x0006C83F File Offset: 0x0006AA3F
		public virtual void outputError(string error)
		{
			this.outputToConsole(error, 12);
		}

		// Token: 0x06001DC4 RID: 7620 RVA: 0x0006C84A File Offset: 0x0006AA4A
		protected virtual void outputToConsole(string value, ConsoleColor color)
		{
			Console.ForegroundColor = color;
			Console.WriteLine(value);
		}
	}
}
