using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;
using MiniAPI.Services.Games.Commands;
using MiniAPI.Services.Games.Queries;
using MiniAPI.Models;
using MiniAPI.Data;
using Microsoft.EntityFrameworkCore;


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
}