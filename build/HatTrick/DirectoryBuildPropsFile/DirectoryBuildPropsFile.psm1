using module ..\AssemblyVersion\AssemblyVersion.psm1
Set-StrictMode -Version Latest

class DirectoryBuildPropsFile
{
    [ValidateNotNullOrEmpty()][string]$OutputPath
    [AssemblyVersion]$AssemblyVersion
    [boolean]$IncludeBuildNumberPartsInPackageVersion

    DirectoryBuildPropsFile(
        [string]$OutputPath,
        [AssemblyVersion]$AssemblyVersion,
        [boolean]$IncludeBuildNumberPartsInPackageVersion
    )
    {
        $this.OutputPath = $OutputPath
        $this.AssemblyVersion = $AssemblyVersion
        $this.IncludeBuildNumberPartsInPackageVersion = $IncludeBuildNumberPartsInPackageVersion
    }
	
	[void] ReplaceVersionPrefixInDirectoryBuildPropsFile()
    {
		$path = Resolve-Path -Path $this.OutputPath
		$xml = New-Object Xml
		$xml.Load($path)
		
		$propertyGroupNode = $xml.SelectSingleNode("/Project/PropertyGroup")
		$versionPrefixNode = $propertyGroupNode.SelectSingleNode("VersionPrefix")
		if ($versionPrefixNode -ne $null)
		{
			$propertyGroupNode.RemoveChild($versionPrefixNode)
		}
		$versionNode = $xml.CreateElement("Version")
		$packageVersionNode = $xml.CreateElement("PackageVersion")
		$informationalVersionNode = $xml.CreateElement("InformationalVersion")

		# set the version to the AssemblyVersion
		$versionNode.InnerText = $this.AssemblyVersion.AssemblyVersion
		
		# construct a new version for packaging
		$version = "{0}.{1}.{2}" -f $this.AssemblyVersion.Major, $this.AssemblyVersion.Minor, $this.AssemblyVersion.Patch
        if ($this.IncludeBuildNumberPartsInPackageVersion)
        {
            $version = "{0}-v{1}-{2}" -f $version, $this.AssemblyVersion.Build, $this.AssemblyVersion.Revision
            if (![string]::IsNullOrWhiteSpace($this.AssemblyVersion.Suffix))
			{	
				$version = "{0}-{1}" -f $version, $this.AssemblyVersion.Suffix
			}
        }
        elseif (![string]::IsNullOrWhiteSpace($this.AssemblyVersion.Suffix))
		{
			$version = "{0}-{1}" -f $version, $this.AssemblyVersion.Suffix
		} 
		$packageVersionNode.InnerText = $version
		$informationalVersionNode.InnerText = $version
		
		$propertyGroupNode.AppendChild($versionNode)
		$propertyGroupNode.AppendChild($packageVersionNode)
		$propertyGroupNode.AppendChild($informationalVersionNode)
		
		$xml.Save($path)
    }
}

function New-DirectoryBuildPropsFile()
{
    param
    (
        [string]$OutputPath,
        [AssemblyVersion]$AssemblyVersion,
        [boolean]$IncludeBuildNumberPartsInPackageVersion
    )

    return [DirectoryBuildPropsFile]::new($OutputPath, $AssemblyVersion, $IncludeBuildNumberPartsInPackageVersion)
}

Export-ModuleMember -Function New-DirectoryBuildPropsFile