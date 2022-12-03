using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace FarmAdvisor.Common.Test.Infrastructure
{
    public class FakeHttpRequest : HttpRequest
    {
        public FakeHttpRequest(Uri url, Stream body = null)
        {
            Scheme = url.Scheme;
            IsHttps = url.Scheme == "https";
            Path = url.AbsolutePath;
            QueryString = new QueryString(url.Query);
            Body = body ?? new MemoryStream();
            if (!string.IsNullOrWhiteSpace(url.Query))
            {
                var queryStringText = url.Query;
                if (queryStringText.StartsWith('?'))
                {
                    queryStringText = queryStringText[1..];
                }
                var queries = queryStringText.Split('&').Select(c =>
                   {
                       var text = c.Split('=');
                       return new KeyValuePair<string, string>(text[0], text[1]);
                   }).ToDictionary(c => c.Key, v => new StringValues(v.Value));
                Query = new QueryCollection(queries);
            }
        }

        public override Stream Body { get; set; } = new MemoryStream();

        public override IHeaderDictionary Headers { get; } = new HeaderDictionary();

        public override IRequestCookieCollection Cookies { get; set; }

        public override string Method { get; set; }

        public override HttpContext HttpContext => new DefaultHttpContext();

        public override string Scheme { get; set; }
        public override bool IsHttps { get; set; }
        public override HostString Host { get; set; }
        public override PathString PathBase { get; set; }
        public override PathString Path { get; set; }
        public override QueryString QueryString { get; set; }
        public override IQueryCollection Query { get; set; }
        public override string Protocol { get; set; }
        public override long? ContentLength { get; set; }
        public override string ContentType { get; set; }

        public override bool HasFormContentType => false;

        public override IFormCollection Form { get; set; }

        public static FakeHttpRequest Create(string url, Stream body = null)
        {
            return new FakeHttpRequest(new Uri(url), body);
        }

        public override Task<IFormCollection> ReadFormAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}