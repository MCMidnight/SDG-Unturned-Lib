using System;

namespace SDG.Unturned
{
	// Token: 0x020002CA RID: 714
	public class INPCReward
	{
		// Token: 0x060014D6 RID: 5334 RVA: 0x0004D563 File Offset: 0x0004B763
		public virtual void GrantReward(Player player)
		{
		}

		// Token: 0x060014D7 RID: 5335 RVA: 0x0004D565 File Offset: 0x0004B765
		public virtual string formatReward(Player player)
		{
			if (!string.IsNullOrEmpty(this.text))
			{
				return this.text;
			}
			return null;
		}

		// Token: 0x060014D8 RID: 5336 RVA: 0x0004D57C File Offset: 0x0004B77C
		public virtual ISleekElement createUI(Player player)
		{
			string text = this.formatReward(player);
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}
			ISleekBox sleekBox = Glazier.Get().CreateBox();
			sleekBox.SizeOffset_Y = 30f;
			sleekBox.SizeScale_X = 1f;
			ISleekLabel sleekLabel = Glazier.Get().CreateLabel();
			sleekLabel.PositionOffset_X = 5f;
			sleekLabel.SizeOffset_X = -10f;
			sleekLabel.SizeScale_X = 1f;
			sleekLabel.SizeScale_Y = 1f;
			sleekLabel.TextAlignment = 3;
			sleekLabel.TextColor = 4;
			sleekLabel.TextContrastContext = 1;
			sleekLabel.AllowRichText = true;
			sleekLabel.Text = text;
			sleekBox.AddChild(sleekLabel);
			return sleekBox;
		}

		// Token: 0x060014D9 RID: 5337 RVA: 0x0004D621 File Offset: 0x0004B821
		public INPCReward(string newText)
		{
			this.text = newText;
		}

		// Token: 0x060014DA RID: 5338 RVA: 0x0004D63B File Offset: 0x0004B83B
		[Obsolete("Removed shouldSend parameter because GrantReward is only called on the server now")]
		public virtual void grantReward(Player player, bool shouldSend)
		{
			this.GrantReward(player);
		}

		/// <summary>
		/// If &gt;0 the game will start a coroutine to grant the reward after waiting.
		/// </summary>
		// Token: 0x0400085C RID: 2140
		public float grantDelaySeconds = -1f;

		// Token: 0x0400085D RID: 2141
		protected string text;
	}
}
