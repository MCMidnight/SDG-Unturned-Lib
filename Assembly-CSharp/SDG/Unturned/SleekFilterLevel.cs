using System;

namespace SDG.Unturned
{
	/// <summary>
	/// Button in the list of levels for server browser filters.
	/// </summary>
	// Token: 0x02000724 RID: 1828
	public class SleekFilterLevel : SleekLevel
	{
		// Token: 0x17000B2C RID: 2860
		// (get) Token: 0x06003C4D RID: 15437 RVA: 0x0011BFC9 File Offset: 0x0011A1C9
		// (set) Token: 0x06003C4E RID: 15438 RVA: 0x0011BFD6 File Offset: 0x0011A1D6
		public bool IsIncludedInFilter
		{
			get
			{
				return this.toggle.Value;
			}
			set
			{
				this.toggle.Value = value;
			}
		}

		// Token: 0x06003C4F RID: 15439 RVA: 0x0011BFE4 File Offset: 0x0011A1E4
		public SleekFilterLevel(LevelInfo level) : base(level)
		{
			this.toggle = Glazier.Get().CreateToggle();
			this.toggle.PositionOffset_X = 20f;
			this.toggle.PositionOffset_Y = 30f;
			this.toggle.OnValueChanged += new Toggled(this.OnToggleValueChanged);
			base.AddChild(this.toggle);
		}

		// Token: 0x06003C50 RID: 15440 RVA: 0x0011C04B File Offset: 0x0011A24B
		protected void OnToggleValueChanged(ISleekToggle toggle, bool value)
		{
			ClickedLevel onClickedLevel = this.onClickedLevel;
			if (onClickedLevel == null)
			{
				return;
			}
			onClickedLevel(this, 0);
		}

		// Token: 0x040025B0 RID: 9648
		protected ISleekToggle toggle;
	}
}
