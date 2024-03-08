using InStockWebAppBLL.Models.UserVM;

namespace InStockWebAppBLL.Features.Interfaces.Domain
{
    public interface IUserRepository
    {
        Task<bool> Create(CreateUserVM createUserVM);
        Task<IEnumerable<GetAllUserVM>> getAll();
        Task<DateTime?> ToggleStatus(string id);
    }
}
