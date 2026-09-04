# _01_WhereItComes

Stage 1 of the Dependency Injection article: the version with no DI at all.

A console menu with three options: register a user, read a user by line number,
exit. Everything lives in `Program.cs` as local functions. No class, no
interface, no abstraction, and the storage path `data/users.csv` is typed out
again in every place that touches it.

No database and no NuGet package, on purpose. The reader should be able to run
it immediately. This is the code the article argues against; each later stage
removes one of its problems.

## Run

```bash
dotnet run
```

```
1) register user
2) read user
0) exit
> 1
email: erdinc@example.com
display name: Erdinc
saved

1) register user
2) read user
0) exit
> 2
line number: 1
erdinc@example.com,Erdinc
```

Rows land in `data/users.csv` in the project folder, one user per line, no
header. Line 1 is the first user. Delete the `data` folder to start over.

## What is deliberately wrong here

- `data/users.csv` is repeated as a literal in three places. A second table
  means a second path, repeated the same way.
- Storage choice is hard-coded. Swapping CSV for a database means rewriting
  both functions.
- Neither function can be tested without touching the real file system.
- The CSV format is split across two functions with nothing keeping them in
  agreement: `RegisterUser` writes the columns, `ReadUser` returns the raw line.
