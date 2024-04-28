using Microsoft.Extensions.Logging;
using Moq;
using TopUpService.Common.Entities;
using TopUpService.Common.Repositiory;
using TopUpService.Common.RequestModel;
using TopUpService.Common.ResponseModel;
using TopUpService.Infrastructure.Service;

namespace TopUpService.API.Test.Services
{
    public class BeneficiaryServiceTest : TestBase
    {
        private Mock<IBeneficiaryRepository> _beneficiaryRepository;
        private Mock<ILogger<BeneficiaryService>> _logger;

        public BeneficiaryServiceTest()
        {
            _beneficiaryRepository = new Mock<IBeneficiaryRepository>();
            _logger = new Mock<ILogger<BeneficiaryService>>();
        }

        [Fact]
        public void AddNewBeneficiaryTest_shoud_return_created_true()
        {
            //Arrange
            var result = new GenericResponseModel(true);
            _beneficiaryRepository.Setup(a => a.AddBeneficiary(It.IsAny<AddNewBeneficiaryRequestModel>())).Returns(result);
            _beneficiaryRepository.Setup(a => a.CheckBeneficiaryExistance(It.IsAny<string>())).Returns(false);
            _beneficiaryRepository.Setup(a => a.GetByUserId(It.IsAny<int>())).Returns(new List<Common.Entities.Beneficiary>());

            //Act
            var service = new BeneficiaryService(_logger.Object, _beneficiaryRepository.Object);
            var response = service.AddNewBeneficiary(new AddNewBeneficiaryRequestModel
            {
                Name = "Test",
                UserId = 1
            });

            //Assert
            _beneficiaryRepository.Verify(a => a.AddBeneficiary(It.IsAny<AddNewBeneficiaryRequestModel>()), Times.Once);
            _beneficiaryRepository.Verify(a => a.CheckBeneficiaryExistance("Test"), Times.Once);
            _beneficiaryRepository.Verify(a => a.GetByUserId(1), Times.Once);
            Assert.Equal(result.Message, response.Message);
            Assert.True(response.IsSuccess);
        }

        [Fact]
        public void AddNewBeneficiaryTest_validation_error_more_than_5()
        {
            //Arrange
            var result = new GenericResponseModel(false, "User Exceed Number Of Available Beneficiaries.");
            var userBeneficiaries = new List<Common.Entities.Beneficiary>
            {
                new Beneficiary{Id=Guid.Empty},
                new Beneficiary{Id=Guid.Empty},
                new Beneficiary{Id=Guid.Empty},
                new Beneficiary{Id=Guid.Empty},
                new Beneficiary{Id=Guid.Empty},
            };
            _beneficiaryRepository.Setup(a => a.AddBeneficiary(It.IsAny<AddNewBeneficiaryRequestModel>())).Returns(result);
            _beneficiaryRepository.Setup(a => a.CheckBeneficiaryExistance(It.IsAny<string>())).Returns(false);
            _beneficiaryRepository.Setup(a => a.GetByUserId(It.IsAny<int>())).Returns(userBeneficiaries);

            //Act
            var service = new BeneficiaryService(_logger.Object, _beneficiaryRepository.Object);
            var response = service.AddNewBeneficiary(new AddNewBeneficiaryRequestModel
            {
                Name = "Test",
                UserId = 1
            });

            //Assert
            _beneficiaryRepository.Verify(a => a.AddBeneficiary(It.IsAny<AddNewBeneficiaryRequestModel>()), Times.Never);
            _beneficiaryRepository.Verify(a => a.CheckBeneficiaryExistance("Test"), Times.Once);
            _beneficiaryRepository.Verify(a => a.GetByUserId(1), Times.Once);
            Assert.Equal(result.Message, response.Message);
            Assert.False(response.IsSuccess);
        }

