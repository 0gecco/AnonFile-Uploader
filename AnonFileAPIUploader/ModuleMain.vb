Imports AnonFileAPI

Module ModuleMain
    Sub Main(ByVal args As String())
        Dim targetFile As String
        If args.Length > 0 Then
            targetFile = args(0)
            Console.Title = "VB.NET AnonFile Uploader [© Gecco 2020] v1.0.11.1"
            Console.ForegroundColor = ConsoleColor.Red
            Console.Clear()
            Console.WriteLine("[ WARN ] Are you sure you want to upload the selected file? Y/N." & vbCrLf & "[ INFO ] Selected file, " & targetFile)
            Dim readKey1 As ConsoleKey = Console.ReadKey().Key
            If readKey1 = ConsoleKey.N Then
                Environment.Exit(0)
            ElseIf readKey1 = ConsoleKey.Y Then
                Console.ForegroundColor = ConsoleColor.Green
                Console.WriteLine(vbCrLf)
                Using anonFileWrapper As AnonFileWrapper = New AnonFileWrapper()
                    Dim anonFile As AnonFile = anonFileWrapper.UploadFile($"" & targetFile)
                    If anonFile.ErrorMessage IsNot Nothing Then
                        Console.WriteLine("[ ERROR ] Error code: {0}", anonFile.ErrorCode)
                        Console.WriteLine("[ ERROR ] Error message: {0}", anonFile.ErrorMessage)
                        Console.WriteLine("[ ERROR ] Error type: {0}", anonFile.ErrorType)
                    Else
                        Console.WriteLine("[ INFO ] URL: {0}", anonFile.FullUrl)
                        Console.WriteLine("[ INFO ] Done uploading: {0}", anonFile.Status)
                        Console.WriteLine("[ INFO ] Direct download link/URL: {0}", anonFileWrapper.GetDirectDownloadLinkFromLink(anonFile.FullUrl))

                        Console.WriteLine("[ INFO ] Press ENTER key to copy the normal download URL or press the SPACEBAR to copy the direct download URL." & vbCrLf & "[ WARN ] Application will exit after this task.")
                        Dim readKey2 As ConsoleKey = Console.ReadKey().Key
                        If readKey2 = ConsoleKey.Enter Then
                            My.Computer.Clipboard.SetText(anonFile.FullUrl)
                            Threading.Thread.Sleep(100)
                            Environment.Exit(0)
                        ElseIf readKey2 = ConsoleKey.Spacebar Then
                            My.Computer.Clipboard.SetText(anonFileWrapper.GetDirectDownloadLinkFromLink(anonFile.FullUrl))
                            Threading.Thread.Sleep(100)
                            Environment.Exit(0)
                        End If
                    End If
                End Using
            End If
        Else
            Environment.Exit(0)
        End If
    End Sub
End Module
