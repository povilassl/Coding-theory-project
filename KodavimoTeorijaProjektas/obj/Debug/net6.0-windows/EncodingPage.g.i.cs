﻿#pragma checksum "..\..\..\EncodingPage.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "05688D9AF557AC25D878748C84AFE5D8D60FB0A0"
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
        
        
        #line 45 "..\..\..\EncodingPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel VectorPanel;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\..\EncodingPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox VectorInput;
        
        #line default
        #line hidden
        
        
        #line 64 "..\..\..\EncodingPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel MessagePanel;
        
        #line default
        #line hidden
        
        
        #line 79 "..\..\..\EncodingPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel ImagePanel;
        
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
            System.Uri resourceLocater = new System.Uri("/KodavimoTeorijaProjektas;V1.0.0.0;component/encodingpage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\EncodingPage.xaml"
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
            
            #line 27 "..\..\..\EncodingPage.xaml"
            ((System.Windows.Controls.RadioButton)(target)).Click += new System.Windows.RoutedEventHandler(this.VectorSelected_Checked);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 31 "..\..\..\EncodingPage.xaml"
            ((System.Windows.Controls.RadioButton)(target)).Click += new System.Windows.RoutedEventHandler(this.MessageSelected_Checked);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 35 "..\..\..\EncodingPage.xaml"
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
            
            #line 56 "..\..\..\EncodingPage.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ValidateButton_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.MessagePanel = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 8:
            
            #line 71 "..\..\..\EncodingPage.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ValidateButton_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.ImagePanel = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 10:
            
            #line 86 "..\..\..\EncodingPage.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ValidateButton_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

