namespace SampleConsoleApp
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using Extensions.Hosting.Bootstrapper;

    public class SampleInitializer : IApplicationInitializer
    {
        private readonly HttpClient _httpClient;

        public SampleInitializer(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task InitializeAsync()
        {
            _ = await _httpClient.GetStringAsync("https://bing.com");
        }
    }
}