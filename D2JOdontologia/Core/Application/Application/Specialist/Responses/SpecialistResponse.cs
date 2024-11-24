using Application.Dtos;


namespace Application.Responses
{
    public class SpecialistResponse : Response
    {
        public SpecialistResponseDto SpecialistData;
        public SpecialtyDto SpecialtyData {  get; set; }
    }
}
