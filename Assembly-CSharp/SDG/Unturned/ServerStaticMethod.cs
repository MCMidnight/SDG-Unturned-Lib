using System;
using SDG.NetPak;
using SDG.NetTransport;

namespace SDG.Unturned
{
	// Token: 0x0200025A RID: 602
	public sealed class ServerStaticMethod : ServerMethodHandle
	{
		// Token: 0x0600122F RID: 4655 RVA: 0x0003E844 File Offset: 0x0003CA44
		public static ServerStaticMethod Get(ServerStaticMethod.ReceiveDelegate action)
		{
			return ServerStaticMethod.Get(action.Method.DeclaringType, action.Method.Name);
		}

		// Token: 0x06001230 RID: 4656 RVA: 0x0003E861 File Offset: 0x0003CA61
		public static ServerStaticMethod Get(ServerStaticMethod.ReceiveDelegateWithContext action)
		{
			return ServerStaticMethod.Get(action.Method.DeclaringType, action.Method.Name);
		}

		// Token: 0x06001231 RID: 4657 RVA: 0x0003E880 File Offset: 0x0003CA80
		public static ServerStaticMethod Get(Type declaringType, string methodName)
		{
			ServerMethodInfo serverMethodInfo = NetReflection.GetServerMethodInfo(declaringType, methodName);
			if (serverMethodInfo != null)
			{
				return new ServerStaticMethod(serverMethodInfo);
			}
			return null;
		}

		// Token: 0x06001232 RID: 4658 RVA: 0x0003E8A0 File Offset: 0x0003CAA0
		public void Invoke(ENetReliability reliability)
		{
			NetPakWriter writerWithStaticHeader = base.GetWriterWithStaticHeader();
			base.SendAndLoopbackIfLocal(reliability, writerWithStaticHeader);
		}

		// Token: 0x06001233 RID: 4659 RVA: 0x0003E8BC File Offset: 0x0003CABC
		public void Invoke(ENetReliability reliability, Action<NetPakWriter> callback)
		{
			NetPakWriter writerWithStaticHeader = base.GetWriterWithStaticHeader();
			callback.Invoke(writerWithStaticHeader);
			base.SendAndLoopbackIfLocal(reliability, writerWithStaticHeader);
		}

		// Token: 0x06001234 RID: 4660 RVA: 0x0003E8DF File Offset: 0x0003CADF
		private ServerStaticMethod(ServerMethodInfo serverMethodInfo) : base(serverMethodInfo)
		{
		}

		// Token: 0x020008E2 RID: 2274
		// (Invoke) Token: 0x060049AC RID: 18860
		public delegate void ReceiveDelegate();

		// Token: 0x020008E3 RID: 2275
		// (Invoke) Token: 0x060049B0 RID: 18864
		public delegate void ReceiveDelegateWithContext(in ServerInvocationContext context);
	}
}
