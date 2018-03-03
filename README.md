![Beamgun Infographic](https://github.com/JLospinoso/beamgun/raw/master/Readme.png)

Installing Beamgun
==

Beamgun v0.2.4 is available
[as an MSI installer](https://s3.amazonaws.com/net.lospi.beamgun/BeamgunInstaller-0.2.4.msi)
and as a [portable .exe](https://s3.amazonaws.com/net.lospi.beamgun/BeamgunApp-0.2.4.zip).
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

Check out these two blog posts for more information:

* [Original](https://jlospinoso.github.io/infosec/usb%20rubber%20ducky/c%23/clr/wpf/.net/security/2016/11/15/usb-rubber-ducky-defeat.html)

* [Update](https://jlospinoso.github.io/infosec/usb%20rubber%20ducky/lan%20turtle/c%23/clr/wpf/.net/security/2016/11/30/beamgun-update-poison-tap.html)

Beamgun's homepage is [jlospinoso.github.io/beamgun/](https://jlospinoso.github.io/beamgun).

Notes
==
Beamgun will run with low-user and elevated privileges (i.e. as administrator), but it will ask for the highest privileges that the logged in user has. When running without administrator privileges, you will be unable to (a) disable network adapters, and (b) disable USB mass storage. This is a feature of Windows security, not a design choice! Thanks to @AlexIljin [for pointing this out](https://github.com/JLospinoso/beamgun/issues/7).

If a network adapter has already been installed on your computer, Beamgun will not alert on its insertion. This has to do with the way Beamgun registers with Windows Management Instrumentation for alerts; it only subscribes to notifications of new `Win32_NetworkAdapters`. When an already-installed network adapter is inserted, it generates a `Win32_PnPEntity` instance (which Beamgun doesn't currently subscribe to). The upshot of this is, when testing Beamgun, you'll need to uninstall the network adapter you are testing in between tests. From a user perspective, this should be expected behavior; if I've already permitted a particular network adapter once, it's probably not a rogue adapter!

Version history
==
* [BeamgunInstaller-0.2.4.msi](https://s3.amazonaws.com/net.lospi.beamgun/BeamgunInstaller-0.2.4.msi) | [BeamgunApp-0.2.4.zip](https://s3.amazonaws.com/net.lospi.beamgun/BeamgunApp-0.2.4.zip): Fixing bug with portable .exe not starting under certain circumstances when registry root key doesn't exist.

* [BeamgunInstaller-0.2.3.msi](https://s3.amazonaws.com/net.lospi.beamgun/BeamgunInstaller-0.2.3.msi) | [BeamgunApp-0.2.3.zip](https://s3.amazonaws.com/net.lospi.beamgun/BeamgunApp-0.2.3.zip): Removing steal focus option. Fixed bugs when disabling. Async-ified version checking.

* [BeamgunInstaller-0.2.2.msi](https://s3.amazonaws.com/net.lospi.beamgun/BeamgunInstaller-0.2.2.msi) | [BeamgunApp-0.2.2.zip](https://s3.amazonaws.com/net.lospi.beamgun/BeamgunApp-0.2.2.zip): Fixes to registry access; graceful handling of cast exceptions.

* [BeamgunInstaller-0.2.1.msi](https://s3.amazonaws.com/net.lospi.beamgun/BeamgunInstaller-0.2.1.msi) | [BeamgunApp-0.2.1.zip](https://s3.amazonaws.com/net.lospi.beamgun/BeamgunApp-0.2.1.zip): Improvement to network adapter alerting; fixes issue where Windows re-enables some adapters if they are immediately disabled after insertion.

* [BeamgunInstaller-0.2.0.msi](https://s3.amazonaws.com/net.lospi.beamgun/BeamgunInstaller-0.2.0.msi) | [BeamgunApp-0.2.0.zip](https://s3.amazonaws.com/net.lospi.beamgun/BeamgunApp-0.2.0.zip): Major overhaul to alerting mechanism, reimplemented using WMI. Added USB storage disable. Added detection for LAN Turtles. Replaced autorun with Windows Task for elevation.

* [BeamgunInstaller-0.1.1.msi](https://s3.amazonaws.com/net.lospi.beamgun/BeamgunInstaller-0.1.1.msi) | [BeamgunApp-0.1.1.zip](https://s3.amazonaws.com/net.lospi.beamgun/BeamgunApp-0.1.1.zip): Fixes to focus stealing, cleaned up WIX installer

* [BeamgunInstaller-0.1.0.msi](https://s3.amazonaws.com/net.lospi.beamgun/BeamgunInstaller-0.1.0.msi) | [BeamgunApp-0.1.0.zip](https://s3.amazonaws.com/net.lospi.beamgun/BeamgunApp-0.1.0.zip): First version

_456 downloads (as of 12/10/2018)_

Press
==
* [Security Now! Episode 589: Q&A 244](https://www.grc.com/securitynow.htm) [Show notes here.](https://www.grc.com/sn/SN-589-Notes.pdf)
* [ISC StormCast for Friday, December 2nd 2016](https://isc.sans.edu/podcastdetail.html)
* [Information Security News, Northwestern University](https://www.youtube.com/watch?v=Jb2dK8j94UI&feature=youtu.be)
* [Sans Newsbites, Volume XVIII, Issue #95](https://www.sans.org/newsletters/newsbites/xviii/95?utm_medium=Social&utm_source=Twitter&utm_content=SM_NB_xviii_95&utm_campaign=Newbites)

Contribute
==

Please report any bugs you find (both feature- and security-related!) right
here on Github.
