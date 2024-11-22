using Application;
using Application.Dtos;
using Application.Specialty;
using Application.Specialty.Responses;
using Domain.Entities;
using Domain.Ports;
using Domain.Specialty.Exceptions;
using Moq;
using NUnit.Framework;

namespace ApplicationTests
{
    [TestFixture]
    public class SpecialtyManagerTests
    {
        private Mock<ISpecialtyRepository> _specialtyRepositoryMock;
        private SpecialtyManager _specialtyManager;

        [SetUp]
        public void SetUp()
        {
            _specialtyRepositoryMock = new Mock<ISpecialtyRepository>();
            _specialtyManager = new SpecialtyManager(_specialtyRepositoryMock.Object);
        }

        [Test]
        public async Task GetAllSpecialties_ShouldReturnSpecialties_WhenSpecialtiesExist()
        {
            var specialties = new List<Specialty>
            {
                new Specialty { Id = 1, Name = "Ortodontia" },
                new Specialty { Id = 2, Name = "Periodontia" }
            };

            _specialtyRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(specialties);

            var response = await _specialtyManager.GetAllSpecialties();

            Assert.IsTrue(response.Success, "The response should indicate success.");
            Assert.AreEqual(2, response.Data.Count, "The response should contain all specialties.");
            Assert.AreEqual("Specialties retrieved successfully.", response.Message);
        }

        [Test]
        public async Task GetAllSpecialties_SpecialtyNotFoundException_WhenNoSpecialtiesExist()
        {
            _specialtyRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Specialty>());

            var response = await _specialtyManager.GetAllSpecialties();

            Assert.IsFalse(response.Success, "The response should indicate failure.");
            Assert.AreEqual(ErrorCode.SPECIALTY_NOT_FOUND, response.ErrorCode, "The error code should indicate that no specialties were found.");
            Assert.AreEqual("No specialties found.", response.Message);
        }
    }
}
