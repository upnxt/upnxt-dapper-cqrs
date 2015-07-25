
namespace Data
{
    public interface ICommand
    {
        void Execute(ISession session);
    }
}