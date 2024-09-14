using System;
using System.Net;

namespace SDG.NetTransport.Loopback
{
	/// <summary>
	/// Dummy connection used in singleplayer.
	/// </summary>
	// Token: 0x02000075 RID: 117
	public struct TransportConnection_Loopback : ITransportConnection, IEquatable<ITransportConnection>
	{
		// Token: 0x060002C7 RID: 711 RVA: 0x0000B90A File Offset: 0x00009B0A
		public static TransportConnection_Loopback Create()
		{
			return new TransportConnection_Loopback(++TransportConnection_Loopback.counter);
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x0000B91E File Offset: 0x00009B1E
		public bool TryGetIPv4Address(out uint address)
		{
			address = 0U;
			return false;
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x0000B924 File Offset: 0x00009B24
		public bool TryGetPort(out ushort port)
		{
			port = 0;
			return false;
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0000B92A File Offset: 0x00009B2A
		public IPAddress GetAddress()
		{
			return null;
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0000B92D File Offset: 0x00009B2D
		public string GetAddressString(bool withPort)
		{
			return null;
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000B930 File Offset: 0x00009B30
		public void CloseConnection()
		{
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0000B932 File Offset: 0x00009B32
		public void Send(byte[] buffer, long size, ENetReliability reliability)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0000B939 File Offset: 0x00009B39
		public override bool Equals(object obj)
		{
			return obj is TransportConnection_Loopback && this.id == ((TransportConnection_Loopback)obj).id;
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0000B958 File Offset: 0x00009B58
		public bool Equals(TransportConnection_Loopback other)
		{
			return this.id == other.id;
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000B968 File Offset: 0x00009B68
		public bool Equals(ITransportConnection other)
		{
			return other is TransportConnection_Loopback && this.id == ((TransportConnection_Loopback)other).id;
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0000B987 File Offset: 0x00009B87
		public override int GetHashCode()
		{
			return this.id.GetHashCode();
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x0000B994 File Offset: 0x00009B94
		public override string ToString()
		{
			if (this == TransportConnection_Loopback.DedicatedServerLoopback)
			{
				return "DedicatedServerLoopback";
			}
			return string.Format("Loopback_{0}", this.id.ToString());
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x0000B9C3 File Offset: 0x00009BC3
		public static bool operator ==(TransportConnection_Loopback lhs, TransportConnection_Loopback rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0000B9CD File Offset: 0x00009BCD
		public static bool operator !=(TransportConnection_Loopback lhs, TransportConnection_Loopback rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0000B9D9 File Offset: 0x00009BD9
		private TransportConnection_Loopback(int id)
		{
			this.id = id;
		}

		// Token: 0x04000140 RID: 320
		public static readonly ITransportConnection DedicatedServer = TransportConnection_Loopback.DedicatedServerLoopback;

		// Token: 0x04000141 RID: 321
		private int id;

		// Token: 0x04000142 RID: 322
		private static int counter;

		// Token: 0x04000143 RID: 323
		private static readonly TransportConnection_Loopback DedicatedServerLoopback = TransportConnection_Loopback.Create();
	}
}
