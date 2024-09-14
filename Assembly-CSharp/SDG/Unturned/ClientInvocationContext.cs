using System;
using System.Diagnostics;
using SDG.NetPak;

namespace SDG.Unturned
{
	/// <summary>
	/// Optional parameter for error logging.
	/// </summary>
	// Token: 0x0200022D RID: 557
	public readonly struct ClientInvocationContext
	{
		// Token: 0x06001132 RID: 4402 RVA: 0x0003B6F4 File Offset: 0x000398F4
		[Conditional("UNITY_EDITOR")]
		[Conditional("DEVELOPMENT_BUILD")]
		[Conditional("DEBUG_NETINVOKABLES")]
		public void ReadParameterFailed(string parameterName)
		{
			UnturnedLog.warn("{0}: unable to read {1}", new object[]
			{
				this.clientMethodInfo,
				parameterName
			});
		}

		// Token: 0x06001133 RID: 4403 RVA: 0x0003B713 File Offset: 0x00039913
		[Conditional("UNITY_EDITOR")]
		[Conditional("DEVELOPMENT_BUILD")]
		[Conditional("DEBUG_NETINVOKABLES")]
		public void IndexOutOfRange(string parameterName, int index, int max)
		{
			UnturnedLog.error("{0}: {1} out of range ({2}/{3})", new object[]
			{
				this.clientMethodInfo,
				parameterName,
				index,
				max
			});
		}

		// Token: 0x06001134 RID: 4404 RVA: 0x0003B744 File Offset: 0x00039944
		[Conditional("UNITY_EDITOR")]
		[Conditional("DEVELOPMENT_BUILD")]
		[Conditional("DEBUG_NETINVOKABLES")]
		public void LogWarning(string message)
		{
			UnturnedLog.warn("{0}: {1}", new object[]
			{
				this.clientMethodInfo,
				message
			});
		}

		// Token: 0x06001135 RID: 4405 RVA: 0x0003B763 File Offset: 0x00039963
		internal ClientInvocationContext(ClientInvocationContext.EOrigin origin, NetPakReader reader, ClientMethodInfo clientMethodInfo)
		{
			this.origin = origin;
			this.reader = reader;
			this.clientMethodInfo = clientMethodInfo;
		}

		// Token: 0x04000565 RID: 1381
		public readonly ClientInvocationContext.EOrigin origin;

		// Token: 0x04000566 RID: 1382
		public readonly NetPakReader reader;

		// Token: 0x04000567 RID: 1383
		internal readonly ClientMethodInfo clientMethodInfo;

		// Token: 0x0200089C RID: 2204
		public enum EOrigin
		{
			// Token: 0x040031CA RID: 12746
			Remote,
			// Token: 0x040031CB RID: 12747
			Loopback,
			// Token: 0x040031CC RID: 12748
			Deferred
		}
	}
}
