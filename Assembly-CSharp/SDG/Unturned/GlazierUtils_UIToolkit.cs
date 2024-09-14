using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SDG.Unturned
{
	// Token: 0x020001A9 RID: 425
	internal static class GlazierUtils_UIToolkit
	{
		/// <summary>
		/// By default, clickable only responds to LeftMouse without the Control modifier.
		/// Unturned (currently) filters left/right mouse and modifiers outside Glazier,
		/// so add activators for left/right and control modifier to all clickables.
		/// </summary>
		// Token: 0x06000D23 RID: 3363 RVA: 0x0002BCB8 File Offset: 0x00029EB8
		public static void AddClickableActivators(Clickable clickable)
		{
			List<ManipulatorActivationFilter> activators = clickable.activators;
			ManipulatorActivationFilter manipulatorActivationFilter = default(ManipulatorActivationFilter);
			manipulatorActivationFilter.button = 0;
			manipulatorActivationFilter.modifiers = 2;
			activators.Add(manipulatorActivationFilter);
			List<ManipulatorActivationFilter> activators2 = clickable.activators;
			manipulatorActivationFilter = default(ManipulatorActivationFilter);
			manipulatorActivationFilter.button = 1;
			activators2.Add(manipulatorActivationFilter);
			List<ManipulatorActivationFilter> activators3 = clickable.activators;
			manipulatorActivationFilter = default(ManipulatorActivationFilter);
			manipulatorActivationFilter.button = 1;
			manipulatorActivationFilter.modifiers = 2;
			activators3.Add(manipulatorActivationFilter);
		}

		// Token: 0x06000D24 RID: 3364 RVA: 0x0002BD2C File Offset: 0x00029F2C
		public static void ApplyTextContrast(IStyle style, ETextContrastContext contrastContext, float alpha)
		{
			switch (SleekShadowStyle.ContextToStyle(contrastContext))
			{
			default:
				style.textShadow = 1;
				style.unityTextOutlineColor = 1;
				style.unityTextOutlineWidth = 1;
				return;
			case 1:
			{
				TextShadow textShadow = default(TextShadow);
				textShadow.color = SleekCustomization.shadowColor.WithAlpha(alpha);
				textShadow.offset = new Vector2(0f, 0f);
				textShadow.blurRadius = 1.5f;
				style.textShadow = textShadow;
				style.unityTextOutlineColor = 1;
				style.unityTextOutlineWidth = 1;
				return;
			}
			case 2:
			{
				TextShadow textShadow = default(TextShadow);
				textShadow.color = SleekCustomization.shadowColor.WithAlpha(alpha);
				textShadow.offset = new Vector2(0f, 1f);
				textShadow.blurRadius = 1f;
				style.textShadow = textShadow;
				style.unityTextOutlineColor = 1;
				style.unityTextOutlineWidth = 1;
				return;
			}
			case 3:
			{
				TextShadow textShadow = default(TextShadow);
				textShadow.color = Color.black;
				textShadow.offset = new Vector2(0f, 1f);
				textShadow.blurRadius = 2f;
				style.textShadow = textShadow;
				style.unityTextOutlineColor = SleekCustomization.shadowColor.WithAlpha(alpha * 0.25f);
				style.unityTextOutlineWidth = 0.25f;
				return;
			}
			}
		}

		// Token: 0x06000D25 RID: 3365 RVA: 0x0002BEAC File Offset: 0x0002A0AC
		public static StyleLength GetFontSize(ESleekFontSize fontSize)
		{
			switch (fontSize)
			{
			case 0:
				return 8f;
			case 1:
				return 10f;
			case 3:
				return 14f;
			case 4:
				return 20f;
			case 5:
				return 50f;
			}
			return 1;
		}

		/// <summary>
		/// USS best practices mentions inline styles have a higher memory overhead, so we
		/// only apply an inline value if it doesn't match the default :root font style.
		/// </summary>
		// Token: 0x06000D26 RID: 3366 RVA: 0x0002BF16 File Offset: 0x0002A116
		public static StyleEnum<FontStyle> GetFontStyle(FontStyle fontStyle)
		{
			if (fontStyle == null)
			{
				return 1;
			}
			return fontStyle;
		}

		/// <summary>
		/// USS best practices mentions inline styles have a higher memory overhead, so we
		/// only apply an inline value if it doesn't match the default :root text alignment.
		/// </summary>
		// Token: 0x06000D27 RID: 3367 RVA: 0x0002BF28 File Offset: 0x0002A128
		public static StyleEnum<TextAnchor> GetTextAlignment(TextAnchor textAlignment)
		{
			if (textAlignment == 4)
			{
				return 1;
			}
			return textAlignment;
		}
	}
}
