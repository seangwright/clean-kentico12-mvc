# Clean Kentico12 MVC

An example of Clean/Onion Architecture with Kentico 12 MVC

## Repository Structure

```bash
📦/
 ┣ 📂docs
 ┣ 📂src
 ┃ ┣ 📂delivery
 ┃ ┃ ┣ 📜Sandbox.Delivery.Core (Core abstractions / POCOs)
 ┃ ┃ ┣ 📜Sandbox.Delivery.Data (Kentico data access)
 ┃ ┃ ┗ 📜Sandbox.Delivery.Web (MVC / Web API)
 ┃ ┣ 📂management
 ┃ ┃ ┣ 📜Sandbox.Management.Core (Coe abstractions / POCOs)
 ┃ ┃ ┣ 📜Sandbox.Management.Data (Kentico data access)
 ┃ ┃ ┗ 📜Sandbox.Management.Web (CMS not included)
 ┃ ┣ 📂shared
 ┃ ┃ ┣ 📜Sandbox.Core.Domain (Shared abstractions / POCOs)
 ┃ ┃ ┗ 📜Sandbox.Data.Kentico (Shared Kentico data access)
 ┣ 📂tests
 ┃ ┗ 📂...
 ┗ 📜Sandbox.sln
```

![Solution Project References](./docs/project-references.drawio.svg)

## Goals

Provide a source for architectural guidance and discussion for the Kentico Xperience community when building Kentico MVC applications.

> Note: This is not a seed project. The repository can be cloned and added to a CMS application, but the primary goal here is a sandbox for patterns and architecture.

> Feel free to copy and paste the code here into your projects (with attribution).

## High Level Patterns

- Thin Controllers
- Request/Handler/ViewModel
- `Result<T>` and `Option<T>` (no `null`)
- Dependency Injection
- Cross-Cutting Concerns via Decoration
- `NodeAliasPath` routing
- Context Abstractions

## How to Use this Repository

Clone this repository locally and open `Sandbox.sln` in Visual Studio.

Then, explore the code base by following code lens references, and using the documentation and diagrams for explanations.

Optional: Connect the repository to a Kentico Xperience CMS (`Sandbox.Management.Web`) application and database and use debugging and code changes to see how requests / responses flow through the architecture.

## External Resources

- [Enterprise Craftsmanship](https://enterprisecraftsmanship.com/posts/functional-c-handling-failures-input-errors/) - Vladimir Khorikov
- [.NET Junkie](https://blogs.cuttingedge.it/steven/posts/2019/di-composition-models-primer/) - Steven van Deursen
- [ploeh blog](https://blog.ploeh.dk/2015/10/26/service-locator-violates-encapsulation/) - Mark Seeman
- [Los Techies](https://lostechies.com/jimmybogard/2016/10/27/cqrsmediatr-implementation-patterns/) - Jimmy Bogard
