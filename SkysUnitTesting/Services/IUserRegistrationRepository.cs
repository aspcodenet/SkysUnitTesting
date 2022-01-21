using SkysUnitTesting.Models;

namespace SkysUnitTesting.Services;

public interface IUserRegistrationRepository
{
    UserRegistration Get(string email);
    void CreateNew(string email);
}