using System;

namespace SDG.Unturned
{
	// Token: 0x02000489 RID: 1161
	public class Action
	{
		// Token: 0x1700072A RID: 1834
		// (get) Token: 0x06002450 RID: 9296 RVA: 0x000915C2 File Offset: 0x0008F7C2
		public ushort source
		{
			get
			{
				return this._source;
			}
		}

		// Token: 0x1700072B RID: 1835
		// (get) Token: 0x06002451 RID: 9297 RVA: 0x000915CA File Offset: 0x0008F7CA
		public EActionType type
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x1700072C RID: 1836
		// (get) Token: 0x06002452 RID: 9298 RVA: 0x000915D2 File Offset: 0x0008F7D2
		public ActionBlueprint[] blueprints
		{
			get
			{
				return this._blueprints;
			}
		}

		// Token: 0x1700072D RID: 1837
		// (get) Token: 0x06002453 RID: 9299 RVA: 0x000915DA File Offset: 0x0008F7DA
		public string text
		{
			get
			{
				return this._text;
			}
		}

		// Token: 0x1700072E RID: 1838
		// (get) Token: 0x06002454 RID: 9300 RVA: 0x000915E2 File Offset: 0x0008F7E2
		public string tooltip
		{
			get
			{
				return this._tooltip;
			}
		}

		// Token: 0x1700072F RID: 1839
		// (get) Token: 0x06002455 RID: 9301 RVA: 0x000915EA File Offset: 0x0008F7EA
		public string key
		{
			get
			{
				return this._key;
			}
		}

		// Token: 0x06002456 RID: 9302 RVA: 0x000915F2 File Offset: 0x0008F7F2
		public Action(ushort newSource, EActionType newType, ActionBlueprint[] newBlueprints, string newText, string newTooltip, string newKey)
		{
			this._source = newSource;
			this._type = newType;
			this._blueprints = newBlueprints;
			this._text = newText;
			this._tooltip = newTooltip;
			this._key = newKey;
		}

		// Token: 0x0400124D RID: 4685
		private ushort _source;

		// Token: 0x0400124E RID: 4686
		private EActionType _type;

		// Token: 0x0400124F RID: 4687
		private ActionBlueprint[] _blueprints;

		// Token: 0x04001250 RID: 4688
		private string _text;

		// Token: 0x04001251 RID: 4689
		private string _tooltip;

		// Token: 0x04001252 RID: 4690
		private string _key;
	}
}
