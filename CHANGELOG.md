# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## 1.4.0 - 2021-09-22
### Added
- FIT files support.
- Photo backup option.
- Double click opens the picture.
- Detecting more photo timestamps from Exif and file names.

### Changed
- Speed improvement (about 500%).
- Some libraries has been updated.
- Writing a timestamp to Exif has been removed.

## 1.3.0 - 2020-02-09
### Added
- Support photos without Exif (required timestamp in the file name).
Examples of the valid file names:
  - 20200209_124658.jpg,
  - IMG_20200209_124658.jpg,
  - IMG_20200209_124658_edited.jpg.

## 1.2.0 - 2018-12-31
### Added
- Extract date and time from filename if DateTimeOriginal Exif tag does't exists.
- Add DateTimeOriginal Exif tag when above condition is true.

## 1.1.0 - 2018-10-03
### Added
- Editable margin of error.
- Editable time offset.
- Auto selecting picture folder from GPX file path.

### Changed
- Small code refactoring.

## 1.0.1 - 2018-08-31
### Added
- This CHANGELOG file.

### Fixed
- Fix "File is locked" errors.
- Fix "File not found" errors.

## 1.0.0 - 2018-07-07
- First release.