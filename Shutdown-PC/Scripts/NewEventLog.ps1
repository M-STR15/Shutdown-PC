$nameSource="ShutdownPC"

if (-not [System.Diagnostics.EventLog]::SourceExists($nameSource)) {
    New-EventLog -LogName Application -Source "ShutdownPC"
     Write-Host "Event source $nameSource byl vytovřen."
} 
else {
    Write-Host "Event source $nameSource již existuje."
}