namespace Extensions.Hosting.ApplicationInitializer
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IApplicationInitializer
    {
        Task InitializeAsync(CancellationToken cancellationToken);
    }
}