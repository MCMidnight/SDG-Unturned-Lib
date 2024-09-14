using System;
using SDG.SteamworksProvider.Services.Store;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200070B RID: 1803
	public class SleekCharacter : SleekWrapper
	{
		// Token: 0x06003BB4 RID: 15284 RVA: 0x00117928 File Offset: 0x00115B28
		public void updateCharacter(Character character)
		{
			this.nameLabel.Text = MenuSurvivorsCharacterUI.localization.format("Name_Label", character.name);
			this.nickLabel.Text = MenuSurvivorsCharacterUI.localization.format("Nick_Label", character.nick);
			this.skillsetLabel.Text = MenuSurvivorsCharacterUI.localization.format("Skillset_" + ((byte)character.skillset).ToString());
		}

		// Token: 0x06003BB5 RID: 15285 RVA: 0x001179A4 File Offset: 0x00115BA4
		private void onClickedButton(ISleekElement button)
		{
			if (!Provider.isPro && this.index >= Customization.FREE_CHARACTERS)
			{
				Provider.provider.storeService.open(new SteamworksStorePackageID(Provider.PRO_ID.m_AppId));
				return;
			}
			ClickedCharacter clickedCharacter = this.onClickedCharacter;
			if (clickedCharacter == null)
			{
				return;
			}
			clickedCharacter(this, this.index);
		}

		// Token: 0x06003BB6 RID: 15286 RVA: 0x001179FC File Offset: 0x00115BFC
		public SleekCharacter(byte newIndex)
		{
			this.index = newIndex;
			this.button = Glazier.Get().CreateButton();
			this.button.SizeScale_X = 1f;
			this.button.SizeScale_Y = 1f;
			this.button.OnClicked += new ClickedButton(this.onClickedButton);
			base.AddChild(this.button);
			this.nameLabel = Glazier.Get().CreateLabel();
			this.nameLabel.PositionOffset_X = 5f;
			this.nameLabel.PositionOffset_Y = 5f;
			this.nameLabel.SizeOffset_X = -10f;
			this.nameLabel.SizeOffset_Y = -10f;
			this.nameLabel.SizeScale_X = 1f;
			this.nameLabel.SizeScale_Y = 1f;
			this.nameLabel.TextAlignment = 1;
			this.button.AddChild(this.nameLabel);
			this.nickLabel = Glazier.Get().CreateLabel();
			this.nickLabel.PositionOffset_X = 5f;
			this.nickLabel.PositionOffset_Y = 5f;
			this.nickLabel.SizeOffset_X = -10f;
			this.nickLabel.SizeOffset_Y = -10f;
			this.nickLabel.SizeScale_X = 1f;
			this.nickLabel.SizeScale_Y = 1f;
			this.nickLabel.TextAlignment = 4;
			this.button.AddChild(this.nickLabel);
			this.skillsetLabel = Glazier.Get().CreateLabel();
			this.skillsetLabel.PositionOffset_X = 5f;
			this.skillsetLabel.PositionOffset_Y = 5f;
			this.skillsetLabel.SizeOffset_X = -10f;
			this.skillsetLabel.SizeOffset_Y = -10f;
			this.skillsetLabel.SizeScale_X = 1f;
			this.skillsetLabel.SizeScale_Y = 1f;
			this.skillsetLabel.TextAlignment = 7;
			this.button.AddChild(this.skillsetLabel);
			if (!Provider.isPro && this.index >= Customization.FREE_CHARACTERS)
			{
				Bundle bundle = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Pro/Pro.unity3d");
				ISleekImage sleekImage = Glazier.Get().CreateImage();
				sleekImage.PositionOffset_X = -20f;
				sleekImage.PositionOffset_Y = -20f;
				sleekImage.PositionScale_X = 0.5f;
				sleekImage.PositionScale_Y = 0.5f;
				sleekImage.SizeOffset_X = 40f;
				sleekImage.SizeOffset_Y = 40f;
				sleekImage.Texture = bundle.load<Texture2D>("Lock_Medium");
				this.button.AddChild(sleekImage);
				bundle.unload();
			}
			this.updateCharacter(Characters.list[(int)this.index]);
		}

		// Token: 0x04002552 RID: 9554
		public ClickedCharacter onClickedCharacter;

		// Token: 0x04002553 RID: 9555
		private byte index;

		// Token: 0x04002554 RID: 9556
		private ISleekButton button;

		// Token: 0x04002555 RID: 9557
		private ISleekLabel nameLabel;

		// Token: 0x04002556 RID: 9558
		private ISleekLabel nickLabel;

		// Token: 0x04002557 RID: 9559
		private ISleekLabel skillsetLabel;
	}
}
