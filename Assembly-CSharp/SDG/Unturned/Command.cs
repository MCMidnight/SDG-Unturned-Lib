using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x02000390 RID: 912
	public class Command : IComparable<Command>
	{
		// Token: 0x17000610 RID: 1552
		// (get) Token: 0x06001CB0 RID: 7344 RVA: 0x00065CFB File Offset: 0x00063EFB
		public string command
		{
			get
			{
				return this._command;
			}
		}

		// Token: 0x17000611 RID: 1553
		// (get) Token: 0x06001CB1 RID: 7345 RVA: 0x00065D03 File Offset: 0x00063F03
		public string info
		{
			get
			{
				return this._info;
			}
		}

		// Token: 0x17000612 RID: 1554
		// (get) Token: 0x06001CB2 RID: 7346 RVA: 0x00065D0B File Offset: 0x00063F0B
		public string help
		{
			get
			{
				return this._help;
			}
		}

		// Token: 0x06001CB3 RID: 7347 RVA: 0x00065D13 File Offset: 0x00063F13
		protected virtual void execute(CSteamID executorID, string parameter)
		{
		}

		// Token: 0x06001CB4 RID: 7348 RVA: 0x00065D15 File Offset: 0x00063F15
		public virtual bool check(CSteamID executorID, string method, string parameter)
		{
			if (method.ToLower() == this.command.ToLower())
			{
				this.execute(executorID, parameter);
				return true;
			}
			return false;
		}

		// Token: 0x06001CB5 RID: 7349 RVA: 0x00065D3A File Offset: 0x00063F3A
		public int CompareTo(Command other)
		{
			return this.command.CompareTo(other.command);
		}

		// Token: 0x04000DBA RID: 3514
		protected Local localization;

		// Token: 0x04000DBB RID: 3515
		protected string _command;

		// Token: 0x04000DBC RID: 3516
		protected string _info;

		// Token: 0x04000DBD RID: 3517
		protected string _help;
	}
}
