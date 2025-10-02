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
├── ViewModels/
│   ├── BaseViewModel.cs
│   ├── LoginViewModel.cs
│   ├── PasswordManagerViewModel.cs
│   └── SessionViewModel.cs
│
├── Views/
│   ├── AppShell.xaml
│   ├── LoginPage.xaml
│   ├── PasswordManagerPage.xaml
│   └── SessionPage.xaml
│
├── App.xaml
├── GlobalXmlns.cs
├── MauiProgram.cs
└── README.md
```
