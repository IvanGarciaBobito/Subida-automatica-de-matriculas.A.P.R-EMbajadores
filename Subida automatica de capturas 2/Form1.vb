Imports WinSCP
Imports System.IO
Public Class Form1
    Dim datenow As String = DateTime.Now.ToString().Replace("/", " ").Replace(":", " ") & ".txt"
    Dim logpath As String = My.Application.Info.DirectoryPath & "\logs\" & datenow

    Public Class local
        Public Nombre As String
        Public tamaño As Integer
    End Class

    Public Class remoto
        Public Nombre As String
        Public tamaño As Integer
    End Class

    Public listlocal As New Generic.List(Of local)
    Public listremoto As New Generic.List(Of remoto)

    Private Function preparedate(ByVal value As Integer) As String
        Dim date1 As Date = Date.Now().AddDays(value)
        Dim result As String = "01007801_" & date1.Year().ToString().Remove(0, 2) & "_"
        If date1.Month <= 9 Then
            result += "0" & date1.Month.ToString() & "_"
        Else
            result += date1.Month.ToString() & "_"
        End If

        If date1.Day <= 9 Then
            result += "0" & date1.Day.ToString()
        Else
            result += date1.Day.ToString()
        End If
        Return result
    End Function
    Private Sub FileTransferred(ByVal sender As Object, ByVal e As TransferEventArgs)
        If e.Error Is Nothing Then
            TextBox1.Text += DateTime.Now.ToString() & "   Archivo " & e.FileName & " subido correctamente" & vbNewLine
            TextBox1.Update()
        Else
            TextBox1.Text += DateTime.Now.ToString() & "   Error al subir el archivo " & e.FileName & " error: " & e.Error.ToString() & vbNewLine
            TextBox1.Update()
        End If
    End Sub


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Show()
        File.Create(logpath).Close()
        Try
            TextBox1.Text = "Creado por Ivan Garcia para la subida automatica de capturas al A.P.R" & vbNewLine
            TextBox1.Update()
            TextBox1.Text += "_____________________________________________________________________" & vbNewLine
            TextBox1.Update()
            TextBox1.Text += vbNewLine & DateTime.Now.ToString() & "   Conectando con el ftp del A.P.R" & vbNewLine
            TextBox1.Update()

            Dim sessionOptions As New SessionOptions
            With sessionOptions
                .Protocol = Protocol.Ftp
                .HostName = "XXXXXX"
                .UserName = "XXXXX"
                .Password = "XXXXXXX"
                .PortNumber = "XXXX"
            End With
            Using Session As Session = New Session
                Session.Open(sessionOptions)
                AddHandler Session.FileTransferred, AddressOf FileTransferred
                TextBox1.Text += DateTime.Now.ToString() & "   Conectado con el ftp del A.P.R" & vbNewLine
                TextBox1.Update()

                For i = -1 To -2 Step -1
                    Dim remotepath As String = preparedate(i)
                    If Session.FileExists("/" & remotepath) Then
                        TextBox1.Text += DateTime.Now.ToString() & "   La carpeta " & remotepath & " existe en el servidor" & vbNewLine
                        TextBox1.Update()
                    Else
                        TextBox1.Text += DateTime.Now.ToString() & "   La carpeta " & remotepath & " no existe en el servidor" & vbNewLine
                        TextBox1.Update()
                        TextBox1.Text += DateTime.Now.ToString() & "   Creando la carpeta " & remotepath & " en el servidor" & vbNewLine
                        TextBox1.Update()
                        Session.CreateDirectory("/" & remotepath)
                        TextBox1.Text += DateTime.Now.ToString() & "   Subiendo carpeta " & remotepath & " al ftp del A.P.R" & vbNewLine
                        TextBox1.Update()
                        Dim folder As New DirectoryInfo("C:\embafichero\01007801\" & remotepath)
                        For Each file As FileInfo In folder.GetFiles()
                            If file.Name = "Thumbs.db" Then
                                My.Computer.FileSystem.DeleteFile("C:\embafichero\01007801\" & remotepath & "\Thumbs.db")
                            Else
                                Dim l As New local
                                l.Nombre = file.Name
                                l.tamaño = file.Length
                                listlocal.Add(l)
                            End If

                        Next
                        Dim mySynchronizationResult As SynchronizationResult
                        mySynchronizationResult = Session.SynchronizeDirectories(SynchronizationMode.Remote, "C:\embafichero\01007801\" & remotepath, "/" & remotepath, False)
                        mySynchronizationResult.Check()

                        Dim directory As RemoteDirectoryInfo = Session.ListDirectory("/" & remotepath)

                        Dim fileInfo As RemoteFileInfo
                        For Each fileInfo In directory.Files
                            Dim l2 As New remoto
                            If fileInfo.Name = ".." Then
                            Else
                                l2.Nombre = fileInfo.Name
                                l2.tamaño = fileInfo.Length
                                listremoto.Add(l2)
                            End If
                        Next
                        For o = 0 To listremoto.Count - 1
                            TextBox1.Text += " Local: " & listlocal(o).Nombre & "  tamaño: " & listlocal(o).tamaño & "   remoto: " & listremoto(o).Nombre & "  tamaño: " & listremoto(o).tamaño & vbNewLine
                            TextBox1.Update()

                        Next
                        TextBox1.Text += " Archivos locales: " & listlocal.Count.ToString() & "  Archivos remotos: " & listremoto.Count.ToString() & vbNewLine
                        TextBox1.Update()

                        listlocal.Clear()
                        listremoto.Clear()
                    End If

                Next
            End Using
        Catch ex As Exception
            TextBox1.Text += DateTime.Now.ToString() & "   Error: " & ex.ToString()
        End Try

        File.WriteAllText(logpath, TextBox1.Text)

    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        TextBox1.SelectionStart = TextBox1.TextLength
        TextBox1.ScrollToCaret()
        TextBox1.SelectedText = Nothing
    End Sub
End Class
