using Microsoft.AspNetCore.Mvc.Rendering;
using WebNet23Online.Data.Models;
using WebNet23Online.Data.Repositories.Interfaces;
using WebNet23Online.Models.AnimeGirl;
using WebNet23Online.Services.Interfaces;

namespace WebNet23Online.Services
{
    public class AnimeGirlGenerator : IAnimeGirlService
    {
        private IEpicMeanlessPhraseGenerator _phraseGenerator;
        private IRandomBuilder _randomBuilder;
        private IAnimeRepository _animeRepository;
        private const string DEFAULT_COVER = "https://i.pinimg.com/736x/49/58/8b/49588bb573d521482b58174210adbb77.jpg";

        public AnimeGirlGenerator(IEpicMeanlessPhraseGenerator epicMeanlessPhraseGenerator,
            IRandomBuilder randomBuilder,
            IAnimeRepository animeRepository)
        {
            _phraseGenerator = epicMeanlessPhraseGenerator;
            _randomBuilder = randomBuilder;
            _animeRepository = animeRepository;
        }

        public List<AnimeGirlImageInfoViewModel> GenerateList(List<AnimeGirlData> animeGirlDatas)
        {
            var viewModels = animeGirlDatas
                .Select(x => new AnimeGirlImageInfoViewModel
                {
                    Id = x.Id,
                    Url = x.Url,
                    Title = x.Name,
                    ConnectedAnimeTitles = string.Join(", ", x.Animes.Select(a => a.Name!)),
                    Likes = x.Likes
                });

            return viewModels
                .ToList();
        }

        public AnimeGirlImageInfoViewModel Generate()
        {
            return new AnimeGirlImageInfoViewModel
            {
                Url = "https://i.pinimg.com/474x/ed/3a/e8/ed3ae86ab479861a1e10e8d0caaf04de.jpg?nii=t",
                Title = _phraseGenerator.Generate(),
                ConnectedAnimeTitles = string.Empty
            };
        }

        public List<IndexAnimeViewModel> AnimeMap(List<AnimeData> animes)
        {
            return animes
                .Select(x => new IndexAnimeViewModel
                {
                    Name = x.Name,
                    Id = x.Id,
                    Url = x.CoverUrl ?? DEFAULT_COVER
                }).ToList();
        }

        public List<SelectListItem> GetListItemsWithAnime()
        {
            var animes = _animeRepository.GetAll();
            var animeListItems = new List<SelectListItem>();
            animeListItems.Add(new SelectListItem
            {
                Text = "SelectAnime",
                Value = ""
            });
            animeListItems.AddRange(animes.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }));
            return animeListItems;
        }
    }
}
