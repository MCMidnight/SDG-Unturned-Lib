using System;
using System.Collections.Generic;
using SDG.NetPak;
using SDG.NetTransport;

namespace SDG.Unturned
{
	// Token: 0x02000226 RID: 550
	public sealed class ClientInstanceMethod<T1, T2, T3, T4, T5, T6> : ClientInstanceMethodBase
	{
		// Token: 0x06001101 RID: 4353 RVA: 0x0003ACBE File Offset: 0x00038EBE
		public static ClientInstanceMethod<T1, T2, T3, T4, T5, T6> Get(Type declaringType, string methodName)
		{
			return ClientMethodHandle.GetInternal<ClientInstanceMethod<T1, T2, T3, T4, T5, T6>, ClientInstanceMethod<T1, T2, T3, T4, T5, T6>.WriteDelegate>(declaringType, methodName, (ClientMethodInfo clientMethodInfo, ClientInstanceMethod<T1, T2, T3, T4, T5, T6>.WriteDelegate generatedWrite) => new ClientInstanceMethod<T1, T2, T3, T4, T5, T6>(clientMethodInfo, generatedWrite));
		}

		// Token: 0x06001102 RID: 4354 RVA: 0x0003ACE8 File Offset: 0x00038EE8
		public void Invoke(NetId netId, ENetReliability reliability, ITransportConnection transportConnection, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
		{
			NetPakWriter writerWithInstanceHeader = base.GetWriterWithInstanceHeader(netId);
			this.generatedWrite(writerWithInstanceHeader, arg1, arg2, arg3, arg4, arg5, arg6);
			base.SendAndLoopbackIfLocal(reliability, transportConnection, writerWithInstanceHeader);
		}

		// Token: 0x06001103 RID: 4355 RVA: 0x0003AD20 File Offset: 0x00038F20
		public void Invoke(NetId netId, ENetReliability reliability, List<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
		{
			NetPakWriter writerWithInstanceHeader = base.GetWriterWithInstanceHeader(netId);
			this.generatedWrite(writerWithInstanceHeader, arg1, arg2, arg3, arg4, arg5, arg6);
			base.SendAndLoopbackIfAnyAreLocal(reliability, transportConnections, writerWithInstanceHeader);
		}

		// Token: 0x06001104 RID: 4356 RVA: 0x0003AD58 File Offset: 0x00038F58
		[Obsolete("Replaced by List overload")]
		public void Invoke(NetId netId, ENetReliability reliability, IEnumerable<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
		{
			List<ITransportConnection> list = transportConnections as List<ITransportConnection>;
			if (list != null)
			{
				this.Invoke(netId, reliability, list, arg1, arg2, arg3, arg4, arg5, arg6);
				return;
			}
			throw new ArgumentException("should be list", "transportConnections");
		}

		// Token: 0x06001105 RID: 4357 RVA: 0x0003AD94 File Offset: 0x00038F94
		public void InvokeAndLoopback(NetId netId, ENetReliability reliability, List<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
		{
			NetPakWriter writerWithInstanceHeader = base.GetWriterWithInstanceHeader(netId);
			this.generatedWrite(writerWithInstanceHeader, arg1, arg2, arg3, arg4, arg5, arg6);
			base.SendAndLoopback(reliability, transportConnections, writerWithInstanceHeader);
		}

		// Token: 0x06001106 RID: 4358 RVA: 0x0003ADCC File Offset: 0x00038FCC
		[Obsolete("Replaced by List overload")]
		public void InvokeAndLoopback(NetId netId, ENetReliability reliability, IEnumerable<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
		{
			List<ITransportConnection> list = transportConnections as List<ITransportConnection>;
			if (list != null)
			{
				this.InvokeAndLoopback(netId, reliability, list, arg1, arg2, arg3, arg4, arg5, arg6);
				return;
			}
			throw new ArgumentException("should be list", "transportConnections");
		}

		// Token: 0x06001107 RID: 4359 RVA: 0x0003AE08 File Offset: 0x00039008
		private ClientInstanceMethod(ClientMethodInfo clientMethodInfo, ClientInstanceMethod<T1, T2, T3, T4, T5, T6>.WriteDelegate generatedWrite) : base(clientMethodInfo)
		{
			this.generatedWrite = generatedWrite;
		}

		// Token: 0x0400055E RID: 1374
		private ClientInstanceMethod<T1, T2, T3, T4, T5, T6>.WriteDelegate generatedWrite;

		// Token: 0x0200088E RID: 2190
		// (Invoke) Token: 0x06004887 RID: 18567
		private delegate void WriteDelegate(NetPakWriter writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);
	}
}
