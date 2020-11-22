using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnit.Framework
{
    class AccountController
    {
        public class PasswordControllerTestFixture
        {
            [Test]
            public void TestValidateEmail(string email, bool expectedResult)
            {
                // Arrange
                var accountController = new AccountController();

                // Act
                var actualResult = accountController.ValidateEmail(email);

                // Assert
                Assert.AreEqual(expectedResult, actualResult);
            }
        }
    }
}
