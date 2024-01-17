namespace CleanArchitecture.Domain.Abstractions;

public abstract class Entity
{
    public Entity()
    {
        Id = Guid.NewGuid().ToString();
    }
    public string Id {  get; set; } // id yi guId yapacağım için string tanımladık ve her seferinde özel Id oluşturacam
    public DateTime CreatedDate {  get; set; }
    public DateTime? UpdatedDate {  get; set; }//? null olabilir anlamına gelir
}
