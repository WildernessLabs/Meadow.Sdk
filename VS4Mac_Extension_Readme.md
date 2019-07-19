# Visual Studio for Mac Extension

This is the add-in for Visual Studio Mac, Xamarin Studio, and MonoDevelop that enables Meadow projects to be built and deployed on Mac and Linux. 

## Installation 

To install the add-in:

 1. Install the latest version of either Visual Studio for Mac, or [Xamarin Studio/MonoDevelop](http://www.monodevelop.com/download/)
 2. Install the add-in from the extensions/add-ins manager:
 3. Open VS Mac/Xamarin Studio and open the **Visual Studio/Xamarin Studio** > **Extensions/Add-ins** menu and select the **Gallery** tab.
 4. Select **All repositories** and Search for `Meadow` (you may have to click the **Refresh** button.
 5. Select the Meadow extension and then click **install** on the right.
 6. Follow installation instructions.


## Building the Add-in from Source

 1. Install the latest version of either Visual Studio for Mac, or [Xamarin Studio/MonoDevelop](http://www.monodevelop.com/download/)
 2. Install the **AddinMaker** maker from the extensions/add-ins manager (follow installation instructions 3-6 above, but search for `AddinMaker` instead).
 3. Clone project from GitHub and open the `VS4Mac_Meadow_Extension.sln` solution with VS Mac/Monodevelop.
 4. Make sure all the nuget packages are restored and then start debugging, which will open a new instance of VS Mac that has the Add-in that just built enabled.

### Creating an Add-in Package

To create an addin package (`.mpack`), first, build the add-in in release mode, and then run the `CreateAddinPackage.sh` script from terminal.

#### Publishing

When publishing, make sure to bump the version number in the `/Properties/AddinInfo.cs` file, as well as the `AssemblyInfo.cs` file.

## Authors

Bryan Costanich