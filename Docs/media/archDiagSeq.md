sequenceDiagram
participant User
participant View (Blazor UI)
participant ViewModel (planned)
participant Frontend Service
participant API Controller
participant Backend Service
participant Repository
participant Data Context (EF Core)

    User->>View (Blazor UI): Initiates action (e.g., login)
    View (Blazor UI)->>ViewModel (planned): Updates UI state, triggers logic
    ViewModel (planned)->>Frontend Service: Calls API (e.g., login)
    Frontend Service->>API Controller: Sends HTTP request
    API Controller->>Backend Service: Forwards request DTO
    Backend Service->>Repository: Business logic, queries data
    Repository->>Data Context (EF Core): Database access
    Data Context (EF Core)-->>Repository: Returns data
    Repository-->>Backend Service: Returns entity
    Backend Service-->>API Controller: Returns Result/DTO
    API Controller-->>Frontend Service: Returns Response DTO
    Frontend Service-->>ViewModel (planned): Updates state
    ViewModel (planned)-->>View (Blazor UI): Updates UI
