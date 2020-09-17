This project features the domain model, which is the representation of the business domain (the reality), intended to get things done there.
The domain model is the heart of the solution.

Most commonly found are domain objects: the domain entities and value objects that encapsulate the domain's data, behavior, and rules.

Behavior that cannot logically be made part of any domain object is encapsulated in a domain service, although this is avoided when possible.

Repository abstractions are often part of the domain as well. For example, IOrderRepo represents a place where Orders are stored.
The domain experts tend to recognize the existence of such things.
