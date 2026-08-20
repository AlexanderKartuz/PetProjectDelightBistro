using DelightBistroMinimalApi.DbStuff.Models;
using DelightBistroMinimalApi.ModelsDto.EntityDto;

namespace DelightBistroMinimalApi.Mappings
{
    public interface IDrinkMapper
    {
        List<DrinkResponse> ToDrinkListResponse(IEnumerable<Drink> drinksData);
        DrinkResponse ToDrinkResponse(Drink drinkData);
        Drink ToEntity(DrinkRequest drinkRequest);
    }
}