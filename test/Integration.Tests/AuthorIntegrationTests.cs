using System.Text;

using FluentAssertions;

using Newtonsoft.Json;

using SolutionTemplate.Application.Authors.Commands;
using SolutionTemplate.Application.Authors.Queries;
using SolutionTemplate.Integration.Tests.Helpers;
using Xunit;

namespace SolutionTemplate.Integration.Tests;


[CollectionDefinition("Author Collection", DisableParallelization = true)]
public class AuthorTestCollectionFixture : ICollectionFixture<CustomWebApplicationFactory> { }


[Collection("Author Collection")]
[TestCaseOrderer("SolutionTemplate.Integration.Tests.Helpers.PriorityOrderer", "Integration.Tests")]
public sealed class AuthorIntegrationTests
{
    private readonly CustomWebApplicationFactory _factory;
    private static Guid _id = Guid.Empty;

    public AuthorIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }


    [Fact, TestPriority(1)]
    public async Task O1_CreateAuthor()
    {
        // Arrange
        var json = JsonConvert.SerializeObject(new CreateAuthor.Command("Email", "Firstname", "Lastname"));
        var stringContent = new StringContent(json, Encoding.UTF8, "application/json");


        // Act
        var response = await _factory.HttpClient.PostAsync("/author", stringContent);


        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        _id = JsonConvert.DeserializeObject<Guid>(responseString);
        _id.Should().NotBeEmpty();
    }

    [Fact, TestPriority(2)]
    public async Task O2_GetAuthor()
    {
        // Act
        var response = await _factory.HttpClient.GetAsync($"/author/{_id}");

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var author = JsonConvert.DeserializeObject<GetAuthor.Response>(responseString);
        author.Should().NotBeNull();
    }


    [Fact, TestPriority(3)]
    public async Task O3_UpdateAuthor()
    {
        // Arrange
        var json = JsonConvert.SerializeObject(new UpdateAuthor.Command(_id, "New Title", "New Content"));
        var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _factory.HttpClient.PutAsync("/author/", stringContent);

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact, TestPriority(4)]
    public async Task O4_GetAuthors()
    {
        // Act
        var response = await _factory.HttpClient.GetAsync("/author");

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var authors = JsonConvert.DeserializeObject<IList<GetAuthors.Response>>(responseString);
        authors.Should().NotBeEmpty();
        authors.Should().HaveCount(2); // 2, because of the seeded data
    }


    [Fact, TestPriority(5)]
    public async Task O5_DeleteAuthor()
    {
        // Act
        var response = await _factory.HttpClient.DeleteAsync($"/author/{_id}");

        // Assert
        response.EnsureSuccessStatusCode();
    }
}
