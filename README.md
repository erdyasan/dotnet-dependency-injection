# dotnet-dependency-injection

Sample code for the article "**.NET'te Dependency Injection**"
(https://erdincyasan.com).

Each folder is one part of the article. Inside a folder the app is **rewritten
step by step across commits**, so the interesting thing here is not the final
state but the diff between two steps.

## Parts

| Folder | Part |
|---|---|
| `_01_WhereItComes/` | Why dependency injection exists. Starts with code that has none. |

## Reading the history

Every step is tagged. To see what one step changed, compare it against the one
before it:

```
https://github.com/erdyasan/dotnet-dependency-injection/compare/where-it-comes-01...where-it-comes-02
```

`git log --oneline` and `git tag` list everything.

## Running a sample

Every sample is a plain console app with no external dependency.

```bash
cd _01_WhereItComes
dotnet run
```

Requires the .NET 10 SDK.
