Imports AnonFileAPI

Module ModuleMain
    Sub Main()
        Using anonFileWrapper As AnonFileWrapper = New AnonFileWrapper()
            Dim anonFile As AnonFile = anonFileWrapper.UploadFile($"C:\Users\Gecco\Desktop\Test.png")
            Console.WriteLine("### Error Code: {0}", anonFile.ErrorCode)
            Console.WriteLine("### Error Message: {0}", anonFile.ErrorMessage)
            Console.WriteLine("### Error Type: {0}", anonFile.ErrorType)
            Console.WriteLine("### Full URL: {0}", anonFile.FullUrl)
            Console.WriteLine("### Status: {0}", anonFile.Status)
            Console.WriteLine("### Download URL: {0}", anonFileWrapper.GetDirectDownloadLinkFromLink(anonFile.FullUrl))
        End Using
    End Sub
End Module
