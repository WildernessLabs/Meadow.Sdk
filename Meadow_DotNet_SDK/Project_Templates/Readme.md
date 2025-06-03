# WildernessLabs.Meadow.Template

Contains a collection of Project Templates for the .NET IoT platform, Meadow.

Meadow is a complete, IoT platform with defense-grade security that runs full .NET applications on embeddable microcontrollers and Linux single-board computers including Raspberry Pi and NVIDIA Jetson.

For more information, visit [developer.wildernesslabs.co](http://developer.wildernesslabs.co/).

To view all Wilderness Labs open-source projects, including samples, visit [github.com/wildernesslabs](https://github.com/wildernesslabs/).

## Installation

You can install Meadow project templates with the command:

`dotnet new install WildernessLabs.Meadow.Template`

Once installed, it will list all the project templates included:

```console
Template Name            Short Name          Language        Tags
-----------------------  ------------------  --------------  -------------------
Meadow Core-Compute App  meadow-ccm          [C#],F#,VB.NET  Meadow/Console
Meadow Desktop App       meadow-desktop      [C#]            Meadow/Console
Meadow F7 Feather App    meadow-feather      [C#],F#,VB.NET  Meadow/Console
Meadow Jetson Nano App   meadow-jetson-nano  [C#]            Meadow/Console
Meadow Library           meadow-library      [C#],F#,VB.NET  Meadow/Library
Meadow Project Lab App   meadow-project-lab  [C#]            Meadow/Console
Meadow Raspberry Pi App  meadow-rpi          [C#]            Meadow/Console
Meadow reTerminal App    meadow-reterminal   [C#]            Meadow/Console
Meadow StartKit App      meadow-startkit     [C#]            Meadow/App/StartKit
```

## Usage

Open Visual Studio 2022, click on File -> New -> Project, in the _Create a new Project_ window, search for Meadow, and you will see the list of Meadow templates avaiable you've just installed.

Alternatively, you can create a Meadow project via console using the _Short Name_ values on the list. For example:

```console
C:\Users\john>dotnet new meadow-feather --name Blinky
The template "Meadow F7 Feather App" was created successfully. 
```

Creates a Meadow application that runs on a Meadow Feather F7 board.

## How to Contribute

- **Found a bug?** [Report an issue](https://github.com/WildernessLabs/Meadow_Issues/issues)
- Have a **feature idea or enhancement request?** [Open a new feature request](https://github.com/WildernessLabs/Meadow_Issues/issues)

## Need Help?

If you have questions or need assistance, please join the Wilderness Labs [community on Slack](http://slackinvite.wildernesslabs.co/).