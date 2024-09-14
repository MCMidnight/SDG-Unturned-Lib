using System;

namespace SDG.Unturned
{
	// Token: 0x020006F4 RID: 1780
	public class AutoSubscribeMap
	{
		/// <summary>
		/// Published steam id to subscribe to.
		/// </summary>
		// Token: 0x040024FF RID: 9471
		public ulong Workshop_File_Id;

		/// <summary>
		/// If logging in after this point, subscribe.
		/// </summary>
		// Token: 0x04002500 RID: 9472
		public DateTime Start;

		/// <summary>
		/// If logging in before this point, subscribe. 
		/// </summary>
		// Token: 0x04002501 RID: 9473
		public DateTime End;
	}
}
