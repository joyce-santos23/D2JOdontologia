using Application.Consultation;
using Application.Dtos;
using Domain.Entities;
using Domain.Ports;
using Moq;
using NUnit.Framework;
using Application;
using ConsultationEntity = Domain.Entities.Consultation;
using Application.Consultation.Dtos;

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
        public async Task MapToResponseDto_ShouldMapCorrectly()
        {
            var consultation = new ConsultationEntity
            {
                Id = 1,
                Patient = new Patient { Id = 1, Name = "John Doe" }, 
                Schedule = new Schedule
                {
                    Id = 10,
                    Data = DateTime.UtcNow.AddDays(1),
                    Specialist = new Specialist { Id = 2, Name = "Dr. Smith" } 
                },
                Procedure = "Consulta Odontológica",
                CreatedAt = DateTime.UtcNow
            };

            var result = ConsultationResponseDto.MapToResponseDto(consultation);

            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("John Doe", result.PatientName);
            Assert.AreEqual("Dr. Smith", result.SpecialistName);
            Assert.AreEqual(consultation.Schedule.Data, result.ScheduleDate);
            Assert.AreEqual("Consulta Odontológica", result.Procedure);
            Assert.AreEqual(consultation.CreatedAt, result.CreatedAt);
        }


        [Test]
        public async Task UpdateConsultation_ShouldFail_WhenNewScheduleIsUnavailable()
        {
            var consultationDto = new ConsultationUpdateRequestDto { ScheduleId = 10, Procedure = "Nova Consulta" };
            var existingConsultation = new ConsultationEntity { Id = 1, ScheduleId = 9, PatientId = 1, Procedure = "Consulta Antiga" };
            var unavailableSchedule = new Schedule { Id = 10, IsAvailable = false };

            _consultationRepositoryMock.Setup(repo => repo.Get(1)).ReturnsAsync(existingConsultation);
            _scheduleRepositoryMock.Setup(repo => repo.Get(10)).ReturnsAsync(unavailableSchedule);

            var response = await _consultationManager.UpdateConsultation(1, consultationDto);

            Assert.IsFalse(response.Success);
            Assert.AreEqual(ErrorCode.INVALID_DATE, response.ErrorCode);
            Assert.AreEqual("The selected schedule is not available.", response.Message);
        }

        [Test]
        public async Task UpdateConsultation_ShouldUpdateOldAndNewScheduleAvailability()
        {
            var consultationDto = new ConsultationUpdateRequestDto { ScheduleId = 10, Procedure = "Nova Consulta" };
            var existingConsultation = new ConsultationEntity { Id = 1, ScheduleId = 9, PatientId = 1, Procedure = "Consulta Antiga" };
            var oldSchedule = new Schedule { Id = 9, IsAvailable = false };
            var newSchedule = new Schedule { Id = 10, IsAvailable = true };

            _consultationRepositoryMock.Setup(repo => repo.Get(1)).ReturnsAsync(existingConsultation);
            _scheduleRepositoryMock.Setup(repo => repo.Get(9)).ReturnsAsync(oldSchedule);
            _scheduleRepositoryMock.Setup(repo => repo.Get(10)).ReturnsAsync(newSchedule);

            var response = await _consultationManager.UpdateConsultation(1, consultationDto);

            Assert.IsTrue(response.Success);
            Assert.AreEqual("Consultation updated successfully.", response.Message);
            Assert.IsTrue(oldSchedule.IsAvailable);
            Assert.IsFalse(newSchedule.IsAvailable);
        }


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

            var firstConsultation = response.Data.FirstOrDefault();
            Assert.AreEqual(1, firstConsultation.Id);
            Assert.AreEqual("Consulta", firstConsultation.Procedure);

            var secondConsultation = response.Data.LastOrDefault();
            Assert.AreEqual(2, secondConsultation.Id);
            Assert.AreEqual("Tratamento", secondConsultation.Procedure);
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
            var consultationId = 1;
            var consultationDto = new ConsultationUpdateRequestDto { ScheduleId = 10, Procedure = "Nova Consulta" };
            var existingConsultation = new ConsultationEntity { Id = consultationId, ScheduleId = 9, PatientId = 1, Procedure = "Consulta Antiga" };

            _consultationRepositoryMock.Setup(repo => repo.Get(consultationId)).ReturnsAsync(existingConsultation);
            _consultationRepositoryMock.Setup(repo => repo.Update(It.IsAny<ConsultationEntity>()));

            var response = await _consultationManager.UpdateConsultation(consultationId, consultationDto);

            Assert.IsTrue(response.Success);
            Assert.AreEqual("Consultation updated successfully.", response.Message);
            Assert.AreEqual("Nova Consulta", response.ConsultationData.Procedure);
        }


        [Test]
        public async Task UpdateConsultation_ShouldReturnError_WhenConsultationNotFound()
        {
            var consultationId = 1;
            var consultationDto = new ConsultationUpdateRequestDto { ScheduleId = 10, Procedure = "Nova Consulta" };

            _consultationRepositoryMock.Setup(repo => repo.Get(consultationId)).ReturnsAsync((ConsultationEntity)null);

            var response = await _consultationManager.UpdateConsultation(consultationId, consultationDto);

            Assert.IsFalse(response.Success);
            Assert.AreEqual(ErrorCode.CONSULTATION_NOT_FOUND, response.ErrorCode);
            Assert.AreEqual("Consultation not found.", response.Message);
        }


        #endregion
    }
}
