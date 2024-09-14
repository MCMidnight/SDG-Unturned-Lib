using System;

namespace SDG.Unturned
{
	/// <summary>
	/// Interface between the dedicated server command I/O and per-platform console.
	/// </summary>
	// Token: 0x020003E5 RID: 997
	public interface ICommandInputOutput
	{
		/// <summary>
		/// Called when this implementation is setup by command window.
		/// </summary>
		// Token: 0x06001DB4 RID: 7604
		void initialize(CommandWindow commandWindow);

		/// <summary>
		/// Called when this implementation is deleted or application quits.
		/// </summary>
		// Token: 0x06001DB5 RID: 7605
		void shutdown(CommandWindow commandWindow);

		/// <summary>
		/// Called each Unity update.
		/// </summary>
		// Token: 0x06001DB6 RID: 7606
		void update();

		/// <summary>
		/// Broadcasts when the enter key is pressed.
		/// </summary>
		// Token: 0x14000077 RID: 119
		// (add) Token: 0x06001DB7 RID: 7607
		// (remove) Token: 0x06001DB8 RID: 7608
		event CommandInputHandler inputCommitted;

		/// <summary>
		/// Print white message.
		/// </summary>
		// Token: 0x06001DB9 RID: 7609
		void outputInformation(string information);

		/// <summary>
		/// Print yellow message.
		/// </summary>
		// Token: 0x06001DBA RID: 7610
		void outputWarning(string warning);

		/// <summary>
		/// Print red message.
		/// </summary>
		// Token: 0x06001DBB RID: 7611
		void outputError(string error);
	}
}
