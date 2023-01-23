using Moq;
using SkysUnitTesting.Models;
using SkysUnitTesting.Services;
using static SkysUnitTesting.Services.IRegistrationService;

namespace SkysUnitTestingTests.Services;

//public class FakeUserRepository : IUserRegistrationRepository
//{
//    public bool CreateNewHasBeenCalled = false;
//    public List<string> RegisteredEmails = new List<string>();
//    public UserRegistration Get(string email)
//    {
//        if (RegisteredEmails.Contains(email))
//            return new UserRegistration();
//        return null;
//    }

//    public void CreateNew(string email)
//    {
//        CreateNewHasBeenCalled = true;
//    }
//}

//public class FakeEmailService : IEmailService
//{
//    public bool SendEmailhasBeenCalled = false;
//    public void SendEmail(string email)
//    {
//        SendEmailhasBeenCalled = true;
//    }
//}

[TestClass]
public class RegistrationServiceTests
{
    private RegistrationService sut;
    private Mock<IUserRegistrationRepository> userRepositoryMock;
    private Mock<IEmailService> emailServiceMock;

    public RegistrationServiceTests()
    {
        userRepositoryMock = new Mock<IUserRegistrationRepository>();
        emailServiceMock = new Mock<IEmailService>();

        sut = new RegistrationService(userRepositoryMock.Object, emailServiceMock.Object);
    }

    [TestMethod]
    public void When_using_invalid_domain_then_error_code_is_returned()
    {
        //ARRANGE
        var email = "aaa@aaa.se";
        //ACT
        var result = sut.RegisterUser(email);

        //ASSERT
        Assert.AreEqual(RegistrationStatus.WrongEmailDomain, result);
    }


    [TestMethod]
    public void When_user_exists_then_error_code_is_returned()
    {
        //ARRANGE
        var email = "aaa@hej.se";
        userRepositoryMock.Setup(userRepository => userRepository.Get(email)).Returns(new UserRegistration());
        
        //ACT
        var result = sut.RegisterUser(email);

        //ASSERT
        Assert.AreEqual(RegistrationStatus.AlreadyRegistered, result);
    }


    [TestMethod]
    public void When_ok_user_is_saved()
    {
        //ARRANGE
        var email = "aaa@hej.se";

        //ACT
        var result = sut.RegisterUser(email);

        //ASSERT
        userRepositoryMock.Verify(userRep=> userRep.CreateNew(email), Times.Once());

    }

    [TestMethod]
    public void When_ok_email_is_sent()
    {
        //ARRANGE
        var email = "aaa@hej.se";

        //ACT
        var result = sut.RegisterUser(email);

        //ASSERT
        emailServiceMock.Verify(emailSer=> emailSer.SendEmail(email), Times.Once());

    }




    //[TestInitialize]
    //public void TestInitialze()
    //{
    //    sut = new RegistrationService();
    //}
}