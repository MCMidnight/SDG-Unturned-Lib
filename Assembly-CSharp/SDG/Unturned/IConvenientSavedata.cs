using System;

namespace SDG.Unturned
{
	// Token: 0x02000421 RID: 1057
	public interface IConvenientSavedata
	{
		// Token: 0x06001FAE RID: 8110
		bool read(string key, out string value);

		// Token: 0x06001FAF RID: 8111
		void write(string key, string value);

		// Token: 0x06001FB0 RID: 8112
		bool read(string key, out DateTime value);

		// Token: 0x06001FB1 RID: 8113
		void write(string key, DateTime value);

		// Token: 0x06001FB2 RID: 8114
		bool read(string key, out bool value);

		// Token: 0x06001FB3 RID: 8115
		void write(string key, bool value);

		// Token: 0x06001FB4 RID: 8116
		bool read(string key, out long value);

		// Token: 0x06001FB5 RID: 8117
		void write(string key, long value);

		// Token: 0x06001FB6 RID: 8118
		bool hasFlag(string flag);

		// Token: 0x06001FB7 RID: 8119
		void setFlag(string flag);
	}
}
