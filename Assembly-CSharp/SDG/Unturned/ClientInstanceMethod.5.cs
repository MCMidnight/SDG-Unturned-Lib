using System;
using System.Collections.Generic;
using SDG.NetPak;
using SDG.NetTransport;

namespace SDG.Unturned
{
	// Token: 0x02000224 RID: 548
	public sealed class ClientInstanceMethod<T1, T2, T3, T4> : ClientInstanceMethodBase
	{
		// Token: 0x060010F3 RID: 4339 RVA: 0x0003AA2E File Offset: 0x00038C2E
		public static ClientInstanceMethod<T1, T2, T3, T4> Get(Type declaringType, string methodName)
		{
			return ClientMethodHandle.GetInternal<ClientInstanceMethod<T1, T2, T3, T4>, ClientInstanceMethod<T1, T2, T3, T4>.WriteDelegate>(declaringType, methodName, (ClientMethodInfo clientMethodInfo, ClientInstanceMethod<T1, T2, T3, T4>.WriteDelegate generatedWrite) => new ClientInstanceMethod<T1, T2, T3, T4>(clientMethodInfo, generatedWrite));
		}

		// Token: 0x060010F4 RID: 4340 RVA: 0x0003AA58 File Offset: 0x00038C58
		public void Invoke(NetId netId, ENetReliability reliability, ITransportConnection transportConnection, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
		{
			NetPakWriter writerWithInstanceHeader = base.GetWriterWithInstanceHeader(netId);
			this.generatedWrite(writerWithInstanceHeader, arg1, arg2, arg3, arg4);
			base.SendAndLoopbackIfLocal(reliability, transportConnection, writerWithInstanceHeader);
		}

		// Token: 0x060010F5 RID: 4341 RVA: 0x0003AA8C File Offset: 0x00038C8C
		public void Invoke(NetId netId, ENetReliability reliability, List<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
		{
			NetPakWriter writerWithInstanceHeader = base.GetWriterWithInstanceHeader(netId);
			this.generatedWrite(writerWithInstanceHeader, arg1, arg2, arg3, arg4);
			base.SendAndLoopbackIfAnyAreLocal(reliability, transportConnections, writerWithInstanceHeader);
		}

		// Token: 0x060010F6 RID: 4342 RVA: 0x0003AAC0 File Offset: 0x00038CC0
		[Obsolete("Replaced by List overload")]
		public void Invoke(NetId netId, ENetReliability reliability, IEnumerable<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
		{
			List<ITransportConnection> list = transportConnections as List<ITransportConnection>;
			if (list != null)
			{
				this.Invoke(netId, reliability, list, arg1, arg2, arg3, arg4);
				return;
			}
			throw new ArgumentException("should be list", "transportConnections");
		}

		// Token: 0x060010F7 RID: 4343 RVA: 0x0003AAF8 File Offset: 0x00038CF8
		public void InvokeAndLoopback(NetId netId, ENetReliability reliability, List<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
		{
			NetPakWriter writerWithInstanceHeader = base.GetWriterWithInstanceHeader(netId);
			this.generatedWrite(writerWithInstanceHeader, arg1, arg2, arg3, arg4);
			base.SendAndLoopback(reliability, transportConnections, writerWithInstanceHeader);
		}

		// Token: 0x060010F8 RID: 4344 RVA: 0x0003AB2C File Offset: 0x00038D2C
		[Obsolete("Replaced by List overload")]
		public void InvokeAndLoopback(NetId netId, ENetReliability reliability, IEnumerable<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
		{
			List<ITransportConnection> list = transportConnections as List<ITransportConnection>;
			if (list != null)
			{
				this.InvokeAndLoopback(netId, reliability, list, arg1, arg2, arg3, arg4);
				return;
			}
			throw new ArgumentException("should be list", "transportConnections");
		}

		// Token: 0x060010F9 RID: 4345 RVA: 0x0003AB64 File Offset: 0x00038D64
		private ClientInstanceMethod(ClientMethodInfo clientMethodInfo, ClientInstanceMethod<T1, T2, T3, T4>.WriteDelegate generatedWrite) : base(clientMethodInfo)
		{
			this.generatedWrite = generatedWrite;
		}

		// Token: 0x0400055C RID: 1372
		private ClientInstanceMethod<T1, T2, T3, T4>.WriteDelegate generatedWrite;

		// Token: 0x0200088A RID: 2186
		// (Invoke) Token: 0x06004879 RID: 18553
		private delegate void WriteDelegate(NetPakWriter writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4);
	}
}
