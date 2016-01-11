﻿#pragma checksum "..\..\..\Controls\FeedbackControl.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "13CD869AE3B0406A669324A23B4DCC01"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

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


namespace TravelConn.Controls {
    
    
    /// <summary>
    /// FeedbackControl
    /// </summary>
    public partial class FeedbackControl : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 43 "..\..\..\Controls\FeedbackControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid FeedbackGrid;
        
        #line default
        #line hidden
        
        
        #line 60 "..\..\..\Controls\FeedbackControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock departTextBlock;
        
        #line default
        #line hidden
        
        
        #line 61 "..\..\..\Controls\FeedbackControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock arriveTextBlock;
        
        #line default
        #line hidden
        
        
        #line 62 "..\..\..\Controls\FeedbackControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock durationTextBlock;
        
        #line default
        #line hidden
        
        
        #line 63 "..\..\..\Controls\FeedbackControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock changesTextBlock;
        
        #line default
        #line hidden
        
        
        #line 66 "..\..\..\Controls\FeedbackControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button moreInfoBtn;
        
        #line default
        #line hidden
        
        
        #line 67 "..\..\..\Controls\FeedbackControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button printBtn;
        
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
            System.Uri resourceLocater = new System.Uri("/TravelConn;component/controls/feedbackcontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Controls\FeedbackControl.xaml"
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
            this.FeedbackGrid = ((System.Windows.Controls.Grid)(target));
            
            #line 43 "..\..\..\Controls\FeedbackControl.xaml"
            this.FeedbackGrid.MouseUp += new System.Windows.Input.MouseButtonEventHandler(this.FeedbackGrid_MouseUp);
            
            #line default
            #line hidden
            
            #line 43 "..\..\..\Controls\FeedbackControl.xaml"
            this.FeedbackGrid.MouseEnter += new System.Windows.Input.MouseEventHandler(this.FeedbackGrid_MouseEnter);
            
            #line default
            #line hidden
            
            #line 43 "..\..\..\Controls\FeedbackControl.xaml"
            this.FeedbackGrid.MouseLeave += new System.Windows.Input.MouseEventHandler(this.FeedbackGrid_MouseLeave);
            
            #line default
            #line hidden
            return;
            case 2:
            this.departTextBlock = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            this.arriveTextBlock = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 4:
            this.durationTextBlock = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 5:
            this.changesTextBlock = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 6:
            this.moreInfoBtn = ((System.Windows.Controls.Button)(target));
            
            #line 66 "..\..\..\Controls\FeedbackControl.xaml"
            this.moreInfoBtn.Click += new System.Windows.RoutedEventHandler(this.moreInfoButton_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.printBtn = ((System.Windows.Controls.Button)(target));
            
            #line 67 "..\..\..\Controls\FeedbackControl.xaml"
            this.printBtn.Click += new System.Windows.RoutedEventHandler(this.Print_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

