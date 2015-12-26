$execFile = ".\Konnie.Application\bin\Debug\Konnie.exe"
$params = "file1.konnie","file2.konnie","konnieTask"

# Wait until the started process has finished
& $execFile $params
if (-not $?)
{
    # Show error message
}