# Simple HTTP Server for Frontend
# This script starts a local HTTP server to serve the frontend files
# This helps avoid CORS issues when making API calls

Write-Host "Starting Frontend Server..." -ForegroundColor Green
Write-Host "API should be running on: http://localhost:5016" -ForegroundColor Yellow
Write-Host "Frontend will be available at: http://localhost:8080" -ForegroundColor Yellow
Write-Host ""
Write-Host "Press Ctrl+C to stop the server" -ForegroundColor Red
Write-Host ""

# Start Python HTTP server (if available)
try {
    python -m http.server 8080
} catch {
    # Fallback to PowerShell HTTP server
    Write-Host "Python not found, using PowerShell HTTP server..." -ForegroundColor Yellow
    
    $listener = New-Object System.Net.HttpListener
    $listener.Prefixes.Add("http://localhost:8080/")
    $listener.Start()
    
    Write-Host "Server started at http://localhost:8080" -ForegroundColor Green
    
    while ($listener.IsListening) {
        $context = $listener.GetContext()
        $request = $context.Request
        $response = $context.Response
        
        $localPath = $request.Url.LocalPath
        $filePath = Join-Path $PSScriptRoot $localPath.TrimStart('/')
        
        if ($localPath -eq "/" -or $localPath -eq "") {
            $filePath = Join-Path $PSScriptRoot "index.html"
        }
        
        if (Test-Path $filePath -PathType Leaf) {
            $content = Get-Content $filePath -Raw
            $buffer = [System.Text.Encoding]::UTF8.GetBytes($content)
            $response.ContentLength64 = $buffer.Length
            $response.OutputStream.Write($buffer, 0, $buffer.Length)
        } else {
            $response.StatusCode = 404
        }
        
        $response.Close()
    }
} 