﻿#pragma checksum "..\..\SurfaceWindow1.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "D7E4FF483E6B5AF31181050B780F3FF4"
//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.1008
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Maps.MapControl.WPF;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Controls.Primitives;
using Microsoft.Surface.Presentation.Controls.TouchVisualizations;
using Microsoft.Surface.Presentation.Input;
using Microsoft.Surface.Presentation.Palettes;
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


namespace SurfaceApplication1 {
    
    
    /// <summary>
    /// SurfaceWindow1
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
    public partial class SurfaceWindow1 : Microsoft.Surface.Presentation.Controls.SurfaceWindow, System.Windows.Markup.IComponentConnector {
        
        
        #line 12 "..\..\SurfaceWindow1.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas Wrapper;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\SurfaceWindow1.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Microsoft.Maps.MapControl.WPF.Map Map;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\SurfaceWindow1.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel EventsPanel;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/SurfaceApplication1;component/surfacewindow1.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\SurfaceWindow1.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.Wrapper = ((System.Windows.Controls.Canvas)(target));
            
            #line 13 "..\..\SurfaceWindow1.xaml"
            this.Wrapper.TouchDown += new System.EventHandler<System.Windows.Input.TouchEventArgs>(this.TagDown);
            
            #line default
            #line hidden
            
            #line 13 "..\..\SurfaceWindow1.xaml"
            this.Wrapper.TouchMove += new System.EventHandler<System.Windows.Input.TouchEventArgs>(this.TagMove);
            
            #line default
            #line hidden
            
            #line 13 "..\..\SurfaceWindow1.xaml"
            this.Wrapper.TouchUp += new System.EventHandler<System.Windows.Input.TouchEventArgs>(this.TagGone);
            
            #line default
            #line hidden
            return;
            case 2:
            this.Map = ((Microsoft.Maps.MapControl.WPF.Map)(target));
            return;
            case 3:
            this.EventsPanel = ((System.Windows.Controls.StackPanel)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

