using System;

namespace SDG.Unturned
{
	// Token: 0x02000310 RID: 784
	public class CommandLineFlag
	{
		// Token: 0x1700045D RID: 1117
		// (get) Token: 0x060017AB RID: 6059 RVA: 0x00056F6E File Offset: 0x0005516E
		// (set) Token: 0x060017AC RID: 6060 RVA: 0x00056F76 File Offset: 0x00055176
		public string flag { get; protected set; }

		// Token: 0x1700045E RID: 1118
		// (get) Token: 0x060017AD RID: 6061 RVA: 0x00056F7F File Offset: 0x0005517F
		// (set) Token: 0x060017AE RID: 6062 RVA: 0x00056F87 File Offset: 0x00055187
		public bool defaultValue { get; protected set; }

		// Token: 0x060017AF RID: 6063 RVA: 0x00056F90 File Offset: 0x00055190
		public static implicit operator bool(CommandLineFlag flag)
		{
			return flag.value;
		}

		// Token: 0x060017B0 RID: 6064 RVA: 0x00056F98 File Offset: 0x00055198
		public CommandLineFlag(bool defaultValue, string flag)
		{
			this.defaultValue = defaultValue;
			this.flag = flag;
			this.value = ((Environment.CommandLine.IndexOf(flag, 3) >= 0) ? (!defaultValue) : defaultValue);
		}

		// Token: 0x04000AA5 RID: 2725
		public bool value;
	}
}
