using System;
using System.Diagnostics;
using System.Threading;

namespace SDG.Unturned
{
	// Token: 0x0200081E RID: 2078
	public static class ThreadUtil
	{
		// Token: 0x17000BA3 RID: 2979
		// (get) Token: 0x060046F2 RID: 18162 RVA: 0x001A7EC1 File Offset: 0x001A60C1
		// (set) Token: 0x060046F3 RID: 18163 RVA: 0x001A7EC8 File Offset: 0x001A60C8
		public static Thread gameThread { get; private set; }

		/// <summary>
		/// Called once by Setup.
		/// </summary>
		// Token: 0x060046F4 RID: 18164 RVA: 0x001A7ED0 File Offset: 0x001A60D0
		public static void setupGameThread()
		{
			if (ThreadUtil.gameThread == null)
			{
				ThreadUtil.gameThread = Thread.CurrentThread;
				GameThreadQueueUtil.Setup();
				return;
			}
			throw new Exception("gameThread has already been setup");
		}

		/// <summary>
		/// Extension method for Thread class.
		/// Plugins use this.
		/// I might have accidentally removed it due to zero refs and Pustalorc was mad:
		/// https://github.com/SmartlyDressedGames/Unturned-3.x-Community/discussions/4131
		/// </summary>
		// Token: 0x060046F5 RID: 18165 RVA: 0x001A7EF5 File Offset: 0x001A60F5
		public static bool IsGameThread(this Thread thread)
		{
			return thread == ThreadUtil.gameThread;
		}

		/// <summary>
		/// Throw an exception if current thread is not the game thread.
		/// </summary>
		// Token: 0x060046F6 RID: 18166 RVA: 0x001A7EFF File Offset: 0x001A60FF
		public static void assertIsGameThread()
		{
			if (Thread.CurrentThread != ThreadUtil.gameThread)
			{
				throw new NotSupportedException("This function should only be called from the game thread. (e.g. from Unity's Update)");
			}
		}

		/// <summary>
		/// Only on dedicated server: throw an exception if current thread is not the game thread.
		/// </summary>
		// Token: 0x060046F7 RID: 18167 RVA: 0x001A7F18 File Offset: 0x001A6118
		[Conditional("WITH_GAME_THREAD_ASSERTIONS")]
		internal static void ConditionalAssertIsGameThread()
		{
			if (Thread.CurrentThread != ThreadUtil.gameThread)
			{
				throw new NotSupportedException("This function should only be called from the game thread. (e.g. from Unity's Update)");
			}
		}
	}
}
