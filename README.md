# EF core support for ICG framework
The library contains couple of useful classes and extensions to register DbContext in ServiceCollection.

The library is dependent to core version of entity framework and the following external library:
- UnitOfWork: https://github.com/Arch/UnitOfWork/

<!-- TOC -->

- [EF core support for ICG framework](#ef-core-support-for-icg-framework)
    - [How to use](#how-to-use)
        - [API/Console projects](#apiconsole-projects)
            - [Extensions](#extensions)
            - [Extra Extensions](#extra-extensions)
        - [DbContextBase](#dbcontextbase)
        - [Test Projects](#test-projects)
            - [Extensions](#extensions)

<!-- /TOC -->

## How to use
The library provides helper methods for both API projects and test (unit and integration) projects.

### API/Console projects
To use the libray in any projects you can register context using the following snippet:

```csharp
services.AddSqlDbContext<DbContextClass>(Configuration.GetConnectionString("DbContextConnectionString"));
```

The libary provides some base classes and extensions which facilitate dealing with different scenarios in the development. The original libary provides the full implementation of unit of work which can be useful to every projects. Here is a sample of the usage:

``` csharp
public CompanyListRequestHandler(IUnitOfWork<DealManagementDbContext> unitOfWork,
    ILogger<CompanyListRequestHandler> logger)
{
    _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
}
public async Task<IEnumerable<CompanyDto>> Handle(CompanyListRequest request, 
    CancellationToken cancellationToken)
{
    if (request == null) throw new ArgumentNullException(nameof(request));

    _logger.LogInformation($"Handling company list request");

    var repositry = await _unitOfWork.GetRepository<Company>();

    // Return the list of the companies
}
```

Built-in support is available for CRUD operation as one or many operations.

#### Extensions
The library provides extended list of the extensions

```csharp
IEnumerable<TEntity> GetList<TEntity>(Specification<TEntity> spec,
	Func<IQueryable<TEntity>, IQueryable<TEntity>> extend = null)

Task<IEnumerable<TEntity>> GetListAsync<TEntity>(Specification<TEntity> spec,
	Func<IQueryable<TEntity>, IQueryable<TEntity>> extend = null)

IEnumerable<TEntity> GetList<TEntity>(
	Func<IQueryable<TEntity>, IQueryable<TEntity>> extend = null)

Task<IEnumerable<TEntity>> GetListAsync<TEntity>(
	Func<IQueryable<TEntity>, IQueryable<TEntity>> extend = null)      

IEnumerable<TEntity> GetList<TEntity>(Expression<Func<TEntity, bool>> predicate,
	Func<IQueryable<TEntity>, IQueryable<TEntity>> extend = null)

IEnumerable<TEntity> GetListAsync<TEntity>(Expression<Func<TEntity, bool>> predicate,
	Func<IQueryable<TEntity>, IQueryable<TEntity>> extend = null)

TEntity GetFirstItemOrDefault<TEntity>(Specification<TEntity> spec,
	Func<IQueryable<TEntity>, IQueryable<TEntity>> extend = null)

Task<TEntity> GetFirstItemOrDefaultAsync<TEntity>(Specification<TEntity> spec,
	Func<IQueryable<TEntity>, IQueryable<TEntity>> extend = null)    

TEntity GetFirstItemOrDefault<TEntity>(Expression<Func<TEntity, bool>> predicate,
	Func<IQueryable<TEntity>, IQueryable<TEntity>> extend = null)

Task<TEntity> GetFirstItemOrDefaultAsync<TEntity>(Expression<Func<TEntity, bool>> predicate,
	Func<IQueryable<TEntity>, IQueryable<TEntity>> extend = null)
  
```

Here is some samples from the usage:

**Simple Query**

```csharp
var companies = await _unitOfWork.GetRepository<Company>().GetListAsync(q => q.OrderBy(c => c.CompanyName));
```

**Query with includes and paging**

```csharp
var deals = await _unitOfWork.GetRepository<Deal>().GetListAsync(new ActiveDealsSpecification(),
                    q => q.OrderBy(m => m.DealName).Include(m => m.Company).Include(m => m.Team));
```

#### Extra Extensions
The library also provides extensions for Paging and Projections

```csharp
IQueryable<T> PagedBy<T>(this IQueryable<T> quaryable, PagingOptions options)

IQueryable<T> PagedBy<T>(this IQueryable<T> quaryable, int pageIndex, int pageSize)

IQueryable<TResult> ProjectTo<T, TResult>(this IQueryable<T> quaryable, Func<T, TResult> projection)
```

### DbContextBase
Using this class as a base class for DbContext will help you to set auditing information automatically.

In such scenarios that you would like to persist **LastModifiedDate** and **LastModifierBy**, You classes need to implement either of these two interfaces:
- IAuditable interface 
- Auditable abstract class

In both cases DbContextBase has a support for this and in each SaveChanges (or Async version) call it updates these fields automatically.

### Test Projects
To use the libary for test projects:

```csharp
services.AddInMemoryDbContext<DbContextClass>();

OR

services.AddInMemoryDbContext<DbContextClass>("in memory context name");
```

Also there are couple of extensions which helps to facilitate the unit testing. Here is the list:

#### Extensions

```csharp
TDbContext ToDbContext<T, TDbContext>(this IEnumerable<T> entities, Action<TDbContext> action = null)

IUnitOfWork<TDbContext> ToTypedUnitOfWork<T, TDbContext>(this IEnumerable<T> entities, Action<TDbContext> action = null) 
```

And this is a sample usage:
```csharp
[Fact]
public async Task Should_DealRequestListHandler_ReturnAllDealsInAListWhenCalled()
{
	// Given
	var unitOfWork = _fixture.Deals.ToTypedUnitOfWork<Deal, DealManagementDbContext>();
	var logger = Substitute.For<ILogger<DealListRequestHandler>>();
	var sut = new DealListRequestHandler(unitOfWork, _fixture.TypeMapper, logger);

	// When
	var result = (await sut.Handle(new DealListRequest(), CancellationToken.None)).ToList();

	// Then
	result.Should().NotBeNull().And.HaveCount(3);
}
```

Also there are some helper classes and methods which are facilitating the testing:
* TypeMapper
* FakeOperationContext



