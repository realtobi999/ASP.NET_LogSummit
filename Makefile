.PHONY: test

test:
	cd backend/LogSummitApi.Tests && dotnet test

build:
	cd backend && dotnet build