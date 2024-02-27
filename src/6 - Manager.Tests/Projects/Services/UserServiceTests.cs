
using AutoMapper;
using EscNet.Cryptography.Interfaces;
using Manager.Infra.Interfaces;
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
             _sut = new UserService(mapper: _mapper, userRepository: _userRepositoryMock.Object, rijdaelCryptography: _rijndaelCryptographyMock.Object);
        }
    }
}