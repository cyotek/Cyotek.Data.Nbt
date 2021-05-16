# Adding a new tag type

[Rough notes to remind me how to add new tag types to the
library given how T4 templates are used with abandon]

TODO: Writing these notes remind me how messy this is. Consider
refactoring all these arrays to some sort of object. Or maybe
use source generators instead of T4. There's also a fair chunk
of boilerplate that could be automated

TODO: The templates only support sequential tag numbers, e.g. 1,
2, 3, 4... it doesn't support adding something custom, e.g. 1,
2, 100.

## Updating NbtGeneratorData.t4

* Add a new constant representing the tag type for referencing
  elsewhere, e.g. `const int tagTypeLongArray = 12;` (optional)
* Add a new entry to the `nbtTypeNames` array containing the tag
  name, e.g. `LongArray`. This value will be used for method
  names so keep to normal constraints
* Add a new entry to the `nbtTagTypeNames` array containing the
  name of new class, e.g. `TagLongArray`
* Add a new entry to the `netTypeNames` array containing the
  type of the data hosted by the tag, e.g. `long[]`
* Add a new entry to the `defaultValues` array containing the
  expected default value for the tag, e.g. `new long[0]`.
  Currently only used by tests.
* Add a new entry to the `objectConversions` array containing
  the code required to convert or cast an object to the tag
  value, e.g. `(long[])value`
* Add a new entry to the `nbtTypeDescriptions` array containing
  a description of the tag, e.g. `An array of 64bit integers of
  unspecified format`
* Add a new entry to the `tagTestData` array containing the code
  to initialise test data, e.g. `new[] { long.MinValue / 2,
  2994, long.MaxValue / 2, 4294394 }`
* Add a new entry to the `altTagTestData` array containing the
  code to initialise alternate test data, e.g. `new[]
  {long.MinValue / 3, 2994, long.MaxValue / 3, 4294394 }`
* Add a new entry to the `tagListTestData` array containing the
  code to initialise test data for use with lists of the new
  tag, e.g. `new long[][] {new [] { long.MinValue / 2, 2994 },
  new [] { long.MaxValue / 2, 4294394 } }`
* If the new type is an array type, update the `IsArrayType`
  function accordingly

Execute **Transform All Templates** after making these changes
to cause all the auto-generated C# files to be rebuild.

## Creating the tag class

Add a concrete implementation of the tag class. It should
inherit from `Tag` and implement `IEquatable<<TagName>>`. For
array types, consider adding a T4 template which imports
`TagIntArray.generated.tt` to get the bulk of functionality.
(TODO: Add one for the non-array classes)

At a bare minimum, the new class needs to

* Add four constructors, default, name, name and value, value
* Override the `TagType` property to return the appropriate
  value
* Override the `GetValue` and `SetValue` methods used to set
  untyped value
* Override `ToValueString` to return a string version of the
  value
* Provide a typed `Value` property
* Override `GetHashCode` and calculate a hash based on the name
  and typed value
* Implement `Equals(TagClass)`

## Adding Typed Helpers

* Add methods `TagCompound.Get<TypeName>Value` that returns the
  tag value. One should accept just a name, the other a name and
  default value. For example, `long[] GetLongArrayValue(string
  name)` and `long[] GetLongArrayValue(string name, long[]
  defaultValue)`  (TODO: Auto generate)
* Add method `TagCompound.Get<TypeName`> that returns the tag
  class, e.g. `TagLong GetLong(string name)` (TODO: Auto
  generate)
* Updated `TagCollection.Add(string,object)` to support new tag
  (TODO: Auto generate)
* Updated `TagDictionary.Add(string,object)` to support new tag
  (TODO: Auto generate)

## Updating Binary Serialisation

* Update `BinaryTagReader.ReadTag` to include a new case for the
  tag (TODO: Auto generate?)
* Also update `BinaryTagReader.ReadTag` to change the maximum
  type check at the top of the method (TODO: Auto generate?)
* Update `BinaryTagReader.ReadListTag` to include a new case for
  the tag (TODO: Auto generate?, consolidate why both `ReadTag`
  and `ReadListTag` are duplicating all this)
* Also update `BinaryTagReader.ReadList` to change the maximum
  type check at the top of the method (TODO: Auto generate?)
* Add an implementation of `ReadBinaryTagReader.<TagName>`
* Update `XmlTagReader.ReadTag` to include a new case for the
  tag (TODO: Auto generate?)
* Add an implementation of
  `BinaryTagWriter.WriteValue<TagValue>`

## Updating XML Serialisation

* Add an implementation of `Read<TagName>` to `XmlTagReader`
* Add an implementation of `WriteValue<TagValue>` to
  `XnkTagWriter`

## Adding tests

* Add a new `partial` class inheriting from `TestBase`
* Add any bespoke tests to this class
* Add a new T4 file which imports from `TagTests.t4` and which
  will include common tests for constructors, value
  manipulation, equality checks, hash codes, etc, etc
