namespace SampleConsoleApp
{
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Extensions.Hosting.ApplicationInitializer;

    public class SampleInitializer : IApplicationInitializer
    {
        private readonly HttpClient _httpClient;

        public SampleInitializer(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task InitializeAsync(CancellationToken cancellationToken)
        {
            var content = await _httpClient.GetStringAsync("https://bing.com");
        }
    }
}