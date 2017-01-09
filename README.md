# Entity base types
This library provides a number of base types and interfaces useful when working with ORMs and mapped entities.
In particular, it introduces the concept of an 'identity', separated from the primitive values which make up that identity.
For example:

```csharp
public class Person : Entity<long>
{
  public virtual string Name { get; set; }
  
  public virtual void SetIdentity(long id)
  {
    // The IdentityValue property has protected visibility
    // You may choose how (or if) you expose it
    base.IdentityValue = id;
  }
}

// And now in a code block elsewhere:
var person = new Person() {
  Name = "Joe Bloggs"
};
person.SetIdentity(5);

// An IIdentity<Person> is a representation of the
// identity (primary key value) for a Person instance.
// It is not tied to the 'long' data-type though; the
// rest of your application need not know the data-type
// of Person's primary key.
IIdentity<Person> = person.GetIdentity();
```

## Entity inheritance
Represent inheritance (and allow equality comparisons across inherited identities) by marking them with an attribute:

```csharp
public class Person : Entity<long> { /* Implementation omitted */ }

[BaseType(typeof(Person))]
public class Employee : Person { /* Implementation omitted */ }

public class Animal : Entity<long> { /* Implementation omitted */ }

// Then in a code block elsewhere:
Person myPerson = GetMyPerson();
Employee myEmployee = GetMyEmployee();

// If the two objects have the same identity value then this will be true.
// This is because they share the same base entity type.
bool personAndEmployeeAreSame = myPerson.IdentityEquals(myEmployee);

// Compared with:
Animal myAnimal = GetMyAnimal();
Employee myEmployee = GetMyEmployee();

// This will NEVER be true, regardless of the identity values.
bool animalAndEmployeeAreSame = myAnimal.IdentityEquals(myEmployee);

```