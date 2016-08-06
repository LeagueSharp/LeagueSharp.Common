//Taken from http://keestalkstech.com/2014/03/a-slightly-better-webclient-class/

namespace LeagueSharp.Common
{
    using System;
    using System.Net;

    public class BetterWebClient : WebClient
    {
        #region Fields

        private WebRequest _request;

        #endregion

        #region Constructors and Destructors

        public BetterWebClient(CookieContainer cookies, bool autoRedirect = true)
        {
            this.CookieContainer = cookies ?? new CookieContainer();
            this.AutoRedirect = autoRedirect;
        }

        #endregion

        #region Public Properties

        //Gets or sets whether to automatically follow a redirect
        public bool AutoRedirect { get; set; }

        //Gets or sets the cookie container, contains all the 
        //cookies for all the requests
        public CookieContainer CookieContainer { get; set; }

        //Gets last cookie header
        public string Cookies
        {
            get
            {
                return this.GetHeaderValue("Set-Cookie");
            }
        }

        public string Kappa
        {
            get
            {
                return "version5";
            }
        }

        //Get last location header
        public string Location
        {
            get
            {
                return this.GetHeaderValue("Location");
            }
        }

        //Get last status code
        public HttpStatusCode StatusCode
        {
            get
            {
                var result = HttpStatusCode.BadRequest;

                if (this._request != null)
                {
                    var response = this.GetWebResponse(this._request) as HttpWebResponse;

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

        public string GetHeaderValue(string headerName)
        {
            string result = null;

            if (this._request != null)
            {
                var response = this.GetWebResponse(this._request) as HttpWebResponse;
                if (response != null)
                {
                    result = response.Headers[headerName];
                }
            }

            return result;
        }

        #endregion

        #region Methods

        protected override WebRequest GetWebRequest(Uri address)
        {
            this._request = base.GetWebRequest(address);

            var httpRequest = this._request as HttpWebRequest;

            if (httpRequest != null)
            {
                httpRequest.AllowAutoRedirect = this.AutoRedirect;
                httpRequest.CookieContainer = this.CookieContainer;
                httpRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            }

            return this._request;
        }

        #endregion
    }
}