$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Definition
rm -r ${scriptDir}\Generated
dotnet run --project ..\Ivy.MdToIvy\Ivy.MdToIvy.csproj -- convert ${scriptDir}\Docs\*.md ${scriptDir}\Generated