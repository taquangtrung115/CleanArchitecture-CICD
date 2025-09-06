#############################################################
#############################################################
#############################################################
#"C:\WWW\DemoCICD\BE\PROD"
#"C:\WWW\DemoCICD\BE\PROD_BACKUP"
# run ".\rollback.ps1 [FOLDER_BACKUP]"

$Source = $args[0]
$SourceCode = '.\'+$Source+'\*'
$Destination = '.\..\PROD'
$WebSiteName = 'democicd.prod.com'
$AppPoolName = 'democicd.prod.com'

powershell Import-Module WebAdministration

function Main {
	if ($Source -eq $null)
	{
		Write-Host "You must provide source folder which is need to backup from."
	}
	else
	{
		Write-Host "Backup from " $Source " to destination " $Destination
		
		try {
			
			StopIIS
			
			Backup
			
			StartIIS
		
			Write-Host "Backup from " $Source "has been completed!"
		}
		catch {
			Write-Host "An error occurred when backup from " $Source "!"
		}
		
	}
}

# Stop IIS APP POOL
function StopIIS {
	
	$WebSiteState = Get-WebsiteState -Name $WebSiteName
	if ($WebSiteState.Value -eq "Started")
	{
		Stop-Website -Name $WebSiteName
		Write-Output "Website $($WebSiteName) stopped."
	}
	else
	{
		Write-Output "Website $($WebSiteName) has already been stopped."
	}
	
	$AppPoolState = Get-WebAppPoolState -Name $AppPoolName
	if ($AppPoolState.Value -eq "Started")
	{
		Stop-WebAppPool -Name $AppPoolName
		Write-Output "WebAppPool $($AppPoolName) stopped."
	}
	else
	{
		Write-Output "WebAppPool $($AppPoolName) has already been stopped."
	}
	
	iisreset
	
	#Write-Output "Waiting until service stopped"
	#ping google.com /n 5
}

function StartIIS {
	# Start IIS APP POOL
	$AppPoolState = Get-WebAppPoolState -Name $AppPoolName
	if ($AppPoolState.Value -eq "Stopped")
	{
		Start-WebAppPool -Name $AppPoolName
		Write-Output "WebAppPool $($AppPoolName) started."
	}
	else
	{
		Write-Output "WebAppPool $($AppPoolName) has already been started."
	}
	
	$WebSiteState = Get-WebsiteState -Name $WebSiteName
	if ($WebSiteState.Value -eq "Stopped")
	{
		Start-Website -Name $WebSiteName
		Write-Output "Website $($WebSiteName) started."
	}
	else
	{
		Write-Output "Website $($WebSiteName) has already been started."
	}
	
	iisreset
}

# Backup
function Backup {
	Copy-Item -Path $SourceCode -Destination $Destination
}

Main


