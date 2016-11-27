![Beamgun Infographic](https://github.com/JLospinoso/beamgun/raw/master/Readme.png)

Installing Beamgun
==

Beamgun v0.2.0 is available 
[as an MSI installer](https://s3.amazonaws.com/net.lospi.beamgun/BeamgunInstaller-0.2.0.msi) 
and as a [portable .exe](https://s3.amazonaws.com/net.lospi.beamgun/BeamgunApp-0.2.0.zip). 
We recommend you use the MSI installer so that Beamgun restarts
on reboot.

Building from source
==

Simply clone the repository:
```sh
git clone git@github.com:JLospinoso/beamgun.git
```

Open `Beamgun.sln` and build. The installer can be found in the `bin` directory of the `BeamgunInstaller` project.

Read more
==

Check out [this blog post](https://jlospinoso.github.io/infosec/usb%20rubber%20ducky/c%23/clr/wpf/.net/security/2016/11/15/usb-rubber-ducky-defeat.html) for more details!

Version history
==

* [BeamgunInstaller-0.2.0.msi](https://s3.amazonaws.com/net.lospi.beamgun/BeamgunInstaller-0.2.0.msi) | [BeamgunApp-0.2.0.zip](https://s3.amazonaws.com/net.lospi.beamgun/BeamgunApp-0.2.0.zip): Major overhaul to alerting mechanism, reimplemented using WMI. Added USB storage disable. Added detection for LAN Turtles. Replaced autorun with Windows Task for elevation.

* [BeamgunInstaller-0.1.1.msi](https://s3.amazonaws.com/net.lospi.beamgun/BeamgunInstaller-0.1.1.msi) | [BeamgunApp-0.1.1.zip](https://s3.amazonaws.com/net.lospi.beamgun/BeamgunApp-0.1.1.zip): Fixes to focus stealing, cleaned up WIX installer

* [BeamgunInstaller-0.1.0.msi](https://s3.amazonaws.com/net.lospi.beamgun/BeamgunInstaller-0.1.0.msi) | [BeamgunApp-0.1.0.zip](https://s3.amazonaws.com/net.lospi.beamgun/BeamgunApp-0.1.0.zip): First version

Downloads (as of 11/27/2016)
==
* Beamgun 0.1.1: _108_ (10 MSI, 12 zip)
* Beamgun 0.1.0: _108_ (84 MSI, 26 zip)

Contribute
==

Please report any bugs you find (both feature- and security-related!) right
here on Github.