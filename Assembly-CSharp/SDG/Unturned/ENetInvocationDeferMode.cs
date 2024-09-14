using System;

namespace SDG.Unturned
{
	/// <summary>
	/// Defines how instance methods handle invocation when the target instance does not exist yet, for example
	/// if the target instance is async loading or has time sliced instantiation.
	/// </summary>
	// Token: 0x0200023F RID: 575
	public enum ENetInvocationDeferMode
	{
		/// <summary>
		/// Invocation should be ignored if the target instance does not exist.
		/// This is the only applicable defer mode for static methods and server methods.
		/// </summary>
		// Token: 0x0400057E RID: 1406
		Discard,
		/// <summary>
		/// Invocation will be queued up if the target instance does not exist.
		/// Originally an "Overwrite" mode was considered for cases like SetHealth where only the newest value is
		/// displayed, but this was potentially error-prone if multiple queued methods depended on values from each other.
		/// </summary>
		// Token: 0x0400057F RID: 1407
		Queue
	}
}
