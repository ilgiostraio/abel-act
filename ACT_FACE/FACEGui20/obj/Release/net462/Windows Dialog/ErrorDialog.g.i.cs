﻿#pragma checksum "..\..\..\..\Windows Dialog\ErrorDialog.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "B91A62E5841A019C2FDAF7E04D46757DD9947B765560117893276C0AFB94D48E"
//------------------------------------------------------------------------------
// <auto-generated>
//     Il codice è stato generato da uno strumento.
//     Versione runtime:4.0.30319.42000
//
//     Le modifiche apportate a questo file possono provocare un comportamento non corretto e andranno perse se
//     il codice viene rigenerato.
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
using System.Windows.Forms.Integration;
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


namespace Act.Face.FACEGui20 {
    
    
    /// <summary>
    /// ErrorDialog
    /// </summary>
    public partial class ErrorDialog : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 10 "..\..\..\..\Windows Dialog\ErrorDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border borderCustomDialog;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\..\Windows Dialog\ErrorDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image imgInstructionIcon;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\..\..\Windows Dialog\ErrorDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbInstructionHeading;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\..\Windows Dialog\ErrorDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbInstructionText;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\..\Windows Dialog\ErrorDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbAdditionalDetailsText;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\..\Windows Dialog\ErrorDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button errorDialogOkButton;
        
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
            System.Uri resourceLocater = new System.Uri("/Act.Face.FACEGui20;V1.0.0.0;component/windows%20dialog/errordialog.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Windows Dialog\ErrorDialog.xaml"
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
            this.borderCustomDialog = ((System.Windows.Controls.Border)(target));
            return;
            case 2:
            this.imgInstructionIcon = ((System.Windows.Controls.Image)(target));
            return;
            case 3:
            this.tbInstructionHeading = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 4:
            this.tbInstructionText = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 5:
            this.tbAdditionalDetailsText = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 6:
            this.errorDialogOkButton = ((System.Windows.Controls.Button)(target));
            
            #line 30 "..\..\..\..\Windows Dialog\ErrorDialog.xaml"
            this.errorDialogOkButton.Click += new System.Windows.RoutedEventHandler(this.errorDialogOkButton_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

