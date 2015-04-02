using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Web.UI;

// Information about this assembly is defined by the following
// attributes.
//
// change them to the information which is associated with the assembly
// you compile.

[assembly: AssemblyTitle("NeatUpload")]
[assembly: AssemblyDescription("NeatUpload allows ASP.NET developers to stream uploaded files to disk and allows users to monitor upload progress")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Dean Brettle")]
[assembly: AssemblyProduct("NeatUpload")]
[assembly: AssemblyCopyright("Copyright 2005, 2006 Dean Brettle.  Licensed under the Lesser General Public License.")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// The assembly version has following format :
//
// Major.Minor.Build.Revision
//
// You can specify all values by your own or you can build default build and revision
// numbers with the '*' character (the default):

[assembly: AssemblyVersion("1.2.*")]

[assembly: AssemblyInformationalVersion("NeatUpload-1.2.25")]

// The following attributes specify the key for the sign of your assembly. See the
// .NET Framework documentation for more information about signing.
// This is not required, if you don't want signing let these attributes like they're.
[assembly: AssemblyDelaySign(false)]
[assembly: AssemblyKeyFile("")]

// This helps with VS designer support.
[assembly: TagPrefix("Brettle.Web.NeatUpload", "Upload")]

// This makes it easier to link with code that require CLS compliance.
[assembly: CLSCompliant(true)]

#if USE_LOG4NET
[assembly: log4net.Config.XmlConfigurator(ConfigFile="log4net.config", Watch=true)]
#else
#warning LOGGING DISABLED.  To enable logging, add a reference to log4net and define USE_LOG4NET.
#endif

