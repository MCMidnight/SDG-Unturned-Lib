using System;

namespace SDG.Unturned
{
	// Token: 0x0200029C RID: 668
	public class CommandLineFloat : CommandLineValue<float>
	{
		// Token: 0x06001424 RID: 5156 RVA: 0x0004AC35 File Offset: 0x00048E35
		public CommandLineFloat(string key) : base(key)
		{
		}

		// Token: 0x06001425 RID: 5157 RVA: 0x0004AC40 File Offset: 0x00048E40
		protected override bool tryParse(string stringValue)
		{
			float value;
			if (float.TryParse(stringValue, ref value))
			{
				base.value = value;
				return true;
			}
			return false;
		}
	}
}
