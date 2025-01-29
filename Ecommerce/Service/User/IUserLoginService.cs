namespace Ecommerce.Service;

public interface IUserLoginService<TL>
{
    string Authenticate(TL TLEntityLoginDto);
}