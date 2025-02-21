namespace BlazorApp.Server.Builder
{
    public interface IBuilder<T>
    {
        T Build();
        void Reset();
    }
}
