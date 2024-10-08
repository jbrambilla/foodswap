using foodswap.Common.Models;

namespace foodswap.Features.SwapperFeatures.Models;

public class Swapper : BaseModel
{
    public Swapper(string userId, string name, string description)
    {
        UserId = userId;
        Name = name;
        Description = description;
    }

    public string UserId { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;

    public void Update(string name, string description)
    {
        Name = name;
        Description = description;
    }

    //EF RELATION
    public ICollection<FoodSwap> FoodSwaps { get; set; } = new List<FoodSwap>();
}