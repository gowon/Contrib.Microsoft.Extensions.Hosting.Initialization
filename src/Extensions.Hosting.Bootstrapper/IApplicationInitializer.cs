namespace Extensions.Hosting.Bootstrapper
{
    using System.Threading.Tasks;

    public interface IApplicationInitializer
    {
        Task InitializeAsync();
    }
}