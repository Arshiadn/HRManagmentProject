namespace HrApi.Interfaces;

public interface IReadRepository<T>
{
    Task<IReadOnlyList<T>> ListAsync(
    ISpecification<T> specification,
    CancellationToken cancellationToken);

    Task<int> CountAsync(
        ISpecification<T> specification,
        CancellationToken cancellationToken);

    Task<T?> FirstOrDefaultAsync(
        ISpecification<T> specification,
        CancellationToken cancellationToken);
}