        [Fact]
        public void GetAllUserBeneficiaries_should_return_list_of_beneficiaries()
        {
            //Arrange
            var userBeneficiaries = new List<Common.Entities.Beneficiary>
            {
                new Beneficiary{Id=Guid.Empty,UserId=1},
                new Beneficiary{Id=Guid.Empty,UserId=1},
                new Beneficiary{Id=Guid.Empty,UserId=1},
                new Beneficiary{Id=Guid.Empty,UserId=1},
                new Beneficiary{Id=Guid.Empty,UserId=1},
            };
            _beneficiaryRepository.Setup(a => a.GetByUserId(It.IsAny<int>())).Returns(userBeneficiaries);

            //Act
            var service = new BeneficiaryService(_logger.Object, _beneficiaryRepository.Object);
            var response = service.GetAllUserBeneficiaries(1);

            //Assert
            _beneficiaryRepository.Verify(a => a.GetByUserId(1), Times.Once);
            Assert.Equal(userBeneficiaries.Count, response.Count);
        }

        [Fact]
        public void GetBeneficiaryBalance_should_return_balance()
        {
            //Arrange
            var balance = new BeneficiaryResponseModel
            {
                Balance = 500,
                Id = Guid.Empty,
                Name = "Test"
            };
            _beneficiaryRepository.Setup(a => a.GetBeneficiaryBalance(It.IsAny<Guid>())).Returns(balance);

            //Act
            var service = new BeneficiaryService(_logger.Object, _beneficiaryRepository.Object);
            var response = service.GetBeneficiaryBalance(Guid.Empty);

            //Assert
            Assert.Equal(balance.Balance, response.Balance);
            Assert.Equal(balance.Name, response.Name);
            Assert.Equal(balance.Id, response.Id);
        }

        [Fact]
        public void TopUpBeneficiary_should_top_up_balance()
        {
            //Arrange
            var result = new GenericResponseModel(true);

            var request = new TopUpRequestModel
            {
                BeneficiaryId = Guid.Empty,
                IsVerified = true,
                TopUpValue = 0,
                UserId = 1
            };

            _beneficiaryRepository.Setup(a => a.TopUpBeneficiary(It.IsAny<TopUpRequestModel>())).Returns(result);
            _beneficiaryRepository.Setup(a => a.WithdrawUserBalance(It.IsAny<int>(), It.IsAny<decimal>(), It.IsAny<int>())).Returns(result);
            _beneficiaryRepository.Setup(a => a.GetBeneficiaryById(It.IsAny<Guid>())).Returns(new Beneficiary
            {
                Id = Guid.Empty,
                UserId = 1,
                Name = "Test",
                Balance = 0
            });
            _beneficiaryRepository.Setup(a => a.GetByUserTransactions(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<Guid>())).Returns(new List<Transaction>());
            _beneficiaryRepository.Setup(a => a.GetByUserTransactions(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<Guid?>())).Returns(new List<Transaction>());
            _beneficiaryRepository.Setup(a => a.GetUserById(It.IsAny<int>())).Returns(new TopUpUser { Balance = 1000, Id = 1, Name = "TestUser" });

            //Act
            var service = new BeneficiaryService(_logger.Object, _beneficiaryRepository.Object);
            var response = service.TopUpBeneficiary(request);

            //Assert
            _beneficiaryRepository.Verify(a => a.TopUpBeneficiary(It.Is<TopUpRequestModel>(a => a.TopUpValue == request.TopUpValue)), Times.Once);
            _beneficiaryRepository.Verify(a => a.WithdrawUserBalance(request.UserId, request.TopUpValue, 1), Times.Once);
            _beneficiaryRepository.Verify(a => a.GetUserById(1), Times.Once);
            Assert.True(response.IsSuccess);
            Assert.Equal(response.Message, result.Message);
        }

