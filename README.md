## Introduction
You were hired as a consultant for BestBlogs<sup>TM</sup> company. The company 
needs your help with finishing the implementation of a REST api for its newest blog product.
Your client has already written some tests and implemented some parts of the .NET service. You were tasked to finish 
the implementation and make all tests pass. Follow this README to complete the code in a correct order. Good luck!


### Persistence layer
Implement the empty methods in [src/Repository/CommentRepository.cs](src/Repository/CommentRepository.cs) and [src/Repository/PostRepository.cs](src/Repository/CommentRepository.cs).
You should use the in memory database, in [src/Repository/BlogContext.cs](src/Repository/BlogContext.cs), to persist the data.

- `CommentRepository.GetAll` - finds all existing comments
- `CommentRepository.Get` - find the comment by id
- `CommentRepository.Create` - inserts a given comment
- `CommentRepository.Update` - updates a given comment
- `CommentRepository.Delete` - deletes a given comment
- `CommentRepository.GetByPostId` - finds all comments by post id


- `PostRepository.GetAll` - finds all existing posts
- `PostRepository.Get` - finds the post by id
- `PostRepository.Create` - inserts a given post
- `PostRepository.Update` - updates a given comment
- `PostRepository.Delete` - deletes a given comment

### Web layer
Implement a web layer for the blog API. This step involves implementing REST API controllers in [src/Api/Controllers](src/Api/Controllers) folder.

**Comment endpoints**

- GET `/comments` - returns all comments
- GET `/comments/{id}` - returns comment by id
- POST `/comments` - creates a new comment
- PUT `/comments` - updates a comment
- DELETE `/comments/{id}` - deletes a comment by id

**Post endpoints**

- GET `/posts` - returns all posts
- GET `/posts/{id}` - returns post by id
- POST `/posts` - creates a new post
- PUT `/posts` - updates a post
- DELETE `/posts/{id}` - deletes a post by id
- GET `/posts/{id}/comments` - returns all comments for a given post id

#### Request Validations
Implement the following request validations:

**Comment**

- `PostId` must be an existing post id
- `Content` should not have more than 120 characters
- `Author` should not have more than 30 characters

**Post**

- `Title` should not have more than 30 characters
- `Content` should not have more than 1200 characters

### Tests
Add your tests to [test](test) folder. 
There's a sample test in [test/Api.Tests/CommentControllerTests.cs](test/Api.Tests/CommentControllerTests.cs).

### Extra
If you feel like this is to easy, we have some extra challenges you might want to try:

1. Make use of custom exceptions to handle known errors within your code
2. Make the data persisted, instead of in-memory
3. Add documentation to your API (e.g. /swagger/v1/swagger.json)
4. "Containerize" your application (e.g. Dockerfile)

## Uploading
If you struggle at any time when uploading your solution, you can upload to a private GitHub repository and we will review it.

## Good luck!
