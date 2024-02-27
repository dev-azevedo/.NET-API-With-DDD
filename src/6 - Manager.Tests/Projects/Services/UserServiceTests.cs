
using AutoMapper;
using Bogus.DataSets;
using EscNet.Cryptography.Interfaces;
using FluentAssertions;
using Manager.Domain.Entities;
using Manager.Infra.Interfaces;
using Manager.Services.DTO;
using Manager.Services.Interfaces;
using Manager.Services.Services;
using Manager.Tests.Configuration;
using Moq;

namespace Manager.Tests.Projects.Services
{
    public class UserServiceTests
    {
        private readonly IUserService _sut;
        private readonly IMapper _mapper;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IRijndaelCryptography> _rijndaelCryptographyMock;

        public UserServiceTests()
        {
            _mapper = AutoMapperConfiguration.GetConfiguration();
            _userRepositoryMock = new Mock<IUserRepository>();
            _rijndaelCryptographyMock = new Mock<IRijndaelCryptography>();
            _sut = new UserService(
                    mapper: _mapper, 
                    userRepository: _userRepositoryMock.Object, 
                    rijdaelCryptography: _rijndaelCryptographyMock.Object
                );
        }

        [Fact(DisplayName = "Create Valid User")]
        [Trait("Category", "Servicess")]
        public async Task Create_WhenUserIsValid_ReturnsUserDTO()
        {
            // Given
            var userToCreate = new UserDTO{Name = "Jhow", Email = "jhow@azevedo.com", Password = "Dev@123"};
            
            var encryptedPassword = new Lorem().Sentence();

            var userCreated = _mapper.Map<User>(userToCreate);

            userCreated.ChangePassword(encryptedPassword);

            _userRepositoryMock.Setup(x => x.GetByEmail(It.IsAny<string>())).ReturnsAsync(() => null);

            _rijndaelCryptographyMock.Setup(x => x.Encrypt(It.IsAny<string>())).Returns(encryptedPassword);
            
            _userRepositoryMock.Setup(x => x.Create(It.IsAny<User>())).ReturnsAsync(() => userCreated);

            // When
            var result = await _sut.Create(userToCreate);

            // Then
            result.Should().BeEquivalentTo(_mapper.Map<UserDTO>(userCreated));
        }
    }
}