using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Online_Store_ASP.NET_Core_MVC.Controllers;
using Online_Store_ASP.NET_Core_MVC.Models;
using Xunit;

namespace Online_Store_ASP.NET_Core_MVC.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
        private readonly Mock<RoleManager<IdentityRole>> _roleManagerMock;
        private readonly IConfiguration _configuration;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            var userStoreMock = new Mock<IUserStore<IdentityUser>>();
            _userManagerMock = new Mock<UserManager<IdentityUser>>(
                userStoreMock.Object, null!, null!, null!, null!, null!, null!, null!, null!);

            var roleStoreMock = new Mock<IRoleStore<IdentityRole>>();
            _roleManagerMock = new Mock<RoleManager<IdentityRole>>(
                roleStoreMock.Object, null!, null!, null!, null!);

            var configData = new Dictionary<string, string?>
            {
                { "Jwt:Token", "ThisIsASecretKeyForTestingPurposesOnly123!" },
                { "Jwt:ValidateIssuer", "https://test-issuer.com" },
                { "Jwt:ValidateAudience", "https://test-audience.com" }
            };
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(configData)
                .Build();

            _controller = new AuthController(
                _userManagerMock.Object,
                _roleManagerMock.Object,
                _configuration);
        }

        #region SetRoles Tests

        [Fact]
        public async Task SetRoles_WhenAllRolesExist_ReturnsOkAlreadyDone()
        {
            _roleManagerMock.Setup(r => r.RoleExistsAsync(UsersRoles.USER)).ReturnsAsync(true);
            _roleManagerMock.Setup(r => r.RoleExistsAsync(UsersRoles.ADMIN)).ReturnsAsync(true);
            _roleManagerMock.Setup(r => r.RoleExistsAsync(UsersRoles.OWNER)).ReturnsAsync(true);

            var result = await _controller.SetRoles();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Set Role, It has already been Done", okResult.Value);
        }

        [Fact]
        public async Task SetRoles_WhenNoRolesExist_CreatesAllRolesAndReturnsOk()
        {
            _roleManagerMock.Setup(r => r.RoleExistsAsync(UsersRoles.USER)).ReturnsAsync(false);
            _roleManagerMock.Setup(r => r.RoleExistsAsync(UsersRoles.ADMIN)).ReturnsAsync(false);
            _roleManagerMock.Setup(r => r.RoleExistsAsync(UsersRoles.OWNER)).ReturnsAsync(false);
            _roleManagerMock.Setup(r => r.CreateAsync(It.IsAny<IdentityRole>()))
                .ReturnsAsync(IdentityResult.Success);

            var result = await _controller.SetRoles();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Successfull, Set Role", okResult.Value);
            _roleManagerMock.Verify(r => r.CreateAsync(It.Is<IdentityRole>(
                role => role.Name == UsersRoles.USER)), Times.Once);
            _roleManagerMock.Verify(r => r.CreateAsync(It.Is<IdentityRole>(
                role => role.Name == UsersRoles.ADMIN)), Times.Once);
            _roleManagerMock.Verify(r => r.CreateAsync(It.Is<IdentityRole>(
                role => role.Name == UsersRoles.OWNER)), Times.Once);
        }

        [Fact]
        public async Task SetRoles_WhenSomeRolesExist_CreatesAllRolesAndReturnsOk()
        {
            _roleManagerMock.Setup(r => r.RoleExistsAsync(UsersRoles.USER)).ReturnsAsync(true);
            _roleManagerMock.Setup(r => r.RoleExistsAsync(UsersRoles.ADMIN)).ReturnsAsync(false);
            _roleManagerMock.Setup(r => r.RoleExistsAsync(UsersRoles.OWNER)).ReturnsAsync(true);
            _roleManagerMock.Setup(r => r.CreateAsync(It.IsAny<IdentityRole>()))
                .ReturnsAsync(IdentityResult.Success);

            var result = await _controller.SetRoles();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Successfull, Set Role", okResult.Value);
        }

        #endregion

        #region Register Tests

        [Fact]
        public async Task Register_WhenUserAlreadyExists_ReturnsBadRequest()
        {
            var registerDto = new RegisterDto
            {
                UserName = "existingUser",
                Email = "test@test.com",
                Password = "Pass123"
            };
            var existingUser = new IdentityUser { UserName = "existingUser" };
            _userManagerMock.Setup(u => u.FindByNameAsync("existingUser"))
                .ReturnsAsync(existingUser);

            var result = await _controller.Register(registerDto);

            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("Username already exists", badResult.Value!.ToString());
        }

        [Fact]
        public async Task Register_WhenCreateFails_ReturnsBadRequestWithErrors()
        {
            var registerDto = new RegisterDto
            {
                UserName = "newUser",
                Email = "new@test.com",
                Password = "weak"
            };
            _userManagerMock.Setup(u => u.FindByNameAsync("newUser"))
                .ReturnsAsync((IdentityUser?)null);
            _userManagerMock.Setup(u => u.CreateAsync(It.IsAny<IdentityUser>(), "weak"))
                .ReturnsAsync(IdentityResult.Failed(
                    new IdentityError { Description = "Password too short" }));

            var result = await _controller.Register(registerDto);

            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("Password too short", badResult.Value!.ToString());
        }

        [Fact]
        public async Task Register_WhenSuccessful_ReturnsOkAndAssignsUserRole()
        {
            var registerDto = new RegisterDto
            {
                UserName = "newUser",
                Email = "new@test.com",
                Password = "StrongPass1!"
            };
            _userManagerMock.Setup(u => u.FindByNameAsync("newUser"))
                .ReturnsAsync((IdentityUser?)null);
            _userManagerMock.Setup(u => u.CreateAsync(It.IsAny<IdentityUser>(), "StrongPass1!"))
                .ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(u => u.AddToRoleAsync(It.IsAny<IdentityUser>(), UsersRoles.USER))
                .ReturnsAsync(IdentityResult.Success);

            var result = await _controller.Register(registerDto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Successfull, User Created", okResult.Value);
            _userManagerMock.Verify(u => u.AddToRoleAsync(
                It.IsAny<IdentityUser>(), UsersRoles.USER), Times.Once);
        }

        #endregion

        #region Login Tests

        [Fact]
        public async Task Login_WhenUserNotFound_ReturnsUnauthorized()
        {
            var loginDto = new LoginDto { UserName = "nonexistent", Password = "pass" };
            _userManagerMock.Setup(u => u.FindByNameAsync("nonexistent"))
                .ReturnsAsync((IdentityUser?)null);

            var result = await _controller.Login(loginDto);

            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Invalid Creden", unauthorizedResult.Value);
        }

        [Fact]
        public async Task Login_WhenPasswordIncorrect_ReturnsUnauthorized()
        {
            var loginDto = new LoginDto { UserName = "testUser", Password = "wrongpass" };
            var user = new IdentityUser { UserName = "testUser", Id = "user-id-1" };
            _userManagerMock.Setup(u => u.FindByNameAsync("testUser"))
                .ReturnsAsync(user);
            _userManagerMock.Setup(u => u.CheckPasswordAsync(user, "wrongpass"))
                .ReturnsAsync(false);

            var result = await _controller.Login(loginDto);

            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Invalid Password", unauthorizedResult.Value);
        }

        [Fact]
        public async Task Login_WhenCredentialsValid_ReturnsOkWithToken()
        {
            var loginDto = new LoginDto { UserName = "testUser", Password = "correctpass" };
            var user = new IdentityUser { UserName = "testUser", Id = "user-id-1" };
            _userManagerMock.Setup(u => u.FindByNameAsync("testUser"))
                .ReturnsAsync(user);
            _userManagerMock.Setup(u => u.CheckPasswordAsync(user, "correctpass"))
                .ReturnsAsync(true);
            _userManagerMock.Setup(u => u.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { UsersRoles.USER });

            var result = await _controller.Login(loginDto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
            var token = okResult.Value!.ToString();
            Assert.False(string.IsNullOrWhiteSpace(token));
        }

        [Fact]
        public async Task Login_WhenUserHasMultipleRoles_ReturnsToken()
        {
            var loginDto = new LoginDto { UserName = "adminUser", Password = "adminpass" };
            var user = new IdentityUser { UserName = "adminUser", Id = "admin-id-1" };
            _userManagerMock.Setup(u => u.FindByNameAsync("adminUser"))
                .ReturnsAsync(user);
            _userManagerMock.Setup(u => u.CheckPasswordAsync(user, "adminpass"))
                .ReturnsAsync(true);
            _userManagerMock.Setup(u => u.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { UsersRoles.USER, UsersRoles.ADMIN });

            var result = await _controller.Login(loginDto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        #endregion

        #region SetRoleAdmin Tests

        [Fact]
        public async Task SetRoleAdmin_WhenUserNotFound_ReturnsBadRequest()
        {
            var dto = new UpdateRoleDto { UserName = "nonexistent" };
            _userManagerMock.Setup(u => u.FindByNameAsync("nonexistent"))
                .ReturnsAsync((IdentityUser?)null);

            var result = await _controller.SetRoleAdmin(dto);

            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid UserName ", badResult.Value);
        }

        [Fact]
        public async Task SetRoleAdmin_WhenUserExists_AddsAdminRoleAndReturnsOk()
        {
            var dto = new UpdateRoleDto { UserName = "testUser" };
            var user = new IdentityUser { UserName = "testUser" };
            _userManagerMock.Setup(u => u.FindByNameAsync("testUser"))
                .ReturnsAsync(user);
            _userManagerMock.Setup(u => u.AddToRoleAsync(user, UsersRoles.ADMIN))
                .ReturnsAsync(IdentityResult.Success);

            var result = await _controller.SetRoleAdmin(dto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Successful", okResult.Value);
            _userManagerMock.Verify(u => u.AddToRoleAsync(user, UsersRoles.ADMIN), Times.Once);
        }

        #endregion
    }
}
