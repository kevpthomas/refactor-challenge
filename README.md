The attached project is a poorly written products API.

Your job, should you choose to accept it, is to make changes to this project to make it better. Simple. There are no rules, changes are not limited to pure refactors.

There is no time limit (we all work at different speed!), but as a guideline, we recommend spending between 2-4 hours on the exercise. 

Please consider all aspects of good software engineering (including but not limited to design, reliability, readability, extensibility, quality) and show us how you'll make it #beautiful.

Once completed, send back your solution in a zip file (source code only please to keep the zip small) and include a new README describing the improvements you have made and the rationale behind those decisions. 

Good luck!


## Improvement Updates (28/01/2019)
The Xero refactor assessment feedback raised the following:
* No service layer added
* Did not add a global exception handler
* Did not use async methods in API controllers
* Used inheritance in models instead of composition (minor)

All of these are valid points. The short answer is I decided to focus initially on overall architecture and security vulnerabilities as
my top priorities, and then reassess after completing those changes. I decided to stop refactoring after I finished those items due to
time constraints.

I have now done a bit more refactoring, see below for more details.

### Did not use async methods in API controllers
I refactored the ProductsController endpoints to be async, and added async methods to the repositories. I retained the synchronous repository
methods to follow standard .NET library practices to have both sync and async methods. I also refactored the unit tests so that I have examples
of tests over async methods.

Note that the NPoco library has a bug when inserting new records using a pre-generated GUID primary key, thereby requiring an Insert
method overload requesting the tableName, primaryKeyName, an autoIncrement flag, and the poco instance. There is no equivalent asynchronous
method on the NPoco IDatabase definition. I overcame this issue using a 
[Dataflow (Task Parallel Library)](https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/dataflow-task-parallel-library) Action Block.

## Improvements
I decided to take a comprehensive approach to improving the supplied code base, because this is for a senior developer position. Therefore,
I evaluated the overall architecture as well as the code, and made fairly comprehensive changes to the original code for the products API.

One of the first changes I made was to introduce Swagger (via the Swashbuckle package) to allow easy manual interaction with the API endpoints.
The GetAll and SearchByName methods violated Swagger's endpoint conventions, so I refactored SearchByName to have a different route. See below
for more details about API improvements.

Also, I standardised all of the namespaces to be RefactorThis to match the project names. ReSharper suggested the change and provided an easy
command to safely perform this refactoring step.

### Architecture improvements
The original architecture had tight coupling between the application's core logic from the services it uses. This tight coupling makes it hard
to maintain the products API, because it is hard to replace service frameworks without breaking references.

