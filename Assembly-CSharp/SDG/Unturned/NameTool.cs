using System;

namespace SDG.Unturned
{
	// Token: 0x0200075D RID: 1885
	public class NameTool
	{
		// Token: 0x06003DB2 RID: 15794 RVA: 0x00128B4A File Offset: 0x00126D4A
		public static bool checkNames(string input, string name)
		{
			return input.Length <= name.Length && name.ToLower().IndexOf(input.ToLower()) != -1;
		}

		/// <summary>
		/// If updating this method please remember to update the support article:
		/// https://support.smartlydressedgames.com/hc/en-us/articles/13452208765716
		/// </summary>
		// Token: 0x06003DB3 RID: 15795 RVA: 0x00128B74 File Offset: 0x00126D74
		public static bool isValid(string name)
		{
			for (int i = 0; i < name.Length; i++)
			{
				char c = name.get_Chars(i);
				if (c <= '\u001f')
				{
					return false;
				}
				if (c >= '~')
				{
					return false;
				}
				if (c == '/' || c == '\\' || c == '`' || c == '\'' || c == '"')
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Does name contain rich text tags?
		/// Some players were abusing rich text enabled servers by inserting admin colors into their steam name.
		/// </summary>
		// Token: 0x06003DB4 RID: 15796 RVA: 0x00128BC8 File Offset: 0x00126DC8
		public static bool containsRichText(string name)
		{
			return name.IndexOf("<color", 5) != -1 || name.IndexOf("<b>", 5) != -1 || name.IndexOf("<i>", 5) != -1 || name.IndexOf("<size", 5) != -1 || name.IndexOf("<voffset", 5) != -1 || name.IndexOf("<sprite", 5) != -1;
		}
	}
}
