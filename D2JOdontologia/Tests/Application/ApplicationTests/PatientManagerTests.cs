using Application;
using Application.Dtos;
using Application.Patient;
using Application.Patient.Requests;
using Domain.Entities;
using Domain.Patient.Exceptions;
using Domain.Ports;
using Moq;
using NUnit.Framework;

namespace ApplicationTests
{
   
    public class PatientManagerTests
    {
        private Mock<IPatientRepository> _mockPatientRepository;
        private PatientManager _patientManager;

        [SetUp]
        public void SetUp()
        {
            _mockPatientRepository = new Mock<IPatientRepository>();
            _patientManager = new PatientManager(_mockPatientRepository.Object);
        }

        [Test]
        public async Task CreatePatient_ShouldReturnSuccess_WhenPatientIsCreatedSuccessfully()
        {
            var request = new CreatePatientRequest
            {
                PatientData = new PatientDto
                {
                    Name = "John Doe",
                    Fone = "123456789",
                    Address = "123 Main St",
                    Email = "john.doe@example.com",
                    Birth = new DateOnly(1990, 1, 1),
                    Cpf = "12345678901",
                    CreatedAt = DateTime.UtcNow
                }
            };

            _mockPatientRepository
                .Setup(repo => repo.Create(It.IsAny<Patient>()))
                .ReturnsAsync(1); 

            var response = await _patientManager.CreatePatient(request);

            Assert.IsTrue(response.Success);
            Assert.AreEqual(1, response.PatientData.Id);
            Assert.AreEqual("John Doe", response.PatientData.Name);
        }

        [Test]
        public async Task CreatePatient_ShouldReturnError_WhenRequiredFieldsAreMissing()
        {
            // Definindo os casos de teste
            var testCases = new[]
            {
                new { Field = "Name", PatientData = new PatientDto { Name = "", Fone = "123456789", Address = "123 Main St", Email = "john.doe@example.com", Birth = new DateOnly(1990, 1, 1), Cpf = "12345678901" }, ExpectedMessage = "Name is required." },
                new { Field = "Fone", PatientData = new PatientDto { Name = "John Doe", Fone = "", Address = "123 Main St", Email = "john.doe@example.com", Birth = new DateOnly(1990, 1, 1), Cpf = "12345678901" }, ExpectedMessage = "Phone (Fone) is required." },
                new { Field = "Address", PatientData = new PatientDto { Name = "John Doe", Fone = "123456789", Address = "", Email = "john.doe@example.com", Birth = new DateOnly(1990, 1, 1), Cpf = "12345678901" }, ExpectedMessage = "Address is required." },
                
            };

            foreach (var testCase in testCases)
            {
                // Arrange
                var request = new CreatePatientRequest
                {
                    PatientData = testCase.PatientData
                };

                // Act
                var response = await _patientManager.CreatePatient(request);

                // Assert
                Assert.IsFalse(response.Success, $"Failed for field: {testCase.Field}");
                Assert.AreEqual(ErrorCode.MISSING_REQUIRED_INFORMATION, response.ErrorCode, $"Error code mismatch for field: {testCase.Field}");
                Assert.AreEqual(testCase.ExpectedMessage, response.Message, $"Message mismatch for field: {testCase.Field}");
            }
        }


        [Test]
        public async Task CreatePatient_InvalidEmailException_WhenEmailIsInvalid()
        {
            var request = new CreatePatientRequest
            {
                PatientData = new PatientDto
                {
                    Name = "John Doe",
                    Fone = "123456789",
                    Address = "123 Main St",
                    Email = "invalid-email",
                    Birth = new DateOnly(1990, 1, 1),
                    Cpf = "12345678901",
                    CreatedAt = DateTime.UtcNow
                }
            };

            var response = await _patientManager.CreatePatient(request);

            Assert.IsFalse(response.Success);
            Assert.AreEqual(ErrorCode.INVALID_EMAIL, response.ErrorCode);
            Assert.AreEqual(response.Message, "The given email is invalid");
        }



        [Test]
        public async Task GetAll_ShouldReturnListOfPatients()
        {
            var patients = new List<Patient>
    {
        new Patient { Id = 1, Name = "John Doe", Cpf = "12345678901" },
        new Patient { Id = 2, Name = "Jane Doe", Cpf = "98765432100" }
    };

            _mockPatientRepository
                .Setup(repo => repo.GetAll())
                .ReturnsAsync(patients);

            var response = await _patientManager.GetAllPatient();

            Assert.AreEqual(2, response.Count());
            Assert.IsTrue(response.All(r => r.Success));
            Assert.AreEqual("John Doe", response.First().PatientData.Name);
        }

        [Test]
        public async Task GetPatient_ShouldReturnError_WhenPatientDoesNotExist()
        {
            _mockPatientRepository
                .Setup(repo => repo.Get(It.IsAny<int>()))
                .ReturnsAsync((Patient)null);

            var response = await _patientManager.GetPatient(1);

            Assert.IsFalse(response.Success);
            Assert.AreEqual(ErrorCode.PATIENT_NOT_FOUND, response.ErrorCode);
            Assert.AreEqual("No patient record was found with the given id", response.Message);
        }




    }

}
