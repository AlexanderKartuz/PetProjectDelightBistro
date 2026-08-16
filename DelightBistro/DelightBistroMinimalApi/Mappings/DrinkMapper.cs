using DelightBistroMinimalApi.DbStuff;
using DelightBistroMinimalApi.ModelsDto.EntityDto;

namespace DelightBistroMinimalApi.Mappings
{
    public class DrinkMapper : IDrinkMapper
    {
        public Drink ToEntity(DrinkRequest drinkRequest)
        {
            return new Drink
            {
                Name = drinkRequest.Name,
                Price = drinkRequest.Price,
                Description = drinkRequest.Description,
                ImgUrl = drinkRequest.ImgUrl,
            };
        }

        public DrinkResponse ToDrinkResponse(Drink drinkData)
        {
            return new DrinkResponse
            {
                Id = drinkData.Id,
                Name = drinkData.Name,
                Price = drinkData.Price,
                Description = drinkData.Description,
                ImgUrl = drinkData.ImgUrl,
            };
        }

        public List<DrinkResponse> ToDrinkListResponse(IEnumerable<Drink> drinksData)
        {
            return drinksData.Select(ToDrinkResponse).ToList();
        }
    }
}
