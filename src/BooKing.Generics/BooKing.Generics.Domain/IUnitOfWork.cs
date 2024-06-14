namespace BooKing.Generics.Domain;
public interface IUnitOfWork
{
    Task Commit();
}
