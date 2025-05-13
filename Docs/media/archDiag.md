classDiagram
%% Frontend Layers
class View["View Layer"] {
+Blazor Pages & Components
+Authentication UI
}

    class ViewModel["ViewModel Layer (Future)"] {
        +AuthViewModel
        +Form State & Validation
        +UI Logic
    }
    
    class FrontendService["Frontend Services"] {
        +AuthenticationService
        +Login()/Register()/Logout()
    }
    
    %% Backend Layers
    class ApiController["API Controllers"] {
        +AuthController
        +RESTful Endpoints
    }
    
    class BackendService["Backend Services"] {
        +AuthenticationService
        +UserService
        +PasswordValidator
    }
    
    class Repository["Repositories"] {
        +UserRepository
        +CRUD Operations
    }
    
    class DbContext["Data Context"] {
        +KizukuContext
        +Entity Framework Core
    }
    
    %% Shared Models
    class Entities["Domain Models"] {
        +User
        +Entity Properties
    }
    
    class RequestDTOs["Request DTOs"] {
        +LoginRequest
        +RegisterRequest
        +Input validation attributes
    }
    
    class ResponseDTOs["Response DTOs"] {
        +UserResponse
        +AuthResponse
        +Presentation-ready data
    }
    
    class Result["Result Pattern"] {
        +Success/Error handling
        +Typed responses
    }
    
    %% Architecture Flow
    View --> ViewModel : binds to (planned)
    ViewModel --> FrontendService : uses
    FrontendService --> ApiController : HTTP requests
    ApiController --> BackendService : calls
    BackendService --> Repository : uses
    Repository --> DbContext : queries
    DbContext --> Database[(SQLite)] : connects to
    
    %% Cross-Layer Dependencies
    ViewModel ..> RequestDTOs : creates/validates
    ViewModel ..> ResponseDTOs : consumes
    FrontendService ..> RequestDTOs : sends
    FrontendService ..> ResponseDTOs : receives
    ApiController ..> RequestDTOs : receives/validates
    ApiController ..> ResponseDTOs : returns
    BackendService ..> Entities : processes
    Repository ..> Entities : maps to/from
    BackendService ..> Result : returns
    Repository ..> Result : returns
    ApiController ..> Result : transforms to HTTP responses
