using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000791 RID: 1937
	public class MenuCreditsUI
	{
		// Token: 0x06004051 RID: 16465 RVA: 0x0014A9A1 File Offset: 0x00148BA1
		public static void open()
		{
			if (MenuCreditsUI.active)
			{
				return;
			}
			MenuCreditsUI.active = true;
			MenuCreditsUI.container.AnimateIntoView();
		}

		// Token: 0x06004052 RID: 16466 RVA: 0x0014A9BB File Offset: 0x00148BBB
		public static void close()
		{
			if (!MenuCreditsUI.active)
			{
				return;
			}
			MenuCreditsUI.active = false;
			MenuCreditsUI.container.AnimateOutOfView(0f, -1f);
		}

		// Token: 0x06004053 RID: 16467 RVA: 0x0014A9DF File Offset: 0x00148BDF
		private static void onClickedReturnButton(ISleekElement button)
		{
			MenuCreditsUI.close();
			MenuPauseUI.open();
		}

		// Token: 0x06004054 RID: 16468 RVA: 0x0014A9EC File Offset: 0x00148BEC
		public MenuCreditsUI()
		{
			MenuCreditsUI.localization = Localization.read("/Menu/MenuCredits.dat");
			MenuCreditsUI.container = new SleekFullscreenBox();
			MenuCreditsUI.container.PositionOffset_X = 10f;
			MenuCreditsUI.container.PositionOffset_Y = 10f;
			MenuCreditsUI.container.PositionScale_Y = -1f;
			MenuCreditsUI.container.SizeOffset_X = -20f;
			MenuCreditsUI.container.SizeOffset_Y = -20f;
			MenuCreditsUI.container.SizeScale_X = 1f;
			MenuCreditsUI.container.SizeScale_Y = 1f;
			MenuUI.container.AddChild(MenuCreditsUI.container);
			MenuCreditsUI.active = false;
			MenuCreditsUI.returnButton = new SleekButtonIcon(MenuPauseUI.icons.load<Texture2D>("Exit"));
			MenuCreditsUI.returnButton.PositionOffset_X = -250f;
			MenuCreditsUI.returnButton.PositionOffset_Y = 100f;
			MenuCreditsUI.returnButton.PositionScale_X = 0.5f;
			MenuCreditsUI.returnButton.SizeOffset_X = 500f;
			MenuCreditsUI.returnButton.SizeOffset_Y = 50f;
			MenuCreditsUI.returnButton.text = MenuPauseUI.localization.format("Return_Button");
			MenuCreditsUI.returnButton.tooltip = MenuPauseUI.localization.format("Return_Button_Tooltip");
			MenuCreditsUI.returnButton.onClickedButton += new ClickedButton(MenuCreditsUI.onClickedReturnButton);
			MenuCreditsUI.returnButton.fontSize = 3;
			MenuCreditsUI.returnButton.iconColor = 2;
			MenuCreditsUI.container.AddChild(MenuCreditsUI.returnButton);
			MenuCreditsUI.creditsBox = Glazier.Get().CreateBox();
			MenuCreditsUI.creditsBox.PositionOffset_X = -250f;
			MenuCreditsUI.creditsBox.PositionOffset_Y = 160f;
			MenuCreditsUI.creditsBox.PositionScale_X = 0.5f;
			MenuCreditsUI.creditsBox.SizeOffset_X = 500f;
			MenuCreditsUI.creditsBox.SizeOffset_Y = -260f;
			MenuCreditsUI.creditsBox.SizeScale_Y = 1f;
			MenuCreditsUI.creditsBox.FontSize = 3;
			MenuCreditsUI.container.AddChild(MenuCreditsUI.creditsBox);
			MenuCreditsUI.scrollBox = Glazier.Get().CreateScrollView();
			MenuCreditsUI.scrollBox.PositionOffset_X = 5f;
			MenuCreditsUI.scrollBox.PositionOffset_Y = 5f;
			MenuCreditsUI.scrollBox.SizeOffset_X = -10f;
			MenuCreditsUI.scrollBox.SizeOffset_Y = -10f;
			MenuCreditsUI.scrollBox.SizeScale_X = 1f;
			MenuCreditsUI.scrollBox.SizeScale_Y = 1f;
			MenuCreditsUI.scrollBox.ScaleContentToWidth = true;
			MenuCreditsUI.creditsBox.AddChild(MenuCreditsUI.scrollBox);
			float y = 0f;
			MenuCreditsUI.AddHeader(MenuCreditsUI.localization.format("Header_Unturned"), ref y);
			MenuCreditsUI.AddRow("Nelson Sexton", "adding bugs and breaking the game", ref y);
			MenuCreditsUI.AddRow("Tyler \"MoltonMontro\" Pope", "community+web+server admin", ref y);
			MenuCreditsUI.AddRow("Sven Mawby", "RocketMod", ref y);
			MenuCreditsUI.AddRow("Riley Labrecque", "Steamworks .NET", ref y);
			MenuCreditsUI.AddRow("Stephen McKamey", "A* Pathfinding Project", ref y);
			MenuCreditsUI.AddRow("James Newton-King", "Json .NET", ref y);
			MenuCreditsUI.AddRow("Still North Media", "The Firearm Sound Library", ref y);
			MenuCreditsUI.AddRow("Peter Wayne", "GameMaster Audio Pro Sound Collection", ref y);
			MenuCreditsUI.AddRow("John '00' Fleming", "Title Music", ref y);
			MenuCreditsUI.AddRow("staswalle", "Loading Screen Music", ref y);
			MenuCreditsUI.AddHeader(MenuCreditsUI.localization.format("Header_CommunityTeam"), ref y);
			string[] array = new string[]
			{
				"Deathismad",
				"James",
				"Retuuyo",
				"Fran-war",
				"SongPhoenix",
				"Lu",
				"Morkva",
				"Reaver",
				"Shadow",
				"Yarrrr",
				"DeusExMachina",
				"Pablo824",
				"Genestic12",
				"Armaros",
				"Great Hero J",
				"SomeCatIDK"
			};
			Array.Sort<string>(array);
			MenuCreditsUI.AddRowColumns(array, ref y);
			MenuCreditsUI.AddHeader(MenuCreditsUI.localization.format("Header_MapCreators"), ref y);
			string[] array2 = new string[]
			{
				"Nicolas \"Putin3D\" Arisi",
				"Mia \"Myria\" Brookman",
				"Ben \"Paladin\" Hoefer",
				"Nathan \"Wolf_Maniac\" Zwerka",
				"Nolan \"Azz\" Ross",
				"Husky",
				"Emily Barry",
				"Justin \"Gamez2much\" Morton",
				"Terran \"Spyjack\" Orion",
				"Alex \"Rain\" Storanov",
				"Amanda \"Mooki2much\" Hubler",
				"Joshua \"Storm_Epidemic\" Rist",
				"Th3o",
				"Diesel_Sisel",
				"Misterl212",
				"Mitch \"Sketches\" Wheaton",
				"AnimaticFreak",
				"NSTM",
				"Maciej \"Renaxon\" Maziarz",
				"Daniel \"danaby2\" Segboer",
				"Dug",
				"Thom \"Spebby\" Mott",
				"Steven \"MeloCa\" Nadeau",
				"Ethan \"Vilespring\" Lossner",
				"SluggedCascade",
				"Sam \"paper_walls84\" Clerke",
				"clue",
				"Vilaskis \"BATTLEKOT\" Shaleshev",
				"Andrii \"TheCubicNoobik\" Vitiv",
				"Oleksandr \"BlackLion\" Shcherba",
				"Dmitriy \"Potatoes\" Usenko",
				"Liya \"Ms.Evrika\" Bognat",
				"Denis \"FlodotelitoKifo\" Souza",
				"Joao \"L2\" Vitor",
				"Josh \"Leprechan12\" Hogan",
				"Toothy Deerryte",
				"Witness Protection"
			};
			Array.Sort<string>(array2);
			MenuCreditsUI.AddRowColumns(array2, ref y);
			MenuCreditsUI.scrollBox.ContentSizeOffset = new Vector2(0f, y);
		}

		// Token: 0x06004055 RID: 16469 RVA: 0x0014AF6C File Offset: 0x0014916C
		private static void AddHeader(string key, ref float verticalOffset)
		{
			ISleekLabel sleekLabel = Glazier.Get().CreateLabel();
			sleekLabel.PositionOffset_Y = verticalOffset;
			sleekLabel.SizeOffset_Y = 50f;
			sleekLabel.SizeScale_X = 1f;
			sleekLabel.TextAlignment = 4;
			sleekLabel.FontSize = 4;
			sleekLabel.Text = MenuCreditsUI.localization.format(key);
			MenuCreditsUI.scrollBox.AddChild(sleekLabel);
			verticalOffset += sleekLabel.SizeOffset_Y;
		}

		// Token: 0x06004056 RID: 16470 RVA: 0x0014AFD8 File Offset: 0x001491D8
		private static void AddRow(string contributor, string contribution, ref float verticalOffset)
		{
			ISleekLabel sleekLabel = Glazier.Get().CreateLabel();
			sleekLabel.PositionOffset_Y = verticalOffset;
			sleekLabel.SizeOffset_Y = 30f;
			sleekLabel.SizeScale_X = 1f;
			sleekLabel.TextAlignment = 3;
			sleekLabel.FontSize = 3;
			sleekLabel.Text = contributor;
			MenuCreditsUI.scrollBox.AddChild(sleekLabel);
			ISleekLabel sleekLabel2 = Glazier.Get().CreateLabel();
			sleekLabel2.PositionOffset_Y = verticalOffset;
			sleekLabel2.SizeOffset_Y = 30f;
			sleekLabel2.SizeScale_X = 1f;
			sleekLabel2.TextAlignment = 5;
			sleekLabel2.FontSize = 3;
			sleekLabel2.Text = contribution;
			MenuCreditsUI.scrollBox.AddChild(sleekLabel2);
			verticalOffset += 30f;
		}

		// Token: 0x06004057 RID: 16471 RVA: 0x0014B084 File Offset: 0x00149284
		private static void AddRowColumns(string[] contributors, ref float verticalOffset)
		{
			int num = 0;
			foreach (string text in contributors)
			{
				ISleekLabel sleekLabel = Glazier.Get().CreateLabel();
				sleekLabel.PositionOffset_Y = verticalOffset;
				sleekLabel.PositionScale_X = (float)num * 0.5f;
				sleekLabel.SizeOffset_Y = 30f;
				sleekLabel.SizeScale_X = 0.5f;
				sleekLabel.TextAlignment = 4;
				sleekLabel.FontSize = 3;
				sleekLabel.Text = text;
				MenuCreditsUI.scrollBox.AddChild(sleekLabel);
				num++;
				if (num >= 2)
				{
					num = 0;
					verticalOffset += 30f;
				}
			}
			if (num > 0)
			{
				verticalOffset += 30f;
			}
		}

		// Token: 0x04002928 RID: 10536
		private static SleekFullscreenBox container;

		// Token: 0x04002929 RID: 10537
		public static bool active;

		// Token: 0x0400292A RID: 10538
		private static SleekButtonIcon returnButton;

		// Token: 0x0400292B RID: 10539
		private static ISleekBox creditsBox;

		// Token: 0x0400292C RID: 10540
		private static ISleekScrollView scrollBox;

		// Token: 0x0400292D RID: 10541
		private static Local localization;
	}
}
