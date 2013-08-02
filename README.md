PK.Deployment
=============
Custom scripts and tasks for MSDeploy
***
MsDeploy:
-------------------
* Added option to select `ParametersXMLFiles` as a build action so that this does not has to be done by editing the project file directly

Features:
---------
* `MsDeployAdditionalManifestFiles`:

	Selecting this build action for a file in Visual Studio will copy the file to the location where an MsDeploy package (WebPublishMethod=Package) will be created. This means it will not be part of the content of the package but will be placed next to the package zip file and/or manifest file. This can be used to copy custom deploy scripts or parameter files to the package folder.