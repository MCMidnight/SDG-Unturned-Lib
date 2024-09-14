using System;

namespace SDG.Unturned
{
	// Token: 0x0200029E RID: 670
	public class CommandLineBool : CommandLineValue<bool>
	{
		// Token: 0x06001428 RID: 5160 RVA: 0x0004AC74 File Offset: 0x00048E74
		public CommandLineBool(string key) : base(key)
		{
		}

		// Token: 0x06001429 RID: 5161 RVA: 0x0004AC80 File Offset: 0x00048E80
		protected override bool tryParse(string stringValue)
		{
			if (stringValue.Equals("y", 3) || stringValue.Equals("yes", 3) || stringValue == "1" || stringValue.Equals("true", 3))
			{
				base.value = true;
				return true;
			}
			if (stringValue.Equals("n", 3) || stringValue.Equals("no", 3) || stringValue == "0" || stringValue.Equals("false", 3))
			{
				base.value = false;
				return true;
			}
			return false;
		}
	}
}
