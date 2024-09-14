using System;
using System.Collections.Generic;
using SDG.NetPak;
using SDG.NetTransport;

namespace SDG.Unturned
{
	// Token: 0x0200022A RID: 554
	public sealed class ClientInstanceMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : ClientInstanceMethodBase
	{
		// Token: 0x0600111D RID: 4381 RVA: 0x0003B256 File Offset: 0x00039456
		public static ClientInstanceMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Get(Type declaringType, string methodName)
		{
			return ClientMethodHandle.GetInternal<ClientInstanceMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, ClientInstanceMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.WriteDelegate>(declaringType, methodName, (ClientMethodInfo clientMethodInfo, ClientInstanceMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.WriteDelegate generatedWrite) => new ClientInstanceMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(clientMethodInfo, generatedWrite));
		}

		// Token: 0x0600111E RID: 4382 RVA: 0x0003B280 File Offset: 0x00039480
		public void Invoke(NetId netId, ENetReliability reliability, ITransportConnection transportConnection, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
		{
			NetPakWriter writerWithInstanceHeader = base.GetWriterWithInstanceHeader(netId);
			this.generatedWrite(writerWithInstanceHeader, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
			base.SendAndLoopbackIfLocal(reliability, transportConnection, writerWithInstanceHeader);
		}

		// Token: 0x0600111F RID: 4383 RVA: 0x0003B2C0 File Offset: 0x000394C0
		public void Invoke(NetId netId, ENetReliability reliability, List<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
		{
			NetPakWriter writerWithInstanceHeader = base.GetWriterWithInstanceHeader(netId);
			this.generatedWrite(writerWithInstanceHeader, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
			base.SendAndLoopbackIfAnyAreLocal(reliability, transportConnections, writerWithInstanceHeader);
		}

		// Token: 0x06001120 RID: 4384 RVA: 0x0003B300 File Offset: 0x00039500
		[Obsolete("Replaced by List overload")]
		public void Invoke(NetId netId, ENetReliability reliability, IEnumerable<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
		{
			List<ITransportConnection> list = transportConnections as List<ITransportConnection>;
			if (list != null)
			{
				this.Invoke(netId, reliability, list, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
				return;
			}
			throw new ArgumentException("should be list", "transportConnections");
		}

		// Token: 0x06001121 RID: 4385 RVA: 0x0003B344 File Offset: 0x00039544
		public void InvokeAndLoopback(NetId netId, ENetReliability reliability, List<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
		{
			NetPakWriter writerWithInstanceHeader = base.GetWriterWithInstanceHeader(netId);
			this.generatedWrite(writerWithInstanceHeader, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
			base.SendAndLoopback(reliability, transportConnections, writerWithInstanceHeader);
		}

		// Token: 0x06001122 RID: 4386 RVA: 0x0003B384 File Offset: 0x00039584
		[Obsolete("Replaced by List overload")]
		public void InvokeAndLoopback(NetId netId, ENetReliability reliability, IEnumerable<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
		{
			List<ITransportConnection> list = transportConnections as List<ITransportConnection>;
			if (list != null)
			{
				this.InvokeAndLoopback(netId, reliability, list, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
				return;
			}
			throw new ArgumentException("should be list", "transportConnections");
		}

		// Token: 0x06001123 RID: 4387 RVA: 0x0003B3C8 File Offset: 0x000395C8
		private ClientInstanceMethod(ClientMethodInfo clientMethodInfo, ClientInstanceMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.WriteDelegate generatedWrite) : base(clientMethodInfo)
		{
			this.generatedWrite = generatedWrite;
		}

		// Token: 0x04000562 RID: 1378
		private ClientInstanceMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.WriteDelegate generatedWrite;

		// Token: 0x02000896 RID: 2198
		// (Invoke) Token: 0x060048A3 RID: 18595
		private delegate void WriteDelegate(NetPakWriter writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10);
	}
}
