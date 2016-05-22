Imports System.IO

Public Class WatchFile

    Dim watchersSystem()
    Dim watchersDropbox()
    Dim numberWatcher As Integer = 0

    Public Sub New(number As Integer)
        ReDim Preserve watchersSystem(CInt(number * 1.2) + 1)
        ReDim Preserve watchersDropbox(CInt(number * 1.2) + 1)
    End Sub

    Public Sub AddElement(path1 As String, path2 As String)

        If watchersDropbox.Length = numberWatcher Then
            ReDim Preserve watchersSystem(CInt(numberWatcher * 1.2) + 1)
            ReDim Preserve watchersDropbox(CInt(numberWatcher * 1.2) + 1)
        End If

        Dim watchSystem As New FileSystemWatcher()
        Dim watchDropbox As New FileSystemWatcher()
        Try
            watchSystem.Path = getPath(path1)
            watchDropbox.Path = getPath(path2)
            watchSystem.NotifyFilter = (NotifyFilters.LastAccess Or NotifyFilters.LastWrite Or NotifyFilters.FileName Or NotifyFilters.DirectoryName)
            watchDropbox.NotifyFilter = (NotifyFilters.LastAccess Or NotifyFilters.LastWrite Or NotifyFilters.FileName Or NotifyFilters.DirectoryName)
            watchSystem.Filter = "*.txt"
            watchDropbox.Filter = "*.txt"
            AddHandler watchSystem.Changed, AddressOf OnChanged
            AddHandler watchDropbox.Changed, AddressOf OnChanged
            watchSystem.EnableRaisingEvents = True
            watchDropbox.EnableRaisingEvents = True
            watchersSystem(numberWatcher) = watchSystem
            watchersDropbox(numberWatcher) = watchDropbox
            numberWatcher += 1
        Catch ex As Exception
            Environment.Exit(0)
        End Try

    End Sub

    ' Define the event handlers.
    Public Sub OnChanged(source As Object, e As FileSystemEventArgs)
        ' Specify what is done when a file is changed, created, or deleted.
        MsgBox("Ha cambiat el fitxer " & e.FullPath)
    End Sub

    Private Function getPath(path As String) As String
        Dim strArray() As String = Split(path, "\")
        strArray(strArray.Length - 1) = ""
        Return String.Join("\", strArray)
    End Function

End Class
