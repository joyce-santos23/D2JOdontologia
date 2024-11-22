using Application.Dtos;


namespace Application.Responses
{
    public class SpecialistResponse : Response
    {
        public SpecialistDto SpecialistData;
        public SpecialtyDto SpecialtyData {  get; set; }
    }
}
