using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000795 RID: 1941
	public class MenuTitleUI
	{
		// Token: 0x06004082 RID: 16514 RVA: 0x0014E027 File Offset: 0x0014C227
		public static void open()
		{
			if (MenuTitleUI.active)
			{
				return;
			}
			MenuTitleUI.active = true;
			MenuTitleUI.container.AnimateIntoView();
		}

		// Token: 0x06004083 RID: 16515 RVA: 0x0014E041 File Offset: 0x0014C241
		public static void close()
		{
			if (!MenuTitleUI.active)
			{
				return;
			}
			MenuTitleUI.active = false;
			MenuTitleUI.container.AnimateOutOfView(0f, 1f);
		}

		// Token: 0x06004084 RID: 16516 RVA: 0x0014E068 File Offset: 0x0014C268
		private static void onClickedStatButton(ISleekElement button)
		{
			byte b;
			do
			{
				b = (byte)Random.Range(1, (int)(MenuTitleUI.STAT_COUNT + 1));
			}
			while (b == (byte)MenuTitleUI.stat);
			MenuTitleUI.stat = (EPlayerStat)b;
			if (MenuTitleUI.stat == EPlayerStat.KILLS_ZOMBIES_NORMAL)
			{
				int num;
				Provider.provider.statisticsService.userStatisticsService.getStatistic("Kills_Zombies_Normal", out num);
				long num2;
				Provider.provider.statisticsService.globalStatisticsService.getStatistic("Kills_Zombies_Normal", out num2);
				MenuTitleUI.statButton.Text = MenuTitleUI.localization.format("Stat_Kills_Zombies_Normal", num.ToString("n0"), num2.ToString("n0"));
				return;
			}
			if (MenuTitleUI.stat == EPlayerStat.KILLS_PLAYERS)
			{
				int num3;
				Provider.provider.statisticsService.userStatisticsService.getStatistic("Kills_Players", out num3);
				long num4;
				Provider.provider.statisticsService.globalStatisticsService.getStatistic("Kills_Players", out num4);
				MenuTitleUI.statButton.Text = MenuTitleUI.localization.format("Stat_Kills_Players", num3.ToString("n0"), num4.ToString("n0"));
				return;
			}
			if (MenuTitleUI.stat == EPlayerStat.FOUND_ITEMS)
			{
				int num5;
				Provider.provider.statisticsService.userStatisticsService.getStatistic("Found_Items", out num5);
				long num6;
				Provider.provider.statisticsService.globalStatisticsService.getStatistic("Found_Items", out num6);
				MenuTitleUI.statButton.Text = MenuTitleUI.localization.format("Stat_Found_Items", num5.ToString("n0"), num6.ToString("n0"));
				return;
			}
			if (MenuTitleUI.stat == EPlayerStat.FOUND_RESOURCES)
			{
				int num7;
				Provider.provider.statisticsService.userStatisticsService.getStatistic("Found_Resources", out num7);
				long num8;
				Provider.provider.statisticsService.globalStatisticsService.getStatistic("Found_Resources", out num8);
				MenuTitleUI.statButton.Text = MenuTitleUI.localization.format("Stat_Found_Resources", num7.ToString("n0"), num8.ToString("n0"));
				return;
			}
			if (MenuTitleUI.stat == EPlayerStat.FOUND_EXPERIENCE)
			{
				int num9;
				Provider.provider.statisticsService.userStatisticsService.getStatistic("Found_Experience", out num9);
				long num10;
				Provider.provider.statisticsService.globalStatisticsService.getStatistic("Found_Experience", out num10);
				MenuTitleUI.statButton.Text = MenuTitleUI.localization.format("Stat_Found_Experience", num9.ToString("n0"), num10.ToString("n0"));
				return;
			}
			if (MenuTitleUI.stat == EPlayerStat.KILLS_ZOMBIES_MEGA)
			{
				int num11;
				Provider.provider.statisticsService.userStatisticsService.getStatistic("Kills_Zombies_Mega", out num11);
				long num12;
				Provider.provider.statisticsService.globalStatisticsService.getStatistic("Kills_Zombies_Mega", out num12);
				MenuTitleUI.statButton.Text = MenuTitleUI.localization.format("Stat_Kills_Zombies_Mega", num11.ToString("n0"), num12.ToString("n0"));
				return;
			}
			if (MenuTitleUI.stat == EPlayerStat.DEATHS_PLAYERS)
			{
				int num13;
				Provider.provider.statisticsService.userStatisticsService.getStatistic("Deaths_Players", out num13);
				long num14;
				Provider.provider.statisticsService.globalStatisticsService.getStatistic("Deaths_Players", out num14);
				MenuTitleUI.statButton.Text = MenuTitleUI.localization.format("Stat_Deaths_Players", num13.ToString("n0"), num14.ToString("n0"));
				return;
			}
			if (MenuTitleUI.stat == EPlayerStat.KILLS_ANIMALS)
			{
				int num15;
				Provider.provider.statisticsService.userStatisticsService.getStatistic("Kills_Animals", out num15);
				long num16;
				Provider.provider.statisticsService.globalStatisticsService.getStatistic("Kills_Animals", out num16);
				MenuTitleUI.statButton.Text = MenuTitleUI.localization.format("Stat_Kills_Animals", num15.ToString("n0"), num16.ToString("n0"));
				return;
			}
			if (MenuTitleUI.stat == EPlayerStat.FOUND_CRAFTS)
			{
				int num17;
				Provider.provider.statisticsService.userStatisticsService.getStatistic("Found_Crafts", out num17);
				long num18;
				Provider.provider.statisticsService.globalStatisticsService.getStatistic("Found_Crafts", out num18);
				MenuTitleUI.statButton.Text = MenuTitleUI.localization.format("Stat_Found_Crafts", num17.ToString("n0"), num18.ToString("n0"));
				return;
			}
			if (MenuTitleUI.stat == EPlayerStat.FOUND_FISHES)
			{
				int num19;
				Provider.provider.statisticsService.userStatisticsService.getStatistic("Found_Fishes", out num19);
				long num20;
				Provider.provider.statisticsService.globalStatisticsService.getStatistic("Found_Fishes", out num20);
				MenuTitleUI.statButton.Text = MenuTitleUI.localization.format("Stat_Found_Fishes", num19.ToString("n0"), num20.ToString("n0"));
				return;
			}
			if (MenuTitleUI.stat == EPlayerStat.FOUND_PLANTS)
			{
				int num21;
				Provider.provider.statisticsService.userStatisticsService.getStatistic("Found_Plants", out num21);
				long num22;
				Provider.provider.statisticsService.globalStatisticsService.getStatistic("Found_Plants", out num22);
				MenuTitleUI.statButton.Text = MenuTitleUI.localization.format("Stat_Found_Plants", num21.ToString("n0"), num22.ToString("n0"));
				return;
			}
			if (MenuTitleUI.stat == EPlayerStat.ACCURACY)
			{
				int num23;
				Provider.provider.statisticsService.userStatisticsService.getStatistic("Accuracy_Shot", out num23);
				int num24;
				Provider.provider.statisticsService.userStatisticsService.getStatistic("Accuracy_Hit", out num24);
				long num25;
				Provider.provider.statisticsService.globalStatisticsService.getStatistic("Accuracy_Shot", out num25);
				long num26;
				Provider.provider.statisticsService.globalStatisticsService.getStatistic("Accuracy_Hit", out num26);
				float num27;
				if (num23 == 0 || num24 == 0)
				{
					num27 = 0f;
				}
				else
				{
					num27 = (float)num24 / (float)num23;
				}
				double num28;
				if (num25 == 0L || num26 == 0L)
				{
					num28 = 0.0;
				}
				else
				{
					num28 = (double)num26 / (double)num25;
				}
				MenuTitleUI.statButton.Text = MenuTitleUI.localization.format("Stat_Accuracy", new object[]
				{
					num23.ToString("n0"),
					(float)((int)(num27 * 10000f)) / 100f,
					num25.ToString("n0"),
					(double)((long)(num28 * 10000.0)) / 100.0
				});
				return;
			}
			if (MenuTitleUI.stat == EPlayerStat.HEADSHOTS)
			{
				int num29;
				Provider.provider.statisticsService.userStatisticsService.getStatistic("Headshots", out num29);
				long num30;
				Provider.provider.statisticsService.globalStatisticsService.getStatistic("Headshots", out num30);
				MenuTitleUI.statButton.Text = MenuTitleUI.localization.format("Stat_Headshots", num29.ToString("n0"), num30.ToString("n0"));
				return;
			}
			if (MenuTitleUI.stat == EPlayerStat.TRAVEL_FOOT)
			{
				int m;
				Provider.provider.statisticsService.userStatisticsService.getStatistic("Travel_Foot", out m);
				long m2;
				Provider.provider.statisticsService.globalStatisticsService.getStatistic("Travel_Foot", out m2);
				if (OptionsSettings.metric)
				{
					MenuTitleUI.statButton.Text = MenuTitleUI.localization.format("Stat_Travel_Foot", m.ToString("n0") + " m", m2.ToString("n0") + " m");
					return;
				}
				MenuTitleUI.statButton.Text = MenuTitleUI.localization.format("Stat_Travel_Foot", MeasurementTool.MtoYd(m).ToString("n0") + " yd", MeasurementTool.MtoYd(m2).ToString("n0") + " yd");
				return;
			}
			else if (MenuTitleUI.stat == EPlayerStat.TRAVEL_VEHICLE)
			{
				int m3;
				Provider.provider.statisticsService.userStatisticsService.getStatistic("Travel_Vehicle", out m3);
				long m4;
				Provider.provider.statisticsService.globalStatisticsService.getStatistic("Travel_Vehicle", out m4);
				if (OptionsSettings.metric)
				{
					MenuTitleUI.statButton.Text = MenuTitleUI.localization.format("Stat_Travel_Vehicle", m3.ToString("n0") + " m", m4.ToString("n0") + " m");
					return;
				}
				MenuTitleUI.statButton.Text = MenuTitleUI.localization.format("Stat_Travel_Vehicle", MeasurementTool.MtoYd(m3).ToString("n0") + " yd", MeasurementTool.MtoYd(m4).ToString("n0") + " yd");
				return;
			}
			else
			{
				if (MenuTitleUI.stat == EPlayerStat.ARENA_WINS)
				{
					int num31;
					Provider.provider.statisticsService.userStatisticsService.getStatistic("Arena_Wins", out num31);
					long num32;
					Provider.provider.statisticsService.globalStatisticsService.getStatistic("Arena_Wins", out num32);
					MenuTitleUI.statButton.Text = MenuTitleUI.localization.format("Stat_Arena_Wins", num31.ToString("n0"), num32.ToString("n0"));
					return;
				}
				if (MenuTitleUI.stat == EPlayerStat.FOUND_BUILDABLES)
				{
					int num33;
					Provider.provider.statisticsService.userStatisticsService.getStatistic("Found_Buildables", out num33);
					long num34;
					Provider.provider.statisticsService.globalStatisticsService.getStatistic("Found_Buildables", out num34);
					MenuTitleUI.statButton.Text = MenuTitleUI.localization.format("Stat_Found_Buildables", num33.ToString("n0"), num34.ToString("n0"));
					return;
				}
				if (MenuTitleUI.stat == EPlayerStat.FOUND_THROWABLES)
				{
					int num35;
					Provider.provider.statisticsService.userStatisticsService.getStatistic("Found_Throwables", out num35);
					long num36;
					Provider.provider.statisticsService.globalStatisticsService.getStatistic("Found_Throwables", out num36);
					MenuTitleUI.statButton.Text = MenuTitleUI.localization.format("Stat_Found_Throwables", num35.ToString("n0"), num36.ToString("n0"));
				}
				return;
			}
		}

		// Token: 0x06004085 RID: 16517 RVA: 0x0014EA44 File Offset: 0x0014CC44
		public MenuTitleUI()
		{
			MenuTitleUI.localization = Localization.read("/Menu/MenuTitle.dat");
			MenuTitleUI.container = new SleekFullscreenBox();
			MenuTitleUI.container.PositionOffset_X = 10f;
			MenuTitleUI.container.PositionOffset_Y = 10f;
			MenuTitleUI.container.SizeOffset_X = -20f;
			MenuTitleUI.container.SizeOffset_Y = -20f;
			MenuTitleUI.container.SizeScale_X = 1f;
			MenuTitleUI.container.SizeScale_Y = 1f;
			MenuUI.container.AddChild(MenuTitleUI.container);
			MenuTitleUI.active = true;
			MenuTitleUI.titleBox = Glazier.Get().CreateBox();
			MenuTitleUI.titleBox.SizeOffset_Y = 100f;
			MenuTitleUI.titleBox.SizeScale_X = 1f;
			MenuTitleUI.container.AddChild(MenuTitleUI.titleBox);
			MenuTitleUI.titleLabel = Glazier.Get().CreateLabel();
			MenuTitleUI.titleLabel.SizeScale_X = 1f;
			MenuTitleUI.titleLabel.SizeOffset_Y = 70f;
			MenuTitleUI.titleLabel.FontSize = 5;
			MenuTitleUI.titleLabel.Text = Provider.APP_NAME;
			MenuTitleUI.titleBox.AddChild(MenuTitleUI.titleLabel);
			MenuTitleUI.authorLabel = Glazier.Get().CreateLabel();
			MenuTitleUI.authorLabel.PositionOffset_Y = 60f;
			MenuTitleUI.authorLabel.SizeScale_X = 1f;
			MenuTitleUI.authorLabel.SizeOffset_Y = 30f;
			MenuTitleUI.authorLabel.Text = MenuTitleUI.localization.format("Author_Label", Provider.APP_VERSION, Provider.APP_AUTHOR);
			MenuTitleUI.titleBox.AddChild(MenuTitleUI.authorLabel);
			MenuTitleUI.statButton = Glazier.Get().CreateButton();
			MenuTitleUI.statButton.PositionOffset_Y = 110f;
			MenuTitleUI.statButton.SizeOffset_Y = 50f;
			MenuTitleUI.statButton.SizeScale_X = 1f;
			MenuTitleUI.statButton.OnClicked += new ClickedButton(MenuTitleUI.onClickedStatButton);
			MenuTitleUI.container.AddChild(MenuTitleUI.statButton);
			MenuTitleUI.stat = EPlayerStat.NONE;
			MenuTitleUI.onClickedStatButton(MenuTitleUI.statButton);
		}

		// Token: 0x04002968 RID: 10600
		private static readonly byte STAT_COUNT = 18;

		// Token: 0x04002969 RID: 10601
		private static Local localization;

		// Token: 0x0400296A RID: 10602
		private static SleekFullscreenBox container;

		// Token: 0x0400296B RID: 10603
		public static bool active;

		// Token: 0x0400296C RID: 10604
		private static ISleekBox titleBox;

		// Token: 0x0400296D RID: 10605
		private static ISleekLabel titleLabel;

		// Token: 0x0400296E RID: 10606
		private static ISleekLabel authorLabel;

		// Token: 0x0400296F RID: 10607
		private static ISleekButton statButton;

		// Token: 0x04002970 RID: 10608
		private static EPlayerStat stat;
	}
}
