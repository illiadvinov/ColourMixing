namespace CodeBase.Infrastructure.Pool
{
    public interface IPool<T>
    {
        void Initialize();
        void Add(T t);
        T GetRandom();
        T GetSpecific(int index);
    }
}