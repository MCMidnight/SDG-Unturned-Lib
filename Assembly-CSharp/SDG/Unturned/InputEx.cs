using System;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// Extensions to the built-in Input class.
	/// </summary>
	// Token: 0x02000803 RID: 2051
	public static class InputEx
	{
		/// <summary>
		/// Wrapper for Input.GetKey, but returns false while typing in a uGUI text field.
		/// </summary>
		// Token: 0x06004651 RID: 18001 RVA: 0x001A3AB3 File Offset: 0x001A1CB3
		public static bool GetKey(KeyCode key)
		{
			return Input.GetKey(key) && Glazier.Get().ShouldGameProcessKeyDown;
		}

		/// <summary>
		/// Wrapper for Input.GetKeyDown, but returns false while typing in a uGUI text field.
		/// </summary>
		// Token: 0x06004652 RID: 18002 RVA: 0x001A3AC9 File Offset: 0x001A1CC9
		public static bool GetKeyDown(KeyCode key)
		{
			return Input.GetKeyDown(key) && Glazier.Get().ShouldGameProcessKeyDown;
		}

		/// <summary>
		/// Wrapper for Input.GetKeyUp, but returns false while typing in a uGUI text field.
		/// </summary>
		// Token: 0x06004653 RID: 18003 RVA: 0x001A3ADF File Offset: 0x001A1CDF
		public static bool GetKeyUp(KeyCode key)
		{
			return Input.GetKeyUp(key) && Glazier.Get().ShouldGameProcessKeyDown;
		}

		/// <summary>
		/// Should be used anywhere that Input.GetKeyDown opens a UI.
		///
		/// Each frame one input event can be consumed. This is a hack to prevent multiple UI-related key presses from
		/// interfering during the same frame. Only the first input event proceeds, while the others are ignored.
		/// </summary>
		/// <returns>True if caller should proceed, false otherwise.</returns>
		// Token: 0x06004654 RID: 18004 RVA: 0x001A3AF5 File Offset: 0x001A1CF5
		public static bool ConsumeKeyDown(KeyCode key)
		{
			return InputEx.GetKeyDown(key) && InputEx.keyGuard.Consume();
		}

		/// <summary>
		/// Get mouse position in viewport coordinates where zero is the bottom left and one is the top right.
		/// </summary>
		// Token: 0x17000B9D RID: 2973
		// (get) Token: 0x06004655 RID: 18005 RVA: 0x001A3B0C File Offset: 0x001A1D0C
		public static Vector2 NormalizedMousePosition
		{
			get
			{
				Vector2 result = Input.mousePosition;
				result.x /= (float)Screen.width;
				result.y /= (float)Screen.height;
				return result;
			}
		}

		// Token: 0x04002F4A RID: 12106
		private static OncePerFrameGuard keyGuard;
	}
}
