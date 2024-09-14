using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000811 RID: 2065
	public static class RichTextUtil
	{
		/// <summary>
		/// Remove all color rich formatting so that shadow text displays correctly.
		/// </summary>
		// Token: 0x06004692 RID: 18066 RVA: 0x001A4B8F File Offset: 0x001A2D8F
		public static string replaceColorTags(string text)
		{
			return RichTextUtil.richTextColorTagRegex.Replace(text, string.Empty);
		}

		/// <summary>
		/// Shadow text needs the color tags removed, otherwise the shadow uses those colors.
		/// </summary>
		// Token: 0x06004693 RID: 18067 RVA: 0x001A4BA1 File Offset: 0x001A2DA1
		public static GUIContent makeShadowContent(GUIContent content)
		{
			return new GUIContent(RichTextUtil.replaceColorTags(content.text), RichTextUtil.replaceColorTags(content.tooltip));
		}

		/// <summary>
		/// Wrap text with color tags.
		/// </summary>
		// Token: 0x06004694 RID: 18068 RVA: 0x001A4BBE File Offset: 0x001A2DBE
		public static string wrapWithColor(string text, string color)
		{
			return string.Format("<color={0}>{1}</color>", color, text);
		}

		/// <summary>
		/// Wrap text with color tags.
		/// </summary>
		// Token: 0x06004695 RID: 18069 RVA: 0x001A4BCC File Offset: 0x001A2DCC
		public static string wrapWithColor(string text, Color32 color)
		{
			return RichTextUtil.wrapWithColor(text, Palette.hex(color));
		}

		/// <summary>
		/// Wrap text with color tags.
		/// </summary>
		// Token: 0x06004696 RID: 18070 RVA: 0x001A4BDA File Offset: 0x001A2DDA
		public static string wrapWithColor(string text, Color color)
		{
			return RichTextUtil.wrapWithColor(text, color);
		}

		/// <summary>
		/// Replace br tags with newlines.
		/// </summary>
		// Token: 0x06004697 RID: 18071 RVA: 0x001A4BE8 File Offset: 0x001A2DE8
		public static void replaceNewlineMarkup(ref string s)
		{
			s = s.Replace("<br>", "\n");
		}

		/// <summary>
		/// Should player be allowed to write given text on a sign?
		/// Keep in mind that newer signs use TMP, whereas older signs use uGUI.
		/// </summary>
		// Token: 0x06004698 RID: 18072 RVA: 0x001A4BFD File Offset: 0x001A2DFD
		public static bool isTextValidForSign(string text)
		{
			return string.IsNullOrEmpty(text) || (text.IndexOf("<size", 5) == -1 && text.IndexOf("<voffset", 5) == -1 && text.IndexOf("<sprite", 5) == -1);
		}

		/// <summary>
		/// Disable style, align, and space because they make server list unfair.
		/// </summary>
		// Token: 0x06004699 RID: 18073 RVA: 0x001A4C40 File Offset: 0x001A2E40
		internal static bool IsTextValidForServerListShortDescription(string text)
		{
			return string.IsNullOrEmpty(text) || (RichTextUtil.isTextValidForSign(text) && text.IndexOf("<style", 5) == -1 && text.IndexOf("<align", 5) == -1 && text.IndexOf("<space", 5) == -1 && text.IndexOf("<scale", 5) == -1 && text.IndexOf("<pos", 5) == -1);
		}

		// Token: 0x04003012 RID: 12306
		private static Regex richTextColorTagRegex = new Regex("</*color.*?>", 1);
	}
}
