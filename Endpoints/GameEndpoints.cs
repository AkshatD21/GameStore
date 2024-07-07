using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using GameStore.Api.Mapping;

namespace GameStore.Api.Endpoints;

public static class GameEndpoints
{
    const string GetGameEndPoint = "GetGame";
    private static readonly List<GameDto> games = [
        new(
            1,
            "Street Fighter",
            "Fighting",
            19.99M,
            new DateOnly(1992, 7, 5)
        ),
        new(
            2,
            "MineCraft",
            "Roleplaying",
            9.99M,
            new DateOnly(1989, 8, 1)
        ),
        new(
            3,
            "Fifa",
            "Sports",
            10.9M,
            new DateOnly(1909, 9, 8)
        ),
    ];

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app){

        var group = app.MapGroup("games").WithParameterValidation();


        //GET all games
        group.MapGet("/", ()=>games);
        
        //GET games by Id
        group.MapGet("/{id}", (int id) => 
        {
            GameDto? game = games.Find(game => game.Id == id);
            return game is null ? Results.NotFound() : Results.Ok(game);
        })
          .WithName(GetGameEndPoint);
        
        //POST a Game
        group.MapPost("/",(CreateGameDtos newGame, GameStoreContext dbContext)=>{
            
            Game game = newGame. ToEntity();
            game.Genre = dbContext. Genres. Find (newGame. GenreId);

            dbContext.Games.Add(game);
            dbContext.SaveChanges();

        return Results.CreatedAtRoute(GetGameEndPoint, new {id = game.Id}, game.ToDto());
        });    

        //PUT a game
        group.MapPut("/{id}", (int id, UpdateGameDto updatedGame)=>
        {
            var index = games.FindIndex(game => game.Id == id);
        
            if(index == -1){
                Results.NotFound();
            }
        
            games[index] = new GameDto(
                id,
                updatedGame.Name,
                updatedGame.Genre,
                updatedGame.Price,
                updatedGame.ReleaseDate
            );
            
            return Results.NoContent();
            });
        
            //DELETE Game
            app.MapDelete("games/{id}",(int id) =>
            {
                games.RemoveAll(game => game.Id == id);
            
                return Results.NoContent();
            });
        
            return group;
            }
        }        
