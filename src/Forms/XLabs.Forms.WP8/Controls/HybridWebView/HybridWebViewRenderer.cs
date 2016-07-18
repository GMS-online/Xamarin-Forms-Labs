// ***********************************************************************
// Assembly         : XLabs.Forms.WP8
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="HybridWebViewRenderer.cs" company="XLabs Team">
//     Copyright (c) XLabs Team. All rights reserved.
// </copyright>
// <summary>
//       This project is licensed under the Apache 2.0 license
//       https://github.com/XLabs/Xamarin-Forms-Labs/blob/master/LICENSE
//       
//       XLabs is a open source project that aims to provide a powerfull and cross 
//       platform set of controls tailored to work with Xamarin Forms.
// </summary>
// ***********************************************************************
// 

using System;
using Microsoft.Phone.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;
using XLabs.Forms.Controls;
using NavigationEventArgs = System.Windows.Navigation.NavigationEventArgs;

[assembly: ExportRenderer(typeof(HybridWebView), typeof(HybridWebViewRenderer))]

namespace XLabs.Forms.Controls
{
    /// <summary>
    /// The hybrid web view renderer.
    /// </summary>
    public partial class HybridWebViewRenderer : ViewRenderer<HybridWebView, WebBrowser>
    {
        /// <summary>
        ///     Called when [element changed].
        /// </summary>
        /// <param name="e">The e.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<HybridWebView> e)
        {
            base.OnElementChanged(e);

            if (this.Control == null && e.NewElement != null)
            {
                var webView = new WebBrowser { IsScriptEnabled = true, IsGeolocationEnabled = true };

                webView.Navigating += WebViewNavigating;
                webView.LoadCompleted += WebViewLoadCompleted;
                webView.ScriptNotify += WebViewOnScriptNotify;

                SetNativeControl(webView);
            }

            Unbind(e.OldElement);
            Bind();
        }

        /// <summary>
        ///     Webs the view on script notify.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="notifyEventArgs">The <see cref="NotifyEventArgs" /> instance containing the event data.</param>
        private void WebViewOnScriptNotify(object sender, NotifyEventArgs notifyEventArgs)
        {
            this.Element.MessageReceived(notifyEventArgs.Value);
        }

        /// <summary>
        ///     Webs the view load completed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Navigation.NavigationEventArgs" /> instance containing the event data.</param>
        private void WebViewLoadCompleted(object sender, NavigationEventArgs e)
        {
            this.Inject(NativeFunction + GetFuncScript());
            //this.Inject(GetFuncScript());
            Element.OnLoadFinished(sender, new EventArgs<Uri>(e.Uri));
        }

        /// <summary>
        ///     Loads the content.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="contentFullName">Full name of the content.</param>
        partial void LoadContent(object sender, HybridWebView.LoadContentEventArgs contentArgs)
        {
            this.Control.NavigateToString(contentArgs.Content);
            //LoadFromContent(sender, contentFullName);
        }

        /// <summary>
        /// Handles navigation started events.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="NavigatingEventArgs" /> instance containing the event data.</param>
        private void WebViewNavigating(object sender, NavigatingEventArgs e)
        {
            Element.OnNavigating(e.Uri);
        }

        /// <summary>
        ///     Injects the specified script.
        /// </summary>
        /// <param name="script">The script.</param>
        partial void Inject(string script)
        {
            Device.BeginInvokeOnMainThread(() => this.Control.InvokeScript("eval", script));
        }

        /// <summary>
        ///     Loads the specified URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        partial void Load(Uri uri)
        {
            if (uri != null)
            {
                this.Control.Source = uri;
            }
        }

        /// <summary>
        /// Loads from content.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="contentArgs">The <see cref="HybridWebView.LoadContentEventArgs"/> instance containing the event data.</param>
        partial void LoadFromContent(object sender, HybridWebView.LoadContentEventArgs contentArgs)
        {
            Element.Uri = new Uri(contentArgs.Content, UriKind.Relative);
        }

        /// <summary>
        ///     Loads from string.
        /// </summary>
        /// <param name="html">The HTML.</param>
        partial void LoadFromString(string html)
        {
            Control.NavigateToString(html);
        }

        partial void CanGoBack(object sender, HybridWebView.CanGoBackEventArgs args)
        {
            throw new NotImplementedException();
        }

        partial void ClearHistory(object sender, EventArgs args)
        {
            throw new NotImplementedException();
        }

        partial void GoBack(object sender, HybridWebView.GoBackEventArgs args)
        {
            throw new NotImplementedException();
        }

        partial void Reload(object sender, EventArgs args)
        {
            throw new NotImplementedException();
        }
    }
}