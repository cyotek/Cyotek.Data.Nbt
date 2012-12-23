# Cyotek.Data.Nbt Change Log

## 1.0.3.3
### Changes and new features
* Added a number of `TagCollection.Add` overloads without the `name` parameter to make it somewhat easier to populate lists via code.
* Added a few more tests

### Bug Fixes
* Fixed problems trying to load invalid documents with the `NbtDocument` class and having them misidentified as "custom" documents.
* Fixed an infinite loop when loading an XML file containing a list tag with no children

## 1.0.3.2 and below
* Change history not available in this document