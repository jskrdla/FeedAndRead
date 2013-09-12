Imports System.IO

Partial Class Default2
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Directory.Exists(Common.GetFolder) Then Directory.CreateDirectory(Common.GetFolder)
        If Not Directory.Exists(Common.GetFolder + "/Deleted") Then Directory.CreateDirectory(Common.GetFolder + "/Deleted")
        If Not Directory.Exists(Common.GetFolder + "/Shared") Then Directory.CreateDirectory(Common.GetFolder + "/Shared")

        LoadNext()
    End Sub

    Protected Sub LoadNext()
        HiddenField1.Value = GetFirstImage()
        If HiddenField1.Value.Length > 0 Then
            Image1.Visible = True
            Image1.ImageUrl = "/images/" + HiddenField1.Value
        Else
            Image1.Visible = False
        End If
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim fi As New FileInfo(Common.GetFolder + "\" + HiddenField1.Value)
        If fi.Exists() Then
            fi.MoveTo(Common.GetFolder + "\Shared\" + HiddenField1.Value)
        End If
        LoadNext()
    End Sub

    Protected Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Dim fi As New FileInfo(Common.GetFolder + "\" + HiddenField1.Value)
        If fi.Exists() Then
            fi.MoveTo(Common.GetFolder + "\Deleted\" + HiddenField1.Value)
        End If
        LoadNext()
    End Sub

    Private Function GetFirstImage() As String
        Dim di As New DirectoryInfo(Common.GetFolder)
        If di.Exists Then
            Dim iCount As Integer = 0
            iCount = di.GetFiles("*.gif").Count
            iCount = iCount + di.GetFiles("*.jpg").Count
            lblImageCount.Text = iCount.ToString
            For Each fi As FileInfo In di.GetFiles
                Return fi.Name
            Next
        End If
        Return ""
    End Function

End Class
