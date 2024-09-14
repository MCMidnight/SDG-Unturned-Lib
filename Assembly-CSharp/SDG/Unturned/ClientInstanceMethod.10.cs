using System;
using System.Collections.Generic;
using SDG.NetPak;
using SDG.NetTransport;

namespace SDG.Unturned
{
	// Token: 0x02000229 RID: 553
	public sealed class ClientInstanceMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9> : ClientInstanceMethodBase
	{
		// Token: 0x06001116 RID: 4374 RVA: 0x0003B0E4 File Offset: 0x000392E4
		public static ClientInstanceMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9> Get(Type declaringType, string methodName)
		{
			return ClientMethodHandle.GetInternal<ClientInstanceMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9>, ClientInstanceMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9>.WriteDelegate>(declaringType, methodName, (ClientMethodInfo clientMethodInfo, ClientInstanceMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9>.WriteDelegate generatedWrite) => new ClientInstanceMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9>(clientMethodInfo, generatedWrite));
		}

		// Token: 0x06001117 RID: 4375 RVA: 0x0003B10C File Offset: 0x0003930C
		public void Invoke(NetId netId, ENetReliability reliability, ITransportConnection transportConnection, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
		{
			NetPakWriter writerWithInstanceHeader = base.GetWriterWithInstanceHeader(netId);
			this.generatedWrite(writerWithInstanceHeader, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
			base.SendAndLoopbackIfLocal(reliability, transportConnection, writerWithInstanceHeader);
		}

		// Token: 0x06001118 RID: 4376 RVA: 0x0003B148 File Offset: 0x00039348
		public void Invoke(NetId netId, ENetReliability reliability, List<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
		{
			NetPakWriter writerWithInstanceHeader = base.GetWriterWithInstanceHeader(netId);
			this.generatedWrite(writerWithInstanceHeader, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
			base.SendAndLoopbackIfAnyAreLocal(reliability, transportConnections, writerWithInstanceHeader);
		}

		// Token: 0x06001119 RID: 4377 RVA: 0x0003B184 File Offset: 0x00039384
		[Obsolete("Replaced by List overload")]
		public void Invoke(NetId netId, ENetReliability reliability, IEnumerable<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
		{
			List<ITransportConnection> list = transportConnections as List<ITransportConnection>;
			if (list != null)
			{
				this.Invoke(netId, reliability, list, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
				return;
			}
			throw new ArgumentException("should be list", "transportConnections");
		}

		// Token: 0x0600111A RID: 4378 RVA: 0x0003B1C8 File Offset: 0x000393C8
		public void InvokeAndLoopback(NetId netId, ENetReliability reliability, List<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
		{
			NetPakWriter writerWithInstanceHeader = base.GetWriterWithInstanceHeader(netId);
			this.generatedWrite(writerWithInstanceHeader, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
			base.SendAndLoopback(reliability, transportConnections, writerWithInstanceHeader);
		}

		// Token: 0x0600111B RID: 4379 RVA: 0x0003B204 File Offset: 0x00039404
		[Obsolete("Replaced by List overload")]
		public void InvokeAndLoopback(NetId netId, ENetReliability reliability, IEnumerable<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
		{
			List<ITransportConnection> list = transportConnections as List<ITransportConnection>;
			if (list != null)
			{
				this.InvokeAndLoopback(netId, reliability, list, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
				return;
			}
			throw new ArgumentException("should be list", "transportConnections");
		}

		// Token: 0x0600111C RID: 4380 RVA: 0x0003B246 File Offset: 0x00039446
		private ClientInstanceMethod(ClientMethodInfo clientMethodInfo, ClientInstanceMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9>.WriteDelegate generatedWrite) : base(clientMethodInfo)
		{
			this.generatedWrite = generatedWrite;
		}

		// Token: 0x04000561 RID: 1377
		private ClientInstanceMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9>.WriteDelegate generatedWrite;

		// Token: 0x02000894 RID: 2196
		// (Invoke) Token: 0x0600489C RID: 18588
		private delegate void WriteDelegate(NetPakWriter writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9);
	}
}
