using System;
using System.Collections.Generic;
using SDG.NetPak;
using SDG.NetTransport;

namespace SDG.Unturned
{
	// Token: 0x0200023B RID: 571
	public sealed class ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> : ClientMethodHandle
	{
		// Token: 0x060011AF RID: 4527 RVA: 0x0003CC70 File Offset: 0x0003AE70
		public static ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> Get(ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.ReceiveDelegate action)
		{
			return ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.Get(action.Method.DeclaringType, action.Method.Name);
		}

		// Token: 0x060011B0 RID: 4528 RVA: 0x0003CC8D File Offset: 0x0003AE8D
		public static ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> Get(ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.ReceiveDelegateWithContext action)
		{
			return ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.Get(action.Method.DeclaringType, action.Method.Name);
		}

		// Token: 0x060011B1 RID: 4529 RVA: 0x0003CCAA File Offset: 0x0003AEAA
		public static ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> Get(Type declaringType, string methodName)
		{
			return ClientMethodHandle.GetInternal<ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.WriteDelegate>(declaringType, methodName, (ClientMethodInfo clientMethodInfo, ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.WriteDelegate generatedWrite) => new ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(clientMethodInfo, generatedWrite));
		}

		// Token: 0x060011B2 RID: 4530 RVA: 0x0003CCD4 File Offset: 0x0003AED4
		public void Invoke(ENetReliability reliability, ITransportConnection transportConnection, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)
		{
			NetPakWriter writerWithStaticHeader = base.GetWriterWithStaticHeader();
			this.generatedWrite(writerWithStaticHeader, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
			base.SendAndLoopbackIfLocal(reliability, transportConnection, writerWithStaticHeader);
		}

		// Token: 0x060011B3 RID: 4531 RVA: 0x0003CD14 File Offset: 0x0003AF14
		public void Invoke(ENetReliability reliability, List<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)
		{
			NetPakWriter writerWithStaticHeader = base.GetWriterWithStaticHeader();
			this.generatedWrite(writerWithStaticHeader, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
			base.SendAndLoopbackIfAnyAreLocal(reliability, transportConnections, writerWithStaticHeader);
		}

		// Token: 0x060011B4 RID: 4532 RVA: 0x0003CD54 File Offset: 0x0003AF54
		[Obsolete("Replaced by List overload")]
		public void Invoke(ENetReliability reliability, IEnumerable<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)
		{
			List<ITransportConnection> list = transportConnections as List<ITransportConnection>;
			if (list != null)
			{
				this.Invoke(reliability, list, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
				return;
			}
			throw new ArgumentException("should be list", "transportConnections");
		}

		// Token: 0x060011B5 RID: 4533 RVA: 0x0003CD9C File Offset: 0x0003AF9C
		public void InvokeAndLoopback(ENetReliability reliability, List<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)
		{
			NetPakWriter writerWithStaticHeader = base.GetWriterWithStaticHeader();
			this.generatedWrite(writerWithStaticHeader, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
			base.SendAndLoopback(reliability, transportConnections, writerWithStaticHeader);
		}

		// Token: 0x060011B6 RID: 4534 RVA: 0x0003CDDC File Offset: 0x0003AFDC
		[Obsolete("Replaced by List overload")]
		public void InvokeAndLoopback(ENetReliability reliability, IEnumerable<ITransportConnection> transportConnections, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)
		{
			List<ITransportConnection> list = transportConnections as List<ITransportConnection>;
			if (list != null)
			{
				this.InvokeAndLoopback(reliability, list, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
				return;
			}
			throw new ArgumentException("should be list", "transportConnections");
		}

		// Token: 0x060011B7 RID: 4535 RVA: 0x0003CE22 File Offset: 0x0003B022
		private ClientStaticMethod(ClientMethodInfo clientMethodInfo, ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.WriteDelegate generatedWrite) : base(clientMethodInfo)
		{
			this.generatedWrite = generatedWrite;
		}

		// Token: 0x04000574 RID: 1396
		private ClientStaticMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.WriteDelegate generatedWrite;

		// Token: 0x020008CB RID: 2251
		// (Invoke) Token: 0x06004965 RID: 18789
		public delegate void ReceiveDelegate(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12);

		// Token: 0x020008CC RID: 2252
		// (Invoke) Token: 0x06004969 RID: 18793
		public delegate void ReceiveDelegateWithContext(in ClientInvocationContext context, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12);

		// Token: 0x020008CD RID: 2253
		// (Invoke) Token: 0x0600496D RID: 18797
		private delegate void WriteDelegate(NetPakWriter writer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12);
	}
}
