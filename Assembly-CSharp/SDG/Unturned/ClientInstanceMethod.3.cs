using System;
using System.Collections.Generic;
using SDG.NetPak;
using SDG.NetTransport;

namespace SDG.Unturned
{
	// Token: 0x02000222 RID: 546
	public sealed class ClientInstanceMethod<T1, T2> : ClientInstanceMethodBase
	{
		// Token: 0x060010E5 RID: 4325 RVA: 0x0003A7C6 File Offset: 0x000389C6
		public static ClientInstanceMethod<T1, T2> Get(Type declaringType, string methodName)
		{
			return ClientMethodHandle.GetInternal<ClientInstanceMethod<T1, T2>, ClientInstanceMethod<T1, T2>.WriteDelegate>(declaringType, methodName, (ClientMethodInfo clientMethodInfo, ClientInstanceMethod<T1, T2>.WriteDelegate generatedWrite) => new ClientInstanceMethod<T1, T2>(clientMethodInfo, generatedWrite));
		}

		// Token: 0x060010E6 RID: 4326 RVA: 0x0003A7F0 File Offset: 0x000389F0
		public void Invoke(NetId netId, ENetReliability reliability, ITransportConnection transportConnection, T1 arg1, T2 arg2)
		{
			NetPakWriter writerWithInstanceHeader = base.GetWriterWithInstanceHeader(netId);
			this.generatedWrite(writerWithInstanceHeader, arg1, arg2);
			base.SendAndLoopbackIfLocal(reliability, transportConnection, writerWithInstanceHeader);
		}

		// Token: 0x060010E7 RID: 4327 RVA: 0x0003A820 File Offset: 0x00038A20
		public void Invoke(NetId netId, ENetReliability reliability, List<ITransportConnection> transportConnections, T1 arg1, T2 arg2)
		{
			NetPakWriter writerWithInstanceHeader = base.GetWriterWithInstanceHeader(netId);
			this.generatedWrite(writerWithInstanceHeader, arg1, arg2);
			base.SendAndLoopbackIfAnyAreLocal(reliability, transportConnections, writerWithInstanceHeader);
		}

		// Token: 0x060010E8 RID: 4328 RVA: 0x0003A850 File Offset: 0x00038A50
		[Obsolete("Replaced by List overload")]
		public void Invoke(NetId netId, ENetReliability reliability, IEnumerable<ITransportConnection> transportConnections, T1 arg1, T2 arg2)
		{
			List<ITransportConnection> list = transportConnections as List<ITransportConnection>;
			if (list != null)
			{
				this.Invoke(netId, reliability, list, arg1, arg2);
				return;
			}
			throw new ArgumentException("should be list", "transportConnections");
		}

		// Token: 0x060010E9 RID: 4329 RVA: 0x0003A884 File Offset: 0x00038A84
		public void InvokeAndLoopback(NetId netId, ENetReliability reliability, List<ITransportConnection> transportConnections, T1 arg1, T2 arg2)
		{
			NetPakWriter writerWithInstanceHeader = base.GetWriterWithInstanceHeader(netId);
			this.generatedWrite(writerWithInstanceHeader, arg1, arg2);
			base.SendAndLoopback(reliability, transportConnections, writerWithInstanceHeader);
		}

		// Token: 0x060010EA RID: 4330 RVA: 0x0003A8B4 File Offset: 0x00038AB4
		[Obsolete("Replaced by List overload")]
		public void InvokeAndLoopback(NetId netId, ENetReliability reliability, IEnumerable<ITransportConnection> transportConnections, T1 arg1, T2 arg2)
		{
			List<ITransportConnection> list = transportConnections as List<ITransportConnection>;
			if (list != null)
			{
				this.InvokeAndLoopback(netId, reliability, list, arg1, arg2);
				return;
			}
			throw new ArgumentException("should be list", "transportConnections");
		}

		// Token: 0x060010EB RID: 4331 RVA: 0x0003A8E8 File Offset: 0x00038AE8
		private ClientInstanceMethod(ClientMethodInfo clientMethodInfo, ClientInstanceMethod<T1, T2>.WriteDelegate generatedWrite) : base(clientMethodInfo)
		{
			this.generatedWrite = generatedWrite;
		}

		// Token: 0x0400055A RID: 1370
		private ClientInstanceMethod<T1, T2>.WriteDelegate generatedWrite;

		// Token: 0x02000886 RID: 2182
		// (Invoke) Token: 0x0600486B RID: 18539
		private delegate void WriteDelegate(NetPakWriter writer, T1 arg1, T2 arg2);
	}
}
