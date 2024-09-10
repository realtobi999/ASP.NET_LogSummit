.PHONY: test

test_backend:
	clear && cd backend/LogSummitApi.Tests && dotnet test

build_backend:
	cd backend && dotnet build

run_backend:
	cd backend/LogSummitApi.Presentation && dotnet run

run_frontend:
	cd frontend && npm install && npm run dev
