Cyotek.Data.Nbt Change Log
==========================

[3.0]
-----

Removed ITag.Value. Each Tag implementation has a strongly typed Value property without boxing.
Added ITag.SetValue and ITag.GetValue methods that allow access to a value when you don't know the concrete type - these method box.
Concrete tag classes are now sealed
Reworked TagFactory to try and avoid boxing

[2.1.0] - 2016-02-24
--------------------

### Added
* Added new constructor to `XmlTagWriter` allowing you to specify a `XmlWriter`. Useful for when calling `Write*` methods directly, without first using `WriteDocument`
* Added new constructor to `XmlTagReader` allowing you to specify a `XmlReader`. Useful for when calling `Read*` methods directly, without first using `ReadDocument`

### Fixed
* XML documents that either didn't include any whitespace between elements or that used self closing tags weren't loaded correctly

[2.0.0] - 2016-02-16
--------------------

This major update includes breaking changes, mostly around the `ITagReader` and `ITagWriter` interfaces and implementing classes. The `NbtDocument` class has been updated so that methods such as `Load` and `Save` now can accept a `Stream` rather than always being forced to use a file.

I've also changed the license to the [GNU Lesser General Public License, version 2.1](https://www.gnu.org/licenses/old-licenses/lgpl-2.1.html). Originally I said it was licensed under Creative Commons Share Alike to match the original code this library was derived from, but it was actually LGPL all along.


### Added
* Added overloads accepting `Stream` arguments to `LoadDocument`, `Load`, `Save`, `IsNbtDocument` and `GetDocumentFormat` methods
* When saving a document, you can now specify `Auto` for compression which essentially lets the serializer choose the compression (on for binary, off for XML)

### Changed
* The `ITagWriter` and `ITagReader` interfaces have also been intensively refactored to use separate enums for options and compression (see `NbtOptions` in the **Removed** section), and to use a source `Stream` rather than a filename, and to remove some very silly API designs
* The `Custom` value for the `NbtFormat` enum has been renamed to `Invalid` reflecting the fact that custom support is no longer supported natively
* Added (hopefully) correct detection of gzip or deflate streams, removing the awful practice of relying on exceptions to drop between the formats
* All interfaces, classes and enums which strictly deal with serialization or de-serialization have been moved to the `Serialization` namespace
* Methods which would previously accept null input, or missing files such as `IsNbtDocument`, `GetDocumentName` and `GetDocument` format now throw the appropriate `ArgumentNullException` or `FileNotFoundException` exceptions

### Removed
* Removed the `NbtOptions` enum, replacing it with `CompressionOption`, `WriteTagOption` and `ReadTagOption` which appropriate options. The `SingleUse` flag has been fully removed as it was a hack for XML support
* Removed the `ITagEditor` interface, the `TagEditorAttribute` class and all defined instances of `TagEditor`. The editor support doesn't really belong in the core library, but rather by the separate GUI library.
* Removed the `TagReader` and `TagWriter` classes as they were fairly pointless
* Removed the `ReaderType` and `WriterType` properties from `NbtDocument` along with constructors that accepted types, `NbtFormat` values, or file names.
* Removed the static `NbtDocument.DefaultFormat` property
* Removed the static `IsDeflateDocument`, `IsGzipDocument`, `IsXmlDocument` and `IsRawDocument` methods from `NbtDocument` - they never should have been public in the first place, and have been replaced with proper stream detection
* `NbtDocument` no longer supports custom serialization options

### Fixed
* The `ITagWriter` interface is now correctly public


1.0.4.5
-------

### Changed
* Reformatted using C# 6 syntax, and minor corrections to avoid double casts

### Fixed
* Added culture info to date conversions


1.0.4.4
-------

### Added
* Added `ToString` overrides to `TagCollection` and `TagDictionary`

### Fixed
* Saving an empty byte array didn't store the zero length size correctly, causing a crash when trying to reload the document. Documents affected by this bug are essentially corrupt and can't be loaded.


1.0.4.3
-------

### Fixed
* Removed `EnumExtensions` class and replaced all `HasFlag` calls with bitwise statements


1.0.4.2
-------

### Fixed
* Added missing `GetByteValue` to `TagCompound`


1.0.4.1
-------

### Fixed
* `NbtDocument.GetDocumentName` now automatically returns `false` if the format of the document is not XML or binary.
* Removed exception when lists have a type of 0, as although it makes sense as it's essentially an invalid tag, some Minecraft tags are using zero type lists.


1.0.4.0
-------

### Added
* Added tests for `Tag.ToValueString`
* `ICollectionTag` now explicitly inherits from `ITag` so you don't have to cast back to the former if you only have a reference to the latter.
* Added new `TagEventArgs` helper class

### Fixed
* `TagIntArray.ToValueString` and `TagByteArray.ToValueString` now return appropriate values instead of the .NET type name for their respective arrays.


1.0.3.6
-------

### Added
* Added `AddRange` method to `TagDictionary`
* Added `AddRange` and `Add(bool)` methods to `TagCollection`
* Added tests for `TagDictionary`, `TagCollection` and `TagEnd`, and added some other tests for classes without full coverage [still more to go]
* Added new `NbtOptions.HeaderOnly` flag, allowing the name of a tag to be read without loading the full body

### Changed
* `Tag.Value` is now virtual
* Setting the value of the `TagEnd` class now has no effect, and retrieving the value always returns `null`

### Fixed
* Calling `NbtDocument.GetDocumentName` no longer loads the entire NBT document to return a single string value
* `TagDictionary.Add(Guid)` and `TagCollection.Add(Guid)` now return `ITag` instead of `void`
* Removed `AddIfNotDefault` methods from `TagDictionary`
* `BinaryTagReader.Load` always passed in default options rather than whatever the caller had specified


1.0.3.5
-------

### Fixed
* Changed `TagEditor` attributes on tag classes to remove fixed version information


1.0.3.4
-------

### Changed
* Code refactoring according to Resharper suggestions

### Fixed
* Fixed corrupt document tree and crash when loading XML document with self closing tags


1.0.3.3
-------

### Added
* Added a number of `TagCollection.Add` overloads without the `name` parameter to make it somewhat easier to populate lists via code.
* Added a few more tests

### Fixed
* Fixed problems trying to load invalid documents with the `NbtDocument` class and having them misidentified as "custom" documents.
* Fixed an infinite loop when loading an XML file containing a list tag with no children

1.0.3.2 and earlier
-------------------
* Change history not available in this document
