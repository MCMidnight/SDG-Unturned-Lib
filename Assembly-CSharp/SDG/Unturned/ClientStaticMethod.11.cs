using System;
using System.Collections.Generic;
using SDG.NetPak;
using SDG.NetTransport;

namespace SDG.Unturned
{
	// Token: 0x02000239 RID: 569
	public sealed class ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : ClientMethodHandle
	{
		// Token: 0x0600119D RID: 4509 RVA: 0x0003C908 File Offset: 0x0003AB08
		public static ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Get(ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.ReceiveDelegate action)
		{
			return ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.Get(action.Method.DeclaringType, action.Method.Name);
		}

		// Token: 0x0600119E RID: 4510 RVA: 0x0003C925 File Offset: 0x0003AB25
		public static ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Get(ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.ReceiveDelegateWithContext action)
		{
			return ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.Get(action.Method.DeclaringType, action.Method.Name);
		}

		// Token: 0x0600119F RID: 4511 RVA: 0x0003C942 File Offset: 0x0003AB42
		public static ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Get(Type declaringType, string methodName)
		{
			return ClientMethodHandle.GetInternal<ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.WriteDelegate>(declaringType, methodName, (ClientMethodInfo clientMethodInfo, ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.WriteDelegate generatedWrite) => new ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(clientMethodInfo, generatedWrite));
		}

		// Token: 0x060011A0 RID: 4512 RVA: 0x0003C96C File Offset: 0x0003AB6C
		public void Invoke(ENetReliability reliability, ITransportConnection transportConnection, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
		{
			NetPakWriter writerWithStaticHeader = base.GetWriterWithStaticHeader();
			this.generatedWrite(writerWithStaticHeader, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
			base.SendAndLoopbackIfLocal(reliability, transportConnection, writerWithStaticHeader);
		}

		// Token: 0x060011A1 RID: 4513 RVA: 0x0003C9A8 File Offset: 0x0003ABA8
		public void Invoke(ENetReliability reliability, List<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
		{
			NetPakWriter writerWithStaticHeader = base.GetWriterWithStaticHeader();
			this.generatedWrite(writerWithStaticHeader, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
			base.SendAndLoopbackIfAnyAreLocal(reliability, transportConnections, writerWithStaticHeader);
		}

		// Token: 0x060011A2 RID: 4514 RVA: 0x0003C9E4 File Offset: 0x0003ABE4
		[Obsolete("Replaced by List overload")]
		public void Invoke(ENetReliability reliability, IEnumerable<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
		{
			List<ITransportConnection> list = transportConnections as List<ITransportConnection>;
			if (list != null)
			{
				this.Invoke(reliability, list, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
				return;
			}
			throw new ArgumentException("should be list", "transportConnections");
		}

		// Token: 0x060011A3 RID: 4515 RVA: 0x0003CA28 File Offset: 0x0003AC28
		public void InvokeAndLoopback(ENetReliability reliability, List<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
		{
			NetPakWriter writerWithStaticHeader = base.GetWriterWithStaticHeader();
			this.generatedWrite(writerWithStaticHeader, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
			base.SendAndLoopback(reliability, transportConnections, writerWithStaticHeader);
		}

		// Token: 0x060011A4 RID: 4516 RVA: 0x0003CA64 File Offset: 0x0003AC64
		[Obsolete("Replaced by List overload")]
		public void InvokeAndLoopback(ENetReliability reliability, IEnumerable<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
		{
			List<ITransportConnection> list = transportConnections as List<ITransportConnection>;
			if (list != null)
			{
				this.InvokeAndLoopback(reliability, list, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
				return;
			}
			throw new ArgumentException("should be list", "transportConnections");
		}

		// Token: 0x060011A5 RID: 4517 RVA: 0x0003CAA6 File Offset: 0x0003ACA6
		private ClientStaticMethod(ClientMethodInfo clientMethodInfo, ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.WriteDelegate generatedWrite) : base(clientMethodInfo)
		{
			this.generatedWrite = generatedWrite;
		}

		// Token: 0x04000572 RID: 1394
		private ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.WriteDelegate generatedWrite;

		// Token: 0x020008C3 RID: 2243
		// (Invoke) Token: 0x06004947 RID: 18759
		public delegate void ReceiveDelegate(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10);

		// Token: 0x020008C4 RID: 2244
		// (Invoke) Token: 0x0600494B RID: 18763
		public delegate void ReceiveDelegateWithContext(in ClientInvocationContext context, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10);

		// Token: 0x020008C5 RID: 2245
		// (Invoke) Token: 0x0600494F RID: 18767
		private delegate void WriteDelegate(NetPakWriter writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10);
	}
}