        [Fact]
        public void TopUpBeneficiary_should_return_false_Top_up_value_bigger_than_balance()
        {
            //Arrange
            var result = new GenericResponseModel(false, "Top Up Value Should Be Less Than Or Equal Balance.");

            var request = new TopUpRequestModel
            {
                BeneficiaryId = Guid.Empty,
                IsVerified = true,
                TopUpValue = 150,
                UserId = 1
            };

            _beneficiaryRepository.Setup(a => a.TopUpBeneficiary(It.IsAny<TopUpRequestModel>())).Returns(result);
            _beneficiaryRepository.Setup(a => a.WithdrawUserBalance(It.IsAny<int>(), It.IsAny<decimal>(), It.IsAny<int>())).Returns(result);
            _beneficiaryRepository.Setup(a => a.GetBeneficiaryById(It.IsAny<Guid>())).Returns(new Beneficiary
            {
                Id = Guid.Empty,
                UserId = 1,
                Name = "Test",
                Balance = 100
            });
            _beneficiaryRepository.Setup(a => a.GetByUserTransactions(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<Guid>())).Returns(new List<Transaction>());
            _beneficiaryRepository.Setup(a => a.GetByUserTransactions(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<Guid?>())).Returns(new List<Transaction>());
            _beneficiaryRepository.Setup(a => a.GetUserById(It.IsAny<int>())).Returns(new TopUpUser { Balance = 1000, Id = 1, Name = "TestUser" });

            //Act
            var service = new BeneficiaryService(_logger.Object, _beneficiaryRepository.Object);
            var response = service.TopUpBeneficiary(request);

            //Assert
            _beneficiaryRepository.Verify(a => a.TopUpBeneficiary(It.Is<TopUpRequestModel>(a => a.TopUpValue == request.TopUpValue)), Times.Never);
            Assert.False(response.IsSuccess);
            Assert.Equal(response.Message, result.Message);
        }

        [Fact]
        public void TopUpBeneficiary_should_return_false_VeryfiedUserMax500()
        {
            //Arrange
            var result = new GenericResponseModel(false, "User Is Verified, Exceed The Max Top Up Value.");

            var request = new TopUpRequestModel
            {
                BeneficiaryId = Guid.Empty,
                IsVerified = true,
                TopUpValue = 600,
                UserId = 1
            };

            _beneficiaryRepository.Setup(a => a.TopUpBeneficiary(It.IsAny<TopUpRequestModel>())).Returns(result);
            _beneficiaryRepository.Setup(a => a.GetBeneficiaryById(It.IsAny<Guid>())).Returns(new Beneficiary
            {
                Id = Guid.Empty,
                UserId = 1,
                Name = "Test",
                Balance = 0
            });
            _beneficiaryRepository.Setup(a => a.GetByUserTransactions(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<Guid>())).Returns(new List<Transaction>());
            _beneficiaryRepository.Setup(a => a.GetByUserTransactions(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<Guid?>())).Returns(new List<Transaction>());
            _beneficiaryRepository.Setup(a => a.GetUserById(It.IsAny<int>())).Returns(new TopUpUser { Balance = 1000, Id = 1, Name = "TestUser" });

            //Act
            var service = new BeneficiaryService(_logger.Object, _beneficiaryRepository.Object);
            var response = service.TopUpBeneficiary(request);

            //Assert
            _beneficiaryRepository.Verify(a => a.TopUpBeneficiary(It.Is<TopUpRequestModel>(a => a.TopUpValue == request.TopUpValue)), Times.Never);
            Assert.False(response.IsSuccess);
            Assert.Equal(response.Message, result.Message);
        }

