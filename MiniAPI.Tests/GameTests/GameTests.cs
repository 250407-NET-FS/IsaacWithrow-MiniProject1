using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;
using MiniAPI.Services.Games.Commands;
using MiniAPI.Services.Games.Queries;
using MiniAPI.Models;
using MiniAPI.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;


namespace MiniAPI.Tests;

public class GameTests
{
    private readonly Mock<UserManager<User>> _userManagerMock;

    private readonly DbContextOptions<MiniAPIContext> _dbOptions;

    public GameTests()
    {
        _userManagerMock = new Mock<UserManager<User>>();
        _dbOptions = new DbContextOptionsBuilder<MiniAPIContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        // _dbMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));
        // _dbMock.Setup(x => x.Add(It.IsAny<Game>()));
    }

    GameCreateDTO validGameCreateDTO = new GameCreateDTO
    {
        Title = "siege",
        Publisher = "goobisoft",
        Price = 39.99m,
        ImageData = "someimage",
        ImageMimeType = "image.jpeg"
    };

    Guid validOwnerId = Guid.NewGuid();

    [Fact]
    public async Task CreateGameCommand_ShouldCreateValidGame()
    {
        //Arrange
        using var context = new MiniAPIContext(_dbOptions);
        var command = new CreateGame.Command
        {
            Dto = validGameCreateDTO,
            OwnerID = validOwnerId
        };
        var handler = new CreateGame.Handler(context);

        //Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.IsType<Guid>(result);
        // _dbMock.Verify(x => x.Add(It.IsAny<Game>()), Times.Once);
        // _dbMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        Assert.Single(context.Games);
    }

    [Fact]
    public async Task DeleteGameCommand_ShouldDeleteValidGame()
    {
        //Arrange
        using var context = new MiniAPIContext(_dbOptions);
        Game game = new Game(validGameCreateDTO);
        game.OwnerID = validOwnerId;
        context.Games.Add(game);
        await context.SaveChangesAsync();
        Assert.Single(context.Games);
        var command = new DeleteGame.Command
        {
            Id = game.GameID,
            UserId = validOwnerId
        };
        var handler = new DeleteGame.Handler(context);

        //Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.Equal(game.GameID, result);
        // _dbMock.Verify(x => x.Add(It.IsAny<Game>()), Times.Once);
        // _dbMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        Assert.Empty(context.Games);
    }

    [Fact]
    public async Task UpdateGameCommand_ShouldUpdateGame()
    {
        //Arrange
        using var context = new MiniAPIContext(_dbOptions);
        Game game = new Game(validGameCreateDTO);
        game.OwnerID = validOwnerId;
        context.Games.Add(game);
        await context.SaveChangesAsync();
        Assert.Single(context.Games);
        var mapperMock = new Mock<IMapper>();
        mapperMock
        .Setup(m => m.Map<GameUpdateDTO, Game>(It.IsAny<GameUpdateDTO>(), It.IsAny<Game>()))
        .Callback<GameUpdateDTO, Game>((src, dest) =>
        {
            dest.Title = src.Title!;
            dest.Publisher = src.Publisher!;
            dest.Price = src.Price;
            dest.ImageData = src.ImageData!;
            dest.ImageMimeType = src.ImageMimeType!;
        });
        var mapper = mapperMock.Object;
        GameUpdateDTO dto = new GameUpdateDTO
        {
            Title = "joe",
            Publisher = "joe",
            Price = 0.0m,
            ImageData = "joey",
            ImageMimeType = "joey"
        };
        var command = new UpdateGame.Command
        {
            Id = game.GameID,
            UserId = validOwnerId,
            Dto = dto
        };
        var handler = new UpdateGame.Handler(context, mapper);

        //Act
        var result = await handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.Equal(game.GameID, result);
        //_dbMock.Verify(x => x.Add(It.IsAny<Game>()), Times.Once);
        //_dbMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        Game g = context.Games.FirstOrDefault()!;
        Assert.Equal(dto.Price, g.Price);
        Assert.Equal(dto.ImageData, g.ImageData);
        Assert.Equal(dto.ImageMimeType, g.ImageMimeType);
        Assert.Equal(dto.Title, g.Title);
        Assert.Equal(dto.Publisher, g.Publisher);
    }

    [Fact]
    public async Task GetAllGames_ShouldReturnGames()
    {
        //Arrange
        using var context = new MiniAPIContext(_dbOptions);
        Game game = new Game(validGameCreateDTO);
        game.OwnerID = validOwnerId;
        context.Games.Add(game);
        await context.SaveChangesAsync();
        Assert.Single(context.Games);
        var query = new GetAllGames.Query { };
        var handler = new GetAllGames.Handler(context);

        //Act
        var result = await handler.Handle(query, CancellationToken.None);

        //Assert
        Assert.Equal(game.GameID, result[0].GameID);
        // _dbMock.Verify(x => x.Add(It.IsAny<Game>()), Times.Once);
        // _dbMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        Assert.Single(context.Games);
    }

    [Fact]
    public async Task DeleteGame_ShouldThrowException()
    {
        // Arrange
        using var context = new MiniAPIContext(_dbOptions);
        Game game = new Game(validGameCreateDTO);
        game.OwnerID = validOwnerId;
        context.Games.Add(game);
        await context.SaveChangesAsync();
        Assert.Single(context.Games);

        var command = new DeleteGame.Command
        {
            Id = game.GameID,
            UserId = Guid.NewGuid()
        };

        var handler = new DeleteGame.Handler(context);

        // Act and Assert
        await Assert.ThrowsAnyAsync<Exception>(async () =>
        {
            await handler.Handle(command, CancellationToken.None);
        });
    }

    [Fact]
    public async Task UpdateGame_ShouldThrowException()
    {
        // Arrange
        using var context = new MiniAPIContext(_dbOptions);
        Game game = new Game(validGameCreateDTO);
        game.OwnerID = validOwnerId;
        context.Games.Add(game);
        await context.SaveChangesAsync();
        Assert.Single(context.Games);

        var mapperMock = new Mock<IMapper>();
        mapperMock
        .Setup(m => m.Map<GameUpdateDTO, Game>(It.IsAny<GameUpdateDTO>(), It.IsAny<Game>()))
        .Callback<GameUpdateDTO, Game>((src, dest) =>
        {
            dest.Title = src.Title!;
            dest.Publisher = src.Publisher!;
            dest.Price = src.Price;
            dest.ImageData = src.ImageData!;
            dest.ImageMimeType = src.ImageMimeType!;
        });
        var mapper = mapperMock.Object;
        GameUpdateDTO dto = new GameUpdateDTO
        {
            Title = "joe",
            Publisher = "joe",
            Price = 0.0m,
            ImageData = "joey",
            ImageMimeType = "joey"
        };
        var command = new UpdateGame.Command
        {
            Id = game.GameID,
            UserId = Guid.NewGuid(),
            Dto = dto
        };
        var handler = new UpdateGame.Handler(context, mapper);

        // Act and Assert
        await Assert.ThrowsAnyAsync<Exception>(async () =>
        {
            await handler.Handle(command, CancellationToken.None);
        });
    }
}