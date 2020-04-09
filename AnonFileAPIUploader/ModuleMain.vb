Imports System.IO
Imports System.Threading
Imports AnonFileAPI
Imports Microsoft.VisualBasic.CompilerServices

Module ModuleMain
    Private googlesAddress As String = "216.58.211.4:80"
    Private anonFilesAddress As String = "194.32.146.52"

    Sub Main(ByVal args As String())
        Try
            Dim spinner = New SpinningAnimation.Spinner(10, 10)
            If ConnectionAvailable() = False Then
                Dim targetFilePath As String
                If args.Length > 0 Then
                    targetFilePath = args(0)
                    Dim targetFileInfo As New FileInfo(targetFilePath)
                    Dim targetSize As Long = targetFileInfo.Length
                    If targetSize <= &H500000000 Then
                        Console.Title = "AnonFile Client - File Uploader [© Gecco 2020] v" & My.Application.Info.Version.ToString
                        PrintINFO("[ INFO ] ", "Target servers ping is: " & Ping())
                        PrintOK("[  OK  ] ", "File size is not exceeding the limit. Size: " & ConvertByteScale(targetSize))
                        Thread.Sleep(100)
                        PrintWARN("[ WARN ] ", "Are you sure you want to upload the selected file? Y or N.")
                        PrintINFO("[ INFO ] ", "Selected file: " & targetFilePath & vbCrLf)
                        Console.CursorVisible = False
                        Dim readKey1 As ConsoleKey = Console.ReadKey().Key
                        If readKey1 = ConsoleKey.N Then
                            Console.SetCursorPosition(0, Console.CursorTop - 1)
                            ClearCurrentConsoleLine()
                            PrintWARN("[ WARN ] ", "Exiting console...")
                            Thread.Sleep(800)
                            Environment.Exit(0)
                        ElseIf readKey1 = ConsoleKey.Y Then
                            Console.SetCursorPosition(0, Console.CursorTop - 1)
                            PrintOK("[  OK  ] ", "Successfully launched upload.")
                            Using anonFileWrapper As AnonFileWrapper = New AnonFileWrapper()
                                spinner.Start()
                                Dim anonFile As AnonFile = anonFileWrapper.UploadFile($"" & targetFilePath)
                                Thread.Sleep(200)
                                If anonFile.ErrorMessage IsNot Nothing Then
                                    ClearCurrentConsoleLine()
                                    PrintFAIL("[ FAIL ] ", "Error message: " & anonFile.ErrorMessage)
                                    PrintFAIL("[ FAIL ] ", "Error type: " & anonFile.ErrorType)
                                    PrintFAIL("[ FAIL ] ", "Error code: " & anonFile.ErrorCode)
                                Else
                                    spinner.Stop()
                                    ClearCurrentConsoleLine()
                                    Console.CursorVisible = True
                                    PrintOK("[  OK  ] ", "File was successfully uploaded!")
                                    ClearCurrentConsoleLine()
                                    PrintINFO("[ INFO ] ", "Download URL: " & anonFile.FullUrl)
                                    PrintINFO("[ INFO ] ", "Uploaded file size: " & ConvertByteScale(targetSize))
                                    PrintINFO("[ INFO ] ", "Direct download URL: " & anonFileWrapper.GetDirectDownloadLinkFromLink(anonFile.FullUrl) & vbCrLf)
                                    Thread.Sleep(100)
                                    PrintINFO("[ INFO ] ", "To copy the normal URL, press enter; copy direct download URL, press SPACEBAR; copy shorter URL, press ESC.")
                                    PrintWARN("[ WARN ] ", "Application will exit after URL is copied.")
                                    Console.Title = "AnonFile Client - File Uploader [© Gecco 2020] v" & My.Application.Info.Version.ToString & " (Finished Uploading)"
                                    Dim readKey2 As ConsoleKey = Console.ReadKey().Key
                                    If readKey2 = ConsoleKey.Enter Then
                                        My.Computer.Clipboard.SetText(anonFile.FullUrl)
                                        Thread.Sleep(100)
                                        Environment.Exit(0)
                                    ElseIf readKey2 = ConsoleKey.Spacebar Then
                                        My.Computer.Clipboard.SetText(anonFileWrapper.GetDirectDownloadLinkFromLink(anonFile.FullUrl))
                                        Thread.Sleep(100)
                                        Environment.Exit(0)
                                    ElseIf readKey2 = ConsoleKey.Escape Then
                                        My.Computer.Clipboard.SetText(anonFile.ShortUrl)
                                        Thread.Sleep(100)
                                        Environment.Exit(0)
                                    End If
                                End If
                            End Using
                        End If
                    Else
                        Console.Title = "AnonFile Client - File Uploader [© Gecco 2020] v" & My.Application.Info.Version.ToString & " (Maximum File Size Exceeded)"
                        Console.Clear()
                        PrintINFO("[ INFO ] ", "Target servers ping is: " & Ping())
                        Thread.Sleep(200)
                        PrintFAIL("[ FAIL ] ", "File is too big. Maximum allowed file size is 20GB. Press ENTER to exit.")
                        Console.ReadKey()
                        Environment.Exit(0)
                    End If
                Else
                    Environment.Exit(0)
                End If
            Else
                Console.Title = "AnonFile Client - File Uploader [© Gecco 2020] v" & My.Application.Info.Version.ToString & " (No Internet Connection Available)"
                Console.Clear()
                Thread.Sleep(200)
                PrintFAIL("[ FAIL ] ", "No working internet connection was found. Press ENTER to exit.")
                Console.ReadKey()
                Environment.Exit(0)
            End If
        Catch exception As Exception
            MsgBox(exception.ToString, MsgBoxStyle.Critical, "AnonFileUploader - The application crashed and is unable to recover.")
        End Try
    End Sub

    Private Sub PrintINFO(ByVal coloredPart As String, ByVal msg As String)
        Console.ForegroundColor = ConsoleColor.White
        Console.Write(coloredPart)
        Console.ForegroundColor = ConsoleColor.White
        Console.WriteLine(msg)
    End Sub

    Public Sub PrintOK(ByVal coloredPart As String, ByVal msg As String)
        Console.ForegroundColor = ConsoleColor.Green
        Console.Write(coloredPart)
        Console.ForegroundColor = ConsoleColor.White
        Console.WriteLine(msg)
    End Sub

    Private Sub PrintWARN(ByVal coloredPart As String, ByVal msg As String)
        Console.ForegroundColor = ConsoleColor.Yellow
        Console.Write(coloredPart)
        Console.ForegroundColor = ConsoleColor.White
        Console.WriteLine(msg)
    End Sub

    Private Sub PrintFAIL(ByVal coloredPart As String, ByVal msg As String)
        Console.ForegroundColor = ConsoleColor.Red
        Console.Write(coloredPart)
        Console.ForegroundColor = ConsoleColor.White
        Console.WriteLine(msg)
    End Sub
    Public Sub ClearCurrentConsoleLine()
        Dim currentLineCursor As Integer = Console.CursorTop
        Console.SetCursorPosition(0, Console.CursorTop)
        Console.Write(New String(" "c, Console.WindowWidth))
        Console.SetCursorPosition(0, currentLineCursor)
    End Sub
    Public Function ConvertByteScale(ByVal bytes As Long) As String
        If bytes.ToString.Length < 4 Then
            Return Conversions.ToString(bytes & " Bytes")
        End If
        Dim num As Double = (bytes / 1024)
        Dim str As String
        If num < 1024 Then
            str = "KB"
        Else
            num /= 1024
            If num < 1024 Then
                str = "MB"
            Else
                num /= 1024
                str = "GB"
            End If
        End If
        Return num.ToString(".0") & " " & str
    End Function

    Private Function Ping() As String
        Dim Result As Net.NetworkInformation.PingReply
        Dim SendPing As New Net.NetworkInformation.Ping
        Result = SendPing.Send(anonFilesAddress)
        Return Result.RoundtripTime & "m/s"
    End Function

    Public Function ConnectionAvailable() As Boolean
        Try
            Return My.Computer.Network.Ping(googlesAddress)
        Catch ex As Exception
            Return False
        End Try
    End Function
End Module
