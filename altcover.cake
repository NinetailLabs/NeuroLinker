/*
 * Execute NUnit tests.
 * Script uses OpenCover to calculate coverage results
 */

#region Tools

#addin nuget:?package=Cake.Coverlet&version=2.4.2
#tool nuget:?package=NUnit.ConsoleRunner&version=3.10.0

#endregion

#region Variables

// Indicate if the unit tests passed
var testPassed = false;
// Path where coverage results should be saved
var coverPath = "./coverageResults.xml";
// Test result output file
var testResultFile = "./TestResult.xml";
// Filter used to locate unit test dlls
var unitTestFilter = "./*Tests/*.Tests.csproj";

#endregion

#region Tasks

// Execute unit tests
Task ("UnitTests")
    .Does (() =>
    {
        var blockText = "Unit Tests";
        StartBlock(blockText);

        RemoveCoverageResults();
        ExecuteUnitTests();
        PushTestResults(testResultFile);

        EndBlock(blockText);
    });

Task ("FailBuildIfTestFailed")
    .Does(() => {
        var blockText = "Build Success Check";
        StartBlock(blockText);

        if(!testPassed)
        {
            throw new CakeException("Unit test have failed - Failing the build");
        }

        EndBlock(blockText);
    });

#endregion

#region Private Methods

// Delete the coverage results if it already exists
private void RemoveCoverageResults()
{
    if(FileExists(coverPath))
    {
        Information("Clearing existing coverage results");
        DeleteFile(coverPath);
    }
}

// Execute NUnit tests
private void ExecuteUnitTests()
{
    testPassed = true;

    var testAssemblies = GetFiles(unitTestFilter);

    foreach(var assembly in testAssemblies)
    {
        try
        {
            var coverOutput = MakeAbsolute(File(coverPath));

            Information($"Testing: {assembly}");

            var testSettings = new DotNetCoreTestSettings {
			Configuration = buildConfiguration,
			NoBuild = true,
            ArgumentCustomization = args=> args
                .Append("/p:AltCover=true")
                .Append($"/p:AltCoverXmlReport={coverOutput}")
		};

            DotNetCoreTest(assembly.ToString(), testSettings);
        }
        catch(Exception ex)
        {
            testPassed = false;
            Error(ex.Message);
            Error("There was an error while executing tests");
        }
    }
}

#endregion