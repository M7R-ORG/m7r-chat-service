set -euo pipefail

dotnet build

dotnet format

dotnet test
