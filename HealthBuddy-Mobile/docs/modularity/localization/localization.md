# Text localization

## About

This document contains information about enabling different languages in the application.

## Infrastructure

As it is decsribed in [this document](resources.md), default text resource files are located in the `Resources/Localization.resx` feature folders. .NET supports localization by the ability to use multiple resource files with the language code postfix. The list of these postfixes should be used in the same way as it is described in ISO (e.g. [here](http://www.lingoes.net/en/translator/langcode.htm)). For instance, if you put `Localization.ru.resx` in the same folder as the original `Localization.resx`, it will be referenced by the system if the current language of the application is set to the Russian.

## How to add localization resources manually

1. To localize some specific feature you should copy the existing resource file `Resources/Localization.resx` of this feature and use it as basic template for localization
2. Replace all text lines in English which you want to be localized in the required language. It is important to retain the `name` attributes of each xml node and alter only the text itself.
3. After translation completion rename the copied file to have the corresponding to the language postfix. That means that file's name will follow the pattern `Localization.{language_code}.resx`. Then you should put it in the same location as the original text resource file (`Resources/Localization.resx`). This file can be added in the `Resources` folder as is, withoud meddling with the **Covi.csproj** file.
4. As soon as all of the features that were needed to translate into required language will have the localized resource files, you should find application configuration file (`Resources/app.config` for **Covi.iOS**; `Assets/app.config` for **Covi.Android**).
5. One of the configuration entries will have the localization information:

```xml
    <!--Lists supported localization by the culture code, each code separated by comma -->
    <add key="LocalizationConstants_SupportedLocales" value="en-US,ru" />
```

6. Add the required locale/language code in the `value`, separating it from the others by '`,`' symbol in both configuration files.

After these steps the added localization should be present on the language selection page of the application.

## Localization scripts

These scripts provide an automated way to aggregate feature localization resource files into a single bundle for further easier localization using 3rd party tools.
The process is split in the following scripts for each step:

1. init_meta.py

This script goes throe all resource files and prepares a metadata json file, which references every localization file in the solution.

Example: `init_meta.py ../src/ ./results`

Example of a result file:
```javascript
{
    "localeName": "ru", // Locale for which the locale is being created
    "items": {
        "AboutUs": "Covi\\Features\\AboutUs\\Resources\\Localization.ru.resx", // feature name (based on a folder name in which resources folder is being placed)
        "Account": "Covi\\Features\\Account\\Resources\\Localization.ru.resx", // path to a resource file
    }
}
```

If some features should be ignored - simply remove them from metadata file.

2. add_language.py

This script creates a localization files for provided localization based on the resources described in a metadata file

Example: `add_locale.py ../ ./default.json it`

After adding a new locale, to create a metadata file - simply call init_meta.py again.

3. bundle_locale.py

This script goes throe files in metadata file and aggregates them into one single resource file, prefixing each entry with the name of the feature it belongs to.

Example: `bundle_locale.py ../src/ ./results/default.json ./results/"`

The resutl file can be uploaded to any other 3rd party service for localization or converted in any other format. However, the keys should stay unchanged.

4. unbundle_locale.py

This script goes throe a merged file and splits it into feature based resoruce files based on the data from metadata file.

Example: `unbundle_locale.py ../src/  results/Localization.resx results/default.json`