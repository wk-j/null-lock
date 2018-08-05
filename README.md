## Null Lock Analyzer

```csharp
static void LockNull() {
    lock (null) {
        Console.WriteLine("Bug ...");
    }
}
```

Installation

```bash
dotnet tool install -g wk.NullLock
```

Usage

```bash
$ wk-null-lock-analyzer tests/MyApp/MyApp.csproj
! Program.cs(7,19): error P911: '<null>' is not a reference type as required by the lock statement.
```