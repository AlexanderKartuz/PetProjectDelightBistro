namespace MovieMinimalApi.Dtos
{
    public record CreateMovieRequest(string Name, string Url, int Rating, List<string>? Tags);

    public record MovieDto(int Id, string Name, string Url, int Rating, List<string> Tags);

    public record TagRequest(int MovieId, string TagName);
}
