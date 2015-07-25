using Contracts.Enums;

namespace Contracts.Models
{
    public class Animal : IEntity
    {
        public string Name { get; set; }
        public CommonName CommonName { get; set; }
        public int Id { get; set; }
    }
}