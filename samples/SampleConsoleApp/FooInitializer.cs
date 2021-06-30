namespace SampleConsoleApp
{
    using System;
    using System.Threading.Tasks;
    using Extensions.Hosting.Bootstrapper;

    public class FooInitializer : IApplicationInitializer
    {
        private readonly Foo _foo;

        public FooInitializer(Foo foo)
        {
            _foo = foo;
        }

        public Task InitializeAsync()
        {
            _foo.Value = DateTime.Now.ToLongDateString();
            return Task.CompletedTask;
        }
    }
}