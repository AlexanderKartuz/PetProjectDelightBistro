using System.Collections;
using WebNet23Online.Models.DTOs;

namespace WebNet23Online.Models.AnimeGirl
{
    public class MainIndexViewModel
    {
        public List<AnimeGirlImageInfoViewModel> AnimeGirls { get; set; }
        public List<IndexAnimeViewModel> Animes { get; set; }
        public bool CanDeleteGirl { get; set; }
        public JokeDto Joke { get; set; }
        public WaifuDtoRoot Waifu { get; set; }
        public List<CatDto> Cats { get; set; }
    }
}
