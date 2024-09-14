using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SDG.Unturned
{
	// Token: 0x02000423 RID: 1059
	internal class ConvenientSavedataImplementation : IConvenientSavedata
	{
		// Token: 0x06001FBD RID: 8125 RVA: 0x0007ABB6 File Offset: 0x00078DB6
		public bool read(string key, out string value)
		{
			return this.Strings.TryGetValue(key, ref value);
		}

		// Token: 0x06001FBE RID: 8126 RVA: 0x0007ABC5 File Offset: 0x00078DC5
		public void write(string key, string value)
		{
			this.Strings[key] = value;
			this.isDirty = true;
		}

		// Token: 0x06001FBF RID: 8127 RVA: 0x0007ABDB File Offset: 0x00078DDB
		public bool read(string key, out DateTime value)
		{
			return this.DateTimes.TryGetValue(key, ref value);
		}

		// Token: 0x06001FC0 RID: 8128 RVA: 0x0007ABEA File Offset: 0x00078DEA
		public void write(string key, DateTime value)
		{
			this.DateTimes[key] = value;
			this.isDirty = true;
		}

		// Token: 0x06001FC1 RID: 8129 RVA: 0x0007AC00 File Offset: 0x00078E00
		public bool read(string key, out bool value)
		{
			return this.Booleans.TryGetValue(key, ref value);
		}

		// Token: 0x06001FC2 RID: 8130 RVA: 0x0007AC0F File Offset: 0x00078E0F
		public void write(string key, bool value)
		{
			this.Booleans[key] = value;
			this.isDirty = true;
		}

		// Token: 0x06001FC3 RID: 8131 RVA: 0x0007AC25 File Offset: 0x00078E25
		public bool read(string key, out long value)
		{
			return this.Integers.TryGetValue(key, ref value);
		}

		// Token: 0x06001FC4 RID: 8132 RVA: 0x0007AC34 File Offset: 0x00078E34
		public void write(string key, long value)
		{
			this.Integers[key] = value;
			this.isDirty = true;
		}

		// Token: 0x06001FC5 RID: 8133 RVA: 0x0007AC4A File Offset: 0x00078E4A
		public bool hasFlag(string flag)
		{
			return this.Flags.Contains(flag);
		}

		// Token: 0x06001FC6 RID: 8134 RVA: 0x0007AC58 File Offset: 0x00078E58
		public void setFlag(string flag)
		{
			this.Flags.Add(flag);
			this.isDirty = true;
		}

		// Token: 0x04000FA9 RID: 4009
		public Dictionary<string, string> Strings = new Dictionary<string, string>();

		// Token: 0x04000FAA RID: 4010
		public Dictionary<string, DateTime> DateTimes = new Dictionary<string, DateTime>();

		// Token: 0x04000FAB RID: 4011
		public Dictionary<string, bool> Booleans = new Dictionary<string, bool>();

		// Token: 0x04000FAC RID: 4012
		public Dictionary<string, long> Integers = new Dictionary<string, long>();

		// Token: 0x04000FAD RID: 4013
		public HashSet<string> Flags = new HashSet<string>();

		// Token: 0x04000FAE RID: 4014
		[JsonIgnore]
		public bool isDirty;
	}
}
