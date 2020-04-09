Imports System.Threading

Public Class SpinningAnimation
    Public Class Spinner
        Implements IDisposable

        Private Const Sequence As String = "/-\|"
        Private counter As Integer = 0
        Private ReadOnly left As Integer
        Private ReadOnly top As Integer
        Private ReadOnly delay As Integer
        Private active As Boolean
        Private ReadOnly thread As Thread

        Public Sub New(ByVal left As Integer, ByVal top As Integer, ByVal Optional delay As Integer = 100)
            Me.left = left
            Me.top = top
            Me.delay = delay
            thread = New Thread(AddressOf Spin)
        End Sub

        Public Sub Start()
            active = True
            If Not thread.IsAlive Then thread.Start()
        End Sub

        Public Sub [Stop]()
            active = False
            Draw(" "c)
        End Sub

        Private Sub Spin()
            While active
                Turn()
                Thread.Sleep(delay)
            End While
        End Sub

        Private Sub Draw(ByVal c As Char)
            Console.CursorVisible = False
            Console.Write(New String(" ", Console.BufferWidth))
            PrintOK("[  OK  ] ", "Uploading selected file to the AnonFile network... Please wait. " & c)
            Console.SetCursorPosition(0, 5)
        End Sub

        Private Sub Turn()
            Draw(Sequence(Interlocked.Increment(counter) Mod Sequence.Length))
        End Sub

        Public Sub Dispose()
            [Stop]()
        End Sub

        Private Sub IDisposable_Dispose() Implements IDisposable.Dispose
            Call Dispose()
        End Sub
    End Class
End Class
