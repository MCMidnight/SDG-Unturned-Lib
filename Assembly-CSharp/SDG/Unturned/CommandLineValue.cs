using System;

namespace SDG.Unturned
{
	/// <summary>
	/// Parses -X=Y from command-line.
	/// Ideally we could do "where T : TryParse" but for the meantime there are specialized subclasses.
	/// </summary>
	// Token: 0x0200029A RID: 666
	public abstract class CommandLineValue<T>
	{
		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x0600141A RID: 5146 RVA: 0x0004AB32 File Offset: 0x00048D32
		// (set) Token: 0x0600141B RID: 5147 RVA: 0x0004AB3A File Offset: 0x00048D3A
		public string key { get; protected set; }

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x0600141C RID: 5148 RVA: 0x0004AB43 File Offset: 0x00048D43
		// (set) Token: 0x0600141D RID: 5149 RVA: 0x0004AB4B File Offset: 0x00048D4B
		public bool hasValue { get; protected set; }

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x0600141E RID: 5150 RVA: 0x0004AB54 File Offset: 0x00048D54
		// (set) Token: 0x0600141F RID: 5151 RVA: 0x0004AB5C File Offset: 0x00048D5C
		public T value { get; protected set; }

		// Token: 0x06001420 RID: 5152
		protected abstract bool tryParse(string stringValue);

		// Token: 0x06001421 RID: 5153 RVA: 0x0004AB68 File Offset: 0x00048D68
		public CommandLineValue(string key)
		{
			this.key = key;
			this.hasValue = false;
			this.value = default(T);
			string text;
			if (CommandLine.TryParseValue(key, out text))
			{
				if (string.IsNullOrWhiteSpace(text))
				{
					UnturnedLog.warn("Expected non-empty value for '{0}' on command-line", new object[]
					{
						key
					});
					return;
				}
				if (this.tryParse(text))
				{
					this.hasValue = true;
					UnturnedLog.info("Parsed '{0}' as '{1}' from command-line", new object[]
					{
						key,
						this.value
					});
					return;
				}
				UnturnedLog.warn("Unable to parse '{0}' as '{1}' from command-line", new object[]
				{
					key,
					text
				});
			}
		}
	}
}
