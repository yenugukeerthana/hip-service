namespace In.ProjectEKA.HipService.Common
{
    using System;
    using System.Net.Http;
    using System.Net.Mime;
    using System.Text;
    using Microsoft.Net.Http.Headers;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using static Constants;

    public static class HttpRequestHelper
    {
        public static HttpRequestMessage CreateHttpRequest<T>(
            HttpMethod method,
            string url,
            T content,
            string token,
            string cmSuffix,
            string correlationId,
            string xtoken = null)
        {
            var json = JsonConvert.SerializeObject(content, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            });
            
            var httpRequestMessage = new HttpRequestMessage(method, new Uri($"{url}"))
            {
                Content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json)
            };

            if (token != null)
                httpRequestMessage.Headers.Add(HeaderNames.Authorization, token);
            if(xtoken != null)
                httpRequestMessage.Headers.Add("X-Token", xtoken);
            if (cmSuffix != null)
                httpRequestMessage.Headers.Add("X-CM-ID", cmSuffix);
            if (correlationId != null)
                httpRequestMessage.Headers.Add(CORRELATION_ID, correlationId);
            return httpRequestMessage;
        }

        public static HttpRequestMessage CreateHttpRequest<T>(HttpMethod method,string url, T content, String correlationId)
        {
            // ReSharper disable once IntroduceOptionalParameters.Global
            return CreateHttpRequest(method,url, content, null, null, correlationId);
        }
    }
}
