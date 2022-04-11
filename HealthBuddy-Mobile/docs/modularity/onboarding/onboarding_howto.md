# How to create and register your own step

## Create your own module
First of all you need to create module. You can place it in your own feature folder (if you have one) or in **Covi/OnBoarding/Modules** and **Covi/Steps**.

+ Covi
    + Features
        + MyOwnFeature
            + Modules
            + OnBoardingStep

**Covi/MyOwnFeature/Modules/MyOwnFeatureModule.cs**

```cs
    public class MyOwnFeatureModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var service = containerProvider.Resolve<IOnBoardingSetupService>();

            service?.RegisterStep(containerProvider.Resolve<MyOwnFeatureOnBoardingStep>);
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<MyOwnFeatureOnBoardingStep>();
        }
    }
```

Module registers itself in the IOnBoardingSetupService, but you should register it in **Covi/App.xaml.cs**. Go to `ConfigureOnBoarding` method and add you own module here in order to embed it in onboarding process. Be aware that steps appears in the order you define.

```cs
moduleCatalog.AddModule<Features.MyOwnFeature.OnBoardingStep.PushNotificationOnBoardingModule>(InitializationMode.WhenAvailable);
```

## Create you own step

**Covi/MyOwnFeature/OnBoardingStep/MyOwnFeatureOnBoardingStep.cs**

```cs
    public class MyOwnFeatureOnBoardingStep : IOnBoardingStep
    {
        private const string MyOwnFeatureStepImage = "my_own_feature_step.svg";

        public MyOwnFeatureOnBoardingStep()
        {
            Title = Localization.OnBoarding_MyOwnFeatureStep_Title.ToUpper();
            SubTitle = Localization.OnBoardind_MyOwnFeatureStep_SubTitle;
            IconCode = MyOwnFeatureStepImage;
            Instructions = new List<InstructionItem>
            {
                new InstructionItem(Localization.OnBoarding_MyOwnFeatureStep_Instruction1),
                new InstructionItem(Localization.OnBoarding_MyOwnFeatureStep_Instruction2),
                new InstructionItem(Localization.OnBoarding_MyOwnFeatureStep_Instruction3)
            };
        }

        // Ste ptitle
        public string Title { get; }
        
        // Step subtitle
        public string SubTitle { get; }
        
        // Step icon to display
        public string IconCode { get; }
        
        // Error message to display in case of any errors of ruther processing
        public string ErrorMessage { get; }
        
        // List of instructiosn to display
        public IReadOnlyList<InstructionItem> Instructions { get; }

        // Here you define the method that decides what to do when user presses the "next" button.
        // If this method returns false - the onbording wont proceed to the next step. So in this case the step is becoming mandatory.
        // If the step is not mandatory - simply return true.
        public Task<bool> HandleStepAsync()
        {
            return Task.FromResult(true);
        }
    }
```

## Handle step Next button action and make it mandatory

In the `HandleStepAsync` method you are able to do any interaction with the user (e.g. permission request) and based on the answer (or other logic) allow (return true) or deny (return false) user to go further.

For example you may ask user to provide bluetooth permission for contact tracing. Every time the user taps 'Next' button on the screen - The `HandleStepAsync` will be called and check if bluetooth permission is granted. As the bluetooth is mandatory, it will not proceed unless permission is granted. However, if the bluetooth is not available on the device (e.g. advertising) - the step can prohibit to proceed.

If your step is not 'mandatory' - just return true in any case.