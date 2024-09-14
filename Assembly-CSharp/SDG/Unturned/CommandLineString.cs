using System;

namespace SDG.Unturned
{
	// Token: 0x0200029D RID: 669
	public class CommandLineString : CommandLineValue<string>
	{
		// Token: 0x06001426 RID: 5158 RVA: 0x0004AC61 File Offset: 0x00048E61
		public CommandLineString(string key) : base(key)
		{
		}

		// Token: 0x06001427 RID: 5159 RVA: 0x0004AC6A File Offset: 0x00048E6A
		protected override bool tryParse(string stringValue)
		{
			base.value = stringValue;
			return true;
		}
	}
}
