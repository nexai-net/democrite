# Democrite.Toolbox

Technical library shared by all the Democrite projects.

> [!NOTE]
> This library was previously published as the separate packages **Elvex.Toolbox**.<br />
> It is now part of the Democrite repository and renamed **Democrite.Toolbox** (namespaces `Elvex.Toolbox.*` -> `Democrite.Toolbox.*`).

## Projects

| Project | Content |
| --- | --- |
| Democrite.Toolbox.Abstractions | Interfaces, models, attributes, conditions, expressions, ... |
| Democrite.Toolbox | Implementations: extensions, serialization (Newtonsoft converters, type solver), disposables, patterns (strategy, workers), services (hash, file system, globalization), ... |
| Democrite.Toolbox.Configuration | Configuration helpers |
| Democrite.Toolbox.WPF / .WPF.Abstractions | WPF tools (Windows only) |
| Democrite.Toolbox.WPF.UI / .WPF.UI.Avalon | WPF controls, themes and AvalonDock integration (Windows only), XAML namespace `http://democrite.toolbox.ui/` |
| tests/Democrite.Toolbox.UnitTests.ToolKit | Unit test helpers (xunit, AutoFixture, NFluent, NSubstitute) |
| tests/* | Unit tests and WPF / ComServer test applications |
| benchmarks/Democrite.Toolbox.Benchmark.Sandbox | BenchmarkDotNet sandbox |

## Build and test

The projects are part of `Democrite.sln` (solution folders *Toolbox*, *Toolbox/Tests* and *Toolbox/Benchmarks*):

```shell
dotnet build Democrite.sln
dotnet test Democrite.sln
```

## License

MIT, cf. [LICENSE](LICENSE) (original author: Mickael Thumerel).
