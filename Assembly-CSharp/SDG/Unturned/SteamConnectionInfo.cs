using System;

namespace SDG.Unturned
{
	// Token: 0x0200068D RID: 1677
	public class SteamConnectionInfo
	{
		// Token: 0x17000A0A RID: 2570
		// (get) Token: 0x06003880 RID: 14464 RVA: 0x0010B5A8 File Offset: 0x001097A8
		public uint ip
		{
			get
			{
				return this._ip;
			}
		}

		// Token: 0x17000A0B RID: 2571
		// (get) Token: 0x06003881 RID: 14465 RVA: 0x0010B5B0 File Offset: 0x001097B0
		public ushort port
		{
			get
			{
				return this._port;
			}
		}

		// Token: 0x17000A0C RID: 2572
		// (get) Token: 0x06003882 RID: 14466 RVA: 0x0010B5B8 File Offset: 0x001097B8
		public string password
		{
			get
			{
				return this._password;
			}
		}

		// Token: 0x06003883 RID: 14467 RVA: 0x0010B5C0 File Offset: 0x001097C0
		public SteamConnectionInfo(uint newIP, ushort newPort, string newPassword)
		{
			this._ip = newIP;
			this._port = newPort;
			this._password = newPassword;
		}

		// Token: 0x06003884 RID: 14468 RVA: 0x0010B5DD File Offset: 0x001097DD
		public SteamConnectionInfo(string newIP, ushort newPort, string newPassword)
		{
			this._ip = Parser.getUInt32FromIP(newIP);
			this._port = newPort;
			this._password = newPassword;
		}

		// Token: 0x06003885 RID: 14469 RVA: 0x0010B600 File Offset: 0x00109800
		public SteamConnectionInfo(string newIPPort, string newPassword)
		{
			string[] componentsFromSerial = Parser.getComponentsFromSerial(newIPPort, ':');
			this._ip = Parser.getUInt32FromIP(componentsFromSerial[0]);
			this._port = ushort.Parse(componentsFromSerial[1]);
			this._password = newPassword;
		}

		// Token: 0x0400217C RID: 8572
		public uint _ip;

		// Token: 0x0400217D RID: 8573
		public ushort _port;

		// Token: 0x0400217E RID: 8574
		public string _password;
	}
}
