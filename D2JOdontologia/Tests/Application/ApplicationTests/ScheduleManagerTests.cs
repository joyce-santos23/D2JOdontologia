using Application.Dtos;
using Application.Schedule;
using Application.Schedule.Requests;
using Domain.Ports;
using Moq;
using NUnit.Framework;

namespace Application.Tests
{
    [TestFixture]
    public class ScheduleManagerTests
    {
        private Mock<IScheduleRepository> _scheduleRepositoryMock;
        private Mock<ISpecialistRepository> _specialistRepositoryMock;
        private ScheduleManager _scheduleManager;

        [SetUp]
        public void SetUp()
        {
            _scheduleRepositoryMock = new Mock<IScheduleRepository>();
            _specialistRepositoryMock = new Mock<ISpecialistRepository>();
            _scheduleManager = new ScheduleManager(_scheduleRepositoryMock.Object, _specialistRepositoryMock.Object);
        }

        [Test]
        public async Task CreateSchedules_ShouldCreateSchedules_WhenDataIsValid()
        {
            var request = new CreateScheduleRequest
            {
                ScheduleData = new ScheduleRequestDto
                {
                    SpecialistId = 1,
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today.AddDays(1),
                    StartTime = new TimeSpan(9, 0, 0),
                    EndTime = new TimeSpan(17, 0, 0),
                    IntervalMinutes = 60
                }
            };

            var specialist = new Domain.Entities.Specialist { Id = 1, Name = "Test Specialist" };

            _specialistRepositoryMock
                .Setup(repo => repo.Get(It.IsAny<int>()))
                .ReturnsAsync(specialist);

            _scheduleRepositoryMock
                .Setup(repo => repo.AddSchedulesAsync(It.IsAny<IEnumerable<Domain.Entities.Schedule>>()))
                .Returns(Task.CompletedTask);

            var response = await _scheduleManager.CreateSchedules(request);

            Assert.IsTrue(response.Success);
            Assert.IsNotEmpty(response.Data);
            Assert.AreEqual("Schedules created successfully.", response.Message);
        }

        [Test]
        public async Task CreateSchedules_ShouldReturnError_WhenSpecialistNotFound()
        {
            var request = new CreateScheduleRequest
            {
                ScheduleData = new ScheduleRequestDto
                {
                    SpecialistId = 1,
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today.AddDays(1),
                    StartTime = new TimeSpan(9, 0, 0),
                    EndTime = new TimeSpan(17, 0, 0),
                    IntervalMinutes = 60
                }
            };

            _specialistRepositoryMock
                .Setup(repo => repo.Get(It.IsAny<int>()))
                .ReturnsAsync((Domain.Entities.Specialist)null);

            var response = await _scheduleManager.CreateSchedules(request);

            Assert.IsFalse(response.Success);
            Assert.AreEqual("The specialist ID provided was not found.", response.Message);
        }

        [Test]
        public async Task GetSchedule_ShouldReturnSchedule_WhenScheduleExists()
        {
            var schedule = new Domain.Entities.Schedule { Id = 1, SpecialistId = 1, Data = DateTime.Now, IsAvailable = true };

            _scheduleRepositoryMock
                .Setup(repo => repo.Get(It.IsAny<int>()))
                .ReturnsAsync(schedule);

            var response = await _scheduleManager.GetSchedule(1);

            Assert.IsTrue(response.Success);
            Assert.IsNotNull(response.ScheduleData);
            Assert.AreEqual(schedule.Id, response.ScheduleData.Id);
        }

        [Test]
        public async Task GetSchedule_ShouldReturnError_WhenScheduleDoesNotExist()
        {
            _scheduleRepositoryMock
                .Setup(repo => repo.Get(It.IsAny<int>()))
                .ReturnsAsync((Domain.Entities.Schedule)null);

            var response = await _scheduleManager.GetSchedule(1);

            Assert.IsFalse(response.Success);
            Assert.AreEqual("No schedule found with the provided ID.", response.Message);
        }

        [Test]
        public async Task UpdateScheduleAvailability_ShouldUpdate_WhenScheduleExists()
        {
            var schedule = new Domain.Entities.Schedule { Id = 1, SpecialistId = 1, Data = DateTime.Now, IsAvailable = true };

            _scheduleRepositoryMock
                .Setup(repo => repo.Get(It.IsAny<int>()))
                .ReturnsAsync(schedule);

            _scheduleRepositoryMock
                .Setup(repo => repo.UpdateAsync(It.IsAny<Domain.Entities.Schedule>()))
                .Returns(Task.CompletedTask);

            var response = await _scheduleManager.UpdateScheduleAvailability(1, false);

            Assert.IsTrue(response.Success);
            Assert.AreEqual("Schedule marked as unavailable.", response.Message);
        }

        [Test]
        public async Task UpdateScheduleAvailability_ShouldReturnError_WhenScheduleDoesNotExist()
        {
            _scheduleRepositoryMock
                .Setup(repo => repo.Get(It.IsAny<int>()))
                .ReturnsAsync((Domain.Entities.Schedule)null);

            var response = await _scheduleManager.UpdateScheduleAvailability(1, false);

            Assert.IsFalse(response.Success);
            Assert.AreEqual("No schedule found with the provided ID.", response.Message);
        }

        [Test]
        public async Task GetAvailableSchedules_ShouldReturnAvailableSchedules()
        {
            var schedules = new List<Domain.Entities.Schedule>
            {
                new Domain.Entities.Schedule { Id = 1, SpecialistId = 1, Data = DateTime.Now, IsAvailable = true },
                new Domain.Entities.Schedule { Id = 2, SpecialistId = 1, Data = DateTime.Now.AddHours(1), IsAvailable = true }
            };

            _scheduleRepositoryMock
                .Setup(repo => repo.GetAvailableSchedulesBySpecialist(It.IsAny<int>()))
                .ReturnsAsync(schedules);

            var response = await _scheduleManager.GetAvailableSchedules(1);

            Assert.IsTrue(response.Success);
            Assert.AreEqual(2, response.Data.Count);
        }

        [Test]
        public async Task GetAvailableSchedules_ShouldReturnError_WhenNoSchedulesAreAvailable()
        {
            _scheduleRepositoryMock
                .Setup(repo => repo.GetAvailableSchedulesBySpecialist(It.IsAny<int>()))
                .ReturnsAsync(new List<Domain.Entities.Schedule>());

            var response = await _scheduleManager.GetAvailableSchedules(1);

            Assert.IsFalse(response.Success);
            Assert.AreEqual("No available schedules found for the given specialist.", response.Message);
        }
    }
}
