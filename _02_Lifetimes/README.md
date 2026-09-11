# _02_Lifetimes

Stage 2 of the Dependency Injection article: the three container lifetimes, and
what breaks when one service outlives another.

A console menu with three options: register a user, change language, exit. The
outer loop opens one scope per menu round, so "round" and "scope" mean the same
thing here. Three services, one per lifetime:

- `Translator` — **singleton**. Reads `messages.en.txt` and `messages.tr.txt` in
  its constructor and keeps both catalogs for the life of the process.
- `OperationLogger` — **scoped**. Collects lines in memory and appends them to
  `log.txt` when the scope is disposed.
- `FieldValidator` — **transient**. A new instance, and a new empty error list,
  on every resolve.

One NuGet package, `Microsoft.Extensions.DependencyInjection`, and nothing else.

## Run

```bash
dotnet run
```

```
translator: reading messages.en.txt
translator: reading messages.tr.txt

1) English
2) Türkçe
> 1

1) Register user
2) Change language
0) Exit
Your choice > 1
type =exit to close this scope
Please enter a name > E
Please enter your email > not-an-email
  ! Name must be at least 2 characters
  ! Email is invalid
Please enter a name > Erdinc
Please enter your email > erdinc@example.com
Saved
Please enter a name > =exit

1) Register user
2) Change language
0) Exit
Your choice > 0
```

That session leaves this in `log.txt`:

```
+++ BEGIN SCOPE = b59b36ab-09fe-406b-a82d-195519bd7c85 +++
language selected: en
user not added: E / not-an-email -> error.name.short, error.email.invalid
user added: Erdinc / erdinc@example.com
+++ END SCOPE = b59b36ab-09fe-406b-a82d-195519bd7c85 +++
+++ BEGIN SCOPE = 0cdc179f-33e6-4fc5-9e54-80a758e18a8b +++
exit requested
+++ END SCOPE = 0cdc179f-33e6-4fc5-9e54-80a758e18a8b +++
```

`log.txt` and the two message files sit next to the binary, in
`bin/Debug/net10.0/`, because `Translator` and `OperationLogger` both build
their paths from `AppContext.BaseDirectory`. The log is appended to, never
truncated — delete it to start over.

## What the run shows

- **Singleton.** The two `translator: reading ...` lines appear once, at
  startup. Every later round gets the same instance, so the files are never
  read again. The selected language survives across scopes for the same reason:
  it is state on an object nobody replaces.
- **Scoped.** `log.txt` holds one `BEGIN`/`END` block per menu round, each with
  its own id. Registering users and exiting land in different blocks because
  they happened in different scopes.
- **Transient.** The second user is not rejected for the first user's mistakes.
  `FieldValidator` is resolved inside the input loop and accumulates errors in a
  field, so it needs to be a fresh instance every time. Registered as scoped
  (step 06) it carries one user's errors into the next; step 07 makes it
  transient.

## Where lifetimes leak

Steps 08 to 13 are one arc: break it, make the breakage visible, fix it.

- **08** resolves `OperationLogger` from the root provider instead of from the
  scope. It compiles and it runs, but there is now one logger for the whole
  process, and it belongs to a provider nobody disposes — so `log.txt` is never
  written at all.
- **09** adds `provider.Dispose()` before the app exits. The log comes back, but
  as a single block covering the entire session instead of one per round. This
  is the quiet version of the bug: wrong output, no error.
- **10** turns scope validation on:
  ```csharp
  var provider = services.BuildServiceProvider(new ServiceProviderOptions
  {
      ValidateScopes = true
  });
  ```
  The same code now fails loudly:
  ```
  System.InvalidOperationException: Cannot resolve scoped service 'OperationLogger' from root provider.
  ```
- **11** resolves from the scope again, and the per-round blocks return.
- **12** gives the singleton `Translator` a constructor dependency on the scoped
  `OperationLogger`. That is a captive dependency: the scoped object is pinned
  inside a singleton and would live forever. Validation catches it on the first
  resolve:
  ```
  System.InvalidOperationException: Cannot consume scoped service 'OperationLogger' from singleton 'Translator'.
  ```
- **13** drops that dependency. `Program.cs` logs the language change instead,
  where a scoped logger is legal.

The two exceptions are the point of the part: the container can prove both
mistakes at runtime, but only after `ValidateScopes` is on.

## Reading the steps

Each step is a tag. Compare one against the one before it:

```
https://github.com/erdyasan/dotnet-dependency-injection/compare/lifetimes-10...lifetimes-11
```
