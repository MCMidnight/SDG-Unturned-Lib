using System;

namespace SDG.Framework.IO.FormattedFiles
{
	// Token: 0x020000BF RID: 191
	public interface IFormattedTypeWriter
	{
		// Token: 0x0600051A RID: 1306
		void write(IFormattedFileWriter writer, object value);
	}
}
