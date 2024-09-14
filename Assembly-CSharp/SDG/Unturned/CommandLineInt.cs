using System;

namespace SDG.Unturned
{
	// Token: 0x0200029B RID: 667
	public class CommandLineInt : CommandLineValue<int>
	{
		// Token: 0x06001422 RID: 5154 RVA: 0x0004AC0A File Offset: 0x00048E0A
		public CommandLineInt(string key) : base(key)
		{
		}

		// Token: 0x06001423 RID: 5155 RVA: 0x0004AC14 File Offset: 0x00048E14
		protected override bool tryParse(string stringValue)
		{
			int value;
			if (int.TryParse(stringValue, ref value))
			{
				base.value = value;
				return true;
			}
			return false;
		}
	}
}
