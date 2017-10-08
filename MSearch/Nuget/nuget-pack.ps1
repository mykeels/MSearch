# A powershell script for packing dotnet-standard projects into nuget packages
[string]$versionNo = ""

if (@(gci *.txt).Count -eq 0) {
	[System.IO.File]::WriteAllText("version-no.txt", "1.0.0")
}

$versionNo = Get-Content "version-no.txt"

Write-Host "Old Version: $versionNo"

if (![System.Text.RegularExpressions.Regex]::IsMatch($versionNo, "\d+\.\d+\.\d+")) {
	# Check that the version number in the file follows the 1.0.0 format
	 Write-Host ("Invalid Version Number") -foreground "yellow"
}

$version1 = [int]($versionNo.Split('.')[0])
$version2 = [int]($versionNo.Split('.')[1])
$version3 = [int]($versionNo.Split('.')[2])


$version3++

Write-Host "New Version: $($version1).$($version2).$($version3)"

if ($version3 > 1000) {
	$version3 = 0;
	$version2++;
	if ($version2 > 1000) {
		$version2 = 0;
		$version++;
	}
}

$versionNo = "$($version1).$($version2).$($version3)"

# Write-Host "Version No: $($versionNo)"

cd..

dotnet pack -o "nuget" /p:Version="$($versionNo)"

[System.IO.File]::WriteAllText("version-no.txt", $versionNo)