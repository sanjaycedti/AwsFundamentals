using System;
namespace Consumers.Api.Services;

public interface IGitHubService
{
    Task<bool> IsValidGitHubUser(string username);
}

