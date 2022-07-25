using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Restaurant.Backend.Account.Controllers;
using Restaurant.Backend.Dto.Account;

namespace Restaurant.Backend.Account.Test
{
    public class CompanyControllerTest : BaseControllerTest<CompanyController>
    {
        [Test]
        public void LoginTest()
        {
            // Setup
            var controller = new CompanyController(LoggerController.Object, Config.Object, Mapper);

            // Act
            var result = controller.Login(new CompanyLoginDto()).Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreSame(typeof(OkObjectResult), result.GetType());
            Assert.IsNotEmpty($"{(result as OkObjectResult)?.Value}");

        }
    }
}
