namespace GameStore.Api.Dtos;

public record class GameDto(
    int Id,
    string Name,
    string Genre,
    decimal Price,
    DateOnly ReleaseDate
)
{
    public static implicit operator GameDto(GameSummaryDto v)
    {
        throw new NotImplementedException();
    }
}
