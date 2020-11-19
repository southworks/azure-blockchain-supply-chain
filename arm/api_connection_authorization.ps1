
Param(
    [Parameter(Mandatory=$true)]
    [string] $ResourceGroupName,

    [Parameter(Mandatory=$true)]
    [string] $ConnectionName
)

Function Show-OAuthWindow {
    Add-Type -AssemblyName System.Windows.Forms
 
    $form = New-Object -TypeName System.Windows.Forms.Form -Property @{Width=600;Height=800}
    $web  = New-Object -TypeName System.Windows.Forms.WebBrowser -Property @{Width=580;Height=780;Url=($url -f ($Scope -join "%20")) }
    $DocComp  = {
            $Global:uri = $web.Url.AbsoluteUri
            if ($Global:Uri -match "error=[^&]*|code=[^&]*") {$form.Close() }
    }
    $web.ScriptErrorsSuppressed = $true
    $web.Add_DocumentCompleted($DocComp)
    $form.Controls.Add($web)
    $form.Add_Shown({$form.Activate()})
    $form.ShowDialog() | Out-Null
    }

#login to get an access code 
az login

# get the connection
$connection = az resource show -g $ResourceGroupName --resource-type "Microsoft.Web/connections" --name $ConnectionName

#get the links needed for consent
$ListLinksCmdOut = az resource invoke-action --action "listConsentLinks" --resource-group $ResourceGroupName --resource-type "Microsoft.Web/connections" --name $ConnectionName  --request-body "{ \`"parameters\`": [{\`"parameterName\`": \`"token\`", \`"redirectUrl\`": \`"https://ema1.exp.azure.com/ema/default/authredirect\`" }]}"
$consentResponseJSON = -Join $ListLinksCmdOut
$consentResponse  = ConvertFrom-Json -InputObject $consentResponseJSON
$url  = $consentResponse.Value.Link 

#prompt user to login and grab the code after auth
Show-OAuthWindow -URL $url

$regex = '(code=)(.*)$'
    $code  = ($uri | Select-string -pattern $regex).Matches[0].Groups[2].Value
    Write-output "Received an accessCode: $code"

if (-Not [string]::IsNullOrEmpty($code)) {
	$parameters = @{}
	$parameters.Add("code", $code)

    #confirm the consent code
	 az resource invoke-action --action "confirmConsentCode" --resource-group $ResourceGroupName --resource-type "Microsoft.Web/connections" --name $ConnectionName  --request-body "{\`"code\`":\`"$code\`"}"
}
