using Application.Consultation;
using Application.Dtos;
using Domain.Entities;
using Domain.Ports;
using Moq;
using NUnit.Framework;
using Application;
using ConsultationEntity = Domain.Entities.Consultation;

namespace ApplicationTests.Consultation
{
    [TestFixture]
    public class ConsultationManagerTests
    {
        private Mock<IConsultationRepository> _consultationRepositoryMock;
        private Mock<IPatientRepository> _patientRepositoryMock;
        private Mock<IScheduleRepository> _scheduleRepositoryMock;
        private ConsultationManager _consultationManager;

        [SetUp]
        public void Setup()
        {
            _consultationRepositoryMock = new Mock<IConsultationRepository>();
            _patientRepositoryMock = new Mock<IPatientRepository>();
            _scheduleRepositoryMock = new Mock<IScheduleRepository>();

            _consultationManager = new ConsultationManager(
                _consultationRepositoryMock.Object,
                _patientRepositoryMock.Object,
                _scheduleRepositoryMock.Object);
        }

        #region CreateConsultation Tests

        [Test]
        public async Task CreateConsultation_ShouldReturnSuccess_WhenValidConsultationIsCreated()
        {
            var consultationDto = new ConsultationRequestDto { PatientId = 1, ScheduleId = 10, Procedure = "Consulta" };
            var patient = new Patient { Id = 1, Name = "Test Patient" };
            var schedule = new Schedule { Id = 10, Data = DateTime.UtcNow.AddDays(1), IsAvailable = true };

            _patientRepositoryMock.Setup(repo => repo.Get(consultationDto.PatientId)).ReturnsAsync(patient);
            _scheduleRepositoryMock.Setup(repo => repo.Get(consultationDto.ScheduleId)).ReturnsAsync(schedule);
            _consultationRepositoryMock.Setup(repo => repo.Create(It.IsAny<ConsultationEntity>())).ReturnsAsync(1);

            var response = await _consultationManager.CreateConsultation(consultationDto);

            Assert.IsTrue(response.Success);
            Assert.AreEqual("Consultation created successfully.", response.Message);
        }

        // Other CreateConsultation tests...

        #endregion

        #region GetConsultationsByDate Tests

        [Test]
        public async Task GetConsultationsByDate_ShouldReturnSuccess_WhenConsultationsExist()
        {
            var date = DateTime.UtcNow.Date;
            var consultations = new List<ConsultationEntity>
    {
        new ConsultationEntity
        {
            Id = 1,
            ScheduleId = 10,
            PatientId = 1,
            Procedure = "Consulta",
            CreatedAt = DateTime.UtcNow
        },
        new ConsultationEntity
        {
            Id = 2,
            ScheduleId = 11,
            PatientId = 2,
            Procedure = "Tratamento",
            CreatedAt = DateTime.UtcNow
        }
    };

            _consultationRepositoryMock.Setup(repo => repo.GetByDate(date)).ReturnsAsync(consultations);

            var response = await _consultationManager.GetConsultationsByDate(date);

            Assert.IsTrue(response.Success);
            Assert.AreEqual(2, response.Data.Count);
            Assert.AreEqual("Consultations retrieved successfully.", response.Message);

            Assert.AreEqual(1, response.Data[0].Id);
            Assert.AreEqual("Consulta", response.Data[0].Procedure);
            Assert.AreEqual(10, response.Data[0].ScheduleId);

            Assert.AreEqual(2, response.Data[1].Id);
            Assert.AreEqual("Tratamento", response.Data[1].Procedure);
            Assert.AreEqual(11, response.Data[1].ScheduleId);
        }


        [Test]
        public async Task GetConsultationsByDate_ShouldReturnError_WhenNoConsultationsFound()
        {
            var date = DateTime.UtcNow.Date;

            _consultationRepositoryMock.Setup(repo => repo.GetByDate(date)).ReturnsAsync(new List<ConsultationEntity>());

            var response = await _consultationManager.GetConsultationsByDate(date);

            Assert.IsFalse(response.Success);
            Assert.AreEqual(ErrorCode.CONSULTATION_NOT_FOUND, response.ErrorCode);
            Assert.AreEqual("No consultations found for the given date.", response.Message);
        }

        #endregion

        #region GetConsultationsByPatient Tests

        [Test]
        public async Task GetConsultationsByPatient_ShouldReturnSuccess_WhenConsultationsExist()
        {
            var patientId = 1;
            var consultations = new List<ConsultationEntity>
            {
                new ConsultationEntity { Id = 1, ScheduleId = 10, PatientId = 1, Procedure = "Consulta", CreatedAt = DateTime.UtcNow }
            };

            _consultationRepositoryMock.Setup(repo => repo.GetByPatient(patientId)).ReturnsAsync(consultations);

            var response = await _consultationManager.GetConsultationsByPatient(patientId);

            Assert.IsTrue(response.Success);
            Assert.AreEqual(1, response.Data.Count);
            Assert.AreEqual("Consultations retrieved successfully.", response.Message);
        }

        [Test]
        public async Task GetConsultationsByPatient_ShouldReturnError_WhenNoConsultationsFound()
        {
            var patientId = 1;

            _consultationRepositoryMock.Setup(repo => repo.GetByPatient(patientId)).ReturnsAsync(new List<ConsultationEntity>());

            var response = await _consultationManager.GetConsultationsByPatient(patientId);

            Assert.IsFalse(response.Success);
            Assert.AreEqual(ErrorCode.CONSULTATION_NOT_FOUND, response.ErrorCode);
            Assert.AreEqual("No consultations found for the given patient.", response.Message);
        }

        #endregion

        #region UpdateConsultation Tests

        [Test]
        public async Task UpdateConsultation_ShouldReturnSuccess_WhenConsultationIsUpdated()
        {
            var consultationDto = new ConsultationRequestDto { Id = 1, ScheduleId = 10, Procedure = "Nova Consulta" };
            var existingConsultation = new ConsultationEntity { Id = 1, ScheduleId = 9, PatientId = 1, Procedure = "Consulta Antiga" };

            _consultationRepositoryMock.Setup(repo => repo.Get(consultationDto.Id)).ReturnsAsync(existingConsultation);
            _consultationRepositoryMock.Setup(repo => repo.Update(It.IsAny<ConsultationEntity>()));

            var response = await _consultationManager.UpdateConsultation(consultationDto);

            Assert.IsTrue(response.Success);
            Assert.AreEqual("Consultation updated successfully.", response.Message);
            Assert.AreEqual("Nova Consulta", response.ConsultationData.Procedure);
        }

        [Test]
        public async Task UpdateConsultation_ShouldReturnError_WhenConsultationNotFound()
        {
            var consultationDto = new ConsultationRequestDto { Id = 1, ScheduleId = 10, Procedure = "Nova Consulta" };

            _consultationRepositoryMock.Setup(repo => repo.Get(consultationDto.Id)).ReturnsAsync((ConsultationEntity)null);

            var response = await _consultationManager.UpdateConsultation(consultationDto);

            Assert.IsFalse(response.Success);
            Assert.AreEqual(ErrorCode.CONSULTATION_NOT_FOUND, response.ErrorCode);
            Assert.AreEqual("Consultation not found.", response.Message);
        }

        #endregion
    }
}
