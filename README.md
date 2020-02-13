This repository has been **archived** and merged into **[CSF.ORM](https://github.com/csf-dev/CSF.ORM)**.  Further development on this work can be found there.

*It was found that a number of repositories were so closely linked that they should be converted to a single repository, even though they are multiple separate NuGet packages.*

---

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

## Open source license
All source files within this project are released as open source software,
under the terms of [the MIT license].

[the MIT license]: http://opensource.org/licenses/MIT

This software is distributed in the hope that it will be useful, but please
remember that:

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
