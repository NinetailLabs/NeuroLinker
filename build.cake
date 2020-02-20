#region ScriptImports

// Scripts
#load "CakeScripts/base/base.buildsystem.cake"
#load "CakeScripts/base/base.variables.cake"
#load "CakeScripts/base/base.setup.cake"
#load "CakeScripts/base/base.nuget.restore.cake"
#load "CakeScripts/base/base.msbuild.cake"
#load "CakeScripts/base/base.altcover.cake"
#load "CakeScripts/base/base.coveralls.upload.cake"
#load "CakeScripts/base/base.gitreleasenotes.cake"
#load "CakeScripts/base/base.nuget.pack.cake"
#load "CakeScripts/base/base.nuget.push.cake"
#load "CakeScripts/base/base.docfx.cake"

#endregion

#region Tasks

// Set up variables specific for the project
Task ("VariableSetup")
	.Does(() => {
		projectName = "NeuroLinker";
		releaseFolderString = "./{0}/bin/{1}/netstandard2.0";
		releaseBinaryType = "dll";
		repoOwner = "NinetailLabs";
		botName = "NinetailLabsBot";
		botEmail = "gitbot@ninetaillabs.com";
		botToken = EnvironmentVariable("BotToken");
		gitRepo = string.Format("https://github.com/{0}/{1}.git", repoOwner, projectName);
	});

Task ("GrabMsdnPackage")
	.Does(() => {
		DownloadFile("https://www.nuget.org/api/v2/package/msdn.4.5.2/0.1.0-alpha-1611021200", $"./tools/MsdnDocs.nupkg");
		Unzip("./tools/MsdnDocs.nupkg", "./tools/MsdnDocs");
	});

Task ("Default")
	.IsDependentOn ("DiscoverBuildDetails")
	.IsDependentOn ("OutputVariables")
	.IsDependentOn ("LocateFiles")
	.IsDependentOn ("VariableSetup")
	.IsDependentOn ("GrabMsdnPackage")
	.IsDependentOn ("NugetRestore")
	.IsDependentOn ("Build")
	.IsDependentOn ("UnitTests")
	.IsDependentOn ("CoverageUpload")
	.IsDependentOn ("GenerateReleaseNotes")
	.IsDependentOn ("NugetPack")
	.IsDependentOn ("NugetPush")
	.IsDependentOn ("Documentation")
	.IsDependentOn ("FailBuildIfTestFailed");

#endregion

#region RunTarget

RunTarget (target);

#endregion