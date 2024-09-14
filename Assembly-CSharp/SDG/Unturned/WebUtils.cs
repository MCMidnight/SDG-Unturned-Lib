using System;

namespace SDG.Unturned
{
	// Token: 0x02000823 RID: 2083
	internal static class WebUtils
	{
		/// <summary>
		/// The game uses Process.Start to open web links when the Steam overlay is unavailable, which could be exploited
		/// to e.g. download and execute files. To prevent this we only allow valid http or https urls.
		/// </summary>
		/// <param name="autoPrefix">If true, prefix with https:// if neither http:// or https:// is specified.</param>
		// Token: 0x0600470C RID: 18188 RVA: 0x001A8268 File Offset: 0x001A6468
		internal static bool ParseThirdPartyUrl(string uriString, out string result, bool autoPrefix = true, bool useLinkFiltering = true)
		{
			if (string.IsNullOrEmpty(uriString))
			{
				result = null;
				return false;
			}
			uriString = uriString.Trim();
			if (autoPrefix && !uriString.StartsWith("http://", 5) && !uriString.StartsWith("https://", 5))
			{
				uriString = "https://" + uriString;
			}
			Uri uri;
			if (!Uri.TryCreate(uriString, 1, ref uri))
			{
				result = null;
				return false;
			}
			if (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps)
			{
				result = uri.AbsoluteUri;
				return true;
			}
			result = null;
			return false;
		}

		/// <summary>
		/// This version just doesn't return the parsed URL.
		/// </summary>
		// Token: 0x0600470D RID: 18189 RVA: 0x001A82F8 File Offset: 0x001A64F8
		internal static bool CanParseThirdPartyUrl(string uriString, bool autoPrefix = true, bool useLinkFiltering = true)
		{
			string text;
			return WebUtils.ParseThirdPartyUrl(uriString, out text, autoPrefix, useLinkFiltering);
		}
	}
}
