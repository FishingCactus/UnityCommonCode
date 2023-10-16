# Changelog

## [1.0.16] - 2023-10-16
### Added
- Add CustomPropertyDrawer attribute to EnumDictionary's property drawer

## [1.0.15] - 2023-10-04
### Added
- Add NetworkStatusChecker to check for Internet availability

## [1.0.14] - 2023-10-03
### Updated
- Add missing namespace around SerializableDictionary class.

## [1.0.13] - 2023-09-13
### Updated
- Singleton: add option for destroy on load.

## [1.0.12] - 2023-07-12
### Added
- Add EnumHelper GetToggledFlagsCount & Clamp IComparable functions.

## [1.0.11] - 2023-07-07
### Added
- Add mathmod function.

## [1.0.10] - 2023-06-14
### Updated
- CreateNewPrefab tools : folder selection added, last used folder path stored in EditorPlayerPrefs.

## [1.0.9] - 2023-05-11
### Added
- SerializableDictionary: add explicit implementation of IReadOnlyDictionary

## [1.0.8] - 2023-05-09
### Added
- Add implicit operator to create an enabled Optional<T> from a T value

## [1.0.7] - 2023-04-26
### Added
- Add method to EnumHelper to get a random value in a specific range

## [1.0.6] - 2023-03-10
### Fix
- Fix EnumDictionary foldout & position.

## [1.0.5] - 2023-02-23
### Fix
- Remove Task delegate because useless and was preventing System.Threading.Tasks.Task to be used in FishingCactus without specifying the whole path.

## [1.0.4] - 2022-01-27
### Added
- Add Max attribute for integer and float fields and Optional helper struct

## [1.0.3] - 2022-08-09
### Fix
- Fix SerializableDictionary setter bug for existing keys

## [1.0.2] - 2022-05-25
### Added
- Add WizzardUtils.DisplayPathSelector().

## [1.0.1] - 2022-05-25
### Added
- Add SerializableDictionary utility.

## [1.0.0] - 2022-03-31
### Added
- First version of the package.