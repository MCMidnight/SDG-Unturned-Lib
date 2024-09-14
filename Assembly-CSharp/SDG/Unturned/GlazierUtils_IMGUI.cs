using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000174 RID: 372
	internal static class GlazierUtils_IMGUI
	{
		// Token: 0x060009D0 RID: 2512 RVA: 0x0002140A File Offset: 0x0001F60A
		public static int getScaledFontSize(int originalFontSize)
		{
			return Mathf.CeilToInt((float)originalFontSize * GraphicsSettings.userInterfaceScale);
		}

		// Token: 0x060009D1 RID: 2513 RVA: 0x0002141C File Offset: 0x0001F61C
		public static void drawAngledImageTexture(Rect area, Texture texture, float angle, Color color)
		{
			if (texture != null)
			{
				if (!GUI.enabled)
				{
					color.a *= 0.5f;
				}
				GUI.color = color;
				Matrix4x4 matrix = GUI.matrix;
				GUIUtility.RotateAroundPivot(angle, area.center);
				GUI.DrawTexture(area, texture, 0);
				GUI.matrix = matrix;
				GUI.color = Color.white;
			}
		}

		// Token: 0x060009D2 RID: 2514 RVA: 0x00021478 File Offset: 0x0001F678
		public static void drawImageTexture(Rect area, Texture texture, Color color)
		{
			if (texture != null)
			{
				if (!GUI.enabled)
				{
					color.a *= 0.5f;
				}
				GUI.color = color;
				GUI.DrawTexture(area, texture, 0);
				GUI.color = Color.white;
			}
		}

		// Token: 0x060009D3 RID: 2515 RVA: 0x000214B4 File Offset: 0x0001F6B4
		public static void drawTile(Rect area, Texture texture, Color color)
		{
			if (texture != null)
			{
				if (!GUI.enabled)
				{
					color.a *= 0.5f;
				}
				GUI.color = color;
				float userInterfaceScale = GraphicsSettings.userInterfaceScale;
				GUI.DrawTextureWithTexCoords(area, texture, new Rect(0f, 0f, area.width / (float)texture.width / userInterfaceScale, area.height / (float)texture.height / userInterfaceScale));
				GUI.color = Color.white;
			}
		}

		// Token: 0x060009D4 RID: 2516 RVA: 0x0002152F File Offset: 0x0001F72F
		public static void drawSliced(Rect area, Texture texture, Color color, GUIStyle style)
		{
			if (texture != null)
			{
				if (!GUI.enabled)
				{
					color.a *= 0.5f;
				}
				GUI.backgroundColor = color;
				GUI.Box(area, string.Empty, style);
				GUI.color = Color.white;
			}
		}

		// Token: 0x060009D5 RID: 2517 RVA: 0x0002156D File Offset: 0x0001F76D
		public static bool drawToggle(Rect area, Color color, bool state, GUIContent content)
		{
			GUI.backgroundColor = color;
			state = GUI.Toggle(area, state, content);
			return state;
		}

		// Token: 0x060009D6 RID: 2518 RVA: 0x00021580 File Offset: 0x0001F780
		public static bool drawButton(Rect area, Color color)
		{
			if (GlazierUtils_IMGUI.allowInput)
			{
				GUI.backgroundColor = color;
				return GUI.Button(area, "");
			}
			GlazierUtils_IMGUI.drawBox(area, color);
			return false;
		}

		// Token: 0x060009D7 RID: 2519 RVA: 0x000215A3 File Offset: 0x0001F7A3
		public static void drawBox(Rect area, Color color)
		{
			GUI.backgroundColor = color;
			GUI.Box(area, "");
		}

		// Token: 0x060009D8 RID: 2520 RVA: 0x000215B8 File Offset: 0x0001F7B8
		public static void drawLabel(Rect area, FontStyle fontStyle, TextAnchor fontAlignment, int fontSize, GUIContent shadowContent, Color color, GUIContent content, ETextContrastContext shadowStyle)
		{
			GUI.skin.label.fontStyle = fontStyle;
			GUI.skin.label.alignment = fontAlignment;
			GUI.skin.label.fontSize = GlazierUtils_IMGUI.getScaledFontSize(fontSize);
			bool richText = GUI.skin.label.richText;
			GUI.skin.label.richText = (shadowContent != null);
			if (shadowContent == null)
			{
				GlazierUtils_IMGUI.drawLabelOutline(area, content, SleekShadowStyle.ContextToStyle(shadowStyle), color.a);
			}
			else
			{
				GlazierUtils_IMGUI.drawLabelOutline(area, shadowContent, SleekShadowStyle.ContextToStyle(shadowStyle), color.a);
			}
			GUI.contentColor = color;
			GUI.Label(area, content);
			GUI.skin.label.richText = richText;
		}

		// Token: 0x060009D9 RID: 2521 RVA: 0x00021670 File Offset: 0x0001F870
		public static void drawLabel(Rect area, FontStyle fontStyle, TextAnchor fontAlignment, int fontSize, bool isRich, Color color, string text, ETextContrastContext shadowStyle)
		{
			GUI.skin.label.fontStyle = fontStyle;
			GUI.skin.label.alignment = fontAlignment;
			GUI.skin.label.fontSize = GlazierUtils_IMGUI.getScaledFontSize(fontSize);
			if (isRich)
			{
				bool richText = GUI.skin.label.richText;
				GUI.skin.label.richText = isRich;
				GUI.Label(area, text);
				GUI.skin.label.richText = richText;
				return;
			}
			GlazierUtils_IMGUI.drawLabelOutline(area, text, SleekShadowStyle.ContextToStyle(shadowStyle), color.a);
			GUI.contentColor = color;
			GUI.Label(area, text);
		}

		// Token: 0x060009DA RID: 2522 RVA: 0x00021718 File Offset: 0x0001F918
		public static string drawField(Rect area, FontStyle fontStyle, TextAnchor fontAlignment, int fontSize, Color color_0, Color color_1, string text, int maxLength, bool multiline, ETextContrastContext shadowStyle)
		{
			return GlazierUtils_IMGUI.DrawTextInputField(area, fontStyle, fontAlignment, fontSize, color_0, color_1, text, maxLength, string.Empty, multiline, shadowStyle);
		}

		// Token: 0x060009DB RID: 2523 RVA: 0x00021740 File Offset: 0x0001F940
		public static string DrawTextInputField(Rect area, FontStyle fontStyle, TextAnchor fontAlignment, int fontSize, Color color_0, Color color_1, string text, int maxLength, string hint, bool multiline, ETextContrastContext shadowStyle)
		{
			GUI.skin.textArea.fontStyle = fontStyle;
			GUI.skin.textArea.alignment = fontAlignment;
			GUI.skin.textArea.fontSize = GlazierUtils_IMGUI.getScaledFontSize(fontSize);
			GUI.skin.textField.fontStyle = fontStyle;
			GUI.skin.textField.alignment = fontAlignment;
			GUI.skin.textField.fontSize = GlazierUtils_IMGUI.getScaledFontSize(fontSize);
			GUI.backgroundColor = color_0;
			GUI.contentColor = color_1;
			if (GlazierUtils_IMGUI.allowInput)
			{
				if (text == null)
				{
					text = string.Empty;
				}
				if (maxLength > 0)
				{
					if (multiline)
					{
						text = GUI.TextArea(area, text, maxLength);
					}
					else
					{
						text = GUI.TextField(area, text, maxLength);
					}
				}
				else if (multiline)
				{
					text = GUI.TextArea(area, text);
				}
				else
				{
					text = GUI.TextField(area, text);
				}
				if (text.Length < 1)
				{
					GlazierUtils_IMGUI.drawLabel(area, fontStyle, fontAlignment, fontSize, false, color_1 * 0.5f, hint, shadowStyle);
				}
				return text;
			}
			GlazierUtils_IMGUI.drawBox(area, color_0);
			GlazierUtils_IMGUI.drawLabel(area, fontStyle, fontAlignment, fontSize, false, color_1, text, shadowStyle);
			return text;
		}

		// Token: 0x060009DC RID: 2524 RVA: 0x00021858 File Offset: 0x0001FA58
		public static string DrawPasswordField(Rect area, FontStyle fontStyle, TextAnchor fontAlignment, int fontSize, Color color_0, Color color_1, string text, int maxLength, string hint, char replace, ETextContrastContext shadowStyle)
		{
			GUI.skin.textField.fontStyle = fontStyle;
			GUI.skin.textField.alignment = fontAlignment;
			GUI.skin.textField.fontSize = GlazierUtils_IMGUI.getScaledFontSize(fontSize);
			GUI.backgroundColor = color_0;
			GUI.contentColor = color_1;
			if (GlazierUtils_IMGUI.allowInput)
			{
				if (text == null)
				{
					text = string.Empty;
				}
				if (maxLength > 0)
				{
					text = GUI.PasswordField(area, text, replace, maxLength);
				}
				else
				{
					text = GUI.PasswordField(area, text, replace);
				}
				if (text.Length < 1)
				{
					GlazierUtils_IMGUI.drawLabel(area, fontStyle, fontAlignment, fontSize, false, color_1 * 0.5f, hint, shadowStyle);
				}
				return text;
			}
			GlazierUtils_IMGUI.drawBox(area, color_0);
			string text2 = string.Empty;
			if (text != null)
			{
				for (int i = 0; i < text.Length; i++)
				{
					text2 += replace.ToString();
				}
			}
			GlazierUtils_IMGUI.drawLabel(area, fontStyle, fontAlignment, fontSize, false, color_1, text2, shadowStyle);
			return text;
		}

		// Token: 0x060009DD RID: 2525 RVA: 0x00021945 File Offset: 0x0001FB45
		public static float drawSlider(Rect area, ESleekOrientation orientation, float state, float size, Color color)
		{
			GUI.backgroundColor = color;
			if (orientation == null)
			{
				state = GUI.HorizontalScrollbar(area, state, size, 0f, 1f);
			}
			else
			{
				state = GUI.VerticalScrollbar(area, state, size, 0f, 1f);
			}
			return state;
		}

		// Token: 0x060009DE RID: 2526 RVA: 0x0002197C File Offset: 0x0001FB7C
		private static void drawLabelOutline(Rect area, GUIContent content, Vector2[] offsets, float magnitude)
		{
			foreach (Vector2 a in offsets)
			{
				GUI.Label(new Rect(area.position + a * magnitude, area.size), content);
			}
		}

		/// <summary>
		/// Helper for drawing label outline/shadow so that we can easily change it.
		/// </summary>
		// Token: 0x060009DF RID: 2527 RVA: 0x000219C8 File Offset: 0x0001FBC8
		private static void drawLabelOutline(Rect area, GUIContent content, ETextContrastStyle shadowStyle, float alpha)
		{
			Color shadowColor = SleekCustomization.shadowColor;
			switch (shadowStyle)
			{
			case 0:
				return;
			case 1:
				shadowColor.a = 0.5f * alpha;
				GUI.contentColor = shadowColor;
				GlazierUtils_IMGUI.drawLabelOutline(area, content, GlazierUtils_IMGUI.outlineOffsets_4way, 1f);
				return;
			case 2:
			{
				shadowColor.a = 0.5f * alpha;
				GUI.contentColor = shadowColor;
				float num = area.x;
				area.x = num + 1f;
				num = area.y;
				area.y = num + 1f;
				GUI.Label(area, content);
				return;
			}
			case 3:
				shadowColor.a = 0.5f * alpha;
				GUI.contentColor = shadowColor;
				GlazierUtils_IMGUI.drawLabelOutline(area, content, GlazierUtils_IMGUI.outlineOffsets_8way, 2f);
				shadowColor.a = 1f * alpha;
				GUI.contentColor = shadowColor;
				GlazierUtils_IMGUI.drawLabelOutline(area, content, GlazierUtils_IMGUI.outlineOffsets_8way, 1f);
				return;
			default:
				return;
			}
		}

		// Token: 0x060009E0 RID: 2528 RVA: 0x00021AAC File Offset: 0x0001FCAC
		private static void drawLabelOutline(Rect area, string text, Vector2[] offsets, float magnitude)
		{
			foreach (Vector2 a in offsets)
			{
				GUI.Label(new Rect(area.position + a * magnitude, area.size), text);
			}
		}

		/// <summary>
		/// Helper for drawing label outline/shadow so that we can easily change it.
		/// </summary>
		// Token: 0x060009E1 RID: 2529 RVA: 0x00021AF8 File Offset: 0x0001FCF8
		private static void drawLabelOutline(Rect area, string text, ETextContrastStyle shadowStyle, float alpha)
		{
			Color shadowColor = SleekCustomization.shadowColor;
			switch (shadowStyle)
			{
			case 0:
				return;
			case 1:
				shadowColor.a = 0.5f * alpha;
				GUI.contentColor = shadowColor;
				GlazierUtils_IMGUI.drawLabelOutline(area, text, GlazierUtils_IMGUI.outlineOffsets_4way, 1f);
				return;
			case 2:
			{
				shadowColor.a = 0.5f * alpha;
				GUI.contentColor = shadowColor;
				float num = area.x;
				area.x = num + 1f;
				num = area.y;
				area.y = num + 1f;
				GUI.Label(area, text);
				return;
			}
			case 3:
				shadowColor.a = 0.5f * alpha;
				GUI.contentColor = shadowColor;
				GlazierUtils_IMGUI.drawLabelOutline(area, text, GlazierUtils_IMGUI.outlineOffsets_8way, 2f);
				shadowColor.a = 1f * alpha;
				GUI.contentColor = shadowColor;
				GlazierUtils_IMGUI.drawLabelOutline(area, text, GlazierUtils_IMGUI.outlineOffsets_8way, 1f);
				return;
			default:
				return;
			}
		}

		// Token: 0x060009E2 RID: 2530 RVA: 0x00021BD9 File Offset: 0x0001FDD9
		public static int GetFontSize(ESleekFontSize fontSize)
		{
			switch (fontSize)
			{
			case 0:
				return 8;
			case 1:
				return 10;
			case 3:
				return 14;
			case 4:
				return 20;
			case 5:
				return 50;
			}
			return 12;
		}

		// Token: 0x060009E3 RID: 2531 RVA: 0x00021C0B File Offset: 0x0001FE0B
		public static string CreateUniqueControlName()
		{
			GlazierUtils_IMGUI.controlNameCounter++;
			return "Glazier" + GlazierUtils_IMGUI.controlNameCounter.ToString();
		}

		// Token: 0x040003D0 RID: 976
		public static bool allowInput = true;

		// Token: 0x040003D1 RID: 977
		private static Vector2[] outlineOffsets_4way = new Vector2[]
		{
			new Vector2(0f, 1f),
			new Vector2(1f, 0f),
			new Vector2(0f, -1f),
			new Vector2(-1f, 0f)
		};

		// Token: 0x040003D2 RID: 978
		private static Vector2[] outlineOffsets_8way = new Vector2[]
		{
			new Vector2(0f, 1f),
			new Vector2(0.707f, 0.707f),
			new Vector2(1f, 0f),
			new Vector2(0.707f, -0.707f),
			new Vector2(0f, -1f),
			new Vector2(-0.707f, -0.707f),
			new Vector2(-1f, 0f),
			new Vector2(-0.707f, 0.707f)
		};

		// Token: 0x040003D3 RID: 979
		private static int controlNameCounter = -1;
	}
}
