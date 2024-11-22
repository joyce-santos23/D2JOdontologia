namespace Application
{
    public enum ErrorCode
    {
        //Patient
        PATIENT_NOT_FOUND = 1,
        COULD_NOT_STORE_DATA = 2,
        INVALID_PERSON_ID = 3,
        MISSING_REQUIRED_INFORMATION = 4,
        INVALID_EMAIL = 5,

        //Specialty
        SPECIALTY_NOT_FOUND = 6,

        //Specialist
        SPECIALIST_NOT_FOUND = 7,
        INVALID_CRO = 8,

        //Schedule
        INVALID_SCHEDULE_DATES = 9,
        SCHEDULE_NOT_FOUND = 10,

        //Consultation
        CONSULTATION_NOT_FOUND = 11,
        INVALID_DATE = 12,

    }

    public abstract class Response
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public ErrorCode ErrorCode { get; set; }

    }
}
