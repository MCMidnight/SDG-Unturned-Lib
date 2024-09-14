using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020006C4 RID: 1732
	public class ControlsSettings
	{
		/// <summary>
		/// Replace instances of <plugin_num /> with their bound key text.
		/// Allows server effects to display plugin hotkeys.
		/// </summary>
		// Token: 0x060039B9 RID: 14777 RVA: 0x0010E848 File Offset: 0x0010CA48
		public static void formatPluginHotkeysIntoText(ref string text)
		{
			for (int i = 0; i < (int)ControlsSettings.NUM_PLUGIN_KEYS; i++)
			{
				KeyCode pluginKeyCode = ControlsSettings.getPluginKeyCode(i);
				string text2 = ControlsSettings.PLUGIN_KEY_TOKENS[i];
				string keyCodeText = MenuConfigurationControlsUI.getKeyCodeText(pluginKeyCode);
				text = text.Replace(text2, keyCodeText);
			}
		}

		/// <summary>
		/// Item 0 is "1" and item 9 is "0"
		/// </summary>
		// Token: 0x060039BA RID: 14778 RVA: 0x0010E884 File Offset: 0x0010CA84
		public static string getEquipmentHotkeyText(int index)
		{
			return MenuConfigurationControlsUI.getKeyCodeText(ControlsSettings.getEquipmentHotbarKeyCode(index));
		}

		// Token: 0x17000A5D RID: 2653
		// (get) Token: 0x060039BB RID: 14779 RVA: 0x0010E891 File Offset: 0x0010CA91
		public static ControlBinding[] bindings
		{
			get
			{
				return ControlsSettings._bindings;
			}
		}

		// Token: 0x17000A5E RID: 2654
		// (get) Token: 0x060039BC RID: 14780 RVA: 0x0010E898 File Offset: 0x0010CA98
		public static KeyCode left
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.LEFT].key;
			}
		}

		// Token: 0x17000A5F RID: 2655
		// (get) Token: 0x060039BD RID: 14781 RVA: 0x0010E8AA File Offset: 0x0010CAAA
		public static KeyCode up
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.UP].key;
			}
		}

		// Token: 0x17000A60 RID: 2656
		// (get) Token: 0x060039BE RID: 14782 RVA: 0x0010E8BC File Offset: 0x0010CABC
		public static KeyCode right
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.RIGHT].key;
			}
		}

		// Token: 0x17000A61 RID: 2657
		// (get) Token: 0x060039BF RID: 14783 RVA: 0x0010E8CE File Offset: 0x0010CACE
		public static KeyCode down
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.DOWN].key;
			}
		}

		// Token: 0x17000A62 RID: 2658
		// (get) Token: 0x060039C0 RID: 14784 RVA: 0x0010E8E0 File Offset: 0x0010CAE0
		public static KeyCode jump
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.JUMP].key;
			}
		}

		// Token: 0x17000A63 RID: 2659
		// (get) Token: 0x060039C1 RID: 14785 RVA: 0x0010E8F2 File Offset: 0x0010CAF2
		public static KeyCode leanLeft
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.LEAN_LEFT].key;
			}
		}

		// Token: 0x17000A64 RID: 2660
		// (get) Token: 0x060039C2 RID: 14786 RVA: 0x0010E904 File Offset: 0x0010CB04
		public static KeyCode leanRight
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.LEAN_RIGHT].key;
			}
		}

		// Token: 0x17000A65 RID: 2661
		// (get) Token: 0x060039C3 RID: 14787 RVA: 0x0010E916 File Offset: 0x0010CB16
		public static KeyCode rollLeft
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.ROLL_LEFT].key;
			}
		}

		// Token: 0x17000A66 RID: 2662
		// (get) Token: 0x060039C4 RID: 14788 RVA: 0x0010E928 File Offset: 0x0010CB28
		public static KeyCode rollRight
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.ROLL_RIGHT].key;
			}
		}

		// Token: 0x17000A67 RID: 2663
		// (get) Token: 0x060039C5 RID: 14789 RVA: 0x0010E93A File Offset: 0x0010CB3A
		public static KeyCode pitchUp
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.PITCH_UP].key;
			}
		}

		// Token: 0x17000A68 RID: 2664
		// (get) Token: 0x060039C6 RID: 14790 RVA: 0x0010E94C File Offset: 0x0010CB4C
		public static KeyCode pitchDown
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.PITCH_DOWN].key;
			}
		}

		// Token: 0x17000A69 RID: 2665
		// (get) Token: 0x060039C7 RID: 14791 RVA: 0x0010E95E File Offset: 0x0010CB5E
		public static KeyCode primary
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.PRIMARY].key;
			}
		}

		// Token: 0x17000A6A RID: 2666
		// (get) Token: 0x060039C8 RID: 14792 RVA: 0x0010E970 File Offset: 0x0010CB70
		public static KeyCode yawLeft
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.YAW_LEFT].key;
			}
		}

		// Token: 0x17000A6B RID: 2667
		// (get) Token: 0x060039C9 RID: 14793 RVA: 0x0010E982 File Offset: 0x0010CB82
		public static KeyCode yawRight
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.YAW_RIGHT].key;
			}
		}

		// Token: 0x17000A6C RID: 2668
		// (get) Token: 0x060039CA RID: 14794 RVA: 0x0010E994 File Offset: 0x0010CB94
		public static KeyCode thrustIncrease
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.THRUST_INCREASE].key;
			}
		}

		// Token: 0x17000A6D RID: 2669
		// (get) Token: 0x060039CB RID: 14795 RVA: 0x0010E9A6 File Offset: 0x0010CBA6
		public static KeyCode thrustDecrease
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.THRUST_DECREASE].key;
			}
		}

		// Token: 0x17000A6E RID: 2670
		// (get) Token: 0x060039CC RID: 14796 RVA: 0x0010E9B8 File Offset: 0x0010CBB8
		public static KeyCode locker
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.LOCKER].key;
			}
		}

		// Token: 0x17000A6F RID: 2671
		// (get) Token: 0x060039CD RID: 14797 RVA: 0x0010E9CA File Offset: 0x0010CBCA
		public static KeyCode secondary
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.SECONDARY].key;
			}
		}

		// Token: 0x17000A70 RID: 2672
		// (get) Token: 0x060039CE RID: 14798 RVA: 0x0010E9DC File Offset: 0x0010CBDC
		public static KeyCode reload
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.RELOAD].key;
			}
		}

		// Token: 0x17000A71 RID: 2673
		// (get) Token: 0x060039CF RID: 14799 RVA: 0x0010E9EE File Offset: 0x0010CBEE
		public static KeyCode attach
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.ATTACH].key;
			}
		}

		// Token: 0x17000A72 RID: 2674
		// (get) Token: 0x060039D0 RID: 14800 RVA: 0x0010EA00 File Offset: 0x0010CC00
		public static KeyCode firemode
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.FIREMODE].key;
			}
		}

		// Token: 0x17000A73 RID: 2675
		// (get) Token: 0x060039D1 RID: 14801 RVA: 0x0010EA12 File Offset: 0x0010CC12
		public static KeyCode dashboard
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.DASHBOARD].key;
			}
		}

		// Token: 0x17000A74 RID: 2676
		// (get) Token: 0x060039D2 RID: 14802 RVA: 0x0010EA24 File Offset: 0x0010CC24
		public static KeyCode inventory
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.INVENTORY].key;
			}
		}

		// Token: 0x17000A75 RID: 2677
		// (get) Token: 0x060039D3 RID: 14803 RVA: 0x0010EA36 File Offset: 0x0010CC36
		public static KeyCode crafting
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.CRAFTING].key;
			}
		}

		// Token: 0x17000A76 RID: 2678
		// (get) Token: 0x060039D4 RID: 14804 RVA: 0x0010EA48 File Offset: 0x0010CC48
		public static KeyCode skills
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.SKILLS].key;
			}
		}

		// Token: 0x17000A77 RID: 2679
		// (get) Token: 0x060039D5 RID: 14805 RVA: 0x0010EA5A File Offset: 0x0010CC5A
		public static KeyCode map
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.MAP].key;
			}
		}

		// Token: 0x17000A78 RID: 2680
		// (get) Token: 0x060039D6 RID: 14806 RVA: 0x0010EA6C File Offset: 0x0010CC6C
		public static KeyCode quests
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.QUESTS].key;
			}
		}

		// Token: 0x17000A79 RID: 2681
		// (get) Token: 0x060039D7 RID: 14807 RVA: 0x0010EA7E File Offset: 0x0010CC7E
		public static KeyCode players
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.PLAYERS].key;
			}
		}

		// Token: 0x17000A7A RID: 2682
		// (get) Token: 0x060039D8 RID: 14808 RVA: 0x0010EA90 File Offset: 0x0010CC90
		public static KeyCode voice
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.VOICE].key;
			}
		}

		// Token: 0x17000A7B RID: 2683
		// (get) Token: 0x060039D9 RID: 14809 RVA: 0x0010EAA2 File Offset: 0x0010CCA2
		public static KeyCode interact
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.INTERACT].key;
			}
		}

		// Token: 0x17000A7C RID: 2684
		// (get) Token: 0x060039DA RID: 14810 RVA: 0x0010EAB4 File Offset: 0x0010CCB4
		public static KeyCode crouch
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.CROUCH].key;
			}
		}

		// Token: 0x17000A7D RID: 2685
		// (get) Token: 0x060039DB RID: 14811 RVA: 0x0010EAC6 File Offset: 0x0010CCC6
		public static KeyCode prone
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.PRONE].key;
			}
		}

		// Token: 0x17000A7E RID: 2686
		// (get) Token: 0x060039DC RID: 14812 RVA: 0x0010EAD8 File Offset: 0x0010CCD8
		public static KeyCode stance
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.STANCE].key;
			}
		}

		// Token: 0x17000A7F RID: 2687
		// (get) Token: 0x060039DD RID: 14813 RVA: 0x0010EAEA File Offset: 0x0010CCEA
		public static KeyCode sprint
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.SPRINT].key;
			}
		}

		// Token: 0x17000A80 RID: 2688
		// (get) Token: 0x060039DE RID: 14814 RVA: 0x0010EAFC File Offset: 0x0010CCFC
		public static KeyCode modify
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.MODIFY].key;
			}
		}

		// Token: 0x17000A81 RID: 2689
		// (get) Token: 0x060039DF RID: 14815 RVA: 0x0010EB0E File Offset: 0x0010CD0E
		public static KeyCode snap
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.SNAP].key;
			}
		}

		// Token: 0x17000A82 RID: 2690
		// (get) Token: 0x060039E0 RID: 14816 RVA: 0x0010EB20 File Offset: 0x0010CD20
		public static KeyCode focus
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.FOCUS].key;
			}
		}

		// Token: 0x17000A83 RID: 2691
		// (get) Token: 0x060039E1 RID: 14817 RVA: 0x0010EB32 File Offset: 0x0010CD32
		public static KeyCode ascend
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.ASCEND].key;
			}
		}

		// Token: 0x17000A84 RID: 2692
		// (get) Token: 0x060039E2 RID: 14818 RVA: 0x0010EB44 File Offset: 0x0010CD44
		public static KeyCode descend
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.DESCEND].key;
			}
		}

		// Token: 0x17000A85 RID: 2693
		// (get) Token: 0x060039E3 RID: 14819 RVA: 0x0010EB56 File Offset: 0x0010CD56
		public static KeyCode tool_0
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.TOOL_0].key;
			}
		}

		// Token: 0x17000A86 RID: 2694
		// (get) Token: 0x060039E4 RID: 14820 RVA: 0x0010EB68 File Offset: 0x0010CD68
		public static KeyCode tool_1
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.TOOL_1].key;
			}
		}

		// Token: 0x17000A87 RID: 2695
		// (get) Token: 0x060039E5 RID: 14821 RVA: 0x0010EB7A File Offset: 0x0010CD7A
		public static KeyCode tool_2
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.TOOL_2].key;
			}
		}

		// Token: 0x17000A88 RID: 2696
		// (get) Token: 0x060039E6 RID: 14822 RVA: 0x0010EB8C File Offset: 0x0010CD8C
		public static KeyCode tool_3
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.TOOL_3].key;
			}
		}

		// Token: 0x17000A89 RID: 2697
		// (get) Token: 0x060039E7 RID: 14823 RVA: 0x0010EB9E File Offset: 0x0010CD9E
		public static KeyCode terminal
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.TERMINAL].key;
			}
		}

		// Token: 0x17000A8A RID: 2698
		// (get) Token: 0x060039E8 RID: 14824 RVA: 0x0010EBB0 File Offset: 0x0010CDB0
		public static KeyCode screenshot
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.SCREENSHOT].key;
			}
		}

		// Token: 0x17000A8B RID: 2699
		// (get) Token: 0x060039E9 RID: 14825 RVA: 0x0010EBC2 File Offset: 0x0010CDC2
		public static KeyCode refreshAssets
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.REFRESH_ASSETS].key;
			}
		}

		// Token: 0x17000A8C RID: 2700
		// (get) Token: 0x060039EA RID: 14826 RVA: 0x0010EBD4 File Offset: 0x0010CDD4
		public static KeyCode clipboardDebug
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.CLIPBOARD_DEBUG].key;
			}
		}

		// Token: 0x17000A8D RID: 2701
		// (get) Token: 0x060039EB RID: 14827 RVA: 0x0010EBE6 File Offset: 0x0010CDE6
		public static KeyCode hud
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.HUD].key;
			}
		}

		// Token: 0x17000A8E RID: 2702
		// (get) Token: 0x060039EC RID: 14828 RVA: 0x0010EBF8 File Offset: 0x0010CDF8
		public static KeyCode other
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.OTHER].key;
			}
		}

		// Token: 0x17000A8F RID: 2703
		// (get) Token: 0x060039ED RID: 14829 RVA: 0x0010EC0A File Offset: 0x0010CE0A
		public static KeyCode global
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.GLOBAL].key;
			}
		}

		// Token: 0x17000A90 RID: 2704
		// (get) Token: 0x060039EE RID: 14830 RVA: 0x0010EC1C File Offset: 0x0010CE1C
		public static KeyCode local
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.LOCAL].key;
			}
		}

		// Token: 0x17000A91 RID: 2705
		// (get) Token: 0x060039EF RID: 14831 RVA: 0x0010EC2E File Offset: 0x0010CE2E
		public static KeyCode group
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.GROUP].key;
			}
		}

		// Token: 0x17000A92 RID: 2706
		// (get) Token: 0x060039F0 RID: 14832 RVA: 0x0010EC40 File Offset: 0x0010CE40
		public static KeyCode gesture
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.GESTURE].key;
			}
		}

		// Token: 0x17000A93 RID: 2707
		// (get) Token: 0x060039F1 RID: 14833 RVA: 0x0010EC52 File Offset: 0x0010CE52
		public static KeyCode vision
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.VISION].key;
			}
		}

		// Token: 0x17000A94 RID: 2708
		// (get) Token: 0x060039F2 RID: 14834 RVA: 0x0010EC64 File Offset: 0x0010CE64
		public static KeyCode tactical
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.TACTICAL].key;
			}
		}

		// Token: 0x17000A95 RID: 2709
		// (get) Token: 0x060039F3 RID: 14835 RVA: 0x0010EC76 File Offset: 0x0010CE76
		public static KeyCode perspective
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.PERSPECTIVE].key;
			}
		}

		// Token: 0x17000A96 RID: 2710
		// (get) Token: 0x060039F4 RID: 14836 RVA: 0x0010EC88 File Offset: 0x0010CE88
		public static KeyCode dequip
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.DEQUIP].key;
			}
		}

		// Token: 0x17000A97 RID: 2711
		// (get) Token: 0x060039F5 RID: 14837 RVA: 0x0010EC9A File Offset: 0x0010CE9A
		public static KeyCode inspect
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.INSPECT].key;
			}
		}

		// Token: 0x17000A98 RID: 2712
		// (get) Token: 0x060039F6 RID: 14838 RVA: 0x0010ECAC File Offset: 0x0010CEAC
		public static KeyCode rotate
		{
			get
			{
				return ControlsSettings.bindings[(int)ControlsSettings.ROTATE].key;
			}
		}

		// Token: 0x060039F7 RID: 14839 RVA: 0x0010ECBE File Offset: 0x0010CEBE
		public static KeyCode getPluginKeyCode(int index)
		{
			return ControlsSettings.bindings[(int)ControlsSettings.PLUGIN_0 + index].key;
		}

		// Token: 0x060039F8 RID: 14840 RVA: 0x0010ECD2 File Offset: 0x0010CED2
		public static KeyCode getEquipmentHotbarKeyCode(int index)
		{
			return ControlsSettings.bindings[64 + index].key;
		}

		/// <summary>
		/// When held the cursor is released.
		/// </summary>
		// Token: 0x17000A99 RID: 2713
		// (get) Token: 0x060039F9 RID: 14841 RVA: 0x0010ECE3 File Offset: 0x0010CEE3
		public static KeyCode CustomModal
		{
			get
			{
				return ControlsSettings.bindings[74].key;
			}
		}

		// Token: 0x060039FA RID: 14842 RVA: 0x0010ECF2 File Offset: 0x0010CEF2
		private static bool isTooImportantToMessUp(KeyCode key)
		{
			return key == KeyCode.Mouse0 || key == KeyCode.Mouse1;
		}

		// Token: 0x060039FB RID: 14843 RVA: 0x0010ED0C File Offset: 0x0010CF0C
		public static void bind(byte index, KeyCode key)
		{
			if (index == ControlsSettings.HUD)
			{
				if (ControlsSettings.isTooImportantToMessUp(key))
				{
					key = KeyCode.Home;
				}
			}
			else if (index == ControlsSettings.OTHER)
			{
				if (ControlsSettings.isTooImportantToMessUp(key))
				{
					key = KeyCode.LeftControl;
				}
			}
			else if (index == ControlsSettings.TERMINAL)
			{
				if (ControlsSettings.isTooImportantToMessUp(key))
				{
					key = KeyCode.BackQuote;
				}
			}
			else if (index == ControlsSettings.REFRESH_ASSETS && ControlsSettings.isTooImportantToMessUp(key))
			{
				key = KeyCode.PageUp;
			}
			if (ControlsSettings.bindings[(int)index] == null)
			{
				ControlsSettings.bindings[(int)index] = new ControlBinding(key);
				return;
			}
			ControlsSettings.bindings[(int)index].key = key;
		}

		// Token: 0x060039FC RID: 14844 RVA: 0x0010ED9C File Offset: 0x0010CF9C
		public static void restoreDefaults()
		{
			ControlsSettings.bind(ControlsSettings.LEFT, KeyCode.A);
			ControlsSettings.bind(ControlsSettings.RIGHT, KeyCode.D);
			ControlsSettings.bind(ControlsSettings.UP, KeyCode.W);
			ControlsSettings.bind(ControlsSettings.DOWN, KeyCode.S);
			ControlsSettings.bind(ControlsSettings.JUMP, KeyCode.Space);
			ControlsSettings.bind(ControlsSettings.LEAN_LEFT, KeyCode.Q);
			ControlsSettings.bind(ControlsSettings.LEAN_RIGHT, KeyCode.E);
			ControlsSettings.bind(ControlsSettings.PRIMARY, KeyCode.Mouse0);
			ControlsSettings.bind(ControlsSettings.SECONDARY, KeyCode.Mouse1);
			ControlsSettings.bind(ControlsSettings.INTERACT, KeyCode.F);
			ControlsSettings.bind(ControlsSettings.CROUCH, KeyCode.X);
			ControlsSettings.bind(ControlsSettings.PRONE, KeyCode.Z);
			ControlsSettings.bind(ControlsSettings.STANCE, KeyCode.O);
			ControlsSettings.bind(ControlsSettings.SPRINT, KeyCode.LeftShift);
			ControlsSettings.bind(ControlsSettings.RELOAD, KeyCode.R);
			ControlsSettings.bind(ControlsSettings.ATTACH, KeyCode.T);
			ControlsSettings.bind(ControlsSettings.FIREMODE, KeyCode.V);
			ControlsSettings.bind(ControlsSettings.DASHBOARD, KeyCode.Tab);
			ControlsSettings.bind(ControlsSettings.INVENTORY, KeyCode.G);
			ControlsSettings.bind(ControlsSettings.CRAFTING, KeyCode.Y);
			ControlsSettings.bind(ControlsSettings.SKILLS, KeyCode.U);
			ControlsSettings.bind(ControlsSettings.MAP, KeyCode.M);
			ControlsSettings.bind(ControlsSettings.QUESTS, KeyCode.I);
			ControlsSettings.bind(ControlsSettings.PLAYERS, KeyCode.P);
			ControlsSettings.bind(ControlsSettings.VOICE, KeyCode.LeftAlt);
			ControlsSettings.bind(ControlsSettings.MODIFY, KeyCode.LeftShift);
			ControlsSettings.bind(ControlsSettings.SNAP, KeyCode.LeftControl);
			ControlsSettings.bind(ControlsSettings.FOCUS, KeyCode.F);
			ControlsSettings.bind(ControlsSettings.ASCEND, KeyCode.Q);
			ControlsSettings.bind(ControlsSettings.DESCEND, KeyCode.E);
			ControlsSettings.bind(ControlsSettings.TOOL_0, KeyCode.Q);
			ControlsSettings.bind(ControlsSettings.TOOL_1, KeyCode.W);
			ControlsSettings.bind(ControlsSettings.TOOL_2, KeyCode.E);
			ControlsSettings.bind(ControlsSettings.TOOL_3, KeyCode.R);
			ControlsSettings.bind(ControlsSettings.TERMINAL, KeyCode.BackQuote);
			ControlsSettings.bind(ControlsSettings.SCREENSHOT, KeyCode.Insert);
			ControlsSettings.bind(ControlsSettings.REFRESH_ASSETS, KeyCode.PageUp);
			ControlsSettings.bind(ControlsSettings.CLIPBOARD_DEBUG, KeyCode.PageDown);
			ControlsSettings.bind(ControlsSettings.HUD, KeyCode.Home);
			ControlsSettings.bind(ControlsSettings.OTHER, KeyCode.LeftControl);
			ControlsSettings.bind(ControlsSettings.GLOBAL, KeyCode.J);
			ControlsSettings.bind(ControlsSettings.LOCAL, KeyCode.K);
			ControlsSettings.bind(ControlsSettings.GROUP, KeyCode.L);
			ControlsSettings.bind(ControlsSettings.GESTURE, KeyCode.C);
			ControlsSettings.bind(ControlsSettings.VISION, KeyCode.N);
			ControlsSettings.bind(ControlsSettings.TACTICAL, KeyCode.B);
			ControlsSettings.bind(ControlsSettings.PERSPECTIVE, KeyCode.H);
			ControlsSettings.bind(ControlsSettings.DEQUIP, KeyCode.CapsLock);
			ControlsSettings.bind(ControlsSettings.ROLL_LEFT, KeyCode.LeftArrow);
			ControlsSettings.bind(ControlsSettings.ROLL_RIGHT, KeyCode.RightArrow);
			ControlsSettings.bind(ControlsSettings.PITCH_UP, KeyCode.UpArrow);
			ControlsSettings.bind(ControlsSettings.PITCH_DOWN, KeyCode.DownArrow);
			ControlsSettings.bind(ControlsSettings.YAW_LEFT, KeyCode.A);
			ControlsSettings.bind(ControlsSettings.YAW_RIGHT, KeyCode.D);
			ControlsSettings.bind(ControlsSettings.THRUST_INCREASE, KeyCode.W);
			ControlsSettings.bind(ControlsSettings.THRUST_DECREASE, KeyCode.S);
			ControlsSettings.bind(ControlsSettings.LOCKER, KeyCode.O);
			ControlsSettings.bind(ControlsSettings.INSPECT, KeyCode.F);
			ControlsSettings.bind(ControlsSettings.ROTATE, KeyCode.R);
			ControlsSettings.bind(ControlsSettings.PLUGIN_0, KeyCode.Comma);
			ControlsSettings.bind(ControlsSettings.PLUGIN_1, KeyCode.Period);
			ControlsSettings.bind(ControlsSettings.PLUGIN_2, KeyCode.Slash);
			ControlsSettings.bind(ControlsSettings.PLUGIN_3, KeyCode.Semicolon);
			ControlsSettings.bind(ControlsSettings.PLUGIN_4, KeyCode.Quote);
			ControlsSettings.bind(64, KeyCode.Alpha1);
			ControlsSettings.bind(65, KeyCode.Alpha2);
			ControlsSettings.bind(66, KeyCode.Alpha3);
			ControlsSettings.bind(67, KeyCode.Alpha4);
			ControlsSettings.bind(68, KeyCode.Alpha5);
			ControlsSettings.bind(69, KeyCode.Alpha6);
			ControlsSettings.bind(70, KeyCode.Alpha7);
			ControlsSettings.bind(71, KeyCode.Alpha8);
			ControlsSettings.bind(72, KeyCode.Alpha9);
			ControlsSettings.bind(73, KeyCode.Alpha0);
			ControlsSettings.bind(74, KeyCode.Keypad0);
			ControlsSettings.aiming = EControlMode.HOLD;
			ControlsSettings.crouching = EControlMode.TOGGLE;
			ControlsSettings.proning = EControlMode.TOGGLE;
			ControlsSettings.sprinting = EControlMode.HOLD;
			ControlsSettings.leaning = EControlMode.HOLD;
			ControlsSettings.voiceMode = EControlMode.HOLD;
			ControlsSettings.sensitivityScalingMode = ESensitivityScalingMode.ProjectionRatio;
			ControlsSettings.projectionRatioCoefficient = 1f;
			ControlsSettings.mouseAimSensitivity = 0.2f;
			ControlsSettings.invert = false;
			ControlsSettings.invertFlight = false;
		}

		// Token: 0x060039FD RID: 14845 RVA: 0x0010F18C File Offset: 0x0010D38C
		public static void load()
		{
			ControlsSettings.restoreDefaults();
			if (ReadWrite.fileExists("/Controls.dat", true))
			{
				Block block = ReadWrite.readBlock("/Controls.dat", true, 0);
				if (block != null)
				{
					byte b = block.readByte();
					if (b > 10)
					{
						ControlsSettings.mouseAimSensitivity = block.readSingle();
						if (b < 16)
						{
							ControlsSettings.mouseAimSensitivity = 0.2f;
						}
						else if (b < 18)
						{
							ControlsSettings.mouseAimSensitivity *= 0.2f;
						}
						ControlsSettings.invert = block.readBoolean();
						if (b > 13)
						{
							ControlsSettings.invertFlight = block.readBoolean();
						}
						else
						{
							ControlsSettings.invertFlight = false;
						}
						if (b > 11)
						{
							ControlsSettings.aiming = (EControlMode)block.readByte();
							ControlsSettings.crouching = (EControlMode)block.readByte();
							ControlsSettings.proning = (EControlMode)block.readByte();
						}
						else
						{
							ControlsSettings.aiming = EControlMode.HOLD;
							ControlsSettings.crouching = EControlMode.TOGGLE;
							ControlsSettings.proning = EControlMode.TOGGLE;
						}
						if (b > 12)
						{
							ControlsSettings.sprinting = (EControlMode)block.readByte();
						}
						else
						{
							ControlsSettings.sprinting = EControlMode.HOLD;
						}
						if (b > 14)
						{
							ControlsSettings.leaning = (EControlMode)block.readByte();
						}
						else
						{
							ControlsSettings.leaning = EControlMode.HOLD;
						}
						byte b2 = block.readByte();
						for (byte b3 = 0; b3 < b2; b3 += 1)
						{
							if ((int)b3 >= ControlsSettings.bindings.Length)
							{
								block.readByte();
							}
							else
							{
								ushort key = block.readUInt16();
								ControlsSettings.bind(b3, (KeyCode)key);
							}
						}
						if (b < 17)
						{
							ControlsSettings.bind(ControlsSettings.DEQUIP, KeyCode.CapsLock);
						}
						if (b < 19)
						{
							ControlsSettings.sensitivityScalingMode = ESensitivityScalingMode.ProjectionRatio;
						}
						else
						{
							ControlsSettings.sensitivityScalingMode = (ESensitivityScalingMode)block.readByte();
						}
						if (b < 20)
						{
							ControlsSettings.projectionRatioCoefficient = 1f;
						}
						else
						{
							ControlsSettings.projectionRatioCoefficient = block.readSingle();
						}
						if (b >= 21)
						{
							ControlsSettings.voiceMode = (EControlMode)block.readByte();
							return;
						}
						ControlsSettings.voiceMode = EControlMode.HOLD;
					}
				}
			}
		}

		// Token: 0x060039FE RID: 14846 RVA: 0x0010F324 File Offset: 0x0010D524
		public static void save()
		{
			Block block = new Block();
			block.writeByte(21);
			block.writeSingle(ControlsSettings.mouseAimSensitivity);
			block.writeBoolean(ControlsSettings.invert);
			block.writeBoolean(ControlsSettings.invertFlight);
			block.writeByte((byte)ControlsSettings.aiming);
			block.writeByte((byte)ControlsSettings.crouching);
			block.writeByte((byte)ControlsSettings.proning);
			block.writeByte((byte)ControlsSettings.sprinting);
			block.writeByte((byte)ControlsSettings.leaning);
			block.writeByte((byte)ControlsSettings.bindings.Length);
			byte b = 0;
			while ((int)b < ControlsSettings.bindings.Length)
			{
				ControlBinding controlBinding = ControlsSettings.bindings[(int)b];
				block.writeUInt16((ushort)controlBinding.key);
				b += 1;
			}
			block.writeByte((byte)ControlsSettings.sensitivityScalingMode);
			block.writeSingle(ControlsSettings.projectionRatioCoefficient);
			block.writeByte((byte)ControlsSettings.voiceMode);
			ReadWrite.writeBlock("/Controls.dat", true, block);
		}

		// Token: 0x060039FF RID: 14847 RVA: 0x0010F404 File Offset: 0x0010D604
		static ControlsSettings()
		{
			for (int i = 0; i < ControlsSettings.bindings.Length; i++)
			{
				ControlsSettings.bindings[i] = new ControlBinding(KeyCode.F);
			}
			ControlsSettings.PLUGIN_KEY_TOKENS = new string[(int)ControlsSettings.NUM_PLUGIN_KEYS];
			for (int j = 0; j < (int)ControlsSettings.NUM_PLUGIN_KEYS; j++)
			{
				string text = string.Format("<plugin_{0}/>", j);
				ControlsSettings.PLUGIN_KEY_TOKENS[j] = text;
			}
		}

		// Token: 0x04002250 RID: 8784
		private const byte SAVEDATA_VERSION_ADDED_SENSITIVITY_SCALING_MODE = 19;

		// Token: 0x04002251 RID: 8785
		private const byte SAVEDATA_VERSION_ADDED_SCALING_COEFFICIENT = 20;

		// Token: 0x04002252 RID: 8786
		private const byte SAVEDATA_VERSION_ADDED_VOICE_TOGGLE = 21;

		// Token: 0x04002253 RID: 8787
		private const byte SAVEDATA_VERSION_NEWEST = 21;

		// Token: 0x04002254 RID: 8788
		public static readonly byte SAVEDATA_VERSION = 21;

		// Token: 0x04002255 RID: 8789
		public static readonly byte LEFT = 0;

		// Token: 0x04002256 RID: 8790
		public static readonly byte RIGHT = 1;

		// Token: 0x04002257 RID: 8791
		public static readonly byte UP = 2;

		// Token: 0x04002258 RID: 8792
		public static readonly byte DOWN = 3;

		// Token: 0x04002259 RID: 8793
		public static readonly byte JUMP = 4;

		// Token: 0x0400225A RID: 8794
		public static readonly byte LEAN_LEFT = 5;

		// Token: 0x0400225B RID: 8795
		public static readonly byte LEAN_RIGHT = 6;

		// Token: 0x0400225C RID: 8796
		public static readonly byte PRIMARY = 7;

		// Token: 0x0400225D RID: 8797
		public static readonly byte SECONDARY = 8;

		// Token: 0x0400225E RID: 8798
		public static readonly byte INTERACT = 9;

		// Token: 0x0400225F RID: 8799
		public static readonly byte CROUCH = 10;

		// Token: 0x04002260 RID: 8800
		public static readonly byte PRONE = 11;

		// Token: 0x04002261 RID: 8801
		public static readonly byte SPRINT = 12;

		// Token: 0x04002262 RID: 8802
		public static readonly byte RELOAD = 13;

		// Token: 0x04002263 RID: 8803
		public static readonly byte ATTACH = 14;

		// Token: 0x04002264 RID: 8804
		public static readonly byte FIREMODE = 15;

		// Token: 0x04002265 RID: 8805
		public static readonly byte DASHBOARD = 16;

		// Token: 0x04002266 RID: 8806
		public static readonly byte INVENTORY = 17;

		// Token: 0x04002267 RID: 8807
		public static readonly byte CRAFTING = 18;

		// Token: 0x04002268 RID: 8808
		public static readonly byte SKILLS = 19;

		// Token: 0x04002269 RID: 8809
		public static readonly byte MAP = 20;

		// Token: 0x0400226A RID: 8810
		public static readonly byte QUESTS = 54;

		// Token: 0x0400226B RID: 8811
		public static readonly byte PLAYERS = 21;

		// Token: 0x0400226C RID: 8812
		public static readonly byte VOICE = 22;

		// Token: 0x0400226D RID: 8813
		public static readonly byte MODIFY = 23;

		// Token: 0x0400226E RID: 8814
		public static readonly byte SNAP = 24;

		// Token: 0x0400226F RID: 8815
		public static readonly byte FOCUS = 25;

		// Token: 0x04002270 RID: 8816
		public static readonly byte ASCEND = 51;

		// Token: 0x04002271 RID: 8817
		public static readonly byte DESCEND = 52;

		// Token: 0x04002272 RID: 8818
		public static readonly byte TOOL_0 = 26;

		// Token: 0x04002273 RID: 8819
		public static readonly byte TOOL_1 = 27;

		// Token: 0x04002274 RID: 8820
		public static readonly byte TOOL_2 = 28;

		// Token: 0x04002275 RID: 8821
		public static readonly byte TOOL_3 = 50;

		// Token: 0x04002276 RID: 8822
		public static readonly byte TERMINAL = 55;

		// Token: 0x04002277 RID: 8823
		public static readonly byte SCREENSHOT = 56;

		// Token: 0x04002278 RID: 8824
		public static readonly byte REFRESH_ASSETS = 57;

		// Token: 0x04002279 RID: 8825
		public static readonly byte CLIPBOARD_DEBUG = 58;

		// Token: 0x0400227A RID: 8826
		public static readonly byte HUD = 29;

		// Token: 0x0400227B RID: 8827
		public static readonly byte OTHER = 30;

		// Token: 0x0400227C RID: 8828
		public static readonly byte GLOBAL = 31;

		// Token: 0x0400227D RID: 8829
		public static readonly byte LOCAL = 32;

		// Token: 0x0400227E RID: 8830
		public static readonly byte GROUP = 33;

		// Token: 0x0400227F RID: 8831
		public static readonly byte GESTURE = 34;

		// Token: 0x04002280 RID: 8832
		public static readonly byte VISION = 35;

		// Token: 0x04002281 RID: 8833
		public static readonly byte TACTICAL = 36;

		// Token: 0x04002282 RID: 8834
		public static readonly byte PERSPECTIVE = 37;

		// Token: 0x04002283 RID: 8835
		public static readonly byte DEQUIP = 38;

		// Token: 0x04002284 RID: 8836
		public static readonly byte STANCE = 39;

		// Token: 0x04002285 RID: 8837
		public static readonly byte ROLL_LEFT = 40;

		// Token: 0x04002286 RID: 8838
		public static readonly byte ROLL_RIGHT = 41;

		// Token: 0x04002287 RID: 8839
		public static readonly byte PITCH_UP = 42;

		// Token: 0x04002288 RID: 8840
		public static readonly byte PITCH_DOWN = 43;

		// Token: 0x04002289 RID: 8841
		public static readonly byte YAW_LEFT = 44;

		// Token: 0x0400228A RID: 8842
		public static readonly byte YAW_RIGHT = 45;

		// Token: 0x0400228B RID: 8843
		public static readonly byte THRUST_INCREASE = 46;

		// Token: 0x0400228C RID: 8844
		public static readonly byte THRUST_DECREASE = 47;

		// Token: 0x0400228D RID: 8845
		public static readonly byte LOCKER = 53;

		// Token: 0x0400228E RID: 8846
		public static readonly byte INSPECT = 48;

		// Token: 0x0400228F RID: 8847
		public static readonly byte ROTATE = 49;

		// Token: 0x04002290 RID: 8848
		public static readonly byte PLUGIN_0 = 59;

		// Token: 0x04002291 RID: 8849
		public static readonly byte PLUGIN_1 = 60;

		// Token: 0x04002292 RID: 8850
		public static readonly byte PLUGIN_2 = 61;

		// Token: 0x04002293 RID: 8851
		public static readonly byte PLUGIN_3 = 62;

		// Token: 0x04002294 RID: 8852
		public static readonly byte PLUGIN_4 = 63;

		// Token: 0x04002295 RID: 8853
		public static readonly byte NUM_PLUGIN_KEYS = 5;

		// Token: 0x04002296 RID: 8854
		public static readonly string[] PLUGIN_KEY_TOKENS;

		// Token: 0x04002297 RID: 8855
		public const byte ITEM_0 = 64;

		// Token: 0x04002298 RID: 8856
		public const byte ITEM_1 = 65;

		// Token: 0x04002299 RID: 8857
		public const byte ITEM_2 = 66;

		// Token: 0x0400229A RID: 8858
		public const byte ITEM_3 = 67;

		// Token: 0x0400229B RID: 8859
		public const byte ITEM_4 = 68;

		// Token: 0x0400229C RID: 8860
		public const byte ITEM_5 = 69;

		// Token: 0x0400229D RID: 8861
		public const byte ITEM_6 = 70;

		// Token: 0x0400229E RID: 8862
		public const byte ITEM_7 = 71;

		// Token: 0x0400229F RID: 8863
		public const byte ITEM_8 = 72;

		// Token: 0x040022A0 RID: 8864
		public const byte ITEM_9 = 73;

		// Token: 0x040022A1 RID: 8865
		public const byte NUM_ITEM_HOTBAR_KEYS = 10;

		/// <summary>
		/// When held the cursor is released.
		/// </summary>
		// Token: 0x040022A2 RID: 8866
		public const byte CUSTOM_MODAL = 74;

		/// <summary>
		/// Multiplier for Input.GetAxis("mouse_x") and Input.GetAxis("mouse_y")
		/// </summary>
		// Token: 0x040022A3 RID: 8867
		public static float mouseAimSensitivity;

		// Token: 0x040022A4 RID: 8868
		public static bool invert;

		// Token: 0x040022A5 RID: 8869
		public static bool invertFlight;

		// Token: 0x040022A6 RID: 8870
		public static EControlMode aiming;

		// Token: 0x040022A7 RID: 8871
		public static EControlMode crouching;

		// Token: 0x040022A8 RID: 8872
		public static EControlMode proning;

		// Token: 0x040022A9 RID: 8873
		public static EControlMode sprinting;

		// Token: 0x040022AA RID: 8874
		public static EControlMode leaning;

		// Token: 0x040022AB RID: 8875
		public static EControlMode voiceMode;

		// Token: 0x040022AC RID: 8876
		public static ESensitivityScalingMode sensitivityScalingMode;

		// Token: 0x040022AD RID: 8877
		public static float projectionRatioCoefficient;

		// Token: 0x040022AE RID: 8878
		private static ControlBinding[] _bindings = new ControlBinding[75];
	}
}
