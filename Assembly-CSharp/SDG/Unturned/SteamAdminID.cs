using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x02000682 RID: 1666
	public class SteamAdminID
	{
		// Token: 0x170009FB RID: 2555
		// (get) Token: 0x06003813 RID: 14355 RVA: 0x001094F6 File Offset: 0x001076F6
		public CSteamID playerID
		{
			get
			{
				return this._playerID;
			}
		}

		// Token: 0x06003814 RID: 14356 RVA: 0x001094FE File Offset: 0x001076FE
		public SteamAdminID(CSteamID newPlayerID, CSteamID newJudgeID)
		{
			this._playerID = newPlayerID;
			this.judgeID = newJudgeID;
		}

		// Token: 0x04002147 RID: 8519
		private CSteamID _playerID;

		// Token: 0x04002148 RID: 8520
		public CSteamID judgeID;
	}
}
