**Template Breakdown**

*Overview* - 
- `shared:`  Has shared projects for all the respective 4 layers of the clean architecture. These projects should contain any global-standard/widely-shared/conventions related specifications and implementations.
- `db:` A console application (`DbManager`) have been isolated from the rest of the project to maintain and keep ef migrations outside the project source code.
- 4 main projects for each layers of clean architecture.

1. Nullability
	- Since it reduces the chances of NullRefException so it's good to enable.
	- Having said that, DTOs, CQs, EF Models (i.e. classes that are initialized via framework and not manually via dev-written code) should still disable this feature.
	- Considering we use DI, EF Code First, API Framework (DTO Model Binding) so enabiling this feature we don't gain much.
	- So, this should be disabled atleast for Domain, Application Layers. And DbContext class in Infrastructure layer.

2. `Application.Shared` project has following important service interfaces (implementated in `Infrastructure.Shared`) to start with - 
	- IAppSettings
	- ICachingService
	- IFileStorageService
	- ILookupService
	- IHttpService, etc.

	[**NOTE**: Any of the above mentioned service implementations can be modified as per the need of individual projects]

3. Always use IDateTimeService instead of using static DateTime.Now, UtcNow, etc. This improves testability.
4. How Lookup can be utilized for ease - 
	- Store all lookup data(e.g. enums) configurations in LookupTypes, LookupValues and add FK ref wherever needed in other tables.
	- Few endpoints have been exposed in `SysController` to maintain/retrive the lookups.
	- Now in you command/query model if you do the following, the public accessor of the backing field gets automatically populated by the `LookupModelProcessor` defined in `API.Shared` 
		- Add a backing field (named as `_{FKNameWithIdSuffixRemovedAndInCamelCase}`) and a public accessor for this backing field.
		- Annotate the FK property with the `Lookup("{LookupTypeName}")` attribute.		
5. Application.Shared have some base CQ Models, CQ Handlers and VMs as follows - 
	- CQ Models and Handlers (CQRS)
		- BaseHandler (Use when your CQ are not particular entity specific or your entity can't derive from BaseEntity)
		- GetSingle(Query/QueryHandler)
		- GetList(Query/QueryHandler)
		- SaveSingle(Command/CommandHandler)
		- DeleteSingle(Command/CommandHandler)
	- VMs (Models)
		- BaseVM, BaseVM<T>
		- SingleVM<T>
		- ListVM<T>
		- FileVM

6. `ExampleController` have some examples to showcase the usage of following - 
	- Handling files in application layer by the usage of FileVM and FileIM.
	- Making external http calls.

7. Application.Shared also expose `IMapFrom` and `IMapTo` interfaces for auto registeration of mappers for ease.

------

**Conventions**

*CQRS*
- Keep Command/Query as prefix for .cs file of your command/queries. For e.g. `CommandSaveEmpDetails.cs`
- Command/Query and handlers naming should be of the following format - `SaveEmpDetailsCommand`, `GetEmpDetailsQuery` | `SaveEmpDetailsCommandHandler`, `GetEmpDetailsQueryHandler`.
- Anything related to a command/query should be kept with-in the same file. If needed, encapsulate related code in regions.
	- Command/Query Model
	- Related Input Models (if needed)
	- View Models
	- Validator
	- Handler
- FluentValidator should contain validations only for models. Complex business logic related validations which involved db queries shoudl be done inside the handlers. Devs just need to throw ValidationException from the code, an appropriate response message would be framed by the `HanldeResult` method of `ApiControllerBase`.
- Any implementation that seems to be of reusable nature, should be considered to move in a Service.
