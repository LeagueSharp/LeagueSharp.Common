#region LICENSE

/*
 Copyright 2014 - 2014 LeagueSharp
 BetterWebClient.cs is part of LeagueSharp.Common.
 
 LeagueSharp.Common is free software: you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation, either version 3 of the License, or
 (at your option) any later version.
 
 LeagueSharp.Common is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 GNU General Public License for more details.
 
 You should have received a copy of the GNU General Public License
 along with LeagueSharp.Common. If not, see <http://www.gnu.org/licenses/>.
*/

#endregion

#region

using System;
using System.Net;

#endregion

//Taken from http://keestalkstech.com/2014/03/a-slightly-better-webclient-class/

namespace LeagueSharp.Common
{
    public class BetterWebClient : WebClient
    {
        private WebRequest _request;

        public BetterWebClient(CookieContainer cookies, bool autoRedirect = true)
        {
            CookieContainer = cookies ?? new CookieContainer();
            AutoRedirect = autoRedirect;
        }

        //Gets or sets whether to automatically follow a redirect
        public bool AutoRedirect { get; set; }

        //Gets or sets the cookie container, contains all the 
        //cookies for all the requests
        public CookieContainer CookieContainer { get; set; }

        //Gets last cookie header
        public string Cookies
        {
            get { return GetHeaderValue("Set-Cookie"); }
        }

        //Get last location header
        public string Location
        {
            get { return GetHeaderValue("Location"); }
        }

        //Get last status code
        public HttpStatusCode StatusCode
        {
            get
            {
                var result = HttpStatusCode.BadRequest;

                if (_request != null)
                {
                    var response = base.GetWebResponse(_request) as HttpWebResponse;

                    if (response != null)
                    {
                        result = response.StatusCode;
                    }
                }

                return result;
            }
        }

        public string GetHeaderValue(string headerName)
        {
            string result = null;

            if (_request != null)
            {
                var response = base.GetWebResponse(_request) as HttpWebResponse;
                if (response != null)
                {
                    result = response.Headers[headerName];
                }
            }

            return result;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            _request = base.GetWebRequest(address);

            var httpRequest = _request as HttpWebRequest;

            if (httpRequest != null)
            {
                httpRequest.AllowAutoRedirect = AutoRedirect;
                httpRequest.CookieContainer = CookieContainer;
                httpRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            }

            return _request;
        }
    }
}