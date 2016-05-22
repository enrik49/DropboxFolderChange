Imports System.IO
Imports System.Text.RegularExpressions

Public Class Form1

    Dim fileConf As String = "C:\prova.txt"
    Dim watcher As New List(Of FileSystemWatcher())
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Load configuration
        loadConf()
        visibilityForm(False)
        Run(DataGridView1.Rows.Count)
        DataGridView1.Rows(0).Selected = True
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Button6.Text = "Add new rule"
        textBrowser("", "")
        visibilityForm(True)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim row As Integer = DataGridView1.CurrentCell.RowIndex
        If (row >= 0) Then
            Button6.Text = "Modify rule"
            textBrowser(DataGridView1.Rows(row).Cells(0).Value, DataGridView1.Rows(row).Cells(1).Value)
            visibilityForm(True)
        Else
            MsgBox("Select row to modify.")
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim row As Integer = DataGridView1.CurrentCell.RowIndex
        visibilityForm(False)
        If (row >= 0) Then
            Dim answer As Integer = MsgBox("Are you sure you want to delete the rule?", vbYesNo + vbQuestion, "Empty Sheet")
            If answer = vbYes Then
                DataGridView1.Rows.RemoveAt(row)
                modifyFile()
            End If
        Else
            MsgBox("Select row to delete.")
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        browseFile(TextBox1)
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        browseFile(TextBox2)
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        If validateForm() Then
            If Button6.Text = "Modify rule" Then
                Dim row As Integer = DataGridView1.CurrentCell.RowIndex
                DataGridView1.Rows(row).Cells(0).Value() = TextBox1.Text
                DataGridView1.Rows(row).Cells(1).Value() = TextBox2.Text
                modifyFile()
            Else
                Dim str As String = TextBox1.Text & ";" & TextBox2.Text & ";" & Date.Today
                cutStr(str)
                newLine(str)
            End If
            visibilityForm(False)
        End If
    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        DataGridView1.Rows(e.RowIndex).Selected = True
    End Sub

    Private Sub Run(sizeGrid As Integer)

        Dim args() As String = System.Environment.GetCommandLineArgs()

        ' Create a new FileSystemWatcher and set its properties.
        For i As Integer = 0 To sizeGrid
            Dim watch As New FileSystemWatcher()

            watch.Path = "C:\"
            ' Watch for changes in LastAccess and LastWrite times, and
            ' the renaming of files or directories. 
            watch.NotifyFilter = (NotifyFilters.LastAccess Or NotifyFilters.LastWrite Or NotifyFilters.FileName Or NotifyFilters.DirectoryName)

            ' Add event handlers.
            AddHandler watch.Changed, AddressOf OnChanged

            ' Begin watching.
            watch.EnableRaisingEvents = True
            watcher.Add(watch)
        Next

    End Sub

    ' Define the event handlers.
    Private Shared Sub OnChanged(source As Object, e As FileSystemEventArgs)
        ' Specify what is done when a file is changed, created, or deleted.
        MsgBox("Ha cambiat el fitxer " & e.FullPath)
    End Sub

    Private Sub loadConf()
        Try
            Dim sr As New StreamReader(fileConf)
            While sr.Peek() >= 0
                cutStr(sr.ReadLine())
            End While

            sr.Close()
        Catch E As Exception
            Console.WriteLine("The file could not be read:")
            Console.WriteLine(E.Message)
        End Try
    End Sub

    Private Sub cutStr(line As String)
        Dim delimiter As String = ";"
        DataGridView1.Rows.Add(line.Split(delimiter.ToCharArray()))
    End Sub

    Private Sub browseFile(textB As TextBox)
        If (OpenFileDialog1.ShowDialog() = DialogResult.OK) Then
            textB.Text = OpenFileDialog1.FileName
        End If

    End Sub

    Private Sub visibilityForm(state As Boolean)
        Label1.Visible = state
        Label2.Visible = state
        TextBox1.Visible = state
        TextBox2.Visible = state
        Button4.Visible = state
        Button5.Visible = state
        Button6.Visible = state
    End Sub

    Private Sub textBrowser(strText1 As String, strText2 As String)
        TextBox1.Text = strText1
        TextBox2.Text = strText2
    End Sub

    Private Function validateForm() As Boolean

        If TextBox1.Text = "" Or TextBox2.Text = "" Then
            MsgBox("The file/s isn't selected")
            Return False
        Else
            Dim Formato As String = "^[A-Z]:\\.*$"
            If My.Computer.FileSystem.FileExists(TextBox1.Text) And My.Computer.FileSystem.FileExists(TextBox2.Text) Then
                Return True
            Else
                MsgBox("Use a button Browse")
                Return False
            End If
        End If
    End Function

    Private Sub modifyFile()
        Dim TheFileLines As New List(Of String)
        For i As Integer = 0 To DataGridView1.Rows.Count - 2
            TheFileLines.Add(DataGridView1.Rows(i).Cells(0).Value() & ";" & DataGridView1.Rows(i).Cells(1).Value() & ";" & Date.Today)
        Next
        System.IO.File.WriteAllLines(fileConf, TheFileLines.ToArray)

    End Sub

    Private Sub newLine(str As String)
        Dim file As System.IO.StreamWriter
        file = My.Computer.FileSystem.OpenTextFileWriter(fileConf, True)
        file.WriteLine(str)
        file.Close()
    End Sub

End Class
