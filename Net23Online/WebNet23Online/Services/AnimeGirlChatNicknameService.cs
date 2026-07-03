using WebNet23Online.Services.Interfaces;

namespace WebNet23Online.Services;

public class AnimeGirlChatNicknameService : IAnimeGirlChatNicknameService
{
    private static readonly string[] Adjectives =
    [
        "Розовый",
        "Неизвестный",
        "Сонный",
        "Храбрый",
        "Ленивый",
        "Весёлый",
        "Тайный",
        "Пушистый",
        "Загадочный",
        "Сказочный"
    ];

    private static readonly string[] Nouns =
    [
        "Пони",
        "Барсук",
        "Енот",
        "Котик",
        "Дракон",
        "Единорог",
        "Лисёнок",
        "Панда",
        "Кролик",
        "Сова"
    ];

    public string Generate()
    {
        var adjective = Adjectives[Random.Shared.Next(Adjectives.Length)];
        var noun = Nouns[Random.Shared.Next(Nouns.Length)];
        return $"{adjective} {noun}";
    }
}
