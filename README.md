# City-Searching-Assignment

A straightforward search API service built with **.NET 8**. This project focuses on delivering core search functionality using standard .NET libraries to ensure code readability and ease of maintenance.

## 📋 Runtime Prerequisites

Before you begin, ensure you have met the following requirements:

* **.NET SDK**: Version 8.0

## 🚀 How to Build and Run

1.  **run project:**
    Just open solution "City-Searching-Assignment.sln" via visual studio and run the project.
    The API will be available at `http://localhost:7072` (or as configured in `launchSettings.json`).

2.  **Test the endpoint:**
    You can test the search functionality using Swagger (if enabled) or via curl/Postman.

---

## 💡 Key Design Decisions and Trade-offs

### 1. In-Memory Data Loading (JSON to Object)
* **Decision:** The application reads the entire JSON dataset once at startup and converts it into in-memory collections (e.g., `List<T>` or `Dictionary`).
* **Trade-off:**
    * *Pros:* **Maximized Read Performance**. Once loaded, data retrieval is instantaneous because it eliminates expensive File I/O operations during search requests.
    * *Cons:* **Higher Memory Footprint**. This approach requires enough RAM to hold the entire dataset. While excellent for small-to-medium datasets, it would need a database or streaming approach for massive datasets (GBs).

### 2. Repository Pattern (Data Access Layer)
* **Decision:** Decoupled the data access logic into a dedicated Repository layer.
* **Trade-off:**
    * *Pros:* **Flexibility & Maintainability**. It abstracts the data source from the business logic. This allows swapping the underlying storage mechanism (e.g., switching from a JSON file to a SQL Database) in the future without breaking the API controllers.
    * *Cons:* **Added Complexity**. Introduces an extra layer of abstraction which increases the project structure complexity compared to accessing data directly in the controller.

### 3. Data Transfer Objects (DTO) Pattern
* **Decision:** Implemented DTOs to decouple the internal data models (deserialized from JSON) from the external API responses.
* **Trade-off:**
    * *Pros:* **Security & Contract Stability**. Prevents exposing internal implementation details or unnecessary fields to the client. It ensures the API response format remains consistent even if the underlying data structure changes.
    * *Cons:* **Boilerplate Code**. Requires creating additional classes and writing mapping logic (converting raw data to DTOs) compared to returning the raw object directly.

### 4. Usage of Local Functions
* **Decision:** Utilized C# **Local Functions** (nested functions) to encapsulate helper logic within the scope where it is needed.
* **Trade-off:**
    * *Pros:* **Better Encapsulation & Readability**. Keeps the main class clean by not exposing private helper methods that are only used in one place. It also allows access to variables in the containing scope (closures) without passing them as parameters.
    * *Cons:* **Testability**. Local functions cannot be unit-tested in isolation; they must be tested through the parent method.

---

## 🔮 What I Would Improve (Future Work)

1.  **Pagination (Limit & Offset):**
    * Currently, the API returns all matching results. I would implement pagination parameters (e.g., `?page=1&pageSize=20`) to limit the data sent over the network. This reduces payload size and prevents client-side performance issues when the result set is large.

2.  **Observability (Structured Logging):**
    * Integrate a library like **Serilog** to enable structured logging (JSON). This allows for better log aggregation and analysis (e.g., tracking "most searched keywords" or "slow queries") in tools like ELK Stack or Seq.

3.  **Containerization:**
    * Add **Docker** support to ensure the application runs consistently across development, testing, and production environments.