﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18052
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Realeyes.UnitTests.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Realeyes.UnitTests.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;UTF-8&quot;?&gt;
        ///&lt;gesmes:Envelope xmlns:gesmes=&quot;http://www.gesmes.org/xml/2002-08-01&quot; xmlns=&quot;http://www.ecb.int/vocabulary/2002-08-01/eurofxref&quot;&gt;
        ///	&lt;gesmes:subject&gt;Reference rates&lt;/gesmes:subject&gt;
        ///	&lt;gesmes:Sender&gt;
        ///		&lt;gesmes:name&gt;European Central Bank&lt;/gesmes:name&gt;
        ///	&lt;/gesmes:Sender&gt;
        ///	&lt;Cube&gt;
        ///		&lt;Cube time=&quot;2015-02-26&quot;&gt;
        ///			&lt;Cube currency=&quot;USD&quot; rate=&quot;1.1317&quot;/&gt;
        ///			&lt;Cube currency=&quot;JPY&quot; rate=&quot;134.54&quot;/&gt;
        ///			&lt;Cube currency=&quot;BGN&quot; rate=&quot;1.9558&quot;/&gt;
        ///			&lt;Cube currency=&quot;CZK&quot; rate=&quot;27.515&quot;/&gt;
        ///	 [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string xml_EcbSample {
            get {
                return ResourceManager.GetString("xml_EcbSample", resourceCulture);
            }
        }
    }
}
