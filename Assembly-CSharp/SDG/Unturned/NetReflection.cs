using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x0200024D RID: 589
	public static class NetReflection
	{
		/// <summary>
		/// Log all known net methods.
		/// </summary>
		// Token: 0x060011F5 RID: 4597 RVA: 0x0003D770 File Offset: 0x0003B970
		public static void Dump()
		{
			NetReflection.Log(string.Format("{0} client methods ({1} bits):", NetReflection.clientMethods.Count, NetReflection.clientMethodsBitCount));
			for (int i = 0; i < NetReflection.clientMethods.Count; i++)
			{
				ClientMethodInfo clientMethodInfo = NetReflection.clientMethods[i];
				NetReflection.Log(string.Format("{0} {1}", i, clientMethodInfo));
			}
			NetReflection.Log(string.Format("{0} server methods ({1} bits):", NetReflection.serverMethods.Count, NetReflection.serverMethodsBitCount));
			for (int j = 0; j < NetReflection.serverMethods.Count; j++)
			{
				ServerMethodInfo serverMethodInfo = NetReflection.serverMethods[j];
				NetReflection.Log(string.Format("{0} {1}", j, serverMethodInfo));
			}
		}

		// Token: 0x060011F6 RID: 4598 RVA: 0x0003D83C File Offset: 0x0003BA3C
		public static void SetLogCallback(Action<string> logCallback)
		{
			NetReflection.logCallback = logCallback;
			if (NetReflection.pendingMessages != null)
			{
				foreach (string text in NetReflection.pendingMessages)
				{
					logCallback.Invoke(text);
				}
				NetReflection.pendingMessages = null;
			}
		}

		// Token: 0x060011F7 RID: 4599 RVA: 0x0003D8A4 File Offset: 0x0003BAA4
		internal static ClientMethodInfo GetClientMethodInfo(Type declaringType, string methodName)
		{
			foreach (ClientMethodInfo clientMethodInfo in NetReflection.clientMethods)
			{
				if (clientMethodInfo.declaringType == declaringType && clientMethodInfo.name.Equals(methodName, 4))
				{
					return clientMethodInfo;
				}
			}
			NetReflection.Log("Unable to find client method info for " + declaringType.Name + "." + methodName);
			return null;
		}

		// Token: 0x060011F8 RID: 4600 RVA: 0x0003D930 File Offset: 0x0003BB30
		internal static ServerMethodInfo GetServerMethodInfo(Type declaringType, string methodName)
		{
			foreach (ServerMethodInfo serverMethodInfo in NetReflection.serverMethods)
			{
				if (serverMethodInfo.declaringType == declaringType && serverMethodInfo.name.Equals(methodName, 4))
				{
					return serverMethodInfo;
				}
			}
			NetReflection.Log("Unable to find server method info for " + declaringType.Name + "." + methodName);
			return null;
		}

		// Token: 0x060011F9 RID: 4601 RVA: 0x0003D9BC File Offset: 0x0003BBBC
		private static bool FindAndRemoveGeneratedMethod(List<NetReflection.GeneratedMethod> generatedMethods, string methodName, out NetReflection.GeneratedMethod foundMethod)
		{
			for (int i = generatedMethods.Count - 1; i >= 0; i--)
			{
				NetReflection.GeneratedMethod generatedMethod = generatedMethods[i];
				if (generatedMethod.attribute.targetMethodName == methodName)
				{
					generatedMethods.RemoveAtFast(i);
					foundMethod = generatedMethod;
					return true;
				}
			}
			foundMethod = default(NetReflection.GeneratedMethod);
			return false;
		}

		// Token: 0x060011FA RID: 4602 RVA: 0x0003DA10 File Offset: 0x0003BC10
		private static ClientMethodReceive FindClientReceiveMethod(Type generatedType, List<NetReflection.GeneratedMethod> generatedMethods, string methodName)
		{
			NetReflection.GeneratedMethod generatedMethod;
			if (NetReflection.FindAndRemoveGeneratedMethod(generatedMethods, methodName, out generatedMethod))
			{
				try
				{
					return (ClientMethodReceive)generatedMethod.info.CreateDelegate(typeof(ClientMethodReceive));
				}
				catch
				{
					NetReflection.Log(string.Concat(new string[]
					{
						"Exception creating delegate for client ",
						generatedType.Name,
						".",
						methodName,
						" receive implementation"
					}));
					return null;
				}
			}
			NetReflection.Log(string.Concat(new string[]
			{
				"Unable to find client ",
				generatedType.Name,
				".",
				methodName,
				" receive implementation"
			}));
			return null;
		}

		// Token: 0x060011FB RID: 4603 RVA: 0x0003DAC8 File Offset: 0x0003BCC8
		internal static T CreateClientWriteDelegate<T>(ClientMethodInfo clientMethod) where T : Delegate
		{
			T result;
			try
			{
				result = (clientMethod.writeMethodInfo.CreateDelegate(typeof(T)) as T);
			}
			catch
			{
				NetReflection.Log(string.Format("Exception creating delegate for client {0} write", clientMethod));
				result = default(T);
			}
			return result;
		}

		// Token: 0x060011FC RID: 4604 RVA: 0x0003DB28 File Offset: 0x0003BD28
		private static ServerMethodReceive FindServerReceiveMethod(Type generatedType, List<NetReflection.GeneratedMethod> generatedMethods, string methodName)
		{
			NetReflection.GeneratedMethod generatedMethod;
			if (NetReflection.FindAndRemoveGeneratedMethod(generatedMethods, methodName, out generatedMethod))
			{
				try
				{
					return (ServerMethodReceive)generatedMethod.info.CreateDelegate(typeof(ServerMethodReceive));
				}
				catch
				{
					NetReflection.Log(string.Concat(new string[]
					{
						"Exception creating delegate for server ",
						generatedType.Name,
						".",
						methodName,
						" receive implementation"
					}));
					return null;
				}
			}
			NetReflection.Log(string.Concat(new string[]
			{
				"Unable to find server ",
				generatedType.Name,
				".",
				methodName,
				" receive implementation"
			}));
			return null;
		}

		// Token: 0x060011FD RID: 4605 RVA: 0x0003DBE0 File Offset: 0x0003BDE0
		internal static T CreateServerWriteDelegate<T>(ServerMethodInfo serverMethod) where T : Delegate
		{
			T result;
			try
			{
				result = (serverMethod.writeMethodInfo.CreateDelegate(typeof(T)) as T);
			}
			catch
			{
				NetReflection.Log(string.Format("Exception creating delegate for server {0} write", serverMethod));
				result = default(T);
			}
			return result;
		}

		/// <summary>
		/// This class gets used from type initializers, so Unity's built-in log is not an option unfortunately.
		/// </summary>
		// Token: 0x060011FE RID: 4606 RVA: 0x0003DC40 File Offset: 0x0003BE40
		private static void Log(string message)
		{
			if (NetReflection.logCallback != null)
			{
				NetReflection.logCallback.Invoke(message);
				return;
			}
			NetReflection.pendingMessages = new List<string>();
			NetReflection.pendingMessages.Add(message);
		}

		/// <summary>
		/// Not *really* supported but *might* probably work. Adding for public discussion #4176.
		/// </summary>
		// Token: 0x060011FF RID: 4607 RVA: 0x0003DC6C File Offset: 0x0003BE6C
		public static void RegisterFromAssembly(Assembly assembly)
		{
			List<NetReflection.GeneratedMethod> list = new List<NetReflection.GeneratedMethod>();
			List<NetReflection.GeneratedMethod> list2 = new List<NetReflection.GeneratedMethod>();
			foreach (Type type in assembly.GetTypes())
			{
				if (type.IsClass && type.IsAbstract)
				{
					NetInvokableGeneratedClassAttribute customAttribute = CustomAttributeExtensions.GetCustomAttribute<NetInvokableGeneratedClassAttribute>(type);
					if (customAttribute != null)
					{
						list.Clear();
						list2.Clear();
						foreach (MethodInfo methodInfo in type.GetMethods(24))
						{
							NetInvokableGeneratedMethodAttribute customAttribute2 = CustomAttributeExtensions.GetCustomAttribute<NetInvokableGeneratedMethodAttribute>(methodInfo);
							if (customAttribute2 != null)
							{
								NetReflection.GeneratedMethod generatedMethod = default(NetReflection.GeneratedMethod);
								generatedMethod.info = methodInfo;
								generatedMethod.attribute = customAttribute2;
								ENetInvokableGeneratedMethodPurpose purpose = customAttribute2.purpose;
								if (purpose != ENetInvokableGeneratedMethodPurpose.Read)
								{
									if (purpose != ENetInvokableGeneratedMethodPurpose.Write)
									{
										NetReflection.Log(string.Format("Generated method {0}.{1} unknown purpose {2}", type.Name, methodInfo.Name, customAttribute2.purpose));
									}
									else
									{
										list2.Add(generatedMethod);
									}
								}
								else
								{
									list.Add(generatedMethod);
								}
							}
						}
						foreach (MethodInfo methodInfo2 in customAttribute.targetType.GetMethods(30))
						{
							SteamCall customAttribute3 = CustomAttributeExtensions.GetCustomAttribute<SteamCall>(methodInfo2);
							if (customAttribute3 != null)
							{
								ParameterInfo[] parameters = methodInfo2.GetParameters();
								if (customAttribute3.validation == ESteamCallValidation.ONLY_FROM_SERVER)
								{
									ClientMethodInfo clientMethodInfo = new ClientMethodInfo();
									clientMethodInfo.declaringType = methodInfo2.DeclaringType;
									clientMethodInfo.debugName = string.Format("{0}.{1}", methodInfo2.DeclaringType, methodInfo2.Name);
									clientMethodInfo.name = methodInfo2.Name;
									clientMethodInfo.customAttribute = customAttribute3;
									bool flag = parameters.Length == 1 && parameters[0].ParameterType.GetElementType() == typeof(ClientInvocationContext);
									if (methodInfo2.IsStatic && flag)
									{
										clientMethodInfo.readMethod = (Delegate.CreateDelegate(typeof(ClientMethodReceive), methodInfo2, false) as ClientMethodReceive);
									}
									else
									{
										clientMethodInfo.readMethod = NetReflection.FindClientReceiveMethod(type, list, methodInfo2.Name);
										if (!flag)
										{
											NetReflection.GeneratedMethod generatedMethod2;
											if (NetReflection.FindAndRemoveGeneratedMethod(list2, methodInfo2.Name, out generatedMethod2))
											{
												clientMethodInfo.writeMethodInfo = generatedMethod2.info;
											}
											else
											{
												NetReflection.Log(string.Concat(new string[]
												{
													"Unable to find client ",
													type.Name,
													".",
													methodInfo2.Name,
													" write implementation"
												}));
											}
										}
									}
									clientMethodInfo.methodIndex = (uint)NetReflection.clientMethods.Count;
									NetReflection.clientMethods.Add(clientMethodInfo);
								}
								else if (customAttribute3.validation == ESteamCallValidation.SERVERSIDE || customAttribute3.validation == ESteamCallValidation.ONLY_FROM_OWNER)
								{
									ServerMethodInfo serverMethodInfo = new ServerMethodInfo();
									serverMethodInfo.declaringType = methodInfo2.DeclaringType;
									serverMethodInfo.name = methodInfo2.Name;
									serverMethodInfo.debugName = string.Format("{0}.{1}", methodInfo2.DeclaringType, methodInfo2.Name);
									serverMethodInfo.customAttribute = customAttribute3;
									bool flag2 = parameters.Length == 1 && parameters[0].ParameterType.GetElementType() == typeof(ServerInvocationContext);
									if (methodInfo2.IsStatic && flag2)
									{
										serverMethodInfo.readMethod = (Delegate.CreateDelegate(typeof(ServerMethodReceive), methodInfo2, false) as ServerMethodReceive);
									}
									else
									{
										serverMethodInfo.readMethod = NetReflection.FindServerReceiveMethod(type, list, methodInfo2.Name);
										if (!flag2)
										{
											NetReflection.GeneratedMethod generatedMethod3;
											if (NetReflection.FindAndRemoveGeneratedMethod(list2, methodInfo2.Name, out generatedMethod3))
											{
												serverMethodInfo.writeMethodInfo = generatedMethod3.info;
											}
											else
											{
												NetReflection.Log(string.Concat(new string[]
												{
													"Unable to find server ",
													type.Name,
													".",
													methodInfo2.Name,
													" write implementation"
												}));
											}
										}
									}
									if (customAttribute3.ratelimitHz > 0)
									{
										serverMethodInfo.rateLimitIndex = NetReflection.rateLimitedMethodsCount;
										customAttribute3.rateLimitIndex = NetReflection.rateLimitedMethodsCount;
										customAttribute3.ratelimitSeconds = 1f / (float)customAttribute3.ratelimitHz;
										NetReflection.rateLimitedMethodsCount++;
									}
									else
									{
										serverMethodInfo.rateLimitIndex = -1;
									}
									serverMethodInfo.methodIndex = (uint)NetReflection.serverMethods.Count;
									NetReflection.serverMethods.Add(serverMethodInfo);
								}
							}
						}
						foreach (NetReflection.GeneratedMethod generatedMethod4 in list)
						{
							NetReflection.Log(string.Concat(new string[]
							{
								"Generated read method ",
								type.Name,
								".",
								generatedMethod4.info.Name,
								" not used"
							}));
						}
						foreach (NetReflection.GeneratedMethod generatedMethod5 in list2)
						{
							NetReflection.Log(string.Concat(new string[]
							{
								"Generated write method ",
								type.Name,
								".",
								generatedMethod5.info.Name,
								" not used"
							}));
						}
					}
				}
			}
			NetReflection.clientMethodsLength = (uint)NetReflection.clientMethods.Count;
			NetReflection.clientMethodsBitCount = NetPakConst.CountBits(NetReflection.clientMethodsLength);
			NetReflection.serverMethodsLength = (uint)NetReflection.serverMethods.Count;
			NetReflection.serverMethodsBitCount = NetPakConst.CountBits(NetReflection.serverMethodsLength);
		}

		// Token: 0x06001200 RID: 4608 RVA: 0x0003E1D8 File Offset: 0x0003C3D8
		static NetReflection()
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			NetReflection.RegisterFromAssembly(Assembly.GetExecutingAssembly());
			stopwatch.Stop();
			NetReflection.Log(string.Format("Reflect net invokables: {0}ms", stopwatch.ElapsedMilliseconds));
		}

		// Token: 0x0400059C RID: 1436
		internal static List<ClientMethodInfo> clientMethods = new List<ClientMethodInfo>();

		// Token: 0x0400059D RID: 1437
		internal static uint clientMethodsLength;

		// Token: 0x0400059E RID: 1438
		internal static int clientMethodsBitCount;

		// Token: 0x0400059F RID: 1439
		internal static List<ServerMethodInfo> serverMethods = new List<ServerMethodInfo>();

		// Token: 0x040005A0 RID: 1440
		internal static uint serverMethodsLength;

		// Token: 0x040005A1 RID: 1441
		internal static int serverMethodsBitCount;

		/// <summary>
		/// Number of server methods with rate limits.
		/// </summary>
		// Token: 0x040005A2 RID: 1442
		internal static int rateLimitedMethodsCount = 0;

		// Token: 0x040005A3 RID: 1443
		private static List<string> pendingMessages;

		// Token: 0x040005A4 RID: 1444
		private static Action<string> logCallback;

		// Token: 0x020008D0 RID: 2256
		private struct GeneratedMethod
		{
			// Token: 0x040031EB RID: 12779
			public MethodInfo info;

			// Token: 0x040031EC RID: 12780
			public NetInvokableGeneratedMethodAttribute attribute;
		}
	}
}
