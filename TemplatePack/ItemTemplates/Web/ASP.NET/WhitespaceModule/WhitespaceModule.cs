using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;

/// <summary>
/// Removes whitespace from the webpage. Be careful!
/// This doesn't work when dynamicCompressionBeforeCache="true" in web.config on IIS 7+
/// </summary>
public class $safeitemname$ : IHttpModule
{
    #region IHttpModule Members

    void IHttpModule.Dispose()
    {
        // Nothing to dispose; 
    }

    void IHttpModule.Init(HttpApplication context)
    {
        context.PostRequestHandlerExecute += PostRequestHandlerExecute;
    }

    #endregion

    private void PostRequestHandlerExecute(object sender, EventArgs e)
    {
        HttpApplication app = sender as HttpApplication;
        
        if (app.Response.ContentType.Equals("text/html", StringComparison.OrdinalIgnoreCase))
        {
            app.Response.Filter = new WhitespaceFilter(app.Response.Filter);
        }
    }

    #region Stream filter

    private class WhitespaceFilter : Stream
    {

        public WhitespaceFilter(Stream sink)
        {
            _sink = sink;
        }

        private Stream _sink;

        #region Properites

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override void Flush()
        {
            _sink.Flush();
        }

        public override long Length
        {
            get { return 0; }
        }

        private long _position;
        public override long Position
        {
            get { return _position; }
            set { _position = value; }
        }

        #endregion

        #region Methods

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _sink.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _sink.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _sink.SetLength(value);
        }

        public override void Close()
        {
            _sink.Close();
        }

        private static readonly Regex REGEX_BETWEEN_TAGS = new Regex(@">\s+<", RegexOptions.Compiled);
        private static readonly Regex REGEX_LINE_BREAKS = new Regex(@"\n\s+", RegexOptions.Compiled);

        public override void Write(byte[] buffer, int offset, int count)
        {
            byte[] data = new byte[count];
            Buffer.BlockCopy(buffer, offset, data, 0, count);
            string html = System.Text.Encoding.Default.GetString(buffer);

            html = Regex.Replace(html, @">\s+<", "><");
            html = Regex.Replace(html, @"\s+", " ");

            byte[] outdata = System.Text.Encoding.Default.GetBytes(html.Trim());
            _sink.Write(outdata, 0, outdata.GetLength(0));
        }

        #endregion

    }

    #endregion

}