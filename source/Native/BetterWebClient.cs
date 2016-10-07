// <copyright file="BetterWebClient.cs" company="LeagueSharp">
// Copyright (c) LeagueSharp. All rights reserved.
// </copyright>

namespace LeagueSharp.Common
{
    using System;
    using System.Net;

    /// <summary>
    ///     A better web client.
    /// </summary>
    public class BetterWebClient : WebClient
    {
        #region Fields

        private WebRequest request;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="BetterWebClient" /> class.
        /// </summary>
        /// <param name="cookieContainer">
        ///     The cookie container.
        /// </param>
        /// <param name="autoRedirect">
        ///     A value indicating whether to auto redirect.
        /// </param>
        public BetterWebClient(CookieContainer cookieContainer, bool autoRedirect = true)
        {
            this.CookieContainer = cookieContainer ?? new CookieContainer();
            this.AutoRedirect = autoRedirect;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets a value indicating whether to auto redirect.
        /// </summary>
        public bool AutoRedirect { get; set; }

        /// <summary>
        ///     Gets or sets the cookie container.
        /// </summary>
        public CookieContainer CookieContainer { get; set; }

        /// <summary>
        ///     Gets the cookies.
        /// </summary>
        public string Cookies => this.GetHeaderValue("Set-Cookie");

        /// <summary>
        ///     Gets the version.
        /// </summary>
        public string Kappa { get; } = "version5";

        /// <summary>
        ///     Gets the location.
        /// </summary>
        public string Location => this.GetHeaderValue("Location");

        /// <summary>
        ///     Gets the status code.
        /// </summary>
        public HttpStatusCode StatusCode
        {
            get
            {
                var result = HttpStatusCode.BadRequest;

                if (this.request != null)
                {
                    var response = this.GetWebResponse(this.request) as HttpWebResponse;
                    if (response != null)
                    {
                        result = response.StatusCode;
                    }
                }

                return result;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Gets the header value.
        /// </summary>
        /// <param name="header">
        ///     The header.
        /// </param>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public string GetHeaderValue(string header)
        {
            string result = null;

            if (this.request != null)
            {
                var response = this.GetWebResponse(this.request) as HttpWebResponse;
                if (response != null)
                {
                    result = response.Headers[header];
                }
            }

            return result;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override WebRequest GetWebRequest(Uri address)
        {
            this.request = base.GetWebRequest(address);

            var httpRequest = this.request as HttpWebRequest;

            if (httpRequest != null)
            {
                httpRequest.AllowAutoRedirect = this.AutoRedirect;
                httpRequest.CookieContainer = this.CookieContainer;
                httpRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            }

            return this.request;
        }

        #endregion
    }
}