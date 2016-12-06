![Beamgun Infographic](https://github.com/JLospinoso/beamgun/raw/master/Readme.png)

Installing Beamgun
==

Beamgun v0.2.2 is available 
[as an MSI installer](https://s3.amazonaws.com/net.lospi.beamgun/BeamgunInstaller-0.2.2.msi) 
and as a [portable .exe](https://s3.amazonaws.com/net.lospi.beamgun/BeamgunApp-0.2.2.zip). 
We recommend you use the MSI installer so that Beamgun restarts
on reboot.

As of v0.2.2, binaries are now signed by "Open Source Developer, Joshua Alfred Lospinoso" with SHA1 Thumbprint 7a 54 58 8a b6 1e bc 30 4f 7f 78 93 38 87 e8 ad 31 10 72 c5.

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

Beamgun's homepage is [beamgun.io](http://beamgun.io/).

Version history
==
* [BeamgunInstaller-0.2.2.msi](https://s3.amazonaws.com/net.lospi.beamgun/BeamgunInstaller-0.2.2.msi) | [BeamgunApp-0.2.2.zip](https://s3.amazonaws.com/net.lospi.beamgun/BeamgunApp-0.2.2.zip): Fixes to registry access; graceful handling of cast exceptions.

* [BeamgunInstaller-0.2.1.msi](https://s3.amazonaws.com/net.lospi.beamgun/BeamgunInstaller-0.2.1.msi) | [BeamgunApp-0.2.1.zip](https://s3.amazonaws.com/net.lospi.beamgun/BeamgunApp-0.2.1.zip): Improvement to network adapter alerting; fixes issue where Windows re-enables some adapters if they are immediately disabled after insertion.

* [BeamgunInstaller-0.2.0.msi](https://s3.amazonaws.com/net.lospi.beamgun/BeamgunInstaller-0.2.0.msi) | [BeamgunApp-0.2.0.zip](https://s3.amazonaws.com/net.lospi.beamgun/BeamgunApp-0.2.0.zip): Major overhaul to alerting mechanism, reimplemented using WMI. Added USB storage disable. Added detection for LAN Turtles. Replaced autorun with Windows Task for elevation.

* [BeamgunInstaller-0.1.1.msi](https://s3.amazonaws.com/net.lospi.beamgun/BeamgunInstaller-0.1.1.msi) | [BeamgunApp-0.1.1.zip](https://s3.amazonaws.com/net.lospi.beamgun/BeamgunApp-0.1.1.zip): Fixes to focus stealing, cleaned up WIX installer

* [BeamgunInstaller-0.1.0.msi](https://s3.amazonaws.com/net.lospi.beamgun/BeamgunInstaller-0.1.0.msi) | [BeamgunApp-0.1.0.zip](https://s3.amazonaws.com/net.lospi.beamgun/BeamgunApp-0.1.0.zip): First version

_316 downloads (as of 12/6/2016)_

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
