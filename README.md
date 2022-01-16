
## The Solution consists of these Projects

 #### 1. Component.Utilities
 > Class Library project (.net standard), it is a shared component, doesn't cover any business logic, practically it doesn't belong to any specific solution, it can be used in any other solution.
it provides the Error handling, Logging (NLog), CustomExceptions and Swagger features to any .net solution
 
#### 2. Component.Matching
> Class Library project (.net standard), it is used to provide matching services based on configurable matching and similarity rules
it has its own lookup database for matching purposes 

 #### 3. Test 
 > .net Core 5 NUnit test, contains the unit test project of Component.Matching project

#### 4. API.Test
 > .net Core 5 NUnit test, to test the api endpoints including middleware
 
#### 5. Api.Matching
> .net Core 5 RESTful API, it is the Startup project and the final user front API
>
> - Swagger URL : https://localhost:44326/swagger/index.html
The API can be tested using Swagger directly without needing to use postman or fiddler 
 
> - Matching two persons Endpoint : https://localhost:44326/api/v1/persons/Match , POST Request
```Request Body Example
{
  "first": {
  "firstName": "Andy",
  "lastName": "Crao",
  "dateOfBirth": "2021-12-19",
  "identificationNumber": "931212311"
},
  "second": {
  "firstName": "Andrew",
  "lastName": "Craw",
  "dateOfBirth": "2021-12-18",
  "identificationNumber": "931212312"
}
```