        [Fact]
        public void TopUpBeneficiary_should_return_false_NotVeryfiedUserMax1000()
        {
            //Arrange
            var result = new GenericResponseModel(false, "User Is Not Verified, Exceed The Max Top Up Value.");

            var request = new TopUpRequestModel
            {
                BeneficiaryId = Guid.Empty,
                IsVerified = false,
                TopUpValue = 1500,
                UserId = 1
            };

            _beneficiaryRepository.Setup(a => a.TopUpBeneficiary(It.IsAny<TopUpRequestModel>())).Returns(result);
            _beneficiaryRepository.Setup(a => a.GetBeneficiaryById(It.IsAny<Guid>())).Returns(new Beneficiary
            {
                Id = Guid.Empty,
                UserId = 1,
                Name = "Test",
                Balance = 0
            });
            _beneficiaryRepository.Setup(a => a.GetByUserTransactions(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<Guid>())).Returns(new List<Transaction>());
            _beneficiaryRepository.Setup(a => a.GetByUserTransactions(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<Guid?>())).Returns(new List<Transaction>());
            _beneficiaryRepository.Setup(a => a.GetUserById(It.IsAny<int>())).Returns(new TopUpUser { Balance = 5000, Id = 1, Name = "TestUser" });

            //Act
            var service = new BeneficiaryService(_logger.Object, _beneficiaryRepository.Object);
            var response = service.TopUpBeneficiary(request);

            //Assert
            _beneficiaryRepository.Verify(a => a.TopUpBeneficiary(It.Is<TopUpRequestModel>(a => a.TopUpValue == request.TopUpValue)), Times.Never);
            Assert.False(response.IsSuccess);
            Assert.Equal(response.Message, result.Message);
        }

        [Fact]
        public void TopUpBeneficiary_should_return_false_UserMax3000()
        {
            //Arrange
            var result = new GenericResponseModel(false, "User Exceed The Max Top Up Limit Per All Beneficiaries.");

            var request = new TopUpRequestModel
            {
                BeneficiaryId = Guid.Empty,
                IsVerified = false,
                TopUpValue = 100,
                UserId = 1
            };

            _beneficiaryRepository.Setup(a => a.TopUpBeneficiary(It.IsAny<TopUpRequestModel>())).Returns(result);
            _beneficiaryRepository.Setup(a => a.GetBeneficiaryById(It.IsAny<Guid>())).Returns(new Beneficiary
            {
                Id = Guid.Empty,
                UserId = 1,
                Name = "Test",
                Balance = 0
            });
            _beneficiaryRepository.Setup(a => a.GetByUserTransactions(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<Guid>())).Returns(new List<Transaction>());
            _beneficiaryRepository.Setup(a => a.GetByUserTransactions(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), null)).Returns(new List<Transaction> { new Transaction { Amount = 3000, UserId = 1, BeneficiaryId = Guid.Empty, FeeAmount = 1 } });
            _beneficiaryRepository.Setup(a => a.GetUserById(It.IsAny<int>())).Returns(new TopUpUser { Balance = 1000, Id = 1, Name = "TestUser" });

            //Act
            var service = new BeneficiaryService(_logger.Object, _beneficiaryRepository.Object);
            var response = service.TopUpBeneficiary(request);

            //Assert
            _beneficiaryRepository.Verify(a => a.TopUpBeneficiary(It.Is<TopUpRequestModel>(a => a.TopUpValue == request.TopUpValue)), Times.Never);
            Assert.False(response.IsSuccess);
            Assert.Equal(response.Message, result.Message);
        }

        [Fact]
        public void GetAllTopUpOptions_should_return_all_top_up_options()
        {
            //Arrange
            //Act
            var service = new BeneficiaryService(_logger.Object, _beneficiaryRepository.Object);
            var response = service.GetAllTopUpOptions();
            //Assert
            Assert.NotEmpty(response);
        }

        [Fact]
        public void GetTopUpUser_should_return_user()
        {
            //Arrange
            var user = new TopUpUser
            {
                Balance = 500,
                Id = 1,
                Name = "Test"
            };
            _beneficiaryRepository.Setup(a => a.GetUserById(It.IsAny<int>())).Returns(user);

            //Act
            var service = new BeneficiaryService(_logger.Object, _beneficiaryRepository.Object);
            var response = service.GetTopUpUser(1);

            //Assert
            Assert.Equal(user.Balance, response.Balance);
            Assert.Equal(user.Name, response.Name);
            Assert.Equal(user.Id, response.Id);
        }
    }
}