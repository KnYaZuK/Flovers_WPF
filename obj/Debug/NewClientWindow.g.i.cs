﻿#pragma checksum "..\..\NewClientWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "797F6FEAEBB4796117C5C1EA607CE209"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.0
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using MahApps.Metro.Controls;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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


namespace Flovers_WPF {
    
    
    /// <summary>
    /// NewClientWindow
    /// </summary>
    public partial class NewClientWindow : MahApps.Metro.Controls.MetroWindow, System.Windows.Markup.IComponentConnector {
        
        
        #line 7 "..\..\NewClientWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid grid;
        
        #line default
        #line hidden
        
        
        #line 11 "..\..\NewClientWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tb_name;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\NewClientWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tb_phone;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\NewClientWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tb_email;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\NewClientWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tb_referer_;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\NewClientWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button button_Create;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\NewClientWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button button_Update;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\NewClientWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView listview_Clients_Cards;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Flovers_WPF;component/newclientwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\NewClientWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 5 "..\..\NewClientWindow.xaml"
            ((Flovers_WPF.NewClientWindow)(target)).Loaded += new System.Windows.RoutedEventHandler(this.MetroWindow_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.grid = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.tb_name = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.tb_phone = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.tb_email = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.tb_referer_ = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            this.button_Create = ((System.Windows.Controls.Button)(target));
            
            #line 32 "..\..\NewClientWindow.xaml"
            this.button_Create.Click += new System.Windows.RoutedEventHandler(this.button_Create_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.button_Update = ((System.Windows.Controls.Button)(target));
            
            #line 33 "..\..\NewClientWindow.xaml"
            this.button_Update.Click += new System.Windows.RoutedEventHandler(this.button_Update_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.listview_Clients_Cards = ((System.Windows.Controls.ListView)(target));
            
            #line 37 "..\..\NewClientWindow.xaml"
            this.listview_Clients_Cards.MouseRightButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.listview_MouseRightButtonUp);
            
            #line default
            #line hidden
            
            #line 37 "..\..\NewClientWindow.xaml"
            this.listview_Clients_Cards.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.listview_MouseLeftButtonUp);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

