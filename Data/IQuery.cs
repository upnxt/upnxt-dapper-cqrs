
namespace Data
{
    public interface IQuery<out T>
    {
        T Execute(ISession session);
    }
}