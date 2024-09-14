using System;
using System.Collections.Generic;
using SDG.NetPak;
using SDG.NetTransport;

namespace SDG.Unturned
{
	// Token: 0x02000220 RID: 544
	public sealed class ClientInstanceMethod : ClientInstanceMethodBase
	{
		// Token: 0x060010D2 RID: 4306 RVA: 0x0003A4DC File Offset: 0x000386DC
		public static ClientInstanceMethod Get(Type declaringType, string methodName)
		{
			ClientMethodInfo clientMethodInfo = NetReflection.GetClientMethodInfo(declaringType, methodName);
			if (clientMethodInfo != null)
			{
				return new ClientInstanceMethod(clientMethodInfo);
			}
			return null;
		}

		// Token: 0x060010D3 RID: 4307 RVA: 0x0003A4FC File Offset: 0x000386FC
		public void Invoke(NetId netId, ENetReliability reliability, ITransportConnection transportConnection)
		{
			NetPakWriter writerWithInstanceHeader = base.GetWriterWithInstanceHeader(netId);
			base.SendAndLoopbackIfLocal(reliability, transportConnection, writerWithInstanceHeader);
		}

		// Token: 0x060010D4 RID: 4308 RVA: 0x0003A51C File Offset: 0x0003871C
		public void Invoke(NetId netId, ENetReliability reliability, ITransportConnection transportConnection, Action<NetPakWriter> callback)
		{
			NetPakWriter writerWithInstanceHeader = base.GetWriterWithInstanceHeader(netId);
			callback.Invoke(writerWithInstanceHeader);
			base.SendAndLoopbackIfLocal(reliability, transportConnection, writerWithInstanceHeader);
		}

		// Token: 0x060010D5 RID: 4309 RVA: 0x0003A544 File Offset: 0x00038744
		public void Invoke(NetId netId, ENetReliability reliability, List<ITransportConnection> transportConnections)
		{
			NetPakWriter writerWithInstanceHeader = base.GetWriterWithInstanceHeader(netId);
			base.SendAndLoopbackIfAnyAreLocal(reliability, transportConnections, writerWithInstanceHeader);
		}

		// Token: 0x060010D6 RID: 4310 RVA: 0x0003A564 File Offset: 0x00038764
		[Obsolete("Replaced by List overload")]
		public void Invoke(NetId netId, ENetReliability reliability, IEnumerable<ITransportConnection> transportConnections)
		{
			List<ITransportConnection> list = transportConnections as List<ITransportConnection>;
			if (list != null)
			{
				this.Invoke(netId, reliability, list);
				return;
			}
			throw new ArgumentException("should be list", "transportConnections");
		}

		// Token: 0x060010D7 RID: 4311 RVA: 0x0003A594 File Offset: 0x00038794
		public void Invoke(NetId netId, ENetReliability reliability, List<ITransportConnection> transportConnections, Action<NetPakWriter> callback)
		{
			NetPakWriter writerWithInstanceHeader = base.GetWriterWithInstanceHeader(netId);
			callback.Invoke(writerWithInstanceHeader);
			base.SendAndLoopbackIfAnyAreLocal(reliability, transportConnections, writerWithInstanceHeader);
		}

		// Token: 0x060010D8 RID: 4312 RVA: 0x0003A5BC File Offset: 0x000387BC
		[Obsolete("Replaced by List overload")]
		public void Invoke(NetId netId, ENetReliability reliability, IEnumerable<ITransportConnection> transportConnections, Action<NetPakWriter> callback)
		{
			List<ITransportConnection> list = transportConnections as List<ITransportConnection>;
			if (list != null)
			{
				this.Invoke(netId, reliability, list, callback);
				return;
			}
			throw new ArgumentException("should be list", "transportConnections");
		}

		// Token: 0x060010D9 RID: 4313 RVA: 0x0003A5F0 File Offset: 0x000387F0
		public void InvokeAndLoopback(NetId netId, ENetReliability reliability, List<ITransportConnection> transportConnections)
		{
			NetPakWriter writerWithInstanceHeader = base.GetWriterWithInstanceHeader(netId);
			base.SendAndLoopback(reliability, transportConnections, writerWithInstanceHeader);
		}

		// Token: 0x060010DA RID: 4314 RVA: 0x0003A610 File Offset: 0x00038810
		[Obsolete("Replaced by List overload")]
		public void InvokeAndLoopback(NetId netId, ENetReliability reliability, IEnumerable<ITransportConnection> transportConnections)
		{
			List<ITransportConnection> list = transportConnections as List<ITransportConnection>;
			if (list != null)
			{
				this.InvokeAndLoopback(netId, reliability, list);
				return;
			}
			throw new ArgumentException("should be list", "transportConnections");
		}

		// Token: 0x060010DB RID: 4315 RVA: 0x0003A640 File Offset: 0x00038840
		public void InvokeAndLoopback(NetId netId, ENetReliability reliability, List<ITransportConnection> transportConnections, Action<NetPakWriter> callback)
		{
			NetPakWriter writerWithInstanceHeader = base.GetWriterWithInstanceHeader(netId);
			callback.Invoke(writerWithInstanceHeader);
			base.SendAndLoopback(reliability, transportConnections, writerWithInstanceHeader);
		}

		// Token: 0x060010DC RID: 4316 RVA: 0x0003A668 File Offset: 0x00038868
		[Obsolete("Replaced by List overload")]
		public void InvokeAndLoopback(NetId netId, ENetReliability reliability, IEnumerable<ITransportConnection> transportConnections, Action<NetPakWriter> callback)
		{
			List<ITransportConnection> list = transportConnections as List<ITransportConnection>;
			if (list != null)
			{
				this.InvokeAndLoopback(netId, reliability, list, callback);
				return;
			}
			throw new ArgumentException("should be list", "transportConnections");
		}

		// Token: 0x060010DD RID: 4317 RVA: 0x0003A69A File Offset: 0x0003889A
		private ClientInstanceMethod(ClientMethodInfo clientMethodInfo) : base(clientMethodInfo)
		{
		}
	}
}
