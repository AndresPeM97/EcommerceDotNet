namespace Ecommerce.Service;

public interface IUserService<T, TI, TU>
{
    Task<IEnumerable<T>> Get();
    Task<T> Add(TI TIEntityInsertDto);
    Task<T> Update(TU TUEntityUpdateDto);
    Task<T> Delete(int id);
    bool Validate(TI TIEntityInsertDto);
    bool Validate(TU TUEntityUpdateDto);
}