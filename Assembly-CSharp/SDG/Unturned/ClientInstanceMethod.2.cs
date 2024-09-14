using System;
using System.Collections.Generic;
using SDG.NetPak;
using SDG.NetTransport;

namespace SDG.Unturned
{
	// Token: 0x02000221 RID: 545
	public sealed class ClientInstanceMethod<T> : ClientInstanceMethodBase
	{
		// Token: 0x060010DE RID: 4318 RVA: 0x0003A6A3 File Offset: 0x000388A3
		public static ClientInstanceMethod<T> Get(Type declaringType, string methodName)
		{
			return ClientMethodHandle.GetInternal<ClientInstanceMethod<T>, ClientInstanceMethod<T>.WriteDelegate>(declaringType, methodName, (ClientMethodInfo clientMethodInfo, ClientInstanceMethod<T>.WriteDelegate generatedWrite) => new ClientInstanceMethod<T>(clientMethodInfo, generatedWrite));
		}

		// Token: 0x060010DF RID: 4319 RVA: 0x0003A6CC File Offset: 0x000388CC
		public void Invoke(NetId netId, ENetReliability reliability, ITransportConnection transportConnection, T arg)
		{
			NetPakWriter writerWithInstanceHeader = base.GetWriterWithInstanceHeader(netId);
			this.generatedWrite(writerWithInstanceHeader, arg);
			base.SendAndLoopbackIfLocal(reliability, transportConnection, writerWithInstanceHeader);
		}

		// Token: 0x060010E0 RID: 4320 RVA: 0x0003A6F8 File Offset: 0x000388F8
		public void Invoke(NetId netId, ENetReliability reliability, List<ITransportConnection> transportConnections, T arg)
		{
			NetPakWriter writerWithInstanceHeader = base.GetWriterWithInstanceHeader(netId);
			this.generatedWrite(writerWithInstanceHeader, arg);
			base.SendAndLoopbackIfAnyAreLocal(reliability, transportConnections, writerWithInstanceHeader);
		}

		// Token: 0x060010E1 RID: 4321 RVA: 0x0003A724 File Offset: 0x00038924
		[Obsolete("Replaced by List overload")]
		public void Invoke(NetId netId, ENetReliability reliability, IEnumerable<ITransportConnection> transportConnections, T arg)
		{
			List<ITransportConnection> list = transportConnections as List<ITransportConnection>;
			if (list != null)
			{
				this.Invoke(netId, reliability, list, arg);
				return;
			}
			throw new ArgumentException("should be list", "transportConnections");
		}

		// Token: 0x060010E2 RID: 4322 RVA: 0x0003A758 File Offset: 0x00038958
		public void InvokeAndLoopback(NetId netId, ENetReliability reliability, List<ITransportConnection> transportConnections, T arg)
		{
			NetPakWriter writerWithInstanceHeader = base.GetWriterWithInstanceHeader(netId);
			this.generatedWrite(writerWithInstanceHeader, arg);
			base.SendAndLoopback(reliability, transportConnections, writerWithInstanceHeader);
		}

		// Token: 0x060010E3 RID: 4323 RVA: 0x0003A784 File Offset: 0x00038984
		[Obsolete("Replaced by List overload")]
		public void InvokeAndLoopback(NetId netId, ENetReliability reliability, IEnumerable<ITransportConnection> transportConnections, T arg)
		{
			List<ITransportConnection> list = transportConnections as List<ITransportConnection>;
			if (list != null)
			{
				this.InvokeAndLoopback(netId, reliability, list, arg);
				return;
			}
			throw new ArgumentException("should be list", "transportConnections");
		}

		// Token: 0x060010E4 RID: 4324 RVA: 0x0003A7B6 File Offset: 0x000389B6
		private ClientInstanceMethod(ClientMethodInfo clientMethodInfo, ClientInstanceMethod<T>.WriteDelegate generatedWrite) : base(clientMethodInfo)
		{
			this.generatedWrite = generatedWrite;
		}

		// Token: 0x04000559 RID: 1369
		private ClientInstanceMethod<T>.WriteDelegate generatedWrite;

		// Token: 0x02000884 RID: 2180
		// (Invoke) Token: 0x06004864 RID: 18532
		private delegate void WriteDelegate(NetPakWriter writer, T arg);
	}
}
