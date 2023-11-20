using System.Text;

using Meziantou.Extensions.Logging.Xunit;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using SolutionTemplate.Application._;
using SolutionTemplate.Application.Articles.Commands;
using SolutionTemplate.Application.Articles.Queries;
using SolutionTemplate.Integration.Tests.Helpers;


namespace SolutionTemplate.Integration.Tests;


//[CollectionDefinition(nameof(ArticleIntegrationTests), DisableParallelization = true)]
//public class ArticleTestCollectionFixture : ICollectionFixture<ApiTestApplicationFactory> { }

//[Collection(nameof(ArticleIntegrationTests))]
public sealed class ArticleIntegrationTests : IAsyncLifetime, IAsyncDisposable
{
    //private readonly ITestOutputHelper _testOutputHelper;
    private readonly ApiTestApplicationFactory _factory;
    //private readonly HttpClient _client;
    //private readonly Func<Task> _resetDatabase;
    public ArticleIntegrationTests(ITestOutputHelper testOutputHelper)
    {
        //_testOutputHelper = testOutputHelper;
        _factory = new ApiTestApplicationFactory(testOutputHelper);

        
        //var waf = new ApiTestApplicationFactory()
        //    .WithWebHostBuilder(builder => 
        //    {
        //        builder.ConfigureLogging(x =>
        //        {
        //            x.ClearProviders();
        //            x.Services.AddSingleton<ILoggerProvider>(new XUnitLoggerProvider(_testOutputHelper));
        //        });
        //    })
        //    .WithWebHostBuilder;

        //_client = factory.HttpClient;
        //_resetDatabase = factory.ResetStateAsync;
    }

    [Fact]
    public async Task CreateArticle()
    {
        // Arrange
        var json = JsonConvert.SerializeObject(new CreateArticle.Command("Title", "Content", "Email", "Firstname", "Lastname", "1", "2", "3"));
        var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _factory.HttpClient.PostAsync("/article/", stringContent);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var article = JsonConvert.DeserializeObject<CreateArticle.Response>(responseString);

        article.Should().NotBeNull();
    }

    [Fact]
    public async Task GetArticle()
    {
        // Act
        var response = await _factory.HttpClient.GetAsync($"/article/{TestData.ArticleSolutionTemplate.Id}");

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var article = JsonConvert.DeserializeObject<GetArticle.Response>(responseString);

        article.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateArticle()
    {
        // Arrange
        var json = JsonConvert.SerializeObject(new UpdateArticle.Command(TestData.ArticleSolutionTemplate.Id, "New Title", "New Content"));
        var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _factory.HttpClient.PutAsync("/article/", stringContent);


        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task GetArticles()
    {
        // Act
        var response = await _factory.HttpClient.GetAsync("/article");

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var articles = JsonConvert.DeserializeObject<PagedList<GetArticles.Response>>(responseString);

        articles!.Items.Should().HaveCount(2); // This works because the database is reset after each test
    }


    [Fact]
    public async Task DeleteArticle()
    {
        // Act
        var response = await _factory.HttpClient.DeleteAsync($"/article/{TestData.ArticleSolutionTemplate.Id}");

        // Assert
        response.EnsureSuccessStatusCode();
    }



    public Task InitializeAsync() => _factory.InitializeAsync();
    public Task DisposeAsync()
    {

        return _factory.DisposeAsync();
        //_factory.ResetStateAsync();
    }

    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        await _factory.DisposeAsync();
    }
}