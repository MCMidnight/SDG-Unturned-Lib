using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000168 RID: 360
	internal class GlazierLabel_IMGUI : GlazierElementBase_IMGUI, ISleekLabel, ISleekElement
	{
		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000921 RID: 2337 RVA: 0x0001FD76 File Offset: 0x0001DF76
		// (set) Token: 0x06000922 RID: 2338 RVA: 0x0001FD7E File Offset: 0x0001DF7E
		public string Text
		{
			get
			{
				return this._text;
			}
			set
			{
				this._text = value;
				this.calculateContent();
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000923 RID: 2339 RVA: 0x0001FD8D File Offset: 0x0001DF8D
		// (set) Token: 0x06000924 RID: 2340 RVA: 0x0001FD95 File Offset: 0x0001DF95
		public string TooltipText
		{
			get
			{
				return this._tooltip;
			}
			set
			{
				this._tooltip = value;
				this.calculateContent();
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000925 RID: 2341 RVA: 0x0001FDA4 File Offset: 0x0001DFA4
		// (set) Token: 0x06000926 RID: 2342 RVA: 0x0001FDAC File Offset: 0x0001DFAC
		public FontStyle FontStyle { get; set; }

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000927 RID: 2343 RVA: 0x0001FDB5 File Offset: 0x0001DFB5
		// (set) Token: 0x06000928 RID: 2344 RVA: 0x0001FDBD File Offset: 0x0001DFBD
		public TextAnchor TextAlignment { get; set; } = 4;

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000929 RID: 2345 RVA: 0x0001FDC6 File Offset: 0x0001DFC6
		// (set) Token: 0x0600092A RID: 2346 RVA: 0x0001FDCE File Offset: 0x0001DFCE
		public ESleekFontSize FontSize
		{
			get
			{
				return this.fontSizeEnum;
			}
			set
			{
				this.fontSizeEnum = value;
				this.fontSizeInt = GlazierUtils_IMGUI.GetFontSize(this.fontSizeEnum);
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x0600092B RID: 2347 RVA: 0x0001FDE8 File Offset: 0x0001DFE8
		// (set) Token: 0x0600092C RID: 2348 RVA: 0x0001FDF0 File Offset: 0x0001DFF0
		public ETextContrastContext TextContrastContext { get; set; }

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x0600092D RID: 2349 RVA: 0x0001FDF9 File Offset: 0x0001DFF9
		// (set) Token: 0x0600092E RID: 2350 RVA: 0x0001FE01 File Offset: 0x0001E001
		public SleekColor TextColor { get; set; } = GlazierConst.DefaultLabelForegroundColor;

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x0600092F RID: 2351 RVA: 0x0001FE0A File Offset: 0x0001E00A
		// (set) Token: 0x06000930 RID: 2352 RVA: 0x0001FE12 File Offset: 0x0001E012
		public bool AllowRichText { get; set; }

		// Token: 0x06000931 RID: 2353 RVA: 0x0001FE1B File Offset: 0x0001E01B
		protected virtual void calculateContent()
		{
			this.content = new GUIContent(this.Text, this.TooltipText);
			if (this.AllowRichText)
			{
				this.shadowContent = RichTextUtil.makeShadowContent(this.content);
				return;
			}
			this.shadowContent = null;
		}

		// Token: 0x06000932 RID: 2354 RVA: 0x0001FE55 File Offset: 0x0001E055
		public GlazierLabel_IMGUI()
		{
			this.calculateContent();
			this.FontSize = 2;
		}

		// Token: 0x06000933 RID: 2355 RVA: 0x0001FE94 File Offset: 0x0001E094
		public override void OnGUI()
		{
			GlazierUtils_IMGUI.drawLabel(this.drawRect, this.FontStyle, this.TextAlignment, this.fontSizeInt, this.shadowContent, this.TextColor, this.content, this.TextContrastContext);
			base.ChildrenOnGUI();
		}

		// Token: 0x04000379 RID: 889
		private string _text = "";

		// Token: 0x0400037A RID: 890
		private string _tooltip = "";

		// Token: 0x0400037D RID: 893
		protected int fontSizeInt;

		// Token: 0x0400037E RID: 894
		private ESleekFontSize fontSizeEnum;

		// Token: 0x04000382 RID: 898
		public GUIContent content;

		// Token: 0x04000383 RID: 899
		protected GUIContent shadowContent;
	}
}
