.PHONY: test

test:
	clear && cd backend/LogSummitApi.Tests && dotnet test

build:
	cd backend && dotnet build

run:
	cd backend/LogSummitApi.Presentation && dotnet run
