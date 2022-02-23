using Castle.DynamicProxy.Generators.Emitters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SkysUnitTesting.Models;
using SkysUnitTesting.Services;

namespace TestProject1.Services;


// FAKE OBJECT
// public class FakeUserRepository : IUserRegistrationRepository
//{
//    public UserRegistration Get(string email)
//    {
//        if (email == "hej@hej.com") return new UserRegistration();
//        return null;
//    }

//    public void CreateNew(string email)
//    {
//        throw new System.NotImplementedException();
//    }
//}

[TestClass]
public class RegistrationServiceTests
{
    private RegistrationService sut;
    private Mock<IUserRegistrationRepository> repositoryMock;
    private readonly Mock<IEmailService> emailMock;

    public RegistrationServiceTests()
    {
        repositoryMock = new Mock<IUserRegistrationRepository>();
        emailMock = new Mock<IEmailService>();
        sut = new RegistrationService(repositoryMock.Object, emailMock.Object);
    }

    [TestMethod]
    public void ShouldReturnInvalidDomainErrorCodeWhenNotCorrectEmaiDomainIsUsed()
    {
        var result = sut.RegisterUser("hej@hej2.com");
        Assert.AreEqual(IRegistrationService.RegistrationStatus.WrongEmailDomain, result);
        //kolla så att hej.se eller hej.com
    }


    [TestMethod]
    public void ShouldReturnAlreadyRegisteredErrorCodeWhenAlreadyRegistered()
    {
        //ARRANGE
        repositoryMock.Setup(a => a.Get("hej@hej.com"))
            .Returns(new UserRegistration());

        //ACT
        var result = sut.RegisterUser("hej@hej.com");

        //ASSERT
        Assert.AreEqual(IRegistrationService.RegistrationStatus.AlreadyRegistered, result);
        //kolla så att hej.se eller hej.com
    }

    [TestMethod]
    public void ShouldSendEmailWhenRegistrationIsOk()
    {
        string email = "hej2222@hej.com";

        repositoryMock.Setup(a => a.Get(email)).Returns((UserRegistration) null);



        var result = sut.RegisterUser(email);

        emailMock.Verify(e=>e.SendEmail(email), Times.Once);

        
        Assert.AreEqual(IRegistrationService.RegistrationStatus.Ok, result);
        //Har det skickats email??
        //kolla så att hej.se eller hej.com
    }


}