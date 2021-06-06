### Exchange API

#### Endpoint 1: /api/Currency/rate/{currency} [GET]
In {currency}, send USD or BRL to get the current exchange rate. When others, a 404 not found error will be returned.

#### Endpoint 2: /api/Currency/purchase [POST]
Send a userId, amount to purchase in ARS and the currency to exchange. If ok, a 200 status code will be returned. If you exceed your monthly limit, a 403 status code will be returned.

#### Some remarks
- I used MSSqlLocalDB and Entity Framework for persistence of Transactions. Migrations folder contains all of its creation.
- When running from Visual Studio, it opens swagger by now. Feel free to use any other API tester.
- There is a middleware for handling exceptions and logging.
- .NET Core 5.
- Tests only cover controller methods.

#### Answering to the questions in the challenge
- About User ID in input endpoint: it's a bad practice for RESTful. One of REST constraints is to make resources cacheable and chaching userID is dangerous for privacy. 
- About how to improve this: ideally, API should use authentication and authorization, with a login endpoint and user and password parameters, and API retrieving a web token to the user; then user should use this token in all transactions (JWTBearer scheme). Also, methods/classes should be decorated with [Authorize] attribute.