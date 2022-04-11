# Text resources

## About

This document contains information about the text resources in the application: rules of their location, and how they are supposed to be created.

## Modularity

Since this application follows feature-based approach, default solution with the single text resource file (which supposed to contain all of the text in the applicaiton) will not be possible to support if the application follows the approach of complete project separation of the features. For this case each feature has its own resource file `Resources/Localization.resx`. Resource file with the generic texts is located in the `Resources` folder in the core of the **Covi** project.

If you need to create new feature, create the text resource file in the `Resources` folder with the enabled code generation (to have `Localization.Designer.cs` connected with the default resource file).

## Language

Default text resource language is English: all text in the `Resources/Localization.resx` files should be only in English.

You can read the instructions about text localization [here](localization.md).
