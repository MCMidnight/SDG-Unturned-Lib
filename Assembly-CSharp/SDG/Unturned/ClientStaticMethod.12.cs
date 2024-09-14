using System;
using System.Collections.Generic;
using SDG.NetPak;
using SDG.NetTransport;

namespace SDG.Unturned
{
	// Token: 0x0200023A RID: 570
	public sealed class ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : ClientMethodHandle
	{
		// Token: 0x060011A6 RID: 4518 RVA: 0x0003CAB6 File Offset: 0x0003ACB6
		public static ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> Get(ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.ReceiveDelegate action)
		{
			return ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.Get(action.Method.DeclaringType, action.Method.Name);
		}

		// Token: 0x060011A7 RID: 4519 RVA: 0x0003CAD3 File Offset: 0x0003ACD3
		public static ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> Get(ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.ReceiveDelegateWithContext action)
		{
			return ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.Get(action.Method.DeclaringType, action.Method.Name);
		}

		// Token: 0x060011A8 RID: 4520 RVA: 0x0003CAF0 File Offset: 0x0003ACF0
		public static ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> Get(Type declaringType, string methodName)
		{
			return ClientMethodHandle.GetInternal<ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.WriteDelegate>(declaringType, methodName, (ClientMethodInfo clientMethodInfo, ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.WriteDelegate generatedWrite) => new ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(clientMethodInfo, generatedWrite));
		}

		// Token: 0x060011A9 RID: 4521 RVA: 0x0003CB18 File Offset: 0x0003AD18
		public void Invoke(ENetReliability reliability, ITransportConnection transportConnection, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
		{
			NetPakWriter writerWithStaticHeader = base.GetWriterWithStaticHeader();
			this.generatedWrite(writerWithStaticHeader, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
			base.SendAndLoopbackIfLocal(reliability, transportConnection, writerWithStaticHeader);
		}

		// Token: 0x060011AA RID: 4522 RVA: 0x0003CB58 File Offset: 0x0003AD58
		public void Invoke(ENetReliability reliability, List<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
		{
			NetPakWriter writerWithStaticHeader = base.GetWriterWithStaticHeader();
			this.generatedWrite(writerWithStaticHeader, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
			base.SendAndLoopbackIfAnyAreLocal(reliability, transportConnections, writerWithStaticHeader);
		}

		// Token: 0x060011AB RID: 4523 RVA: 0x0003CB98 File Offset: 0x0003AD98
		[Obsolete("Replaced by List overload")]
		public void Invoke(ENetReliability reliability, IEnumerable<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
		{
			List<ITransportConnection> list = transportConnections as List<ITransportConnection>;
			if (list != null)
			{
				this.Invoke(reliability, list, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
				return;
			}
			throw new ArgumentException("should be list", "transportConnections");
		}

		// Token: 0x060011AC RID: 4524 RVA: 0x0003CBDC File Offset: 0x0003ADDC
		public void InvokeAndLoopback(ENetReliability reliability, List<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
		{
			NetPakWriter writerWithStaticHeader = base.GetWriterWithStaticHeader();
			this.generatedWrite(writerWithStaticHeader, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
			base.SendAndLoopback(reliability, transportConnections, writerWithStaticHeader);
		}

		// Token: 0x060011AD RID: 4525 RVA: 0x0003CC1C File Offset: 0x0003AE1C
		[Obsolete("Replaced by List overload")]
		public void InvokeAndLoopback(ENetReliability reliability, IEnumerable<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
		{
			List<ITransportConnection> list = transportConnections as List<ITransportConnection>;
			if (list != null)
			{
				this.InvokeAndLoopback(reliability, list, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
				return;
			}
			throw new ArgumentException("should be list", "transportConnections");
		}

		// Token: 0x060011AE RID: 4526 RVA: 0x0003CC60 File Offset: 0x0003AE60
		private ClientStaticMethod(ClientMethodInfo clientMethodInfo, ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.WriteDelegate generatedWrite) : base(clientMethodInfo)
		{
			this.generatedWrite = generatedWrite;
		}

		// Token: 0x04000573 RID: 1395
		private ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.WriteDelegate generatedWrite;

		// Token: 0x020008C7 RID: 2247
		// (Invoke) Token: 0x06004956 RID: 18774
		public delegate void ReceiveDelegate(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11);

		// Token: 0x020008C8 RID: 2248
		// (Invoke) Token: 0x0600495A RID: 18778
		public delegate void ReceiveDelegateWithContext(in ClientInvocationContext context, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11);

		// Token: 0x020008C9 RID: 2249
		// (Invoke) Token: 0x0600495E RID: 18782
		private delegate void WriteDelegate(NetPakWriter writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11);
	}
}
