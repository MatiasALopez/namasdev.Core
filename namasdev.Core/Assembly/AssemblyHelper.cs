using System;
using System.Globalization;
using System.Reflection;

namespace namasdev.Core.Assembly
{
    public class AssemblyHelper
    {
		public static void RedirectAssembly(string shortName, Version targetVersion, string publicKeyToken)
		{
			ResolveEventHandler handler = null;

			handler = (sender, args) => {
				// Use latest strong name & version when trying to load SDK assemblies
				var requestedAssembly = new AssemblyName(args.Name);
				if (requestedAssembly.Name != shortName)
					return null;

				requestedAssembly.Version = targetVersion;
				requestedAssembly.SetPublicKeyToken(new AssemblyName("x, PublicKeyToken=" + publicKeyToken).GetPublicKeyToken());
				requestedAssembly.CultureInfo = CultureInfo.InvariantCulture;

				AppDomain.CurrentDomain.AssemblyResolve -= handler;

				return System.Reflection.Assembly.Load(requestedAssembly);
			};
			AppDomain.CurrentDomain.AssemblyResolve += handler;
		}
	}
}
