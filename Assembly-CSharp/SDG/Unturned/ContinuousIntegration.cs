using System;
using System.Diagnostics;

namespace SDG.Unturned
{
	/// <summary>
	/// Report success or failure from game systems, conditionally compiled into the Windows 64-bit build.
	/// </summary>
	// Token: 0x020007F7 RID: 2039
	public class ContinuousIntegration
	{
		/// <summary>
		/// Call when the server is done all loading without running into errors.
		/// Ignored if not running in CI mode, otherwise exits the server successfully with error code 0.
		/// </summary>
		// Token: 0x06004612 RID: 17938 RVA: 0x001A2D6E File Offset: 0x001A0F6E
		[Conditional("DEVELOPMENT_BUILD")]
		public static void reportSuccess()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Call when the server encounters any error.
		/// Ignored if not running in CI mode, otherwise exits the server with error code 1.
		/// </summary>
		// Token: 0x06004613 RID: 17939 RVA: 0x001A2D75 File Offset: 0x001A0F75
		[Conditional("DEVELOPMENT_BUILD")]
		public static void reportFailure(object message)
		{
			throw new NotImplementedException();
		}
	}
}
