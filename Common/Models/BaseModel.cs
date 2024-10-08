namespace foodswap.Common.Models;
public abstract class BaseModel
{
    public BaseModel()
    {
        Id = Guid.NewGuid();
    }

    public Guid Id { get; protected set; }
    public DateTime CreatedAt { get; protected set; }
    public DateTime UpdatedAt { get; protected set; }
}