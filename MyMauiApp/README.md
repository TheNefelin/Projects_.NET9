# My Maui App .NET 9

## Structure
```
MyMauiApp/
│
├── Properties/
│   └── launchSettings.json
│
├── Converters/
│   ├── InvertedBoolConverter.cs
│   └── IsNotNullOrEmptyConverter.cs
│
├── Extensions/
│   └── HttpResponseExtensions.cs
│
├── Models/
│   ├── ApiResponse.cs
│   ├── AuthUserLogged.cs
│   └── AuthUserLogin.cs
│
├── Platforms/
│   └── ...
│
├── Resources/
│   └── ...
│
├── Services/
│   ├── IAuthService.cs
│   └── AuthService.cs
│
├── Settings/
│   ├── AppSettings.cs
│   └── AppSettingsTemplate.cs
│
├── ViewModels/
│   ├── BaseViewModel.cs
│   ├── LoginViewModel.cs
│   ├── PasswordManagerViewModel.cs
│   └── SessionViewModel.cs
│
├── Views/
│   ├── AppShell.xaml
│   ├── CreateEditPasswordPage.xaml
│   ├── LoginPage.xaml
│   ├── PasswordManagerPage.xaml
│   ├── PasswordPromptPage.xaml
│   └── SessionPage.xaml
│
├── App.xaml
├── GlobalXmlns.cs
├── MauiProgram.cs
└── README.md
```
