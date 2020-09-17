The application layer forms the composition root that combines the domain and infrastructure layers into a functional whole.
It forms the entire application, with exception of the technical layer in which it is presented (e.g. Blazor, MVC REST API).

ApplicationServices are logical bundles of methods, each of which form a use case made available by the application.
Such use cases tend to be transactional.
(The presentation layer can expose such use cases to the front end, to an API, etc.)

ApplicationService methods are the ideal point for integration testing:
A use case makes perfect sense within the context of an application, and there are clear expectations.
Little or no mocking is required, since most dependencies can be tested right along.
SQLite allows most database interaction to be tested.

By testing at this level, we remain decoupled from the technology used to expose the application to the outside world, which may change over time.

This layer also features adapters, which are used to translate between domain entities and contract models.
Domain entities are not exposed to the outside. Doing so would make them very hard to change, but the domain model should be free to change.
Instead, contract models are exposed to the outside.
