using System;
using System.Collections.Generic;
using SDG.NetPak;
using SDG.NetTransport;

namespace SDG.Unturned
{
	// Token: 0x02000227 RID: 551
	public sealed class ClientInstanceMethod<T1, T2, T3, T4, T5, T6, T7> : ClientInstanceMethodBase
	{
		// Token: 0x06001108 RID: 4360 RVA: 0x0003AE18 File Offset: 0x00039018
		public static ClientInstanceMethod<T1, T2, T3, T4, T5, T6, T7> Get(Type declaringType, string methodName)
		{
			return ClientMethodHandle.GetInternal<ClientInstanceMethod<T1, T2, T3, T4, T5, T6, T7>, ClientInstanceMethod<T1, T2, T3, T4, T5, T6, T7>.WriteDelegate>(declaringType, methodName, (ClientMethodInfo clientMethodInfo, ClientInstanceMethod<T1, T2, T3, T4, T5, T6, T7>.WriteDelegate generatedWrite) => new ClientInstanceMethod<T1, T2, T3, T4, T5, T6, T7>(clientMethodInfo, generatedWrite));
		}

		// Token: 0x06001109 RID: 4361 RVA: 0x0003AE40 File Offset: 0x00039040
		public void Invoke(NetId netId, ENetReliability reliability, ITransportConnection transportConnection, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
		{
			NetPakWriter writerWithInstanceHeader = base.GetWriterWithInstanceHeader(netId);
			this.generatedWrite(writerWithInstanceHeader, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
			base.SendAndLoopbackIfLocal(reliability, transportConnection, writerWithInstanceHeader);
		}

		// Token: 0x0600110A RID: 4362 RVA: 0x0003AE78 File Offset: 0x00039078
		public void Invoke(NetId netId, ENetReliability reliability, List<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
		{
			NetPakWriter writerWithInstanceHeader = base.GetWriterWithInstanceHeader(netId);
			this.generatedWrite(writerWithInstanceHeader, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
			base.SendAndLoopbackIfAnyAreLocal(reliability, transportConnections, writerWithInstanceHeader);
		}

		// Token: 0x0600110B RID: 4363 RVA: 0x0003AEB0 File Offset: 0x000390B0
		[Obsolete("Replaced by List overload")]
		public void Invoke(NetId netId, ENetReliability reliability, IEnumerable<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
		{
			List<ITransportConnection> list = transportConnections as List<ITransportConnection>;
			if (list != null)
			{
				this.Invoke(netId, reliability, list, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
				return;
			}
			throw new ArgumentException("should be list", "transportConnections");
		}

		// Token: 0x0600110C RID: 4364 RVA: 0x0003AEF0 File Offset: 0x000390F0
		public void InvokeAndLoopback(NetId netId, ENetReliability reliability, List<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
		{
			NetPakWriter writerWithInstanceHeader = base.GetWriterWithInstanceHeader(netId);
			this.generatedWrite(writerWithInstanceHeader, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
			base.SendAndLoopback(reliability, transportConnections, writerWithInstanceHeader);
		}

		// Token: 0x0600110D RID: 4365 RVA: 0x0003AF28 File Offset: 0x00039128
		[Obsolete("Replaced by List overload")]
		public void InvokeAndLoopback(NetId netId, ENetReliability reliability, IEnumerable<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
		{
			List<ITransportConnection> list = transportConnections as List<ITransportConnection>;
			if (list != null)
			{
				this.InvokeAndLoopback(netId, reliability, list, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
				return;
			}
			throw new ArgumentException("should be list", "transportConnections");
		}

		// Token: 0x0600110E RID: 4366 RVA: 0x0003AF66 File Offset: 0x00039166
		private ClientInstanceMethod(ClientMethodInfo clientMethodInfo, ClientInstanceMethod<T1, T2, T3, T4, T5, T6, T7>.WriteDelegate generatedWrite) : base(clientMethodInfo)
		{
			this.generatedWrite = generatedWrite;
		}

		// Token: 0x0400055F RID: 1375
		private ClientInstanceMethod<T1, T2, T3, T4, T5, T6, T7>.WriteDelegate generatedWrite;

		// Token: 0x02000890 RID: 2192
		// (Invoke) Token: 0x0600488E RID: 18574
		private delegate void WriteDelegate(NetPakWriter writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);
	}
}
