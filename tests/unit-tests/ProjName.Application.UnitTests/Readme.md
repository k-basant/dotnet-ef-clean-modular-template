**Conventions**

- DB queries should not be mocked. We'll test against a physical test db.
- Any other infrastructure layer services would/should be mocked.
- NSubstitute for mocking.
- Test method naming convention - `{MethodName}_{WhatIsExpected}_{WhenItIsExpected}`

- Generating Test Coverage Report - 
	XML Report
	`coverlet Application.UnitTests.dll --target "dotnet" --targetargs "test ../../../ --no-build" --format cobertura --include "[Application]*" --include "[Application.*]*" --exclude "[Application.*]*.CQRS.*" --exclude "[Application.*]*.Models.*" --exclude "[Application.*]*.QueryExtensions" --exclude "[Application]Random""`
	[NOTE: In the above command, last arg was added because last one doesn't gets applied (may be bug in the tool).]

	HTML from XML Report
	`reportgenerator -reports:"{Path_To_XML_Report}" -targetdir:"coveragereport" -reporttypes:Html`
	For e.g.:
	`reportgenerator -reports:"D:\Work\Office\thinkbridge\Projects\thinkstack\dotnet-project-templates\net6-clean-arch\Code\tests\unit-tests\Application.UnitTests\bin\Debug\net6.0\coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html`