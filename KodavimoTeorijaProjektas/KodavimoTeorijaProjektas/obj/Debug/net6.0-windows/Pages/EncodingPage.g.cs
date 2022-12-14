#pragma checksum "..\..\..\..\Pages\EncodingPage.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "DF603F1662C44C8AF3BE4AE5F9B91C67B72DB887"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using KodavimoTeorijaProjektas;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace KodavimoTeorijaProjektas {
    
    
    /// <summary>
    /// EncodingPage
    /// </summary>
    public partial class EncodingPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 61 "..\..\..\..\Pages\EncodingPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel VectorPanel;
        
        #line default
        #line hidden
        
        
        #line 68 "..\..\..\..\Pages\EncodingPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox VectorInput;
        
        #line default
        #line hidden
        
        
        #line 82 "..\..\..\..\Pages\EncodingPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel MessagePanel;
        
        #line default
        #line hidden
        
        
        #line 89 "..\..\..\..\Pages\EncodingPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox MessageInput;
        
        #line default
        #line hidden
        
        
        #line 104 "..\..\..\..\Pages\EncodingPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel ImagePanel;
        
        #line default
        #line hidden
        
        
        #line 117 "..\..\..\..\Pages\EncodingPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel EncodedVectorPanel;
        
        #line default
        #line hidden
        
        
        #line 134 "..\..\..\..\Pages\EncodingPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock EncodedVector;
        
        #line default
        #line hidden
        
        
        #line 143 "..\..\..\..\Pages\EncodingPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel UploadedImagePanel;
        
        #line default
        #line hidden
        
        
        #line 160 "..\..\..\..\Pages\EncodingPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image UploadedImage;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.8.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/KodavimoTeorijaProjektas;component/pages/encodingpage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Pages\EncodingPage.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.8.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 41 "..\..\..\..\Pages\EncodingPage.xaml"
            ((System.Windows.Controls.RadioButton)(target)).Click += new System.Windows.RoutedEventHandler(this.VectorSelected_Checked);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 45 "..\..\..\..\Pages\EncodingPage.xaml"
            ((System.Windows.Controls.RadioButton)(target)).Click += new System.Windows.RoutedEventHandler(this.MessageSelected_Checked);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 49 "..\..\..\..\Pages\EncodingPage.xaml"
            ((System.Windows.Controls.RadioButton)(target)).Click += new System.Windows.RoutedEventHandler(this.ImageSelected_Checked);
            
            #line default
            #line hidden
            return;
            case 4:
            this.VectorPanel = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 5:
            this.VectorInput = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            
            #line 74 "..\..\..\..\Pages\EncodingPage.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ValidateButton_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.MessagePanel = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 8:
            this.MessageInput = ((System.Windows.Controls.TextBox)(target));
            return;
            case 9:
            
            #line 96 "..\..\..\..\Pages\EncodingPage.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ValidateButton_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.ImagePanel = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 11:
            
            #line 112 "..\..\..\..\Pages\EncodingPage.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.SelectImageButton_Click);
            
            #line default
            #line hidden
            return;
            case 12:
            this.EncodedVectorPanel = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 13:
            this.EncodedVector = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 14:
            this.UploadedImagePanel = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 15:
            this.UploadedImage = ((System.Windows.Controls.Image)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

