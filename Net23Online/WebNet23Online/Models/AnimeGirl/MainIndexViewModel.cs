using WebNet23Online.Data.HelperModels.SteamPagination;
using WebNet23Online.Models.DTOs;

namespace WebNet23Online.Models.AnimeGirl
{
    public class MainIndexViewModel
    {
        public List<AnimeGirlImageInfoViewModel> AnimeGirls { get; set; }
        public List<AnimeGirlImageInfoViewModel> AllAnimeGirls { get; set; }
        public List<IndexAnimeViewModel> Animes { get; set; }
        public AnimeGirlHeroesPaginationFilterViewModel HeroesFilter { get; set; } = new();
        public PaginationMetadataViewModel HeroesPagination { get; set; } = new();
        public bool CanDeleteGirl { get; set; }
        public JokeDto Joke { get; set; }
        public WaifuDtoRoot Waifu { get; set; }
        public List<CatDto> Cats { get; set; }
    }
}