I implemented the [Clean Architecture approach advocated by Steve Smith](https://devintxcontent.blob.core.windows.net/showcontent/Speaker%20Presentations%20Fall%202017/Clean%20Architecture%20with%20ASP.NET%20Core.pdf).
This clean architecture decouples the application's core logic from the services it uses, and is informed by:
* [Posts-And-Adapters architecture](http://www.dossier-andreas.net/software_architecture/ports_and_adapters.html)
* [The Onion Architecture](https://jeffreypalermo.com/2008/07/the-onion-architecture-part-1/)

Admittedly, this architectural overhaul is a bit of overkill for such a simple project, but I thought it would be fun to demonstrate the benefits
of such an approach. 

Note: ideally, there should be no direct project reference from the Web project (RefactorThis) to the Infrastructure project. This would prevent
a developer from accidentally referencing Infrastructure in Web (all references should be to Core instead). However, I felt it was out of scope
to attempt to solve dynamic loading of the RefactorThis.Infrastructure library.

To support the new Clean Architecture:
* Added the **TinyIoC** package to provide basic IoC functionality. The IoC injects the correct dependencies from Infrastructure, allowing Web to reference 
  the abstractions and models defined in Core.
* Added the **AutoMapper** package to simplify mapping between API models defined in Web and Entity models defined in Core.
* Shifted the connection string from the **Helpers** class to **web.config**. This allows greater maintainability because web.config can be 
  transformed when deployed to different environments.

### API and Database Access improvements
The original design had Products, Product, ProductOptions, and ProductOption entity models. These models served multiple roles:
* API DTO models
* Database entity models
* Repository (CRUD) functionality

These models violated the Single Responsibility Principle and also did not support Domain Driven Design. They forced a tight coupling between
the core logic and CRUD operations performed via SqlClient operations. They limited the ability to validate API requests or database operations.
The database operations used dynamically generated SQL without parameters, thereby exposing a SQL Injection vulnerability. SqlClient class instances
(e.g., Connections and SqlCommands) were not disposed of properly.

The API endpoints did not always make use of the requested Id values. For example, UpdateOption did not use the supplied productId, because the underlying
ProductOption class methods did not support a ProductId. This could lead to confusion for developers and possibly the introduction of logic errors.
Some API endpoints requested duplicate values. For example, CreateOption required a productId in the URL and in the supplied ProductOption. However
there was no check to ensure matching values, and a better design would be to remove duplication, thereby simplifying interactions with the API's.

The API endpoints did not perform any [input validation](https://www.owasp.org/index.php/Input_Validation_Cheat_Sheet), and passed values 
directly into non-parameterised SQL statements. As noted above, this provided a significant security risk from 
[SQL Injection](https://www.owasp.org/index.php/Top_10-2017_A1-Injection). Exceptions were not sanitised in the API responses, 
thereby creating [Sensitive Data Exposure vulnerabilities](https://www.owasp.org/index.php/Top_10-2017_A3-Sensitive_Data_Exposure). For example,
when I posted a duplicate Product Id, the response included sensitive implementation details:

```
{
  "Message": "An error has occurred.",
  "ExceptionMessage": "Violation of PRIMARY KEY constraint 'PK__Product__3214EC073E1E30BC'. Cannot insert duplicate key in object 'dbo.Product'. The duplicate key value is (9f2e9176-35ee-4f0a-ae55-83023d2db1a4).\r\nThe statement has been terminated.",
  "ExceptionType": "System.Data.SqlClient.SqlException",
  "StackTrace": "   at System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)\r\n   at System.Data.SqlClient.SqlInternalConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)\r\n   at System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)\r\n   at System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)\r\n   at System.Data.SqlClient.SqlCommand.RunExecuteNonQueryTds(String methodName, Boolean async, Int32 timeout, Boolean asyncWrite)\r\n   at System.Data.SqlClient.SqlCommand.InternalExecuteNonQuery(TaskCompletionSource`1 completion, String methodName, Boolean sendToPipe, Int32 timeout, Boolean& usedCache, Boolean asyncWrite, Boolean inRetry)\r\n   at System.Data.SqlClient.SqlCommand.ExecuteNonQuery()\r\n   at RefactorThis.Models.Product.Save() in C:\\Dev\\Sandbox\\refactor-challenge\\RefactorThis\\Models\\Products.cs:line 86\r\n   at RefactorThis.Controllers.ProductsController.Create(Product product) in C:\\Dev\\Sandbox\\refactor-challenge\\RefactorThis\\Controllers\\ProductsController.cs:line 54\r\n   at lambda_method(Closure , Object , Object[] )\r\n   at System.Web.Http.Controllers.ReflectedHttpActionDescriptor.ActionExecutor.<>c__DisplayClass6_1.<GetExecutor>b__0(Object instance, Object[] methodParameters)\r\n   at System.Web.Http.Controllers.ReflectedHttpActionDescriptor.ActionExecutor.Execute(Object instance, Object[] arguments)\r\n   at System.Web.Http.Controllers.ReflectedHttpActionDescriptor.ExecuteAsync(HttpControllerContext controllerContext, IDictionary`2 arguments, CancellationToken cancellationToken)\r\n--- End of stack trace from previous location where exception was thrown ---\r\n   at System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)\r\n   at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)\r\n   at System.Web.Http.Controllers.ApiControllerActionInvoker.<InvokeActionAsyncCore>d__1.MoveNext()\r\n--- End of stack trace from previous location where exception was thrown ---\r\n   at System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)\r\n   at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)\r\n   at System.Web.Http.Controllers.ActionFilterResult.<ExecuteAsync>d__5.MoveNext()\r\n--- End of stack trace from previous location where exception was thrown ---\r\n   at System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)\r\n   at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)\r\n   at System.Web.Http.Dispatcher.HttpControllerDispatcher.<SendAsync>d__15.MoveNext()"
}
```

The API PUT and POST endpoints did not return the created/modified entity, which reduces the usefulness of the API's. Also Microsoft recommends
returning an **IHttpActionResult** for all REST operations as of Web API 2 - see the 
[Action Results documentation](https://docs.microsoft.com/en-us/aspnet/web-api/overview/getting-started-with-aspnet-web-api/action-results).

Lastly, the tightly-coupled design made the existing code essentially untestable by unit tests. It's critical to left-shift testing effort to 
unit tests as much as possible for performance and maintenance reasons (unit tests run much faster than web/functional tests).

I resolved all of these issues by introducing the Repository pattern, creating separate DTO and entity models, and refactoring the ProductsController
methods for consistency and better security. I implemented the **NPoco** lightweight
ORM in my repository implementations. Key changes:
* Parameterised SQL to prevent SQL injection attacks
* Simple validations added to DTOs (e.g., string length limit of 100 on Name to match the database column definition)
* Repositories and models follow the Single Responsibility Principle
* Loosely-coupled design, enabled by IoC constructor injection of dependencies, supports unit testing
* Clean Architecture allows Repository implementation to change to another ORM with minimal refactoring of the Web project
* All endpoints return **IHttpActionResult**
* I retained the original design that allows POST methods to set the Id GUID value. Depending on the business requirements, it might be better
  to change the design so that those Id values are generated internally for newly-created entities.

I refactored the ProductsController following the guidelines set forth in Martin Fowler's [Branch by Abstraction](https://martinfowler.com/bliki/BranchByAbstraction.html)
approach. I first introducted an abstraction layer containing the original Product/ProductOptions code, and then gradually refactored to my
new Repository architecture. This allowed me to safely make incremental changes and verify with manual testing via Swagger.

### Unit Testing
I added a unit testing library with a few sample unit tests to show how the new loosely-coupled architecture supports easy unit testing.
It was out of scope to thoroughly test all applicable controller and repository methods. I favour the following libraries:
* NUnit for test composition and running
* Shouldly for assertions
* Moq and AutoMock for mocking
* Bogus to create fake random values


## Instructions

To set up the project:

* Open project in VS.
* Restore nuget packages and rebuild.
* Run the project.

There should be these endpoints:

1. `GET /products` - gets all products.
2. `GET /products?name={name}` - finds all products matching the specified name.
3. `GET /products/{id}` - gets the project that matches the specified ID - ID is a GUID.
4. `POST /products` - creates a new product.
5. `PUT /products/{id}` - updates a product.
6. `DELETE /products/{id}` - deletes a product and its options.
7. `GET /products/{id}/options` - finds all options for a specified product.
8. `GET /products/{id}/options/{optionId}` - finds the specified product option for the specified product.
9. `POST /products/{id}/options` - adds a new product option to the specified product.
10. `PUT /products/{id}/options/{optionId}` - updates the specified product option.
11. `DELETE /products/{id}/options/{optionId}` - deletes the specified product option.

All models are specified in the `/Models` folder, but should conform to:

**Product:**
```
{
  "Id": "01234567-89ab-cdef-0123-456789abcdef",
  "Name": "Product name",
  "Description": "Product description",
  "Price": 123.45,
  "DeliveryPrice": 12.34
}
```

**Products:**
```
{
  "Items": [
    {
      // product
    },
    {
      // product
    }
  ]
}
```

**Product Option:**
```
{
  "Id": "01234567-89ab-cdef-0123-456789abcdef",
  "Name": "Product name",
  "Description": "Product description"
}
```

**Product Options:**
```
{
  "Items": [
    {
      // product option
    },
    {
      // product option
    }
  ]
}
```
