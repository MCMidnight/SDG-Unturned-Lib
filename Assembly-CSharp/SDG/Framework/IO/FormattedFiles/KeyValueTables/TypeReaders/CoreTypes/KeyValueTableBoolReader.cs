using System;

namespace SDG.Framework.IO.FormattedFiles.KeyValueTables.TypeReaders.CoreTypes
{
	// Token: 0x020000D9 RID: 217
	public class KeyValueTableBoolReader : IFormattedTypeReader
	{
		// Token: 0x06000589 RID: 1417 RVA: 0x00015340 File Offset: 0x00013540
		public object read(IFormattedFileReader reader)
		{
			string text = reader.readValue();
			if (text == null)
			{
				return false;
			}
			if (text.Equals("false", 5))
			{
				return false;
			}
			if (text.Equals("true", 5))
			{
				return true;
			}
			if (text.Equals("0", 5) || text.Equals("no", 5) || text.Equals("n", 5) || text.Equals("f", 5))
			{
				return false;
			}
			if (text.Equals("1", 5) || text.Equals("yes", 5) || text.Equals("y", 5) || text.Equals("t", 5))
			{
				return true;
			}
			return false;
		}
	}
}
