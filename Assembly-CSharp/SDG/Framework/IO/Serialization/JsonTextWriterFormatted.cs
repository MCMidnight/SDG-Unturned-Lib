using System;
using System.IO;
using Newtonsoft.Json;

namespace SDG.Framework.IO.Serialization
{
	// Token: 0x020000B8 RID: 184
	public class JsonTextWriterFormatted : JsonTextWriter
	{
		// Token: 0x060004EC RID: 1260 RVA: 0x0001409C File Offset: 0x0001229C
		public override void WriteStartArray()
		{
			base.Formatting = Formatting.None;
			base.WriteIndent();
			base.WriteStartArray();
			base.Formatting = Formatting.Indented;
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x000140B8 File Offset: 0x000122B8
		public JsonTextWriterFormatted(TextWriter textWriter) : base(textWriter)
		{
			base.IndentChar = '\t';
			base.Indentation = 1;
		}
	}
}
