using Application.Dtos;
using Application.Responses;
using Application.Specialist;
using Application.Specialist.Requests;
using Domain.Ports;
using Domain.Specialist.Exceptions;
using Moq;
using NUnit.Framework;
using SpecialistEntity = Domain.Entities.Specialist;

namespace Application.Tests
{
    [TestFixture]
    public class SpecialistManagerTests
    {
        private Mock<ISpecialistRepository> _specialistRepositoryMock;
        private Mock<ISpecialtyRepository> _specialtyRepositoryMock;
        private SpecialistManager _specialistManager;

        [SetUp]
        public void SetUp()
        {
            _specialistRepositoryMock = new Mock<ISpecialistRepository>();
            _specialtyRepositoryMock = new Mock<ISpecialtyRepository>();
            _specialistManager = new SpecialistManager(_specialistRepositoryMock.Object, _specialtyRepositoryMock.Object);
        }

        [Test]
        public async Task CreateSpecialist_ShouldReturnSuccess_WhenSpecialistIsCreated()
        {
            var specialistDto = new SpecialistDto
            {
                Id = 0,
                Name = "Valid Name",
                Fone = "123456789",
                Address = "Valid Address",
                Email = "validemail@example.com",
                CroNumber = "12345",
                CroState = "SP",
                SpecialtyIds = new List<int> { -1, -2 }
            };

            var request = new CreateSpecialistRequest { SpecialistData = specialistDto };

            _specialtyRepositoryMock
                .Setup(repo => repo.Get(It.IsAny<int>()))
                .ReturnsAsync((int id) => new Domain.Entities.Specialty { Id = id, Name = $"Specialty {id}" });

            _specialistRepositoryMock
                .Setup(repo => repo.Create(It.IsAny<SpecialistEntity>()))
                .ReturnsAsync(1);

            var response = await _specialistManager.CreateSpecialist(request);

            Assert.IsTrue(response.Success);
            Assert.AreEqual("Specialist created successfully!", response.Message);
            Assert.AreEqual(1, request.SpecialistData.Id);
        }

        [Test]
        public async Task CreateSpecialist_ShouldReturnError_WhenSpecialtyNotFound()
        {
            var specialistDto = new SpecialistDto
            {
                Id = 0,
                Name = "Valid Name",
                Fone = "123456789",
                Address = "Valid Address",
                Email = "validemail@example.com",
                CroNumber = "12345",
                CroState = "SP",
                SpecialtyIds = new List<int> { -99 } 
            };

            var request = new CreateSpecialistRequest { SpecialistData = specialistDto };

            _specialtyRepositoryMock
                .Setup(repo => repo.Get(It.IsAny<int>()))
                .ReturnsAsync((Domain.Entities.Specialty)null);

            var response = await _specialistManager.CreateSpecialist(request);

            Assert.IsFalse(response.Success);
            Assert.AreEqual(ErrorCode.SPECIALTY_NOT_FOUND, response.ErrorCode);
            Assert.AreEqual("Specialty with ID -99 not found.", response.Message);
        }

        [Test]
        public async Task GetAllSpecialists_ShouldReturnSpecialists()
        {
            var specialists = new List<SpecialistEntity>
            {
                new SpecialistEntity { Id = -1, Name = "Specialist 1", CroNumber = "123", CroState = "SP" },
                new SpecialistEntity { Id = -2, Name = "Specialist 2", CroNumber = "456", CroState = "RJ" }
            };

            _specialistRepositoryMock
                .Setup(repo => repo.GetAll())
                .ReturnsAsync(specialists);

            var response = await _specialistManager.GetAllSpecialists();

            Assert.IsNotEmpty(response);
            Assert.AreEqual(2, response.Count());
            Assert.AreEqual("Specialist 1", response.First().Name);
        }

        [Test]
        public async Task GetSpecialist_ShouldReturnSpecialist_WhenFound()
        {
            var specialist = new SpecialistEntity { Id = -1, Name = "Test Specialist", CroNumber = "12345", CroState = "SP" };

            _specialistRepositoryMock
                .Setup(repo => repo.Get(It.IsAny<int>()))
                .ReturnsAsync(specialist);

            var response = await _specialistManager.GetSpecialist(1);

            Assert.IsTrue(response.Success);
            Assert.AreEqual("Test Specialist", response.SpecialistData.Name);
        }

        [Test]
        public async Task GetSpecialist_ShouldReturnError_WhenSpecialistNotFound()
        {
            _specialistRepositoryMock
                .Setup(repo => repo.Get(It.IsAny<int>()))
                .ReturnsAsync((SpecialistEntity)null);

            var response = await _specialistManager.GetSpecialist(1);

            Assert.IsFalse(response.Success);
        }
    }
}
