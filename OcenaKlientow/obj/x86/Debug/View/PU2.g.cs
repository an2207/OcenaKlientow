﻿#pragma checksum "C:\Users\Mateusz Łopusiński\Desktop\Studia\Semestr 5\PO\1.0.2.1\1.0.2\OcenaKlientow\OcenaKlientow\View\PU2.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "F659DC809325AF58B4E3009E9F943039"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OcenaKlientow.View
{
    partial class PU2 : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                {
                    this.OsobyFizyczne = (global::Windows.UI.Xaml.Controls.ListView)(target);
                    #line 90 "..\..\..\View\PU2.xaml"
                    ((global::Windows.UI.Xaml.Controls.ListView)this.OsobyFizyczne).SelectionChanged += this.OsobyFizyczne_OnSelectionChanged;
                    #line default
                }
                break;
            case 2:
                {
                    this.textBlock = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 3:
                {
                    this.textBlock1 = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 4:
                {
                    this.textBlock1_Copy = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 5:
                {
                    this.textBlock1_Copy1 = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 6:
                {
                    this.StatusNameBox = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 7:
                {
                    this.StatusDateBox = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 8:
                {
                    this.StatusBoxPkt = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 9:
                {
                    this.CountStatus = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 84 "..\..\..\View\PU2.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.CountStatus).Click += this.CountStatus_OnClick;
                    #line default
                }
                break;
            case 10:
                {
                    this.GradeDetails = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 85 "..\..\..\View\PU2.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.GradeDetails).Click += this.GradeDetails_OnClick;
                    #line default
                }
                break;
            case 11:
                {
                    this.CountAllStatuses = (global::Windows.UI.Xaml.Controls.Button)(target);
                }
                break;
            case 12:
                {
                    this.OsobyPrawne = (global::Windows.UI.Xaml.Controls.ListView)(target);
                    #line 24 "..\..\..\View\PU2.xaml"
                    ((global::Windows.UI.Xaml.Controls.ListView)this.OsobyPrawne).SelectionChanged += this.OsobyPrawne_OnSelectionChanged;
                    #line default
                }
                break;
            case 13:
                {
                    this.IdFizyczna = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 14:
                {
                    this.NazwaFizyczna = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 15:
                {
                    this.SearchFizyczna = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 73 "..\..\..\View\PU2.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.SearchFizyczna).Click += this.SearchFizyczna_OnClick;
                    #line default
                }
                break;
            case 16:
                {
                    this.IdPrawna = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 17:
                {
                    this.NazwaPrawna = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 18:
                {
                    this.SearchPrawna = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 22 "..\..\..\View\PU2.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.SearchPrawna).Click += this.SearchPrawna_OnClick;
                    #line default
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

