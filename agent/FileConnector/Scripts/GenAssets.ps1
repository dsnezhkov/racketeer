function generateName($baseName,$count,$operation,$addKey){

    $list = @()
    $idx = 1;
    for ($idx; $idx -ne $count+1; $idx++)
    {
        $filePathObj = New-Object PSObject
        $filePathStr =  -join ($baseName, $idx, ".txt")
        $filePathObj | Add-Member Noteproperty FilePath $filePathStr
        $filePathObj | Add-Member Noteproperty Operation $operation
        if ($addKey){
            $keyGUID = New-Guid
            $filePathObj | Add-Member Noteproperty Key $keyGUID.Guid
        }
        $list = $list + $filePathObj
    }
    return $list
}

function outList($fileListObj){
    foreach ($filePath in $fileListObj){
        Write-Output $filePath
    }
}


$assets =
@(
    <# [pscustomobject]@{
        baseName="C:\Users\dev\Desktop\UnsafeFiles\file";
        count=100;
        operation=1;
        key=$true},

    [pscustomobject]@{
        baseName="C:\Users\dev\Desktop\UnsafeFiles\SubFiles\file";
        count=100;
        operation=1;
        key=$true},
   #>
    [pscustomobject]@{
        baseName="\share\UnsafeFiles\file";
        count=100;
        operation=1;
        key=$true},
    [pscustomobject]@{
        baseName="\share\UnsafeFiles\SubFiles\file";
        count=100;
        operation=1;
        key=$true} 
  
)




$files = @{}
foreach ($asset in $assets){

    $assetDirective = generateName $asset.baseName $asset.count $asset.operation $asset.key
    $files["Files"] = $files["Files"] + $assetDirective
}


$files | ConvertTo-Json 

