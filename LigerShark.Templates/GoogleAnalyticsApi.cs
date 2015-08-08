﻿﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace LigerShark.Templates
{
    public class GoogleAnalyticsApi
    {
        /*
        *   Author: Tyler Hughes
        *   Credit for the Track function and the Enum HitType goes to 0liver (https://gist.github.com/0liver/11229128)
        *   Credit goes to spyriadis (http://www.spyriadis.net/2014/07/google-analytics-measurement-protocol-track-events-c/)
        *       for the idea of putting the values for each tracking method in its own function
        *
        *   Documentation of the Google Analytics Measurement Protocol can be found at:
        *   https://developers.google.com/analytics/devguides/collection/protocol/v1/devguide
        */

        private string endpoint = "http://www.google-analytics.com/collect";
        private string googleVersion = "1";
        private string googleTrackingID = "UA-XXXX-Y";
        private string googleClientID = "555";

        public GoogleAnalyticsApi(string trackingID, string clientID)
        {
            this.googleTrackingID = trackingID;
            this.googleClientID = clientID;
        }

        public void TrackEvent(string category, string action, string label, int? value = null)
        {
            if (string.IsNullOrEmpty(category)) throw new ArgumentNullException("category");
            if (string.IsNullOrEmpty(action)) throw new ArgumentNullException("action");

            var values = DefaultValues;

            values.Add("t", HitType.@event.ToString());             // Event hit type
            values.Add("ec", category);                             // Event Category. Required.
            values.Add("ea", action);                               // Event Action. Required.
            if (label != null) values.Add("el", label);             // Event label.
            if (value != null) values.Add("ev", value.ToString());  // Event value.

            Track(values);
        }

        public void TrackPageview(string category, string action, string label, int? value = null)
        {
            if (string.IsNullOrEmpty(category)) throw new ArgumentNullException("category");
            if (string.IsNullOrEmpty(action)) throw new ArgumentNullException("action");

            var values = DefaultValues;

            values.Add("t", HitType.@pageview.ToString());          // Event hit type
            values.Add("ec", category);                             // Event Category. Required.
            values.Add("ea", action);                               // Event Action. Required.
            if (label != null) values.Add("el", label);             // Event label.
            if (value != null) values.Add("ev", value.ToString());  // Event value.

            Track(values);
        }

        private void Track(Dictionary<string, string> values)
        {
            var request = (HttpWebRequest)WebRequest.Create(endpoint);
            request.Method = "POST";
            request.KeepAlive = false;

            var postDataString = values
                .Aggregate("", (data, next) => string.Format("{0}&{1}={2}", data, next.Key,
                                                             HttpUtility.UrlEncode(next.Value)))
                .TrimEnd('&');

            // set the Content-Length header to the correct value
            request.ContentLength = Encoding.UTF8.GetByteCount(postDataString);

            // write the request body to the request
            using (var writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(postDataString);
            }

            try
            {
                // Send the response to the server
                var webResponse = (HttpWebResponse)request.GetResponse();

                if (webResponse.StatusCode != HttpStatusCode.OK)
                {
                    throw new HttpException((int)webResponse.StatusCode, "Google Analytics tracking did not return OK 200");
                }

                webResponse.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private enum HitType
        {
            // ReSharper disable InconsistentNaming
            @event,
            @pageview,
            // ReSharper restore InconsistentNaming
        }

        private Dictionary<string, string> DefaultValues
        {
            get
            {
                var data = new Dictionary<string, string>();
                data.Add("v", googleVersion);         // The protocol version. The value should be 1.
                data.Add("tid", googleTrackingID);    // Tracking ID / Web property / Property ID.
                data.Add("cid", googleClientID);      // Anonymous Client ID (must be unique).
                return data;
            }
        }
    }
}
