namespace AngularMarketplace.Server.DTOs.Producer
{
    public class CreateProducerDTO
    {
        public string Name { get; set; }

        public ICollection<int> Categories { get; set; }
    }
}
