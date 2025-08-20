namespace DTO.Models.Producer
{
    public class CreateProducerDTO
    {
        public string Name { get; set; }

        public ICollection<int> Categories { get; set; }
    }
}
