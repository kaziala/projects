﻿#pragma checksum "C:\Users\mohdm\Downloads\groupproject-highlander final\groupproject-highlander\HighlanderSimulator\HighlanderSimulator\MainPage.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "90D291AE84B373E378EACF4322168EF2C193BBCF048BCD5C2D92100D06D16969"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HighlanderSimulator
{
    partial class MainPage : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 0.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 2: // MainPage.xaml line 28
                {
                    this.SizeTextBox = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 3: // MainPage.xaml line 31
                {
                    this.GoodCountTextBox = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 4: // MainPage.xaml line 34
                {
                    this.BadCountTextBox = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 5: // MainPage.xaml line 36
                {
                    global::Windows.UI.Xaml.Controls.Button element5 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)element5).Click += this.StartSimulation_Click;
                }
                break;
            case 6: // MainPage.xaml line 38
                {
                    this.SimulationGrid = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 7: // MainPage.xaml line 42
                {
                    this.UserProvidedRadioButton = (global::Windows.UI.Xaml.Controls.RadioButton)(target);
                    ((global::Windows.UI.Xaml.Controls.RadioButton)this.UserProvidedRadioButton).Checked += this.UserProvidedRadioButton_Checked;
                }
                break;
            case 8: // MainPage.xaml line 43
                {
                    this.ProgramDeterminedRadioButton = (global::Windows.UI.Xaml.Controls.RadioButton)(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 0.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

