using System;
using System.Collections.Generic;
using System.IO;

namespace SDG.Framework.IO.FormattedFiles.KeyValueTables
{
	// Token: 0x020000C5 RID: 197
	public class LimitedKeyValueTableReader : KeyValueTableReader
	{
		/// <summary>
		/// After the key "limit" is loaded we stop reading.
		/// </summary>
		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600055D RID: 1373 RVA: 0x00014D2B File Offset: 0x00012F2B
		// (set) Token: 0x0600055E RID: 1374 RVA: 0x00014D33 File Offset: 0x00012F33
		public string limit { get; protected set; }

		// Token: 0x0600055F RID: 1375 RVA: 0x00014D3C File Offset: 0x00012F3C
		protected override bool canContinueReadDictionary(StreamReader input, Dictionary<string, object> scope)
		{
			return !(this.dictionaryKey == this.limit) && base.canContinueReadDictionary(input, scope);
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x00014D5B File Offset: 0x00012F5B
		public LimitedKeyValueTableReader()
		{
			this.limit = null;
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x00014D6A File Offset: 0x00012F6A
		public LimitedKeyValueTableReader(StreamReader input) : this(null, input)
		{
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x00014D74 File Offset: 0x00012F74
		public LimitedKeyValueTableReader(string newLimit, StreamReader input) : base(input)
		{
			this.limit = newLimit;
		}
	}
}
