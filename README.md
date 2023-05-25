# GYBS
GYBS is a set of small but useful things for any .NET Standard project. The idea behind is not to enforce anything - just grab the parts you're intrested in and help yourself.

## How to use it?

### Base
Base library, available at [NuGet](https://www.nuget.org/packages/Gybs), consists of three main parts:
* various extension methods
* `IResult` interface its implementation
* dependency injection utils

#### Extension methods
Example usage:
```
Task<bool> IsPresent(string str) => str.IsPresent().ToCompletedTask();
ValueTask<bool> IsPresent(string str) => str.IsPresent().ToCompletedValueTask();

new [] { 1, 2, 3, 4 }.ForEach(e => Magic(e));
await new [] { 1, 2, 3, 4 }.ForEachAsync(async e => await MagicAsync(e));
```

#### `IResult` and `Result`
`IResult` represents the operation result, where `Result` is an implementation of this interface.

Examples:
```
return Result.Success(new Model());
return new Model().ToSuccessfulResult();
```

```
return Result.Failure("Field1", "value");
return Result.Failure<Model>(m => m.Field2, "value");
return Result.Failure<Model>(m => m.Collection1.First().Field3, "value");
return Result.Failure<Model>(m => m.List1[0].Field4, "value");
```

```
var errors = new ResultErrorsDictionary();
errors.Add("Field1", "value");
errors.Add<Model>(m => m.Field2, "value");
errors.Add<Model>(m => m.Collection1.First().Field3, "value");
errors.Add<Model>(m => m.List1[0].Field4, "value");
return Result.Failure(errors).Map<Model>();
```

#### Dependency injection
Interfaces and attributes for component auto-registration.
```
class DummyScopedService : IScopedService {}

[SingletonService]
class DummySingletonService {}

[SingletonService, SingletonService("special")]
class DummySpecialSingletonService {}

serviceCollection.AddGybs(builder => {
    builder.AddInterfaceServices();
    builder.AddAttributeServices();
    builder.AddAttributeServices(group: "special");
);
```

### Logic.Operations
Operations library, available at [NuGet](https://www.nuget.org/packages/Gybs.Logic.Operations), allows to decouple the operation contracts from the handler implementations. This is achieved by:
* separating `IOperation` from `IOperationHandler`
* putting the `IOperationBus` between, which takes the reponsibility of finding the correct handler for each operation

By default, library provides the `ServiceProviderOperationBus` which resolves the handler from DI container.

```
serviceCollection.AddGybs(builder => {
    builder.AddServiceProviderOperationBus();
    builder.AddOperationFactory();
    builder.AddAttributeServices();
);

class DummyOperation : IOperation<string> {}

[TransientService]
class DummyOperationHandler : IOperationHandler<DummyOperation, string>
{
    public async Task<IResult<string>> HandleAsync(DummyOperation operation)
    {
        return "success".ToSuccessfulResult();
    }
}

IOperationFactory factory;

var result = await factory
    .Create<DummyOperation>()
    .HandleAsync();
```

### Logic.Cqrs
CQRS library, available at [NuGet](https://www.nuget.org/packages/Gybs.Logic.Cqrs), is a wrapper around Operations replacing `IOperation` and `IOperationHandler` with `IQuery/ICommand` and `IQueryHandler/ICommandHandler`.

### Logic.Events
Events library, available at [NuGet](https://www.nuget.org/packages/Gybs.Logic.Events), provides two basic interfaces for the events support: `IEvent` and `IEventBus`. Additionally, it provides the `InMemoryEventBus` implementation.

```
serviceCollection.AddGybs(builder => {
    builder.AddInMemoryEventBus();
);

class Event : IEvent {}

IEventBus eventBus;
await eventBus.SubscribeAsync(e => Task.CompletedTask);
await eventBus.SendAsync(new Event());
```

### Logic.Validation
Validation, available at [NuGet](https://www.nuget.org/packages/Gybs.Logic.Validation), allows to separate validation logic from the rest of the application. This is achived by grouping the implementations of `IValidationRule` interface into the single validator by `IValidator`.

```
serviceCollection.AddGybs(builder => {
    builder.AddValidator();
    builder.AddAttributeServices();
);

[TransientService]
class ValueIsPresentRule : IValidationRule<string>, IValidationRule<RandomType>
{
    public async Task<IResult> ValidateAsync(string str)
    {
        return result.IsPresent()
            ? Result.Success()
            : Result.Failure(null);
    }
    
    public async Task<IResult> ValidateAsync(RandomType type)
    {
        return type is not null
            ? Result.Success()
            : Result.Failure(null);
    }
}

IValidator validator;

var result = await validator
    .Require<ValueIsPresentRule>
        .WithOptions(o => o.StopIfFailed())
        .WithData("")
    .Require<ValueIsPresentRule>
        .WithData(new RandomType())
    .Require<ValueIsPresentRule>
        .WithData((string)null)
    .Require<ValueIsPresentRule>
        .WithData((RandomType)null)
    .Require<ValueIsPresentRule>
        .WithData(() => obj.Property)
    .ValidateAsync();
```

### Data.Ef
Ef library, available at [NuGet](https://www.nuget.org/packages/Gybs.Data.Ef), provides the wrappers around `DbContext` and `DbSet<>` for grouping the extensions with the queries.

```
class Context : DbContext
{
    public DbSet<Model> Models { get; set;}
}

static class ModelQueries
{
    public static IQueryable<Model> Active(this DbSetQueries<Model> models)
    {
        return models.Entities.Where(m => m.Active);
    }
}

new Context().Models.Queries().Active();
```

### Data.Repositories
Repositories library, available at [NuGet](https://www.nuget.org/packages/Gybs.Data.Repositories), provides interfaces for repository and unit of work patterns.

## Why to use it?
I don't know. You need to figure it out yourself.