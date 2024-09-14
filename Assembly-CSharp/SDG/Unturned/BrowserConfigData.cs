using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SDG.Unturned
{
	// Token: 0x020006D8 RID: 1752
	public class BrowserConfigData
	{
		// Token: 0x06003AED RID: 15085 RVA: 0x001130A4 File Offset: 0x001112A4
		public BrowserConfigData()
		{
			this.Icon = string.Empty;
			this.Thumbnail = string.Empty;
			this.Desc_Hint = string.Empty;
			this.Desc_Full = string.Empty;
			this.Desc_Server_List = string.Empty;
			this.Login_Token = string.Empty;
			this.BookmarkHost = string.Empty;
			this.Monetization = EServerMonetizationTag.Unspecified;
			this.Links = null;
		}

		// Token: 0x040023AD RID: 9133
		public string Icon;

		// Token: 0x040023AE RID: 9134
		public string Thumbnail;

		// Token: 0x040023AF RID: 9135
		public string Desc_Hint;

		// Token: 0x040023B0 RID: 9136
		public string Desc_Full;

		// Token: 0x040023B1 RID: 9137
		public string Desc_Server_List;

		/// <summary>
		/// https://steamcommunity.com/dev/managegameservers
		/// </summary>
		// Token: 0x040023B2 RID: 9138
		public string Login_Token;

		/// <summary>
		/// IP address, DNS name, or a web address (to perform GET request) to advertise.
		///
		/// Servers not using Fake IP can specify just a DNS entry. This way if server's IP changes clients can rejoin.
		/// For example, if you own the "example.com" domain you could add an A record "myunturnedserver" pointing at
		/// your game server IP and set that record here "myunturnedserver.example.com".
		///
		/// Servers using Fake IP are assigned random ports at startup, but can implement a web API endpoint to return
		/// the IP and port. Clients perform a GET request if this string starts with http:// or https://. The returned
		/// text can be an IP address or DNS name with optional query port override. (e.g., "127.0.0.1:27015")
		/// </summary>
		// Token: 0x040023B3 RID: 9139
		public string BookmarkHost;

		// Token: 0x040023B4 RID: 9140
		[JsonConverter(typeof(StringEnumConverter))]
		public EServerMonetizationTag Monetization;

		// Token: 0x040023B5 RID: 9141
		public BrowserConfigData.Link[] Links;

		// Token: 0x020009ED RID: 2541
		public struct Link
		{
			// Token: 0x040034B6 RID: 13494
			public string Message;

			// Token: 0x040034B7 RID: 13495
			public string Url;
		}
	}
}
